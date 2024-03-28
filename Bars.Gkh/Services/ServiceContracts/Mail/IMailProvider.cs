namespace Bars.Gkh.Services.ServiceContracts.Mail
{
    using Bars.B4;
    using Bars.Gkh.Models;

    /// <summary>
    /// Провайдер электроной почты
    /// </summary>
    /// <typeparam name="T">Данные для письма</typeparam>
    public interface IMailProvider<T> where T : class
    {
        /// <summary>
        /// Подготовить данные для письма
        /// </summary>
        T PrepareData(BaseParams baseParams);
        
        /// <summary>
        /// Подготовить сообщение в письме
        /// </summary>
        string PrepareMessage(T mailData);

        /// <summary>
        /// Отправить письмо
        /// </summary>
        void SendMessage(MailInfo mailInfo);
    }
}