using System.Collections.Generic;

namespace OFrameLibrary.Models
{
    public class JsonValidationResult<T>
    {
        public bool IsJsonDeserialized { get; set; } = false;

        public bool IsJsonEmpty { get; set; } = true;

        public bool IsModelEmpty { get; set; } = true;

        public bool IsModelValid { get; set; } = true;

        public string Message { get; set; } = "Json Empty";

        public List<T> Model { get; set; }
    }
}
