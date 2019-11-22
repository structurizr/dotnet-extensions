using Mono.Cecil;

namespace Structurizr.Analysis
{
    /// <summary>
    /// Implements a component identification rule based on matching a type name against a case-sensitive suffix.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The NameSuffixTypeMatcher class provides component identification by looking for types whose names end with the
    /// value of the <see cref="Suffix"/> property. The name comparison is case-sensitive.
    /// </para>
    /// </remarks>
    public class NameSuffixTypeMatcher : ITypeMatcher
    {
        /// <summary>
        /// Gets the string used for suffix comparison for identifying components with this rule.
        /// </summary>
        /// <value>A string used for case-sensitive comparisons against types used by the rule.</value>
        public string Suffix { get; private set; }

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
        /// Creates a new instance of <see cref="NameSuffixTypeMatcher"/> using the provided suffix string.
        /// </summary>
        /// <param name="suffix">A string used for case-sensitive comparisons against the end of type names.</param>
        /// <param name="description">The description to set on components found by this rule.</param>
        /// <param name="technology">The technology name(s) to set on components found by this rule.</param>
        public NameSuffixTypeMatcher(string suffix, string description, string technology)
        {
            this.Suffix = suffix;
            this.Description = description;
            this.Technology = technology;
        }

        /// <inheritdoc />
        /// <remarks>
        /// <para>
        /// The rule implemented by <see cref="NameSuffixTypeMatcher"/> identifies components from types whose names
        /// end with the case-sensitive string in <see cref="Suffix"/>. All types in the assembly whose names end with
        /// that suffix will be identified as a component and added to the model.
        /// </para>
        /// </remarks>
        public bool Matches(TypeDefinition type)
        {
            return type.Name.EndsWith(this.Suffix);
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
