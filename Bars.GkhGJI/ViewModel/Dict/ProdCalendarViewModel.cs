namespace Bars.GkhGji.ViewModel
{
    using Bars.B4;
    using Entities;
    using System.Linq;

    public class ProdCalendarViewModel : BaseViewModel<ProdCalendar>
    {
        public override IDataResult List(IDomainService<ProdCalendar> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var data = domain.GetAll().Select(
                x => new
                {
                    x.Id,
                    x.ProdDate,
                    x.Name
                });

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}