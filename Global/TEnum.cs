using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Global
{
    public enum TMETHOD : int
    {
        GET,
        POST
    }


    public enum TYPE_LOGGER : int
    {
        ERROR = -1,
        WARNING,
        DEBUG,
        NORMAL
    }

    public enum TGUID : int
    {
        DEFAULT,
        DEFAULT_2,
        DEFAULT_3,
        DEFAULT_4,
        DATE,
        TIME
    }

    public enum TTHREAD
    {
        NONE,
        STOP,
        START,
        PAUSE,
        RESUME,
        RUNNING
    }

    public enum TCODE
    {
        NONE = -1,
        OK = 1,
        ERROR = 2
    }

    public enum TENCODE
    {
        DES,
        TDES,
        RSA,
        AES_256,
        TWOFISH
    }
}
