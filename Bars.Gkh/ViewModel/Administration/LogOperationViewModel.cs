namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class LogOperationViewModel : BaseViewModel<LogOperation>
    {
        public override IDataResult List(IDomainService<LogOperation> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    User = x.User.Login ?? string.Empty,
                    x.StartDate,
                    x.EndDate,
                    x.Comment,
                    x.OperationType,
                    x.LogFile
                })
                .OrderIf(loadParams.Order.Length == 0, false, x => x.StartDate)
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}