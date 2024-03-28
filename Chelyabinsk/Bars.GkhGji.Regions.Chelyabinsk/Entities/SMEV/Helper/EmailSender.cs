using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    public class EmailSender
    {
        #region singleton

        public static EmailSender Instance
        {
            get
            {
                if (_instance == null) _instance = new EmailSender();
                return _instance;
            }
        }

        private static EmailSender _instance = null;

        #endregion singleton

        #region constructors

        public EmailSender()
        {
            //ugzhi@mail.ru  пароль: A89zvB77kx
            Login = "og@gzhi.gov74.ru";
            Password = "ySp4mXJ0";
            Host = "mail2.gov74.ru";
            FromEmail = "og@gzhi.gov74.ru";
            FromText = "ГУ ГЖИ Челябинской области";
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

            SmtpClient client = new SmtpClient("mail2.gov74.ru", 25);
            client.Credentials = new NetworkCredential("og@gzhi.gov74.ru", "ySp4mXJ0");
            client.EnableSsl = true;
            client.Send(message);
        }

        #endregion public methods
    }
}
