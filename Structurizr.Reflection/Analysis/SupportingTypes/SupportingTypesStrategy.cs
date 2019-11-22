using System.Collections.Generic;

namespace Structurizr.Analysis
{
    /// <summary>
    /// Provides a base type for strategies used to identify supporting types for components.
    /// </summary>
    /// <seealso cref="Structurizr.Component.CodeElements" />
    /// <seealso cref="Structurizr.Component.AddSupportingType(string)" />
    public abstract class SupportingTypesStrategy
    {

        /// <summary>
        /// Gets the type repository to use for looking up supporting types.
        /// </summary>
        /// <value>The type repository instance provided by the component finder strategy using this strategy.</value>
        public ITypeRepository TypeRepository { get; set; }

        /// <summary>
        /// Identifies and returns supporting types of the provided <see cref="Component"/>.
        /// </summary>
        /// <param name="component">The component for which supporting types are to be found.</param>
        /// <returns>A set of assembly qualified type names for types which support the component.</returns>
        public abstract HashSet<string> FindSupportingTypes(Component component);

    }

}