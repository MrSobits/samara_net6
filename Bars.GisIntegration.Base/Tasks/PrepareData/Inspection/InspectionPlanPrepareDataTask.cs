namespace Bars.GisIntegration.Base.Tasks.PrepareData.Inspection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.InspectionAsync;
    using Bars.GisIntegration.Base.Tasks.PrepareData;

    /// <summary>
    /// Задача подготовки данных по договорам
    /// </summary>
    public class InspectionPlanPrepareDataTask : BasePrepareDataTask<importInspectionPlanRequest>
    {
        private List<InspectionPlan> plans;
        private List<Examination> examinations;

        /// <summary>
        /// Размер блока предаваемых данных (максимальное количество записей)
        /// </summary>
        private const int Portion = 1000;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var inspectionPlanExtractor = this.Container.Resolve<IDataExtractor<InspectionPlan>>("InspectionPlanExtractor");
            var examinationExtractor = this.Container.Resolve<IDataExtractor<Examination>>("InspectionPlanExaminationExtractor");

            try
            {
                this.plans = this.RunExtractor(inspectionPlanExtractor, parameters);
                parameters.Add("selectedPlans", this.plans);

                this.examinations = this.RunExtractor(examinationExtractor, parameters);
            }
            finally
            {
                this.Container.Release(inspectionPlanExtractor);
                this.Container.Release(examinationExtractor);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();

            result.AddRange(this.ValidateObjectList(this.plans, this.CheckPlan));
            result.AddRange(this.ValidateObjectList(this.examinations, this.CheckExamination));

            return result;
        }

        /// <summary>
        /// Проверка проверки перед импортом
        /// </summary>
        /// <param name="examination">Проверка</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckExamination(Examination examination)
        {
            var messages = new StringBuilder();

            if (!examination.InspectionNumber.HasValue)
            {
                messages.Append("InspectionNumber ");
            }

            if (examination.GisContragent == null || !examination.GisContragent.OrganizationType.HasValue)
            {
                messages.Append("Subject ");
            }

            if (examination.Objective.IsEmpty())
            {
                messages.Append("Objective ");
            }

            if (examination.BaseCode.IsEmpty() || examination.BaseGuid.IsEmpty())
            {
                messages.Append("Base ");
            }

            if (!examination.From.HasValue || examination.From == DateTime.MinValue)
            {
                messages.Append("MonthFrom ");
                messages.Append("YearFrom ");
            }

            if (examination.ExaminationFormCode.IsEmpty() || examination.ExaminationFormGuid.IsEmpty())
            {
                messages.Append("ExaminationForm ");
            }

            return new ValidateObjectResult
            {
                Id = examination.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Проверка"
            };
        }

        /// <summary>
        /// Проверка плана перед импортом
        /// </summary>
        /// <param name="plan">План</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckPlan(InspectionPlan plan)
        {
            var messages = new StringBuilder();

            if (!plan.Year.HasValue)
            {
                messages.Append("Year ");
            }

            return new ValidateObjectResult
            {
                Id = plan.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "План проверки"
            };
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importInspectionPlanRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importInspectionPlanRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();

                var request = this.GetRequestObject(iterationList, transportGuidDictionary);
                request.Id = Guid.NewGuid().ToString();

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }

        /// <summary>
        /// Получить объект запроса
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта</param>
        /// <param name="transportGuidDictionary">Список объектов для импорта</param>
        /// <returns>Объект запроса</returns>
        private importInspectionPlanRequest GetRequestObject(IEnumerable<InspectionPlan> listForImport, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var planList = new List<importInspectionPlanRequestImportInspectionPlan>();
            transportGuidDictionary.Add(typeof(InspectionPlan), new Dictionary<string, long>());
            transportGuidDictionary.Add(typeof(Examination), new Dictionary<string, long>());

            foreach (var plan in listForImport)
            {
                var planItem = this.GetImportInspectionPlanRequestPlan(plan, transportGuidDictionary);
                planList.Add(planItem);
            }

            return new importInspectionPlanRequest { ImportInspectionPlan = planList.ToArray() };
        }

        /// <summary>
        /// Получить раздел ImportInspectionPlan
        /// </summary>
        /// <param name="plan">План проверок</param>
        /// <param name="transportGuidDictionary">Словать транспортных идентификаторов</param>
        /// <returns>Раздел ImportInspectionPlan</returns>
        private importInspectionPlanRequestImportInspectionPlan GetImportInspectionPlanRequestPlan(InspectionPlan plan, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            object inspectionPlanItem;

            if (!plan.UriRegistrationNumber.HasValue)
            {
                inspectionPlanItem = true;
            }
            else
            {
                inspectionPlanItem = (plan.UriRegistrationNumber ?? 0).ToString();
            }

            var importPlannedExamination = this.GetImportInspectionPlanRequestExamination(plan.Id, transportGuidDictionary);

            var items = new List<object>();
            var itemsElementName = new List<ItemsChoiceType3>();

            if (plan.Operation != RisEntityOperation.Delete)
            {
                items.Add(true); //Sing
                items.Add(
                    new InspectionPlanType
                    {
                        Year = (short)(plan.Year ?? 0),
                        Item = inspectionPlanItem
                    });

                itemsElementName.Add(ItemsChoiceType3.Sign);
                itemsElementName.Add(ItemsChoiceType3.InspectionPlan);

                for (int i = 0; i < importPlannedExamination.Length; i++)
                {
                    items.Add(importPlannedExamination[i]);
                    itemsElementName.Add(ItemsChoiceType3.ImportPlannedExamination);
                }
            }
            else
            {
                // Удаление плана проверок. Также удаляются все плановые проверки: не передавать!!! (из xml)
            }

            var result = new importInspectionPlanRequestImportInspectionPlan
            {
                TransportGUID = Guid.NewGuid().ToString(),
                Items = items.ToArray(),
                ItemsElementName = itemsElementName.ToArray()

            };

            if (plan.Operation == RisEntityOperation.Update)
            {
                result.InspectionPlanGuid = plan.Guid;
            }
            transportGuidDictionary[typeof(InspectionPlan)].Add(result.TransportGUID, plan.Id);

            return result;
        }

        /// <summary>
        /// Получить Item раздела Subject
        /// </summary>
        /// <param name="contragent">Контрагент проверки</param>
        /// <returns>Item раздела Subject</returns>
        private RegOrgType GetSubjectItem(RisContragent contragent)
        {
            RegOrgType result = null;

            if (contragent?.OrganizationType == GisOrganizationType.Legal)
            {
                result = new ScheduledExaminationSubjectInPlanInfoTypeOrganization
                {
                    orgRootEntityGUID = contragent.OrgRootEntityGuid,
                    ActualActivityPlace = contragent.FactAddress
                };
            }
            else if (contragent?.OrganizationType == GisOrganizationType.Entps)
            {
                result = new ScheduledExaminationSubjectInPlanInfoTypeIndividual
                {
                    orgRootEntityGUID = contragent.OrgRootEntityGuid,
                    ActualActivityPlace = contragent.FactAddress
                };
            }

            return result;
        }

        /// <summary>
        /// Получить раздел ImportPlannedExamination
        /// </summary>
        /// <param name="planId">Идентификатор плана проверок</param>
        /// <param name="transportGuidDictionary">Словать транспортных идентификаторов</param>
        /// <returns>Раздел ImportPlannedExamination</returns>
        private importInspectionPlanRequestImportInspectionPlanImportPlannedExamination[] GetImportInspectionPlanRequestExamination(long planId, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var result = new List<importInspectionPlanRequestImportInspectionPlanImportPlannedExamination>();

            foreach (var examination in this.examinations.Where(x => x.InspectionPlan.Id == planId))
            {
                var examinationItem = new importInspectionPlanRequestImportInspectionPlanImportPlannedExamination
                {
                    TransportGUID = Guid.NewGuid().ToString(),
                    PlannedExamination = new PlannedExaminationType
                    {
                        PlannedExaminationOverview = new PlannedExaminationTypePlannedExaminationOverview
                        {
                            NumberInPlan = examination.InspectionNumber ?? 0,
                            URIRegistrationNumber = (examination.UriRegistrationNumber ?? 0).ToString(),
                            URIRegistrationDate = examination.UriRegistrationDate ?? DateTime.MinValue,
                            URIRegistrationDateSpecified = examination.UriRegistrationDate.HasValue
                        },
                        Subject = new ScheduledExaminationSubjectInPlanInfoType
                        {
                            Item = this.GetSubjectItem(examination.GisContragent)
                        },
                        PlannedExaminationInfo = new PlannedExaminationTypePlannedExaminationInfo
                        {
                            Objective = examination.Objective,
                            Base = new nsiRef
                            {
                                Code = examination.BaseCode,
                                GUID = examination.BaseGuid
                            },
                            MonthFrom = (examination.From ?? DateTime.MinValue).Month,
                            YearFrom = (short)(examination.From ?? DateTime.MinValue).Year,
                            Duration = new PlannedExaminationTypePlannedExaminationInfoDuration
                            {
                                ItemElementName = ItemChoiceType.WorkDays,
                                Item = examination.Duration ?? 0
                            },
                            ExaminationForm = new nsiRef
                            {
                                Code = examination.ExaminationFormCode,
                                GUID = examination.ExaminationFormGuid
                            },
                            ProsecutorAgreementInformation = examination.ProsecutorAgreementInformation
                        }
                    }
                };

                if (examination.Operation == RisEntityOperation.Delete)
                {
                    examinationItem.DeletePlannedExamination = true;
                    examinationItem.DeletePlannedExaminationSpecified = true;
                }
                else if (examination.Operation == RisEntityOperation.Update)
                {
                    examinationItem.PlannedExaminationGuid = examination.Guid;
                }

                result.Add(examinationItem);
                transportGuidDictionary[typeof(Examination)].Add(examinationItem.TransportGUID, examination.Id);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получает список порций объектов ГИС для формирования объектов для запроса
        /// </summary>
        /// <returns>Список порций объектов ГИС</returns>
        private List<IEnumerable<InspectionPlan>> GetPortions()
        {
            var result = new List<IEnumerable<InspectionPlan>>();

            //TODO: придумать, как учесть ограничение - не более 1000 проверок в одном плане

            if (this.plans.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(this.plans.Skip(startIndex).Take(InspectionPlanPrepareDataTask.Portion));
                    startIndex += InspectionPlanPrepareDataTask.Portion;
                }
                while (startIndex < this.plans.Count);
            }

            return result;
        }
    }
}