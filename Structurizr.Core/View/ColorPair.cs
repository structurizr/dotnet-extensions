using System;
using System.Runtime.Serialization;

namespace Structurizr
{

    [DataContract]
    public sealed class ColorPair
    {

        private string _background;
        private string _foreground;

        internal ColorPair()
        { }

        public ColorPair(string background, string foreground)
        {
            this.Background = background;
            this.Foreground = foreground;
        }

        [DataMember(Name = "background", EmitDefaultValue = false)]
        public string Background
        {
            get { return this._background; }
            set
            {
                if (Color.IsHexColorCode(value))
                {
                    this._background = value.ToLower();
                }
                else
                {
                    throw new ArgumentException("'" + value + "' is not a valid hex color code.");
                }
            }
        }

        [DataMember(Name = "foreground", EmitDefaultValue = false)]
        public string Foreground
        {
            get { return this._foreground; }
            set
            {
                if (Color.IsHexColorCode(value))
                {
                    this._foreground = value.ToLower();
                }
                else
                {
                    throw new ArgumentException("'" + value + "' is not a valid hex color code.");
                }
            }
        }

    }

}
