using Newtonsoft.Json;
using Structurizr.IO.Json;
using System.IO;

namespace Structurizr.Encryption
{
    public class EncryptedJsonReader
    {

        public EncryptedWorkspace Read(StringReader reader)
        {
            EncryptedWorkspace workspace = JsonConvert.DeserializeObject<EncryptedWorkspace>(
                reader.ReadToEnd(),
                new Newtonsoft.Json.Converters.StringEnumConverter(),
                new PaperSizeJsonConverter(),
                new EncryptionStrategyJsonConverter());

            return workspace;
        }

    }
}
