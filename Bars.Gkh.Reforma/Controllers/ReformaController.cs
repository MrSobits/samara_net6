namespace Bars.Gkh.Reforma.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Reforma.Domain;

    /// <summary>
    ///     Контроллер параметров интеграции с Реформой
    /// </summary>
    public class ReformaController : BaseController
    {
        /// <summary>
        ///     Сервис параметров
        /// </summary>
        public ISyncService Service { get; set; }

        /// <summary>
        ///     Получение параметров
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetParams()
        {
            var data = Service.GetParams();
            if (!data.Success)
            {
                return JsonNetResult.Failure(data.Message);
            }

            var parameters = (Dictionary<string, object>)data.Data;
            if (!string.IsNullOrEmpty(parameters["Password"].ToString()))
            {
                parameters["Password"] = "ЭтоПарольВерьМне";
                data.Data = parameters;
            }

            return new JsonNetResult(data);
        }

        /// <summary>
        ///     Сохранение параметров
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public virtual ActionResult SaveParams(BaseParams baseParams)
        {
            var result = Service.SaveParams(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        ///     Запуск интеграции
        /// </summary>
        /// <returns>Результат</returns>
        public virtual ActionResult RunNow()
        {
            var result = Service.RunNow();
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получение информации об объекте синхронизации
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public virtual ActionResult GetDetails(BaseParams baseParams)
        {
            var type = baseParams.Params.GetAs<string>("type");
            var id = baseParams.Params.GetAs<long>("id");
            if (id == 0)
            {
                return JsonNetResult.Failure("Не задан параметр: id");
            }

            IDataResult result;
            switch (type)
            {
                case "manorg":
                    result = Service.GetManOrgDetails(id);
                    return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
                case "robject":
                    result = Service.GetRobjectDetails(id);
                    return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
                default:
                    return JsonNetResult.Failure("Неверно задан параметр: type");
            }
        }
    }
}