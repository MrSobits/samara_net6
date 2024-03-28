namespace Bars.Gkh.Regions.Voronezh.ViewModels.Impl
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Dto;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.ViewModels;
    using Bars.Gkh.Utils;
    using Castle.Windsor;
    using Entities;
    using Enums;
    using Gkh.Domain;

    /// <summary>
    /// Представление для Л/С
    /// </summary>
    internal class VoronezhPersonalAccountDistributionViewModel : IPersonalAccountDistributionViewModel
    {
        private readonly IWindsorContainer container;

        private readonly IPersonalAccountFilterService filterService;
        private readonly IDomainService<AccountPaymentInfoSnapshot> accountSnapshotDomain;
        private readonly IDomainService<PaymentDocumentSnapshot> snapshotDomain;
        private readonly IDomainService<BasePersonalAccount> accountDomain;
        private readonly IDomainService<LawSuitDebtWorkSSP> pirSSP;
        private readonly IDomainService<LawSuitDebtWorkSSPDocument> pirSSPDocument;

        /// <summary>
        /// .ctor
        /// </summary>
        public VoronezhPersonalAccountDistributionViewModel(
            IWindsorContainer container,
            IPersonalAccountFilterService filterService,
            IDomainService<AccountPaymentInfoSnapshot> accountSnapshotDomain,
            IDomainService<PaymentDocumentSnapshot> snapshotDomain,
            IDomainService<LawSuitDebtWorkSSP> lawSuitDebtWorkSSPDomain,
            IDomainService<LawSuitDebtWorkSSPDocument> lawSuitDebtWorkSSPDocumentDomain,
        IDomainService<BasePersonalAccount> accountDomain)
        {
            this.filterService = filterService;
            this.accountSnapshotDomain = accountSnapshotDomain;
            this.snapshotDomain = snapshotDomain;
            this.accountDomain = accountDomain;
            this.container = container;
            this.pirSSP = lawSuitDebtWorkSSPDomain;
            this.pirSSPDocument = lawSuitDebtWorkSSPDocumentDomain;
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
                var dataByDocs = pirSSP.GetAll().Where(x => x.CbNumberDocument != "" && x.CbNumberDocument != null && x.LawsuitOwnerInfo != null && x.CbNumberDocument.ToLower().Contains(docnumber.ToLower()))
                    .Select(x => x.LawsuitOwnerInfo.PersonalAccount.Id).Distinct().ToList();

                dataByDocs.AddRange(pirSSPDocument.GetAll().Where(x => x.NumberString != "" && x.NumberString != null && x.NumberString.ToLower().Contains(docnumber.ToLower()))
                    .Select(x => x.LawSuitDebtWorkSSP.LawsuitOwnerInfo.PersonalAccount.Id).Distinct().ToList());

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