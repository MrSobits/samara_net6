namespace Bars.Gkh.RegOperator.Regions.Tatarstan.ViewModel
{
    using System.Linq;
    using B4;

    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Вьюха для счета начислений дома
    /// </summary>
    public class RealityObjectChargeAccountOperationViewModel : BaseViewModel<RealityObjectChargeAccountOperation>
    {
        /// <summary>
        /// Список по периодам
        /// </summary>
        /// <param name="domainService">Домен-сервис <see cref="RealityObjectChargeAccountOperation"/></param>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns></returns>
        public override IDataResult List(IDomainService<RealityObjectChargeAccountOperation> domainService, BaseParams baseParams)
        {

            var persAccPerSummary = Container.Resolve<IDomainService<PersonalAccountPeriodSummary>>();

            try
            {
                var accountId = baseParams.Params.GetAsId("accId");

                if (accountId == 0)
                {
                    accountId = baseParams.GetLoadParam().Filter.GetAsId("accId");
                }

                var loadParam = baseParams.GetLoadParam();

                loadParam.Order = loadParam.Order
                    .Concat(new OrderField[] {
                        new OrderField()
                        {
                            Name = "Date",
                            Asc = true
                        }})
                    .ToArray();

                var data =
                    domainService.GetAll()
                        .Where(x => x.Account.Id == accountId)
                        .Select(x => new
                        {
                            x.Id,
                            x.Account,
                            Period = x.Period.Name,
                            x.Date,
                            x.SaldoIn,
                            x.ChargedTotal,
                            x.ChargedPenalty,
                            x.PaidTotal,
                            x.PaidPenalty,
                            x.SaldoOut
                        })
                        .Filter(loadParam, this.Container);

                return new ListDataResult(data.Order(loadParam).Paging(loadParam), data.Count());
            }
            finally
            {
                Container.Release(persAccPerSummary);
            }
        }
    }
}