using System;
using System.Runtime.Serialization;

namespace Structurizr.Documentation
{

    /// <summary>
    /// Represents a single (architecture) decision, as described at http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions
    /// </summary>
    [DataContract]
    public sealed class Decision
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

        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; internal set; }

        [DataMember(Name = "date", EmitDefaultValue = false)]
        public DateTime Date { get; internal set; }

        [DataMember(Name = "title", EmitDefaultValue = false)]
        public string Title { get; internal set; }

        [DataMember(Name = "status", EmitDefaultValue = true)]
        public DecisionStatus Status { get; internal set; }

        [DataMember(Name = "format", EmitDefaultValue = true)]
        public Format Format { get; internal set; }

        [DataMember(Name = "content", EmitDefaultValue = false)]
        public string Content { get; internal set; }

        internal Decision()
        {
        }

        internal Decision(Element element, string id, DateTime date, string title, DecisionStatus status, Format format, string content)
        {
            Element = element;
            Id = id;
            Date = date;
            Title = title;
            Status = status;
            Format = format;
            Content = content;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Decision);
        }

        public bool Equals(Decision decision)
        {
            if (decision == this)
            {
                return true;
            }

            if (decision == null)
            {
                return false;
            }

            if (ElementId != null)
            {
                return ElementId.Equals(decision.ElementId) && Id == decision.Id;
            }
            else
            {
                return Id == decision.Id;
            }
        }

        public override int GetHashCode()
        {
            int result = ElementId != null ? ElementId.GetHashCode() : 0;
            result = 31 * result + Id.GetHashCode();
            return result;
        }


    }

}