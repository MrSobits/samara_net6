namespace Bars.GkhDi.DomainService.Service
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities.Service;

    public class TariffForConsumersOtherServiceViewModel : BaseViewModel<TariffForConsumersOtherService>
    {
        public override IDataResult List(IDomainService<TariffForConsumersOtherService> domainService, BaseParams baseParams)
        {
            var otherServiceId = baseParams.Params.GetAs<long>("otherServiceId");

            return domainService.GetAll()
                .Where(x => x.OtherService.Id == otherServiceId)
                .Select(x => new
                {
                    x.Id,
                    x.DateStart,
                    x.DateEnd,
                    x.TariffIsSetFor,
                    x.TypeOrganSetTariffDi,
                    x.Cost
                })
                .ToListDataResult(this.GetLoadParam(baseParams), this.Container);
        }
    }
}
