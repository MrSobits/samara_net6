namespace Bars.Gkh.RegOperator.Regions.Tatarstan.FormatDataExport.ProxySelectors.Impl
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

    /// <summary>
    /// Сервис получения <see cref="PayDogovProxy"/> для РТ
    /// </summary>
    public class PayDogovSelectorService : BaseProxySelectorService<PayDogovProxy>
    {
        /// <inheritdoc />
        protected override ICollection<PayDogovProxy> GetAdditionalCache()
        {
            var performedWorkActRepository = this.Container.ResolveRepository<PerformedWorkAct>();

            using (this.Container.Using(performedWorkActRepository))
            {
                return this.GetProxies(performedWorkActRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, PayDogovProxy> GetCache()
        {
            var performedWorkActRepository = this.Container.ResolveRepository<PerformedWorkAct>();

            using (this.Container.Using(performedWorkActRepository))
            {
                var objectCrQuery = this.FilterService.GetFiltredQuery<ObjectCr>();

                var performedWorkActPaymentkQuery = this.FilterService.ObjectCrIds.Any()
                    ? performedWorkActRepository.GetAll()
                        .WhereContainsBulked(x => x.ObjectCr.Id, this.FilterService.ObjectCrIds)
                    : performedWorkActRepository.GetAll()
                        .Where(x => objectCrQuery.Any(y => y == x.ObjectCr));

                return this.GetProxies(performedWorkActPaymentkQuery).ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<PayDogovProxy> GetProxies(IQueryable<PerformedWorkAct> performedWorkActQuery)
        {
            var buildContractRepository = this.Container.ResolveRepository<BuildContractTypeWork>();

            using (this.Container.Using(buildContractRepository))
            {
                return performedWorkActQuery
                    .Join(buildContractRepository.GetAll(),
                        x => x.TypeWorkCr,
                        x => x.TypeWork,
                        (x, c) => new
                        {
                            x.Id,
                            ContractId = c.BuildContract.GetNullableId(),
                            ContragentRecipient = c.BuildContract.Builder.Contragent.GetNullableId(),
                            ContragentPayer = c.BuildContract.Contragent.GetNullableId(),
                            x.DateFrom,
                            x.Sum,
                            WorkId = c.Id
                        })
                    .AsEnumerable()
                    .Select(x => new PayDogovProxy
                    {
                        Id = x.Id,
                        ContractId = x.ContractId,
                        State = 1,
                        PaymentType = 2,
                        ContragentRecipient = x.ContragentRecipient,
                        ContragentPayer = x.ContragentPayer,
                        PaymentDate = x.DateFrom,
                        PaymentSum = x.Sum,

                        WorkId = x.WorkId,
                        OwnerSum = x.Sum
                    })
                    .ToList();
            }
        }
    }
}