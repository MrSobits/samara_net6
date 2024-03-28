namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;

    public class ShortProgramRealityObjectViewModel : BaseViewModel<ShortProgramRealityObject>
    {
        public override IDataResult List(IDomainService<ShortProgramRealityObject> domainService, BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");
            var year = baseParams.Params.GetAs<long>("year");

            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Where(x => x.ProgramVersion.Municipality.Id == municipalityId && x.ProgramVersion.IsMain)
                .Where(x => x.Year == year)
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.RealityObject.Address
                })
                .OrderBy(x => x.Address)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}