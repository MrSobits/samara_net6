namespace Bars.GisIntegration.Inspection.DataExtractors.Examination
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Inspection.Enums;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Экстрактор проверок
    /// </summary>
    public class ExaminationExtractor : BaseDataExtractor<Examination, Disposal>
    {
        /// <summary>
        /// Менеджер справочников
        /// </summary>
        public IDictionaryManager DictionaryManager { get; set; }

        /// <summary>
        /// Словарь контрагентов
        /// </summary>
        private Dictionary<long, RisContragent> risContragentByGkhId;

        /// <summary>
        /// Список ЖКХ контрагентов
        /// </summary>
        private Dictionary<long, Contragent> gkhContragentsById;

        /// <summary>
        /// Инспекторы распоряжений
        /// </summary>
        private Dictionary<long, string> inspectorsByDisposalId;

        /// <summary>
        /// Переданные в ГИС предписания по идентификаторам ЖКХ
        /// </summary>
        private Dictionary<long, string> preceptionsByPrescriptionId;

        /// <summary>
        /// Наименование первой цели обследования по индентификатору распоряжения
        /// </summary>
        private Dictionary<long, string> purposeNameByDisposalId;

        /// <summary>
        /// Наименование первой задачи обследования по индентификатору распоряжения
        /// </summary>
        private Dictionary<long, string> taskNameByDisposalId;

        /// <summary>
        /// Нарушения по идентификатору документа (акта или протокола)
        /// </summary>
        private Dictionary<long, string> violationsByDocId;

        /// <summary>
        /// Дома по актам
        /// </summary>
        private Dictionary<long, string> roByActCheckId;

        /// <summary>
        /// Лица, присутствующие при проверке, по актам
        /// </summary>
        private Dictionary<long, string> witnessesByActCheckId;

        /// <summary>
        /// Периоды проверок актов
        /// </summary>
        private Dictionary<long, List<DateTime>> actCheckPeriodByActId;

        /// <summary>
        /// Акты по этапу родителя (распоряжения)
        /// </summary>
        private Dictionary<InspectionGjiStage, ActCheck> actChecksByDisposalStage;

        /// <summary>
        /// Протоколы по этапу родителя (распоряжения)
        /// </summary>
        private Dictionary<InspectionGjiStage, Protocol> protocolsByDisposalStage;

        /// <summary>
        /// Cправочник "Вид осуществления контрольной деятельности"
        /// </summary>
        private IDictionary oversightActivityDict;

        /// <summary>
        /// Справочник "Форма проведения проверки (Основания проверок = \"Плановые проверки юридических лиц\", \"Проверки по требованию прокуратуры\", \"Проверки по поручению руководителей органов государственного контроля\")"
        /// </summary>
        private IDictionary examinationJurPersonFormDictionary;

        /// <summary>
        /// Справочник "Форма проведения проверки (Основания проверок = "Проверки по обращениям граждан", "Проверки соискаталей лицензии")"
        /// </summary>
        private IDictionary examinationFormDictionary;

        /// <summary>
        /// Справочник "Основание проведения проверки юр. лица"
        /// </summary>
        private IDictionary typeBaseJurPersonDictionary;

        /// <summary>
        /// Справочник "Предмет проверки"
        /// </summary>
        private IDictionary examinationObjectDictionary;

        /// <summary>
        /// Справочник "Вид документа по результатам проверки"
        /// </summary>
        private IDictionary examinationResultDocTypeDictionary;

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Например, подготовить словари с данными
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            var risContragentDomain = this.Container.ResolveDomain<RisContragent>();
            var preceptDomain = this.Container.ResolveDomain<Precept>();
            var contragentDomain = this.Container.ResolveRepository<Contragent>();
            try
            {
                this.risContragentByGkhId = risContragentDomain.GetAll()
                    .AsEnumerable()
                    .GroupBy(x => x.GkhId)
                    .ToDictionary(x => x.Key, y => y.First());

                var gkhContragentIdList = this.risContragentByGkhId.Keys.ToList();

                this.gkhContragentsById = contragentDomain.GetAll()
                    .Where(x => gkhContragentIdList.Contains(x.Id))
                    .AsEnumerable()
                    .ToDictionary(x => x.Id);

                this.preceptionsByPrescriptionId = preceptDomain.GetAll()
                    .Where(x => x.Guid != null && x.ExternalSystemEntityId != 0)
                    .Select(
                        x => new
                        {
                            x.ExternalSystemEntityId,
                            x.Guid
                        })
                    .ToList()
                    .GroupBy(x => x.ExternalSystemEntityId)
                    .ToDictionary(x => x.Key, x => x.First().Guid);
            }
            finally
            {
                this.Container.Release(risContragentDomain);
                this.Container.Release(preceptDomain);
                this.Container.Release(contragentDomain);
            }

            this.oversightActivityDict = this.DictionaryManager.GetDictionary("OversightActivityTypeDictionary");
            this.examinationJurPersonFormDictionary = this.DictionaryManager.GetDictionary("ExaminationJurPersonFormDictionary");
            this.examinationFormDictionary = this.DictionaryManager.GetDictionary("ExaminationFormDictionary");
            this.typeBaseJurPersonDictionary = this.DictionaryManager.GetDictionary("TypeBaseJurPersonDictionary");
            this.examinationObjectDictionary = this.DictionaryManager.GetDictionary("ExaminationObjectDictionary");
            this.examinationResultDocTypeDictionary = this.DictionaryManager.GetDictionary("ExaminationResultDocTypeDictionary");
        }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<Disposal> GetExternalEntities(DynamicDictionary parameters)
        {
            var selectedList = parameters.GetAs("selectedList", string.Empty);
            var selectedIds = selectedList.ToUpper() == "ALL" ? null : selectedList.ToLongArray();
            var fromWizard = parameters.GetAs<bool>("fromWizard");

            var disposalDomain = this.Container.ResolveDomain<Disposal>();
            var actCheckDomain = this.Container.ResolveDomain<ActCheck>();
            var documentGjiInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
            var typeSurveyGoalInspGjiDomain = this.Container.ResolveDomain<TypeSurveyGoalInspGji>();
            var typeSurveyTaskInspGjiDomain = this.Container.ResolveDomain<TypeSurveyTaskInspGji>();
            var disposalTypeSurveyDomain = this.Container.ResolveDomain<DisposalTypeSurvey>();
            var protocolDomain = this.Container.ResolveDomain<Protocol>();
            var actCheckWitnessDomain = this.Container.ResolveDomain<ActCheckWitness>();
            var inspectionGjiViolStageDomain = this.Container.ResolveDomain<InspectionGjiViolStage>();
            var actCheckPeriodDomain = this.Container.ResolveDomain<ActCheckPeriod>();
            var actCheckRealityObjectDomain = this.Container.ResolveDomain<ActCheckRealityObject>();

            try
            {
                var actCheckParentStageIds = actCheckDomain.GetAll()
                    .Where(x => x.Inspection != null && x.Stage != null && x.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Select(x => x.Stage.Parent.Id).ToList(); // родительские этапы актов проверок (для отсева распоряжений)

                var disposals = disposalDomain.GetAll()
                    .WhereIfContains(selectedIds != null, x => x.Id, selectedIds)
                    .Where(x => x.Inspection != null && x.TypeDocumentGji == TypeDocumentGji.Disposal && x.Stage != null)
                    .Where( // допустимые типы проверок
                        x =>
                            x.Inspection.TypeBase == TypeBase.PlanJuridicalPerson ||
                                x.Inspection.TypeBase == TypeBase.ProsecutorsClaim ||
                                x.Inspection.TypeBase == TypeBase.DisposalHead ||
                                x.Inspection.TypeBase == TypeBase.CitizenStatement ||
                                x.Inspection.TypeBase == TypeBase.Inspection ||
                                x.Inspection.TypeBase == TypeBase.LicenseApplicants
                    )
                    .Select(
                        x => new
                        {
                            x.Inspection,
                            x.Stage.Position,
                            Disposal = x
                        })
                    .ToList()
                    .GroupBy(x => x.Inspection)
                    .Select(x => x.OrderBy(z => z.Position).First().Disposal) // первые распоряжения по проверкам
                    .Where(x => actCheckParentStageIds.Contains(x.Stage.Id)) // проверка на созданные акты
                    .ToList();

                if (!fromWizard) // эти данные нужны только для сохранения
                {
                    var disposalStages = disposals.Select(x => x.Stage); // родительские этапы для этапов актов
                    var disposalIds = disposals.Select(x => x.Id).ToList(); // идентификаторы распоряжений

                    this.inspectorsByDisposalId = documentGjiInspectorDomain.GetAll()
                        .Where(
                            x =>
                                x.DocumentGji != null && x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Disposal
                                    && disposalIds.Contains(x.DocumentGji.Id))
                        .Select(
                            x => new
                            {
                                x.DocumentGji.Id,
                                Inspector = x.Inspector.Fio + " " + x.Inspector.Position
                            })
                        .ToList()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, x => string.Join(", ", x.Select(y => y.Inspector)));

                    var surveyGoalByTypeId = typeSurveyGoalInspGjiDomain.GetAll()
                        .Where(x => x.SurveyPurpose != null && x.TypeSurvey != null)
                        .Select(
                            x => new
                            {
                                PurposeName = x.SurveyPurpose.Name,
                                TypeSurveyId = x.TypeSurvey.Id
                            })
                        .ToList()
                        .GroupBy(x => x.TypeSurveyId)
                        .ToDictionary(x => x.Key, x => x.First().PurposeName);

                    var surveyTaskByTypeId = typeSurveyTaskInspGjiDomain.GetAll()
                        .Where(x => x.SurveyObjective != null && x.TypeSurvey != null)
                        .Select(
                            x => new
                            {
                                TaskName = x.SurveyObjective.Name,
                                TypeSurveyId = x.TypeSurvey.Id
                            })
                        .ToList()
                        .GroupBy(x => x.TypeSurveyId)
                        .ToDictionary(x => x.Key, x => x.First().TaskName);

                    var typeSurveyByDisposal = disposalTypeSurveyDomain.GetAll()
                        .Where(x => x.Disposal != null && x.TypeSurvey != null && disposalIds.Contains(x.Disposal.Id))
                        .Select(
                            x => new
                            {
                                DisposalId = x.Disposal.Id,
                                TypeSurveyId = x.TypeSurvey.Id
                            })
                        .ToList()
                        .GroupBy(x => x.DisposalId)
                        .ToDictionary(x => x.Key, x => x.First().TypeSurveyId);

                    this.purposeNameByDisposalId = typeSurveyByDisposal.ToDictionary(x => x.Key, x => surveyGoalByTypeId.Get(x.Value));
                    this.taskNameByDisposalId = typeSurveyByDisposal.ToDictionary(x => x.Key, x => surveyTaskByTypeId.Get(x.Value));

                    // найти первые сформированные по распоряжениям акты (связь по родительским этапам)
                    // найти первые сформированные по актам протоколы
                    // составить словари: первый акт - распоряжение (этап распоряжения), первый протокол акта - распоряжение (этап распоряжения)
                    this.actChecksByDisposalStage = actCheckDomain.GetAll()
                        .Where(x => x.Stage != null && x.Stage.Parent != null && disposalStages.Contains(x.Stage.Parent))
                        .Select(
                            x => new
                            {
                                x.Stage.Position,
                                ParentStage = x.Stage.Parent,
                                Act = x
                            })
                        .ToList()
                        .GroupBy(x => x.ParentStage)
                        .ToDictionary(x => x.Key, x => x.OrderBy(y => y.Position).First().Act);

                    this.protocolsByDisposalStage = protocolDomain.GetAll()
                        .Where(x => x.Stage != null && x.Stage.Parent != null && disposalStages.Contains(x.Stage.Parent))
                        .Select(
                            x => new
                            {
                                x.Stage.Position,
                                ParentStage = x.Stage.Parent,
                                Protocol = x
                            })
                        .ToList()
                        .GroupBy(x => x.ParentStage)
                        .ToDictionary(x => x.Key, x => x.OrderBy(y => y.Position).First().Protocol);

                    var actCheckIds = this.actChecksByDisposalStage.Values.Select(y => y.Id).ToList();

                    this.witnessesByActCheckId = actCheckWitnessDomain.GetAll()
                        .Where(x => x.ActCheck != null && x.IsFamiliar && actCheckIds.Contains(x.ActCheck.Id))
                        .Select(
                            x => new
                            {
                                ActCheckId = x.ActCheck.Id,
                                Person = x.Fio + " " + x.Position
                            })
                        .ToList()
                        .GroupBy(x => x.ActCheckId)
                        .ToDictionary(
                            x => x.Key,
                            x => string.Join(" ,", x.Select(y => y.Person)));


                    this.roByActCheckId = actCheckRealityObjectDomain.GetAll()
                        .Where(x => x.ActCheck != null && x.RealityObject != null && actCheckIds.Contains(x.ActCheck.Id))
                        .Select(
                            x => new
                            {
                                ActCheckId = x.ActCheck.Id,
                                RealityObjectAddress = x.RealityObject.Address
                            })
                        .ToList()
                        .GroupBy(x => x.ActCheckId)
                        .Where(x => x.Count() == 1)

                        //согласно логике ActCheckService.GetInfo - адрес заполняется только если для акта есть одна запись actCheckRealityObject
                        .ToDictionary(x => x.Key, x => x.First().RealityObjectAddress);

                    var protocolIds = this.protocolsByDisposalStage.Values.Select(y => y.Id).ToList();

                    this.violationsByDocId = inspectionGjiViolStageDomain.GetAll()
                        .Where(x => x.Document != null && x.InspectionViolation != null && x.InspectionViolation.Violation != null)
                        .Where(
                            x =>
                                protocolIds.Contains(x.Document.Id)
                                    || actCheckIds.Contains(x.Document.Id))
                        .Select(
                            x => new
                            {
                                DocumentId = x.Document.Id,
                                x.InspectionViolation.Violation.Name
                            })
                        .ToList()
                        .GroupBy(x => x.DocumentId)
                        .ToDictionary(x => x.Key, x => string.Join(", ", x.Select(y => y.Name)));

                    this.actCheckPeriodByActId = actCheckPeriodDomain.GetAll()
                        .Where(x => x.ActCheck != null && x.DateCheck != null && actCheckIds.Contains(x.ActCheck.Id))
                        .Select(
                            x => new
                            {
                                ActCheckId = x.ActCheck.Id,
                                DateCheck = x.DateCheck ?? DateTime.MinValue
                            })
                        .ToList()
                        .GroupBy(x => x.ActCheckId)
                        .ToDictionary(x => x.Key, x => x.Select(y => y.DateCheck).ToList());
                }

                return disposals;
            }
            finally
            {
                this.Container.Release(disposalDomain);
                this.Container.Release(actCheckDomain);
                this.Container.Release(protocolDomain);
                this.Container.Release(inspectionGjiViolStageDomain);
                this.Container.Release(actCheckPeriodDomain);
                this.Container.Release(actCheckRealityObjectDomain);
                this.Container.Release(disposalTypeSurveyDomain);
                this.Container.Release(typeSurveyGoalInspGjiDomain);
                this.Container.Release(typeSurveyTaskInspGjiDomain);
                this.Container.Release(documentGjiInspectorDomain);
                this.Container.Release(actCheckWitnessDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="disposal">Сущность внешней системы</param>
        /// <param name="examination">Ris сущность</param>
        protected override void UpdateRisEntity(Disposal disposal, Examination examination)
        {
            var inspection = disposal.Inspection;
            var risContragent = this.risContragentByGkhId.Get(inspection?.Contragent?.Id ?? 0);
            var gkhFrguContragent = this.gkhContragentsById.Get(this.Contragent.GkhId);

            var fio = inspection?.PhysicalPerson?.Split(' ', '.');
            var oversightActivityId = (long)(inspection?.TypeBase == TypeBase.LicenseApplicants ? OversightActivityType.LicenseControl : OversightActivityType.StateAndMunicipalControl);
            var oversightActivity = this.oversightActivityDict.GetDictionaryRecord(oversightActivityId);
            var examinationForm = this.GetExaminationForm(inspection);
            var objectItem = this.examinationObjectDictionary.GetDictionaryRecord((long)ExaminationObject.Compliance); // всегда передаём одно и то же
            var resultInfoDocument = this.GetResultInfoDocument(disposal);
            var resultDocumentType = this.examinationResultDocTypeDictionary.GetDictionaryRecord((long)
                (resultInfoDocument?.TypeDocumentGji == TypeDocumentGji.ActCheck ? ExaminationResultDocType.ActCheck :
                (resultInfoDocument?.TypeDocumentGji == TypeDocumentGji.Protocol ? ExaminationResultDocType.Protocol : 0)));
            var offences = this.violationsByDocId?.Get(resultInfoDocument?.Id ?? 0);
            var actCheck = this.actChecksByDisposalStage?.Get(disposal.Stage);
            var actAddress = this.roByActCheckId?.Get(actCheck?.Id ?? 0);
            var actWitness = this.witnessesByActCheckId?.Get(actCheck?.Id ?? 0);

            examination.ExternalSystemEntityId = disposal.Id;
            examination.ExternalSystemName = "gkh";
            examination.ShouldNotBeRegistered = false;
            examination.IsScheduled = (inspection?.TypeBase == TypeBase.PlanJuridicalPerson || inspection?.TypeBase == TypeBase.Inspection);

            examination.SubjectType =
                inspection?.PersonInspection == PersonInspection.PhysPerson
                ? ExaminationSubjectType.Citizen
                : (inspection?.Contragent?.OrganizationForm.Name == "Индивидуальные предприниматели"
                    ? ExaminationSubjectType.Individual
                    : ExaminationSubjectType.Organization);

            examination.GisContragent = risContragent;
            examination.LastName = fio?.Length > 0 ? fio[0] : string.Empty;
            examination.FirstName = fio?.Length > 1 ? fio[1] : string.Empty;

            examination.OversightActivitiesCode = oversightActivity?.GisCode;
            examination.OversightActivitiesGuid = oversightActivity?.GisGuid;

            examination.ExaminationFormCode = examinationForm?.GisCode;
            examination.ExaminationFormGuid = examinationForm?.GisGuid;

            examination.OrderNumber = disposal.DocumentNumber + (disposal.DocumentSubNum.HasValue ? "/" + disposal.DocumentSubNum : string.Empty);
            examination.OrderDate = disposal.DocumentDate;
            examination.FunctionRegistryNumber = gkhFrguContragent?.FrguRegNumber;
            examination.AuthorizedPersons = disposal.ResponsibleExecution?.Fio + ", " + disposal.ResponsibleExecution?.Position;
            examination.InvolvedExperts = this.inspectorsByDisposalId?.Get(disposal.Id);

            if (inspection?.TypeBase == TypeBase.PlanJuridicalPerson)
            {
                var typeBase = this.typeBaseJurPersonDictionary.GetDictionaryRecord((long)((inspection as BaseJurPerson)?.TypeBaseJuralPerson ?? 0));

                examination.BaseCode = typeBase?.GisCode;
                examination.BaseGuid = typeBase?.GisGuid;
            }

            if (inspection?.TypeBase == TypeBase.DisposalHead && (inspection as BaseDispHead)?.TypeBaseDispHead == TypeBaseDispHead.FailureRemoveViolation)
            {
                var previousDocument = (inspection as BaseDispHead).PrevDocument;

                if (previousDocument.TypeDocumentGji == TypeDocumentGji.Prescription)
                {
                    examination.PreceptGuid = this.preceptionsByPrescriptionId?.Get(previousDocument.Id);
                }
            }

            examination.Objective = this.purposeNameByDisposalId?.Get(disposal.Id);
            examination.Tasks = this.taskNameByDisposalId?.Get(disposal.Id);

            examination.ObjectCode = objectItem?.GisCode;
            examination.ObjectGuid = objectItem?.GisGuid;

            examination.From = disposal.DateStart;
            examination.To = disposal.DateEnd;

            if (disposal.DateStart.HasValue && disposal.DateEnd.HasValue)
            {
                examination.Duration = (double)(disposal.DateEnd.Value - disposal.DateStart.Value).Days + 1;
            }

            examination.ProsecutorAgreementInformation = disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement
                ? disposal.TypeAgreementResult.GetDescriptionName()
                : string.Empty;

            examination.ResultDocumentTypeCode = resultDocumentType?.GisCode;
            examination.ResultDocumentTypeGuid = resultDocumentType?.GisGuid;

            examination.ResultDocumentNumber = resultInfoDocument.DocumentNumber;
            examination.ResultDocumentDateTime = resultInfoDocument.DocumentDate;

            examination.HasOffence = !offences.IsEmpty();
            examination.IdentifiedOffences = offences;

            examination.ResultFrom = this.actCheckPeriodByActId?.Get(actCheck?.Id ?? 0)?.OrderBy(x => x)?.FirstOrDefault();
            examination.ResultTo = this.actCheckPeriodByActId?.Get(actCheck?.Id ?? 0)?.OrderByDescending(x => x)?.FirstOrDefault();
            examination.ResultPlace = actAddress;
            examination.FamiliarizationDate = actCheck?.DocumentDate;
            examination.IsSigned = !actWitness.IsEmpty();
            examination.FamiliarizedPerson = actWitness;

            if (disposal.Inspection.TypeBase == TypeBase.Inspection)
            {
                examination.EventDescription = "Провести проверку документов на соответствие действующему жилищному законодательству";
            }
            else
            {
                switch (disposal.KindCheck.Code)
                {
                    // Плановая выездная
                    case TypeCheck.PlannedExit:
                        examination.EventDescription = "Провести комплексное обследование дома, направленное на обеспечение сохранности жилищного фонда, надлежащее содержание и ремонт конструктивных элементов, инженерных систем и придомовой территории. В случае выявления нарушений принять предусмотренные законодательством меры по их устранению.";
                        break;

                    // Внеплановая выездная
                    case TypeCheck.NotPlannedExit:
                        examination.EventDescription = "Провести комплексное обследование дома, направленное на обеспечение сохранности жилищного фонда, надлежащее содержание и ремонт конструктивных элементов, инженерных систем и придомовой территории. В случае выявления нарушений принять предусмотренные законодательством меры по их устранению. ";
                        break;

                    // Плановая документарная
                    case TypeCheck.PlannedDocumentation:
                        examination.EventDescription = "Провести проверку документов на соответствие действующему жилищному законодательству.";
                        break;

                    // Внеплановая документарная
                    case TypeCheck.NotPlannedDocumentation:
                        examination.EventDescription = "Провести проверку документов на соответствие действующему жилищному законодательству.";
                        break;
                }
            }
            

        }

        /// <summary>
        /// Получить первый акт или первый протокол первого акта, сформированного по распоряжению
        /// </summary>
        /// <param name="disposal">Распоряжение</param>
        /// <returns>Первый акт или первый протокол первого акта</returns>
        private DocumentGji GetResultInfoDocument(Disposal disposal)
        {
            var disposalStage = disposal.Stage;

            // если в словаре протоколов будет запись для текущего распоряжения, то для блока ResultsInfo используем его, 
            // иначе проверяем словарь актов
            return this.protocolsByDisposalStage.Get(disposalStage) ?? this.actChecksByDisposalStage.Get(disposalStage) as DocumentGji;
        }

        /// <summary>
        /// Получить форму проверки
        /// </summary>
        /// <param name="inspection">Проверка</param>
        /// <returns>Форма проверки</returns>
        private IDictionaryRecord GetExaminationForm(InspectionGji inspection)
        {
            var dictionary = this.examinationJurPersonFormDictionary;
            var examinationFormId = (long)TypeFormInspection.ExitAndDocumentary; // Для проверок с основанием = "Инспекционные проверки" всегда жестко передавать форму проверки = "Выездная и документарная"

            var licenseApplicants = inspection as BaseLicenseApplicants;
            var statement = inspection as BaseStatement;
            var dispHead = inspection as BaseDispHead;
            var jurPerson = inspection as BaseJurPerson;
            var prosClaim = inspection as BaseProsClaim;

            if (licenseApplicants != null)
            {
                examinationFormId = (long)licenseApplicants.TypeForm;
                dictionary = this.examinationFormDictionary;
            }
            else if (statement != null)
            {
                examinationFormId = (long)statement.TypeForm;
                dictionary = this.examinationFormDictionary;
            }
            else if (dispHead != null)
            {
                examinationFormId = (long)dispHead.TypeForm;
            }
            else if (jurPerson != null)
            {
                examinationFormId = (long)jurPerson.TypeForm;
            }
            else if (prosClaim != null)
            {
                examinationFormId = (long)prosClaim.TypeForm;
            }

            return dictionary.GetDictionaryRecord(examinationFormId);
        }
    }
}
