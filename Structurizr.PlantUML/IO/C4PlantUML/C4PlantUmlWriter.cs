using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Structurizr.IO.C4PlantUML.ModelExtensions;

// Source base version copied from https://gist.github.com/coldacid/465fa8f3a4cd3fdd7b640a65ad5b86f4 (https://github.com/structurizr/dotnet/issues/47) 
// kirchsth: Extended with dynamic and deployment view
// kirchsth: updated to update generated source to new C4PlantUml stdlib v2.2.0 (no additional dynamic and deployment view macros are required anymore, calls updated)
// kirchsth: Support ViewConfiguration, tags and styles
// kirchsth: next planed C4PlantUml stdlib v2.3.0 features can be used with CustomBaseUrl https://raw.githubusercontent.com/kirchsth/C4-PlantUML/extended/
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
        /// C4PlantUml stdlib v2.2.0 () supports dynamic or deployment diagrams. They can be used via the PlantUML-stdlib and no 
        /// special CustomBaseUrl is required.
        /// Only next stdlib features (like Person shapes) has to be defined via CustomBaseUrl=https://raw.githubusercontent.com/kirchsth/C4-PlantUML/extended/
        /// (if the value is empty/null then PlantUML-stdlib with added definitions is used)
        /// </summary>
        public string CustomBaseUrl { get; set; } =""; // @"https://raw.githubusercontent.com/kirchsth/C4-PlantUML/extended/";
        public bool EnableNextFeatures { get; set; } = false; // true;

        public string AdditionalProlog { get; set; }
        public string AdditionalEpilog { get; set; }

        protected override void Write(SystemLandscapeView view, ViewConfiguration viewConfiguration, TextWriter writer)
        {
            var showBoundary = view.EnterpriseBoundaryVisible ?? true;

            WriteProlog(view, viewConfiguration, writer);

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

            WriteEpilog(view, viewConfiguration, writer);
        }

        protected override void Write(SystemContextView view, ViewConfiguration viewConfiguration, TextWriter writer)
        {
            var showBoundary = view.EnterpriseBoundaryVisible ?? true;

            WriteProlog(view, viewConfiguration, writer);

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

            WriteEpilog(view, viewConfiguration, writer);
        }

        protected override void Write(ContainerView view, ViewConfiguration viewConfiguration, TextWriter writer)
        {
            var externals = view.Elements
                .Select(ev => ev.Element)
                .Where(e => !(e is Container));
            var showBoundary = externals.Any();

            WriteProlog(view, viewConfiguration, writer);

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

            WriteEpilog(view, viewConfiguration, writer);
        }

        protected override void Write(ComponentView view, ViewConfiguration viewConfiguration, TextWriter writer)
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

            WriteProlog(view, viewConfiguration, writer);

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

            WriteEpilog(view, viewConfiguration, writer);
        }

        protected override void Write(DynamicView view, ViewConfiguration viewConfiguration, TextWriter writer)
        {
            WriteProlog(view, viewConfiguration, writer);

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

            WriteEpilog(view, viewConfiguration, writer);
        }

        protected override void Write(DeploymentView view, ViewConfiguration viewConfiguration, TextWriter writer)
        {
            WriteProlog(view, viewConfiguration, writer);

            view.Elements
                .Where(ev => ev.Element is DeploymentNode && ev.Element.Parent == null)
                .Select(ev => ev.Element as DeploymentNode)
                .OrderBy(e => e.Name).ToList()
                .ForEach(e => Write(e, writer, 0));

            Write(view.Relationships, writer);

            WriteEpilog(view, viewConfiguration, writer);
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

            foreach (SoftwareSystemInstance systemInstance in deploymentNode.SoftwareSystemInstances)
            {
                Write(systemInstance, writer, indentLevel + 1);
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

            if (e is SoftwareSystemInstance)
            {
                return "Software System";
            }

            return "";
        }

        // TODO: code copied from `PlantUMLWriter, maybe cleanup in class itself missing
        private bool HasValue(string s)
        {
            return s != null && s.Trim().Length > 0;
        }

        protected override void WriteProlog(View view, ViewConfiguration viewConfiguration, TextWriter writer)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            writer.WriteLine("@startuml");

            string diagramType;
            HashSet<string> existingLegendTags; // (already mapped) tags (styles) which have to be overwritten not added

            switch (view)
            {
                case SystemLandscapeView _:
                case SystemContextView _:
                    diagramType = "Context";
                    existingLegendTags = new HashSet<string> { "person", "system", "external_person", "external_system" };
                    break;

                case ContainerView _:
                    diagramType = "Container";
                    existingLegendTags = new HashSet<string> { "person", "system", "container", "external_person", "external_system", "external_container" };
                    break;

                case DynamicView _:
                    diagramType = "Dynamic";
                    existingLegendTags = new HashSet<string> { "person", "system", "container", "component", "external_person", "external_system", "external_container", "external_component" };
                    break;

                case DeploymentView _:
                    diagramType = "Deployment";
                    existingLegendTags = new HashSet<string> { "person", "system", "container", "external_person", "external_system", "external_container", "node" };
                    break;

                default:
                    diagramType = "Component";
                    existingLegendTags = new HashSet<string> { "person", "system", "container", "component", "external_person", "external_system", "external_container", "external_component" };
                    break;
            }

            writer.WriteLine(!string.IsNullOrWhiteSpace(CustomBaseUrl)
                ? $"!includeurl {CustomBaseUrl}C4_{diagramType}.puml"
                : $"!include <C4/C4_{diagramType}>");

            writer.WriteLine();
            writer.WriteLine($"' {view.GetType()}: {view.Key}");
            writer.WriteLine("title " + GetTitle(view));
            writer.WriteLine();

            if (LayoutAsSketch)
                writer.WriteLine("LAYOUT_AS_SKETCH()");

            if (EnableNextFeatures)
                writer.WriteLine("SHOW_PERSON_OUTLINE()");

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

            WriteExistingStyles(view, existingLegendTags, viewConfiguration, writer);

            if (!string.IsNullOrWhiteSpace(AdditionalProlog))
                writer.WriteLine(AdditionalProlog);
        }

        protected override void WriteEpilog(View view, ViewConfiguration viewConfiguration, TextWriter writer)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            if (!string.IsNullOrWhiteSpace(AdditionalEpilog))
                writer.WriteLine(AdditionalEpilog);

            if (LayoutWithLegend)
            {
                writer.WriteLine();
                writer.WriteLine("SHOW_LEGEND()");  // C4 PlantUML workaround add ()
            }

            writer.WriteLine("@enduml");
            writer.WriteLine();
        }

        protected virtual void WriteExistingStyles(View view, HashSet<string> existingLegendTags, ViewConfiguration viewConfiguration, TextWriter writer)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            ElementStyle baseES = viewConfiguration.Styles.Elements.FirstOrDefault(es => es.Tag == "Element");
            RelationshipStyle definedRS = viewConfiguration.Styles.Relationships.FirstOrDefault(rs => rs.Tag == "Relationship");

            if (EnableNextFeatures) // linestyle
            {
                // add Back related style (which is typically dotted in Structurizr) (and if defined then it will be overwritten with viewConfiguration.Styles.Relationships)
                writer.WriteLine("AddRelTag(\"Back\", $textColor=$ARROW_COLOR, $lineColor=$ARROW_COLOR, $lineStyle = DottedLine())");
                writer.WriteLine();
            }

            foreach (var es in viewConfiguration.Styles.Elements)
                if (es != baseES) // skip Element
                    Write(es, baseES, existingLegendTags, writer);
            foreach (var rs in viewConfiguration.Styles.Relationships)
                Write(rs, definedRS, writer);

            if (viewConfiguration.Styles.Elements.Count > 0 || viewConfiguration.Styles.Relationships.Count > 0)
                writer.WriteLine();
        }

        protected virtual void Write(ElementStyle es, ElementStyle baseElementStyle, HashSet<string> existingLegendTags, TextWriter writer)
        {
            var defined = StructurizrTags2DiagramTags.TryGetValue(es.Tag, out var diagramTag);
            if (!defined)
                diagramTag = es.Tag;

            // UpdateElementStyle or AddElementTag(elementName, ?bgColor, ?fontColor, ?borderColor, ?shadowing, ?shape) // ?shadowing not used; ?shape only rounded or eight-sided
            var allArgs = new StringBuilder();
            WriteColor("$bgColor", es.Background, baseElementStyle?.Background, false, allArgs);
            WriteColor("$fontColor", es.Color, baseElementStyle?.Color, false, allArgs);
            WriteColor("$borderColor", es.Stroke, baseElementStyle?.Stroke, false, allArgs);
            if (EnableNextFeatures)
            {
                // default shape of element is ignored
                WriteShape(es.Shape, allArgs);
            }

            if (allArgs.Length > 0)
            {
                writer.Write(defined ? "UpdateElementStyle" : "AddElementTag");
                writer.WriteLine($"({diagramTag}{allArgs})");
            }
        }

        protected virtual void Write(RelationshipStyle rs, RelationshipStyle definedRelationshipStyle, TextWriter writer)
        {
            // only "Relationship" is predefined (which is defaultRelationshipStyle)
            var diagramTag = rs.Tag;
            var defined = (rs == definedRelationshipStyle);

            // UpdateRelStyle or AddRelTag(tagStereo, ?textColor, ?lineColor, ?lineStyle)
            var allArgs = new StringBuilder();
            WriteColor("$textColor", rs.Color, definedRelationshipStyle?.Color, defined, allArgs);
            WriteColor("$lineColor", rs.Color, definedRelationshipStyle?.Color, defined, allArgs);
            if (!defined)
            {
                if (EnableNextFeatures)
                {
                    if (rs.Dashed == true) // C# Structurize does not support all styles
                        allArgs.Append($", $lineStyle = DashedLine()");
                }
            }

            if (allArgs.Length > 0)
            {
                writer.Write(defined ? "UpdateRelStyle(" : $"AddRelTag({diagramTag}, ");
                writer.WriteLine($"{allArgs.Remove(0, 2)})"); // remove first ", "
            }
        }

        protected void WriteColor(string argName, string elementColor, string defaultColor, bool lineColorsRequired, StringBuilder allArgs)
        {
            var color = elementColor;
            if (string.IsNullOrWhiteSpace(color))
                color = defaultColor;

            if (!string.IsNullOrWhiteSpace(color))
                allArgs.Append($", {argName} = \"{color}\"");
            else if (lineColorsRequired)
                allArgs.Append($", {argName} = $ARROW_COLOR");
        }

        protected void WriteShape(Shape elementShape, StringBuilder allArgs)
        {
            if (EnableNextFeatures)
            {
                switch (elementShape)
                {
                    case Shape.RoundedBox:
                        allArgs.Append($", $shape = RoundedBoxShape()");
                        break;
                    case Shape.Hexagon:
                        allArgs.Append($", $shape = EightSidedShape()");
                        break;
                    default:
                        // all other ignored atm (Database handled via ..Db() extension)
                        break;
                }
            }
        }

        protected static Dictionary<string, string> StructurizrTags2DiagramTags = new Dictionary<string, string>
        {
            // Element is handled via defaultElementStyle and is not added as tag
            ["Element"] = "",
            ["Person"] = "person",
            ["Software System"] = "system",
            ["Container"] = "container",
            ["Component"] = "component",
            ["Deployment Node"] = "node"
            // ?? how should this tags be mapped -> reused without special mapping atm
            // ["Infrastructure Node"] = "",
            // ["Software System Instance"] = "",
            // ["Container Instance"] = "",
        };

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
                    case SoftwareSystemInstance sysIn:
                        macro = "System";
                        title = sysIn.SoftwareSystem.Name;
                        description = sysIn.SoftwareSystem.Description;
                        external = sysIn.SoftwareSystem.Location == Location.External;
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
            WriteTags(element, writer);
            writer.WriteLine(")");
        }

        private void WriteTags(Element element, TextWriter writer)
        {
            var tags = element.GetAllTags().Where(t => !StructurizrTags2DiagramTags.ContainsKey(t)).Reverse().ToList();
            if (tags.Count > 0)
            {
                var combinedTags = string.Join("+", tags);
                writer.Write($", $tags=\"{EscapeText(combinedTags)}\"");
            }
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

            if (relationshipView.Response == true)
            {
                var swap = source;
                source = dest;
                dest = swap;
            }

            var macro = GetSpecificLayoutMacro(relationshipView);

            writer.Write($"{macro}({source}, {dest}, \"{EscapeText(label)}\"");
            if (tech != null)
                writer.Write($", \"{EscapeText(tech)}\"");
            WriteTags(relationshipView, writer);
            writer.WriteLine(")");
        }

        private void WriteTags(RelationshipView relationshipView, TextWriter writer)
        {
            var relationship = relationshipView.Relationship;
            var tags = new List<string>();
            if (relationshipView.Response == true)
                tags.Add("Back");

            tags.AddRange(relationship.GetAllTags().Where(t => t != "Relationship").Reverse());
            if (tags.Count > 0)
            {
                var combinedTags = string.Join("+", tags);
                writer.Write($", $tags=\"{EscapeText(combinedTags)}\"");
            }
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

            if (relationshipView.Response == true)
            {
                var swap = source;
                source = dest;
                dest = swap;
            }

            var macro = GetSpecificLayoutMacro(relationshipView);
            macro = "RelIndex" + macro.Substring("Rel".Length);

            writer.Write($"{macro}(\"{order}\", {source}, {dest}, \"{EscapeText(label)}\"");
            if (tech != null)
                writer.Write($", \"{EscapeText(tech)}\"");
            WriteTags(relationshipView, writer);
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