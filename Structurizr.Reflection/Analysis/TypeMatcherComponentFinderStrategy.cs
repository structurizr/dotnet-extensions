using System;
using System.Collections.Generic;

namespace Structurizr.Analysis
{

    /// <summary>
    /// Implements a component finder strategy which uses a collection of <see cref="ITypeMatcher"/> objects to identify
    /// components and their dependencies.
    /// </summary>
    /// <seealso cref="CustomAttributeTypeMatcher" />
    /// <seealso cref="ExtendsClassTypeMatcher" />
    /// <seealso cref="InterfaceImplementationTypeMatcher" />
    /// <seealso cref="NameSuffixTypeMatcher" />
    public class TypeMatcherComponentFinderStrategy : ComponentFinderStrategy
    {

        private ComponentFinder _componentFinder;

        /// <inheritdoc />
        public ComponentFinder ComponentFinder
        {
            get { return _componentFinder; }
            set
            {
                _componentFinder = value;
            }
        }

        private HashSet<Component> _componentsFound = new HashSet<Component>();

        private ITypeRepository _typeRepository;
        private List<ITypeMatcher> _typeMatchers = new List<ITypeMatcher>();
        private List<SupportingTypesStrategy> _supportingTypesStrategies = new List<SupportingTypesStrategy>();

        /// <summary>
        /// Creates a new instance of <see cref="TypeMatcherComponentFinderStrategy"/> for identifying
        /// components using the provided type matchers.
        /// </summary>
        /// <param name="typeMatchers">
        /// An array of objects implementing <see cref="ITypeMatcher"> that will be used to identify components in the
        /// provided assembly.
        /// </param>
        public TypeMatcherComponentFinderStrategy(params ITypeMatcher[] typeMatchers)
        {
            this._typeMatchers.AddRange(typeMatchers);
        }

        /// <inheritdoc />
        public void BeforeFindComponents()
        {
            _typeRepository = new ReflectionTypeRepository(_componentFinder.Namespace, _componentFinder.Exclusions);
            foreach (SupportingTypesStrategy strategy in _supportingTypesStrategies)
            {
                strategy.TypeRepository = _typeRepository;
            }
        }

        /// <inheritdoc />
        public IEnumerable<Component> FindComponents()
        {
            foreach (Type type in _typeRepository.GetAllTypes())
            {
                foreach (ITypeMatcher typeMatcher in this._typeMatchers)
                {
                    if (typeMatcher.Matches(type))
                    {
                        Component component = ComponentFinder.Container.AddComponent(
                            type.Name,
                            type,
                            typeMatcher.GetDescription(),
                            typeMatcher.GetTechnology());
                        _componentsFound.Add(component);
                    }
                }
            }

            return _componentsFound;
        }

        /// <inheritdoc />
        public void AfterFindComponents()
        {
            // before finding dependencies, let's find the types that are used to implement each component
            foreach (Component component in _componentsFound)
            {
                foreach (CodeElement codeElement in component.CodeElements)
                {
                    codeElement.Visibility = _typeRepository.FindVisibility(codeElement.Type);
                    codeElement.Category = _typeRepository.FindCategory(codeElement.Type);
                }

                foreach (SupportingTypesStrategy strategy in _supportingTypesStrategies)
                {
                    foreach (string type in strategy.FindSupportingTypes(component))
                    {
                        if (ComponentFinder.Container.GetComponentOfType(type) == null)
                        {
                            CodeElement codeElement = component.AddSupportingType(type);
                            codeElement.Visibility = _typeRepository.FindVisibility(type);
                            codeElement.Category = _typeRepository.FindCategory(type);
                        }
                    }
                }
            }

            foreach (Component component in ComponentFinder.Container.Components)
            {
                if (component.Type != null)
                {
                    AddEfferentDependencies(component, component.Type, new HashSet<string>());

                    // and repeat for the supporting types
                    foreach (CodeElement codeElement in component.CodeElements)
                    {
                        AddEfferentDependencies(component, codeElement.Type, new HashSet<string>());
                    }
                }
            }
        }

        private void AddEfferentDependencies(Component component, string type, HashSet<string> typesVisited)
        {
            typesVisited.Add(type);

            foreach (string referencedTypeName in _typeRepository.GetReferencedTypes(type))
            {
                Component destinationComponent = ComponentFinder.Container.GetComponentOfType(referencedTypeName);
                if (destinationComponent != null)
                {
                    if (component != destinationComponent)
                    {
                        component.Uses(destinationComponent, "");
                    }
                }
                else if (!typesVisited.Contains(referencedTypeName))
                {
                    AddEfferentDependencies(component, referencedTypeName, typesVisited);
                }
            }
        }

        /// <summary>
        /// Adds a strategy for identifying supporting types of components identified by this strategy.
        /// </summary>
        /// <param name="strategy">
        /// A <see cref="SupportingTypesStrategy"/> instance to use for identifying supporting types.
        /// </param>
        public void AddSupportingTypesStrategy(SupportingTypesStrategy strategy)
        {
            if (strategy != null)
            {
                _supportingTypesStrategies.Add(strategy);
                strategy.TypeRepository = _typeRepository;
            }
        }

    }
}