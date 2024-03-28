namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using B4;
    using Entities;
    using System;
    using System.Linq;

    public class GisGmpViewModel : BaseViewModel<GisGmp>
    {
        public override IDataResult List(IDomainService<GisGmp> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var paymentsContainer = this.Container.Resolve<IDomainService<PayReg>>();

            var payments = paymentsContainer.GetAll()
                .Where(x => x.GisGmp != null)
                .GroupBy(x => x.GisGmp.Id)
                .Select(x => new
                {
                    x.Key,
                    Sum = (decimal)x.Sum(y => y.Amount)
                }).ToDictionary(x => x.Key, x => x.Sum);

            var data = domainService.GetAll()
               .AsEnumerable()
                .Select(x=> new
                {
                    x.Id,
                    RequestDate = x.ObjectCreateDate,
                    x.GisGmpPaymentsType,
                    Inspector = x.Inspector.Fio,
                    x.RequestState,
                    x.AltPayerIdentifier,
                    x.UIN,
                    x.BillFor,
                    x.TotalAmount,
                    PaymentsAmount = payments.ContainsKey(x.Id)? payments[x.Id]:0,
                    x.GisGmpChargeType,
                    x.MessageId, 
                    x.CalcDate,
                }).AsQueryable()
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


     
    }
}
