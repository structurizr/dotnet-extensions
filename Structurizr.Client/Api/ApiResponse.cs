using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Structurizr.Api
{

    [DataContract]
    internal sealed class ApiResponse
    {

        [DataMember(Name = "success", EmitDefaultValue = false)]
        internal bool Success;

        [DataMember(Name = "message", EmitDefaultValue = false)]
        internal string Message;

        [DataMember(Name = "revision", EmitDefaultValue = false)]
        internal long? Revision;

        static internal ApiResponse Parse(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                Converters = new List<JsonConverter> {
                    new IsoDateTimeConverter()
                }
            };

            ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(json, settings);
            return apiResponse;
        }

    }
}