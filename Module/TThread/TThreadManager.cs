using HNBackend.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HNBackend.Module.TThread
{
    public abstract class TThreadManager
    {
        private string _LOG_FILE = "TThreadManager.log";

        private TTHREAD _status = TTHREAD.NONE;
        private int _timeInterval = 0;
        private string _sessionID = string.Empty;
        private Dictionary<string, TTHREAD> _lstThreadManager = null;

        public string SessionID { get => _sessionID; set => _sessionID = value; }

        protected TThreadManager(int timeInterval)
        {
            _lstThreadManager = new Dictionary<string, TTHREAD>();
            _status = TTHREAD.STOP;
            _timeInterval = timeInterval;
        }

        public void StartChecking()
        {
            _status = TTHREAD.START;
            _sessionID = TGlobal.CreateGUID();
            _lstThreadManager.Add(_sessionID, _status);

            try
            {
                if (_status == TTHREAD.STOP)
                    return;

                DoChecking();
            }
            catch (Exception ex)
            {
                TGlobal.Log(_LOG_FILE, "[StartChecking] Exception: " + ex.Message + "\r\n" + ex.StackTrace, TYPE_LOGGER.ERROR);
            }
        }

        protected abstract void MainProcessing();

        protected void SleepTime(int miliseconds)
        {
            try
            {
                int oneSecond = 1 * 1000;
                if (miliseconds < oneSecond)
                {
                    Thread.Sleep(miliseconds);
                    return;
                }

                int delayTime = 0;
                while (_status == TTHREAD.START)
                {
                    Thread.Sleep(oneSecond);
                    delayTime += oneSecond;
                    if (delayTime >= miliseconds)
                        break;
                }
            }
            catch (Exception ex)
            {
                TGlobal.Log(_LOG_FILE, "[SleepTime] Exception: " + ex.Message + "\r\n" + ex.StackTrace, TYPE_LOGGER.ERROR);
            }
        }


        public void StopChecking(string sessionID)
        {
            try
            {
                int idx = FindIndexThread(sessionID);
                if (idx > -1)
                {
                    _lstThreadManager.Remove(sessionID);
                    _lstThreadManager.Add(sessionID, TTHREAD.STOP);
                }
            }
            catch (Exception ex)
            {
                TGlobal.Log(_LOG_FILE, "[StopChecking ] Exception: " + ex.Message + "\r\n" + ex.StackTrace, TYPE_LOGGER.ERROR);
            }
        }

        private void DoChecking()
        {
            try
            {
                int waitTime = _timeInterval;
                if (waitTime < 1000)
                    waitTime = waitTime * 1000;

                GetLiveStatus:
                if (_lstThreadManager != null && _lstThreadManager.Count > 0)
                {
                    foreach (var session in _lstThreadManager)
                    {
                        var status = session.Value;
                        if (status != TTHREAD.START)
                            return;

                        Pause:
                        if (status == TTHREAD.PAUSE)
                        {
                            Thread.Sleep(1000);
                            goto Pause;
                        }

                        while (status == TTHREAD.START)
                        {
                            MainProcessing();
                            SleepTime(waitTime);

                            goto GetLiveStatus;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TGlobal.Log(_LOG_FILE, "[DoChecking] Exception: " + ex.Message + "\r\n" + ex.StackTrace, TYPE_LOGGER.ERROR);
            }
        }

        private int FindIndexThread(string sessionID)
        {
            try
            {
                if (_lstThreadManager != null && _lstThreadManager.Count > 0)
                {
                    int idx = 0;
                    foreach (var ss in _lstThreadManager)
                    {
                        if (ss.Key == sessionID)
                            return idx;
                        idx++;
                    }
                    return idx;
                }
            }
            catch (Exception ex)
            {
                TGlobal.Log(_LOG_FILE, "[FindIndexThread ] Exception: " + ex.Message + "\r\n" + ex.StackTrace, TYPE_LOGGER.ERROR);
            }
            return -1;
        }
    }
}
