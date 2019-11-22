using Mono.Cecil;

using Structurizr.Cecil;

namespace Structurizr.Analysis
{
    /// <summary>
    /// Implements a component identification rule based on inheritance from a class.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The ExtendsClassTypeMatcher class provides component identification by looking for types which are subtypes of
    /// the class represented by the <see cref="ClassType"/> property.
    /// </para>
    /// </remarks>
    public class ExtendsClassTypeMatcher : ITypeMatcher
    {
        /// <summary>
        /// Gets the type information of the base class used for identifying components for this rule.
        /// </summary>
        /// <value>The <see cref="TypeDefinition"/> object representing the class used by the rule.</value>
        public TypeDefinition ClassType { get; private set; }

        /// <summary>
        /// Gets the description to use for components matching the rule.
        /// </summary>
        /// <value>A string containing the Description property for matching components.</value>
        /// <seealso cref="GetDescription()" />
        /// <seealso cref="Structurizr.Element.Description" />
        public string Description { get; private set; }

        /// <summary>
        /// Gets the technology name(s) to use for components matching the rule.
        /// </summary>
        /// <returns>A string containing the Technology property for matching components.</returns>
        /// <seealso cref="GetTechnology()" />
        /// <seealso cref="Structurizr.Component.Technology" />
        public string Technology { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="ExtendsClassTypeMatcher"/> based on the provided class.
        /// </summary>
        /// <param name="classType">
        /// The <see cref="TypeDefinition"/> object representing the base class that will be used to identify components.
        /// </param>
        /// <param name="description">The description to set on components found by this rule.</param>
        /// <param name="technology">The technology name(s) to set on components found by this rule.</param>
        public ExtendsClassTypeMatcher(TypeDefinition classType, string description, string technology)
        {
            this.ClassType = classType;
            this.Description = description;
            this.Technology = technology;
        }

        /// <inheritdoc />
        /// <remarks>
        /// <para>
        /// The rule implemented by <see cref="ExtendsClassTypeMatcher"/> identifies components from types that
        /// inherit from the class represented by <see cref="ClassType"/>. All types in the assembly under analysis
        /// which have that class as a base type at any depth of inheritance will be identified as a component and
        /// added to the model.
        /// </para>
        /// </remarks>
        public bool Matches(TypeDefinition type)
        {
            return type.IsSubclassOf(this.ClassType);
        }

        /// <inheritdoc />
        public string GetDescription()
        {
            return this.Description;
        }

        /// <inheritdoc />
        public string GetTechnology()
        {
            return this.Technology;
        }
    }
}
