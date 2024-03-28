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

    /// <summary>
    /// Задача подготовки данных
    /// </summary>
    public class WorkingPlanPrepareDataTask : BasePrepareDataTask<importWorkingPlanRequest>
    {
        /// <summary>
        /// Максимальное количество записей, которые можно экспортировать одним запросом
        /// </summary>
        private const int portionSize = 100;

        private List<WorkingPlan> workingPlanList;
        private List<WorkPlanItem> workPlanItems;
        private Dictionary<long, List<WorkPlanItem>> workPlantemsByWorkListId;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var workingPlanExtractor = this.Container.Resolve<IDataExtractor<WorkingPlan>>("WorkingPlanExtractor");
            var workPlanItemExtractor = this.Container.Resolve<IDataExtractor<WorkPlanItem>>("WorkPlanItemExtractor");

            try
            {
                this.workingPlanList = this.RunExtractor(workingPlanExtractor, parameters);

                parameters.Add("workingPlan", this.workingPlanList);

                this.workPlanItems = this.RunExtractor(workPlanItemExtractor, parameters);

                this.workPlantemsByWorkListId = this.workPlanItems
                    .GroupBy(x => x.WorkingPlan.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());
            }
            finally
            {
                this.Container.Release(workingPlanExtractor);
                this.Container.Release(workPlanItemExtractor);
            }
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importWorkingPlanRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importWorkingPlanRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var workingPlanSection in this.workingPlanList.Section(WorkingPlanPrepareDataTask.portionSize))
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.CreateImportWorkingPlanRequest(workingPlanSection, transportGuidDictionary);

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }

        /// <summary>
        /// Создание объекта запроса importSubsidiaryRequest
        /// </summary>
        /// <param name="workingPlans">Список сведений об обособленных подразделениях</param>
        /// <param name="transportGuidDictionary">Словарь транспортных гуидов</param>
        /// <returns></returns>
        private importWorkingPlanRequest CreateImportWorkingPlanRequest(
            IEnumerable<WorkingPlan> workingPlans,
            IDictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var workingPlanRequest = new List<WorkingPlanType>();
            var workingPlanTransportGuidDictionary = new Dictionary<string, long>();
            var workPlanListTransportGuidDictionary = new Dictionary<string, long>();

            foreach (var workingPlan in workingPlans)
            {
                var workPlans = this.workPlantemsByWorkListId.Get(workingPlan.Id);

                WorkingPlanTypeWorkPlanItem[] data = null;

                if (workPlans.IsNotEmpty())
                {
                    data = workPlans.Select(
                        x =>
                        {
                            var opGuid = Guid.NewGuid().ToString();
                            workPlanListTransportGuidDictionary.Add(opGuid, x.Id);

                            return new WorkingPlanTypeWorkPlanItem
                            {
                                TransportGUID = opGuid,
                                Month = x.Month,
                                Year = x.Year,
                                WorkListItemGUID = x.WorkListItem.Guid,
                                Items = new object[] {x.WorkDate}
                            };
                        })
                        .ToArray();
                }

                var item = new WorkingPlanType
                {
                    TransportGUID = Guid.NewGuid().ToString(),
                    WorkListGUID = workingPlan.WorkList.Guid,
                    Year = workingPlan.Year,
                    WorkPlanItem = data
                };

                workingPlanRequest.Add(item);
            }

            transportGuidDictionary.Add(typeof(WorkingPlan), workingPlanTransportGuidDictionary);
            transportGuidDictionary.Add(typeof(WorkPlanItem), workPlanListTransportGuidDictionary);

            return new importWorkingPlanRequest { WorkingPlan = workingPlanRequest.ToArray() };
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();
            result.AddRange(WorkingPlanPrepareDataTask.ValidateObjectList(this.workingPlanList, this.ValidateWorkingPlan));
            result.AddRange(WorkingPlanPrepareDataTask.ValidateObjectList(this.workPlanItems, this.ValidateWorkingPlan));

            return result;
        }

        private static List<ValidateObjectResult> ValidateObjectList<T>(ICollection<T> objectList, Func<T, ValidateObjectResult> validateObjectFunc)
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

        private ValidateObjectResult ValidateWorkingPlan(WorkingPlan workingPlan)
        {
            var messages = new StringBuilder();

            if (workingPlan.WorkList == null)
            {
                messages.Append($"{nameof(workingPlan.WorkList)} ");
            }

            if (workingPlan.Year == 0)
            {
                messages.Append($"{nameof(workingPlan.Year)} ");
            }

            return new ValidateObjectResult
            {
                Id = workingPlan.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "План по перечню работ/услуг"
            };
        }

        private ValidateObjectResult ValidateWorkingPlan(WorkPlanItem workPlanItem)
        {
            var messages = new StringBuilder();

            if (workPlanItem.WorkingPlan == null)
            {
                messages.Append($"{nameof(workPlanItem.WorkingPlan)} ");
            }

            if (workPlanItem.WorkListItem == null)
            {
                messages.Append($"{nameof(workPlanItem.WorkListItem)} ");
            }

            if (workPlanItem.Year == 0)
            {
                messages.Append($"{nameof(workPlanItem.Year)} ");
            }

            if (workPlanItem.Month < 1 || workPlanItem.Month > 12)
            {
                messages.Append($"{nameof(workPlanItem.Year)} ");
            }

            if (workPlanItem.WorkDate == default(DateTime) || workPlanItem.WorkCount == 0)
            {
                messages.Append($"{nameof(workPlanItem.Year)}/{nameof(workPlanItem.WorkCount)} ");
            }

            return new ValidateObjectResult
            {
                Id = workPlanItem.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "План работ по перечню работ/услуг"
            };
        }
    }
}
