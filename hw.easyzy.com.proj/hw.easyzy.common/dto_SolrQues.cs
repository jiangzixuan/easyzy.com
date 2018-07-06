using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.common
{
    public class dto_SolrQues
    {
        [SolrUniqueKey("id")]
        public int id { get; set; }

        [SolrField("courseid")]
        public int courseid { get; set; }

        [SolrField("kpoints")]
        public string kpoints { get; set; }

        [SolrField("cpoints")]
        public string cpoints { get; set; }

        [SolrField("btypeid")]
        public int btypeid { get; set; }

        [SolrField("difftype")]
        public int difftype { get; set; }

        [SolrField("paperyear")]
        public int paperyear { get; set; }

        [SolrField("usagetimes")]
        public int usagetimes { get; set; }
    }
}
