namespace Bars.Gkh.RegOperator.Regions.Tatarstan.ViewModels
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;

    using DateTime = System.DateTime;

    /// <summary>
    /// Вьюмодель для "Платежный документ по начислениям и оплатам на КР"
    /// </summary>
    public class ConfirmContributionDocViewModel : BaseViewModel<ConfirmContributionDoc>
    {
        public override IDataResult List(IDomainService<ConfirmContributionDoc> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var id = baseParams.Params.GetAs<long>("confContribId");

            var data = domainService.GetAll()
                .Where(x => x.ConfirmContribution.Id == id)
                .Select(x => new
                {
                    x.Id,
                    ConfirmContribution = id,
                    x.RealityObject.Address,
                    x.DocumentNum,
                    DateFrom = x.DateFrom.HasValue ? x.DateFrom : null,
                    TransferDate = x.TransferDate.HasValue ? x.TransferDate : null,
                    x.Scan,
                    x.Amount
                })
                .Filter(loadParams, Container);

            var dataItems = data.Order(loadParams).Paging(loadParams).ToList();

            if(data.Any())
            {
                dataItems.Add(new
                {
                    Id = 0L,
                    ConfirmContribution = id,
                    Address = "Итого по всем загруженным платежкам",
                    DocumentNum = string.Empty,
                    DateFrom = (DateTime?)null,
                    TransferDate = (DateTime?)null,
                    Scan = (FileInfo)null,
                    Amount = data.Sum(item => item.Amount)
                });
            }

            return new ListDataResult(dataItems, data.Count());
        }
    }
}