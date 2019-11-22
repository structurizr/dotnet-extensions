namespace Structurizr.ActiveDirectory
{
    class Program
    {

        private const long WorkspaceId = 1;
        private const string ApiUrl = "";
        private const string ApiKey = "";
        private const string ApiSecret = "";

        private const string AzureActiveDirectoryInstance = "https://login.windows.net/{0}";
        private const string AzureActiveDirectoryTenant = ""; // e.g. something.onmicrosoft.com
        private const string AzureActiveDirectoryClientId = ""; // the "Application ID" of the Structurizr client application
        private const string AzureActiveDirectoryClientKey = ""; // a key for the Structurizr client application
        private const string AzureActiveDirectoryResourceId = ""; // the "Application ID" of the Structurizr on-premises web application

        static void Main()
        {
            Workspace workspace = new Workspace("Getting Started", "This is a model of my software system.");
            Model model = workspace.Model;

            Person user = model.AddPerson("User", "A user of my software system.");
            SoftwareSystem softwareSystem = model.AddSoftwareSystem("Software System", "My software system.");
            user.Uses(softwareSystem, "Uses");

            ViewSet viewSet = workspace.Views;
            SystemContextView contextView = viewSet.CreateSystemContextView(softwareSystem, "SystemContext", "An example of a System Context diagram.");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#1168bd", Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.Person) { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });

            AzureActiveDirectoryStructurizrClient structurizrClient = new AzureActiveDirectoryStructurizrClient(
                ApiUrl, ApiKey, ApiSecret,
                AzureActiveDirectoryInstance, AzureActiveDirectoryTenant, AzureActiveDirectoryClientId, AzureActiveDirectoryClientKey, AzureActiveDirectoryResourceId
            );
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }

    }
}