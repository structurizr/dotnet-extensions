using System;

namespace Structurizr.Annotations
{
    /// <summary>
    /// Specifies that the attributed type class or interface can be considered to be a "component".
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public sealed class ComponentAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a description of the component.
        /// </summary>
        /// <value>A description of the component.</value>
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets a description of the technology used to implement the component.
        /// </summary>
        /// <value>A description of the technology used to implement the component.</value>
        public string Technology { get; set; } = String.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentAttribute"/> class.
        /// </summary>
        public ComponentAttribute() {}
    }
}
