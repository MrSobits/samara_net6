namespace Bars.GisIntegration.Base.Tasks.PrepareData.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Services;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.ServicesAsync;
    using Bars.GisIntegration.Base.Tasks.PrepareData;

    /// <summary>
    /// Задача подготовки данных перечней работ и услуг
    /// </summary>
    public class WorkingListPrepareDataTask : BasePrepareDataTask<importWorkingListRequest>
    {
        private List<WorkList> workLists;
        private List<WorkListItem> workListItems;
        private Dictionary<long, List<WorkListItem>> workListItemsByWorkListId = null;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var workListExtractor = this.Container.Resolve<IDataExtractor<WorkList>>("WorkListExtractor");
            var workListItemExtractor = this.Container.Resolve<IDataExtractor<WorkListItem>>("WorkListItemExtractor");

            try
            {
                this.workLists = this.RunExtractor(workListExtractor, parameters);
                parameters.Add("workLists", this.workLists);

                this.workListItems = this.RunExtractor(workListItemExtractor, parameters);

                this.workListItemsByWorkListId = this.workListItems
                    .GroupBy(x => x.WorkList.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());
            }
            finally
            {
                this.Container.Release(workListExtractor);
                this.Container.Release(workListItemExtractor);
            }
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importWorkingListRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importWorkingListRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var workList in this.workLists)
            {
                var workListTransportGuids = new Dictionary<string, long>();
                var workListItemTransportGuids = new Dictionary<string, long>();

                var transportGuidsByType = new Dictionary<Type, Dictionary<string, long>>
                {
                    {typeof(WorkList), workListTransportGuids},
                    {typeof(WorkListItem), workListItemTransportGuids}
                };

                var requestWorkListItems = this.workListItemsByWorkListId.Get(workList.Id)
                    .Select(workListItem =>
                    {
                        var requestWorkListItem = new importWorkingListRequestApprovedWorkingListDataWorkListItem
                        {
                            ItemsElementName = new[] { ItemsChoiceType3.TotalCost },
                            Items = new object[] { workListItem.TotalCost },
                            WorkItemNSI = new nsiRef
                            {
                                Code = workListItem.WorkItemCode,
                                GUID = workListItem.WorkItemGuid
                            },
                            Index = workListItem.Index.ToString(),
                            TransportGUID = Guid.NewGuid().ToString()
                        };

                        workListItemTransportGuids.Add(requestWorkListItem.TransportGUID, workListItem.Id);

                        return requestWorkListItem;
                    })
                    .ToArray();

                var requestWorkListData = new importWorkingListRequestApprovedWorkingListData
                {
                    WorkListGUID = workList.Guid,
                    FIASHouseGuid = workList.House.FiasHouseGuid,
                    MonthYearFrom = new WorkingListBaseTypeMonthYearFrom
                    {
                        Month = workList.MonthFrom,
                        Year = workList.YearFrom
                    },
                    MonthYearTo = new WorkingListBaseTypeMonthYearTo
                    {
                        Year = workList.YearTo,
                        Month = workList.MonthTo
                    },
                    Attachment = new[]
                    {
                        new AttachmentType
                        {
                            Name = workList.Attachment.Name,
                            Description = workList.Attachment.Description,
                            AttachmentHASH = workList.Attachment.Hash,
                            Attachment = new ServicesAsync.Attachment
                            {
                                AttachmentGUID = workList.Attachment.Guid
                            }
                        }
                    },
                    ContractGUID = workList.Contract.Guid,
                    TransportGUID = Guid.NewGuid().ToString(),
                    WorkListItem = requestWorkListItems
                };

                var request = new importWorkingListRequest
                {
                    Item = requestWorkListData
                };

                workListTransportGuids.Add(requestWorkListData.TransportGUID, workList.Id);

                result.Add(request, transportGuidsByType);
            }

            return result;
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();

            result.AddRange(this.ValidateObjectList(this.workLists, this.ValidateWorkList));

            result.AddRange(this.ValidateObjectList(this.workListItems, this.ValidateWorkListItem));

            return result;
        }

        private ValidateObjectResult ValidateWorkList(WorkList workList)
        {
            var messages = new StringBuilder();

            if (string.IsNullOrEmpty(workList.House?.FiasHouseGuid))
            {
                messages.Append("FIASHOUSEGUID ");
            }

            if (workList.YearFrom == 0)
            {
                messages.Append("MONTHYEARFROM/YEAR ");
            }

            if (workList.MonthFrom == 0)
            {
                messages.Append("MONTHYEARFROM/MONTH ");
            }

            if (workList.YearTo == 0)
            {
                messages.Append("MONTHYEARTO/YEAR ");
            }

            if (workList.MonthTo == 0)
            {
                messages.Append("MONTHYEARTO/MONTH ");
            }

            if (workList.Attachment == null)
            {
                messages.Append("ATTACHMENT ");
            }

            if (workList.Contract == null)
            {
                messages.Append("CONTRACT ");
            }

            if (this.workListItemsByWorkListId.Get(workList.Id) == null)
            {
                messages.Append("WORKLISTITEMS ");
            }

            return new ValidateObjectResult
            {
                Id = workList.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Перечень работ/услуг"
            };
        }

        private ValidateObjectResult ValidateWorkListItem(WorkListItem workListItem)
        {
            var messages = new StringBuilder();

            if (workListItem.WorkList == null || !this.workLists.Contains(workListItem.WorkList))
            {
                messages.Append("WORKLIST ");
            }

            if (workListItem.WorkItemCode.IsEmpty())
            {
                messages.Append("WORKITEMCODE ");
            }

            if (workListItem.WorkItemGuid.IsEmpty())
            {
                messages.Append("WORKITEMGUID ");
            }

            if (workListItem.Index <= 0)
            {
                messages.Append("INDEX ");
            }

            return new ValidateObjectResult
            {
                Id = workListItem.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Работа/услуга перечня"
            };
        }
    }
}
