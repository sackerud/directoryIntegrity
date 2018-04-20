using System;
using directoryIntegrity.Core.FileSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace directoryIntegrity.Core.ReferenceFile
{
    public class JsonReferenceFileConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IFileSystemEntry);
        }
        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            throw new InvalidOperationException("Use default serialization.");
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            // TODO: JObject.Load expects an object but we have an array of fs entries
            var jsonObject = JObject.Load(reader);
            var fsEntry = default(IFileSystemEntry);
            if (jsonObject["IsDirectory"].Value<bool>())
            {
                fsEntry = new Directory(jsonObject["Path"].ToString());
            }
            else
            {
                fsEntry = new File(jsonObject["Path"].ToString());
            }
            serializer.Populate(jsonObject.CreateReader(), fsEntry);
            return fsEntry;
        }
    }

}
