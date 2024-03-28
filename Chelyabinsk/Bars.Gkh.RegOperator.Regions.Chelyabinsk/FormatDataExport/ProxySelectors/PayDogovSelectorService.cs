namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.FormatDataExport.ProxySelectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.EntityExtensions;
    using Bars.GkhCr.Entities;
    using Bars.GkhRf.Enums;

    /// <summary>
    /// Сервис получения <see cref="PayDogovProxy"/> для Челябинска
    /// </summary>
    public class PayDogovSelectorService : BaseProxySelectorService<PayDogovProxy>
    {
        /// <inheritdoc />
        protected override ICollection<PayDogovProxy> GetAdditionalCache()
        {
            var transferCtrRepository = this.Container.ResolveRepository<TransferCtr>();

            using (this.Container.Using(transferCtrRepository))
            {
                return this.GetProxies(transferCtrRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, PayDogovProxy> GetCache()
        {
            var transferCtrRepository = this.Container.ResolveRepository<TransferCtr>();

            using (this.Container.Using(transferCtrRepository))
            {
                var objectCrQuery = this.FilterService.GetFiltredQuery<ObjectCr>();

                var performedWorkActPaymentkQuery = this.FilterService.ObjectCrIds.Any()
                    ? transferCtrRepository.GetAll()
                        .WhereContainsBulked(x => x.ObjectCr.Id, this.FilterService.ObjectCrIds)
                    : transferCtrRepository.GetAll()
                        .Where(x => objectCrQuery.Any(y => y == x.ObjectCr));

                return this.GetProxies(performedWorkActPaymentkQuery).ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<PayDogovProxy> GetProxies(IQueryable<TransferCtr> transferCtrQuery)
        {
            var buildContractRepository = this.Container.ResolveRepository<BuildContractTypeWork>();

            using (this.Container.Using(buildContractRepository))
            {
                return transferCtrQuery
                    .Join(buildContractRepository.GetAll(),
                        x => x.TypeWorkCr,
                        x => x.TypeWork,
                        (x, c) => new
                        {
                            x.Id,
                            ContractId = c.BuildContract.GetNullableId(),
                            x.PaymentType,
                            ContragentRecipient = c.BuildContract.Builder.Contragent.GetNullableId(),
                            ContragentPayer = c.BuildContract.Contragent.GetNullableId(),
                            x.PaymentDate,
                            x.PaidSum,
                            WorkId = c.Id
                        })
                    .AsEnumerable()
                    .Select(x => new PayDogovProxy
                    {
                        Id = x.Id,
                        ContractId = x.ContractId,
                        State = 1,
                        PaymentType = this.GetPaymentType(x.PaymentType),
                        ContragentRecipient = x.ContragentRecipient,
                        ContragentPayer = x.ContragentPayer,
                        PaymentDate = x.PaymentDate,
                        PaymentSum = x.PaidSum,

                        WorkId = x.WorkId,
                        OwnerSum = x.PaidSum
                    })
                    .ToList();
            }
        }

        private int? GetPaymentType(TypePaymentRfCtr? paymentRfCtr)
        {
            switch (paymentRfCtr)
            {
                case TypePaymentRfCtr.Prepayment:
                    return 1;
                case TypePaymentRfCtr.CrPayment:
                    return 2;
                default:
                    return null;
            }
        }
    }
}