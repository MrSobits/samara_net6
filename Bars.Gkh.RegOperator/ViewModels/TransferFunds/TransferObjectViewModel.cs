namespace Bars.Gkh.RegOperator.ViewModels
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.RegOperator.DomainService;

    using DataResult;
    using Entities;
    using Gkh.Domain;

    public class TransferObjectViewModel : BaseViewModel<TransferObject>
    {
        public ITransferObjectService TransferObjectService { get; set; }

        public override IDataResult List(IDomainService<TransferObject> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var transferRfRecordId = baseParams.Params.GetAsId("transferRfRecordId");
            var dateTransfer = baseParams.Params.GetAs<DateTime>("dateTransfer");

            var filter = Container.ResolveDomain<TransferObject>().GetAll()
                .Where(x => x.TransferRecord.Id == transferRfRecordId)
                .Select(x => x.RealityObject.Id)
                .ToList();

            var chargeAccountRoIds = Container.ResolveDomain<RealityObjectChargeAccountOperation>().GetAll()
                .Where(y => filter.Contains(y.Account.RealityObject.Id))
                .Where(x => x.Date.Month == dateTransfer.Month)
                .Where(x => x.Date.Year == dateTransfer.Year)
                .Select(x => x.Account.RealityObject.Id);

            // Так как в базе хранятся суммы по всем типам собственности, придется расчитать суммы самим,
            // исключая начисления квартир с типом собственности "Муниципальная"
            var chargeAccountPaids = this.TransferObjectService.GetPaids(dateTransfer, chargeAccountRoIds);

            var data = domainService.GetAll()
                .Where(x => x.TransferRecord.Id == transferRfRecordId)
                .Select(x => new
                {
                    x.Id,
                    RealityObject = x.RealityObject.Id,
                    TransferObject = x.TransferRecord.Id,
                    x.RealityObject.Address,
                    x.RealityObject.GkhCode,
                    Municipality = x.RealityObject.Municipality.Name,
                    x.Transferred,
                    TransferredSum = x.TransferredSum.GetValueOrDefault()
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject,
                    x.TransferObject,
                    x.Address,
                    x.GkhCode,
                    x.Municipality,
                    x.Transferred,
                    x.TransferredSum,
                    PaidTotal = chargeAccountPaids.Get(x.RealityObject)
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            var summary = new
            {
                PaidTotal = data.Sum(x => x.PaidTotal),
                TransferredSum = data.Sum(x => x.TransferredSum)
            };

            return new ListSummaryResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count(), summary);
        }
    }
}