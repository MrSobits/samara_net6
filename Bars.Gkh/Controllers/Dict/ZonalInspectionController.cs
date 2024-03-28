namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    // Заглушка для того чтобы в существующих регионах там где от него наследовались не полетело 
    public class ZonalInspectionController : ZonalInspectionController<ZonalInspection>
    {
        // Внимание !! Код override нужно писать не в этом классе, а в ZonalInspectionController<T>
    }

    // Класс переделан для того, чтобы в регионах можно было расширять сущность через subclass 
    // и при этом не писать дублирующий серверный код
    public class ZonalInspectionController<T> : B4.Alt.DataController<T>
        where T : ZonalInspection
    {
        public ActionResult AddInspectors(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IZonalInspectionService>().AddInspectors(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddMunicipalites(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IZonalInspectionService>().AddMunicipalities(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получение по OKATO
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult GetByOkato(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IZonalInspectionService>().GetByOkato(baseParams);
            return new JsonGetResult(result.Data);
        }
    }
}