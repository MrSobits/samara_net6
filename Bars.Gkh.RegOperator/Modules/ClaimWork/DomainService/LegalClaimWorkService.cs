namespace Bars.Gkh.Modules.ClaimWork.DomainService.Impl
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Utils;
    using Gkh.RegOperator.Entities;

    /// <summary>
    /// Сервис по работе с основаниями претензионно-исковой работы для неплательщиков
    /// </summary>
    public class LegalClaimWorkService : DebtorClaimWorkService<LegalClaimWork>
    {
        private readonly IUserIdentity userIdentity;

        public IGkhUserManager UserManager { get; set; }

        public IDomainService<ContragentClw> ContragentClwDomain { get; set; }
        public IDomainService<ContragentClwMunicipality> ContragentClwMunicipalityDomain { get; set; }

        public LegalClaimWorkService(IUserIdentity userIdentity)
        {
            this.userIdentity = userIdentity;
        }

        /// <summary>
        /// Вернуть список ПИР
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <param name="isPaging">Флаг пагинации</param>
        /// <param name="totalCount">Общее количество записей</param>
        /// <returns>Результат запроса</returns>
        public override IList GetList(BaseParams baseParams, bool isPaging, ref int totalCount)
        {
            var loadParam = baseParams.GetLoadParam();

            var config = this.Container.GetGkhConfig<DebtorClaimWorkConfig>();
            var domainService = this.Container.ResolveDomain<LegalClaimWork>();

            try
            {
                List<long> MUsIDs = new List<long>();
                var Contragent = this.UserManager.GetActiveOperator()?.Contragent;
                ContragentClw contragentClw = null;
                if (Contragent != null)
                {
                    contragentClw = ContragentClwDomain.GetAll().Where(x => x.Contragent == Contragent).FirstOrDefault();
                    if (contragentClw != null)
                    {
                        MUsIDs = ContragentClwMunicipalityDomain.GetAll().Where(x => x.ContragentClw == contragentClw).Select(x => x.Municipality.Id).ToList();
                    }
                }

                var configfilterByUser = config.General.FilterByUser;

                var data = domainService.GetAll()
                    .WhereIf(configfilterByUser, x => x.User.Id == this.userIdentity.UserId)
                    .WhereIf(contragentClw != null, x => MUsIDs.Contains(((LegalAccountOwner)x.AccountOwner).Contragent.Municipality.Id)
                        && x.ObjectCreateDate >= contragentClw.DateFrom
                        && x.ObjectCreateDate <= contragentClw.DateTo)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.State,
                            x.DebtorState,
                            Municipality = ((LegalAccountOwner)x.AccountOwner).Contragent.Municipality.Name,
                            AccountOwnerName = ((LegalAccountOwner)x.AccountOwner).Contragent.Name,
                            ((LegalAccountOwner)x.AccountOwner).Contragent.JuridicalAddress,
                            ((LegalAccountOwner)x.AccountOwner).Contragent.Inn,
                            ((LegalAccountOwner)x.AccountOwner).Contragent.Kpp,
                            ContragentState = (ContragentState?)((LegalAccountOwner)x.AccountOwner).Contragent.ContragentState,
                            AccountsNumber = x.AccountDetails.Count(),
                            x.CurrChargeBaseTariffDebt,
                            x.CurrChargeDecisionTariffDebt,
                            x.CurrChargeDebt,
                            x.CurrPenaltyDebt,
                            x.IsDebtPaid,
                            x.DebtPaidDate,
                            UserName = x.User.Name
                        })
                    .Filter(loadParam, this.Container);

                totalCount = data.Count();

                data = isPaging ? data.Order(loadParam).Paging(loadParam) : data.Order(loadParam);

                return data.ToList();
            }
            finally
            {
                this.Container.Release(domainService);
                this.Container.Release(config);
            }
        }

        /// <inheritdoc />
        public override IDataResult UpdateStates(BaseParams baseParams, bool inTask = false)
        {
            if (inTask)
            {
                var debtorType = DebtorType.Legal;
                baseParams.Params["debtorType"] = debtorType;
                return this.CreateTask(baseParams);
            }

            return this.DebtorClaimWorkUpdateService.UpdateStates(baseParams);
        }
    }
}