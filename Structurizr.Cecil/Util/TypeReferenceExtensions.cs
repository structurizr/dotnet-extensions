using System;
using System.Linq;

using Mono.Cecil;

namespace Structurizr.Cecil
{
    /// <summary>
    /// Provides extension methods for <see cref="TypeReference"/>.
    /// </summary>
    public static class TypeReferenceExtensions
    {
        /// <summary>
        /// Gets the assembly qualified name of the type.
        /// </summary>
        /// <param name="type">The <see cref="TypeReference"/> instance representing the type.</param>
        /// <returns>The assembly qualified name of the type.</returns>
        public static string GetAssemblyQualifiedName(this TypeReference type)
        {
            string typeName;

            if (type.IsGenericInstance)
            {
                var genericInstance = (GenericInstanceType)type;
                typeName = String.Format("{0}.{1}[{2}]",
                    genericInstance.Namespace,
                    type.Name,
                    String.Join(",",
                        genericInstance.GenericArguments.Select(p => p.GetAssemblyQualifiedName()).ToArray()
                ));
            }
            else
            {
                typeName = type.FullName;
            }

            var scope = type.Scope as AssemblyNameReference;
            if (scope != null)
            {
                return typeName + ", " + scope.FullName;
            }
            else if (type.Scope != null)
            {
                return typeName + ", " + type.Module.Assembly.FullName;
            }

            return typeName;
        }
    }
}
