using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace ElasticSearchNest
{
    class Security
    {
        public string TickerSymbol { get; set; }

        public string BloombergSymbol { get; set; }
        public string ISINSymbol { get; set; }


        public string SEDOLSymbol
        {
            get;
            set;
        }
        public int? AssetID { get; set; }
        public string CUSIPSymbol { get; set; }
        public string UnderLyingSymbol { get; set; }

    }
}
