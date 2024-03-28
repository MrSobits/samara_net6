namespace Bars.Gkh.RegOperator.FormatDataExport.Domain.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.Utils;

    public class FormatDataExportAccountPaymentInfoSnapshotRepository : BaseFormatDataExportRepository<AccountPaymentInfoSnapshot>
    {
        /// <inheritdoc />
        public override IQueryable<AccountPaymentInfoSnapshot> GetQuery(IFormatDataExportFilterService filterService)
        {
            var viewAccOwnershipHistoryRepository = this.Container.Resolve<IViewAccOwnershipHistoryRepository>();
            using (this.Container.Using(viewAccOwnershipHistoryRepository))
            {
                var persAccQuery = viewAccOwnershipHistoryRepository.GetAllDto(filterService.PeriodId)
                    .WhereIfContainsBulked(filterService.PersAccIds.Any(), x => x.Id, filterService.PersAccIds)
                    .Filter(filterService.PersAccFilter, this.Container);

                return this.FilterByEditDate(this.Repository.GetAll(), filterService)
                    .Where(x => x.Snapshot.Period.Id == filterService.PeriodId)
                    .WhereNotEmptyString(x => x.Snapshot.PaymentReceiverAccount)
                    .Where(x => persAccQuery.Any(p => p.Id == x.AccountId));
            }
        }
    }
}