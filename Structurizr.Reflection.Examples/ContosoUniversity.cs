using System;
using System.Linq;
using System.Reflection;
using Structurizr.Analysis;
using Structurizr.Api;
using System.IO;

namespace Structurizr.Examples
{
    class ContosoUniversity
    {

        private const string AssemblyLocation = @"C:\Users\simon\ContosoUniversity\ContosoUniversity\bin";

        private const long WorkspaceId = 9581;
        private const string ApiKey = "";
        private const string ApiSecret = "";

        static void Main(string[] args)
        {
            Workspace workspace = new Workspace("Contoso University", "A software architecture model of the Contoso University sample project.");
            Model model = workspace.Model;
            ViewSet views = workspace.Views;
            Styles styles = views.Configuration.Styles;

            Person universityStaff = model.AddPerson("University Staff", "A staff member of the Contoso University.");
            SoftwareSystem contosoUniversity = model.AddSoftwareSystem("Contoso University", "Allows staff to view and update student, course, and instructor information.");
            universityStaff.Uses(contosoUniversity, "uses");

            // if the client-side of this application was richer (e.g. it was a single-page app), I would include the web browser
            // as a container (i.e. User --uses-> Web Browser --uses-> Web Application (backend for frontend) --uses-> Database)
            Container webApplication = contosoUniversity.AddContainer("Web Application", "Allows staff to view and update student, course, and instructor information.", "Microsoft ASP.NET MVC");
            Container database = contosoUniversity.AddContainer("Database", "Stores information about students, courses and instructors", "Microsoft SQL Server Express LocalDB");
            database.AddTags("Database");
            universityStaff.Uses(webApplication, "Uses", "HTTPS");
            webApplication.Uses(database, "Reads from and writes to");

            DirectoryInfo directory = new DirectoryInfo(AssemblyLocation);
            foreach (FileInfo file in directory.EnumerateFiles())
            {
                if (file.Extension == ".dll")
                {
                    Assembly.LoadFrom(file.FullName);
                }
            }

            Type iController = Assembly.LoadFrom(Path.Combine(AssemblyLocation, "System.Web.Mvc.dll")).GetType("System.Web.Mvc.IController");
            Type dbContext = Assembly.LoadFrom(Path.Combine(AssemblyLocation, "EntityFramework.dll")).GetType("System.Data.Entity.DbContext");

            TypeMatcherComponentFinderStrategy typeMatcherComponentFinderStrategy = new TypeMatcherComponentFinderStrategy(
                new InterfaceImplementationTypeMatcher(iController, null, "ASP.NET MVC Controller"),
                new ExtendsClassTypeMatcher(dbContext, null, "Entity Framework DbContext")
            );
            typeMatcherComponentFinderStrategy.AddSupportingTypesStrategy(new ReferencedTypesSupportingTypesStrategy(false));

            ComponentFinder componentFinder = new ComponentFinder(
                webApplication,
                "ContosoUniversity",
                typeMatcherComponentFinderStrategy
                //new TypeSummaryComponentFinderStrategy(@"C:\Users\simon\ContosoUniversity\ContosoUniversity.sln", "ContosoUniversity")
            );
            componentFinder.FindComponents();

            // connect the user to the web MVC controllers
            webApplication.Components.ToList().FindAll(c => c.Technology == "ASP.NET MVC Controller").ForEach(c => universityStaff.Uses(c, "uses"));

            // connect all DbContext components to the database
            webApplication.Components.ToList().FindAll(c => c.Technology == "Entity Framework DbContext").ForEach(c => c.Uses(database, "Reads from and writes to"));

            // link the components to the source code
            foreach (Component component in webApplication.Components)
            {
                foreach (CodeElement codeElement in component.CodeElements)
                {
                    if (codeElement.Url != null)
                    {
                        codeElement.Url = codeElement.Url.Replace(new Uri(@"C:\Users\simon\ContosoUniversity\").AbsoluteUri, "https://github.com/simonbrowndotje/ContosoUniversity/blob/master/");
                        codeElement.Url = codeElement.Url.Replace('\\', '/');
                    }
                }
            }

            // rather than creating a component model for the database, let's simply link to the DDL
            // (this is really just an example of linking an arbitrary element in the model to an external resource)
            database.Url = "https://github.com/simonbrowndotje/ContosoUniversity/tree/master/ContosoUniversity/Migrations";

            SystemContextView contextView = views.CreateSystemContextView(contosoUniversity, "Context", "The system context view for the Contoso University system.");
            contextView.AddAllElements();

            ContainerView containerView = views.CreateContainerView(contosoUniversity, "Containers", "The containers that make up the Contoso University system.");
            containerView.AddAllElements();

            ComponentView componentView = views.CreateComponentView(webApplication, "Components", "The components inside the Contoso University web application.");
            componentView.AddAllElements();

            // create an example dynamic view for a feature
            DynamicView dynamicView = views.CreateDynamicView(webApplication, "GetCoursesForDepartment", "A summary of the \"get courses for department\" feature.");
            Component courseController = webApplication.GetComponentWithName("CourseController");
            Component schoolContext = webApplication.GetComponentWithName("SchoolContext");
            dynamicView.Add(universityStaff, "Requests the list of courses from", courseController);
            dynamicView.Add(courseController, "Uses", schoolContext);
            dynamicView.Add(schoolContext, "Gets a list of courses from", database);

            // add some styling
            styles.Add(new ElementStyle(Tags.Person) { Background = "#0d4d4d", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#003333", Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.Container) { Background = "#226666", Color = "#ffffff" });
            styles.Add(new ElementStyle("Database") { Shape = Shape.Cylinder });
            styles.Add(new ElementStyle(Tags.Component) { Background = "#407f7f", Color = "#ffffff" });

            StructurizrClient structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }

    }
}
