using System;
using System.IO;
using System.Linq;
using System.Text;

// Source base version copied from https://gist.github.com/coldacid/465fa8f3a4cd3fdd7b640a65ad5b86f4 (https://github.com/structurizr/dotnet/issues/47) 
// kirchsth: Extended with dynamic and deployment view
namespace Structurizr.IO.C4PlantUML
{
    /// <summary>
    /// Provides a starting point and helper methods for <see cref="IPlantUMLWriter"/> implementations.
    /// </summary>
    public abstract class PlantUMLWriterBase : IPlantUMLWriter
    {
        // enables the calculation of linked elements
        // (If the deployment references are copied in Model.AddContainerInstance() call then no tags are copied,
        // therefore also the linkedRelationship has to be checked)
        protected Model CurrentViewModel;

        /// <inheritdoc/>
        public void Write(Workspace workspace, TextWriter writer)
        {
            if (workspace == null) throw new ArgumentNullException(nameof(workspace));
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            CurrentViewModel = workspace.Model;

            workspace.Views.SystemLandscapeViews.ToList().ForEach(v => Write(v, writer));
            workspace.Views.SystemContextViews.ToList().ForEach(v => Write(v, writer));
            workspace.Views.ContainerViews.ToList().ForEach(v => Write(v, writer));
            workspace.Views.ComponentViews.ToList().ForEach(v => Write(v, writer));
            workspace.Views.DynamicViews.ToList().ForEach(v => Write(v, writer));
            workspace.Views.DeploymentViews.ToList().ForEach(v => Write(v, writer));
        }

        /// <inheritdoc/>
        public void Write(View view, TextWriter writer)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            CurrentViewModel = view.Model;

            switch (view)
            {
                case SystemLandscapeView sl:
                    Write(sl, writer);
                    break;
                case SystemContextView sc:
                    Write(sc, writer);
                    break;
                case ContainerView ct:
                    Write(ct, writer);
                    break;
                case ComponentView cp:
                    Write(cp, writer);
                    break;
                case DynamicView dy:
                    Write(dy, writer);
                    break;
                case DeploymentView de:
                    Write(de, writer);
                    break;
                default:
                    throw new NotSupportedException($"{view.GetType()} not supported for export");
            }
        }

        /// <summary>
        /// Writes a system landscape view in PlantUML format to the provided writer.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="writer"></param>
        protected abstract void Write(SystemLandscapeView view, TextWriter writer);

        /// <summary>
        /// Writes a system context view in PlantUML format to the provided writer.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="writer"></param>
        protected abstract void Write(SystemContextView view, TextWriter writer);

        /// <summary>
        /// Writes a container view in PlantUML format to the provided writer.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="writer"></param>
        protected abstract void Write(ContainerView view, TextWriter writer);

        /// <summary>
        /// Writes a component view in PlantUML format to the provided writer.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="writer"></param>
        protected abstract void Write(ComponentView view, TextWriter writer);

        /// <summary>
        /// Writes a dynamic view in PlantUML format to the provided writer.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="writer"></param>
        protected abstract void Write(DynamicView view, TextWriter writer);

        /// <summary>
        /// Writes a deployment view in PlantUML format to the provided writer.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="writer"></param>
        protected abstract void Write(DeploymentView view, TextWriter writer);

        /// <summary>
        /// Produces a standard PlantUML diagram prolog for the provided view.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="writer"></param>
        /// <exception cref="ArgumentNullException"><paramref name="view"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        protected virtual void WriteProlog(View view, TextWriter writer)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            writer.WriteLine("@startuml");
            writer.WriteLine("' key: " + view.Key);
            writer.WriteLine("title " + GetTitle(view));
        }

        /// <summary>
        /// Produces a standard PlantUML diagram epilog for the provided view.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="writer"></param>
        /// <exception cref="ArgumentNullException"><paramref name="view"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        protected virtual void WriteEpilog(View view, TextWriter writer)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            writer.WriteLine("@enduml");
            writer.WriteLine("");
        }

        /// <summary>
        /// Creates a tokenized string from the provided <see cref="Element"/> for use as a PlantUML object name.
        /// </summary>
        /// <param name="e">The element whose name will be tokenized.</param>
        /// <returns>A string that may be used as a name or ID token for the provided element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="e"/> is <see langword="null"/>.</exception>
        protected string TokenizeName(Element e) =>
            e != null
                ? TokenizeName(e.CanonicalName, e.GetHashCode())
                : throw new ArgumentNullException(nameof(e));

        /// <summary>
        /// Creates a tokenized variant of the provided string, with optional hash code appended.
        /// </summary>
        /// <param name="s">The string to tokenize.</param>
        /// <param name="hash">An optional hash code to append to the tokenized string.</param>
        /// <returns>
        /// The tokenized variant of the string and optional hash code, or the empty string if <paramref name="s"/> is
        /// <see langword="null"/> or empty.
        /// </returns>
        protected string TokenizeName(string s, int? hash = null)
        {
            if (String.IsNullOrWhiteSpace(s)) return "";

            s = s
                .Trim('/')
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("/", "__");
            if (hash.HasValue)
            {
                s = s + "__" + hash.Value.ToString("x");
            }

            return s;
        }

        /// <summary>
        /// Gets the title to use for the provided view.
        /// </summary>
        /// <param name="view">The view whose title or name to retrieve.</param>
        /// <returns>The title of the view, if it was set; otherwise the name of the view.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="view"/> is <see langword="null"/>.</exception>
        protected virtual string GetTitle(View view) =>
            view != null
                ? String.IsNullOrWhiteSpace(view.Title) ? view.Name : view.Title
                : throw new ArgumentNullException(nameof(view));

        protected string BlockText(string s, int blockWidth, string formattedLineBreak)
        {
            var block = s;

            if (blockWidth > 0 && !s.Contains("\n") && !s.Contains("\r"))
            {
                var formatted = new StringBuilder();
                int pos = 0;
                string word = "";

                foreach (var c in s)
                {
                    word += c;
                    if (c == ' ')
                    {
                        if (pos != 0 && pos + word.Length > blockWidth)
                        {
                            formatted.Append(formattedLineBreak);
                            pos = 0;
                        }
                        formatted.Append(word);
                        pos += word.Length;
                        word = "";
                    }
                }

                if (word.Length > 0)
                {
                    if (pos != 0 && pos + word.Length > blockWidth)
                        formatted.Append(formattedLineBreak);
                    formatted.Append(word);
                }

                block = formatted.ToString();
            }

            return block;
        }

        protected string EscapeText(string s) => s.Replace("\"", "&quot;");
    }
}