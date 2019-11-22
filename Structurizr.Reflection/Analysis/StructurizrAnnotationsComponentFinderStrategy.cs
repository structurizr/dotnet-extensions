using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Structurizr.Annotations;

namespace Structurizr.Analysis
{
    /// <summary>
    /// Implements a component finder strategy which uses the attributes from <see cref="Structurizr.Annotations"/> to
    /// identify components, their code elements, and their relationships.
    /// </summary>
    /// <seealso cref="CodeElementAttribute" />
    /// <seealso cref="ComponentAttribute" />
    /// <seealso cref="UsedByContainerAttribute" />
    /// <seealso cref="UsedByPersonAttribute" />
    /// <seealso cref="UsedBySoftwareSystemAttribute" />
    /// <seealso cref="UsesComponentAttribute" />
    /// <seealso cref="UsesContainerAttribute" />
    /// <seealso cref="UsesSoftwareSystemAttribute" />
    public class StructurizrAnnotationsComponentFinderStrategy : ComponentFinderStrategy
    {
        /// <inheritdoc />
        public ComponentFinder ComponentFinder { get; set; }

        private HashSet<Component> _componentsFound = new HashSet<Component>();

        private ITypeRepository _typeRepository;
        private List<SupportingTypesStrategy> _supportingTypesStrategies = new List<SupportingTypesStrategy>();

        /// <inheritdoc />
        public void BeforeFindComponents()
        {
            _typeRepository = new ReflectionTypeRepository(ComponentFinder.Namespace, ComponentFinder.Exclusions);
            foreach (SupportingTypesStrategy strategy in _supportingTypesStrategies)
            {
                strategy.TypeRepository = _typeRepository;
            }
        }

        /// <inheritdoc />
        public IEnumerable<Component> FindComponents()
        {
            List<Type> types = _typeRepository.GetAllTypes().ToList();

            foreach (Type type in types)
            {
                ComponentAttribute componentAttribute = type.GetCustomAttribute<ComponentAttribute>();
                if (componentAttribute == null) continue;

                Component component = ComponentFinder.Container.AddComponent(
                    type.Name,
                    type,
                    componentAttribute.Description,
                    componentAttribute.Technology);
                _componentsFound.Add(component);
            }

            // Look for code elements after finding all the components
            foreach (Type type in types)
            {
                CodeElementAttribute codeElementAttribute = type.GetCustomAttribute<CodeElementAttribute>();
                if (codeElementAttribute == null) continue;

                Component component =
                    ComponentFinder.Container.GetComponentOfType(codeElementAttribute.ComponentName)
                    ?? ComponentFinder.Container.GetComponentWithName(codeElementAttribute.ComponentName);
                if (component != null)
                {
                    CodeElement codeElement = component.AddSupportingType(type.AssemblyQualifiedName);
                    codeElement.Description = codeElementAttribute.Description;
                }
                else
                {
                    Console.WriteLine("Could not find component " + codeElementAttribute.ComponentName + " for type " + type.FullName);
                }
            }

            return _componentsFound;
        }

        /// <inheritdoc />
        public void AfterFindComponents()
        {
            foreach (Component component in _componentsFound)
            {
                foreach (SupportingTypesStrategy strategy in _supportingTypesStrategies)
                {
                    foreach (string type in strategy.FindSupportingTypes(component))
                    {
                        if (ComponentFinder.Container.GetComponentOfType(type) == null)
                        {
                            component.AddSupportingType(type);
                        }
                    }
                }

                foreach (CodeElement codeElement in component.CodeElements)
                {
                    codeElement.Visibility = _typeRepository.FindVisibility(codeElement.Type);
                    codeElement.Category = _typeRepository.FindCategory(codeElement.Type);

                    FindUsesComponentAnnotations(component, codeElement.Type);
                    FindUsesContainerAnnotations(component, codeElement.Type);
                    FindUsesSoftwareSystemAnnotations(component, codeElement.Type);

                    FindUsedByPersonAnnotations(component, codeElement.Type);
                    FindUsedByContainerAnnotations(component, codeElement.Type);
                    FindUsedBySoftwareSystemAnnotations(component, codeElement.Type);
                }
            }
        }

        private void FindUsesComponentAnnotations(Component component, string typeName)
        {
            Type type = _typeRepository.GetType(typeName);
            if (type == null)
            {
                Console.WriteLine("Could not find type " + typeName + " for component " + component.Name);
                return;
            }

            foreach (FieldInfo field in type.GetRuntimeFields())
            {
                var annotation = field.GetCustomAttribute<UsesComponentAttribute>();
                if (annotation == null) continue;

                AddUsesComponentRelationship(component, field.FieldType, annotation);
            }

            foreach (PropertyInfo property in type.GetRuntimeProperties())
            {
                var annotation = property.GetCustomAttribute<UsesComponentAttribute>();
                if (annotation == null) continue;

                AddUsesComponentRelationship(component, property.PropertyType, annotation);
            }

            foreach (MethodInfo method in type.GetRuntimeMethods())
            {
                foreach (var parameter in method.GetParameters())
                {
                    var annotation = parameter.GetCustomAttribute<UsesComponentAttribute>();
                    if (annotation == null) continue;

                    AddUsesComponentRelationship(component, parameter.ParameterType, annotation);
                }
            }
        }

        private void AddUsesComponentRelationship(
            Component component,
            Type destinationType,
            UsesComponentAttribute annotation)
        {
            if (annotation == null)
            {
                Console.WriteLine("Missing UsesComponent annotation on " + component.Name);
                return;
            }

            Component destination = ComponentFinder.Container.GetComponentOfType(destinationType.AssemblyQualifiedName);
            if (destination != null)
            {
                IList<Relationship> relationships = component.Relationships
                    .Where(r => r.Destination.Equals(destination))
                    .ToList();
                if (relationships.Count > 0)
                {
                    foreach (Relationship relationship in relationships)
                    {
                        if (String.IsNullOrEmpty(relationship.Description))
                        {
                            // only change the details of relationships that have no description
                            component.Model.ModifyRelationship(relationship, annotation.Description, annotation.Technology);
                        }
                    }
                }
                else
                {
                    // Relationship doesn't already exist, so add it
                    component.Uses(destination, annotation.Description, annotation.Technology);
                }
            }
            else
            {
                Console.WriteLine("Could not find component with " + destinationType.FullName + " for component " + component.Name);
            }
        }

        private void FindUsesContainerAnnotations(Component component, string typeName)
        {
            Type type = _typeRepository.GetType(typeName);
            if (type == null)
            {
                Console.WriteLine("Could not find type " + typeName + " for component " + component.Name);
                return;
            }

            var annotations = type.GetCustomAttributes<UsesContainerAttribute>();
            foreach(UsesContainerAttribute annotation in annotations)
            {
                Container container = FindContainerByNameOrCanonicalNameOrId(component, annotation.ContainerName);
                if (container != null)
                {
                    component.Uses(container, annotation.Description, annotation.Technology);
                }
                else
                {
                    Console.WriteLine("Could not find container " + annotation.ContainerName + " for component " + component.Name);
                }
            }
        }

        private void FindUsesSoftwareSystemAnnotations(Component component, string typeName)
        {
            var type = _typeRepository.GetType(typeName);
            if (type == null)
            {
                Console.WriteLine("Could not find type " + typeName + " for component " + component.Name);
                return;
            }

            var annotations = type.GetCustomAttributes<UsesSoftwareSystemAttribute>();
            foreach(UsesSoftwareSystemAttribute annotation in annotations)
            {
                SoftwareSystem system = component.Model.GetSoftwareSystemWithName(annotation.SoftwareSystemName);
                if (system != null)
                {
                    component.Uses(system, annotation.Description, annotation.Technology);
                }
                else
                {
                    Console.WriteLine("Could not find software system " + annotation.SoftwareSystemName + " for component " + component.Name);
                }
            }
        }

        private void FindUsedByPersonAnnotations(Component component, string typeName)
        {
            Type type = _typeRepository.GetType(typeName);
            if (type == null)
            {
                Console.WriteLine("Could not find type " + typeName + " for component " + component.Name);
                return;
            }

            var annotations = type.GetCustomAttributes<UsedByPersonAttribute>();
            foreach (UsedByPersonAttribute annotation in annotations)
            {
                Person person = component.Model.GetPersonWithName(annotation.PersonName);
                if (person != null)
                {
                    person.Uses(component, annotation.Description, annotation.Technology);
                }
                else
                {
                    Console.WriteLine("Could not find person " + annotation.PersonName + " using component " + component.Name);
                }
            }
        }

        private void FindUsedByContainerAnnotations(Component component, string typeName)
        {
            Type type = _typeRepository.GetType(typeName);
            if (type == null)
            {
                Console.WriteLine("Could not find type " + typeName + " for component " + component.Name);
                return;
            }

            var annotations = type.GetCustomAttributes<UsedByContainerAttribute>();
            foreach (UsedByContainerAttribute annotation in annotations)
            {
                Container container = FindContainerByNameOrCanonicalNameOrId(component, annotation.ContainerName);
                if (container != null)
                {
                    container.Uses(component, annotation.Description, annotation.Technology);
                }
                else
                {
                    Console.WriteLine("Could not find container " + annotation.ContainerName + " using component " + component.Name);
                }
            }
        }

        private void FindUsedBySoftwareSystemAnnotations(Component component, string typeName)
        {
            Type type = _typeRepository.GetType(typeName);
            if (type == null)
            {
                Console.WriteLine("Could not find type " + typeName + " for component " + component.Name);
                return;
            }

            var annotations = type.GetCustomAttributes<UsedBySoftwareSystemAttribute>();
            foreach (UsedBySoftwareSystemAttribute annotation in annotations)
            {
                SoftwareSystem system = component.Model.GetSoftwareSystemWithName(annotation.SoftwareSystemName);
                if (system != null)
                {
                    system.Uses(component, annotation.Description, annotation.Technology);
                }
                else
                {
                    Console.WriteLine("Could not find software system " + annotation.SoftwareSystemName + " using component " + component.Name);
                }
            }
        }

        private Container FindContainerByNameOrCanonicalNameOrId(Component component, string name)
        {
            // assume that the container resides in the same software system
            Container container = component.Container.SoftwareSystem.GetContainerWithName(name);
            if (container == null)
            {
                // perhaps a canonical name has been specified
                Element element = component.Model.GetElementWithCanonicalName(name);
                if (element != null && element is Container)
                {
                    container = (Container)element;
                }
                else
                {
                    // perhaps it's an element ID?
                    container = component.Model.GetElement(name) as Container;
                }
            }

            return container;
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
