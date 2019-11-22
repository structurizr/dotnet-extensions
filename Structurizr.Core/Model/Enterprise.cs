using System;
using System.Runtime.Serialization;

namespace Structurizr
{

    [DataContract]
    public sealed class Enterprise
    {

        /// <summary>
        /// The name of this enterprise.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        public Enterprise(string name)
        {
            if (name == null || name.Trim().Length == 0)
            {
                throw new ArgumentException("Name must be specified.");
            }

            this.Name = name;
        }

    }
}
