namespace Bars.Gkh.Modules.ClaimWork.DomainService
{
    using B4;
    using B4.Utils;
    using Entities;

    /// <summary>
    /// Интерфейс для получения информации о должнике
    /// </summary>
    public interface IClaimWorkInfoService
    {
        /// <summary>
        /// Получить информацию
        /// </summary>
        void GetInfo(BaseClaimWork claimWork, DynamicDictionary resultDict);

        /// <summary>
        /// Получить данные для грида "Оплата задолженности"
        /// </summary>
        ListDataResult GetDebtPersAccPayments(BaseParams baseParams);
    }
}
