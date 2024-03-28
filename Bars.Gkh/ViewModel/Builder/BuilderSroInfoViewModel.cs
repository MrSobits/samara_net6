namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;

    public class BuilderSroInfoViewModel : BaseViewModel<BuilderSroInfo>
    {
        public override IDataResult List(IDomainService<BuilderSroInfo> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var builderId = baseParams.Params.GetAs<long>("builderId");

            var data = domain.GetAll()
                .Where(x => x.Builder.Id == builderId)
                .Select(x => new
                {
                    x.Id,
                    Work = x.Work.Name,
                    x.DescriptionWork
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}