using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TMail
{
    public class TSendMail
    {
        private TMail _tMail = null;
        private SmtpClient _smtp = null;

        public TSendMail(TMail tMail)
        {
            _tMail = tMail;
            InitMail();
        }

        public void Send (ref TimeSpan tpSend)
        {
            try
            {
                if (_tMail == null)
                    throw new Exception("TMail is null.");

                DateTime dtBefore = DateTime.Now;

                MailMessage mMessage = new MailMessage()
                {
                    From = new MailAddress(_tMail.Email, _tMail.DisplayName),
                    Subject = _tMail.Subject
                };

                if (_tMail.ArrayToEmail != null && _tMail.ArrayToEmail.Count > 0)
                {
                    foreach(var to in _tMail.ArrayToEmail)
                        mMessage.To.Add(to);
                }

                if (_tMail.ArrayCCEmail != null && _tMail.ArrayCCEmail.Count > 0)
                {
                    foreach (var cc in _tMail.ArrayCCEmail)
                        mMessage.CC.Add(cc);
                }

                if (!string.IsNullOrEmpty(_tMail.HtmlBody))
                {
                    mMessage.Body = _tMail.HtmlBody;
                    mMessage.IsBodyHtml = true;
                }

                if (_tMail.ArrayAttach != null && _tMail.ArrayAttach.Count > 0)
                {
                    foreach (var att in _tMail.ArrayAttach)
                        mMessage.Attachments.Add(att);
                }

                mMessage.Priority = _tMail.MailPriority;

                _smtp.Send(mMessage);

                tpSend = DateTime.Now - dtBefore;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitMail()
        {
            try
            {
                _smtp = new SmtpClient(_tMail.HostMail, _tMail.Port);
                _smtp.EnableSsl = _tMail.EnableSsl;
                _smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                _smtp.UseDefaultCredentials = true;
                _smtp.Credentials = new NetworkCredential(_tMail.Email, _tMail.Password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
