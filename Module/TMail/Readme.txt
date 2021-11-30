-- Hướng dẫn call Funtion
1. Attach (nếu có)
 List<Attachment> attachment = new List<Attachment>();
                Attachment attMail = new Attachment("Full path file");
                attachment.Add(attMail);
2. Thông tin
                TMail mail = new TMail()
                {
                    Email = _USER_NAME,
                    Password = _PASS_WORD,
                    Port = _PORT,
                    HostMail = _HOST_GMAIL,
                    EnableSsl = true,
                    Subject = "TEST MAIL",
                    DisplayName = "HNBackend",
                    HtmlBody = "<h1> Hi </h1>",
                    ArrayToEmail = new List<string>() { },
                    ArrayCCEmail = new List<string>() { },
                    ArrayAttach = attachment,
                    MailPriority = MailPriority.High
                };
3. Send
                TimeSpan timeSend = new TimeSpan();
                TSendMail sendMail = new TSendMail(mail);
                sendMail.Send(ref timeSend);