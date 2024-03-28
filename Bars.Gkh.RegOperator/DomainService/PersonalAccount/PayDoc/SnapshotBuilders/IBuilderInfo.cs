namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using System.Collections.Generic;

    /// <summary>
    /// Информация об источнике данных документа оплаты
    /// </summary>
    public interface IBuilderInfo
    {
        /// <summary>
        /// Код источника
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Описание заполняемых полей 
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Получение детализированных дочерних источников
        /// </summary>
        /// <returns></returns>
        IEnumerable<IBuilderInfo> GetChildren();
    }
}
