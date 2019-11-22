using System;
using System.IO;
using Structurizr.IO.PlantUML;

namespace Structurizr.Examples
{
    
    /// <summary>
    /// An example of how to use the PlantUML writer. Run this program and copy/paste
    /// the output into http://www.plantuml.com/plantuml/
    /// </summary>
    public class PlantUML
    {

        static void Main()
        {
            Workspace workspace = new Workspace("Getting Started", "This is a model of my software system.");
            Model model = workspace.Model;

            Person user = model.AddPerson("User", "A user of my software system.");
            SoftwareSystem softwareSystem = model.AddSoftwareSystem("Software System", "My software system.");
            user.Uses(softwareSystem, "Uses");

            ViewSet views = workspace.Views;
            SystemContextView contextView = views.CreateSystemContextView(softwareSystem, "SystemContext", "An example of a System Context diagram.");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            StringWriter stringWriter = new StringWriter();
            PlantUMLWriter plantUMLWriter = new PlantUMLWriter();
            plantUMLWriter.Write(workspace, stringWriter);
            Console.WriteLine(stringWriter.ToString());
        }

    }
    
}