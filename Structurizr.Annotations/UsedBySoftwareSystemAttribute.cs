using System;

namespace Structurizr.Annotations
{
    /// <summary>
    /// Specifies that the named software system uses the component on which this attribute is placed, creating a
    /// relationship from the software system to the component.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = true)]
    public sealed class UsedBySoftwareSystemAttribute : System.Attribute
    {
        /// <summary>
        /// Gets the name of the software system who uses the attributed component.
        /// </summary>
        /// <value>The name of the software system who uses the attributed component.</value>
        public string SoftwareSystemName { get; }

        /// <summary>
        /// Gets or sets a description of the relationship from the named software system to the attributed component.
        /// </summary>
        /// <value>A string describing how the named software system uses the attributed component.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a description of the technology used to implement the relationship between the named software
        /// system and the attributed component.
        /// </summary>
        /// <value>
        /// A string describing the technology used for connecting the named software system to the attributed
        /// component.
        /// </value>
        public string Technology { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsedBySoftwareSystemAttribute"/> class with the name of the
        /// software system using the component specified.
        /// </summary>
        /// <param name="softwareSystemName">The name of the software system which uses the attributed component.</param>
        public UsedBySoftwareSystemAttribute(string softwareSystemName)
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
