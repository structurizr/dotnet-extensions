using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Structurizr
{

    /// <summary>
    /// An instance of a model item in a View.
    /// </summary>
    [DataContract]
    public abstract class ModelItemView
    {

        internal ModelItemView()
        {
        }

        /// <summary>
        /// The collection of name-value property pairs associated with this model item view, as a Dictionary.
        /// </summary>
        [DataMember(Name = "properties", EmitDefaultValue = false)]
        public Dictionary<string, string> Properties = new Dictionary<string, string>();

        /// <summary>
        /// Adds a name-value pair property to this model item view. 
        /// </summary>
        /// <param name="name">the name of the property</param>
        /// <param name="value">the value of the property</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddProperty(string name, string value)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("A property name must be specified.");
            }

            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentException("A property value must be specified.");
            }

            Properties[name] = value;
        }

    }
}
