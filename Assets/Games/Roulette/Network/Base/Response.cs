using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Usen
{
    public class Response
    {
        public int code;
        public string message;
        
        [JsonProperty] protected Error error;
        
        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (error != null)
            {
                code = error.code;
                message = error.message;
            }
        }
        
        public class Error
        {
            public int code;
            public string message;
        }
    }
}