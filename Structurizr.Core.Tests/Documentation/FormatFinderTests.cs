using System;
using System.IO;
using Structurizr.Documentation;
using Xunit;

namespace Structurizr.Core.Tests.Documentation
{
    
    public class FormatFinderTests
    {
    
        [Fact]
        public void Test_FindFormat_ThrowsAnException_WhenAFileIsNotSpecified() {
            try {
                FormatFinder.FindFormat(null);
                throw new TestFailedException();
            } catch (ArgumentException iae) {
                Assert.Equal("A file must be specified.", iae.Message);
            }
        }

        [Fact]
        public void Test_FindFormat_ReturnsMarkdown_WhenAMarkdownFileIsSpecified() {
            Assert.Equal(Format.Markdown, FormatFinder.FindFormat(new FileInfo("foo.md")));
            Assert.Equal(Format.Markdown, FormatFinder.FindFormat(new FileInfo("foo.markdown")));
            Assert.Equal(Format.Markdown, FormatFinder.FindFormat(new FileInfo("foo.text")));
        }

        [Fact]
        public void Test_FindFormat_ReturnsAsciiDoc_WhenAnAsciiDocFileIsSpecified() {
            Assert.Equal(Format.AsciiDoc, FormatFinder.FindFormat(new FileInfo("foo.adoc")));
            Assert.Equal(Format.AsciiDoc, FormatFinder.FindFormat(new FileInfo("foo.asciidoc")));
            Assert.Equal(Format.AsciiDoc, FormatFinder.FindFormat(new FileInfo("foo.asc")));
        }
        
    }
    
}