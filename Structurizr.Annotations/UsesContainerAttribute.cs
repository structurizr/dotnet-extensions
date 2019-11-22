using System;

namespace Structurizr.Annotations
{
    /// <summary>
    /// Specifies that the named container is used by the component on which this attribute is placed, creating a
    /// relationship from the component to the container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = true)]
    public sealed class UsesContainerAttribute : System.Attribute
    {
        /// <summary>
        /// Gets the name of the container used by the attributed component.
        /// </summary>
        /// <value>The name of the container which is used by the attributed component.</value>
        public string ContainerName { get; }

        /// <summary>
        /// Gets or sets a description of the relationship from the attributed component to the named container.
        /// </summary>
        /// <value>A string describing how the named container is used by the attributed component.</value>
        public string Description { get; set; } = "";

        /// <summary>
        /// Gets or sets a description of the technology used to implement the relationship between the attributed
        /// component to the named container.
        /// </summary>
        /// <value>
        /// A string describing the technology used for connecting the attributed component to the named container.
        /// </value>
        public string Technology { get; set; } = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="UsesContainerAttribute"/> class with the name of the
        /// container being used by the attributed component.
        /// </summary>
        /// <param name="containerName">The name of the container which is used by the attributed component.</param>
        public UsesContainerAttribute(string containerName)
        {
#if NET20 || PORTABLE2
            if (String.IsNullOrEmpty(containerName) || containerName.Trim() == String.Empty)
#else
            if (String.IsNullOrWhiteSpace(containerName))
#endif
                throw new ArgumentNullException(nameof(containerName));

            this.ContainerName = containerName;
        }
    }
}
