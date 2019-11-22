using System;
using System.Runtime.Serialization;
using Structurizr.Util;

namespace Structurizr
{

    [DataContract]
    public sealed class Branding
    {

        [DataMember(Name = "font", EmitDefaultValue = false)]
        public Font Font;

        private string _logo;

        [DataMember(Name = "logo", EmitDefaultValue = false)]
        public string Logo
        {
            get { return _logo; }
            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    if (Url.IsUrl(value) || value.StartsWith("data:image/"))
                    {
                        _logo = value.Trim();
                    }
                    else {
                        throw new ArgumentException(value + " is not a valid URL.");
                    }
                }
            }
        }

    }

}