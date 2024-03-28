namespace Bars.GisIntegration.Base.Tasks.PrepareData.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.Services;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Base.ServicesAsync;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Bars.GisIntegration.Base.Utils;

    using Attachment = ServicesAsync.Attachment;

    /// <summary>
    /// Задача подготовки запроса
    /// </summary>
    public class CompletedWorkPrepareDataTask : BasePrepareDataTask<importCompletedWorksRequest>
    {
        /// <summary>
        /// Максимальное количество записей, которые можно экспортировать одним запросом
        /// </summary>
        private const int portionSize = 100;

        private List<RisCompletedWork> risCompletedWorkList;
        private Dictionary<long, List<RisCompletedWork>> risCompletedWorkListByWorkListId;
        private Dictionary<long, WorkList> workListById;
        private Dictionary<long, DateTime[]> dataStartByReportWorkIds;
        private Dictionary<long, string> contractNumberByRealityObjectId;
        private Dictionary<long, long> realityObjectIdByReportWorkId;

        //TODO таски не должны зависеть от сущностей внешней системы! Переделать на использование рис-сущности!
        //public IDomainService<RepairWork> RepairWorkDomainService { get; set; }

        //TODO таски не должны зависеть от сущностей внешней системы! Переделать на использование рис-сущности!
        //public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectDomainService { get; set; }

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var risCompletedWorkExtractor = this.Container.Resolve<IDataExtractor<RisCompletedWork>>("RisCompletedWorkExtractor");

            try
            {
                this.risCompletedWorkList = this.RunExtractor(risCompletedWorkExtractor, parameters);

                this.risCompletedWorkListByWorkListId = this.risCompletedWorkList
                    .GroupBy(x => x.WorkPlanItem.WorkListItem.WorkList.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());

                this.workListById = this.risCompletedWorkList.Select(x => x.WorkPlanItem.WorkListItem.WorkList)
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.First());

                var externalSystemIds = this.risCompletedWorkList.Select(x => x.WorkPlanItem.WorkListItem.ExternalSystemEntityId).ToList();

                this.dataStartByReportWorkIds = new Dictionary<long, DateTime[]>();
                //TODO таски не должны зависеть от сущностей внешней системы! Переделать на использование рис-сущности!
                //this.RepairWorkDomainService.GetAll()
                //.Where(x => externalSystemIds.Contains(x.Id))
                //.Select(
                //    x => new
                //    {
                //        x.Id,
                //        x.RepairObject.RepairProgram.Period.DateStart
                //    })
                //.GroupBy(x => x.Id)
                //.ToDictionary(x => x.Key, x => x.Select(y => y.DateStart).ToArray());

                this.contractNumberByRealityObjectId = new Dictionary<long, string>();
                //TODO таски не должны зависеть от сущностей внешней системы! Переделать на использование рис-сущности!
                //this.ManOrgContractRealityObjectDomainService.GetAll()
                //.Where(x => x.ManOrgContract.ManagingOrganization.Contragent.Id == this.Contragent.GkhId)
                //.Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value > DateTime.Today)
                //.Where(x => x.ManOrgContract.TerminateReason == null || x.ManOrgContract.TerminateReason == "")
                //.Select(
                //    x => new
                //    {
                //        x.RealityObject.Id,
                //        x.ManOrgContract.DocumentNumber
                //    })
                //.GroupBy(x => x.Id)
                //.ToDictionary(x => x.Key, x => x.Select(y => y.DocumentNumber).First());

                this.realityObjectIdByReportWorkId = new Dictionary<long, long>();
                //TODO таски не должны зависеть от сущностей внешней системы! Переделать на использование рис-сущности!
                //this.RepairWorkDomainService.GetAll()
                //.Where(x => externalSystemIds.Contains(x.Id))
                //.Select(
                //    x => new
                //    {
                //        x.Id,
                //        RealityObjectId = x.RepairObject.RealityObject.Id
                //    })
                //.GroupBy(x => x.Id)
                //.ToDictionary(x => x.Key, x => x.Select(y => y.RealityObjectId).First());
            }
            finally
            {
                this.Container.Release(risCompletedWorkExtractor);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();
            result.AddRange(this.ValidateObjectList(this.risCompletedWorkList, this.ValidateRisCompletedWork));
            return result;
        }

        private List<ValidateObjectResult> ValidateObjectList<T>(ICollection<T> objectList, Func<T, ValidateObjectResult> validateObjectFunc)
            where T : BaseRisEntity
        {
            var result = new List<ValidateObjectResult>();
            var objectsToRemove = new List<T>();

            foreach (var obj in objectList)
            {
                var validateResult = validateObjectFunc(obj);

                if (validateResult.State != ObjectValidateState.Success)
                {
                    result.Add(validateResult);
                    objectsToRemove.Add(obj);
                }
            }

            foreach (var objToRemove in objectsToRemove)
            {
                objectList.Remove(objToRemove);
            }

            return result;
        }

        private ValidateObjectResult ValidateRisCompletedWork(RisCompletedWork risCompletedWork)
        {
            var messages = new StringBuilder();

            if (risCompletedWork.ActDate == null)
            {
                messages.Append("RisCompletedWork.ActDate ");
            }

            if (risCompletedWork.ActFile == null)
            {
                messages.Append("RisCompletedWork.ActFile ");
            }

            if (string.IsNullOrEmpty(risCompletedWork.ActNumber))
            {
                messages.Append("RisCompletedWork.ActNumber ");
            }

            if (risCompletedWork.ObjectPhoto == null)
            {
                messages.Append("RisCompletedWork.ObjectPhoto ");
            }

            if (risCompletedWork.WorkPlanItem == null)
            {
                messages.Append("RisCompletedWork.WorkPlanItem ");
            }

            return new ValidateObjectResult
            {
                Id = risCompletedWork.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Акт выполненная работа"
            };
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importCompletedWorksRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importCompletedWorksRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var risCompletedWorkByWorkListItem in this.risCompletedWorkListByWorkListId)
            {
                var workCount = risCompletedWorkByWorkListItem.Value.Count;
                foreach (var risCompletedWorkByWorkListSection in risCompletedWorkByWorkListItem.Value.Section(CompletedWorkPrepareDataTask.portionSize))
                {
                    var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                    var request = this.CreateImportCompletedWorksRequest(
                        risCompletedWorkByWorkListItem.Key,
                        workCount,
                        risCompletedWorkByWorkListSection,
                        transportGuidDictionary);

                    result.Add(request, transportGuidDictionary);
                }
            }

            return result;
        }

        private importCompletedWorksRequest CreateImportCompletedWorksRequest(
            long workListId,
            int workCount,
            IEnumerable<RisCompletedWork> risCompletedWorkSection,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            importCompletedWorksRequest request = null;
            var workList = this.workListById.Get(workListId);
            var guidByIds = new Dictionary<string, long>();

            if (workList != null)
            {
                request = new importCompletedWorksRequest
                {
                    CompletedWorksByPeriod = new CompletedWorksByPeriodType
                    {
                        reportingPeriodGuid = workList.Guid
                    }
                };

                var plannedWorks = new List<CompletedWorksByPeriodTypePlannedWork>();
                var items = new List<object>();

                foreach (var risCompletedWork in risCompletedWorkSection)
                {
                    var item = this.CreateItem(risCompletedWork, guidByIds);
                    var plannedWork = this.CreatePlannedWork(risCompletedWork, item, workCount);

                    plannedWorks.Add(plannedWork);
                    items.Add(item);
                }

                request.CompletedWorksByPeriod.PlannedWork = plannedWorks.ToArray();
                request.CompletedWorksByPeriod.Items = items.ToArray();

                transportGuidDictionary.Add(typeof(RisCompletedWork), guidByIds);
            }

            return request;
        }

        private object CreateItem(RisCompletedWork risCompletedWork, Dictionary<string, long> guidByIds)
        {
            object obj = null;

            if (string.IsNullOrEmpty(risCompletedWork.Guid))
            {
                var roId = this.realityObjectIdByReportWorkId.Get(risCompletedWork.ExternalSystemEntityId);
                var act = new CompletedWorksByPeriodTypeNewAct
                {
                    Name = risCompletedWork.ActFile.Name,
                    Description = risCompletedWork.ActFile.Description,
                    Attachment = new Attachment
                    {
                        AttachmentGUID = risCompletedWork.ActFile.Hash
                    },
                    AttachmentHASH = risCompletedWork.ActFile.Hash,
                    Date = risCompletedWork.ActDate.Value,
                    Number = risCompletedWork.ActNumber,
                    TransportGUID = Guid.NewGuid().ToString(),
                    ContractNumber = this.contractNumberByRealityObjectId.Get(roId)
                };
                obj = act;
                guidByIds.Add(act.TransportGUID, risCompletedWork.Id);
            }
            else
            {
                var act = new CompletedWorksByPeriodTypeExistedAct
                {
                    TransportGUID = Guid.NewGuid().ToString(),
                    ActGUID = risCompletedWork.Guid
                };
                obj = act;
                guidByIds.Add(act.TransportGUID, risCompletedWork.Id);
            }

            return obj;
        }

        private CompletedWorksByPeriodTypePlannedWork CreatePlannedWork(RisCompletedWork risCompletedWork, object item, int workCount)
        {
            var work = new CompletedWorksByPeriodTypePlannedWork
            {
                WorkPlanItemGUID = risCompletedWork.WorkPlanItem.Guid,
                photos = this.CreatePhotoArray(risCompletedWork),
                MonthlyWork = new CompletedWorkTypeMonthlyWork
                {
                    count = workCount.ToStr(),
                    WorkDate = this.dataStartByReportWorkIds.Get(risCompletedWork.WorkPlanItem.WorkListItem.ExternalSystemEntityId)
                },
                plannedCount = workCount.ToStr()
            };

            if (item is CompletedWorksByPeriodTypeNewAct)
            {
                var newAct = item as CompletedWorksByPeriodTypeNewAct;
                work.ItemElementName = ItemChoiceType.ActTransportGUID;
                work.Item = newAct.TransportGUID;
            }
            else if (item is CompletedWorksByPeriodTypeExistedAct)
            {
                var existAct = item as CompletedWorksByPeriodTypeExistedAct;
                work.ItemElementName = ItemChoiceType.ActGUID;
                work.Item = existAct.ActGUID;
            }

            return work;
        }

        private AttachmentType[] CreatePhotoArray(RisCompletedWork risCompletedWork)
        {
            var attachmentList = new List<AttachmentType>();
            attachmentList.Add(
                new AttachmentType
                {
                    Name = risCompletedWork.ObjectPhoto.Name,
                    Description = risCompletedWork.ObjectPhoto.Description,
                    Attachment = new Attachment
                    {
                        AttachmentGUID = risCompletedWork.ObjectPhoto.Guid
                    },
                    AttachmentHASH = risCompletedWork.ObjectPhoto.Hash
                });
            return attachmentList.ToArray();
        }
    }
}