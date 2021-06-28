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

            //
            // Mark containers or components as database or via tags
            //
            Container webApplication = softwareSystem.AddContainer("Web Application", "Delivers content", "Java and spring MVC");
            // Additional tag element
            webApplication.Tags = "Single Page App";

            Container database = softwareSystem.AddContainer("Database", "Stores information", "Relational Database Schema");
            // Additional mark it as database
            database.SetIsDatabase(true);
            
            var httpCall = user.Uses(webApplication, "uses", "HTTP");
            // Additional tag relationship
            httpCall.Tags = "via firewall";

            webApplication.Uses(database, "Reads from and writes to", "JDBC").SetDirection(DirectionValues.Right);

            // add corresponding styles
            var styles = views.Configuration.Styles;
            styles.Add(new ElementStyle("Single Page App") {Background = "#5F9061", Stroke = "#2E4F2E", Color = "#FFFFFF", Shape = Shape.RoundedBox }); // rounded box is supported with next version see below
            styles.Add(new RelationshipStyle("via firewall") {Color = "#B40404", Dashed  = true }); // dashed is supported with next version see below

            var containerView = views.CreateContainerView(softwareSystem, "containers", "");
            containerView.AddAllElements();

            using (var stringWriter = new StringWriter())
            {
                var plantUmlWriter = new C4PlantUmlWriter();
                plantUmlWriter.Write(containerView, workspace.Views.Configuration, stringWriter);
                Console.WriteLine(stringWriter.ToString());
            }

            //
            // Use features of the next planned C4-PlantUML version (v2.3.0 ?)
            //
            using (var stringWriter = new StringWriter())
            {
                var plantUmlWriter = new C4PlantUmlWriter();
                plantUmlWriter.EnableNextFeatures = true;
                plantUmlWriter.CustomBaseUrl = "https://raw.githubusercontent.com/kirchsth/C4-PlantUML/extended/";

                plantUmlWriter.Write(containerView, workspace.Views.Configuration, stringWriter);
                Console.WriteLine(stringWriter.ToString());
            }
        }
    }
}