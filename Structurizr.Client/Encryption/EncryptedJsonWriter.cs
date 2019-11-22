using Newtonsoft.Json;
using Structurizr.IO.Json;
using System.IO;

namespace Structurizr.Encryption
{
    public class EncryptedJsonWriter
    {

        public bool IndentOutput { get; set; }

        public EncryptedJsonWriter(bool indentOutput)
        {
            this.IndentOutput = indentOutput;
        }

        public void Write(EncryptedWorkspace workspace, StringWriter writer)
        {
            string json = JsonConvert.SerializeObject(workspace,
                IndentOutput == true ? Formatting.Indented : Formatting.None,
                new Newtonsoft.Json.Converters.StringEnumConverter(),
                new PaperSizeJsonConverter());

            writer.WriteLine(json);
        }


    }
}
