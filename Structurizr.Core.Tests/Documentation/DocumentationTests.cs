using System;
using System.Collections.Generic;
using Structurizr.Documentation;
using Xunit;

namespace Structurizr.Core.Tests.Documentation
{

    public class DocumentationTests : AbstractTestBase
    {

        private Structurizr.Documentation.Documentation _documentation;

        public DocumentationTests()
        {
            _documentation = Workspace.Documentation;
        }

        [Fact]
        public void test_addSection_ThrowsAnException_WhenTheRelatedElementIsNotPresentInTheAssociatedModel()
        {
            try
            {
                SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
                new Workspace("", "").Documentation.AddSection(softwareSystem, "Title", Format.Markdown, "Content");
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("The element named Software System does not exist in the model associated with this documentation.", iae.Message);
            }
        }

        [Fact]
        public void test_addSection_ThrowsAnException_WhenTheTitleIsNotSpecified()
        {
            try
            {
                _documentation.AddSection(null, null, Format.Markdown, "Content");
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("A title must be specified.", iae.Message);
            }
        }

        [Fact]
        public void test_addSection_ThrowsAnException_WhenTheContentIsNotSpecified()
        {
            try
            {
                _documentation.AddSection(null, "Title", Format.Markdown, null);
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("Content must be specified.", iae.Message);
            }
        }

        [Fact]
        public void test_addSection_ThrowsAnException_WhenASectionExistsWithTheSameTitle()
        {
            try
            {
                _documentation.AddSection(null, "Title", Format.Markdown, "Content");
                _documentation.AddSection(null, "Title", Format.Markdown, "Content");
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("A section with a title of Title already exists for this workspace.", iae.Message);
            }
        }

        [Fact]
        public void test_addSection_ThrowsAnException_WhenASectionExistsWithTheSameTitleForAnElement()
        {
            try
            {
                SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
                _documentation.AddSection(softwareSystem, "Title", Format.Markdown, "Content");
                _documentation.AddSection(softwareSystem, "Title", Format.Markdown, "Content");
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("A section with a title of Title already exists for the element named Software System.", iae.Message);
            }
        }

        [Fact]
        public void test_addSection()
        {
            SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            Section section = _documentation.AddSection(softwareSystem, "Title", Format.Markdown, "Content");

            Assert.Equal(1, _documentation.Sections.Count);
            Assert.True(_documentation.Sections.Contains(section));
            Assert.Same(softwareSystem, section.Element);
            Assert.Equal("Title", section.Title);
            Assert.Equal(Format.Markdown, section.Format);
            Assert.Equal("Content", section.Content);
            Assert.Equal(1, section.Order);
        }

        [Fact]
        public void test_addSection_IncrementsTheSectionOrderNumber()
        {
            SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            Section section1 = _documentation.AddSection(softwareSystem, "Section 1", Format.Markdown, "Content");
            Section section2 = _documentation.AddSection(softwareSystem, "Section 2", Format.Markdown, "Content");
            Section section3 = _documentation.AddSection(softwareSystem, "Section 3", Format.Markdown, "Content");

            Assert.Equal(1, section1.Order);
            Assert.Equal(2, section2.Order);
            Assert.Equal(3, section3.Order);
        }

        [Fact]
        public void test_addDecision_ThrowsAnException_WhenTheIdIsNotSpecified()
        {
            try
            {
                _documentation.AddDecision(null, new DateTime(), "Title", DecisionStatus.Accepted, Format.Markdown, "Content");
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("An ID must be specified.", iae.Message);
            }
        }

        [Fact]
        public void test_addDecision_ThrowsAnException_WhenTheTitleIsNotSpecified()
        {
            try
            {
                _documentation.AddDecision("1", new DateTime(), null, DecisionStatus.Accepted, Format.Markdown, "Content");
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("A title must be specified.", iae.Message);
            }
        }

        [Fact]
        public void test_addDecision_ThrowsAnException_WhenTheContentIsNotSpecified()
        {
            try
            {
                _documentation.AddDecision("1", new DateTime(), "Title", DecisionStatus.Accepted, Format.Markdown, null);
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("Content must be specified.", iae.Message);
            }
        }

        [Fact]
        public void test_addDecision_ThrowsAnException_WhenADecisionExistsWithTheSameId()
        {
            try
            {
                _documentation.AddDecision("1", new DateTime(), "Title", DecisionStatus.Accepted, Format.Markdown, "Content");
                _documentation.AddDecision("1", new DateTime(), "Title", DecisionStatus.Accepted, Format.Markdown, "Content");
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("A decision with an ID of 1 already exists for this workspace.", iae.Message);
            }
        }

        [Fact]
        public void test_addDecision_ThrowsAnException_WhenADecisionExistsWithTheSameIdForAnElement()
        {
            try
            {
                SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
                _documentation.AddDecision(softwareSystem, "1", new DateTime(), "Title", DecisionStatus.Accepted, Format.Markdown, "Content");
                _documentation.AddDecision(softwareSystem, "1", new DateTime(), "Title", DecisionStatus.Accepted, Format.Markdown, "Content");
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("A decision with an ID of 1 already exists for the element named Software System.", iae.Message);
            }
        }

        [Fact]
        public void test_hydrate()
        {
            SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");

            Section section = new Section();
            section.ElementId = softwareSystem.Id;
            section.Title = "Title";
            _documentation.Sections = new HashSet<Section>() { section };

            Decision decision = new Decision();
            decision.Id = "1";
            decision.ElementId = softwareSystem.Id;
            _documentation.Decisions = new HashSet<Decision>() { decision };

            _documentation.Hydrate();

            Assert.Same(softwareSystem, section.Element);
            Assert.Same(softwareSystem, decision.Element);
        }

    }

}