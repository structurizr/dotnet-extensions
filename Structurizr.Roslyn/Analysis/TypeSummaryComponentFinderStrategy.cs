using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Xml.Linq;

namespace Structurizr.Analysis
{

    /// <summary>
    /// This is a component finder that enriches existing components with the type level summary
    /// comment (i.e. this comment). It uses Roslyn and needs access to the source code.
    /// </summary>
    public class TypeSummaryComponentFinderStrategy : ComponentFinderStrategy
    {

        private ComponentFinder _componentFinder;
        /// <inheritdoc />
        public ComponentFinder ComponentFinder
        {
            get { return _componentFinder; }
            set
            {
                _componentFinder = value;
            }
        }

        private HashSet<Component> _components = new HashSet<Component>();

        /// <summary>
        /// Gets or sets the path to the solution (.sln) file containing the project to analyze.
        /// </summary>
        /// <value>The file path of the Visual Studio solution file to use.</value>
        public string PathToSolution { get; set; }

        /// <summary>
        /// Gets or sets the name of the project within the solution to analyze.
        /// </summary>
        /// <value>The name of the project to analyze.</value>
        public string ProjectName { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="TypeSummaryComponentFinderStrategy"/> for a specified project within a
        /// specified Visual Studio solution.
        /// </summary>
        /// <param name="pathToSolution"></param>
        /// <param name="projectName"></param>
        public TypeSummaryComponentFinderStrategy(string pathToSolution, string projectName)
        {
            this.PathToSolution = pathToSolution;
            this.ProjectName = projectName;
        }

        /// <inheritdoc />
        public void BeforeFindComponents()
        {
        }

        /// <inheritdoc />
        public IEnumerable<Component> FindComponents()
        {
            MSBuildWorkspace msWorkspace = MSBuildWorkspace.Create();

            Solution solution = msWorkspace.OpenSolutionAsync(PathToSolution).Result;
            Project project = solution.Projects.Where(p => p.Name == ProjectName).First();
            Compilation compilation = project.GetCompilationAsync().Result;
            foreach (Component component in ComponentFinder.Container.Components)
            {
                foreach (CodeElement codeElement in component.CodeElements)
                {
                    try
                    {
                        string type = codeElement.Type.Substring(0, component.Type.IndexOf(',')); // remove the assembly name from the type
                        INamedTypeSymbol symbol = compilation.GetTypeByMetadataName(type);
                        foreach (Microsoft.CodeAnalysis.Location location in symbol.Locations)
                        {
                            if (location.IsInSource)
                            {
                                codeElement.Url = new Uri(location.SourceTree.FilePath).AbsoluteUri;
                                codeElement.Size += location.SourceTree.GetText().Lines.Count;
                                component.Size += codeElement.Size;
                            }
                        }

                        if (codeElement.Role == CodeElementRole.Primary)
                        {
                            string xml = symbol.GetDocumentationCommentXml();
                            if (xml != null && xml.Trim().Length > 0)
                            {
                                XDocument xdoc = XDocument.Parse(xml);
                                string comment = xdoc.Descendants("summary").First().Value.Trim();
                                component.Description = comment;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Could not get summary comment for " + component.Name + ": " + e.Message);
                    }
                }
            }

            return new HashSet<Component>();
        }

        /// <inheritdoc />
        public void AfterFindComponents()
        {
        }

    }
}
