namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Entities;
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhGji.Regions.Voronezh.Tasks.MVDSendInformationRequest;
    using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
    using System;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using System.Linq;
    using Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Bars.Gkh.Entities;


    public class SMEVMVDExecuteController : BaseController
    {
        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private IDomainService<FileInfo> _fileDomain;

        public SMEVMVDExecuteController(IFileManager fileManager, IDomainService<FileInfo> fileDomain, ITaskManager taskManager)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _taskManager = taskManager;
        }

        public IDomainService<SMEVMVD> SMEVMVDDomain { get; set; }

        public IDomainService<SMEVMVDFile> SMEVMVDFileDomain { get; set; }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVMVDDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.Queued)
                return JsFailure("Запрос уже отправлен");

            try
            {
                _taskManager.CreateTasks(new SendMVDRequestTaskProvider(Container), baseParams);
                return GetResponce(baseParams, taskId);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос данных из МВД не удалось: " + e.Message);
            }
        }


        public ActionResult GetResponce(BaseParams baseParams, Int64 taskId)
        {
            //Из-за нехватки времени все проверки ответа запускают таску на проверку всех ответоп
            var smevRequestData = SMEVMVDDomain.Get(taskId);
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

        public ActionResult CreateMVDRequest(BaseParams baseParams)
        {
            var personId = baseParams.Params.GetAs<long>("personId");
            if (personId == 0)
            {
                return JsFailure("Не найдено должностное лицо");
            }
            var personDomain = Container.Resolve<IDomainService<Person>>();
            var person = personDomain.Get(personId);
            var smevRequestData = new SMEVMVD
            {
                Name = person.Name,
                
                AddressPrimary = person.AddressBirth,
                BirthDate = person.Birthdate.Value,
                CalcDate = DateTime.Now,
                MVDTypeAddressPrimary = !string.IsNullOrEmpty(person.AddressBirth)? MVDTypeAddress.BirthPlace:MVDTypeAddress.LivingPlace,
                MVDTypeAddressAdditional = MVDTypeAddress.LivingPlace,
                AddressAdditional = person.AddressReg,
                PatronymicName = person.Patronymic,
                Surname = person.Surname,
                RegionCodePrimary = GetRegionCode(),
                RegionCodeAdditional = GetRegionCode()
            };
            SMEVMVDDomain.Save(smevRequestData);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            baseParams.Params.Clear();
            if (!baseParams.Params.ContainsKey("taskId"))
                baseParams.Params.Add("taskId", smevRequestData.Id.ToString());
            try
            {
                _taskManager.CreateTasks(new SendMVDRequestTaskProvider(Container), baseParams);
                return GetResponce(baseParams, smevRequestData.Id);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос данных из МВД не удалось: " + e.Message);
            }
        }

        private RegionCodeMVD GetRegionCode()
        {
            var regionDOmain = Container.Resolve<IDomainService<RegionCodeMVD>>();
            try
            {
                return regionDOmain.GetAll().FirstOrDefault(x => x.Code == "036");
            }
            catch
            {
                return null;
            }
            finally
            {
                Container.Release(regionDOmain);
            }
        }

    }
}
