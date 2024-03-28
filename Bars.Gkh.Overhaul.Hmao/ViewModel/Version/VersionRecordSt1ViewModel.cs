namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Entities;

    public class VersionRecordSt1ViewModel : BaseViewModel<VersionRecordStage1>
    {
        public override IDataResult List(IDomainService<VersionRecordStage1> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var versionRecId = loadParams.Filter.GetAs<long>("versionRecId");

            var data = domainService.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.Id == versionRecId)
                .Select(x => new
                    {
                        x.Id,
                        StructuralElement = x.StructuralElement.StructuralElement.Name,
                        PlanYear = x.Stage2Version.Stage3Version.Year
                    });

            return new ListDataResult(data.ToList(), data.Count());
        }
    }
}