using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFrameLibrary.Models
{
    public class JsonValidationResult<T>
    {        
        public bool IsJsonEmpty { get; set; } = true;

        public bool IsJsonDeserialized { get; set; } = false;

        public bool IsModelValid { get; set; } = true;

        public bool IsModelEmpty { get; set; } = true;

        public List<T> Model { get; set; }

        public string Message { get; set; } = "Json Empty";
    }
}
