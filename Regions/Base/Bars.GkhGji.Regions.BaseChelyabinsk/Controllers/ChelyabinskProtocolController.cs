namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Controllers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;

    /// <summary>
    /// Контролленр для работы с протоколами
    /// <remarks>На основе существующего контроллера делаем свой для того чтобы все запросы шли на новый Url</remarks>
    /// </summary>
    public class ChelyabinskProtocolController : ProtocolController<ChelyabinskProtocol>
    {
        /// <summary>
        /// Сервис предоставляет функционал сохранения и получения длинных полей у протокола
        /// </summary>
        public IBlobPropertyService<GkhGji.Entities.Protocol, ProtocolLongText> LongTextService { get; set; }

        /// <summary>
        /// Вернуть описание
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Сохранить описание
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Добавить требования
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult AddRequirements(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IChelyabinskProtocolService>();
            try
            {
                return service.AddRequirements(baseParams).ToJsonResult();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Добавить направления деятельности
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult AddDirections(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IChelyabinskProtocolService>();
            try
            {
                return service.AddDirections(baseParams).ToJsonResult();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Вернуть список текущих направлений деятельности по протоколу
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список направлений</returns>
        public ActionResult ListDirections(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IChelyabinskProtocolService>();

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