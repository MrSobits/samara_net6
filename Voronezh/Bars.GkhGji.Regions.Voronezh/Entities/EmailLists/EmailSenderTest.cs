using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    public class EmailSenderTest
    {
        #region singleton

        public static EmailSenderTest Instance
        {
            get
            {
                if (_instance == null) _instance = new EmailSenderTest();
                return _instance;
            }
        }

        private static EmailSenderTest _instance = null;

        #endregion singleton

        #region constructors

        public EmailSenderTest()
        {
            //Login = "ugzhi@mail.ru";
            //Password = "A89zvB77kx";
            //Host = "smtp.mail.ru";
            //FromEmail = "ugzhi@mail.ru";
            //FromText = "ГУ ГЖИ Челябинской области";
            //IsUseSSL = true;
            //ugzhi@mail.ru  пароль: A89zvB77kx
            Login = @"aksionik@mail.ru";
            Password = "ltkjvfytckjdjv777";
            Host = "smtp.mail.ru";
            FromEmail = "aksionik@mail.ru";
            FromText = "ГЖИ Воронежской области";
            IsUseSSL = true;
            //Login = ConfigurationManager.AppSettings["EmailLogin"];
            //Password = ConfigurationManager.AppSettings["EmailPassword"];
            //Host = ConfigurationManager.AppSettings["EmailHost"];
            //FromEmail = ConfigurationManager.AppSettings["FromEmail"];
            //FromText = ConfigurationManager.AppSettings["FromText"];
            //IsUseSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsUseSSL"]);
        }

        #endregion constructors

        #region public properties

        public String Login { get; private set; }

        public String Password { get; private set; }

        public String Host { get; private set; }

        public String FromEmail { get; private set; }

        public String FromText { get; private set; }

        public Boolean IsUseSSL { get; private set; }

        #endregion public properties

        #region public methods

        public void Send(String toEmails, String subject, String messageBody, Attachment attachment)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(FromEmail, FromText, Encoding.UTF8);
            foreach (var email in toEmails.Split(',', ';'))
            {
                message.To.Add(new MailAddress(email.Trim()));
            }

            message.Body = messageBody.Replace("\r\n", "<br>");
            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;
            message.Subject = subject;

            if(attachment!=null)
                message.Attachments.Add(attachment);
            //AttachmentCollection ak = new AttachmentCollection();
            //Attachment att = new Attachment();
            //message.Attachments = new AttachmentCollection();

            message.SubjectEncoding = Encoding.UTF8;

            SmtpClient client = new SmtpClient("smtp.mail.ru", 25);
            client.Credentials = new NetworkCredential(@"aksionik@mail.ru", "ltkjvfytckjdjv777");
            client.EnableSsl = true;
            client.Send(message);
        }

        #endregion public methods
    }
}
