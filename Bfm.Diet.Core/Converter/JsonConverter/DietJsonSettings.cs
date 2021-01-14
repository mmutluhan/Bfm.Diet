using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bfm.Diet.Core.Converter.JsonConverter
{
    public static class DietJsonSettings
    {
        public static JsonSerializerSettings OwnJsonSerializerSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}