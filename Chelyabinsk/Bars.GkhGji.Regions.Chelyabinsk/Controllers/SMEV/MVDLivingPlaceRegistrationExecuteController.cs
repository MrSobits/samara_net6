namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers
{

    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.GetSMEVAnswers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Entities;
    using BaseChelyabinsk.Enums;
    using System;
    using Microsoft.AspNetCore.Mvc;

    public class MVDLivingPlaceRegistrationExecuteController : BaseController
    {
        private readonly ITaskManager _taskManager;

        public MVDLivingPlaceRegistrationExecuteController(ITaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        public IDomainService<MVDLivingPlaceRegistration> MVDLivingPlaceRegistrationDomain { get; set; }     
        public IDomainService<MVDLivingPlaceRegistrationFile> MVDLivingPlaceRegistrationFileDomain { get; set; }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = MVDLivingPlaceRegistrationDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.Queued)
                return JsFailure("Запрос уже отправлен");


            try
            {
                _taskManager.CreateTasks(new MVDLivingPlaceRegistrationRequestTaskProvider(Container), baseParams);
                return GetResponce(baseParams, taskId);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос данных из ЕГРИП не удалось: " + e.Message);
            }
        }  

        public ActionResult GetResponce(BaseParams baseParams, Int64 taskId)
        {
            //Из-за нехватки времени все проверки ответа запускают таску на проверку всех ответоп
            var smevRequestData = MVDLivingPlaceRegistrationDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.ResponseReceived)
                return JsFailure("Ответ уже получен");

            //if (!baseParams.Params.ContainsKey("taskId"))
            //    baseParams.Params.Add("taskId", taskId);

            try
            {
                _taskManager.CreateTasks(new GetSMEVAnswersTaskProvider(Container), baseParams);
                return JsSuccess("Задача поставлена в очередь задач");
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на проверку ответов не удалось: " + e.Message);
            }
        }      
    }
}
