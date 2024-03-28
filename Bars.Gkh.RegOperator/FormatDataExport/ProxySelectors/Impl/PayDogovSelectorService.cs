namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.EntityExtensions;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Сервис получения <see cref="PayDogovProxy"/>
    /// </summary>
    public class PayDogovSelectorService : BaseProxySelectorService<PayDogovProxy>
    {
        /// <inheritdoc />
        protected override ICollection<PayDogovProxy> GetAdditionalCache()
        {
            var performedWorkActPaymentkRepository = this.Container.ResolveRepository<PerformedWorkActPayment>();

            using (this.Container.Using(performedWorkActPaymentkRepository))
            {
                return this.GetProxies(performedWorkActPaymentkRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, PayDogovProxy> GetCache()
        {
            var performedWorkActPaymentkRepository = this.Container.ResolveRepository<PerformedWorkActPayment>();

            using (this.Container.Using(performedWorkActPaymentkRepository))
            {
                var objectCrQuery = this.FilterService.GetFiltredQuery<ObjectCr>();

                var performedWorkActPaymentkQuery = this.FilterService.ObjectCrIds.Any()
                    ? performedWorkActPaymentkRepository.GetAll()
                        .WhereContainsBulked(x => x.PerformedWorkAct.ObjectCr.Id, this.FilterService.ObjectCrIds)
                    : performedWorkActPaymentkRepository.GetAll()
                        .Where(x => objectCrQuery.Any(y => y == x.PerformedWorkAct.ObjectCr));

                return this.GetProxies(performedWorkActPaymentkQuery).ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<PayDogovProxy> GetProxies(IQueryable<PerformedWorkActPayment> performedWorkActPaymentQuery)
        {
            var buildContractRepository = this.Container.ResolveRepository<BuildContractTypeWork>();

            using (this.Container.Using(buildContractRepository))
            {
                return performedWorkActPaymentQuery
                    .Join(buildContractRepository.GetAll(),
                        x => x.PerformedWorkAct.TypeWorkCr,
                        x => x.TypeWork,
                        (x, c) => new
                        {
                            x.Id,
                            ContractId = c.BuildContract.GetNullableId(),
                            x.TypeActPayment,
                            ContragentRecipient = (long?) c.BuildContract.Builder.Contragent.ExportId,
                            ContragentPayer = (long?) c.BuildContract.Contragent.ExportId,
                            x.DatePayment,
                            x.Paid,
                            WorkId = c.Id
                        })
                    .AsEnumerable()
                    .Select(x => new PayDogovProxy
                    {
                        Id = x.Id,
                        ContractId = x.ContractId,
                        State = 1,
                        PaymentType = x.TypeActPayment == ActPaymentType.PrePayment ? 1 : 2,
                        ContragentRecipient = x.ContragentRecipient,
                        ContragentPayer = x.ContragentPayer,
                        PaymentDate = x.DatePayment,
                        PaymentSum = x.Paid,

                        WorkId = x.WorkId,
                        OwnerSum = x.Paid
                    })
                    .ToList();
            }
        }
    }
}