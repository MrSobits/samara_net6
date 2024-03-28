namespace Bars.GkhGji.DomainService.View
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Entities;

#warning Оптимизировать все запросы в папке View
    public class ViewDisposalWidgetViewModel : BaseViewModel<ViewDisposalWidget>
    {
        public override IDataResult List(IDomainService<ViewDisposalWidget> domainService, BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var activeOperator = userManager.GetActiveOperator();

            if (activeOperator == null)
            {
                return new ListDataResult { Success = false };
            }

            var disposalWidgetlData = domainService.GetAll()
                .Where(x => x.OperatorId == activeOperator.Id)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    Category = x.DateStart.HasValue ? x.DateStart.Value.ToShortDateString() : string.Empty,
                    DateEnd = x.DateEnd.HasValue ? x.DateEnd.Value.ToShortDateString() : string.Empty,
                    Header = x.Number,
                    x.TypeBase,
                    TypeDoc = "disposal"
                })
                .AsEnumerable()
                .Distinct();

            return new ListDataResult(disposalWidgetlData, disposalWidgetlData.Count());
        }
    }
}