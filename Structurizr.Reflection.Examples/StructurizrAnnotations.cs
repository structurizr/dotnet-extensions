using Structurizr.Analysis;
using Structurizr.Api;

namespace Structurizr.Examples
{

    /// <summary>
    /// A small example that illustrates how to use the Structurizr annotations
    /// in conjunction with the StructurizrAnnotationsComponentFinderStrategy.
    /// 
    /// The live workspace is available to view at https://structurizr.com/share/38341
    /// </summary>
    public class StructurizrAnnotations
    {
        private const string DatabaseTag = "Database";

        private const long WorkspaceId = 38341;
        private const string ApiKey = "key";
        private const string ApiSecret = "secret";

        static void Main()
        {
            Workspace workspace = new Workspace("Structurizr for .NET Annotations", "This is a model of my software system.");
            Model model = workspace.Model;

            Person user = model.AddPerson("User", "A user of my software system.");
            SoftwareSystem softwareSystem = model.AddSoftwareSystem("Software System", "My software system.");

            Container webApplication = softwareSystem.AddContainer("Web Application", "Provides users with information.", "C#");
            Container database = softwareSystem.AddContainer("Database", "Stores information.", "Relational database schema");
            database.AddTags(DatabaseTag);

            ComponentFinder componentFinder = new ComponentFinder(
                webApplication,
                "Structurizr.Examples.Annotations",
                new StructurizrAnnotationsComponentFinderStrategy()
            );
            componentFinder.FindComponents();
            model.AddImplicitRelationships();

            ViewSet views = workspace.Views;
            SystemContextView contextView = views.CreateSystemContextView(softwareSystem, "SystemContext", "An example of a System Context diagram.");
            contextView.AddAllElements();

            ContainerView containerView = views.CreateContainerView(softwareSystem, "Containers", "The container diagram from my software system.");
            containerView.AddAllElements();

            ComponentView componentView = views.CreateComponentView(webApplication, "Components", "The component diagram for the web application.");
            componentView.AddAllElements();

            Styles styles = views.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.Element) { Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#1168bd" });
            styles.Add(new ElementStyle(Tags.Container) { Background = "#438dd5" });
            styles.Add(new ElementStyle(Tags.Component) { Background = "#85bbf0", Color = "#000000" });
            styles.Add(new ElementStyle(Tags.Person) { Background = "#08427b", Shape = Shape.Person });
            styles.Add(new ElementStyle(DatabaseTag) { Shape = Shape.Cylinder });

            StructurizrClient structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }
    }
}