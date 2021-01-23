using Newtonsoft.Json;

namespace Bfm.Diet.Core.Json
{
    public class SerializerSettings
    {
        public static readonly JsonSerializerSettings BfmJsonSerializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            TypeNameHandling = TypeNameHandling.All,
            NullValueHandling = NullValueHandling.Include,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
        };
    }
}