using System.Collections.Generic;

using Mono.Cecil;

using Structurizr.Cecil;

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
        /// <inheritdoc />
        public ComponentFinder ComponentFinder { get; set; }

        private HashSet<Component> _componentsFound = new HashSet<Component>();

        private AssemblyDefinition _primaryAssembly;
        private ITypeRepository _typeRepository;
        private List<ITypeMatcher> _typeMatchers = new List<ITypeMatcher>();
        private List<SupportingTypesStrategy> _supportingTypesStrategies = new List<SupportingTypesStrategy>();

        /// <summary>
        /// Creates a new instance of <see cref="TypeMatcherComponentFinderStrategy"/> for identifying
        /// components from the provided assembly using the provided type matchers.
        /// </summary>
        /// <param name="assembly">
        /// An <see cref="AssemblyDefinition"/> instance representing the assembly to analyze.
        /// </param>
        /// <param name="typeMatchers">
        /// An array of objects implementing <see cref="ITypeMatcher"> that will be used to identify components in the
        /// provided assembly.
        /// </param>
        public TypeMatcherComponentFinderStrategy(AssemblyDefinition assembly,
            params ITypeMatcher[] typeMatchers)
        {
            this._primaryAssembly = assembly;
            this._typeMatchers.AddRange(typeMatchers);
        }

        /// <inheritdoc />
        public void BeforeFindComponents()
        {
            _typeRepository = new CecilTypeRepository(
                _primaryAssembly,
                ComponentFinder.Namespace,
                ComponentFinder.Exclusions);
            foreach (SupportingTypesStrategy strategy in _supportingTypesStrategies)
            {
                strategy.TypeRepository = _typeRepository;
            }
        }

        /// <inheritdoc />
        public IEnumerable<Component> FindComponents()
        {
            foreach (TypeDefinition type in _typeRepository.GetAllTypes())
            {
                foreach (ITypeMatcher typeMatcher in this._typeMatchers)
                {
                    if (typeMatcher.Matches(type))
                    {
                        Component component = ComponentFinder.Container.AddComponent(
                            type.Name,
                            type.GetAssemblyQualifiedName(),
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
