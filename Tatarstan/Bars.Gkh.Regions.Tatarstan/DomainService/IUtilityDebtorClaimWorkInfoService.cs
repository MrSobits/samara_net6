namespace Bars.Gkh.Regions.Tatarstan.DomainService
{
    using Bars.B4.Utils;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    /// <summary>
    /// Сервис заполнения информации о должнике ЖКУ
    /// </summary>
    public interface IUtilityDebtorClaimWorkInfoService
    {
        /// <summary>
        /// Получить информацию
        /// </summary>
        void GetInfo(BaseClaimWork claimWork, DynamicDictionary dictionary);
    }
}
