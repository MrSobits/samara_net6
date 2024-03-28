namespace Bars.Gkh.RegOperator.FormatDataExport.Domain.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.Utils;

    public class FormatDataExportPersonalAccountPaymentTransferRepository : BaseFormatDataExportRepository<PersonalAccountPaymentTransfer>
    {
        /// <inheritdoc />
        public override IQueryable<PersonalAccountPaymentTransfer> GetQuery(IFormatDataExportFilterService filterService)
        {
            var viewAccOwnershipHistoryRepository = this.Container.Resolve<IViewAccOwnershipHistoryRepository>();
            var transferDomainService = this.Container.Resolve<ITransferDomainService>();
            using (this.Container.Using(viewAccOwnershipHistoryRepository, transferDomainService))
            {
                var persAccQuery = viewAccOwnershipHistoryRepository.GetAllDto(filterService.PeriodId)
                    .WhereIfContainsBulked(filterService.PersAccIds.Count > 0, x => x.Id, filterService.PersAccIds)
                    .WhereIf(filterService.UseIncremental, x => filterService.StartEditDate < x.ObjectEditDate)
                    .WhereIf(filterService.UseIncremental, x => x.ObjectEditDate < filterService.EndEditDate)
                    .Filter(filterService.PersAccFilter, this.Container);

                return transferDomainService.GetAll<PersonalAccountPaymentTransfer>()
                    .Where(x => persAccQuery.Any(y => y.Id == x.Owner.Id))
                    .Where(x => x.IsAffect)
                    .Where(x => x.ChargePeriod.Id == filterService.PeriodId)
                    .Where(x => x.Reason.StartsWith("Оплата") || x.Reason.StartsWith("Отмена оплаты"));
            }
        }
    }
}