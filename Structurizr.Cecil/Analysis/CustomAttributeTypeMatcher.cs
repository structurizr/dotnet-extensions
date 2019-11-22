using System.Linq;

using Mono.Cecil;

namespace Structurizr.Analysis
{
    /// <summary>
    /// Implements a component identification rule based on the use of custom attributes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The CustomAttributeTypeMatcher class provides component identification by looking for types decorated with the
    /// attribute represented by the <see cref="AttributeType"/> property. This can be any attribute class from the
    /// assembly under analysis or any of its dependencies.
    /// </para>
    /// </remarks>
    public class CustomAttributeTypeMatcher : ITypeMatcher
    {
        /// <summary>
        /// Gets the type information of the attribute type used for identifying components for this rule.
        /// </summary>
        /// <value>The <see cref="TypeDefinition"/> object representing the attribute type used by the rule.</value>
        public TypeDefinition AttributeType { get; private set; }

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
        /// Creates a new instance of <see cref="CustomAttributeTypeMatcher"/> based on the provided attribute type.
        /// </summary>
        /// <param name="attributeType">
        /// The <see cref="TypeDefinition"/> object representing the attribute type that will be used to identify components.
        /// </param>
        /// <param name="description">The description to set on components found by this rule.</param>
        /// <param name="technology">The technology name(s) to set on components found by this rule.</param>
        public CustomAttributeTypeMatcher(TypeDefinition attributeType,
            string description,
            string technology)
        {
            this.AttributeType = attributeType;
            this.Description = description;
            this.Technology = technology;
        }

        /// <inheritdoc />
        /// <remarks>
        /// <para>
        /// The rule implemented by <see cref="CustomAttributeTypeMatcher"/> identifies components from types that are
        /// decorated with the attribute class represented by <see cref="AttributeType"/>. All types in the assembly
        /// under analysis that have this attribute on the type itself (not on any of its members) will be identified
        /// as a component by the rule and added to the model.
        /// </para>
        /// </remarks>
        public bool Matches(TypeDefinition type)
        {
            return type.CustomAttributes.Any(ca => ca.AttributeType.Resolve().MetadataToken == this.AttributeType.MetadataToken);
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
