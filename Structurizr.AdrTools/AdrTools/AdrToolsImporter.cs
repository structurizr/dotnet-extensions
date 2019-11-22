using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Structurizr.Documentation;

namespace Structurizr.AdrTools
{

    public class AdrToolsImporter
    {

        private readonly Regex TitleRegex = new Regex("^# \\d*\\. (.*)$", RegexOptions.Multiline);
        private readonly Regex DateRegex = new Regex("^Date: (\\d\\d\\d\\d-\\d\\d-\\d\\d)$", RegexOptions.Multiline);
        private readonly Regex StatusRegex = new Regex("## Status\\n\\n(\\w*)", RegexOptions.Multiline);

        private const string SupercededAlternativeSpelling = "Superceded";

        private Workspace _workspace;
        private DirectoryInfo _path;

        public AdrToolsImporter(Workspace workspace, DirectoryInfo path)
        {
            if (workspace == null)
            {
                throw new ArgumentException("A workspace must be specified.");
            }

            if (path == null)
            {
                throw new ArgumentException("The path to the architecture decision records must be specified.");
            }

            if (!Directory.Exists(path.FullName))
            {
                throw new ArgumentException(path.FullName + " does not exist.");
            }

            _workspace = workspace;
            _path = path;
        }

        public ISet<Decision> ImportArchitectureDecisionRecords()
        {
            return ImportArchitectureDecisionRecords(null);
        }

        public ISet<Decision> ImportArchitectureDecisionRecords(SoftwareSystem softwareSystem)
        {
            HashSet<Decision> decisions = new HashSet<Decision>();

            IEnumerable<FileInfo> markdownFiles = _path.GetFiles().Where(f => f.Name.EndsWith(".md"));

            // first create an index of filename -> ID
            Dictionary<string, string> index = new Dictionary<string, string>();
            foreach (FileInfo file in markdownFiles)
            {
                index.Add(file.Name, ExtractIntegerIdFromFileName(file));
            }

            foreach (FileInfo file in markdownFiles)
            {
                string id = ExtractIntegerIdFromFileName(file);
                DateTime date = new DateTime();
                string title = "";
                DecisionStatus status = DecisionStatus.Proposed;
                string content = File.ReadAllText(file.FullName, Encoding.UTF8);
                content = content.Replace("\r", "");
                Format format = Format.Markdown;

                title = ExtractTitle(content);
                date = ExtractDate(content);
                status = ExtractStatus(content);

                foreach (string filename in index.Keys) {
                    content = content.Replace(filename, CalculateUrl(softwareSystem, index[filename]));
                }

                Decision decision = _workspace.Documentation.AddDecision(softwareSystem, id, date, title, status, format, content);
                decisions.Add(decision);
            }

            return decisions;
        }

        private string CalculateUrl(SoftwareSystem softwareSystem, string id)
        {
            if (softwareSystem == null) {
                return "#/:" + UrlEncode(id);
            }
            else
            {
                return "#" + UrlEncode(softwareSystem.CanonicalName) + ":" + UrlEncode(id);
            }
        }

        private string UrlEncode(string value)
        {
            return WebUtility.UrlEncode(value).Replace("+", "%20");
        }

        private string ExtractIntegerIdFromFileName(FileInfo file)
        {
            return "" + Convert.ToInt64(file.Name.Substring(0, 4));
        }

        private string ExtractTitle(string content)
        {
            MatchCollection matchCollection = TitleRegex.Matches(content);
            if (matchCollection.Count > 0)
            {
                return matchCollection[0].Groups[1].Value;
            }
            else
            {
                return "Untitled";
            }
        }

        private DateTime ExtractDate(string content)
        {
            MatchCollection matchCollection = DateRegex.Matches(content);
            if (matchCollection.Count > 0)
            {
                string dateAsString = matchCollection[0].Groups[1].Value;
                DateTime date;
                DateTime.TryParseExact(dateAsString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                return date;
            }
            else
            {
                return new DateTime();
            }
        }

        private DecisionStatus ExtractStatus(string content)
        {
            MatchCollection matchCollection = StatusRegex.Matches(content);
            if (matchCollection.Count > 0)
            {
                string status = matchCollection[0].Groups[1].Value;

                if (status == SupercededAlternativeSpelling)
                {
                    return DecisionStatus.Superseded;
                }
                else
                {
                    DecisionStatus decisionStatus;
                    Enum.TryParse(status, out decisionStatus);

                    return decisionStatus;
                }
            }
            else
            {
                return DecisionStatus.Proposed;
            }
        }

    }
}