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
                    curBlock = lstBlockChain.ElementAt(i).Block;
                    preBlock = lstBlockChain.ElementAt(i - 1).Block;

                    if (!preBlock.CurrentHash.Equals(curBlock.PreHash))
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
