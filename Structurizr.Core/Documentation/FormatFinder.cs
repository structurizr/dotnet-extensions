using System;
using System.Collections.Generic;
using System.IO;

namespace Structurizr.Documentation
{
    
    internal class FormatFinder
    {
        
        private static ISet<string> MARKDOWN_EXTENSIONS = new HashSet<string>
        {
            ".md", ".markdown", ".text"
        };

        private static ISet<string> ASCIIDOC_EXTENSIONS = new HashSet<string>
        {
            ".asciidoc", ".adoc", ".asc"
        };

        internal static Format FindFormat(FileSystemInfo file) {
            if (file == null) {
                throw new ArgumentException("A file must be specified.");
            }

            if (MARKDOWN_EXTENSIONS.Contains(file.Extension)) {
                return Format.Markdown;
            } else if (ASCIIDOC_EXTENSIONS.Contains(file.Extension)) {
                return Format.AsciiDoc;
            } else {
                // just assume Markdown
                return Format.Markdown;
            }

        }
        
    }
    
}