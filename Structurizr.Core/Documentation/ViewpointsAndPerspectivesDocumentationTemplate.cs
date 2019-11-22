using System.IO;

namespace Structurizr.Documentation
{
    
    /// <summary>
    ///
    /// An implementation of the "Viewpoints and Perspectives" documentation template
    /// (http://www.viewpoints-and-perspectives.info),
    /// from the "Software Systems Architecture" book by Nick Rozanski and Eoin Woods,
    /// consisting of the following sections:
    ///
    ///  - Introduction (1)
    ///  - Glossary (1)
    ///  - System Stakeholders and Requirements (2)
    ///  - Architectural Forces (2)
    ///  - Architectural Views (3)
    ///  - System Qualities (4)
    ///  - Appendices (5)
    ///
    /// The number in parentheses () represents the grouping, which is simply used to colour code
    /// section navigation buttons when rendered.
    /// </summary>
    public class ViewpointsAndPerspectivesDocumentation : DocumentationTemplate
    {
        
        public ViewpointsAndPerspectivesDocumentation(Workspace workspace) : base(workspace)
        {
        }
        
        /// <summary>
        /// Adds an "Introduction" section relating to a SoftwareSystem from a file.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem the documentation relates to</param>
        /// <param name="files">one or more FileSystemInfo objects that point to the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddIntroductionSection(SoftwareSystem softwareSystem, params FileSystemInfo[] files)
        {
            return AddSection(softwareSystem, "Introduction", files);
        }

        /// <summary>
        /// Adds an "Introduction" section relating to a SoftwareSystem.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem the documentation relates to</param>
        /// <param name="format">the format of the documentation content</param>
        /// <param name="content">a string containing the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddIntroductionSection(SoftwareSystem softwareSystem, Format format, string content)
        {
            return AddSection(softwareSystem, "Introduction", format, content);
        }
        
        /// <summary>
        /// Adds a "Glossary" section relating to a SoftwareSystem from a file.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem the documentation relates to</param>
        /// <param name="files">one or more FileSystemInfo objects that point to the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddGlossarySection(SoftwareSystem softwareSystem, params FileSystemInfo[] files)
        {
            return AddSection(softwareSystem, "Glossary", files);
        }

        /// <summary>
        /// Adds a "Glossary" section relating to a SoftwareSystem.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem the documentation relates to</param>
        /// <param name="format">the format of the documentation content</param>
        /// <param name="content">a string containing the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddGlossarySection(SoftwareSystem softwareSystem, Format format, string content)
        {
            return AddSection(softwareSystem, "Glossary", format, content);
        }
        
        /// <summary>
        /// Adds a "System Stakeholders and Requirements" section relating to a SoftwareSystem from a file.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem the documentation relates to</param>
        /// <param name="files">one or more FileSystemInfo objects that point to the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddSystemStakeholdersAndRequirementsSection(SoftwareSystem softwareSystem, params FileSystemInfo[] files)
        {
            return AddSection(softwareSystem, "System Stakeholders and Requirements", files);
        }

        /// <summary>
        /// Adds a "System Stakeholders and Requirements" section relating to a SoftwareSystem.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem the documentation relates to</param>
        /// <param name="format">the format of the documentation content</param>
        /// <param name="content">a string containing the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddSystemStakeholdersAndRequirementsSection(SoftwareSystem softwareSystem, Format format, string content)
        {
            return AddSection(softwareSystem, "System Stakeholders and Requirements", format, content);
        }
        
        /// <summary>
        /// Adds an "Architectural Forces" section relating to a SoftwareSystem from a file.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem the documentation relates to</param>
        /// <param name="files">one or more FileSystemInfo objects that point to the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddArchitecturalForcesSection(SoftwareSystem softwareSystem, params FileSystemInfo[] files)
        {
            return AddSection(softwareSystem, "Architectural Forces", files);
        }

        /// <summary>
        /// Adds an "Architectural Forces" section relating to a SoftwareSystem.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem the documentation relates to</param>
        /// <param name="format">the format of the documentation content</param>
        /// <param name="content">a string containing the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddArchitecturalForcesSection(SoftwareSystem softwareSystem, Format format, string content)
        {
            return AddSection(softwareSystem, "Architectural Forces", format, content);
        }
        
        /// <summary>
        /// Adds an "Architectural Views" section relating to a SoftwareSystem from a file.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem the documentation relates to</param>
        /// <param name="files">one or more FileSystemInfo objects that point to the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddArchitecturalViewsSection(SoftwareSystem softwareSystem, params FileSystemInfo[] files)
        {
            return AddSection(softwareSystem, "Architectural Views", files);
        }

        /// <summary>
        /// Adds an "Architectural Views" section relating to a SoftwareSystem.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem the documentation relates to</param>
        /// <param name="format">the format of the documentation content</param>
        /// <param name="content">a string containing the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddArchitecturalViewsSection(SoftwareSystem softwareSystem, Format format, string content)
        {
            return AddSection(softwareSystem, "Architectural Views", format, content);
        }
        
        /// <summary>
        /// Adds a "System Qualities" section relating to a SoftwareSystem from a file.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem the documentation relates to</param>
        /// <param name="files">one or more FileSystemInfo objects that point to the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddSystemQualitiesSection(SoftwareSystem softwareSystem, params FileSystemInfo[] files)
        {
            return AddSection(softwareSystem, "System Qualities", files);
        }

        /// <summary>
        /// Adds a "System Qualities" section relating to a SoftwareSystem.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem the documentation relates to</param>
        /// <param name="format">the format of the documentation content</param>
        /// <param name="content">a string containing the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddSystemQualitiesSection(SoftwareSystem softwareSystem, Format format, string content)
        {
            return AddSection(softwareSystem, "System Qualities", format, content);
        }
        
        /// <summary>
        /// Adds a "Appendices" section relating to a SoftwareSystem from a file.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem the documentation relates to</param>
        /// <param name="files">one or more FileSystemInfo objects that point to the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddAppendicesSection(SoftwareSystem softwareSystem, params FileSystemInfo[] files)
        {
            return AddSection(softwareSystem, "Appendices", files);
        }

        /// <summary>
        /// Adds a "Appendices" section relating to a SoftwareSystem.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem the documentation relates to</param>
        /// <param name="format">the format of the documentation content</param>
        /// <param name="content">a string containing the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddAppendicesSection(SoftwareSystem softwareSystem, Format format, string content)
        {
            return AddSection(softwareSystem, "Appendices", format, content);
        }
        
    }
}