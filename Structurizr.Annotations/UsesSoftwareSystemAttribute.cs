using System;

namespace Structurizr.Annotations
{
    /// <summary>
    /// Specifies that the named software system is used by the component on which this attribute is placed, creating a
    /// relationship from the component to the software system.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = true)]
    public sealed class UsesSoftwareSystemAttribute : System.Attribute
    {
        /// <summary>
        /// Gets the name of the software system used by the attributed component.
        /// </summary>
        /// <value>The name of the software system which is used by the attributed component.</value>
        public string SoftwareSystemName { get; }

        /// <summary>
        /// Gets or sets a description of the relationship from the attributed component to the named software system.
        /// </summary>
        /// <value>A string describing how the named software system is used by the attributed component.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a description of the technology used to implement the relationship between the named software
        /// system and the attributed component.
        /// </summary>
        /// <value>
        /// A string describing the technology used for connecting the attributed component to the named software
        /// system.
        /// </value>
        public string Technology { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsesSoftwareSystemAttribute"/> class with the name of the
        /// software system used by the component specified.
        /// </summary>
        /// <param name="softwareSystemName">
        /// The name of the software system which is used by the attributed component.
        /// </param>
        public UsesSoftwareSystemAttribute(string softwareSystemName)
        {
#if NET20 || PORTABLE2
            if (String.IsNullOrEmpty(softwareSystemName) || softwareSystemName.Trim() == String.Empty)
#else
            if (String.IsNullOrWhiteSpace(softwareSystemName))
#endif
                throw new ArgumentNullException(nameof(softwareSystemName));

            this.SoftwareSystemName = softwareSystemName;
        }
    }
}
