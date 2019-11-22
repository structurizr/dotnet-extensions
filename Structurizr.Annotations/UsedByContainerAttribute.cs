using System;

namespace Structurizr.Annotations
{
    /// <summary>
    /// Specifies that the named container uses the component on which this attribute is placed, creating a relationship
    /// from the container to the component.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = true)]
    public sealed class UsedByContainerAttribute : System.Attribute
    {
        /// <summary>
        /// Gets the name of the container which uses the attributed component.
        /// </summary>
        /// <value>The name of the container which uses the attributed component.</value>
        public string ContainerName { get; }

        /// <summary>
        /// Gets or sets a description of the relationship from the named container to the attributed component.
        /// </summary>
        /// <value>A string describing how the named container uses the attributed component.</value>
        public string Description { get; set; } = "";

        /// <summary>
        /// Gets or sets a description of the technology used to implement the relationship between the named container
        /// and the attributed component.
        /// </summary>
        /// <value>
        /// A string describing the technology used for connecting the named container to the attributed component.
        /// </value>
        public string Technology { get; set; } = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="UsedByContainerAttribute"/> class with the name of the
        /// container using the component specified.
        /// </summary>
        /// <param name="containerName">The name of the container which uses the attributed component.</param>
        public UsedByContainerAttribute(string containerName)
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
