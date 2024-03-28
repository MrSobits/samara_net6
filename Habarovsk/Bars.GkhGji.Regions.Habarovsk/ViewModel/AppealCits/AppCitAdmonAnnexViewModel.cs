namespace Bars.GkhGji.Regions.Habarovsk.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Entities;
    using B4.Utils;

    public class AppCitAdmonAnnexViewModel : BaseViewModel<AppCitAdmonAnnex>
    {
        public override IDataResult List(IDomainService<AppCitAdmonAnnex> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("AppealCitsAdmonition", 0L);
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);

            var data = domain.GetAll()
             .Where(x => x.AppealCitsAdmonition.Id == id)
            .Select(x => new
            {
                x.Id,
                x.DocumentDate,
                x.Name,
                x.Description,
                x.File,
                x.SignedFile,
                x.MessageCheck
            })
            .AsQueryable()
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}