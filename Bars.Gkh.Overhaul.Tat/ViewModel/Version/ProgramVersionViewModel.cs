namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.Enum;

    using Entities;

    public class ProgramVersionViewModel : BaseViewModel<ProgramVersion>
    {
        public override IDataResult List(IDomainService<ProgramVersion> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");

            var data = domainService.GetAll()
                .WhereIf(municipalityId != 0, x => x.Municipality.Id == municipalityId)
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    Municipality = x.Municipality.Name,
                    MunicipalityId = x.Municipality.Id,
                    x.Name,
                    x.VersionDate,
                    x.IsMain,
                    x.IsProgramPublished,
                    CopyingState = x.CopyingState == ProgramVersionCopyingState.NotCopied
                        ? string.Empty
                        : x.CopyingState.GetEnumMeta().Display
                })
                .Filter(loadParams, this.Container);

            var count = data.Count();
            
            var resultData = data
                .Order(loadParams)
                .OrderIf(loadParams.Order.Length == 0, false, x => x.IsMain)
                .ToList();
            
            return new ListDataResult(resultData, count);
        }
    }
}
