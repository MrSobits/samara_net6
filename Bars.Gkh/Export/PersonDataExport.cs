using Bars.Gkh.DomainService;

namespace Bars.Gkh.Export
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;

    public class PersonDataExport : BaseDataExportService
    {

        public IPersonService PersonService { get; set; }

        public override IList GetExportData(BaseParams baseParams)
        {
            var totalCount = 0;

            return PersonService.GetList(baseParams, true, ref totalCount);
        }
    }
}