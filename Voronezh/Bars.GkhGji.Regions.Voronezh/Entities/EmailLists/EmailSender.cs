using Bars.B4.Application;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Bars.GkhGji.Regions.Voronezh.Entities
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
            //Login = "ugzhi@mail.ru";
            //Password = "A89zvB77kx";
            //Host = "smtp.mail.ru";
            //FromEmail = "ugzhi@mail.ru";
            //FromText = "ГУ ГЖИ Челябинской области";
            //IsUseSSL = true;
            //ugzhi@mail.ru  пароль: A89zvB77kx
            Login = @"vrn\gzhi";
            Password = "awsna4Ba23N7247N";
            Host = "mail.govvrn.ru";
            FromEmail = "gzhi@govvrn.ru";
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

            SmtpClient client = new SmtpClient("mail.govvrn.ru", 587);
            client.Credentials = new NetworkCredential(@"vrn\gzhi", "awsna4Ba23N7247N");
            client.EnableSsl = false;
            client.Send(message);
        }


        public void SendFKR(String toEmails, String subject, String messageBody, Attachment attachment)
        {
            var appSettings = ApplicationContext.Current.Configuration.AppSettings;
            var smtpClient = appSettings.GetAs<string>("smtpClient");
            var smtpPort = appSettings.GetAs<int>("smtpPort");
            var mailFrom = appSettings.GetAs<string>("smtpEmail");
            var smtpLogin = appSettings.GetAs<string>("smtpLogin");
            var smtpPassword = appSettings.GetAs<string>("smtpPassword");
            var enableSsl = appSettings.GetAs<bool>("enableSsl");
            var mailFromName = appSettings.GetAs<string>("mailFromName");

            MailAddress from = new MailAddress(mailFrom, mailFromName, Encoding.UTF8);
            MailAddress to = new MailAddress(toEmails);
            MailMessage message = new MailMessage(from, to);

            foreach (var email in toEmails.Split(',', ';'))
            {
                message.To.Add(new MailAddress(email.Trim()));
            }

            message.Body = messageBody.Replace("\r\n", "<br>");
            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;
            message.Subject = subject;
            if (attachment != null)
                message.Attachments.Add(attachment);
            //AttachmentCollection ak = new AttachmentCollection();
            //Attachment att = new Attachment();
            //message.Attachments = new AttachmentCollection();

            message.SubjectEncoding = Encoding.UTF8;

            SmtpClient smtp = new SmtpClient(smtpClient, smtpPort);
            try
            {
                smtp.Credentials = new NetworkCredential(smtpLogin, smtpPassword);
                smtp.EnableSsl = enableSsl;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
              throw new Exception("Не удалось отправить кведомление", ex);
            }
           
        }

        #endregion public methods
    }
}
