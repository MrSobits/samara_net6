namespace Bars.GkhGji.Export
{
    using System.Collections;
    using System.Collections;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    public class AppealCitsExecutantDataExport : BaseDataExportService, IAppealCitsExecutantDataExport
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();
            var loadParams = this.GetLoadParam(baseParams);
            var appealCitizensId = baseParams.Params.GetAsId("appealCitizensId");

            return service.GetAll()
                .Where(x=> x.AppealCits.Id == appealCitizensId)
                .Select(x => new
                {
                    x.Id,
                   x.AppealCits.DocumentNumber,
                    Author = x.Author.Fio,
                    Executant = x.Executant.Fio,
                    Controller = x.Controller.Fio,
                    x.Description,
                    x.OrderDate,
                    IsResponsible = x.IsResponsible? "Да": "Нет",
                    x.PerformanceDate,
                    ZonalInspection = x.ZonalInspection.BlankName,
                    State = x.State.Name
                })
                .Filter(loadParams, this.Container)
                .Order(loadParams)
                .ToList();

        }
    }
}
