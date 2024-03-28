namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;

    public class LongTermObjectLoanViewModel : BaseViewModel<LongTermObjectLoan>
    {
        public override IDataResult List(IDomainService<LongTermObjectLoan> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var realtyObjId = baseParams.Params.GetAs<long>("listForAccount");

            if (realtyObjId > 0)
            {
                var data1 = domainService.GetAll()
                .Where(x => x.Object.RealityObject.Id == realtyObjId)
                .Select(x => new
                {
                    x.Id,
                    ObjectIssued = x.ObjectIssued.RealityObject.Address,
                    x.LoanAmount,
                    x.DateIssue,
                    x.DateRepayment,
                    x.PeriodLoan
                })
                .Filter(loadParams, Container);

                return new ListDataResult(data1.Order(loadParams).Paging(loadParams).ToList(), data1.Count());
            }

            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domainService.GetAll()
                .Where(x => x.Object.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    ObjectIssued = x.ObjectIssued.RealityObject.Address,
                    x.LoanAmount,
                    x.DateIssue,
                    x.DateRepayment,
                    x.PeriodLoan
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<LongTermObjectLoan> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAs<long>("id"));

            return new BaseDataResult(new
            {
                obj.Id,
                obj.DateIssue,
                obj.DateRepayment,
                obj.DocumentDate,
                obj.DocumentNumber,
                obj.File,
                obj.LoanAmount,
                Object = obj.Object.Id,
                ObjectIssued = new {obj.ObjectIssued.Id, obj.ObjectIssued.RealityObject.Address},
                obj.PeriodLoan
            });
        }
    }
}