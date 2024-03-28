namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Entities;

    public class VersionParamViewModel : BaseViewModel<VersionParam>
    {
        public override IDataResult List(IDomainService<VersionParam> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            var version = loadParam.Filter.GetAs<long>("version");
            var queryable = domainService.GetAll().Where(x => x.ProgramVersion.Id == version).OrderBy(x => x.Weight).Filter(loadParam, Container);

            return new ListDataResult(queryable.Order(loadParam).ToList(), queryable.Count());
        }
    }
}