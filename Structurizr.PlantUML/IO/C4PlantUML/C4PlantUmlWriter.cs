using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Structurizr.IO.C4PlantUML.ModelExtensions;

// Source base version copied from https://gist.github.com/coldacid/465fa8f3a4cd3fdd7b640a65ad5b86f4 (https://github.com/structurizr/dotnet/issues/47) 
// kirchsth: Extended with dynamic and deployment view
namespace Structurizr.IO.C4PlantUML
{
    public class C4PlantUmlWriter : PlantUMLWriterBase
    {
        public enum LayoutDirection
        {
            TopDown,
            LeftRight
        }

        public bool LayoutWithLegend { get; set; } = true;

        public bool LayoutAsSketch { get; set; } = false;

        public LayoutDirection? Layout { get; set; }

        /// <summary>
        /// PlantUML-stdlib or https://raw.githubusercontent.com/RicardoNiepel/C4-PlantUML/release/1-0/ does not support
        /// dynamic or deployment diagrams. They can be used via the PlantUML-stdlib and in the diagram added definitions
        /// or use a pull-request version which is available at https://raw.githubusercontent.com/kirchsth/C4-PlantUML/extended/
        /// (if the value is empty/null then PlantUML-stdlib with added definitions is used)
        /// </summary>
        public string CustomBaseUrl { get; set; } = ""; // @"https://raw.githubusercontent.com/kirchsth/C4-PlantUML/extended/";

        protected override void Write(SystemLandscapeView view, TextWriter writer)
        {
            var showBoundary = view.EnterpriseBoundaryVisible ?? true;

            WriteProlog(view, writer);

            view.Elements
                .Select(ev => ev.Element)
                .Where(e => e is Person person && person.Location == Location.External)
                .OrderBy(e => e.Name).ToList()
                .ForEach(e => Write(e, writer, 0));

            view.Elements
                .Select(ev => ev.Element)
                .Where(e => e is SoftwareSystem softwareSystem && softwareSystem.Location == Location.External)
                .OrderBy(e => e.Name).ToList()
                .ForEach(e => Write(e, writer, 0));

            if (showBoundary)
            {
                var enterpriseName = view.Model.Enterprise.Name;
                writer.WriteLine($"Enterprise_Boundary({TokenizeName(enterpriseName)}, \"{enterpriseName}\") {{");
            }

            view.Elements
                .Select(ev => ev.Element)
                .Where(e => e is Person person && person.Location == Location.Internal)
                .OrderBy(e => e.Name).ToList()
                .ForEach(e => Write(e, writer, showBoundary ? 1 : 0));

            view.Elements
                .Select(ev => ev.Element)
                .Where(e => e is SoftwareSystem softwareSystem && softwareSystem.Location == Location.Internal)
                .OrderBy(e => e.Name).ToList()
                .ForEach(e => Write(e, writer, showBoundary ? 1 : 0));

            if (showBoundary)
                writer.WriteLine("}");

            Write(view.Relationships, writer);

            WriteEpilog(view, writer);
        }

        protected override void Write(SystemContextView view, TextWriter writer)
        {
            var showBoundary = view.EnterpriseBoundaryVisible ?? true;

            WriteProlog(view, writer);

            if (showBoundary)
            {
                var enterpriseName = view.Model.Enterprise.Name;
                writer.WriteLine($"Enterprise_Boundary({TokenizeName(enterpriseName)}, \"{enterpriseName}\") {{");
            }

            view.Elements
                .Select(ev => ev.Element)
                .OrderBy(e => e.Name).ToList()
                .ForEach(e => Write(e, writer, showBoundary ? 1 : 0));
            Write(view.Relationships, writer);

            if (showBoundary)
                writer.WriteLine("}");

            WriteEpilog(view, writer);
        }

        protected override void Write(ContainerView view, TextWriter writer)
        {
            var externals = view.Elements
                .Select(ev => ev.Element)
                .Where(e => !(e is Container));
            var showBoundary = externals.Any();

            WriteProlog(view, writer);

            externals
                .OrderBy(e => e.Name).ToList()
                .ForEach(e => Write(e, writer, 0));

            if (showBoundary)
                Write(view.SoftwareSystem, writer, 0, true);

            view.Elements
                .Select(ev => ev.Element)
                .Where(e => e is Container)
                .OrderBy(e => e.Name).ToList()
                .ForEach(e => Write(e, writer, showBoundary ? 1 : 0));

            if (showBoundary)
                writer.WriteLine("}");

            Write(view.Relationships, writer);

            WriteEpilog(view, writer);
        }

        protected override void Write(ComponentView view, TextWriter writer)
        {
            var nonComponents = view.Elements
                .Select(ev => ev.Element)
                .Where(e => !(e is Component));
            var nonContainedComponents =
                from ev in view.Elements
                let e = ev.Element
                where e is Component && e.Parent?.Id != view.Container.Id
                group e by e.Parent;
            var showBoundary = nonComponents.Any() || nonContainedComponents.Any();

            WriteProlog(view, writer);

            nonComponents
                .OrderBy(e => e.Name).ToList()
                .ForEach(e => Write(e, writer, 0));

            if (showBoundary)
                Write(view.Container, writer, 0, true);

            view.Elements
                .Select(ev => ev.Element)
                .Where(e => e is Component && e.Parent?.Id == view.Container.Id)
                .OrderBy(e => e.Name).ToList()
                .ForEach(e => Write(e, writer, showBoundary ? 1 : 0));

            if (showBoundary)
                writer.WriteLine("}");

            foreach (var container in nonContainedComponents)
            {
                Write(container.Key, writer, 0, true);

                container
                    .OrderBy(e => e.Name).ToList()
                    .ForEach(e => Write(e, writer, 1));

                writer.WriteLine("}");
            }

            Write(view.Relationships, writer);

            WriteEpilog(view, writer);
        }

        protected override void Write(DynamicView view, TextWriter writer)
        {
            WriteProlog(view, writer);

            IList<Element> innerElements = new List<Element>();
            IList<Element> outerElements = new List<Element>();

            if (view.Element != null)
            {
                // boundary check via parent
                var parentId = view.Element.Id;
                view.Elements
                    .Select(ev => ev.Element)
                    .OrderBy(e => e.Name).ToList()
                    .ForEach(e =>
                    {
                        if (e?.Parent?.Id == parentId || e?.Parent?.Parent?.Id == parentId ||
                            e?.Parent?.Parent?.Parent?.Id == parentId)
                            innerElements.Add(e);
                        else
                            outerElements.Add(e);
                    });
            }
            else
            {
                // TODO: model based boundary checks are missing
                view.Elements
                    .Select(ev => ev.Element)
                    .OrderBy(e => e.Name).ToList()
                    .ForEach(e => innerElements.Add(e));
            }

            var showBoundary = (outerElements.Count > 0);

            outerElements
                .OrderBy(e => e.Name).ToList()
                .ForEach(e => Write(e, writer, 0));

            if (showBoundary)
                Write(view.Element, writer, 0, true);

            innerElements
                .OrderBy(e => e.Name).ToList()
                .ForEach(e => Write(e, writer, showBoundary ? 1 : 0));

            if (showBoundary)
                writer.WriteLine("}");

            WriteDynamicInteractions(view.Relationships, writer);

            WriteEpilog(view, writer);
        }

        protected override void Write(DeploymentView view, TextWriter writer)
        {
            WriteProlog(view, writer);

            view.Elements
                .Where(ev => ev.Element is DeploymentNode && ev.Element.Parent == null)
                .Select(ev => ev.Element as DeploymentNode)
                .OrderBy(e => e.Name).ToList()
                .ForEach(e => Write(e, writer, 0));

            Write(view.Relationships, writer);

            WriteEpilog(view, writer);
        }

        private void Write(DeploymentNode deploymentNode, TextWriter writer, int indentLevel)
        {
            var indent = indentLevel == 0 ? "" : new string(' ', indentLevel * 2);

            Write(deploymentNode, writer, indentLevel, true);

            foreach (DeploymentNode child in deploymentNode.Children)
            {
                Write(child, writer, indentLevel + 1);
            }

            foreach (ContainerInstance containerInstance in deploymentNode.ContainerInstances)
            {
                Write(containerInstance, writer, indentLevel + 1);
            }

            writer.WriteLine($"{indent}}}");
        }

        // TODO: code copied from `PlantUMLWriter, maybe cleanup in class itself missing
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
                Component component = (Component)e;
                return HasValue(component.Technology) ? component.Technology : "Component";
            }

            if (e is DeploymentNode)
            {
                DeploymentNode deploymentNode = (DeploymentNode)e;
                return HasValue(deploymentNode.Technology) ? deploymentNode.Technology : "Deployment Node";
            }

            if (e is ContainerInstance)
            {
                return "Container";
            }

            return "";
        }

        // TODO: code copied from `PlantUMLWriter, maybe cleanup in class itself missing
        private bool HasValue(string s)
        {
            return s != null && s.Trim().Length > 0;
        }

        protected override void WriteProlog(View view, TextWriter writer)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            writer.WriteLine("@startuml");

            switch (view)
            {
                case SystemLandscapeView _:
                case SystemContextView _:
                    writer.WriteLine(!string.IsNullOrWhiteSpace(CustomBaseUrl)
                        ? $"!includeurl {CustomBaseUrl}C4_Context.puml"
                        : $"!include <C4/C4_Context>");
                    break;

                case ComponentView _:
                    writer.WriteLine(!string.IsNullOrWhiteSpace(CustomBaseUrl)
                        ? $"!includeurl {CustomBaseUrl}C4_Component.puml"
                        : $"!include <C4/C4_Component>");
                    break;

                case DynamicView _:
                    writer.WriteLine(!string.IsNullOrWhiteSpace(CustomBaseUrl)
                        ? $"!includeurl {CustomBaseUrl}C4_Dynamic.puml"
                        : $"!include <C4/C4_Dynamic>");
                    break;

                case DeploymentView _:
                    writer.WriteLine(!string.IsNullOrWhiteSpace(CustomBaseUrl)
                        ? $"!includeurl {CustomBaseUrl}C4_Deployment.puml"
                        : $"!include <C4/C4_Deployment>");
                    break;

                default:
                    writer.WriteLine(!string.IsNullOrWhiteSpace(CustomBaseUrl)
                        ? $"!includeurl {CustomBaseUrl}C4_Container.puml"
                        : $"!include <C4/C4_Container>"); // as long no stdlib is used the Component diagram definition can be reused
                    break;
            }

            writer.WriteLine();
            writer.WriteLine($"' {view.GetType()}: {view.Key}");
            writer.WriteLine("title " + GetTitle(view));
            writer.WriteLine();

            if (LayoutAsSketch)
                writer.WriteLine("LAYOUT_AS_SKETCH()");  // C4 PlantUML workaround add ()
            if (Layout.HasValue)
            {
                switch (Layout)
                {
                    case LayoutDirection.LeftRight:
                        writer.WriteLine("LAYOUT_LEFT_RIGHT");
                        break;
                    case LayoutDirection.TopDown:
                        writer.WriteLine("LAYOUT_TOP_DOWN");
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown {nameof(LayoutDirection)} value");
                }
            }
            if (LayoutAsSketch || Layout.HasValue)
                writer.WriteLine();
        }

        protected virtual void WriteEpilog(View view, TextWriter writer)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            if (LayoutWithLegend)
            {
                writer.WriteLine();
                writer.WriteLine("SHOW_LEGEND()");  // C4 PlantUML workaround add ()
            }

            writer.WriteLine("@enduml");
            writer.WriteLine("");
        }

        protected virtual void Write(Element element, TextWriter writer, int indentLevel = 0, bool asBoundary = false)
        {
            var indent = indentLevel == 0 ? "" : new string(' ', indentLevel * 2);

            string
                macro,
                alias = TokenizeName(element),
                title = element.Name,
                description = element.Description,
                technology = null;

            if (asBoundary)
            {
                switch (element)
                {
                    case SoftwareSystem _:
                        macro = "System_Boundary";
                        break;
                    case Container _:
                        macro = "Container_Boundary";
                        break;
                    case DeploymentNode deploymentNode:
                        macro = "Node";
                        title = deploymentNode.Name + (deploymentNode.Instances > 1 ? $" (x{deploymentNode.Instances})" : "");
                        technology = deploymentNode.Technology;
                        break;
                    default:
                        throw new NotSupportedException($"{element.GetType()} not supported boundary type");
                }

                if (technology != null)
                    writer.WriteLine($"{indent}{macro}({alias}, \"{title}\", \"{EscapeText(technology)}\") {{");
                else
                    writer.WriteLine($"{indent}{macro}({alias}, \"{title}\") {{");
                return;
            }

            bool external = false;
            bool isDatabase = false;

            if (element is Person p)
            {
                macro = "Person";
                external = p.Location == Location.External;
            }
            else
            {
                switch (element)
                {
                    case SoftwareSystem sys:
                        macro = "System";
                        external = sys.Location == Location.External;
                        break;
                    case Container cnt:
                        macro = "Container";
                        technology = cnt.Technology ?? "";
                        isDatabase = cnt.GetIsDatabase();
                        break;
                    case Component cmp:
                        macro = "Component";
                        technology = cmp.Technology ?? "";
                        isDatabase = cmp.GetIsDatabase();
                        break;
                    case ContainerInstance cntIn:
                        macro = "Container";
                        title = cntIn.Container.Name;
                        description = cntIn.Container.Description;
                        technology = cntIn.Container.Technology;
                        isDatabase = cntIn.Container.GetIsDatabase();
                        break;

                    default:
                        throw new NotSupportedException($"Unsupported element type {element.GetType()}");
                }
            }

            if (isDatabase)
                macro += "Db";

            if (external)
                macro += "_Ext";

            writer.Write($"{indent}{macro}({alias}, \"{title}\"");
            if (technology != null) // specifically null, empty or whitespace should be handled in this block
            {
                writer.Write($", \"{EscapeText(technology)}\"");
            }
            if (!string.IsNullOrWhiteSpace(description))
            {
                writer.Write($", \"{EscapeText(description)}\"");
            }
            writer.WriteLine(")");
        }

        protected virtual void Write(ISet<RelationshipView> relationships, TextWriter writer)
        {
            relationships
                .OrderBy(rv => rv.Relationship.Source.Name + rv.Relationship.Destination.Name).ToList()
                .ForEach(rv => Write(rv, writer));
        }

        protected virtual void Write(RelationshipView relationshipView, TextWriter writer, string advancedDescription = null)
        {
            var relationship = relationshipView.Relationship;
            string
                source = TokenizeName(relationship.Source),
                dest = TokenizeName(relationship.Destination),
                label = advancedDescription ?? relationship.Description ?? "",
                tech = !string.IsNullOrWhiteSpace(relationship.Technology) ? relationship.Technology : null;

            var macro = GetSpecificLayoutMacro(relationshipView);

            writer.Write($"{macro}({source}, {dest}, \"{EscapeText(label)}\"");
            if (tech != null)
                writer.Write($", \"{EscapeText(tech)}\"");
            writer.WriteLine(")");
        }

        protected virtual void WriteDynamicInteractions(ISet<RelationshipView> relationships, TextWriter writer)
        {
            relationships
                .OrderBy(rv => rv.Order).ToList()
                .ForEach(rv => WriteDynamicInteraction(rv, writer, rv.Order, rv.Description));
        }

        protected virtual void WriteDynamicInteraction(RelationshipView relationshipView, TextWriter writer, string order, string label = null)
        {
            var relationship = relationshipView.Relationship;
            string
                source = TokenizeName(relationship.Source),
                dest = TokenizeName(relationship.Destination),
                tech = !string.IsNullOrWhiteSpace(relationship.Technology) ? relationship.Technology : null;

            var macro = GetSpecificLayoutMacro(relationshipView);
            macro = "RelIndex" + macro.Substring("Rel".Length);

            writer.Write($"{macro}(\"{order}\", {source}, {dest}, \"{EscapeText(label)}\"");
            if (tech != null)
                writer.Write($", \"{EscapeText(tech)}\"");
            writer.WriteLine(")");
        }

        private static string GetSpecificLayoutMacro(RelationshipView relationshipView)
        {
            var direction = relationshipView.GetDirection(out _);
            switch (direction)
            {
                case DirectionValues.Back: return "Rel_Back";
                case DirectionValues.Neighbor: return "Rel_Neighbor";  // C4 PlantUml without u
                case DirectionValues.Neighbour: return "Rel_Neighbor";  // C4 PlantUml without u
                case DirectionValues.BackNeighbor: return "Rel_Back_Neighbor"; // C4 PlantUml without u
                case DirectionValues.BackNeighbour: return "Rel_Back_Neighbor"; // C4 PlantUml without u
                case DirectionValues.Up: return "Rel_Up";
                case DirectionValues.Down: return "Rel_Down";
                case DirectionValues.Left: return "Rel_Left";
                case DirectionValues.Right: return "Rel_Right";
                default: return "Rel";
            }
        }
    }
}