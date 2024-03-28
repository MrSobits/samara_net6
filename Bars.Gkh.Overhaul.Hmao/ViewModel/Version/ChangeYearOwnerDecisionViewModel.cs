namespace Bars.Gkh.Overhaul.Hmao.ViewModel.Version
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.Entities.Version;
    using Bars.Gkh.Utils;

    public class ChangeYearOwnerDecisionViewModel : BaseViewModel<ChangeYearOwnerDecision>
    {
        public override IDataResult List(IDomainService<ChangeYearOwnerDecision> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var versionId = loadParam.Filter.GetAs<long>("versionId");

            if (versionId == 0)
            {
                throw new ArgumentException(nameof(versionId));
            }

            return domainService.GetAll()
                .Where(x => x.VersionRecordStage1.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.OldYear,
                        x.NewYear,
                        x.VersionRecordStage1.Stage2Version.Stage3Version.IndexNumber,
                        RealityObject = x.VersionRecordStage1.RealityObject.Address,
                        CommonEstateObject = x.VersionRecordStage1.StructuralElement.StructuralElement.Group.CommonEstateObject.Name,
                        StructuralElement = x.VersionRecordStage1.StructuralElement.StructuralElement.Name,
                        x.File
                    })
                .ToListDataResult(loadParam, this.Container);
        }
    }
}