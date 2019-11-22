using System.Collections.Generic;

using Mono.Cecil;

namespace Structurizr.Analysis
{
    /// <summary>
    /// Declares an interface for a repository of type information from Mono.Cecil.
    /// </summary>
    public interface ITypeRepository
    {
        /// <summary>
        /// Gets the root namespace of the types stored in the repository.
        /// </summary>
        /// <value>The name of the root namespace of the types in the repository.</value>
        string Namespace { get; }

        /// <summary>
        /// Gets all of the types stored in this type repository.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="TypeDefinition"/> objects, or an empty enumeration if no types were found.
        /// </returns>
        IEnumerable<TypeDefinition> GetAllTypes();

        /// <summary>
        /// Gets a single type from the repository by name.
        /// </summary>
        /// <param name="typeName">The name of the type to return.</param>
        /// <returns>
        /// A <see cref="TypeDefinition"/> for the specified type, or <see langword="null"/> if the type could not
        /// be found.
        /// </returns>
        TypeDefinition GetType(string typeName);

        /// <summary>
        /// Gets all of the types in the repository that are referenced by the named type.
        /// </summary>
        /// <param name="type">The name of the type whose references are to be returned.</param>
        /// <returns>
        /// A collection of <see cref="TypeDefinition"/> objects representing the types referenced by the type named by
        /// <paramref name="type"/>, or an empty enumeration if no types were found.
        /// </returns>
        IEnumerable<string> GetReferencedTypes(string type);

        /// <summary>
        /// Gets the type category (class, interface, etc.) of the named type.
        /// </summary>
        /// <param name="typeName">The name of the type being categorized.</param>
        /// <returns>The type category of the type if not "class" or "struct", otherwise <see langword="null"/>.</returns>
        string FindCategory(string typeName);

        /// <summary>
        /// Gets the access modifier of the named type.
        /// </summary>
        /// <param name="typeName">The name of the type whose access modifier is being looked up.</param>
        /// <returns>
        /// The type's access modifier ("public" or "internal") if the type is found, otherwise <see langword="null"/>.
        /// </returns>
        string FindVisibility(string typeName);
    }
}
