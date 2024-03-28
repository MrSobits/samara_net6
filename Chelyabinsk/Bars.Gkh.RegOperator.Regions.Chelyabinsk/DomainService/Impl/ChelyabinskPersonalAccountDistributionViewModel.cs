namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.ViewModels.Impl
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.Dto;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.ViewModels;
    using Bars.Gkh.Utils;
    using Castle.Windsor;
    using DomainModelServices;
    using Entities;
    using Enums;
    using Gkh.Domain;

    /// <summary>
    /// Представление для Л/С
    /// </summary>
    internal class ChelyabinskPersonalAccountDistributionViewModel : IPersonalAccountDistributionViewModel
    {
        private readonly IWindsorContainer container;

        private readonly IPersonalAccountFilterService filterService;
        private readonly IDomainService<AccountPaymentInfoSnapshot> accountSnapshotDomain;
        private readonly IDomainService<PaymentDocumentSnapshot> snapshotDomain;
        private readonly IDomainService<BasePersonalAccount> accountDomain;
        private readonly IDomainService<AgentPIRDocument> agentPirDocDomain;
        private readonly IDomainService<LawSuitDebtWorkSSP> pirSSPDocument;

        /// <summary>
        /// .ctor
        /// </summary>
        public ChelyabinskPersonalAccountDistributionViewModel(
            IWindsorContainer container,
            IPersonalAccountFilterService filterService,
            IDomainService<AccountPaymentInfoSnapshot> accountSnapshotDomain,
            IDomainService<PaymentDocumentSnapshot> snapshotDomain,
            IDomainService<AgentPIRDocument> agentPIRDocumentDomain,
            IDomainService<LawSuitDebtWorkSSP> lawSuitDebtWorkSSPDomain,
            IDomainService<LawSuitDebtWorkSSPDocument> lawSuitDebtWorkSSPDocumentDomain,
        IDomainService<BasePersonalAccount> accountDomain)
        {
            this.filterService = filterService;
            this.accountSnapshotDomain = accountSnapshotDomain;
            this.snapshotDomain = snapshotDomain;
            this.accountDomain = accountDomain;
            this.agentPirDocDomain = agentPIRDocumentDomain;
            this.container = container;
            this.pirSSPDocument = lawSuitDebtWorkSSPDomain;
        }

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        public ListDataResult List(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var snapshotId = baseParams.Params.GetAsId("snapshotId");

            var docnumber = baseParams.Params.GetAs<string>("rospNumber");

            if (!string.IsNullOrEmpty(docnumber))
            {
                var dataByDocs = agentPirDocDomain.GetAll().Where(x => (x.DocumentType == Gkh.Enums.AgentPIRDocumentType.ApplicationROSP
                || x.DocumentType == Gkh.Enums.AgentPIRDocumentType.Resolution
                || x.DocumentType == Gkh.Enums.AgentPIRDocumentType.SSPList) && x.Number.ToLower().Contains(docnumber.ToLower()))
                    .Select(x => x.AgentPIRDebtor.BasePersonalAccount.Id).Distinct().ToList();

                dataByDocs.AddRange(pirSSPDocument.GetAll().Where(x => x.CbNumberDocument != "" && x.CbNumberDocument != null && x.LawsuitOwnerInfo != null && x.CbNumberDocument.ToLower().Contains(docnumber.ToLower()))
                    .Select(x => x.LawsuitOwnerInfo.PersonalAccount.Id).Distinct().ToList());

                if (dataByDocs.Count > 0)
                {
                    var data = this.accountDomain.GetAll().Where(x=> dataByDocs.Contains(x.Id))
                        .ToDtoWithPaymentAccount()
                      .FilterByBaseParams(baseParams, this.filterService);

                    data = this.FilterSnapshot(data, snapshotId);

                    var result = data.Filter(loadParams, this.container);

                    return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), result.Count());
                }

                return new ListDataResult();
            }
            else
            {
                var data = this.accountDomain.GetAll().ToDtoWithPaymentAccount()
               .FilterByBaseParams(baseParams, this.filterService);

                data = this.FilterSnapshot(data, snapshotId);

                var result = data.Filter(loadParams, this.container);

                return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), result.Count());
            }



           
        }

        private IQueryable<PersonalAccountWithPaymentAccountDto> FilterSnapshot(IQueryable<PersonalAccountWithPaymentAccountDto> query, long snapshotId)
        {
            if (snapshotId < 1)
            {
                return query;
            }

            var snapshot = this.snapshotDomain.Get(snapshotId);
            var accountInfos = this.accountSnapshotDomain.GetAll()
                .Where(x => x.Snapshot.Id == snapshotId)
                .ToArray();

            if (snapshot == null || accountInfos.IsEmpty())
            {
                return query;
            }

            var accIds = accountInfos
                .Select(x => x.AccountId)
                .ToArray();

            var persAccQuery = this.accountDomain.GetAll()
                .Where(x => x.AccountOwner.OwnerType == snapshot.OwnerType)
                .Where(x => accIds.Contains(x.Id));

            switch (snapshot.OwnerType)
            {
                case PersonalAccountOwnerType.Individual:
                    query = query
                        //не слушать решарпер и не менять сравнивание
                        .Where(x => x.AccountOwner.ToLower() == snapshot.Payer.ToLower());
                    break;
                case PersonalAccountOwnerType.Legal:
                    query = query
                        .Where(x => x.OwnerId == snapshot.HolderId);
                    break;
            }

            return query.Where(z => persAccQuery.Any(x => x.Id == z.Id));
        }
    }
}