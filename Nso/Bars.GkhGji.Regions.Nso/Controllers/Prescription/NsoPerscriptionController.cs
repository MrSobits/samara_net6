using Microsoft.AspNetCore.Mvc;
using Bars.B4;
using Bars.GkhGji.Regions.Nso.DomainService;

namespace Bars.GkhGji.Regions.Nso.Controllers
{
    using System.Collections;
    using Bars.GkhGji.Controllers;
    using Bars.GkhGji.Regions.Nso.Entities;

    /// <summary>
    /// Контроллер для работы с направлениями деятельности по предписаниям.
    /// <remarks>На основе существующего контроллера делаем свой для того чтобы все запросы шли на новый Url</remarks>
    /// </summary>
    public class NsoPrescriptionController : PrescriptionController<NsoPrescription>
    {
        /// <summary>
        /// Добавить направления деятельности
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult AddDirections(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IPrescriptionActivityDirectionService>();

            try
            {
                var result = service.AddDirections(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Вернуть список текущих направлений деятельности по предписанию
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список направлений</returns>
        public ActionResult ListDirections(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IPrescriptionActivityDirectionService>();

            try
            {
                var result = (ListDataResult)service.ListDirections(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}
