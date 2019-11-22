using System.Runtime.Serialization;

namespace Structurizr.Documentation
{

    [DataContract]
    public sealed class Section
    {

        public Element Element { get; internal set; }

        private string _elementId;

        /// <summary>
        /// The ID of the element.
        /// </summary>
        [DataMember(Name = "elementId", EmitDefaultValue = false)]
        public string ElementId
        {
            get
            {
                if (this.Element != null)
                {
                    return this.Element.Id;
                }
                else
                {
                    return _elementId;
                }
            }

            set
            {
                _elementId = value;
            }
        }

        [DataMember(Name = "title", EmitDefaultValue = true)]
        public string Title { get; internal set; }

        /// <summary>
        /// (this is for backwards compatibility with older client libraries)
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = true)]
        internal string SectionType
        {
            set { Title = value; }
        }

        [DataMember(Name = "order", EmitDefaultValue = true)]
        public int Order { get; internal set; }
        
        [DataMember(Name = "format", EmitDefaultValue = true)]
        public Format Format { get; internal set; }

        [DataMember(Name = "content", EmitDefaultValue = false)]
        public string Content { get; internal set; }

        internal Section() { }

        internal Section(Element element, string title, int order, Format format, string content) {
            Element = element;
            Title = title;
            Order = order;
            Format = format;
            Content = content;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Section);
        }

        public bool Equals(Section section)
        {
            if (section == this)
            {
                return true;
            }

            if (section == null)
            {
                return false;
            }
            
            if (ElementId != null)
            {
                return ElementId.Equals(section.ElementId) && Title == section.Title;
            }
            else
            {
                return Title == section.Title;
            }
        }

        public override int GetHashCode()
        {
            int result = ElementId != null ? ElementId.GetHashCode() : 0;
            result = 31 * result + Title.GetHashCode();
            return result;
        }

    }
}