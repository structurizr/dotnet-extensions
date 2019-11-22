using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Structurizr.IO.PlantUML
{

    /// <summary>
    ///  A simple PlantUML writer that outputs diagram definitions that can be copy-pasted
    /// into http://plantuml.com/plantuml/ ... it supports enterprise context, system context,
    /// container, component and dynamic diagrams.
    /// 
    /// Note: This won't work if you have two elements named the same on a diagram.
    /// </summary>
    public class PlantUMLWriter
    {

        public void Write(Workspace workspace, TextWriter writer)
        {
            if (workspace != null && writer != null)
            {
                workspace.Views.SystemLandscapeViews.ToList().ForEach(v => Write(v, writer));
                workspace.Views.SystemContextViews.ToList().ForEach(v => Write(v, writer));
                workspace.Views.ContainerViews.ToList().ForEach(v => Write(v, writer));
                workspace.Views.ComponentViews.ToList().ForEach(v => Write(v, writer));
                workspace.Views.DynamicViews.ToList().ForEach(v => Write(v as DynamicView, writer));
                workspace.Views.DeploymentViews.ToList().ForEach(v => Write(v as DeploymentView, writer));
            }
        }

        public void Write(SystemLandscapeView view, TextWriter writer)
        {
            if (view == null)
            {
                throw new ArgumentException("A system landscape view must be specified.");    
            }
            
            try
            {
                WriteHeader(view, writer);

                view.Elements
                    .Select(ev => ev.Element)
                    .Where(e => e is Person && ((Person) e).Location == Location.External)
                    .OrderBy(e => e.Name).ToList()
                    .ForEach(e => Write(e, writer, false));

                view.Elements
                    .Select(ev => ev.Element)
                    .Where(e => e is SoftwareSystem && ((SoftwareSystem) e).Location == Location.External)
                    .OrderBy(e => e.Name).ToList()
                    .ForEach(e => Write(e, writer, false));

                string name = view.Model.Enterprise != null ? view.Model.Enterprise.Name : "Enterprise";
                writer.WriteLine("package \"" + name + "\" {");

                view.Elements
                    .Select(ev => ev.Element)
                    .Where(e => e is Person && ((Person) e).Location == Location.Internal)
                    .OrderBy(e => e.Name).ToList()
                    .ForEach(e => Write(e, writer, true));

                view.Elements
                    .Select(ev => ev.Element)
                    .Where(e => e is SoftwareSystem && ((SoftwareSystem) e).Location == Location.Internal)
                    .OrderBy(e => e.Name).ToList()
                    .ForEach(e => Write(e, writer, true));

                writer.WriteLine("}");

                Write(view.Relationships, writer);

                WriteFooter(writer);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void Write(SystemContextView view, TextWriter writer)
        {
            if (view == null)
            {
                throw new ArgumentException("A system context view must be specified.");    
            }
            
            try
            {
                WriteHeader(view, writer);

                view.Elements
                    .Select(ev => ev.Element)
                    .OrderBy(e => e.Name).ToList()
                    .ForEach(e => Write(e, writer, false));
                Write(view.Relationships, writer);

                writer.WriteLine("@enduml");
                writer.WriteLine("");
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void Write(ContainerView view, TextWriter writer)
        {
            if (view == null)
            {
                throw new ArgumentException("A container view must be specified.");    
            }
            
            try
            {
                WriteHeader(view, writer);

                view.Elements
                    .Select(ev => ev.Element)
                    .Where(e => !(e is Container))
                    .OrderBy(e => e.Name).ToList()
                    .ForEach(e => Write(e, writer, false));

                writer.WriteLine("package " + NameOf(view.SoftwareSystem) + " {");

                view.Elements
                    .Select(ev => ev.Element)
                    .Where(e => e is Container)
                    .OrderBy(e => e.Name).ToList()
                    .ForEach(e => Write(e, writer, true));

                writer.WriteLine("}");

                Write(view.Relationships, writer);

                WriteFooter(writer);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void Write(ComponentView view, TextWriter writer)
        {
            if (view == null)
            {
                throw new ArgumentException("A component view must be specified.");    
            }
            
            try
            {
                WriteHeader(view, writer);

                view.Elements
                    .Select(ev => ev.Element)
                    .Where(e => !(e is Component))
                    .OrderBy(e => e.Name).ToList()
                    .ForEach(e => Write(e, writer, false));

                writer.WriteLine("package " + NameOf(view.Container) + " {");

                view.Elements
                    .Select(ev => ev.Element)
                    .Where(e => e is Component)
                    .OrderBy(e => e.Name).ToList()
                    .ForEach(e => Write(e, writer, true));

                writer.WriteLine("}");

                Write(view.Relationships, writer);

                WriteFooter(writer);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void Write(DynamicView view, TextWriter writer)
        {
            if (view == null)
            {
                throw new ArgumentException("A dynamic view must be specified.");    
            }
            
            try
            {
                WriteHeader(view, writer);

                view.Elements
                    .Select(ev => ev.Element)
                    .OrderBy(e => e.Name).ToList()
                    .ForEach(e => Write(e, writer, false));

                view.Relationships
                    .OrderBy(rv => rv.Order).ToList()
                    .ForEach(r =>
                        writer.WriteLine(
                            String.Format("{0} -> {1} : {2}",
                                r.Relationship.Source.Id,
                                r.Relationship.Destination.Id,
                                HasValue(r.Description)
                                    ? r.Description
                                    : HasValue(r.Relationship.Description)
                                        ? r.Relationship.Description
                                        : ""
                            )
                        ));

                WriteFooter(writer);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void Write(DeploymentView view, TextWriter writer)
        {
            if (view == null)
            {
                throw new ArgumentException("A deployment view must be specified.");    
            }
            
            try
            {
                WriteHeader(view, writer);

                view.Elements
                    .Where(ev => ev.Element is DeploymentNode && ev.Element.Parent == null)
                    .Select(ev => ev.Element as DeploymentNode)
                    .OrderBy(e => e.Name).ToList()
                    .ForEach(e => Write(e, writer, 0));

                Write(view.Relationships, writer);

                WriteFooter(writer);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void Write(DeploymentNode deploymentNode, TextWriter writer, int indent)
        {
            writer.Write(
                String.Format("{0}node \"{1}\" <<{2}>> as {3} {{",
                    CalculateIndent(indent),
                    deploymentNode.Name + (deploymentNode.Instances > 1 ? " (x" + deploymentNode.Instances + ")" : ""),
                    TypeOf(deploymentNode),
                    deploymentNode.Id
                )
            );

            writer.Write(Environment.NewLine);

            foreach (DeploymentNode child in deploymentNode.Children) {
                Write(child, writer, indent + 1);
            }

            foreach (ContainerInstance containerInstance in deploymentNode.ContainerInstances) {
                Write(containerInstance, writer, indent + 1);
            }

            writer.Write(
                String.Format("{0}}}", CalculateIndent(indent))
            );
            writer.Write(Environment.NewLine);
        }

        private void Write(ContainerInstance containerInstance, TextWriter writer, int indent)
        {
            writer.Write(
                String.Format("{0}artifact \"{1}\" <<{2}>> as {3}",
                    CalculateIndent(indent),
                    containerInstance.Container.Name,
                    TypeOf(containerInstance),
                    containerInstance.Id
                )
            );

            writer.Write(Environment.NewLine);
        }

        private string CalculateIndent(int indent)
        {
            StringBuilder buf = new StringBuilder();

            for (int i = 0; i < indent; i++)
            {
                buf.Append("  ");
            }

            return buf.ToString();
        }

        private void Write(Element element, TextWriter writer, bool indent)
        {
            try
            {
                writer.WriteLine(
                    String.Format("{0}{1} \"{2}\" <<{3}>> as {4}",
                        indent ? "  " : "",
                        element is Person ? "actor" : "component",
                        element.Name,
                        TypeOf(element),
                        element.Id
                    )
                );
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void Write(ISet<RelationshipView> relationships, TextWriter writer)
        {
            relationships
                .Select(rv => rv.Relationship)
                .OrderBy(r => r.Source.Name + r.Destination.Name).ToList()
                .ForEach(r => Write(r, writer));
        }

        private void Write(Relationship relationship, TextWriter writer)
        {
            try
            {
                // Write the relationship
                writer.Write(
                    String.Format("{0} ..> {1}",
                        relationship.Source.Id,
                        relationship.Destination.Id)
                );
                // Check if the relationship needs a label
                if (HasValue(relationship.Description) || HasValue(relationship.Technology))
                {
                    writer.Write(
                        String.Format(" :{0}{1}",
                            HasValue(relationship.Description) ? " " + relationship.Description : "",
                            HasValue(relationship.Technology) ? " <<" + relationship.Technology + ">>" : "")
                    );
                }
                // Add a newline
                writer.WriteLine("");
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private string NameOf(Element e)
        {
            return NameOf(e.Name);
        }

        private string NameOf(string s)
        {
            if (s != null)
            {
                return s.Replace(" ", "")
                    .Replace("-", "");
            }
            else
            {
                return "";
            }
        }

        private string TypeOf(Element e)
        {
            if (e is Person)
            {
                return "Person";
            }

            if (e is SoftwareSystem)
            {
                return "Software System";
            }

            if (e is Container)
            {
                return "Container";
            }

            if (e is Component)
            {
                Component component = (Component) e;
                return HasValue(component.Technology) ? component.Technology : "Component";
            }

            if (e is DeploymentNode)
            {
                DeploymentNode deploymentNode = (DeploymentNode) e;
                return HasValue(deploymentNode.Technology) ? deploymentNode.Technology : "Deployment Node";
            }

            if (e is ContainerInstance)
            {
                return "Container";
            }

            return "";
        }

        private bool HasValue(string s)
        {
            return s != null && s.Trim().Length > 0;
        }

        private void WriteHeader(View view, TextWriter writer)
        {
            writer.Write("@startuml");
            writer.Write(Environment.NewLine);

            writer.Write("title " + view.Name);
            writer.Write(Environment.NewLine);
    
            if (!String.IsNullOrEmpty(view.Description))
            {
                writer.Write("caption " + view.Description);
                writer.Write(Environment.NewLine);
            }
        }

        private void WriteFooter(TextWriter writer)
        {
            writer.Write("@enduml");
            writer.Write(Environment.NewLine);
            writer.Write(Environment.NewLine);
        }

    }

}