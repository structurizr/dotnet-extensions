using System;
using System.Collections.Generic;
using System.Linq;

using Mono.Cecil;

using Structurizr.Annotations;
using Structurizr.Cecil;
using Structurizr.Cecil.Util;

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

        private AssemblyDefinition _primaryAssembly;

        private ITypeRepository _typeRepository;

        private List<SupportingTypesStrategy> _supportingTypesStrategies = new List<SupportingTypesStrategy>();

        /// <summary>
        /// Creates a new instance of <see cref="StructurizrAnnotationsComponentFinderStrategy"/> for identifying
        /// components from the provided assembly.
        /// </summary>
        /// <param name="assembly">
        /// An <see cref="AssemblyDefinition"/> instance representing the assembly to analyze.
        /// </param>
        public StructurizrAnnotationsComponentFinderStrategy(AssemblyDefinition assembly)
        {
            this._primaryAssembly = assembly;
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
            List<TypeDefinition> types = _typeRepository.GetAllTypes().ToList();

            foreach (TypeDefinition type in types)
            {
                if (!type.HasCustomAttributes) continue;

                ComponentAttribute componentAttribute =
                    type.ResolvableAttributes<ComponentAttribute>().SingleOrDefault();
                if (componentAttribute != null)
                {
                    Component component = ComponentFinder.Container.AddComponent(
                        type.Name,
                        type.GetAssemblyQualifiedName(),
                        componentAttribute.Description,
                        componentAttribute.Technology
                    );
                    _componentsFound.Add(component);
                }
            }

            // Look for code elements after finding all the components
            foreach (TypeDefinition type in types)
            {
                CodeElementAttribute codeElementAttribute = type.ResolvableAttributes<CodeElementAttribute>().SingleOrDefault();
                if (codeElementAttribute != null)
                {
                    Component component =
                        ComponentFinder.Container.GetComponentOfType(codeElementAttribute.ComponentName)
                        ?? ComponentFinder.Container.GetComponentWithName(codeElementAttribute.ComponentName);
                    if (component != null)
                    {
                        CodeElement codeElement = component.AddSupportingType(type.GetAssemblyQualifiedName());
                        codeElement.Description = codeElementAttribute.Description;
                    }
                    else
                    {
                        Console.WriteLine("Could not find component " + codeElementAttribute.ComponentName + " for type " + type.FullName);
                    }
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
            TypeDefinition type = _typeRepository.GetType(typeName);
            if (type == null)
            {
                Console.WriteLine("Could not find type " + typeName + " for component " + component.Name);
                return;
            }

            foreach (FieldDefinition field in type.Fields)
            {
                if (!field.HasCustomAttributes) continue;
                var annotation = field.ResolvableAttributes<UsesComponentAttribute>().SingleOrDefault();
                if (annotation == null) continue;

                AddUsesComponentRelationship(component, field.FieldType, annotation);
            }

            foreach (PropertyDefinition property in type.Properties)
            {
                if (!property.HasCustomAttributes) continue;
                var annotation = property.ResolvableAttributes<UsesComponentAttribute>().SingleOrDefault();
                if (annotation == null) continue;

                AddUsesComponentRelationship(component, property.PropertyType, annotation);
            }

            foreach (MethodDefinition method in type.Methods)
            {
                foreach (ParameterDefinition parameter in method.Parameters)
                {
                    if (!parameter.HasCustomAttributes) continue;
                    var annotation = parameter.ResolvableAttributes<UsesComponentAttribute>().SingleOrDefault();
                    if (annotation == null) continue;

                    AddUsesComponentRelationship(component, parameter.ParameterType, annotation);
                }
            }
        }

        private void AddUsesComponentRelationship(
            Component component,
            TypeReference destinationType,
            UsesComponentAttribute annotation)
        {
            if (annotation == null)
            {
                Console.WriteLine("Missing UsesComponent annotation on " + component.Name);
                return;
            }

            string destinationTypeName = destinationType.GetAssemblyQualifiedName();
            Component destination = ComponentFinder.Container.GetComponentOfType(destinationTypeName);
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
            TypeDefinition type = _typeRepository.GetType(typeName);
            if (type == null)
            {
                Console.WriteLine("Could not find type " + typeName + " for component " + component.Name);
                return;
            }
            if (!type.HasCustomAttributes) return;

            var annotations = type.ResolvableAttributes<UsesContainerAttribute>().ToList();
            foreach(UsesContainerAttribute annotation in annotations)
            {
                Container container = FindContainerByNameOrCanonicalNameOrId(component, annotation.ContainerName);
                if (container != null)
                {
                    string description = annotation.Description;
                    string technology = annotation.Technology;

                    component.Uses(container, description, technology);
                }
                else
                {
                    Console.WriteLine("Could not find container " + annotation.ContainerName + " for component " + component.Name);
                }
            }
        }

        private void FindUsesSoftwareSystemAnnotations(Component component, string typeName)
        {
            TypeDefinition type = _typeRepository.GetType(typeName);
            if (type == null)
            {
                Console.WriteLine("Could not find type " + typeName + " for component " + component.Name);
                return;
            }
            if (!type.HasCustomAttributes) return;

            var annotations = type.ResolvableAttributes<UsesSoftwareSystemAttribute>().ToList();
            foreach(UsesSoftwareSystemAttribute annotation in annotations)
            {
                SoftwareSystem system = component.Model.GetSoftwareSystemWithName(annotation.SoftwareSystemName);
                if (system != null)
                {
                    string description = annotation.Description;
                    string technology = annotation.Technology;

                    component.Uses(system, description, technology);
                }
                else
                {
                    Console.WriteLine("Could not find software system " + annotation.SoftwareSystemName + " for component " + component.Name);
                }
            }
        }

        private void FindUsedByContainerAnnotations(Component component, string typeName)
        {
            TypeDefinition type = _typeRepository.GetType(typeName);
            if (type == null)
            {
                Console.WriteLine("Could not find type " + typeName + " for component " + component.Name);
                return;
            }
            if (!type.HasCustomAttributes) return;

            var annotations = type.ResolvableAttributes<UsedByContainerAttribute>().ToList();
            foreach(UsedByContainerAttribute annotation in annotations)
            {
                Container container = FindContainerByNameOrCanonicalNameOrId(component, annotation.ContainerName);
                if (container != null)
                {
                    string description = annotation.Description;
                    string technology = annotation.Technology;

                    container.Uses(component, description, technology);
                }
                else
                {
                    Console.WriteLine("Could not find container " + annotation.ContainerName + " using component " + component.Name);
                }
            }
        }

        private void FindUsedByPersonAnnotations(Component component, string typeName)
        {
            TypeDefinition type = _typeRepository.GetType(typeName);
            if (type == null)
            {
                Console.WriteLine("Could not find type " + typeName + " for component " + component.Name);
                return;
            }
            if (!type.HasCustomAttributes) return;

            var annotations = type.ResolvableAttributes<UsedByPersonAttribute>().ToList();
            foreach(UsedByPersonAttribute annotation in annotations)
            {
                Person person = component.Model.GetPersonWithName(annotation.PersonName);
                if (person != null)
                {
                    string description = annotation.Description;
                    string technology = annotation.Technology;

                    person.Uses(component, description, technology);
                }
                else
                {
                    Console.WriteLine("Could not find person " + annotation.PersonName + " using component " + component.Name);
                }
            }
        }

        private void FindUsedBySoftwareSystemAnnotations(Component component, string typeName)
        {
            TypeDefinition type = _typeRepository.GetType(typeName);
            if (type == null)
            {
                Console.WriteLine("Could not find type " + typeName + " for component " + component.Name);
                return;
            }
            if (!type.HasCustomAttributes) return;

            var annotations = type.ResolvableAttributes<UsedBySoftwareSystemAttribute>().ToList();
            foreach(UsedBySoftwareSystemAttribute annotation in annotations)
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
                    container = (Container) element;
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