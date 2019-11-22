using System;
using System.IO;
using System.Linq;
using Structurizr.IO.C4PlantUML;
using Structurizr.IO.C4PlantUML.ModelExtensions;

namespace Structurizr.Examples
{
    /// <summary>
    /// An example of how to use the C4PlantUML writer. Run this program and copy/paste
    /// the output into http://www.plantuml.com/plantuml/
    /// </summary>
    public class C4PlantUML
    {
        static void Main()
        {
            Workspace workspace = new Workspace("Getting Started", "This is a model of my software system.");
            Model model = workspace.Model;

            model.Enterprise = new Enterprise("Some Enterprise");
            
            Person user = model.AddPerson("User", "A user of my software system.");
            SoftwareSystem softwareSystem = model.AddSoftwareSystem("Software System", "My software system.");
            var userUsesSystemRelation = user.Uses(softwareSystem, "Uses");
            // a direction could be added to relation (active in all views)
            // userUsesSystemRelation.SetDirection(DirectionValues.Right);

            ViewSet views = workspace.Views;
            SystemContextView contextView = views.CreateSystemContextView(softwareSystem, "SystemContext", "An example of a System Context diagram.");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // C4PlantUMLWriter support view specific directions too, e.g. "User" should be left of "Software System" only in this view
            contextView.Relationships
                .First(rv => rv.Relationship.SourceId == user.Id && rv.Relationship.DestinationId == softwareSystem.Id)
                .SetDirection(DirectionValues.Right);

            using (var stringWriter = new StringWriter())
            {
                var plantUmlWriter = new C4PlantUmlWriter();
                plantUmlWriter.Write(workspace, stringWriter);
                Console.WriteLine(stringWriter.ToString());
            }


            Container webApplication = softwareSystem.AddContainer("Web Application", "Delivers content", "Java and spring MVC");
            Container database = softwareSystem.AddContainer("Database", "Stores information", "Relational Database Schema");
            // Additional mark it as database
            database.SetIsDatabase(true);
            user.Uses(webApplication, "uses", "HTTP");
            webApplication.Uses(database, "Reads from and writes to", "JDBC").SetDirection(DirectionValues.Right);

            var containerView = views.CreateContainerView(softwareSystem, "containers", "");
            containerView.AddAllElements();

            using (var stringWriter = new StringWriter())
            {
                var plantUmlWriter = new C4PlantUmlWriter();
                plantUmlWriter.Write(containerView, stringWriter);
                Console.WriteLine(stringWriter.ToString());
            }
        }
    }
}