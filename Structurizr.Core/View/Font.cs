using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Structurizr
{

    [DataContract]
    public sealed class Font
    {

        private string _url;

        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name;

        /// <summary>
        /// The URL where more information about this element can be found.
        /// </summary>
        [DataMember(Name = "url", EmitDefaultValue = false)]
        public string Url
        {
            get
            {
                return _url;
            }

            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    if (Util.Url.IsUrl(value))
                    {
                        this._url = value;
                    }
                    else
                    {
                        throw new ArgumentException(value + " is not a valid URL.");
                    }
                }
            }
        }

        internal Font()
        {
        }
        
        public Font(string name)
        {
            this.Name = name;
        }

        public Font(string name, string url)
        {
            this.Name = name;
            this.Url = url;
        }

    }

}
