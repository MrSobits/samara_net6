namespace Bars.Gkh.RegOperator.ViewModels.Impl
{
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.Gkh.RegOperator.Dto;

    using Castle.Windsor;
    using Domain.Extensions;
    using DomainModelServices;
    using Entities;
    using Entities.PersonalAccount.PayDoc;
    using Enums;
    using Gkh.Domain;

    /// <summary>
    /// Представление для Л/С
    /// </summary>
    public class PersonalAccountDistributionViewModel : IPersonalAccountDistributionViewModel
    {
        private readonly IWindsorContainer container;

        private readonly IPersonalAccountFilterService filterService;
        private readonly IDomainService<AccountPaymentInfoSnapshot> accountSnapshotDomain;
        private readonly IDomainService<PaymentDocumentSnapshot> snapshotDomain;
        private readonly IDomainService<BasePersonalAccount> accountDomain;

        /// <summary>
        /// .ctor
        /// </summary>
        public PersonalAccountDistributionViewModel(
            IWindsorContainer container,
            IPersonalAccountFilterService filterService,
            IDomainService<AccountPaymentInfoSnapshot> accountSnapshotDomain,
            IDomainService<PaymentDocumentSnapshot> snapshotDomain,
            IDomainService<BasePersonalAccount> accountDomain)
        {
            this.filterService = filterService;
            this.accountSnapshotDomain = accountSnapshotDomain;
            this.snapshotDomain = snapshotDomain;
            this.accountDomain = accountDomain;
            this.container = container;
        }

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        public ListDataResult List(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var snapshotId = baseParams.Params.GetAsId("snapshotId");

            var data = this.accountDomain.GetAll().ToDtoWithPaymentAccount()
                .FilterByBaseParams(baseParams, this.filterService);

            data = this.FilterSnapshot(data, snapshotId);

            var result = data.Filter(loadParams, this.container);

            return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), result.Count());
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