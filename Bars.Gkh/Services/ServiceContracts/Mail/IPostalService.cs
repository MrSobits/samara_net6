namespace Bars.Gkh.Services.ServiceContracts.Mail
{
    using System.Threading.Tasks;

    using Bars.Gkh.Models;

    /// <summary>
    /// Интерфейс почтового сервиса
    /// </summary>
    public interface IPostalService
    {
        /// <summary>
        /// Отправить письмо
        /// </summary>
        void Send(MailInfo mailInfo);
        
        /// <summary>
        /// Отправить письмо асинхронно
        /// </summary>
        Task SendAsync(MailInfo mailInfo);
    }
}