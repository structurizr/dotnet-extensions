using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Structurizr
{
    
    [DataContract]
    public sealed class Animation
    {
        
        [DataMember(Name = "order", EmitDefaultValue = false)]
        public int Order { get; internal set; }

        private HashSet<string> _elements;

        [DataMember(Name = "elements", EmitDefaultValue = false)]
        public ISet<string> Elements
        {
            get
            {
                return new HashSet<string>(_elements);
            }

            internal set
            {
                _elements = new HashSet<string>(value);
            }
        }

        private HashSet<string> _relationships;

        [DataMember(Name = "relationships", EmitDefaultValue = false)]
        public ISet<string> Relationships
        {
            get
            {
                return new HashSet<string>(_relationships);
            }

            internal set
            {
                _relationships = new HashSet<string>(value);
            }
        }

        internal Animation()
        {
            _elements = new HashSet<string>();
            _relationships = new HashSet<string>();
        }
        
        internal Animation(int order, ISet<Element> elements, ISet<Relationship> relationships) : this()
        {
            Order = order;

            foreach (Element element in elements)
            {
                _elements.Add(element.Id);
            }

            foreach (Relationship relationship in relationships)
            {
                _relationships.Add(relationship.Id);
            }
        }

    }
}