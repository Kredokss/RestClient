using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRestClient.Entities
{
    class Country
    {
        public string code { get; set; }
        public string txt1 { get; set; }
        public string txt2 { get; set; }
        public string txt3 { get; set; }
        public string txt4 { get; set; }
        public string intDialCode { get; set; }
        public int addrFormatId { get; set; }
        public int isVatIncluded { get; set; }
        public int active { get; set; }
    }
}
