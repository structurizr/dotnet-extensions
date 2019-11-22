using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;

namespace Structurizr.IO.Json
{
    public class JsonWriter
    {

        public bool IndentOutput { get; set; }

        public JsonWriter(bool indentOutput)
        {
            this.IndentOutput = indentOutput;
        }

        public void Write(Workspace workspace, TextWriter writer)
        {
            string json = JsonConvert.SerializeObject(workspace,
                IndentOutput ? Formatting.Indented : Formatting.None,
                new StringEnumConverter(),
                new IsoDateTimeConverter(),
                new PaperSizeJsonConverter());

            writer.Write(json);
        }

    }
}
