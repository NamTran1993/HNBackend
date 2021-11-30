using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TMail
{
    public class TMail
    {
        public string HostMail { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string DisplayName { get; set; }
        public List<Attachment> ArrayAttach { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public List<string> ArrayToEmail { get; set; }
        public List<string> ArrayCCEmail { get; set; }
        public MailPriority MailPriority { get; set; } = MailPriority.Low;
    }
}
