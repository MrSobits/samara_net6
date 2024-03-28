namespace Bars.Gkh.Modules.ClaimWork.DomainService
{
    using System.Collections.Generic;
    using B4;
    using Entities;
    using Modules.ClaimWork.Enums;

    /// <summary>
    /// Данный интерфейс отвечает за формирование документа ПИР
    /// </summary>
    public interface IClaimWorkDocRule
    {
        /// <summary>
        /// Идентификатор реализации
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Краткое описание
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Карточка, которую нужно открыть после создания дкоумента
        /// </summary>
        string ActionUrl { get; }

        /// <summary>
        /// Тип документа результата, то есть того который должен получиться в результате формирования
        /// </summary>
        ClaimWorkDocumentType ResultTypeDocument { get; }

        /// <summary>
        /// Получение параметров котоыре переданы с клиента
        /// </summary>
        void SetParams(BaseParams baseParams);

        /// <summary>
        /// Метод формирования документа
        /// </summary>
        IDataResult CreateDocument(BaseClaimWork claimWork);

        /// <summary>
        /// Метод массового формирования документа
        /// </summary>
        IDataResult CreateDocument(IEnumerable<BaseClaimWork> claimWorks);

        /// <summary>
        /// Сформировать документы
        /// </summary>
        /// <param name="claimWorks">ПИР</param>
        /// <param name="fillDebts">Заполнять ли суммы</param>
        /// <returns>Коллекция созданных документов</returns>
        IEnumerable<DocumentClw> FormDocument(IEnumerable<BaseClaimWork> claimWorks, bool fillDebts = true);

        /// <summary>
        /// проверка валидности правила
        /// Например перед выполнением действия требуется проверить
        /// Можно ли формировать какой-то документ, например если по документу есть уже созданные 
        /// то можно не давать создавать новые (если требуется по процессу)
        /// </summary>
        IDataResult ValidationRule(BaseClaimWork claimWork);
    }
}