namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Представление <see cref="ContractPeriodSummDetail" />
    /// </summary>
    public class ContractPeriodSummDetailViewModel : BaseViewModel<ContractPeriodSummDetail>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<ContractPeriodSummDetail> domainService, BaseParams baseParams)
        {
            var contractId = baseParams.Params.GetAs<long>("contractId");

            var loadParam = baseParams.GetLoadParam();

            var result = domainService.GetAll()
                .Where(x => x.ContractPeriodSumm.Id == contractId)
                .Select(
                    x =>
                        new DetailProxy
                        {
                            Id = x.Id,
                            Address = x.PublicServiceOrgContractRealObjInContract.RealityObject.Address,

                            ChargedManOrg = x.ChargedManOrg,
                            PaidManOrg = x.PaidManOrg,
                            SaldoOut = x.SaldoOut,

                            StartDebt = x.StartDebt,
                            ChargedResidents = x.ChargedResidents,
                            RecalcPrevPeriod = x.RecalcPrevPeriod,
                            ChangeSum = x.ChangeSum,
                            NoDeliverySum = x.NoDeliverySum,
                            PaidResidents = x.PaidResidents,
                            EndDebt = x.EndDebt,
                            ChargedToPay = x.ChargedToPay,
                            TransferredPubServOrg = x.TransferredPubServOrg
                        });

            var count = result.Count();

            return new ListDataResult(result.Order(loadParam).Paging(loadParam), count);
        }

        private class DetailProxy
        {
            public long Id { get; set; }

            public string Address { get; set; }

            public decimal ChargedManOrg { get; set; }

            public decimal PaidManOrg { get; set; }

            public decimal SaldoOut { get; set; }

            public decimal StartDebt { get; set; }

            public decimal ChargedResidents { get; set; }

            public decimal RecalcPrevPeriod { get; set; }

            public decimal ChangeSum { get; set; }

            public decimal NoDeliverySum { get; set; }

            public decimal PaidResidents { get; set; }

            public decimal EndDebt { get; set; }

            public decimal ChargedToPay { get; set; }

            public decimal TransferredPubServOrg { get; set; }
        }
    }
}