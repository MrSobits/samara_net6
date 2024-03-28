namespace Bars.Gkh.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class PersonController : B4.Alt.DataController<Person>
    {
        public IPersonService Service { get; set; }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("PersonDataExport");

            try
            {
                return export != null ? export.ExportData(baseParams) : null;
            }
            finally
            {
                Container.Release(export);
            }
        }

        public ActionResult GetContactDetails(BaseParams baseParams)
        {
            var result = Service.GetContactDetails(baseParams);
            return result.Success ? new JsonNetResult(result) : JsFailure(result.Message);
        }

        public ActionResult AddWorkPlace(BaseParams baseParams)
        {
            var result = Service.AddWorkPlace(baseParams);
            return result.Success ? new JsonNetResult(result) : JsFailure(result.Message);
        }

        public ActionResult GetPersonRequestsToExam(BaseParams baseParams)
        {
            var repo = Container.ResolveRepository<PersonRequestToExam>();
            var personId = baseParams.Params.Get("personId").ToLong();
            var data = repo.GetAll().WhereIf(personId > 0, x => x.Person.Id == personId).Select(x => new { x.Id, x.RequestNum, x.RequestDate, x.ProtocolNum }).ToList();
            return new JsonListResult(data);
        }
    }
}