using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace OFrameLibrary.Helpers
{
    public static class JsonHelper
    {
        /// <summary>
        /// JSON Deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString">todo: describe jsonString parameter on JsonDeserialize</param>
        public static T JsonDeserialize<T>(string jsonString)
        {
            var ser = new DataContractJsonSerializer(typeof(T));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

            return (T)ser.ReadObject(ms);
        }

        /// <summary>
        /// JSON Serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">todo: describe t parameter on JsonSerializer</param>
        public static string JsonSerializer<T>(T t)
        {
            var ser = new DataContractJsonSerializer(typeof(T));

            var ms = new MemoryStream();

            ser.WriteObject(ms, t);

            var jsonString = Encoding.UTF8.GetString(ms.ToArray());

            ms.Close();

            return jsonString;
        }
    }
}
