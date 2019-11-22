using System;

namespace Structurizr.Annotations
{
    /// <summary>
    /// Specifies that the type of the attributed field, property, or parameter is a component used by the type to which
    /// the attributed field, property, or parameter belongs.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In documentation for this annotation, the "attributed component" refers to the type containing the field,
    /// property, or parameter on which the attribute is placed. The "used component" is the component specified by the
    /// type of the field, property, or parameter.
    /// </para>
    /// </remarks>
    [AttributeUsage(
        AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter,
        Inherited = false,
        AllowMultiple = false)]
    public sealed class UsesComponentAttribute : System.Attribute
    {
        /// <summary>
        /// Gets a description of the relationship from the attributed component to the used component.
        /// </summary>
        /// <value>A string describing how the used component is used by the attributed component.</value>
        public string Description { get; }

        /// <summary>
        /// Gets or sets a description of the technology used to implement the relationship between the attributed
        /// component to the used component.
        /// </summary>
        /// <value>
        /// A string describing the technology used for connecting the attributed component to the used component.
        /// </value>
        public string Technology { get; set; } = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="UsesComponentAttribute"/> class with a description of the
        /// relationship from the attributed component to the used component.
        /// </summary>
        /// <param name="description">
        /// A string describing how the used component is used by the attributed component.
        /// </param>
        public UsesComponentAttribute(string description)
        {
#if NET20 || PORTABLE2
            if (String.IsNullOrEmpty(description) || description.Trim() == String.Empty)
#else
            if (String.IsNullOrWhiteSpace(description))
#endif
                throw new ArgumentNullException(nameof(description));

            this.Description = description;
        }
    }
}