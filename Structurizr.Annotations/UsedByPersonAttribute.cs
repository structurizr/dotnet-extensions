using System;

namespace Structurizr.Annotations
{
    /// <summary>
    /// Specifies that the named person uses the component on which this attribute is placed, creating a relationship
    /// from the person to the component.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = true)]
    public sealed class UsedByPersonAttribute : System.Attribute
    {
        /// <summary>
        /// Gets the name of the person who uses the attributed component.
        /// </summary>
        /// <value>The name of the person who uses the attributed component.</value>
        public string PersonName { get; }

        /// <summary>
        /// Gets or sets a description of the relationship from the named person to the attributed component.
        /// </summary>
        /// <value>A string describing how the named person uses the attributed component.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a description of the technology used to implement the relationship between the named person
        /// and the attributed component.
        /// </summary>
        /// <value>
        /// A string describing the technology used for connecting the named person to the attributed component.
        /// </value>
        public string Technology { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsedByPersonAttribute"/> class with the name of the person
        /// using the component specified.
        /// </summary>
        /// <param name="personName">The name of the person which uses the attributed component.</param>
        public UsedByPersonAttribute(string personName)
        {
#if NET20 || PORTABLE2
            if (String.IsNullOrEmpty(personName) || personName.Trim() == String.Empty)
#else
            if (String.IsNullOrWhiteSpace(personName))
#endif
                throw new ArgumentNullException(nameof(personName));

            this.PersonName = personName;
        }
    }
}
