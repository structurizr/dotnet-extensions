using System;

namespace Structurizr.Analysis
{
    /// <summary>
    /// Declares an interface for component identification rules.
    /// </summary>
    public interface ITypeMatcher
    {

        /// <summary>
        /// Checks the given <see cref="Type"/> matches against the component identification rule.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the type to match.</param>
        /// <returns><see langword="true"/> if the type matches against the rule; otherwise, <see langword="false"/>.</returns>
        bool Matches(Type type);

        /// <summary>
        /// Gets the description to use for components matching the rule.
        /// </summary>
        /// <returns>A string containing the Description property for matching components.</returns>
        /// <seealso cref="Structurizr.Element.Description" />
        string GetDescription();

        /// <summary>
        /// Gets the technology name(s) to use for components matching the rule.
        /// </summary>
        /// <returns>A string containing the Technology property for matching components.</returns>
        /// <seealso cref="Structurizr.Component.Technology" />
        string GetTechnology();

    }
}
