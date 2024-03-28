namespace Bars.Gkh.Overhaul.Tat.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Entities;

    public class VersionRecordDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var version = loadParams.Filter.GetAs<long>("version");

            var municipalityId = loadParams.Filter.GetAs<long>("municipalityId");

            if (loadParams.Order == null || loadParams.Order.Length == 0)
            {
                loadParams.Order = new[] { new OrderField { Asc = true, Name = "IndexNumber" } };
            }

            return Container.Resolve<IDomainService<VersionRecord>>().GetAll()
                .Where(x => x.ProgramVersion.Id == version && x.ProgramVersion.Municipality.Id == municipalityId)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    RealityObject = x.RealityObject.Address,
                    x.CommonEstateObjects,
                    x.Year,
                    x.CorrectYear,
                    x.IndexNumber,
                    x.Point,
                    x.Sum
                })
                .ToList();
        }
    }
}
