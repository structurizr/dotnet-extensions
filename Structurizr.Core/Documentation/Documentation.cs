using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Structurizr.Documentation
{

    /// <summary>
    /// Represents the documentation within a workspace - a collection of
    /// content in Markdown or AsciiDoc format, optionally with attached images.
    ///
    /// See https://structurizr.com/help/documentation on the Structurizr website for more details.
    /// </summary>
    [DataContract]
    public sealed class Documentation
    {

        public Model Model { get; set; }

        [DataMember(Name = "sections", EmitDefaultValue = false)]
        public ISet<Section> Sections { get; internal set; }

        [DataMember(Name = "decisions", EmitDefaultValue = false)]
        public ISet<Decision> Decisions { get; internal set; }

        [DataMember(Name = "images", EmitDefaultValue = false)]
        public ISet<Image> Images { get; internal set; }

        [JsonConstructor]
        internal Documentation()
        {
            Sections = new HashSet<Section>();
            Decisions = new HashSet<Decision>();
            Images = new HashSet<Image>();
        }

        public Documentation(Model model) : this()
        {
            if (model == null)
            {
                throw new ArgumentException("A model must be specified.");
            }
            
            Model = model;
        }

        public void Hydrate()
        {
            foreach (Section section in Sections)
            {
                if (!string.IsNullOrWhiteSpace(section.ElementId))
                {
                    section.Element = Model.GetElement(section.ElementId);
                }
            }

            foreach (Decision decision in Decisions)
            {
                if (!string.IsNullOrWhiteSpace(decision.ElementId))
                {
                    decision.Element = Model.GetElement(decision.ElementId);
                }
            }
        }

        internal Section AddSection(Element element, string title, Format format, string content)
        {
            if (element != null && !Model.Contains(element))
            {
                throw new ArgumentException("The element named " + element.Name + " does not exist in the model associated with this documentation.");
            }

            CheckTitleIsSpecified(title);
            CheckContentIsSpecified(content);
            CheckSectionIsUnique(element, title);
            CheckFormatIsSpecified(format);

            Section section = new Section(element, title, CalculateOrder(), format, content);
            Sections.Add(section);
            return section;
        }

        private void CheckTitleIsSpecified(string title)
        {
            if (String.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("A title must be specified.");
            }
        }

        private void CheckContentIsSpecified(string title)
        {
            if (String.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Content must be specified.");
            }
        }

        private void CheckFormatIsSpecified(Format format)
        {
            if (format == null)
            {
                throw new ArgumentException("A format must be specified.");
            }
        }

        private void CheckSectionIsUnique(Element element, String title)
        {
            if (element == null)
            {
                foreach (Section section in Sections)
                {
                    if (section.Element == null && title.Equals(section.Title))
                    {
                        throw new ArgumentException("A section with a title of " + title + " already exists for this workspace.");
                    }
                }
            }
            else
            {
                foreach (Section section in Sections)
                {
                    if (element.Id.Equals(section.ElementId) && title.Equals(section.Title))
                    {
                        throw new ArgumentException("A section with a title of " + title + " already exists for the element named " + element.Name + ".");
                    }
                }
            }
        }

        public Decision AddDecision(string id, DateTime date, string title, DecisionStatus status, Format format, string content)
        {
            return AddDecision(null, id, date, title, status, format, content);
        }

        public Decision AddDecision(SoftwareSystem softwareSystem, string id, DateTime date, string title, DecisionStatus status, Format format, string content)
        {
            CheckIdIsSpecified(id);
            CheckTitleIsSpecified(title);
            CheckContentIsSpecified(content);
            CheckDecisionStatusIsSpecified(status);
            CheckFormatIsSpecified(format);
            CheckDecisionIsUnique(softwareSystem, id);

            Decision decision = new Decision(softwareSystem, id, date, title, status, format, content);
            Decisions.Add(decision);

            return decision;
        }

        private void CheckIdIsSpecified(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("An ID must be specified.");
            }
        }

        private void CheckDecisionStatusIsSpecified(DecisionStatus status)
        {
            if (status == null)
            {
                throw new ArgumentException("A status must be specified.");
            }
        }

        private void CheckDecisionIsUnique(Element element, string id)
        {
            if (element == null)
            {
                foreach (Decision decision in Decisions)
                {
                    if (decision.Element == null && id.Equals(decision.Id))
                    {
                        throw new ArgumentException("A decision with an ID of " + id + " already exists for this workspace.");
                    }
                }
            }
            else
            {
                foreach (Decision decision in Decisions)
                {
                    if (element.Id.Equals(decision.ElementId) && id.Equals(decision.Id))
                    {
                        throw new ArgumentException("A decision with an ID of " + id + " already exists for the element named " + element.Name + ".");
                    }
                }
            }
        }


        private int CalculateOrder()
        {
            return Sections.Count+1;
        }

        internal void Add(Image image)
        {
            Images.Add(image);
        }
        
        public bool IsEmpty()
        {
            return Sections.Count == 0 && Images.Count == 0;
        }

    }

}