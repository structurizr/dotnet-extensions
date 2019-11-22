using System;
using System.Collections.Generic;
using System.Linq;

namespace Structurizr.Analysis
{

    /// <summary>
    /// Implements a <see cref="SupportingTypesStrategy"/> which finds all types referenced by the primary code element
    /// of a <see cref="Component"/>, and optionally those referenced by its secondary code elements.
    /// </summary>
    public class ReferencedTypesSupportingTypesStrategy : SupportingTypesStrategy
    {

        private bool _includeIndirectlyReferencedTypes;

        /// <summary>
        /// Creates a new instance of <see cref="ReferencedTypesSupportingTypesStrategy"/> which includes the option to
        /// find supporting types from secondary code elements.
        /// </summary>
        public ReferencedTypesSupportingTypesStrategy() : this(true)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ReferencedTypesSupportingTypesStrategy"/> which allows the option to
        /// find supporting types from secondary code elements to be expressly enabled or disabled.
        /// </summary>
        /// <param name="includeIndirectlyReferencedTypes">
        /// Whether or not to include supporting types identified from secondary code elements of the component.
        /// </param>
        /// <remarks>
        /// <para>
        /// Setting <paramref name="includeIndirectlyReferencedTypes"/> to <see langword="true"/> is equivalent to using
        /// the default constructor for <see cref="ReferencedTypesSupportingTypesStrategy"/>, and will find and add to
        /// a component's supporting types any types referenced by the secondary code elements of the component, in
        /// addition to those types referenced by the primary code element of the component. Setting the param to
        /// <see langword="false"/> instead will only find and add those types which are referenced directly by the
        /// primary code element of the component.
        /// </para>
        /// <para>
        /// Only those types that are in the same namespace as the code elements of a component will actually be added
        /// as supporting types of the component.
        /// </para>
        /// </remarks>
        public ReferencedTypesSupportingTypesStrategy(bool includeIndirectlyReferencedTypes)
        {
            _includeIndirectlyReferencedTypes = includeIndirectlyReferencedTypes;
        }

        /// <inheritdoc />
        public override HashSet<string> FindSupportingTypes(Component component)
        {
            HashSet<string> referencedTypes = new HashSet<string>();
            referencedTypes.UnionWith(GetReferencedTypesInNamespace(component.Type));

            foreach (CodeElement codeElement in component.CodeElements)
            {
                referencedTypes.UnionWith(GetReferencedTypesInNamespace(codeElement.Type));
            }

            if (_includeIndirectlyReferencedTypes) {
                int numberOfTypes = referencedTypes.Count;
                bool foundMore = true;
                while (foundMore) {
                    HashSet<string> indirectlyReferencedTypes = new HashSet<string>();
                    foreach (string type in referencedTypes)
                    {
                        indirectlyReferencedTypes.UnionWith(GetReferencedTypesInNamespace(type));
                    }
                    referencedTypes.UnionWith(indirectlyReferencedTypes);

                    if (referencedTypes.Count > numberOfTypes)
                    {
                        foundMore = true;
                        numberOfTypes = referencedTypes.Count;
                    }
                    else
                    {
                        foundMore = false;
                    }
                }
            }

            return referencedTypes;
        }

        private IEnumerable<string> GetReferencedTypesInNamespace(string typeName)
        {
            IEnumerable<string> referencedTypes = TypeRepository.GetReferencedTypes(typeName);
            return referencedTypes.Where(t => t.StartsWith(TypeRepository.Namespace));
        }
    }

}
