namespace Bars.GkhEdoInteg.Controllers
{
    using System;

    using B4;
    using DomainService;
    using Entities;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Получение обращений из системы Электронного документооборота
    /// </summary>
    public class EdoIntegrationController : BaseController
    {
        /// <summary>
        /// Получение и загрузка данных
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public virtual ActionResult Load(BaseParams baseParams)
        {
            try
            {
                var integration = Container.Resolve<IEmsedService>("Edo integration");
                if (integration != null)
                {
                    string message;
                    return integration.LoadData(baseParams.Params, out message) ? new JsonNetResult(new { success = true, message }) : JsonNetResult.Failure(message);
                }

                var exc = new Exception("Не найдена реализация интеграции ЭДО");
                Container.Resolve<ILogger>().LogError( exc, "Интеграция с ЭДО");
                return JsonNetResult.Failure(exc.Message);
            }
            catch (ValidationException exc)
            {
                Container.Resolve<ILogger>().LogError(exc, "Интеграция с ЭДО");
                return JsonNetResult.Failure(exc.Message);
            }
        }

        /// <summary>
        /// Загрузка сканов обращений граждан 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public virtual ActionResult LoadDocuments(BaseParams baseParams)
        {
            try
            {
                var integration = Container.Resolve<IEmsedService>("Edo integration");
                if (integration != null)
                {
                    string message;
                    return integration.LoadDocuments(baseParams.Params, out message) ? new JsonNetResult(new { success = true, message }) : JsonNetResult.Failure(message);
                }

                var exc = new Exception("Не найдена реализация интеграции ЭДО");
                Container.Resolve<ILogger>().LogError(exc, "Интеграция с ЭДО");
                return JsonNetResult.Failure(exc.Message);
            }
            catch (ValidationException exc)
            {
                Container.Resolve<ILogger>().LogError(exc, "Интеграция с ЭДО");
                return JsonNetResult.Failure(exc.Message);
            }
        }

        /// <summary>
        /// Отправка выбранных документов
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public virtual ActionResult Send(BaseParams baseParams)
        {
            var service = Container.Resolve<IEmsedService>();
            string msg;
            var result = service.Send(baseParams, out msg);
            return result ? JsonNetResult.Success : JsonNetResult.Failure(msg);
        }

        public virtual ActionResult ListAppealCitsLog(BaseParams baseParams)
        {
            var domainService = Container.Resolve<IAppealCitsEdoIntegService>();
            var listResult = (ListDataResult)domainService.ListAppealCitsLog(baseParams);
            return new JsonNetResult(new { success = true, data = listResult.Data, totalCount = listResult.TotalCount });
        }

        public virtual ActionResult ListAppealCits(BaseParams baseParams)
        {
            var domainService = Container.Resolve<IDomainService<AppealCitsCompareEdo>>();
            var viewModel = Container.Resolve<IViewModel<AppealCitsCompareEdo>>();
            var listResult = (ListDataResult)viewModel.List(domainService, baseParams);
            return new JsonNetResult(new { success = true, data = listResult.Data, totalCount = listResult.TotalCount });
        }

        /// <summary>
        /// Получение списка документов проверки и ответов
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public virtual ActionResult ListInspectionDocsAndAnswers(BaseParams baseParams)
        {
            var service = Container.Resolve<IAppealCitsEdoIntegService>();

            var listResult = (ListDataResult)service.ListInspectionDocsAndAnswers(baseParams);
            return new JsonNetResult(new { success = true, data = listResult.Data, totalCount = listResult.TotalCount });
        }
    }
}
