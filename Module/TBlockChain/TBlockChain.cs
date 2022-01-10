using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TBlockChain
{
    public class TBlockChain
    {
        private TBlock _block = null;
        public TBlock Block { get => _block; }

        private object _data = null;
        private string _preHash = string.Empty;
        private int _nonce = 0;

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
                long timeSpan = Global.TGlobal.ConvertDateTimeToInt64(DateTime.UtcNow);
                string valueHash = string.Format("{0} + {1} + {2} + {3}", _preHash, timeSpan, _data, _nonce);

                _block = new TBlock()
                {
                    Data = _data,
                    CurrentHash = TSHA256.TSHA256.THashSHA256(valueHash),
                    TimeSpan = timeSpan,
                    Nonce = _nonce,
                    PreHash = _preHash
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
