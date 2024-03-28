namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    // Пустышка на тот случай если гдето от этого касса наследовались
    public class LocalGovernmentController: LocalGovernmentController<LocalGovernment>
    {
        // Внимание !!! методы добавлять в Generic класс
    }

    // Generic Класс для лучшего расширения в регионазх ущности LocalGovernment
    public class LocalGovernmentController<T> : B4.Alt.DataController<T>
        where T : LocalGovernment
    {
        public ActionResult AddMunicipalities(BaseParams baseParams)
        {

            var Service = Container.Resolve<ILocalGovernmentService>();

            try
            {
                var result = Service.AddMunicipalities(baseParams);

                if (result.Success)
                {
                    return new JsonNetResult(new { success = true });
                }

                return JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(Service);
            }
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            var Service = Container.Resolve<ILocalGovernmentService>();

            try
            {
                var result = Service.GetInfo(baseParams);
                if (result.Success)
                {
                    return new JsonNetResult(result.Data);
                }

                return JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(Service);
            }
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("LocalGovernmentDataExport");
            try
            {
                return export != null ? export.ExportData(baseParams) : null;
            }
            finally 
            {
                Container.Release(export);
            }
        }
    }
} 