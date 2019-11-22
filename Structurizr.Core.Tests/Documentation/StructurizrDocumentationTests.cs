using System;
using System.IO;
using Structurizr.Documentation;
using Xunit;

namespace Structurizr.Core.Tests.Documentation
{
    public class StructurizrDocumentationTests : AbstractTestBase
    {
        
        private SoftwareSystem _softwareSystem;
        private Container _containerA;
        private Container _containerB;
        private Component _componentA1;
        private Component _componentA2;
        private StructurizrDocumentationTemplate _template;

        public StructurizrDocumentationTests()
        {
            _softwareSystem = Model.AddSoftwareSystem("Name", "Description");
            _containerA = _softwareSystem.AddContainer("Container A", "Description", "Technology");
            _containerB = _softwareSystem.AddContainer("Container B", "Description", "Technology");
            _componentA1 = _containerA.AddComponent("Component A1", "Description", "Technology");
            _componentA2 = _containerA.AddComponent("Component A2", "Description", "Technology");
    
            _template = new StructurizrDocumentationTemplate(Workspace);
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
        public void Test_AddAllSectionsWithContentAsstrings()
        {
            Section section;
    
            section = _template.AddContextSection(_softwareSystem, Format.Markdown, "Context section");
            AssertSection(_softwareSystem, "Context", Format.Markdown, "Context section", 1, section);
    
            section = _template.AddFunctionalOverviewSection(_softwareSystem, Format.Markdown, "Functional overview section");
            AssertSection(_softwareSystem, "Functional Overview", Format.Markdown, "Functional overview section", 2, section);
    
            section = _template.AddQualityAttributesSection(_softwareSystem, Format.Markdown, "Quality attributes section");
            AssertSection(_softwareSystem, "Quality Attributes", Format.Markdown, "Quality attributes section", 3, section);
    
            section = _template.AddConstraintsSection(_softwareSystem, Format.Markdown, "Constraints section");
            AssertSection(_softwareSystem, "Constraints", Format.Markdown, "Constraints section", 4, section);
    
            section = _template.AddPrinciplesSection(_softwareSystem, Format.Markdown, "Principles section");
            AssertSection(_softwareSystem, "Principles", Format.Markdown, "Principles section", 5, section);
    
            section = _template.AddSoftwareArchitectureSection(_softwareSystem, Format.Markdown, "Software architecture section");
            AssertSection(_softwareSystem, "Software Architecture", Format.Markdown, "Software architecture section", 6, section);
    
            section = _template.AddContainersSection(_softwareSystem, Format.Markdown, "Containers section");
            AssertSection(_softwareSystem, "Containers", Format.Markdown, "Containers section", 7, section);
    
            section = _template.AddComponentsSection(_containerA, Format.Markdown, "Components section for container A");
            AssertSection(_containerA, "Components", Format.Markdown, "Components section for container A", 8, section);
    
            section = _template.AddComponentsSection(_containerB, Format.Markdown, "Components section for container B");
            AssertSection(_containerB, "Components", Format.Markdown, "Components section for container B", 9, section);
    
            section = _template.AddCodeSection(_componentA1, Format.Markdown, "Code section for component A1");
            AssertSection(_componentA1, "Code", Format.Markdown, "Code section for component A1", 10, section);
    
            section = _template.AddCodeSection(_componentA2, Format.Markdown, "Code section for component A2");
            AssertSection(_componentA2, "Code", Format.Markdown, "Code section for component A2", 11, section);
    
            section = _template.AddDataSection(_softwareSystem, Format.Markdown, "Data section");
            AssertSection(_softwareSystem, "Data", Format.Markdown, "Data section", 12, section);
    
            section = _template.AddInfrastructureArchitectureSection(_softwareSystem, Format.Markdown, "Infrastructure architecture section");
            AssertSection(_softwareSystem, "Infrastructure Architecture", Format.Markdown, "Infrastructure architecture section", 13, section);
    
            section = _template.AddDeploymentSection(_softwareSystem, Format.Markdown, "Deployment section");
            AssertSection(_softwareSystem, "Deployment", Format.Markdown, "Deployment section", 14, section);
    
            section = _template.AddDevelopmentEnvironmentSection(_softwareSystem, Format.Markdown, "Development environment section");
            AssertSection(_softwareSystem, "Development Environment", Format.Markdown, "Development environment section", 15, section);
    
            section = _template.AddOperationAndSupportSection(_softwareSystem, Format.Markdown, "Operation and support section");
            AssertSection(_softwareSystem, "Operation and Support", Format.Markdown, "Operation and support section", 16, section);
    
            section = _template.AddDecisionLogSection(_softwareSystem, Format.Markdown, "Decision log section");
            AssertSection(_softwareSystem, "Decision Log", Format.Markdown, "Decision log section", 17, section);
        }
    
        [Fact]
        public void Test_AddAllSectionsWithContentFromFiles()
        {
            Section section;
            DirectoryInfo root = new DirectoryInfo("Documentation" + Path.DirectorySeparatorChar + "structurizr");
    
            section = _template.AddContextSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "context.md")));
            AssertSection(_softwareSystem, "Context", Format.Markdown, "Context section", 1, section);
    
            section = _template.AddFunctionalOverviewSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "functional-overview.md")));
            AssertSection(_softwareSystem, "Functional Overview", Format.Markdown, "Functional overview section", 2, section);
    
            section = _template.AddQualityAttributesSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "quality-attributes.md")));
            AssertSection(_softwareSystem, "Quality Attributes", Format.Markdown, "Quality attributes section", 3, section);
    
            section = _template.AddConstraintsSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "constraints.md")));
            AssertSection(_softwareSystem, "Constraints", Format.Markdown, "Constraints section", 4, section);
    
            section = _template.AddPrinciplesSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "principles.md")));
            AssertSection(_softwareSystem, "Principles", Format.Markdown, "Principles section", 5, section);
    
            section = _template.AddSoftwareArchitectureSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "software-architecture.md")));
            AssertSection(_softwareSystem, "Software Architecture", Format.Markdown, "Software architecture section", 6, section);
    
            section = _template.AddContainersSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "containers.md")));
            AssertSection(_softwareSystem, "Containers", Format.Markdown, "Containers section", 7, section);
    
            section = _template.AddComponentsSection(_containerA, new FileInfo(Path.Combine(root.FullName, "components-for-containerA.md")));
            AssertSection(_containerA, "Components", Format.Markdown, "Components section for container A", 8, section);
    
            section = _template.AddComponentsSection(_containerB, new FileInfo(Path.Combine(root.FullName, "components-for-containerB.md")));
            AssertSection(_containerB, "Components", Format.Markdown, "Components section for container B", 9, section);
    
            section = _template.AddCodeSection(_componentA1, new FileInfo(Path.Combine(root.FullName, "code-for-componentA1.md")));
            AssertSection(_componentA1, "Code", Format.Markdown, "Code section for component A1", 10, section);
    
            section = _template.AddCodeSection(_componentA2, new FileInfo(Path.Combine(root.FullName, "code-for-componentA2.md")));
            AssertSection(_componentA2, "Code", Format.Markdown, "Code section for component A2", 11, section);
    
            section = _template.AddDataSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "data.md")));
            AssertSection(_softwareSystem, "Data", Format.Markdown, "Data section", 12, section);
    
            section = _template.AddInfrastructureArchitectureSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "infrastructure-architecture.md")));
            AssertSection(_softwareSystem, "Infrastructure Architecture", Format.Markdown, "Infrastructure architecture section", 13, section);
    
            section = _template.AddDeploymentSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "deployment.md")));
            AssertSection(_softwareSystem, "Deployment", Format.Markdown, "Deployment section", 14, section);
    
            section = _template.AddDevelopmentEnvironmentSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "development-environment.md")));
            AssertSection(_softwareSystem, "Development Environment", Format.Markdown, "Development environment section", 15, section);
    
            section = _template.AddOperationAndSupportSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "operation-and-support.md")));
            AssertSection(_softwareSystem, "Operation and Support", Format.Markdown, "Operation and support section", 16, section);
    
            section = _template.AddDecisionLogSection(_softwareSystem, new FileInfo(Path.Combine(root.FullName, "decision-log.md")));
            AssertSection(_softwareSystem, "Decision Log", Format.Markdown, "Decision log section", 17, section);
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