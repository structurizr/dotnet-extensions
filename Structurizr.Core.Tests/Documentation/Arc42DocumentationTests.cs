using System;
using System.IO;
using Structurizr.Documentation;
using Xunit;

namespace Structurizr.Core.Tests.Documentation
{

    public class Arc42DocumentationTests : AbstractTestBase
    {
    
        private SoftwareSystem softwareSystem;
        private Arc42DocumentationTemplate _template;

        public Arc42DocumentationTests()
        {
            softwareSystem = Model.AddSoftwareSystem("Name", "Description");
    
            _template = new Arc42DocumentationTemplate(Workspace);
        }

        [Fact]
        public void Test_Construction_ThrowsAnException_WhenANullWorkspaceIsSpecified() {
            try {
                new Arc42DocumentationTemplate(null);
                throw new TestFailedException();
            } catch (ArgumentException ae) {
                Assert.Equal("A workspace must be specified.", ae.Message);
            }
        }

        [Fact]
        public void test_addAllSectionsWithContentAsStrings()
        {
            Section section;
    
            section = _template.AddIntroductionAndGoalsSection(softwareSystem, Format.Markdown, "Section 1");
            AssertSection(softwareSystem, "Introduction and Goals", Format.Markdown, "Section 1", 1, section);
    
            section = _template.AddConstraintsSection(softwareSystem, Format.Markdown, "Section 2");
            AssertSection(softwareSystem, "Constraints", Format.Markdown, "Section 2", 2, section);
    
            section = _template.AddContextAndScopeSection(softwareSystem, Format.Markdown, "Section 3");
            AssertSection(softwareSystem, "Context and Scope", Format.Markdown, "Section 3", 3, section);
    
            section = _template.AddSolutionStrategySection(softwareSystem, Format.Markdown, "Section 4");
            AssertSection(softwareSystem, "Solution Strategy", Format.Markdown, "Section 4", 4, section);
    
            section = _template.AddBuildingBlockViewSection(softwareSystem, Format.Markdown, "Section 5");
            AssertSection(softwareSystem, "Building Block View", Format.Markdown, "Section 5", 5, section);
    
            section = _template.AddRuntimeViewSection(softwareSystem, Format.Markdown, "Section 6");
            AssertSection(softwareSystem, "Runtime View", Format.Markdown, "Section 6", 6, section);
    
            section = _template.AddDeploymentViewSection(softwareSystem, Format.Markdown, "Section 7");
            AssertSection(softwareSystem, "Deployment View", Format.Markdown, "Section 7", 7, section);
    
            section = _template.AddCrosscuttingConceptsSection(softwareSystem, Format.Markdown, "Section 8");
            AssertSection(softwareSystem, "Crosscutting Concepts", Format.Markdown, "Section 8", 8, section);
    
            section = _template.AddArchitecturalDecisionsSection(softwareSystem, Format.Markdown, "Section 9");
            AssertSection(softwareSystem, "Architectural Decisions", Format.Markdown, "Section 9", 9, section);
    
            section = _template.AddQualityRequirementsSection(softwareSystem, Format.Markdown, "Section 10");
            AssertSection(softwareSystem, "Quality Requirements", Format.Markdown, "Section 10", 10, section);
    
            section = _template.AddRisksAndTechnicalDebtSection(softwareSystem, Format.Markdown, "Section 11");
            AssertSection(softwareSystem, "Risks and Technical Debt", Format.Markdown, "Section 11", 11, section);
    
            section = _template.AddGlossarySection(softwareSystem, Format.Markdown, "Section 12");
            AssertSection(softwareSystem, "Glossary", Format.Markdown, "Section 12", 12, section);
        }

        [Fact]
        public void test_addAllSectionsWithContentFromFiles()
        {
            Section section;
            DirectoryInfo root = new DirectoryInfo("Documentation" + Path.DirectorySeparatorChar + "arc42");
    
            section = _template.AddIntroductionAndGoalsSection(softwareSystem, new FileInfo(Path.Combine(root.FullName, "introduction-and-goals.md")));
            AssertSection(softwareSystem, "Introduction and Goals", Format.Markdown, "Section 1", 1, section);
    
            section = _template.AddConstraintsSection(softwareSystem, new FileInfo(Path.Combine(root.FullName, "constraints.md")));
            AssertSection(softwareSystem, "Constraints", Format.Markdown, "Section 2", 2, section);
    
            section = _template.AddContextAndScopeSection(softwareSystem, new FileInfo(Path.Combine(root.FullName, "context-and-scope.md")));
            AssertSection(softwareSystem, "Context and Scope", Format.Markdown, "Section 3", 3, section);
    
            section = _template.AddSolutionStrategySection(softwareSystem, new FileInfo(Path.Combine(root.FullName, "solution-strategy.md")));
            AssertSection(softwareSystem, "Solution Strategy", Format.Markdown, "Section 4", 4, section);
    
            section = _template.AddBuildingBlockViewSection(softwareSystem, new FileInfo(Path.Combine(root.FullName, "building-block-view.md")));
            AssertSection(softwareSystem, "Building Block View", Format.Markdown, "Section 5", 5, section);
    
            section = _template.AddRuntimeViewSection(softwareSystem, new FileInfo(Path.Combine(root.FullName, "runtime-view.md")));
            AssertSection(softwareSystem, "Runtime View", Format.Markdown, "Section 6", 6, section);
    
            section = _template.AddDeploymentViewSection(softwareSystem, new FileInfo(Path.Combine(root.FullName, "deployment-view.md")));
            AssertSection(softwareSystem, "Deployment View", Format.Markdown, "Section 7", 7, section);
    
            section = _template.AddCrosscuttingConceptsSection(softwareSystem, new FileInfo(Path.Combine(root.FullName, "crosscutting-concepts.md")));
            AssertSection(softwareSystem, "Crosscutting Concepts", Format.Markdown, "Section 8", 8, section);
    
            section = _template.AddArchitecturalDecisionsSection(softwareSystem, new FileInfo(Path.Combine(root.FullName, "architectural-decisions.md")));
            AssertSection(softwareSystem, "Architectural Decisions", Format.Markdown, "Section 9", 9, section);
    
            section = _template.AddQualityRequirementsSection(softwareSystem, new FileInfo(Path.Combine(root.FullName, "quality-requirements.md")));
            AssertSection(softwareSystem, "Quality Requirements", Format.Markdown, "Section 10", 10, section);
    
            section = _template.AddRisksAndTechnicalDebtSection(softwareSystem, new FileInfo(Path.Combine(root.FullName, "risks-and-technical-debt.md")));
            AssertSection(softwareSystem, "Risks and Technical Debt", Format.Markdown, "Section 11", 11, section);
    
            section = _template.AddGlossarySection(softwareSystem, new FileInfo(Path.Combine(root.FullName, "glossary.md")));
            AssertSection(softwareSystem, "Glossary", Format.Markdown, "Section 12", 12, section);
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