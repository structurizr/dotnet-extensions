using Mono.Cecil;

using Structurizr.Cecil;

namespace Structurizr.Analysis
{
    /// <summary>
    /// Implements a component identification rule based on implementation of an interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The InterfaceImplementationTypeMatcher class provides component identification by looking for types which
    /// implement the interface represented by the <see cref="InterfaceType"/> property.
    /// </para>
    /// </remarks>
    public class InterfaceImplementationTypeMatcher : ITypeMatcher
    {
        /// <summary>
        /// Gets the type information of the interface used for identifying components for this rule.
        /// </summary>
        /// <value>The <see cref="TypeDefinition"/> object representing the interface used by the rule.</value>
        public TypeDefinition InterfaceType { get; private set; }

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
        /// Creates a new instance of <see cref="InterfaceImplementationTypeMatcher"/> based on the provided interface.
        /// </summary>
        /// <param name="interfaceType">
        /// The <see cref="TypeDefinition"/> object representing the interface that will be used to identify components.
        /// </param>
        /// <param name="description">The description to set on components found by this rule.</param>
        /// <param name="technology">The technology name(s) to set on components found by this rule.</param>
        public InterfaceImplementationTypeMatcher(TypeDefinition interfaceType,
            string description,
            string technology)
        {
            this.InterfaceType = interfaceType;
            this.Description = description;
            this.Technology = technology;
        }

        /// <inheritdoc />
        /// <remarks>
        /// <para>
        /// The rule implemented by <see cref="InterfaceImplementationTypeMatcher"/> identifies components from types
        /// that implement the interface represented by <see cref="InterfaceType"/>. All types in the assembly under
        /// analysis which implement the interface at any depth of inheritance will be identified as a component and
        /// added to the model.
        /// </para>
        /// </remarks>
        public bool Matches(TypeDefinition type)
        {
            return this.InterfaceType.IsAssignableFrom(type);
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
