namespace Bars.Gkh.RegOperator.Regions.Tatarstan.ViewModels
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;

    using Bars.Gkh.DataResult;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;

    public class TransferObjectViewModel : BaseViewModel<TransferObject>
    {
        public IDomainService<RentPaymentIn> RentPaymentInDomain { get; set; }
        public IDomainService<AccumulatedFunds> AccumulatedFundsDomain { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        public ITransferObjectService TransferObjectService { get; set; }

        public override IDataResult List(IDomainService<TransferObject> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var transferRfRecordId = baseParams.Params.GetAsId("transferRfRecordId");
            var dateTransfer = baseParams.Params.GetAs<DateTime>("dateTransfer");

            var realityObjectIds = Container.ResolveDomain<TransferObject>().GetAll()
                .Where(x => x.TransferRecord.Id == transferRfRecordId)
                .Select(x => x.RealityObject.Id)
                .ToList();

            //var overhaulTypeService = PersAccServiceTypeDomain.FirstOrDefault(x => x.Code == "269");

            var overhaulTypePersAccRoIds = BasePersonalAccountDomain.GetAll()
                //.WhereIf(overhaulTypeService != null, x => x.ServiceType.Id == overhaulTypeService.Id)
                .Where(x => x.Room != null && x.Room.RealityObject != null)
                .Where(x => realityObjectIds.Contains(x.Room.RealityObject.Id))
                .Select(x => x.Room.RealityObject.Id);

            var chargeAccountRoIds = Container.ResolveDomain<RealityObjectChargeAccountOperation>().GetAll()
                .Where(x => overhaulTypePersAccRoIds.Contains(x.Account.RealityObject.Id))
                .Where(x => x.Date.Month == dateTransfer.Month)
                .Where(x => x.Date.Year == dateTransfer.Year)
                .Select(x => x.Account.RealityObject.Id);

            // Так как в базе хранятся суммы по всем типам собственности, придется расчитать суммы самим
            var chargeAccountPaids = this.TransferObjectService.GetPaids(dateTransfer, chargeAccountRoIds);

            var data = domainService.GetAll()
                .Where(x => x.TransferRecord.Id == transferRfRecordId)
                .Where(x => overhaulTypePersAccRoIds.Contains(x.RealityObject.Id))
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