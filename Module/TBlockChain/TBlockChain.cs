using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TBlockChain
{
    public class TBlockChain
    {
        private string _hash_Block = string.Empty;
        private TBlock _block = null;
        private object _data = null;
        private string _preHash = string.Empty;
        private int _nonce = 0;

        public TBlock Block { get => _block; }
        public object Data { get => _data; set => _data = value; }
        public string PreHash { get => _preHash; set => _preHash = value; }
        public int Nonce { get => _nonce; set => _nonce = value; }
        public string Hash_Block { get => _hash_Block; set => _hash_Block = value; }

        public TBlockChain(string preHash, object data, int nonce)
        {
            _preHash = preHash;
            _data = data;
            _nonce = nonce;

            TInitBlock();
            TCreateBlock();
        }


        private void TInitBlock()
        {
            try
            {
                if (_data == null)
                    throw new Exception("Data of Block is null.");

                if (string.IsNullOrEmpty(_preHash))
                    throw new Exception("Previous Hash Is Null Or Empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TCreateBlock()
        {
            try
            {
                string valueHash = string.Format("{0} + {1} + {2}", PreHash, _data, Nonce);

                _block = new TBlock()
                {
                    Data = _data,
                    DataHash = TSHA256.TSHA256.THashSHA256(valueHash),
                    Nonce = Nonce,
                    PreHash = PreHash
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
