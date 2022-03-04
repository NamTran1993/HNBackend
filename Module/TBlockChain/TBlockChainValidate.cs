using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TBlockChain
{
    public static class TBlockChainValidate
    {
        public static bool TIsChainValid(this List<TBlockChain> lstBlockChain)
        {
            try
            {
                if (lstBlockChain == null || lstBlockChain.Count == 0)
                    throw new Exception("List BlockChain is null or count == 0");

                TBlock curBlock = null;
                TBlock preBlock = null;

                for (int i = 1; i < lstBlockChain.Count; i++)
                {
                    TBlockChain tCurrent = lstBlockChain.ElementAt(i);
                    TBlockChain tPre = lstBlockChain.ElementAt(i - 1);

                    curBlock = tCurrent.Block;
                    preBlock = tPre.Block;

                    // ----
                    if(!curBlock.DataHash.Equals(tCurrent.Hash_Block))
                        return false;

                    if (!preBlock.DataHash.Equals(tPre.Hash_Block))
                        return false;

                    if (!curBlock.PreHash.Equals(preBlock.DataHash))
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
