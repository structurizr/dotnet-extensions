using System;
using System.IO;
using Structurizr.Documentation;
using Xunit;

namespace Structurizr.Core.Tests.Documentation
{
    public class ViewpointsAndPerspectivesDocumentationTests : AbstractTestBase
    {

        private SoftwareSystem _softwareSystem;
        private ViewpointsAndPerspectivesDocumentation _template;

        public ViewpointsAndPerspectivesDocumentationTests()
        {
            _softwareSystem = Model.AddSoftwareSystem("Name", "Description");
    
            _template = new ViewpointsAndPerspectivesDocumentation(Workspace);
        }

        [Fact]
        public void Test_Construction_ThrowsAnException_WhenANullWorkspaceIsSpecified() {
            try {
                new StructurizrDocumentationTemplate(null);
                throw new TestFailedException();
            } catch (ArgumentException ae) {
                Assert.Equal("A workspace must be specified.", ae.Message);
            }
        }
    
        [Fact]
        public void Test_AddAllSectionsWithContentAsStrings()
        {
            Section section;
    
            section = _template.AddIntroductionSection(_softwareSystem, Format.Markdown, "Section 1");
            AssertSection(_softwareSystem, "Introduction", Format.Markdown, "Section 1", 1, section);
    
            section = _template.AddGlossarySection(_softwareSystem, Format.Markdown, "Section 2");
            AssertSection(_softwareSystem, "Glossary", Format.Markdown, "Section 2", 2, section);
    
            section = _template.AddSystemStakeholdersAndRequirementsSection(_softwareSystem, Format.Markdown, "Section 3");
            AssertSection(_softwareSystem, "System Stakeholders and Requirements", Format.Markdown, "Section 3", 3, section);
    
            section = _template.AddArchitecturalForcesSection(_softwareSystem, Format.Markdown, "Section 4");
            AssertSection(_softwareSystem, "Architectural Forces", Format.Markdown, "Section 4", 4, section);
    
            section = _template.AddArchitecturalViewsSection(_softwareSystem, Format.Markdown, "Section 5");
            AssertSection(_softwareSystem, "Architectural Views", Format.Markdown, "Section 5", 5, section);
    
            section = _template.AddSystemQualitiesSection(_softwareSystem, Format.Markdown, "Section 6");
            AssertSection(_softwareSystem, "System Qualities", Format.Markdown, "Section 6", 6, section);
    
            section = _template.AddAppendicesSection(_softwareSystem, Format.Markdown, "Section 7");
            AssertSection(_softwareSystem, "Appendices", Format.Markdown, "Section 7", 7, section);
        }
    
        [Fact]
        public void Test_AddAllSectionsWithContentFromFiles()
        {
            Section section;
            DirectoryInfo root = new DirectoryInfo("Documentation" + Path.DirectorySeparatorChar + "viewpointsandperspectives");
    
            section = _template.AddIntroductionSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "01-introduction.md")));
            AssertSection(_softwareSystem, "Introduction", Format.Markdown, "Section 1", 1, section);
    
            section = _template.AddGlossarySection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "02-glossary.md")));
            AssertSection(_softwareSystem, "Glossary", Format.Markdown, "Section 2", 2, section);
    
            section = _template.AddSystemStakeholdersAndRequirementsSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "03-system-stakeholders-and-requirements.md")));
            AssertSection(_softwareSystem, "System Stakeholders and Requirements", Format.Markdown, "Section 3", 3, section);
    
            section = _template.AddArchitecturalForcesSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "04-architectural-forces.md")));
            AssertSection(_softwareSystem, "Architectural Forces", Format.Markdown, "Section 4", 4, section);
    
            section = _template.AddArchitecturalViewsSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "05-architectural-views.md")));
            AssertSection(_softwareSystem, "Architectural Views", Format.Markdown, "Section 5", 5, section);
    
            section = _template.AddSystemQualitiesSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "06-system-qualities.md")));
            AssertSection(_softwareSystem, "System Qualities", Format.Markdown, "Section 6", 6, section);
    
            section = _template.AddAppendicesSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "07-appendices.adoc")));
            AssertSection(_softwareSystem, "Appendices", Format.AsciiDoc, "Section 7", 7, section);
        }
    
        private void AssertSection(Element element, string type, Format format, string content, int order, Section section)
        {
            Assert.True(Workspace.Documentation.Sections.Contains(section));
            Assert.Equal(element, section.Element);
            Assert.Equal(element.Id, section.ElementId);
            Assert.Equal(type, section.Title);
            Assert.Equal(format, section.Format);
            Assert.Equal(content, section.Content);
            Assert.Equal(order, section.Order);
        }
       
    }
}