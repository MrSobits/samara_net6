namespace Bars.Gkh.Regions.Tatarstan.DomainService.Impl
{
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;

    using Castle.Windsor;

    /// <summary>
    /// Сервис заполнения информации о должнике ЖКУ
    /// </summary>
    public class UtilityDebtorClaimWorkInfoService : IUtilityDebtorClaimWorkInfoService
    {
        public IWindsorContainer Container { get; set; }

        public void GetInfo(BaseClaimWork claimWork, DynamicDictionary dictionary)
        {
            if (claimWork.ClaimWorkTypeBase != ClaimWorkTypeBase.UtilityDebtor)
            {
                return;
            }

            var debtorClaimDomain = this.Container.ResolveDomain<UtilityDebtorClaimWork>();

            try
            {
                var debtorClaimWork = debtorClaimDomain.Get(claimWork.Id);
                dictionary.Add("DebtSum", (debtorClaimWork.ChargeDebt ?? 0) + (debtorClaimWork.PenaltyDebt ?? 0));
                dictionary.Add("AccountOwner", debtorClaimWork.AccountOwner);
                dictionary.Add("OwnerType", debtorClaimWork.OwnerType);
            }  
            finally
            {
                this.Container.Release(debtorClaimDomain);
            }
        }
    }
}