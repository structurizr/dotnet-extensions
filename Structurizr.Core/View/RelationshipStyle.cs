using System;
using System.Runtime.Serialization;

namespace Structurizr
{

    /// <summary>
    /// A definition of a relationship style.
    /// </summary>
    [DataContract]
    public sealed class RelationshipStyle
    {

        /// <summary>
        /// The tag to which this relationship style applies.
        /// </summary>
        [DataMember(Name="tag", EmitDefaultValue=false)]
        public string Tag { get; set; }

        /// <summary>
        /// The thickness of the line, in pixels.
        /// </summary>
        [DataMember(Name="thickness", EmitDefaultValue=false)]
        public int? Thickness { get; set; }

        private string _color;

        /// <summary>
        /// The colour of the line, as a HTML RGB hex string (e.g. #123456)
        /// </summary>
        [DataMember(Name = "color", EmitDefaultValue = false)]
        public string Color
        { 
            get {
                return this._color;
            }

            set
            {
                if (Structurizr.Color.IsHexColorCode(value))
                {
                    this._color = value.ToLower();
                }
                else
                {
                    throw new ArgumentException("'" + value + "' is not a valid hex color code.");
                }
            }
        }

        /// <summary>
        /// The standard font size used to render the relationship annotation, in pixels.
        /// </summary>
        [DataMember(Name="fontSize", EmitDefaultValue=false)]
        public int? FontSize { get; set; }
        
        /// <summary>
        /// The width of the relationship annotation, in pixels.
        /// </summary>
        [DataMember(Name="width", EmitDefaultValue=false)]
        public int? Width { get; set; }
        
        /// <summary>
        /// A flag to indicate whether the line is rendered as dashed or not.
        /// </summary>
        /// <value>A flag to indicate whether the line is rendered as dashed or not.</value>
        [DataMember(Name="dashed", EmitDefaultValue=false)]
        public bool? Dashed { get; set; } 
       
        /// <summary>
        /// The routing of the line.
        /// </summary>
        [DataMember(Name="routing", EmitDefaultValue=false)]
        public Routing? Routing { get; set; }

        private int? _position;

        /// <summary>
        /// The position of the annotation along the line; 0 (start) to 100 (end).
        /// </summary>
        [DataMember(Name="position", EmitDefaultValue=false)]
        public int? Position
        {
            get { return _position; }
            set
            {
                if (value != null)
                {
                    if (value < 0)
                    {
                        _position = 0;
                    }
                    else if (value > 100)
                    {
                        _position = 100;
                    }
                    else {
                        _position = value;
                    }
                }
            }
        }

        private int? _opacity;

        /// <summary>
        /// The opacity of the line/text; 0 to 100.
        /// </summary>
        [DataMember(Name = "opacity", EmitDefaultValue = false)]
        public int? Opacity
        {
            get { return _opacity; }
            set
            {
                if (value != null)
                {
                    if (value < 0)
                    {
                        _opacity = 0;
                    }
                    else if (value > 100)
                    {
                        _opacity = 100;
                    }
                    else {
                        _opacity = value;
                    }
                }
            }
        }

        internal RelationshipStyle()
        {
        }

        public RelationshipStyle(string tag)
        {
            this.Tag = tag;
        }  
        
    }
}
