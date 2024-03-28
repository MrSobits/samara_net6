namespace Bars.GkhRf.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhRf.Entities;

    public class PaymentViewModel : BaseViewModel<Payment>
    {
        public override IDataResult List(IDomainService<Payment> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var data = Container.Resolve<IDomainService<ViewPayment>>().GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.Address,
                        Municipality = x.MunicipalityName,
                        x.ChargePopulationCr,
                        x.PaidPopulationCr,
                        x.ChargePopulationHireRf,
                        x.PaidPopulationHireRf,
                        x.ChargePopulationCr185,
                        x.PaidPopulationCr185,
                        x.ChargePopulationBldRepair,
                        x.PaidPopulationBldRepair
                    })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            data = loadParams.Order.Length == 0 ? data.Paging(loadParams) : data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}