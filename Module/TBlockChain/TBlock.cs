using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TBlockChain
{
    public class TBlock
    {
        public string CurrentHash { get; set; }
        public object Data { get; set; }
        public string PreHash { get; set; }
        public long TimeSpan { get; set; }
        public int Nonce { get; set; }
    }
}
