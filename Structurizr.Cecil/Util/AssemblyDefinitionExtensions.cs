using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Mono.Cecil;

namespace Structurizr.Cecil
{
    /// <summary>
    /// Provides extension methods for <see cref="AssemblyDefinition"/>.
    /// </summary>
    public static class AssemblyDefinitionExtensions
    {
        /// <summary>
        /// Generates a collection of assemblies directly or indirectly referenced by this assembly.
        /// </summary>
        /// <param name="assembly">The assembly whose references are to be returned.</param>
        /// <param name="includeSelf">Whether or not to include this assembly in the returned collection.</param>
        /// <returns>A generated collection of referenced assemblies.</returns>
        public static IEnumerable<AssemblyDefinition> EnumerateReferencedAssemblies(this AssemblyDefinition assembly,
            bool includeSelf = true)
        {
            var references = new HashSet<MetadataToken>();

            var queue = new Queue<AssemblyDefinition>();
            queue.Enqueue(assembly);

            if (includeSelf)
            {
                yield return assembly;
            }

            while (queue.Count > 0)
            {
                var assm = queue.Dequeue();

                if (references.Contains(assm.MetadataToken)) continue;
                references.Add(assm.MetadataToken);

                var refs = from m in assm.Modules from r in m.AssemblyReferences select r;
                foreach (var r in refs)
                {
                    AssemblyDefinition refAssm;
                    try
                    {
                        refAssm = assembly.MainModule.AssemblyResolver.Resolve(r);
                        queue.Enqueue(refAssm);
                    }
                    catch (AssemblyResolutionException e)
                    {
                        Debug.WriteLine(e);
                        refAssm = null;
                    }

                    if (refAssm != null)
                        yield return refAssm;
                }
            }
        }
    }
}
