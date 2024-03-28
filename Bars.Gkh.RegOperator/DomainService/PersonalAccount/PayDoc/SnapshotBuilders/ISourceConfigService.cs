namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using System.Collections.Generic;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Интерфейс сервиса настроек источников для документа на оплату
    /// </summary>
    public interface ISourceConfigService
    {
        /// <summary>
        /// Получить дерево настроек
        /// </summary>
        /// <returns></returns>
        TreeNode GetConfigTree();

        /// <summary>
        /// Получить список кодов всех актуальных настроек определенного типа
        /// </summary>
        /// <param name="type">Тип</param>
        /// <returns>Список настроек</returns>
        List<string> GetConfigList(PaymentDocumentType type);

        /// <summary>
        /// Получение списка всех зарегистрированных источников
        /// </summary>
        /// <param name="type">Тип</param>
        /// <returns>Список зарегистрированных источников</returns>
        List<ISnapshotBuilder> GetSourceList(PaymentDocumentType type);
    }
}
