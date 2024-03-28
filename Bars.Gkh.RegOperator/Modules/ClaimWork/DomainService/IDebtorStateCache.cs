namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService
{
    using System.Collections.Generic;

    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Интерфейс кэша обновления состояний ПИР
    /// </summary>
    public interface IDebtorStateCache
    {
        /// <summary>
        /// Инициализировать кэш
        /// </summary>
        /// <param name="claimWorksIds">Основания ПИР</param>
        void Init(IEnumerable<long> claimWorksIds);

        /// <summary>
        /// Очистить
        /// </summary>
        void Clear();

        /// <summary>
        /// Получить типы созданных документов
        /// </summary>
        /// <param name="claimWork">Основание ПИР</param>
        IEnumerable<ClaimWorkDocumentType> GetCreatedDocTypes(BaseClaimWork claimWork);

        /// <summary>
        /// Существует ли документ ПИР
        /// </summary>
        /// <param name="claimWork">Основание ПИР</param>
        /// <param name="type">Тип документа</param>
        /// <returns></returns>
        bool DocumentClwExists(BaseClaimWork claimWork, ClaimWorkDocumentType type);

        /// <summary>
        /// Существует ли заявление о выдаче судебного приказа
        /// </summary>
        /// <param name="claimWork">Основание ПИР</param>
        /// <param name="type">Тип документа</param>
        /// <returns></returns>
        bool CourtOrderClaimExists(BaseClaimWork claimWork, ClaimWorkDocumentType type);

        /// <summary>
        /// Существует ли исковое заявление
        /// </summary>
        /// <param name="claimWork">Основание ПИР</param>
        /// <param name="type">Тип документа</param>
        /// <returns></returns>
        bool LawsuitExists(BaseClaimWork claimWork, ClaimWorkDocumentType type);

        /// <summary>
        /// Получить заявление о выдаче судебного приказа
        /// </summary>
        /// <param name="claimWork">Основание ПИР</param>
        /// <returns></returns>
        CourtOrderClaim GetCourtClaim(BaseClaimWork claimWork);
    }
}
