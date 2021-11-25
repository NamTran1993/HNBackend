using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace HNBackend.Module.TTimer
{
    public abstract class TTimer
    {
        protected Timer _tmrTimer = null;

        protected TTimer(int interval)
        {
            _tmrTimer = new Timer(interval * 1000);
            _tmrTimer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            _tmrTimer.Enabled = true;
        }

        protected void Start()
        {
            try
            {
                if (_tmrTimer != null)
                    _tmrTimer.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public abstract void Timer_Elapsed(object source, ElapsedEventArgs e);
    }
}
