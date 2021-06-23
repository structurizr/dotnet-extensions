using System;
using System.Linq;
using Structurizr.PlantUML.IO.C4PlantUML;

namespace Structurizr.IO.C4PlantUML.ModelExtensions
{
    public static class ModelExtensions
    {
        /// <summary>
        /// new impl. of CanonicalName starts with "{ElementType}://" or "{ElementType}://{DeploymentName}/{DeploymentName}/"   instead of "/" therefore it can be optionally ignored
        /// Additional / in staticNames have to be converted to .
        /// </summary>
        /// <param name="canonicalName">the canonical name with elementType prefix (e.g. Container://SoftwareSystem/Container) or without elementType prefix (e.g. /SoftwareSystem/Container)</param>
        /// <returns></returns>
        public static Element GetElementWithCanonicalOrStaticalName(this Model model, string canonicalName, bool compareOnlyLastPart=true)
        {
            if (string.IsNullOrWhiteSpace(canonicalName))
                throw new ArgumentException("A canonical name must be specified.");
            var found = model.GetElements().FirstOrDefault<Element>((Func<Element, bool>) (x =>
            {
                if (compareOnlyLastPart)
                    return x.CanonicalName.EndsWith(canonicalName);
                else
                    return x.CanonicalName == canonicalName;
            }));

            if (found == null)
            {
                var all = model.GetElements().Select(e => e.CanonicalName).ToList();
                var combined = string.Join("\n", all);
                throw new C4PlantUmlException(
                    $"Element {canonicalName} could not be found. Following elements exist:\n{combined}");
            }
            return found;
        }
    }
}