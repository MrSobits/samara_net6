namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;

    public class RealityObjectSupplierAccountOperationViewModel : BaseViewModel<RealityObjectSupplierAccountOperation>
    {
        public IDomainService<RealObjSupplierAccOperPerfAct> RealObjSupplierAccOperPerfActDomain { get; set; }

        public override IDataResult List(IDomainService<RealityObjectSupplierAccountOperation> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var accountId = loadParams.Filter.GetAs<long>("accId");

            var typeWorkByOperId =
                RealObjSupplierAccOperPerfActDomain.GetAll()
                    .Where(x => x.SupplierAccOperation.Account.Id == accountId)
                    .Select(x => new
                    {
                        x.SupplierAccOperation.Id,
                        x.PerformedWorkAct.TypeWorkCr.Work.Name,
                        StateName = x.PerformedWorkAct.State.Name
                    })
                    .ToDictionary(x => x.Id, y => new { y.Name, y.StateName });

            var data = domainService.GetAll()
                .Where(x => x.Account.Id == accountId)
                .Select(x => new
                {
                    x.Id,
                    x.OperationType,
                    AccId = x.Account.Id,
                    x.Date,
                    x.Debt,
                    x.Credit,
                    WorkName = x.Work.Name
                })
                .AsEnumerable()
                .Where(x => (typeWorkByOperId.Get(x.Id).Return(z => z.StateName)) != "Черновик")
                .Select(x => new
                {
                    x.Id,
                    x.OperationType,
                    x.AccId,
                    x.Date,
                    x.Debt,
                    x.Credit,
                    WorkName = x.WorkName ?? typeWorkByOperId.Get(x.Id).Return(z => z.Name) ?? string.Empty
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}