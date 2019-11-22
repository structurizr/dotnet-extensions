using System;

namespace Structurizr.Annotations
{
    /// <summary>
    /// Specifies that the attributed type class or interface can be considered to be a "component".
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct,
        Inherited = false,
        AllowMultiple = false)]
    public sealed class CodeElementAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the component to which the attributed type belongs.
        /// </summary>
        /// <value>The name of the component which includes the attributed type.</value>
        public string ComponentName { get; }

        /// <summary>
        /// Gets or sets a description of the code element.
        /// </summary>
        /// <value>A description of the code element.</value>
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentAttribute"/> class.
        /// </summary>
        public CodeElementAttribute(string componentName)
         {
#if NET20 || PORTABLE2
            if (String.IsNullOrEmpty(componentName) || componentName.Trim() == String.Empty)
#else
            if (String.IsNullOrWhiteSpace(componentName))
#endif
                throw new ArgumentNullException(nameof(componentName));

            this.ComponentName = componentName;
        }
    }
}
