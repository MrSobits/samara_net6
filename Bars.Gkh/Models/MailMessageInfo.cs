namespace Bars.Gkh.Models
{
    /// <summary>
    /// Информация об электронном письме
    /// </summary>
    public class MailInfo
    {
        public MailInfo()
        {
        }

        public MailInfo(string recieverMailAddress, string mailTheme, string messageBody)
        {
            this.RecieverMailAddress = recieverMailAddress;
            this.MailTheme = mailTheme;
            this.MessageBody = messageBody;
        }
        
        /// <summary>
        /// Почта получателя
        /// </summary>
        public string RecieverMailAddress { get; set; }

        /// <summary>
        /// Тема письма
        /// </summary>
        public string MailTheme { get; set; }

        /// <summary>
        /// Тело письма
        /// </summary>
        public string MessageBody { get; set; }
    }
}