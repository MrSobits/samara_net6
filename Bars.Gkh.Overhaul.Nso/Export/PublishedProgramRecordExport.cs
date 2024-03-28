namespace Bars.Gkh.Overhaul.Nso.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class PublishedProgramRecordExport : BaseDataExportService
    {
        public IDomainService<PublishedProgramRecord> domainService { get; set; }

        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);

            return
                domainService.GetAll()
                            .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                            .Select(x => new
                            {
                                Municipality = x.Stage2.Stage3Version.RealityObject.Municipality.Name,
                                RealityObject = x.Stage2.Stage3Version.RealityObject.Address,
                                x.Sum,
                                x.CommonEstateobject,
                                x.PublishedYear,
                                x.IndexNumber
                            })
                            .OrderBy(x => x.IndexNumber)
                            .ThenBy(x => x.PublishedYear)
                            .Filter(loadParam, Container)
                            .Order(loadParam)
                            .ToList();
        }
    }
}