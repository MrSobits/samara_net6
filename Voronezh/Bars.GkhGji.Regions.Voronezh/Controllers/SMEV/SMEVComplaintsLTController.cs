namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.Gkh.Domain;
    using Bars.B4;
    using Bars.Gkh.DomainService;
    using System;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Modules.FileStorage;
    using System.IO;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using System.Collections.Generic;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
    using Bars.GkhGji.Regions.Voronezh.Tasks.SendPaymentRequest;
    using Bars.GkhGji.Regions.Voronezh.DomainService;
    using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;

    public class SMEVComplaintsLTController : BaseController
    {
        public IDomainService<SMEVComplaintsStep> SMEVComplaintsStepDomain { get; set; }
        public IDomainService<SMEVComplaints> SMEVComplaintsDomain { get; set; }
        public IBlobPropertyService<SMEVComplaints, SMEVComplaintsLongText> LongTextService { get; set; }
        public IDomainService<SMEVComplaintsRequest> SMEVComplaintsRequestDomain { get; set; }
        public IDomainService<SMEVComplaintsRequestFile> SMEVComplaintsRequestFileDomain { get; set; }

        private IFileManager _fileManager;
        private IDomainService<B4.Modules.FileStorage.FileInfo> _fileDomain;
        private readonly ITaskManager _taskManager;

        public SMEVComplaintsLTController(IFileManager fileManager, IDomainService<B4.Modules.FileStorage.FileInfo> fileDomain, ITaskManager taskManager)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _taskManager = taskManager;
        }

        public ActionResult ListComplaintFiles(BaseParams baseParams)
        {
            var appealService = Container.Resolve<IComplaintsService>();//IComplaintsService
            try
            {
                return appealService.ListComplaintFiles(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }

        public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {

            var isGetInfo = baseParams.Params.GetAs<bool>("isGetInfo");

            if (isGetInfo)
            {
                var smevRequestData = SMEVComplaintsDomain.Get(taskId);
                if (smevRequestData == null)
                    return JsFailure("Запрос не сохранен");
                KndRequest acceptRequest = new KndRequest
                {
                    Item = new getComplaintsType
                    {
                        Item = smevRequestData.ComplaintId,
                        unit = GetComplaintUnit(),
                        uploadFiles = true,
                        uploadFilesSpecified = true
                    }
                };
                var voidRequestElement = ToXElement<KndRequest>(acceptRequest);

                SMEVComplaintsRequest voidRequestRequest = new SMEVComplaintsRequest
                {
                    CalcDate = DateTime.Now,
                    TypeComplainsRequest = TypeComplainsRequest.ComplaintsRequest,
                    TextReq = voidRequestElement.ToString(),
                    ComplaintId = smevRequestData.Id
                };
                SMEVComplaintsRequestDomain.Save(voidRequestRequest);

                var baseParamsTask = new BaseParams();

                if (!baseParamsTask.Params.ContainsKey("taskId"))
                    baseParamsTask.Params.Add("taskId", voidRequestRequest.Id.ToString());

                try
                {
                    _taskManager.CreateTasks(new SendComplaintsCustomRequestTaskProvider(Container), baseParamsTask);
                    return GetResponce(baseParamsTask, voidRequestRequest.Id);
                }
                catch (Exception e)
                {
                    return JsFailure("Создание задачи на запрос данных из ЕГРЮЛ не удалось: " + e.Message);
                }
            }
            else
            {
                var smevRequestData = SMEVComplaintsStepDomain.Get(taskId);
                if (smevRequestData == null)
                    return JsFailure("Запрос не сохранен");
                if (smevRequestData.DOTypeStep == DOTypeStep.renewTermStep)
                {
                    if (smevRequestData.DOPetitionResult == DOPetitionResult.Complete)
                    {
                        if (!smevRequestData.NewDate.HasValue)
                        {
                            return JsFailure("Не указана дата восстановления срока");
                        }
                    }
                    if (string.IsNullOrEmpty(smevRequestData.Reason))
                    {
                        return JsFailure("Не указано основание принятия решения");
                    }

                }
                if (smevRequestData.DOTypeStep == DOTypeStep.pauseResolutionStep)
                {
                    if (string.IsNullOrEmpty(smevRequestData.Reason))
                    {
                        return JsFailure("Не указано основание принятия решения");
                    }
                }
                if (smevRequestData.DOTypeStep == DOTypeStep.Info)
                {
                    if (string.IsNullOrEmpty(smevRequestData.Reason) || string.IsNullOrEmpty(smevRequestData.AddDocList))
                    {
                        return JsFailure("Не указано основание принятия решения и/или список запрашиваемых документов");
                    }
                }

                KndRequest acceptRequest = new KndRequest
                {
                    Item = new sendComplaintEventType
                    {
                        id = smevRequestData.SMEVComplaints.ComplaintId,
                        Item = GetItem(smevRequestData),
                        eventTime = DateTime.Now,
                        unit = new unitType
                        {
                            id = "68566872-7cc6-ea4b-0f69-eac0cf88819f",
                            Value = "ГОСУДАРСТВЕННАЯ ЖИЛИЩНАЯ ИНСПЕКЦИЯ ВОРОНЕЖСКОЙ ОБЛАСТИ"
                        }
                    }
                };
                var voidRequestElement = ToXElement<KndRequest>(acceptRequest);

                SMEVComplaintsRequest voidRequestRequest = new SMEVComplaintsRequest
                {
                    CalcDate = DateTime.Now,
                    TypeComplainsRequest = smevRequestData.DOTypeStep == DOTypeStep.Info ? TypeComplainsRequest.DataRequest : TypeComplainsRequest.Step,
                    TextReq = voidRequestElement.ToString(),
                    ComplaintId = smevRequestData.SMEVComplaints.Id
                };
                SMEVComplaintsRequestDomain.Save(voidRequestRequest);
                if (smevRequestData.FileInfo != null)
                {
                    SMEVComplaintsRequestFileDomain.Save(new SMEVComplaintsRequestFile
                    {
                        FileInfo = smevRequestData.FileInfo,
                        SMEVComplaintsRequest = voidRequestRequest,
                        SMEVFileType = SMEVFileType.RequestAttachment
                    });
                }

                var baseParamsTask = new BaseParams();

                if (!baseParamsTask.Params.ContainsKey("taskId"))
                    baseParamsTask.Params.Add("taskId", voidRequestRequest.Id.ToString());

                try
                {
                    smevRequestData.YesNo = Gkh.Enums.YesNo.Yes;
                    SMEVComplaintsStepDomain.Update(smevRequestData);
                    _taskManager.CreateTasks(new SendComplaintsCustomRequestTaskProvider(Container), baseParamsTask);
                    return GetResponce(baseParamsTask, voidRequestRequest.Id);
                }
                catch (Exception e)
                {
                    return JsFailure("Создание задачи на запрос данных из ЕГРЮЛ не удалось: " + e.Message);
                }
            }

        }
        public ActionResult GetResponce(BaseParams baseParams, Int64 taskId)
        {

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

        private XElement ToXElement<T>(object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }
        private unitType[] GetComplaintUnit()
        {
            List<unitType> unitList = new List<unitType>();
            unitList.Add(new unitType
            {
                // id = "1030200000000001",
                id = "68566872-7cc6-ea4b-0f69-eac0cf88819f",
                Value = "ГОСУДАРСТВЕННАЯ ЖИЛИЩНАЯ ИНСПЕКЦИЯ ВОРОНЕЖСКОЙ ОБЛАСТИ"
            });
            return unitList.ToArray();
        }


        private object GetItem(SMEVComplaintsStep step)
        {
            if (step.DOTypeStep == DOTypeStep.renewTermStep)
            {
                return new sendComplaintEventTypeRenewTermStep
                {
                    Item = GetRenewItem(step)
                };
            }
            if (step.DOTypeStep == DOTypeStep.pauseResolutionStep)
            {
                return new sendComplaintEventTypePauseResolutionStep
                {
                    Item = GetPauseItem(step)
                };
            }
            if (step.DOTypeStep == DOTypeStep.Info)
            {
                return new sendComplaintEventTypeResolutionStage
                {
                    Item = new sendComplaintEventTypeResolutionStageRequestData
                    {
                        addDocList = step.AddDocList,
                        reason = step.Reason,
                        signer = new userType
                        {
                            id = "6501c759763fac7767ab79a7",
                            name = "Дробышев Игорь Анатольевич"
                        }
                    }
                };
            }
            return null;
        }

        private object GetPauseItem(SMEVComplaintsStep step)
        {
            if (step.DOPetitionResult == DOPetitionResult.Complete)
            {
                return new sendComplaintEventTypePauseResolutionStepAccept
                {
                    reason = step.Reason,
                    signer = new userType
                    {
                        id = "6501c759763fac7767ab79a7",
                        name = "Дробышев Игорь Анатольевич"
                    }
                };
            }
            if (step.DOPetitionResult == DOPetitionResult.Reject)
            {
                return new sendComplaintEventTypePauseResolutionStepReject
                {
                    reason = step.Reason,
                    signer = new userType
                    {
                        id = "6501c759763fac7767ab79a7",
                        name = "Дробышев Игорь Анатольевич"
                    }
                };
            }
            return null;
        }

        private object GetRenewItem(SMEVComplaintsStep step)
        {
            if (step.DOPetitionResult == DOPetitionResult.notNeeded)
            {
                return new sendComplaintEventTypeRenewTermStepNotNeeded
                {
                    reason = step.Reason
                };
            }
            if (step.DOPetitionResult == DOPetitionResult.Complete)
            {
                return new sendComplaintEventTypeRenewTermStepAccept
                {
                    reason = step.Reason,
                    renewDate = step.NewDate.HasValue ? step.NewDate.Value : step.ObjectCreateDate,
                    signer = new userType
                    {
                        id = "6501c759763fac7767ab79a7",
                        name = "Дробышев Игорь Анатольевич"
                    }
                };
            }
            if (step.DOPetitionResult == DOPetitionResult.Reject)
            {
                return new sendComplaintEventTypeRenewTermStepReject
                {
                    reason = step.Reason,
                    signer = new userType
                    {
                        id = "6501c759763fac7767ab79a7",
                        name = "Дробышев Игорь Анатольевич"
                    }
                };
            }
            return null;
        }


    }
}