using HNBackend.Module.TExtension;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TSendGrid
{
    public class TSendGrid
    {
        private TSendGridData _sendGrid = null;
        private TSendGridAccount _sendGridAccount = null;

        public TSendGrid(TSendGridAccount sendGridAccount ,TSendGridData sendGrid)
        {
            _sendGridAccount = sendGridAccount;
            _sendGrid = sendGrid;
        }

        public string Send()
        {
            string response = string.Empty;
            try
            {
                SendGridClient client = new SendGridClient(_sendGridAccount.API_KEY);
                EmailAddress fAddress = null;
                EmailAddress tAddress = null;

                fAddress = new EmailAddress(_sendGridAccount.FROM_EMAIL_ADDRESS, _sendGrid.DisplayName);
                tAddress = new EmailAddress(_sendGrid.ToEmailAddress);

                var intEmail = MailHelper.CreateSingleEmail(fAddress, tAddress, _sendGrid.Subject, null, _sendGrid.Content);
                var attachments = GetAttachments();

                if (attachments != null && attachments.Count > 0)
                    intEmail.AddAttachments(attachments);

                var responseSend = client.SendEmailAsync(intEmail);
                if (responseSend != null)
                    response = responseSend.Result.TObjectToJson();

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Attachment> GetAttachments()
        {
            try
            {
                if (!string.IsNullOrEmpty(_sendGrid.Attachment))
                {
                    List<string> arrAttack = new List<string>();
                    arrAttack.AddRange(_sendGrid.Attachment.Split(new char[] { ';', '\n' }, StringSplitOptions.RemoveEmptyEntries));
                    if (arrAttack != null && arrAttack.Count > 0)
                    {
                        List<Attachment> arrAttachments = new List<Attachment>();
                        Attachment attachment = null;
                        foreach (var att in arrAttack)
                        {
                            if (File.Exists(att))
                            {
                                byte[] arrBytes = File.ReadAllBytes(att);
                                string convertToBase64 = string.Empty;
                                convertToBase64 = Convert.ToBase64String(arrBytes);
                                string strType = string.Empty;
                                strType = Path.GetExtension(att);
                                attachment = new Attachment()
                                {
                                    Filename = !string.IsNullOrEmpty(att) ? GetFileName(att) : string.Empty,
                                    Content = !string.IsNullOrEmpty(convertToBase64) ? convertToBase64 : string.Empty,
                                    Type = !string.IsNullOrEmpty(strType) ? strType : string.Empty,
                                    ContentId = HNBackend.Global.TGlobal.CreateGUID(HNBackend.Global.TGUID.TIME),
                                    Disposition = "inline"
                                };
                                arrAttachments.Add(attachment);
                            }
                        }

                        return arrAttachments;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetFileName(string path)
        {
            string fileName = string.Empty;
            try
            {
                fileName = Path.GetFileName(path);
            }
            catch (Exception ex)
            {
                fileName = "File_Name";
            }
            return fileName;
        }
    }
}
