namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.Assembly
{
    using System.Collections.Generic;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;

    /// <summary>
    /// Интерфейс сервиса для компоновки лс в файлы квитанций
    /// </summary>
    public interface ICompositionService
    {
        /// <summary>
        /// Получить порцию лс
        /// </summary>
        /// <param name="accountData">Все лс</param>
        /// <param name="config">Настройки</param>
        /// <returns>Перечислимое множество порций</returns>
        IEnumerable<IEnumerable<PaymentDocumentSnapshot>> GetAccountPortion(
            List<PaymentDocumentSnapshot> accountData, 
            RegOperatorConfig config);

        /// <summary>
        /// Получить имя файла по порции
        /// </summary>
        /// <param name="portion">Порция лс</param>
        /// <returns>Наименование файла</returns>
        string GetFileName(IEnumerable<PaymentDocumentSnapshot> portion);
    }
}