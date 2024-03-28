namespace Bars.Gkh.Export
{
    using System.Collections;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class PersonRequestToExamDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var domain = Container.ResolveDomain<PersonRequestToExam>();

            try
            {
                var loadParams = GetLoadParam(baseParams);

                var personId = loadParams.Filter.GetAs("personId", 0L);
                var showAll = loadParams.Filter.GetAs("showAll", false);

                return domain.GetAll()
                    .WhereIf(!showAll, x => x.Person.Id == personId)
                    .Select(x => new
                    {
                        x.Id,
                        x.RequestNum,
                        x.RequestDate,
                        x.Person.FullName,
                        Person = x.Person.Id,
                        x.State
                    })
                    .Filter(loadParams, Container)
                    .Order(loadParams)
                    .ToList();
            }
            finally
            {
                Container.Release(domain);
            }
        }
    }
}