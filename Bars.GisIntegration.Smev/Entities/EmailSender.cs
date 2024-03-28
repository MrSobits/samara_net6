using Bars.B4.Application;
using Bars.B4.Config;
using Castle.Windsor;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Bars.B4.Modules.ESIA.OAuth20.Entities
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
            _container = ApplicationContext.Current.Container;
            var configProvider = _container.Resolve<IConfigProvider>();
            var config = configProvider.GetConfig().GetModuleConfig("Bars.B4.Modules.ESIA.OAuth20");
            MailLogEnabled = config.GetAs<bool>("MailLogEnabled",false,true);
            Login = config.GetAs<string>("MailLogin","",true);
            Password = config.GetAs<string>("MailPassword", "", true);
            SmtpHost = config.GetAs<string>("SmtpHost", "", true);
            SmtpPort = config.GetAs<int>("SmtpPort", 25, true);
            FromEmail = config.GetAs<string>("FromEmail", Login, true);
            ToEmail = config.GetAs<string>("ToEmail", "", true);
            FromText = config.GetAs<string>("FromText", "ESIA Exception Logger", true);
            UseSSL = true;
        }

        #endregion constructors

        #region public properties

        public IWindsorContainer _container { get; set; }

        public Boolean MailLogEnabled { get; private set; }

        public String Login { get; private set; }

        public String Password { get; private set; }

        public String SmtpHost { get; private set; }

        public Int32 SmtpPort { get; private set; }

        public String FromEmail { get; private set; }

        public String ToEmail { get; private set; }

        public String FromText { get; private set; }

        public Boolean UseSSL { get; private set; }

        #endregion public properties

        #region public methods

        public void TrySendIfLogEnabled(String subject, String messageBody)
        {
            if (MailLogEnabled)
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(FromEmail, FromText, Encoding.UTF8);
                message.To.Add(new MailAddress(ToEmail.Trim()));

                message.Subject = subject;
                message.SubjectEncoding = Encoding.UTF8;

                message.Body = messageBody.Replace("\r\n", "<br>");
                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;

                try
                {
                    SmtpClient client = new SmtpClient(SmtpHost, SmtpPort);
                    client.Credentials = new NetworkCredential(Login, Password);
                    client.EnableSsl = true;
                    client.Send(message);
                }
                catch
                {

                }
            }
        }

        #endregion public methods
    }
}
