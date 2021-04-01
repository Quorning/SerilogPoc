using System;
using Newtonsoft.Json;

namespace Infrastructure.Transformation
{
    public interface IJsonSerializer
    {
        T FromJson<T>(string json);
        string ToJson(object value, Newtonsoft.Json.Formatting formating);
        bool TryDeserialize<T>(string json, out T deserializedObject);
    }

    public class JsonSerializer : IJsonSerializer
    {
        public T FromJson<T>(string json)
        {
            if (json == null) throw new ArgumentNullException(nameof(json));
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException(@"Value cannot be null or whitespace.", nameof(json));

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            T convertedObject = JsonConvert.DeserializeObject<T>(json, settings);

            return convertedObject;
        }

        public bool TryDeserialize<T>(string json, out T deserializedObject)
        {
            if (json == null) throw new ArgumentNullException(nameof(json));

            bool canDeserialize;
            deserializedObject = default;
            try
            {
                deserializedObject = FromJson<T>(json);
                canDeserialize = true;
            }
            catch
            {
                canDeserialize = false;
            }
            return canDeserialize;
        }

        public string ToJson(object value, Newtonsoft.Json.Formatting formating = Newtonsoft.Json.Formatting.None)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

            return JsonConvert.SerializeObject(value, formating, settings);
        }
    }
}
