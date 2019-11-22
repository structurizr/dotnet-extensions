using System.IO;
using Structurizr.AdrTools;
using Structurizr.Api;

namespace Structurizr.Examples
{

    /// <summary>
    /// An example of how to import architecture decision records from Nat Pryce's adr-tools tool.
    /// 
    /// The live workspace is available to view at https://structurizr.com/share/39459
    /// </summary>
    class AdrTools
    {

        private const long WorkspaceId = 39459;
        private const string ApiKey = "key";
        private const string ApiSecret = "secret";

        private const string FileSystemTag = "File System";

        static void Main()
        {
            Workspace workspace = new Workspace("adr-tools", "A description of the adr-tools command line utility.");
            Model model = workspace.Model;

            Person user = model.AddPerson("User", "Somebody on a software development team.");
            SoftwareSystem adrTools = model.AddSoftwareSystem("adr-tools", "A command-line tool for working with Architecture Decision Records (ADRs).");
            adrTools.Url ="https://github.com/npryce/adr-tools";

            Container adrShellScripts = adrTools.AddContainer("adr", "A command-line tool for working with Architecture Decision Records (ADRs).", "Shell Scripts");
            adrShellScripts.Url = "https://github.com/npryce/adr-tools/tree/master/src";
            Container fileSystem = adrTools.AddContainer("File System", "Stores ADRs, templates, etc.", "File System");
            fileSystem.AddTags(FileSystemTag);
            user.Uses(adrShellScripts, "Manages ADRs using");
            adrShellScripts.Uses(fileSystem, "Reads from and writes to");
            model.AddImplicitRelationships();

            ViewSet views = workspace.Views;
            SystemContextView contextView = views.CreateSystemContextView(adrTools, "SystemContext", "The system context diagram for adr-tools.");
            contextView.AddAllElements();

            ContainerView containerView = views.CreateContainerView(adrTools, "Containers", "The container diagram for adr-tools.");
            containerView.AddAllElements();

            DirectoryInfo adrDirectory = new DirectoryInfo("Documentation" + Path.DirectorySeparatorChar + "adr");

            AdrToolsImporter adrToolsImporter = new AdrToolsImporter(workspace, adrDirectory);
            adrToolsImporter.ImportArchitectureDecisionRecords(adrTools);

            Styles styles = views.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.Element) { Shape = Shape.RoundedBox, Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#18ADAD", Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.Person) { Shape = Shape.Person, Background = "#008282", Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.Container) { Background = "#6DBFBF" });
            styles.Add(new ElementStyle(FileSystemTag) { Shape = Shape.Folder });

            StructurizrClient structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }

    }
}