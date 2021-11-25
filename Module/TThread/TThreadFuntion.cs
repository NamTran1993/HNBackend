using HNBackend.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HNBackend.Module.TThread
{
    public abstract class TThreadFuntion
    {
        private string _LOG_FILE = "TThreadFuntion.log";

        private Thread _worker = null;

        private string _threadID = string.Empty;
        private bool _workerFree = false;
        private object _dataInfo = null;
        private bool _pause = false;
        private bool _locked = false;
        private bool _finishTask = false;
        private bool _stopNow = false;

        protected string ThreadID { get => _threadID; set => _threadID = value; }
        protected bool WorkerFree { get => _workerFree; set => _workerFree = value; }
        protected object DataInfo { get => _dataInfo; set => _dataInfo = value; }
        protected bool Pause { get => _pause; set => _pause = value; }
        protected bool Locked { get => _locked; set => _locked = value; }
        protected bool FinishTask { get => _finishTask; set => _finishTask = value; }
        protected bool StopNow { get => _stopNow; set => _stopNow = value; }
        public Thread Worker { get => _worker; set => _worker = value; }

        public void MainFuntion()
        {
            try
            {
                WaitingData();
                _workerFree = true;
            }
            catch (Exception ex)
            {
                TGlobal.Log(_LOG_FILE, "[MainFuntion] Exception: " + ex.Message + "\r\n" + ex.StackTrace, TYPE_LOGGER.ERROR);
            }
        }

        protected void StartWorker()
        {
            try
            {
                _workerFree = true;
                _worker = new Thread(new ThreadStart(MainFuntion));
                _worker.IsBackground = true;
                _worker.Start();
            }
            catch (Exception ex)
            {
                TGlobal.Log(_LOG_FILE, "[StartWorker] Exception: " + ex.Message + "\r\n" + ex.StackTrace, TYPE_LOGGER.ERROR);
            }
        }

        protected void StopWorker()
        {
            try
            {
                if (Worker != null)
                {
                    try
                    {
                        Worker.Abort();
                        Worker.Join();
                        Worker = null;
                        _stopNow = true;
                    }
                    catch (ThreadAbortException ex)
                    {
                        TGlobal.Log(_LOG_FILE, "[StopWorker] Exception: " + ex.Message + "\r\n" + ex.StackTrace, TYPE_LOGGER.WARNING);
                    }
                }
            }
            catch (Exception ex)
            {
                TGlobal.Log(_LOG_FILE, "[StopWorker] Exception: " + ex.Message + "\r\n" + ex.StackTrace, TYPE_LOGGER.ERROR);
            }
        }

        protected void WaitingData()
        {
            try
            {
                while (_pause)
                    Thread.Sleep(100);

                if (_stopNow)
                    return;

                if (_dataInfo == null)
                {
                    _pause = true;
                    _workerFree = true;
                    return;
                }
                else
                {
                    ExcuteTask();
                    _workerFree = false;
                }
            }
            catch (Exception ex)
            {
                TGlobal.Log(_LOG_FILE, "[WaitingData] Exception: " + ex.Message + "\r\n" + ex.StackTrace, TYPE_LOGGER.ERROR);
            }
        }

        protected void SleepTime(int miliseconds, TTHREAD status)
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
                while (status == TTHREAD.RUNNING)
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

        protected TTHREAD GetStatus()
        {
            try
            {
                if (_worker != null)
                {
                    switch (_worker.ThreadState)
                    {
                        case ThreadState.Running:
                            return TTHREAD.RUNNING;
                        case ThreadState.Stopped:
                            return TTHREAD.STOP;
                        default:
                            return TTHREAD.NONE;
                    }
                }
            }
            catch (Exception ex)
            {
                TGlobal.Log(_LOG_FILE, "[GetStatus] Exception: " + ex.Message + "\r\n" + ex.StackTrace, TYPE_LOGGER.ERROR);
            }

            return TTHREAD.NONE;
        }

        protected abstract void ExcuteTask();

        protected TThreadFuntion(string threadID, object info)
        {
            _dataInfo = info;
            _threadID = threadID;

            _workerFree = false;
            _pause = false;
            _locked = false;
            _finishTask = false;
            _stopNow = false;
        }
    }
}
