namespace Sobits.GisGkh.ViewModel
{
    using Entities;
    using System.Linq;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.B4;

    public class NsiListViewModel : BaseViewModel<NsiList>
    {
        public override IDataResult List(IDomainService<NsiList> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);

            var data = domain.GetAll()
           .Select(x => new
           {
               x.Id,
               x.EntityName,
               x.ListGroup,
               x.GisGkhCode,
               x.GisGkhName,
               x.MatchDate,
               x.ModifiedDate,
               x.RefreshDate
           })
           .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());

        }
    }
}