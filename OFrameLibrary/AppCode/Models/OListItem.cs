using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace OFrameLibrary.Models
{
    public class OListItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
        public bool Disabled { get; set; }
        public AttributeCollection Attributes { get; set; }
    }
}
