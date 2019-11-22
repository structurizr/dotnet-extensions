using System;
using System.IO;
using System.Linq;
using Structurizr.Documentation;
using Xunit;

namespace Structurizr.AdrTools.Tests
{

    public class AdrToolsImporterTests
    {

        private Workspace _workspace;
        private SoftwareSystem _softwareSystem;
        private Documentation.Documentation _documentation;

        public AdrToolsImporterTests()
        { 
            _workspace = new Workspace("Name", "Description");
            _softwareSystem = _workspace.Model.AddSoftwareSystem("Software System", "Description");
            _documentation = _workspace.Documentation;
        }

        [Fact]
        public void Test_Construction_ThrowsAnException_WhenAWorkspaceIsNotSpecified()
        {
            try
            {
                new AdrToolsImporter(null, null);
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("A workspace must be specified.", iae.Message);
            }
        }

        [Fact]
        public void Test_Construction_ThrowsAnException_WhenADirectoryIsNotSpecified()
        {
            try
            {
                new AdrToolsImporter(_workspace, null);
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("The path to the architecture decision records must be specified.", iae.Message);
            }
        }

        [Fact]
        public void Test_Construction_ThrowsAnException_WhenADirectoryIsSpecifiedButItDoesNotExist()
        {
            try
            {
                new AdrToolsImporter(_workspace, new DirectoryInfo("some-random-path"));
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.True(iae.Message.EndsWith("some-random-path does not exist."));
            }
        }

        [Fact]
        public void Test_ImportArchitectureDecisionRecords()
        {
            AdrToolsImporter importer = new AdrToolsImporter(_workspace, new DirectoryInfo("AdrTools\\adrs"));
            importer.ImportArchitectureDecisionRecords();

            Assert.Equal(10, _documentation.Decisions.Count);

            Decision decision1 = _documentation.Decisions.Where(d => d.Id == "1").First();
            Assert.Equal("1", decision1.Id);
            Assert.Equal("Record architecture decisions", decision1.Title);
            Assert.Equal("2016-02-12T00:00:00.0000000", decision1.Date.ToString("o"));
            Assert.Equal(DecisionStatus.Accepted, decision1.Status);
            Assert.Equal(Format.Markdown, decision1.Format);
            Assert.Equal("# 1. Record architecture decisions\n" +
                "\n" +
                "Date: 2016-02-12\n" +
                "\n" +
                "## Status\n" +
                "\n" +
                "Accepted\n" +
                "\n" +
                "## Context\n" +
                "\n" +
                "We need to record the architectural decisions made on this project.\n" +
                "\n" +
                "## Decision\n" +
                "\n" +
                "We will use Architecture Decision Records, as described by Michael Nygard in this article: http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions\n" +
                "\n" +
                "## Consequences\n" +
                "\n" +
                "See Michael Nygard's article, linked above.\n",
            decision1.Content);
        }

        [Fact]
        public void Test_ImportArchitectureDecisionRecords_RewritesLinksBetweenADRsWhenTheyAreNotAssociatedWithAnElement()
        {
            AdrToolsImporter importer = new AdrToolsImporter(_workspace, new DirectoryInfo("AdrTools\\adrs"));
            importer.ImportArchitectureDecisionRecords();

            Decision decision5 = _documentation.Decisions.Where(d => d.Id == "5").First();
            Assert.True(decision5.Content.Contains("Amended by [9. Help scripts](#/:9)"));
        }

        [Fact]
        public void Test_TmportArchitectureDecisionRecords_RewritesLinksBetweenADRsWhenTheyAreAssociatedWithAnElement()
        {
            AdrToolsImporter importer = new AdrToolsImporter(_workspace, new DirectoryInfo("AdrTools\\adrs"));
            importer.ImportArchitectureDecisionRecords(_softwareSystem);

            Decision decision5 = _documentation.Decisions.Where(d => d.Id == "5").First();
            Assert.True(decision5.Content.Contains("Amended by [9. Help scripts](#%2FSoftware%20System:9)"));
        }

        [Fact]
        public void Test_ImportArchitectureDecisionRecords_SupportsTheIncorrectSpellingOfSuperseded()
        {
            AdrToolsImporter importer = new AdrToolsImporter(_workspace, new DirectoryInfo("AdrTools\\adrs"));
            importer.ImportArchitectureDecisionRecords();

            Decision decision4 = _documentation.Decisions.Where(d => d.Id == "4").First();
            Assert.Equal(DecisionStatus.Superseded, decision4.Status);
            Assert.True(decision4.Content.Contains("Superceded by [10. AsciiDoc format](#/:10)"));
        }

    }

}
