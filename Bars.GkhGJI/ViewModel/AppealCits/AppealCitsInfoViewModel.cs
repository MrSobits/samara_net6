namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using Entities;

    public class AppealCitsInfoViewModel : BaseViewModel<AppealCitsInfo>
    {
        public override IDataResult List(IDomainService<AppealCitsInfo> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.OperationDate,
                    x.AppealDate,
                    x.Correspondent,
                    x.OperationType,
                    x.Operator
                }).OrderByDescending(x=> x.OperationDate)
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}