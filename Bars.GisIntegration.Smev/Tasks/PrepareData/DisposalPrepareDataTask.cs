namespace Bars.GisIntegration.Smev.Tasks.PrepareData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.DataModels;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Bars.GisIntegration.Smev.ConfigSections;
    using Bars.GisIntegration.Smev.Entity;
    using Bars.GisIntegration.Smev.Enums;
    using Bars.GisIntegration.Smev.Exporters;
    using Bars.GisIntegration.Smev.SmevExchangeService.Erp;
    using Bars.GisIntegration.Smev.Tasks.PrepareData.Base;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ControlList;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    using NHibernate.Util;

    public class DisposalPrepareDataTask : ErpPrepareDataTask<LetterToErpType>
    {
        private readonly List<InspectionGjiRealityObject> inspectionRealityObjects = new List<InspectionGjiRealityObject>();
        private readonly List<string> disposalSubjectsAndPurposesAndTasks = new List<string>();
        private readonly Dictionary<long, List<ActCheckRealityObject>> actCheckRealityObjects = new Dictionary<long, List<ActCheckRealityObject>>();
        private readonly Dictionary<long, List<ActCheckWitness>> actCheckWitnesses = new Dictionary<long, List<ActCheckWitness>>();
        private readonly Dictionary<long, List<ActCheckPeriod>> actCheckPeriods = new Dictionary<long, List<ActCheckPeriod>>();
        private readonly Dictionary<long, List<ActCheckViolation>> actCheckViolations = new Dictionary<long, List<ActCheckViolation>>();
        private readonly Dictionary<long, List<Prescription>> prescriptions = new Dictionary<long, List<Prescription>>();
        private readonly List<ControlList> controlLists = new List<ControlList>();
        private readonly List<ControlListQuestion> controlListQuestions = new List<ControlListQuestion>();
        private readonly List<DisposalInspFoundationCheck> inspFoundationChecks = new List<DisposalInspFoundationCheck>();
        private readonly Dictionary<long, List<PrescriptionViol>> prescriptionViolations = new Dictionary<long, List<PrescriptionViol>>();
        private readonly List<Contragent> inspectionContragents = new List<Contragent>();
        private readonly List<ErpGuid> erpGuidsList = new List<ErpGuid>();
        private List<FrguFunction> frguFunctions;
        private string frguParticipantId;
        private string frguSupervisoryBodyId;

        private RequestDataToErp RequestObject { get; set; }

        private long ObjectId { get; set; }

        private TatarstanDisposal BaseDisposal { get; set; }

        /// <summary>
        /// Сервис инспекторов документов ГЖИ
        /// </summary>
        public IDocumentGjiInspectorService ServiceDocInspector { get; set; }

        /// <inheritdoc />
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var id = parameters.GetAsId();
            this.ObjectId = id;
            this.RequestObject = this.GetData(parameters);
        }

        /// <inheritdoc />
        protected override List<ValidateObjectResult> ValidateData()
        {
            var validateData = new List<ValidateObjectResult>();

            switch (this.RequestObject.ExporterType)
            {
                case ExporterType.DisposalInitialization:
                    validateData = this.ValidateDataDisposalInitialize(
                        (((LetterToErpTypeSet) this.RequestObject.RequestObjectData.Item).Items.FirstOrDefault() as CreateInspectionRequestType)
                        ?.Inspection);
                    break;
                case ExporterType.DisposalCorrection:
                    validateData = this.ValidateDataDisposalCorrection(
                        (LetterToErpTypeSet) this.RequestObject.RequestObjectData.Item);
                    break;
            }

            return validateData;
        }

        private List<ValidateObjectResult> ValidateDataDisposalInitialize(InspectionCreateInspectionCommonType inspection)
        {
            var errorsList = new List<string>();

            var connection = inspection.IConnection?.FirstOrDefault();

            if (connection != null && connection.erpIdLinked.IsEmpty())
            {
                errorsList.Add("Не заполнено поле \"Учетный номер проверки в ЕРП\"");
            }

            if (inspection.prosecId.IsEmpty())
            {
                errorsList.Add("Не заполнено поле \"Код прокуратуры\"");
            }

            if (inspection.IAuthority?.First().IAuthorityJointlyFrgu?.FirstOrDefault()?.idBk.IsEmpty() ?? false)
            {
                errorsList.Add(
                    "Не заполнено поле \"Идентификатор органа контроля (надзора) из Федерального реестра государственных и муниципальных услуг (ФРГУ) совместно с которым проводится КНМ\"");
            }

            if (inspection.IAuthority?.First().frguOrgIdBk.IsEmpty() ?? false)
            {
                errorsList.Add("Не заполнено поле \"Идентификатор участика в ФРГУ-ГЖИ\"");
            }

            if (inspection.IObject?.First().IChlists?.Any(x => x.oGuid.IsEmpty()) ?? false)
            {
                errorsList.Add("Не заполнено поле \"Идентификатор проверяемого дома\"");
            }

            if (inspection.IApprove?.IReason?.Any(x => x.iReasonId.IsEmpty()) ?? false)
            {
                errorsList.Add("Не заполнено поле \"Код основания проверки\"");
            }

            if (errorsList.Any())
            {
                throw new Exception(string.Join("; ", errorsList));
            }

            var validateResult = new ValidateObjectResult
            {
                Message = string.Empty,
                State = ObjectValidateState.Success
            };

            return new List<ValidateObjectResult> { validateResult };
        }

        private List<ValidateObjectResult> ValidateDataDisposalCorrection(LetterToErpTypeSet letter)
        {
            var letterItems = letter.Items.Cast<UpdateInspectionRequestType>()
                .Select(x => x.Item).ToList();

            var dictionary = new Dictionary<Type, string[]>
            {
                {
                    typeof(AuthorityUpdateInspectionUpdateCommonType), new[]
                    {
                        "iGuid",
                        "frguOrgGuid"
                    }
                },
                {
                    typeof(IConnectionUpdateInspectionUpdateCommonType), new[]
                    {
                        "iGuid",
                        "erpIdLinked"
                    }
                },
                {
                    typeof(IPublishUpdateInspectionUpdateCommonType), new[]
                    {
                        "iGuid"
                    }
                },
                {
                    typeof(IReasonUpdateInspectionUpdateCommonType), new[]
                    {
                        "iGuid",
                        "iReasonId"
                    }
                },
                {
                    typeof(IApproveDocsUpdateInspectionUpdateCommonType), new[]
                    {
                        "iGuid",
                        "iApproveDocId"
                    }
                },
                {
                    typeof(IApproveUpdateInspectionUpdateCommonType), new[]
                    {
                        "iGuid"
                    }
                },
                {
                    typeof(ChlistsQuestionsUpdateInspectionInsertCommonType), new[]
                    {
                        "chGuid"
                    }
                },
                {
                    typeof(ChlistsUpdateInspectionInsertCommonType), new[]
                    {
                        "oGuid"
                    }
                },
                {
                    typeof(IResultRepresentativeUpdateInspectionInsertCommonType), new[]
                    {
                        "rGuid"
                    }
                },
                {
                    typeof(IResultInformationUpdateInspectionInsertCommonType), new[]
                    {
                        "rGuid"
                    }
                },
                {
                    typeof(IViolationUpdateInspectionInsertCommonType), new[]
                    {
                        "rGuid"
                    }
                },
                {
                    typeof(IResultInspectorUpdateInspectionInsertCommonType), new[]
                    {
                        "rGuid"
                    }
                },
                {
                    typeof(IResultUpdateInspectionInsertCommonType), new[]
                    {
                        "oGuid"
                    }
                },
                {
                    typeof(ObjectUpdateInspectionInsertCommonType), new[]
                    {
                        "iGuid"
                    }
                },
                {
                    typeof(SubjectUpdateInspectionUpdateCommonType), new[]
                    {
                        "iGuid"
                    }
                },
                {
                    typeof(ClassificationLbUpdateInspectionInsertCommonType), new[]
                    {
                        "iGuid"
                    }
                },
                {
                    typeof(ClassificationUpdateInspectionUpdateCommonType), new[]
                    {
                        "iGuid"
                    }
                },
                {
                    typeof(AuthorityJointlyFrguUpdateInspectionUpdateCommonType), new[]
                    {
                        "iGuid",
                        "frguOrgGuid",
                        "frguJointlyGuid"
                    }
                },
                {
                    typeof(AuthorityJointlyFrguUpdateInspectionInsertCommonType), new[]
                    {
                        "iGuid",
                        "frguOrgGuid",
                        "idBk"
                    }
                },
                {
                    typeof(InspectorUpdateInspectionInsertCommonType), new[]
                    {
                        "iGuid",
                        "frguOrgGuid"
                    }
                },
                {
                    typeof(AuthorityServUpdateInspectionUpdateCommonType), new[]
                    {
                        "iGuid",
                        "frguOrgGuid",
                        "frguServGuid"
                    }
                }
            };
            var propertiesHashSet = new HashSet<string>();
            var ofTypeMethodInfo = typeof(Enumerable).GetMethod("OfType");

            foreach (var pair in dictionary)
            {
                var type = pair.Key;
                var properties = pair.Value;
                var genericOfType = ofTypeMethodInfo.MakeGenericMethod(new[] { type });
                var resultCollection = ((IEnumerable<object>) genericOfType.Invoke(null, new object[] { letterItems })).ToList();

                foreach (var propertyStr in properties)
                {
                    var property = type.GetProperty(propertyStr);
                    if (property == null)
                    {
                        continue;
                    }

                    if (resultCollection.Any(x => string.IsNullOrWhiteSpace(property.GetValue(x).ToStr())))
                    {
                        propertiesHashSet.Add(propertyStr);
                    }
                }
            }

            var propertyErrorMessageDict = new Dictionary<string, string>
            {
                { "iGuid", "Не заполнено поле \"Идентификационный номер в ЕРП\"" },
                { "frguOrgGuid", "Не заполнено поле \"Идентификатор органа контроля в ФРГУ-ГЖИ\"" },
                { "frguJointlyGuid", "Не заполнено поле \"Идентификатор органа контроля, совместно с которым проводится КНМ\"" },
                { "erpIdLinked", "Не заполнено поле \"Учетный номер проверки в ЕРП\"" },
                { "iReasonId", "Не заполнено поле \"Основание проверки\"" },
                { "iApproveDocId", "Не заполнено поле \"Идентификатор документа о согласовании КНМ\"" },
                { "chGuid", "Не заполнено поле \"Идентификатор проверочного листа\"" },
                { "oGuid", "Не заполнено поле \"Идентификатор проверяемого дома\"" },
                { "rGuid", "Не заполнено поле \"Идентификатор акта проверки\"" },
                { "idBk", "Не заполнено поле \"Номер организации в ФРГУ\"" },
                { "frguServGuid", "Не заполнено поле \"Идентификатор функции ФРГУ формата GUID\"" },
            };

            var errorsList = new List<string>();

            foreach (var property in propertiesHashSet)
            {
                if (propertyErrorMessageDict.TryGetValue(property, out var message))
                {
                    errorsList.Add(message);
                }
            }

            if (errorsList.Any())
            {
                throw new Exception(string.Join("; ", errorsList));
            }

            var validateResult = new ValidateObjectResult
            {
                Message = string.Empty,
                State = ObjectValidateState.Success
            };

            return new List<ValidateObjectResult> { validateResult };
        }

        /// <inheritdoc />
        protected override LetterToErpType GetRequestObject(ref bool isTestMessage, out long objectId)
        {
            objectId = this.ObjectId;
            return this.RequestObject.RequestObjectData;
        }

        private void InitData(Disposal disposal)
        {
            this.frguParticipantId = this.Container.GetGkhConfig<SmevIntegrationConfig>().FrguParticipantId;
            this.frguSupervisoryBodyId = this.Container.GetGkhConfig<SmevIntegrationConfig>().FrguSupervisoryBodyId;

            var actCheckPeriodDomain = this.Container.ResolveDomain<ActCheckPeriod>();
            var actCheckRoDomain = this.Container.ResolveDomain<ActCheckRealityObject>();
            var actCheckWitnessDomain = this.Container.ResolveDomain<ActCheckWitness>();
            var actCheckViolationDomain = this.Container.ResolveDomain<ActCheckViolation>();
            var disposalSubjectDomain = this.Container.ResolveDomain<DisposalVerificationSubject>();
            var disposalPurposeDomain = this.Container.ResolveDomain<DisposalSurveyPurpose>();
            var disposalTasksDomain = this.Container.ResolveDomain<DisposalSurveyObjective>();
            var documentChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
            var inspectionRoDomain = this.Container.ResolveDomain<InspectionGjiRealityObject>();
            var prescriptionDomain = this.Container.ResolveDomain<Prescription>();
            var inspectionGjiStageDomain = this.Container.ResolveDomain<InspectionGjiStage>();
            var disposalDomain = this.Container.ResolveDomain<TatarstanDisposal>();
            var controlListDomain = this.Container.ResolveDomain<ControlList>();
            var controlListQuestionsDomain = this.Container.ResolveDomain<ControlListQuestion>();
            var inspFoundationCheckDomain = this.Container.ResolveDomain<DisposalInspFoundationCheck>();
            var prescriptionViolDomain = this.Container.ResolveDomain<PrescriptionViol>();
            var baseJurPersonContragentDomain = this.Container.ResolveDomain<BaseJurPersonContragent>();
            var inspectionBaseContragentDomain = this.Container.ResolveDomain<InspectionBaseContragent>();
            var frguFunctionsDomain = this.Container.ResolveDomain<FrguFunction>();

            try
            {
                this.inspectionRealityObjects.AddRange(inspectionRoDomain.GetAll()
                    .Where(x => x.Inspection == disposal.Inspection
                        && x.RealityObject != null));

                var actCheckIdsByDisposal = documentChildrenDomain.GetAll()
                    .Where(x => x.Parent.Id == disposal.Id && x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Select(y => y.Children.Id);

                this.actCheckRealityObjects.AddOrOverride(actCheckRoDomain.GetAll()
                    .Where(x => actCheckIdsByDisposal.Contains(x.ActCheck.Id)
                        && x.RealityObject != null)
                    .AsEnumerable()
                    .GroupBy(x => x.RealityObject.Id)
                    .ToDictionary(x => x.Key, x => x.ToList()));

                this.actCheckWitnesses.AddOrOverride(actCheckWitnessDomain.GetAll()
                    .Where(x => x.ActCheck.Inspection == disposal.Inspection)
                    .GroupBy(x => x.ActCheck.Id)
                    .ToDictionary(x => x.Key, x => x.ToList()));

                this.actCheckPeriods.AddOrOverride(actCheckPeriodDomain.GetAll()
                    .Where(x => x.ActCheck.Inspection == disposal.Inspection)
                    .GroupBy(x => x.ActCheck.Id)
                    .ToDictionary(x => x.Key, x => x.ToList()));

                this.actCheckViolations.AddOrOverride(actCheckViolationDomain.GetAll()
                    .Where(x => x.ActObject.ActCheck.Inspection == disposal.Inspection)
                    .GroupBy(x => x.ActObject.Id)
                    .ToDictionary(x => x.Key, x => x.ToList()));

                var childrenPrescriptions = documentChildrenDomain.GetAll()
                    .Where(x => x.Parent.Inspection == disposal.Inspection)
                    .Join(
                        prescriptionDomain.GetAll(),
                        x => x.Children,
                        y => y,
                        (x, y) => new
                        {
                            x.Parent.Id,
                            Prescription = y
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Prescription).ToList());

                this.prescriptions.AddOrOverride(childrenPrescriptions);

                this.disposalSubjectsAndPurposesAndTasks.AddRange(disposalSubjectDomain.GetAll()
                    .Where(x => x.Disposal == disposal)
                    .Select(x => x.SurveySubject.Name));

                this.disposalSubjectsAndPurposesAndTasks.AddRange(disposalPurposeDomain.GetAll()
                    .Where(x => x.Disposal == disposal)
                    .Select(x => x.SurveyPurpose.Name));

                this.disposalSubjectsAndPurposesAndTasks.AddRange(disposalTasksDomain.GetAll()
                    .Where(x => x.Disposal == disposal)
                    .Select(x => x.SurveyObjective.Name));

                var inspectionStage = inspectionGjiStageDomain.GetAll()
                    .Where(x => x.Inspection.Id == disposal.Inspection.Id)
                    .Where(x => x.Parent == null)
                    .Where(x => x.TypeStage == TypeStage.Disposal)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                this.BaseDisposal = disposalDomain.GetAll()
                    .FirstOrDefault(x => x.Stage.Id == inspectionStage);

                this.controlLists.AddRange(controlListDomain.GetAll()
                    .Where(x => x.Disposal == disposal)
                    .ToList());

                this.inspFoundationChecks.AddRange(inspFoundationCheckDomain.GetAll()
                    .Where(x => x.Disposal == disposal)
                    .ToList());

                this.controlListQuestions.AddRange(controlListQuestionsDomain.GetAll()
                    .Where(x => x.ControlList.Disposal == disposal)
                    .ToList());

                this.prescriptionViolations.AddOrOverride(prescriptionViolDomain.GetAll()
                    .Where(x => x.Document.Inspection == disposal.Inspection)
                    .GroupBy(x => x.Document.Id)
                    .ToDictionary(x => x.Key, x => x.ToList()));

                this.inspectionContragents.AddRange(baseJurPersonContragentDomain.GetAll()
                    .Where(x => x.BaseJurPerson.Id == disposal.Inspection.Id)
                    .Select(x => x.Contragent)
                    .ToList());

                this.inspectionContragents.AddRange(inspectionBaseContragentDomain.GetAll()
                    .Where(x => x.InspectionGji.Id == disposal.Inspection.Id)
                    .Select(x => x.Contragent)
                    .ToList());

                this.frguFunctions = frguFunctionsDomain.GetAll().ToList();
            }
            finally
            {
                this.Container.Release(actCheckPeriodDomain);
                this.Container.Release(actCheckRoDomain);
                this.Container.Release(actCheckWitnessDomain);
                this.Container.Release(actCheckViolationDomain);
                this.Container.Release(disposalSubjectDomain);
                this.Container.Release(disposalPurposeDomain);
                this.Container.Release(documentChildrenDomain);
                this.Container.Release(inspectionRoDomain);
                this.Container.Release(prescriptionDomain);
                this.Container.Release(inspectionGjiStageDomain);
                this.Container.Release(disposalDomain);
                this.Container.Release(disposalTasksDomain);
                this.Container.Release(inspFoundationCheckDomain);
                this.Container.Release(controlListDomain);
                this.Container.Release(controlListQuestionsDomain);
                this.Container.Release(prescriptionViolDomain);
                this.Container.Release(baseJurPersonContragentDomain);
                this.Container.Release(inspectionBaseContragentDomain);
                this.Container.Release(frguFunctionsDomain);
            }
        }

        private RequestDataToErp GetData(DynamicDictionary parameters)
        {
            var disposalDomain = this.Container.ResolveDomain<TatarstanDisposal>();
            var extractor = this.Container.Resolve<IDataExtractor<TatarstanDisposal>>();

            try
            {
                var disposal = extractor.Extract(parameters)?.FirstOrDefault();
                if (disposal == null)
                {
                    throw new NullReferenceException("Не найдены данные");
                }

                var isDisposalInitialization = string.IsNullOrEmpty(disposal.ErpGuid);

                this.InitData(disposal);

                var disposalInspectors = this.ServiceDocInspector.GetInspectorsByDocumentId(disposal.Id)
                    .ToArray();

                var riskCategory = disposal.Inspection.RiskCategory;
                var lastInspectionDate = disposal.Inspection.Contragent != null
                    ? disposalDomain.GetAll()
                        .Where(x => x.Inspection.Contragent == disposal.Inspection.Contragent)
                        .Where(x => x.DocumentDate.HasValue)
                        .Select(x => x.DocumentDate).Min()
                    : null;

                var approveDocType = default(int);
                if ((disposal.KindCheck?.Code == TypeCheck.PlannedExit || disposal.KindCheck?.Code == TypeCheck.PlannedDocumentation))
                {
                    approveDocType = 1;
                }
                else if ((disposal.KindCheck?.Code == TypeCheck.PlannedExit || disposal.KindCheck?.Code == TypeCheck.PlannedDocumentation)
                    && disposal.NcDate.HasValue)
                {
                    approveDocType = 3;
                }

                else if (disposal.KindCheck?.Code == TypeCheck.NotPlannedExit
                    || disposal.KindCheck?.Code == TypeCheck.NotPlannedDocumentation
                    || disposal.KindCheck?.Code == TypeCheck.InspectionSurvey)
                {
                    //не объединил if для лучшей читаемости
                    approveDocType = 1;
                }
                else if ((disposal.KindCheck?.Code == TypeCheck.NotPlannedExit
                        || disposal.KindCheck?.Code == TypeCheck.NotPlannedDocumentation
                        || disposal.KindCheck?.Code == TypeCheck.InspectionSurvey)
                    && disposal.NcDate.HasValue)
                {
                    approveDocType = 5;
                }
                else if ((disposal.KindCheck?.Code == TypeCheck.NotPlannedExit
                        || disposal.KindCheck?.Code == TypeCheck.NotPlannedDocumentation
                        || disposal.KindCheck?.Code == TypeCheck.InspectionSurvey)
                    && disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement)
                {
                    approveDocType = 3;
                }

                var noticeType = default(int);
                switch (disposal.NotificationType)
                {
                    case NotificationType.Individually:
                        noticeType = 1;
                        break;
                    case NotificationType.Courier:
                        noticeType = 2;
                        break;
                    case NotificationType.Agenda:
                        noticeType = 3;
                        break;
                    case NotificationType.Other:
                        noticeType = 44;
                        break;
                }

                var riskId = riskCategory?.Code.ToInt();

                RequestDataToErp result;

                if (isDisposalInitialization)
                {
                    result = new RequestDataToErp
                    {
                        RequestObjectData = this.DisposalInitialization(disposal,
                            disposalInspectors,
                            riskId,
                            noticeType,
                            approveDocType,
                            lastInspectionDate),
                        ExporterType = ExporterType.DisposalInitialization
                    };
                }
                else
                {
                    result = new RequestDataToErp
                    {
                        RequestObjectData =
                            this.DisposalCorrection(disposal, disposalInspectors, approveDocType, riskId, noticeType, lastInspectionDate),
                        ExporterType = ExporterType.DisposalCorrection
                    };
                }

                this.SaveErpGuid();

                return result;
            }
            finally
            {
                this.Container.Release(disposalDomain);
                this.Container.Release(extractor);
            }
        }

        /// <summary>
        /// Сохраняет значения Guid из поля <see cref="erpGuidsList"/>
        /// </summary>
        private void SaveErpGuid()
        {
            var taskTriggerDomain = this.Container.ResolveDomain<RisTaskTrigger>();
            var erpGuidDomain = this.Container.ResolveDomain<ErpGuid>();

            using (this.Container.Using(taskTriggerDomain, erpGuidDomain))
            {
                var task = taskTriggerDomain.FirstOrDefault(x => x.Trigger.Id == this.StorableTrigger.Id)?.Task;
                var erpGuids = this.erpGuidsList.Select(s => new ErpGuid
                {
                    Id = s.Id,
                    EntityId = s.EntityId,
                    EntityType = s.EntityType,
                    AssemblyType = s.AssemblyType,
                    Guid = s.Guid,
                    Task = task
                });

                foreach (var erpGuid in erpGuids)
                {
                    erpGuidDomain.Save(erpGuid);
                }
            }
        }

        /// <summary>
        /// Проверяет существование Guid для сущности в списке <see cref="erpGuidsList"/> и возвращает его
        /// Если в списке отсутствует запись генерирует Guid и создает запись в <see cref="erpGuidsList"/>
        /// </summary>
        /// <param name="entity">Сущность для которой генерируем Guid</param>
        /// <param name="type">Тип сущности</param>
        /// <returns>Возвращает Guid</returns>
        private string GenGuid(IHaveId entity, Type type)
        {
            var guid = this.erpGuidsList.FirstOrDefault(f => f.EntityId == entity.Id && f.EntityType == type.FullName)?.Guid;

            if (!(guid is null))
            {
                return guid;
            }

            var erpGuid = new ErpGuid
            {
                EntityId = entity.Id,
                EntityType = type.FullName,
                AssemblyType = type.Assembly.ToString(),
                Guid = Guid.NewGuid().ToString().ToUpper()
            };
            this.erpGuidsList.Add(erpGuid);

            return erpGuid.Guid;
        }

        private LetterToErpType DisposalInitialization(
            TatarstanDisposal disposal,
            DocumentGjiInspector[] inspectors,
            int? riskId,
            int noticeType,
            int approveDocType,
            DateTime? lastInspectionDate)
        {
            var authority = new AuthorityCommonType[]
            {
                new AuthorityCreateInspectionCommonType
                {
                    frguOrgIdBk = this.frguParticipantId?.ToUpper(),
                    IAuthorityServ = new AuthorityServCommonType[]
                    {
                        new AuthorityServCreateInspectionCommonType
                        {
                            frguServIdBk = this.IsLicenseControl(disposal) ? "1600000000160075511" : "1600000000160074805",
                        }
                    },
                    IInspector = inspectors.Select(x =>
                    {
                        return (InspectorCommonType) new InspectorCreateInspectionCommonType
                        {
                            fullName = x.Inspector?.Fio,
                            position = x.Inspector?.Position,
                            inspectorTypeId = "1",
                            numGuid = this.GenGuid(x, typeof(DocumentGjiInspector))
                        };
                    }).ToArray(),
                    IAuthorityJointlyFrgu = this.inspectionContragents
                        .Select(x => (AuthorityJointlyFrguCommonType) new AuthorityJointlyFrguCreateInspectionCommonType
                        {
                            idBk = x.FrguOrgNumber
                        }).ToArray()
                }
            };

            var iObject = this.inspectionRealityObjects.IsNotEmpty()
                ? this.inspectionRealityObjects.Select(x =>
                {
                    var actCheckRo = this.actCheckRealityObjects.Get(x.RealityObject.Id)?.FirstOrDefault();
                    var actCheck = actCheckRo?.ActCheck;
                    var actPeriods = this.actCheckPeriods.Get(actCheck?.Id ?? 0L) ?? Enumerable.Empty<ActCheckPeriod>();
                    var actCheckStartDate = actPeriods.Where(y => y.DateStart.HasValue).SafeMin(y => y.DateStart.GetValueOrDefault());
                    var actCheckDurationHours = Math.Ceiling(actPeriods.Where(y => y.DateStart.HasValue && y.DateEnd.HasValue)
                        .SafeSum(y => (y.DateEnd.GetValueOrDefault() - y.DateStart.GetValueOrDefault()).TotalHours.ToDecimal()));
                    var actCheckDuratonDays = actPeriods.Where(y => y.DateCheck.HasValue).Select(y => y.DateCheck).Distinct().Count();
                    var actWitnesses = this.actCheckWitnesses.Get(actCheck?.Id ?? 0L) ?? Enumerable.Empty<ActCheckWitness>();
                    var actViolations = this.actCheckViolations.Get(actCheckRo?.Id ?? 0L) ?? Enumerable.Empty<ActCheckViolation>();
                    var prescriptions = this.prescriptions.Get(actCheck?.Id ?? 0L) ?? Enumerable.Empty<Prescription>();
                    var prescriptionViolations = this.prescriptionViolations
                        .Where(y => prescriptions.Select(z => z.Id).Contains(y.Key))
                        .SelectMany(y => y.Value)
                        .ToList();

                    var resultInspectors = this.ServiceDocInspector.GetInspectorsByDocumentId(actCheck?.Id)
                        .AsEnumerable()
                        .Select(y => (IResultInspectorCommonType) new IResultInspectorCreateInspectionCommonType
                        {
                            inspectorTypeId = "1",
                            fullName = y.Inspector.Fio,
                            position = y.Inspector.Position,
                            numGuid = this.GenGuid(y, typeof(DocumentGjiInspector))
                        }).ToArray();

                    var violation = actViolations.Any()
                        ? actViolations.Select(y =>
                        {

                            return (IViolationCommonType) new IViolationCreateInspectionCommonType
                            {
                                violationNote = y.InspectionViolation.Violation.Name,
                                violationAct = y.InspectionViolation.Violation.CodePin,
                                iViolationTypeId = "1",
                                numGuid = this.GenGuid(y, typeof(ActCheckViolation)),
                                VInjunction = prescriptionViolations
                                    .Where(z => z.InspectionViolation == y.InspectionViolation)
                                    .Select(z =>
                                    {
                                        var prescription = prescriptions.FirstOrDefault(presc => presc.Id == z.Document.Id);
                                        if (prescription == null)
                                        {
                                            return null;
                                        }

                                        return (VInjunctionCommonType) new VInjunctionCreateInspectionCommonType
                                        {
                                            code = prescription.DocumentNumber,
                                            dateAppointment = prescription.DocumentDate.GetValueOrDefault(),
                                            dateAppointmentSpecified = prescription.DocumentDate.HasValue,
                                            executionNote = prescription.Closed == YesNoNotSet.Yes
                                                ? "Выявленные нарушения в результате проведения КНМ устранены"
                                                : null,
                                            executionDeadline = z.DatePlanRemoval.GetValueOrDefault(),
                                            executionDeadlineSpecified = z.DatePlanRemoval.HasValue,
                                            numGuid = this.GenGuid(z, typeof(PrescriptionViol))
                                        };
                                    })
                                    .Where(z => z != null)
                                    .ToArray(),
                                VClassificationLb = new[]
                                {
                                    (VClassificationLbCommonType) new VClassificationLbCreateInspectionCommonType
                                    {
                                        lbDocumentName = y.InspectionViolation.Violation.CodePin
                                    }
                                }
                            };
                        }).ToArray()
                        : null;

                    var resultInfoList = new[]
                        {
                            actCheckRo?.HaveViolation == YesNoNotSet.No
                                ? (IResultInformationCommonType) new IResultInformationCreateInspectionCommonType
                                {
                                    text = actCheckRo.Description,
                                    resultInformationTypeId = "2"
                                }
                                : null,
                            actCheck?.AcquaintState != null && actCheck.AcquaintState != AcquaintState.NotAcquainted
                                ? (IResultInformationCommonType) new IResultInformationCreateInspectionCommonType
                                {
                                    text = actCheck.AcquaintState == AcquaintState.RefuseToAcquaint
                                        ? $"Должностное лицо, отказавшееся от ознакомления с актом проверки {actCheck.RefusedToAcquaintPerson}"
                                        : $"Должностное лицо, ознакомившееся с актом проверки {actCheck.AcquaintedPerson}",
                                    resultInformationTypeId = "0",
                                    numGuid = this.GenGuid(actCheckRo, typeof(ActCheckRealityObject))
                                }
                                : null
                        };

                    resultInfoList = resultInfoList?.Where(y => y != null).ToArray();

                    return (ObjectCommonType) new ObjectCreateInspectionCommonType
                    {
                        houseGuid = x.RealityObject.FiasAddress?.HouseGuid?.ToString().ToUpper(), //регулярка ерп воспринимает только заглавные
                        addressTypeId = "2",
                        iObjectTypeId = "4",
                        address = x.RealityObject.Address,
                        numGuid = this.GenGuid(x, typeof(InspectionGjiRealityObject)),
                        IResult = actCheck != null
                            ? new IResultCreateInspectionCommonType
                            {
                                actDateCreate = actCheck.DocumentDate.GetValueOrDefault(),
                                actDateCreateSpecified = actCheck.DocumentDate.HasValue,
                                actAddress = actCheckRo?.RealityObject.Address,
                                actAddressTypeId = "44",
                                actWasNotRead = actCheck.AcquaintState == AcquaintState.RefuseToAcquaint,
                                actWasNotReadSpecified = true,
                                startDate = actCheckStartDate,
                                startDateSpecified = actCheckStartDate.IsValid(),
                                durationHours = actCheckDurationHours > 1 ? actCheckDurationHours.ToStr() : "1",
                                durationDays = actCheckDuratonDays.ToStr(),
                                houseGuid = actCheckRo.RealityObject.FiasAddress?.HouseGuid?.ToString().ToUpper(),
                                IResultInspector = resultInspectors.Any() ? resultInspectors : null,
                                IViolation = violation,
                                IResultInformation = resultInfoList.Any() ? resultInfoList : null,
                                numGuid = this.GenGuid(actCheck, typeof(ActCheck)),
                                IResultRepresentative = actWitnesses?.Select(z =>
                                {
                                    return new IResultRepresentativeCommonType
                                    {
                                        representativeTypeId = "2",
                                        representativeFullName = z.Fio,
                                        representativePosition = z.Position,
                                        numGuid = this.GenGuid(z, typeof(ActCheckWitness))
                                    };
                                }).ToArray()
                            }
                            : null
                    };
                }).ToArray()
                : null;

            var approveDocsList = new List<IApproveDocsCommonType>();

            if (approveDocType == 1)
            {
                approveDocsList.Add(new IApproveDocsCreateInspectionCommonType
                {
                    iApproveDocId = approveDocType.ToString(),
                    docAtr = disposal.DocumentNumber,
                    docDate = disposal.DocumentDate ?? DateTime.MinValue,
                    docDateSpecified = disposal.DocumentDate.HasValue,
                });
            }

            if (approveDocType == 3 && disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement)
            {
                approveDocsList.Add(new IApproveDocsCreateInspectionCommonType
                {
                    iApproveDocId = approveDocType.ToString(),
                    docAtr = disposal.DocumentNumberWithResultAgreement,
                    docDate = disposal.DocumentDateWithResultAgreement ?? DateTime.MinValue,
                    docDateSpecified = disposal.DocumentDateWithResultAgreement.HasValue,
                });
            }

            if (approveDocType == 3 && disposal.NcDate.HasValue || approveDocType == 5)
            {
                approveDocsList.Add(new IApproveDocsCreateInspectionCommonType
                {
                    iApproveDocId = approveDocType.ToString(),
                    docAtr = disposal.NcNum,
                    docDate = disposal.NcDate ?? DateTime.MinValue,
                    docDateSpecified = disposal.NcDate.HasValue,
                });
            }

            var approveDocs = approveDocsList.ToArray();

            var reason = disposal.InspectionBase != null
                ? new IReasonCommonType[]
                {
                    new IReasonCreateInspectionCommonType
                    {
                        isApproveRequired = disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement,
                        isApproveRequiredSpecified = disposal.TypeAgreementProsecutor != TypeAgreementProsecutor.NotSet,
                        iReasonId = disposal.InspectionBase.Code,
                        reasonText = disposal.InspectionBase.Name,
                        reasonDate = disposal.DocumentDate.GetValueOrDefault(),
                        reasonDateSpecified = disposal.DocumentDate.HasValue
                    }
                }
                : null;

            IConnectionCommonType[] connection = null;

            if (disposal.TypeDisposal == TypeDisposalGji.DocumentGji)
            {
                connection = new IConnectionCommonType[]
                {
                    new IConnectionCreateInspectionCommonType
                    {
                        erpIdLinked = this.BaseDisposal.ErpRegistrationNumber,
                        iConnectionTypeId = "1"
                    }
                };
            }

            var inspection = new InspectionCreateInspectionCommonType
            {
                startDate = disposal.DateStart.GetValueOrDefault(),
                startDateSpecified = disposal.DateStart.HasValue,
                isStartMonth = false,
                isStartMonthSpecified = true,
                fzId = "0",
                iTypeId = disposal.KindCheck?.Code == TypeCheck.PlannedExit || disposal.KindCheck?.Code == TypeCheck.PlannedDocumentation
                    ? "0"
                    : "1",
                prosecId = "1050160000",
                domainId = "1033920000000001",
                IAuthority = authority,
                IClassification = new ClassificationCreateInspectionCommonType
                {
                    iRiskId = riskId.HasValue ? riskId.ToString() : null,
                    iSupervisionId = this.IsLicenseControl(disposal) ? "1050" : "1060",
                    iCarryoutTypeId = disposal.KindCheck?.Code == TypeCheck.NotPlannedDocumentation ||
                        disposal.KindCheck?.Code == TypeCheck.PlannedDocumentation
                            ? "2"
                            : "1",
                    iNoticeTypeId = noticeType == default(int) ? null : noticeType.ToString(),
                    iNoticeDate = disposal.NcDate.GetValueOrDefault(),
                    iNoticeDateSpecified = disposal.NcDate.HasValue,
                    IClassificationLb = this.inspFoundationChecks.Any()
                        ? this.inspFoundationChecks.Select(x => (ClassificationLbCommonType) new ClassificationLbCreateInspectionCommonType
                        {
                            lbDocumentName = x.InspFoundationCheck.Name,
                            numGuid = this.GenGuid(x, typeof(DisposalInspFoundationCheck))
                        }).ToArray()
                        : null,
                },
                ISubject = new SubjectCreateInspectionCommonType
                {
                    inn = disposal.Inspection.Contragent?.Inn,
                    ogrn = disposal.Inspection.Contragent?.Ogrn,
                    orgName = disposal.Inspection.Contragent?.Name,
                    iSubjectTypeId = "0"
                },
                IObject = iObject,
                IApprove = new IApproveCreateInspectionCommonType
                {
                    inspTarget = this.disposalSubjectsAndPurposesAndTasks.AggregateWithSeparator("; "),
                    endDate = disposal.DateEnd.GetValueOrDefault(),
                    endDateSpecified = disposal.DateEnd.HasValue,
                    durationDay = disposal.CountDays.HasValue ? Math.Min(disposal.CountDays.Value, 364).ToString() : null,
                    durationHours = disposal.CountHours.HasValue ? Math.Min(disposal.CountHours.Value, 255).ToString() : null,
                    decisionPlace = disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement
                        ? "ул. Большая Красная, 15, Казань, Респ. Татарстан, 420111"
                        : null,
                    decisionSignerTitle = disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement
                        ? disposal.IssuedDisposal?.Position
                        : null,
                    decisionSignerName = disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement
                        ? disposal.IssuedDisposal?.Fio
                        : null,
                    IApproveDocs = approveDocs,
                    IReason = disposal.InspectionBase != null && disposal.InspectionBase.SendErp ? reason : null
                },
                IPublish = new IPublishCreateInspectionCommonType
                {
                    IPublishStatus = "ASK_TO_PUBLISH"
                },
                IConnection = connection
            };

            return new LetterToErpType
            {
                Item = new LetterToErpTypeSet
                {
                    Items = new object[]
                    {
                        new CreateInspectionRequestType
                        {
                            Inspection = inspection
                        }
                    }
                }
            };
        }

        private LetterToErpType DisposalCorrection(
            TatarstanDisposal disposal,
            DocumentGjiInspector[] inspectors,
            int approveDocType,
            int? riskId,
            int noticeType,
            DateTime? lastInspectionDate)
        {
            var resultItems = new List<UpdateInspectionRequestType>();

            //везде гуид.ToUpper(), потому что другая сторона воспринимает только заглавные буквы
            var authority = new AuthorityUpdateInspectionUpdateCommonType
            {
                iGuid = disposal.ErpGuid.ToUpper(),
                frguOrgGuid = this.frguSupervisoryBodyId.ToUpper()
            };

            resultItems.Add(this.GetUpdateInspectionRequestType(disposal, authority, ItemChoiceType.UpdateIAuthority));

            var iObjects = this.inspectionRealityObjects.IsNotEmpty()
                ? this.inspectionRealityObjects.Select(x =>
                {
                    if (!this.actCheckRealityObjects.TryGetValue(x.RealityObject.Id, out var actCheckRos))
                    {
                        return null;
                    }

                    var actCheckRo = this.actCheckRealityObjects.Get(x.RealityObject.Id)?.FirstOrDefault();
                    var actCheck = actCheckRo?.ActCheck;
                    var actPeriods = this.actCheckPeriods.Get(actCheck?.Id ?? 0L) ?? Enumerable.Empty<ActCheckPeriod>();
                    var actCheckStartDate = actPeriods.Where(y => y.DateStart.HasValue).SafeMin(y => y.DateStart.GetValueOrDefault());
                    var actCheckDurationHours = Math.Ceiling(actPeriods.Where(y => y.DateStart.HasValue && y.DateEnd.HasValue)
                        .SafeSum(y => (y.DateEnd.GetValueOrDefault() - y.DateStart.GetValueOrDefault()).TotalHours.ToDecimal()));
                    var actCheckDuratonDays = actPeriods.Where(y => y.DateCheck.HasValue).Select(y => y.DateCheck).Distinct().Count();
                    var actWitnesses = this.actCheckWitnesses.Get(actCheck?.Id ?? 0L) ?? Enumerable.Empty<ActCheckWitness>();
                    var actViolations = this.actCheckViolations.Get(actCheckRo?.Id ?? 0L) ?? Enumerable.Empty<ActCheckViolation>();
                    var actPrescriptions = this.prescriptions.Get(actCheck?.Id ?? 0L) ?? Enumerable.Empty<Prescription>();
                    var prescriptionViolations = this.prescriptionViolations
                        .Where(y => actPrescriptions.Select(z => z.Id).Contains(y.Key))
                        .SelectMany(y => y.Value)
                        .ToList();
                    var actInspectorsDict = this.ServiceDocInspector.GetInspectorsByDocumentId(actCheck.Id)
                        .ToArray();

                    var iObject = this.GetIObject(x, disposal);

                    var iResult = this.GetIResult(actCheck, actCheckRo, actCheckStartDate, actCheckDurationHours, actCheckDuratonDays, x);
                    resultItems.Add(this.GetUpdateInspectionRequestType(disposal,
                        iResult,
                        !string.IsNullOrWhiteSpace(actCheck.ErpGuid) ? ItemChoiceType.UpdateIResult : ItemChoiceType.InsertIResult));

                    var updateResultInspectors = this.GetResultInspectors(actInspectorsDict, true, actCheck);
                    resultItems.AddRange(updateResultInspectors.Select(y =>
                        this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.UpdateIResultInspector)));

                    var insertResultInspectors = this.GetResultInspectors(actInspectorsDict, false, actCheck);
                    resultItems.AddRange(insertResultInspectors.Select(y =>
                        this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.InsertIResultInspector)));

                    var updateViolations = this.GetViolations(actViolations, true, actCheck);
                    resultItems.AddRange(updateViolations.Select(y =>
                        this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.UpdateIViolation)));

                    var insertViolations = this.GetViolations(actViolations, false, actCheck);
                    resultItems.AddRange(insertViolations.Select(y =>
                        this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.InsertIViolation)));

                    var updateVInjuction = this.GetVInjunction(actPrescriptions, prescriptionViolations, actViolations, true);
                    resultItems.AddRange(updateVInjuction.Select(y =>
                        this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.UpdateVInjunction)));

                    var insertVInjunction = this.GetVInjunction(actPrescriptions, prescriptionViolations, actViolations, false);
                    resultItems.AddRange(insertVInjunction.Select(y =>
                        this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.InsertVInjunction)));

                    #region ждет версии 4.0.2
                    // var updateVClassificationLb = this.GetVClassificationLbs(actViolations, true);
                    // resultItems.AddRange(updateVClassificationLb.Select(y =>
                    //     this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.UpdateVClassificationLb)));

                    // var insertVClassificationLb = this.GetVClassificationLbs(actViolations, false);
                    // resultItems.AddRange(insertVClassificationLb.Select(y =>
                    //     this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.InsertVClassificationLb)));
                    #endregion

                    var insertIResultRepresentative = this.GetResultRepresentatives(actWitnesses, false, actCheck);
                    resultItems.AddRange(insertIResultRepresentative.Select(y =>
                        this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.InsertIResultRepresentative)));

                    var resultUpdateRepresentative = this.GetResultRepresentatives(actWitnesses, true, actCheck);
                    resultItems.AddRange(resultUpdateRepresentative.Select(y =>
                        this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.UpdateIResultRepresentative)));

                    var resultInfo = this.GetResultInfos(actCheckRo, !string.IsNullOrWhiteSpace(actCheckRo.ErpGuid), actCheck);
                    if (resultInfo.Any())
                    {
                        resultItems.AddRange(resultInfo.Select(y => this.GetUpdateInspectionRequestType(disposal,
                            y,
                            !string.IsNullOrWhiteSpace(actCheckRo.ErpGuid)
                                ? ItemChoiceType.UpdateIResultInformation
                                : ItemChoiceType.InsertIResultInformation)));
                    }

                    #region ждет версии 4.0.2
                    // var controlListUpdateQuestions = this.GetControlListQuestions(true);
                    // resultItems.AddRange(controlListUpdateQuestions.Select(y =>
                    //     this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.UpdateIChlistQuestions)));

                    // var controlListInsertQuestions = this.GetControlListQuestions(false);
                    // resultItems.AddRange(controlListInsertQuestions.Select(y =>
                    //     this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.InsertIChlistQuestions)));

                    //проверочн листы
                    // var controlUpdateLists = this.GetControlLists(true, x);
                    // resultItems.AddRange(controlUpdateLists.Select(y =>
                    //     this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.UpdateIChlists)));

                    // var controlInsertLists = this.GetControlLists(false, x);
                    // resultItems.AddRange(controlInsertLists.Select(y =>
                    //     this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.InsertIChlists)));
                    #endregion

                    return iObject;
                }).ToArray()
                : null;

            iObjects = iObjects.Where(x => x != null).ToArray();

            if (iObjects?.Any() ?? false)
            {
                resultItems.AddRange(iObjects.Select(y =>
                    this.GetUpdateInspectionRequestType(disposal,
                        y,
                        y is ObjectUpdateInspectionUpdateCommonType ? ItemChoiceType.UpdateIObject : ItemChoiceType.InsertIObject)));
            }

            var docNumber = string.Empty;
            DateTime? docDate = null;
            switch (approveDocType)
            {
                case 1:
                    docNumber = disposal.DocumentNumber;
                    docDate = disposal.DocumentDate;
                    break;
                case 3 when disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement:
                    docNumber = disposal.DocumentNumberWithResultAgreement;
                    docDate = disposal.DocumentDateWithResultAgreement;
                    break;
                case 3 when disposal.NcDate.HasValue:
                case 5:
                    docNumber = disposal.NcNum;
                    docDate = disposal.NcDate;
                    break;
            }

            var approveDoc = new IApproveDocsUpdateInspectionUpdateCommonType
            {
                iApproveDocId = approveDocType.ToString(),
                docAtr = docNumber,
                docDate = docDate ?? DateTime.MinValue,
                docDateSpecified = docDate.HasValue,
                iGuid = disposal.ErpGuid.ToUpper()
            };

            resultItems.Add(this.GetUpdateInspectionRequestType(disposal, approveDoc, ItemChoiceType.UpdateIApproveDocs));

            var iInspectorsUpdate = this.GetIInspectors(true, disposal, inspectors);
            resultItems.AddRange(iInspectorsUpdate.Select(y =>
                this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.UpdateIInspector)));

            var iInspectorsInsert = this.GetIInspectors(false, disposal, inspectors);
            resultItems.AddRange(iInspectorsInsert.Select(y =>
                this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.InsertIInspector)));

            var reason = disposal.InspectionBase != null
                ? new IReasonUpdateInspectionUpdateCommonType
                {
                    iGuid = disposal.ErpGuid.ToUpper(),
                    isApproveRequired = disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement,
                    isApproveRequiredSpecified = disposal.TypeAgreementProsecutor != TypeAgreementProsecutor.NotSet,
                    iReasonId = disposal.InspectionBase.Code,
                    reasonText = disposal.InspectionBase.Name,
                    reasonDate = disposal.DocumentDate.GetValueOrDefault(),
                    reasonDateSpecified = disposal.DocumentDate.HasValue,
                }
                : null;

            resultItems.Add(this.GetUpdateInspectionRequestType(disposal, reason, ItemChoiceType.UpdateIReason));

            //if (disposal.TypeDisposal == TypeDisposalGji.DocumentGji)
            //{
            //    var connection = new IConnectionUpdateInspectionUpdateCommonType
            //    {
            //        erpIdLinked = this.BaseDisposal.RegistrationNumberErp,
            //        iConnectionTypeId = "1",
            //        iGuid = disposal.ErpGuid.ToUpper()
            //    };

            // resultItems.Add(this.GetUpdateInspectionRequestType(disposal, connection, ItemChoiceType.UpdateIConnection));
            //}

            var publish = new IPublishUpdateInspectionUpdateCommonType
            {
                iGuid = disposal.ErpGuid.ToUpper(),
                IPublishStatus = "ASK_TO_PUBLISH"
            };

            resultItems.Add(this.GetUpdateInspectionRequestType(disposal, publish, ItemChoiceType.UpdateIPublish));

            var iApprove = new IApproveUpdateInspectionUpdateCommonType
            {
                iGuid = disposal.ErpGuid.ToUpper(),
                inspTarget = this.disposalSubjectsAndPurposesAndTasks.AggregateWithSeparator("; "),
                endDate = disposal.DateEnd.GetValueOrDefault(),
                endDateSpecified = disposal.DateEnd.HasValue,
                durationDay = disposal.CountDays.HasValue ? Math.Min(disposal.CountDays.Value, 364).ToString() : null,
                durationHours = disposal.CountHours.HasValue ? Math.Min(disposal.CountHours.Value, 255).ToString() : null,
            };

            resultItems.Add(this.GetUpdateInspectionRequestType(disposal, iApprove, ItemChoiceType.UpdateIApprove));

            var updateSubject = new SubjectUpdateInspectionUpdateCommonType
            {
                inn = disposal.Inspection.Contragent?.Inn,
                ogrn = disposal.Inspection.Contragent?.Ogrn,
                orgName = disposal.Inspection.Contragent?.Name,
                iSubjectTypeId = "0",
                iGuid = disposal.ErpGuid.ToUpper()
            };

            resultItems.Add(this.GetUpdateInspectionRequestType(disposal, updateSubject, ItemChoiceType.UpdateISubject));

            var updateIClassificationLb = this.GetIClassificationLbs(true, disposal);
            resultItems.AddRange(updateIClassificationLb.Select(y =>
                this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.UpdateIClassificationLb)));

            var insertIClassificationLb = this.GetIClassificationLbs(false, disposal);
            resultItems.AddRange(insertIClassificationLb.Select(y =>
                this.GetUpdateInspectionRequestType(disposal, y, ItemChoiceType.InsertIClassificationLb)));

            var updateIClassification = new ClassificationUpdateInspectionUpdateCommonType
            {
                iRiskId = riskId.HasValue ? riskId.ToStr() : null,
                iSupervisionId = (this.IsLicenseControl(disposal)) ? "1050" : "1060",
                iNoticeDate = disposal.NcDate.GetValueOrDefault(),
                iNoticeDateSpecified = disposal.NcDate.HasValue,
                iNoticeTypeId = noticeType == default(int) ? null : noticeType.ToString(),
                iCarryoutTypeId = disposal.KindCheck?.Code == TypeCheck.NotPlannedDocumentation ||
                    disposal.KindCheck?.Code == TypeCheck.PlannedDocumentation
                        ? "2"
                        : "1",
                iGuid = disposal.ErpGuid.ToUpper()
            };

            resultItems.Add(this.GetUpdateInspectionRequestType(disposal, updateIClassification, ItemChoiceType.UpdateIClassification));

            var updateIAuthorityServ = new AuthorityServUpdateInspectionUpdateCommonType
            {
                iGuid = disposal.ErpGuid.ToUpper(),
                frguOrgGuid = this.frguSupervisoryBodyId.ToUpper(),
                frguServGuid = this.IsLicenseControl(disposal)
                    ? this.frguFunctions.FirstOrDefault(x => string.Equals("1600000000160075511", x.FrguId))?.Guid
                    : this.frguFunctions.FirstOrDefault(x => string.Equals("1600000000160074805", x.FrguId))?.Guid
            };

            resultItems.Add(this.GetUpdateInspectionRequestType(disposal, updateIAuthorityServ, ItemChoiceType.UpdateIAuthorityServ));

            var updateIAuthorityJointlyFrgu = this.GetIAuthorityJointlyFrgus(true, disposal);
            resultItems.AddRange(updateIAuthorityJointlyFrgu.Select(x =>
                this.GetUpdateInspectionRequestType(disposal, x, ItemChoiceType.UpdateIAuthorityJointlyFrgu)));

            #region 4.0.2
            // var insertIAuthorityJointlyFrgu = this.GetIAuthorityJointlyFrgus(false, disposal);
            // resultItems.Add(this.GetUpdateInspectionRequestType(disposal, insertIAuthorityJointlyFrgu, ItemChoiceType.InsertIAuthorityJointlyFrgu));
            #endregion

            var inspection = new InspectionUpdateInspectionUpdateCommonType
            {
                guid = disposal.ErpGuid.ToUpper(),
                startDate = disposal.DateStart.GetValueOrDefault(),
                startDateSpecified = disposal.DateStart.HasValue,
                isStartMonth = false,
                isStartMonthSpecified = true,
                iTypeId = disposal.KindCheck?.Code == TypeCheck.PlannedExit || disposal.KindCheck?.Code == TypeCheck.PlannedDocumentation
                    ? "0"
                    : "1",
                fzId = "0",
                prosecId = "1050160000",
                domainId = "1033920000000001"
            };

            resultItems.Add(this.GetUpdateInspectionRequestType(disposal, inspection, ItemChoiceType.UpdateInspection));

            return new LetterToErpType
            {
                Item = new LetterToErpTypeSet
                {
                    Items = resultItems.Where(x => x != null).ToArray()
                }
            };
        }

        private UpdateInspectionRequestType GetUpdateInspectionRequestType(TatarstanDisposal disposal, object item, ItemChoiceType itemChoiceType)
        {
            return item == null
                ? null
                : new UpdateInspectionRequestType
                {
                    guid = disposal.ErpGuid.ToUpper(),
                    Item = item,
                    ItemElementName = itemChoiceType
                };
        }

        private bool IsLicenseControl(TatarstanDisposal disposal)
        {
            var inspectionType = disposal.Inspection.TypeBase;
            var manOrg = this.Container.ResolveDomain<ManagingOrganization>();

            using (this.Container.Using(manOrg))
            {
                var inspection = disposal.Inspection;
                var contragent = manOrg.GetAll().Where(x => x.Contragent == inspection.Contragent && x.TypeManagement == TypeManagementManOrg.UK);
                var objectInspection = inspection.PersonInspection;

                switch (inspectionType)
                {
                    case TypeBase.PlanJuridicalPerson:
                        return contragent.Any();
                    case TypeBase.ProsecutorsClaim:
                    case TypeBase.DisposalHead:
                        return (objectInspection == PersonInspection.Official || objectInspection == PersonInspection.Organization) && contragent.Any();
                    case TypeBase.CitizenStatement:
                        return (objectInspection == PersonInspection.Official || objectInspection == PersonInspection.Organization ||
                                objectInspection == PersonInspection.RealityObject)
                            && contragent.Any();
                }

                return false;
            }
        }

        private List<IResultInspectorCommonType> GetResultInspectors(
            DocumentGjiInspector[] actInspectorsDict,
            bool isUpdate,
            ActCheck actCheck)
        {
            var type = isUpdate
                ? typeof(IResultInspectorUpdateInspectionUpdateCommonType)
                : typeof(IResultInspectorUpdateInspectionInsertCommonType);

            return actInspectorsDict
                .WhereIf(isUpdate, y => !string.IsNullOrWhiteSpace(y.ErpGuid))
                .WhereIf(!isUpdate, y => string.IsNullOrWhiteSpace(y.ErpGuid))
                .Select(y =>
                {
                    var instance = Activator.CreateInstance(type);
                    if (!isUpdate)
                    {
                        DisposalPrepareDataTask.SetPropertyValue(instance,
                            type,
                            "rGuid",
                            actCheck.ErpGuid?.ToUpper() ?? this.erpGuidsList
                                .FirstOrDefault(f => f.EntityType == typeof(ActCheck).FullName && f.EntityId == actCheck.Id)?.Guid);
                    }

                    DisposalPrepareDataTask.SetPropertyValue(instance,
                        type,
                        "numGuid",
                        isUpdate ? y.ErpGuid?.ToUpper() : this.GenGuid(y, typeof(DocumentGjiInspector)));
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "inspectorTypeId", "1");
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "fullName", y.Inspector.Fio);
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "position", y.Inspector.Position);

                    return (IResultInspectorCommonType) instance;
                }).ToList();
        }

        private List<IViolationUpdateInspectionCommonType> GetViolations(IEnumerable<ActCheckViolation> actViolations, bool isUpdate, ActCheck actCheck)
        {
            var type = isUpdate
                ? typeof(IViolationUpdateInspectionUpdateCommonType)
                : typeof(IViolationUpdateInspectionInsertCommonType);

            return actViolations
                .WhereIf(isUpdate, y => !string.IsNullOrWhiteSpace(y.ErpGuid))
                .WhereIf(!isUpdate, y => string.IsNullOrWhiteSpace(y.ErpGuid))
                .Select(y =>
                {
                    var instance = Activator.CreateInstance(type);

                    if (!isUpdate)
                    {
                        DisposalPrepareDataTask.SetPropertyValue(instance,
                            type,
                            "rGuid",
                            actCheck.ErpGuid?.ToUpper() ?? this.erpGuidsList
                                .FirstOrDefault(f => f.EntityType == typeof(ActCheck).FullName && f.EntityId == actCheck.Id)?.Guid);
                    }

                    DisposalPrepareDataTask.SetPropertyValue(instance,
                        type,
                        "numGuid",
                        isUpdate ? y.ErpGuid?.ToUpper() : this.GenGuid(y, typeof(ActCheckViolation)));
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "iViolationTypeId", "1");
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "violationNote", y.InspectionViolation.Violation.Name);
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "violationAct", y.InspectionViolation.Violation.CodePin);

                    return (IViolationUpdateInspectionCommonType) instance;
                }).ToList();
        }

        private List<VInjunctionCommonType> GetVInjunction(
            IEnumerable<Prescription> actPrescriptions,
            IEnumerable<PrescriptionViol> prescriptionViols,
            IEnumerable<ActCheckViolation> actCheckViols,
            bool isUpdate)
        {
            var type = isUpdate
                ? typeof(VInjunctionUpdateInspectionUpdateCommonType)
                : typeof(VInjunctionUpdateInspectionInsertCommonType);

            return prescriptionViols
                .WhereIf(isUpdate, y => !string.IsNullOrWhiteSpace(y.ErpGuid))
                .WhereIf(!isUpdate, y => string.IsNullOrWhiteSpace(y.ErpGuid))
                .Select(y =>
                {
                    var prescription = actPrescriptions.FirstOrDefault(x => x.Id == y.Document.Id);
                    if (prescription == null)
                    {
                        return null;
                    }

                    var actCheckViolation = actCheckViols.FirstOrDefault(x => x.InspectionViolation.Id == y.InspectionViolation.Id);

                    var instance = Activator.CreateInstance(type);
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "code", prescription.DocumentNumber);
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "dateAppointment", prescription.DocumentDate.GetValueOrDefault());
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "dateAppointmentSpecified", prescription.DocumentDate.HasValue);
                    DisposalPrepareDataTask.SetPropertyValue(instance,
                        type,
                        "executionNote",
                        prescription.Closed == YesNoNotSet.Yes
                            ? "Выявленные нарушения в результате проведения КНМ устранены"
                            : null);
                    DisposalPrepareDataTask.SetPropertyValue(instance,
                        type,
                        "executionDeadline",
                        y.DatePlanRemoval.HasValue
                            ? y.DatePlanRemoval.Value
                            : default(DateTime));
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "executionDeadlineSpecified", y.DatePlanRemoval.HasValue);

                    if (!isUpdate)
                    {
                        DisposalPrepareDataTask.SetPropertyValue(instance,
                            type,
                            "vGuid",
                            actCheckViolation.ErpGuid?.ToUpper() ??
                            this.erpGuidsList.FirstOrDefault(f => f.EntityType == typeof(ActCheckViolation).FullName
                                && f.EntityId == actCheckViolation.Id)?.Guid);
                    }

                    DisposalPrepareDataTask.SetPropertyValue(instance,
                        type,
                        "numGuid",
                        isUpdate ? y.ErpGuid?.ToUpper() : this.GenGuid(y, typeof(PrescriptionViol)));

                    return (VInjunctionCommonType) instance;
                })
                .Where(x => x != null)
                .ToList();
        }

        private List<VClassificationLbCommonType> GetVClassificationLbs(IEnumerable<ActCheckViolation> actViolations, bool isUpdate)
        {
            var type = isUpdate
                ? typeof(VClassificationLbUpdateInspectionUpdateCommonType)
                : typeof(VClassificationLbUpdateInspectionInsertCommonType);

            return actViolations
                .WhereIf(isUpdate, y => !string.IsNullOrWhiteSpace(y.ErpGuid))
                .WhereIf(!isUpdate, y => string.IsNullOrWhiteSpace(y.ErpGuid))
                .Select(y =>
                {
                    var instance = Activator.CreateInstance(type);

                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "lbDocumentName", y.InspectionViolation.Violation.CodePin);
                    if (isUpdate)
                    {
                        DisposalPrepareDataTask.SetPropertyValue(instance, type, "vGuid", y.ErpGuid.ToUpper());
                    }

                    DisposalPrepareDataTask.SetPropertyValue(instance,
                        type,
                        "numGuid",
                        isUpdate ? y.ErpGuid?.ToUpper() : this.GenGuid(y, typeof(ActCheckViolation)));

                    return (VClassificationLbCommonType) instance;
                }).ToList();
        }

        private List<IResultRepresentativeCommonType> GetResultRepresentatives(IEnumerable<ActCheckWitness> actWitnesses, bool isUpdate, ActCheck actCheck)
        {
            var type = isUpdate
                ? typeof(IResultRepresentativeUpdateInspectionUpdateCommonType)
                : typeof(IResultRepresentativeUpdateInspectionInsertCommonType);

            return actWitnesses
                .WhereIf(isUpdate, y => !string.IsNullOrWhiteSpace(y.ErpGuid))
                .WhereIf(!isUpdate, y => string.IsNullOrWhiteSpace(y.ErpGuid))
                .Select(y =>
                {
                    var instance = Activator.CreateInstance(type);

                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "representativeTypeId", "2");
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "representativeFullName", y.Fio);
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "representativePosition", y.Position);

                    if (!isUpdate)
                    {
                        DisposalPrepareDataTask.SetPropertyValue(instance,
                            type,
                            "rGuid",
                            actCheck.ErpGuid?.ToUpper() ?? this.erpGuidsList
                                .FirstOrDefault(f => f.EntityType == typeof(ActCheck).FullName && f.EntityId == actCheck.Id)?.Guid);
                    }

                    DisposalPrepareDataTask.SetPropertyValue(instance,
                        type,
                        "numGuid",
                        isUpdate ? y.ErpGuid?.ToUpper() : this.GenGuid(y, typeof(ActCheckWitness)));

                    return (IResultRepresentativeCommonType) instance;
                }).ToList();
        }

        private List<IResultInformationCommonType> GetResultInfos(ActCheckRealityObject actCheckRo, bool isUpdate, ActCheck actCheck)
        {
            var type = isUpdate
                ? typeof(IResultInformationUpdateInspectionUpdateCommonType)
                : typeof(IResultInformationUpdateInspectionInsertCommonType);

            var result = new List<IResultInformationCommonType>();

            // Убрана по причине невозможности хранить 2 numGuid для 1 сущности в текущей реализации
            // if (actCheckRo?.HaveViolation == YesNoNotSet.No)
            // {
            //     var instance = Activator.CreateInstance(type);
            //
            //     ExportDisposalPrepareDataTask.SetPropertyValue(instance, type, "text", actCheckRo.Description);
            //     ExportDisposalPrepareDataTask.SetPropertyValue(instance, type, "resultInformationTypeId", "2");
            //
            //     result.Add((IResultInformationCommonType) instance);
            // }

            if (actCheck?.AcquaintState != AcquaintState.NotAcquainted)
            {
                var instance = Activator.CreateInstance(type);

                DisposalPrepareDataTask.SetPropertyValue(instance,
                    type,
                    "text",
                    actCheck.AcquaintState == AcquaintState.RefuseToAcquaint
                        ? $"Должностное лицо, отказавшееся от ознакомления с актом проверки {actCheck.RefusedToAcquaintPerson}"
                        : $"Должностное лицо, ознакомившееся с актом проверки {actCheck.AcquaintedPerson}");
                DisposalPrepareDataTask.SetPropertyValue(instance, type, "resultInformationTypeId", "0");
                DisposalPrepareDataTask.SetPropertyValue(instance,
                    type,
                    "numGuid",
                    isUpdate ? actCheckRo.ErpGuid?.ToUpper() : this.GenGuid(actCheckRo, typeof(ActCheckRealityObject)));

                result.Add((IResultInformationCommonType) instance);
            }

            foreach (var obj in result)
            {
                if (!isUpdate)
                {
                    DisposalPrepareDataTask.SetPropertyValue(obj,
                        type,
                        "rGuid",
                        actCheck.ErpGuid?.ToUpper() ?? this.erpGuidsList
                            .FirstOrDefault(f => f.EntityType == typeof(ActCheck).FullName && f.EntityId == actCheck.Id)?.Guid);
                }
            }

            return result;
        }

        private IResultUpdateInspectionCommonType GetIResult(
            ActCheck actCheck,
            ActCheckRealityObject actCheckRo,
            DateTime actCheckStartDate,
            decimal actCheckDurationHours,
            int actCheckDurationDays,
            InspectionGjiRealityObject inspectionGjiRealityObject)
        {
            var isUpdate = !string.IsNullOrWhiteSpace(actCheck.ErpGuid);
            var type = isUpdate
                ? typeof(IResultUpdateInspectionUpdateCommonType)
                : typeof(IResultUpdateInspectionInsertCommonType);

            var instance = Activator.CreateInstance(type);

            DisposalPrepareDataTask.SetPropertyValue(instance, type, "actDateCreate", actCheck.DocumentDate.GetValueOrDefault());
            DisposalPrepareDataTask.SetPropertyValue(instance, type, "actDateCreateSpecified", actCheck.DocumentDate.HasValue);
            DisposalPrepareDataTask.SetPropertyValue(instance, type, "actAddress", actCheckRo?.RealityObject.Address);
            DisposalPrepareDataTask.SetPropertyValue(instance, type, "actAddressTypeId", "44");
            DisposalPrepareDataTask.SetPropertyValue(instance, type, "actWasNotRead", actCheck.AcquaintState == AcquaintState.RefuseToAcquaint);
            DisposalPrepareDataTask.SetPropertyValue(instance, type, "actWasNotReadSpecified", true);
            DisposalPrepareDataTask.SetPropertyValue(instance, type, "startDate", actCheckStartDate);
            DisposalPrepareDataTask.SetPropertyValue(instance, type, "startDateSpecified", actCheckStartDate.IsValid());
            DisposalPrepareDataTask.SetPropertyValue(instance,
                type,
                "durationHours",
                actCheckDurationHours > 1 ? actCheckDurationHours.ToStr() : "1");
            DisposalPrepareDataTask.SetPropertyValue(instance, type, "durationDays", actCheckDurationDays.ToStr());

            //вернуть при изменении логики ерп
            //ExportDisposalPrepareDataTask.SetPropertyValue(instance, type, "houseGuid", actCheckRo?.RealityObject.FiasAddress?.HouseGuid?.ToString().ToUpper());

            if (!isUpdate)
            {
                DisposalPrepareDataTask.SetPropertyValue(instance,
                    type,
                    "oGuid",
                    inspectionGjiRealityObject.ErpGuid?.ToUpper() ??
                    this.erpGuidsList.FirstOrDefault(f =>
                        f.EntityType == typeof(InspectionGjiRealityObject).FullName && f.EntityId == inspectionGjiRealityObject.Id)?.Guid);
            }

            DisposalPrepareDataTask.SetPropertyValue(instance,
                type,
                "numGuid",
                isUpdate ? actCheck.ErpGuid?.ToUpper() : this.GenGuid(actCheck, typeof(ActCheck)));

            return (IResultUpdateInspectionCommonType) instance;
        }

        private List<ChlistsQuestionsCommonType> GetControlListQuestions(bool isUpdate)
        {
            var type = isUpdate
                ? typeof(ChlistsQuestionsUpdateInspectionUpdateCommonType)
                : typeof(ChlistsQuestionsUpdateInspectionInsertCommonType);

            return this.controlListQuestions
                .WhereIf(isUpdate, y => !string.IsNullOrWhiteSpace(y.ErpGuid))
                .WhereIf(!isUpdate, y => string.IsNullOrWhiteSpace(y.ErpGuid))
                .Select(y =>
                {
                    var instance = Activator.CreateInstance(type);
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "props", y.TypicalQuestion.NormativeDoc.Name);
                    DisposalPrepareDataTask.SetPropertyValue(instance,
                        type,
                        "chlistAnswerTypeId",
                        string.Equals("да", y.TypicalAnswer.Answer, StringComparison.OrdinalIgnoreCase)
                            ? "1"
                            : string.Equals("нет", y.TypicalAnswer.Answer, StringComparison.OrdinalIgnoreCase)
                                ? "2"
                                : "3");
                    DisposalPrepareDataTask.SetPropertyValue(instance,
                        type,
                        "answerText",
                        !string.Equals("да", y.TypicalAnswer.Answer, StringComparison.OrdinalIgnoreCase)
                        && string.Equals("нет", y.TypicalAnswer.Answer, StringComparison.OrdinalIgnoreCase)
                            ? y.TypicalAnswer.Answer
                            : null);
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "question", y.TypicalQuestion.Question);
                    if (!isUpdate)
                    {
                        DisposalPrepareDataTask.SetPropertyValue(instance, type, "chGuid", y.ControlList.ErpGuid?.ToUpper());
                    }

                    DisposalPrepareDataTask.SetPropertyValue(instance,
                        type,
                        "numGuid",
                        isUpdate ? y.ErpGuid?.ToUpper() : this.GenGuid(y, typeof(ControlListQuestion)));

                    return (ChlistsQuestionsCommonType) instance;
                }).ToList();
        }

        private List<ChlistsUpdateInspectionCommonType> GetControlLists(bool isUpdate, InspectionGjiRealityObject inspectionGjiRealityObject)
        {
            var type = isUpdate
                ? typeof(ChlistsUpdateInspectionUpdateCommonType)
                : typeof(ChlistsUpdateInspectionInsertCommonType);

            return this.controlLists
                .WhereIf(isUpdate, y => !string.IsNullOrWhiteSpace(y.ErpGuid))
                .WhereIf(!isUpdate, y => string.IsNullOrWhiteSpace(y.ErpGuid))
                .Select(y =>
                {
                    var instance = Activator.CreateInstance(type);
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "name", y.Name);
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "approval", y.ApprovalDetails);
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "frguOrgBkId", this.frguParticipantId.ToUpper());
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "oGuid", inspectionGjiRealityObject.ErpGuid?.ToUpper());
                    if (isUpdate)
                    {
                        DisposalPrepareDataTask.SetPropertyValue(instance,
                            type,
                            "numGuid",
                            isUpdate ? y.ErpGuid?.ToUpper() : this.GenGuid(y, typeof(ControlList)));
                    }

                    return (ChlistsUpdateInspectionCommonType) instance;
                }).ToList();
        }

        private ObjectUpdateInspectionCommonType GetIObject(
            InspectionGjiRealityObject inspectionGjiRealityObject,
            TatarstanDisposal disposal)
        {
            var isUpdate = !string.IsNullOrWhiteSpace(inspectionGjiRealityObject.ErpGuid);

            var type = isUpdate
                ? typeof(ObjectUpdateInspectionUpdateCommonType)
                : typeof(ObjectUpdateInspectionInsertCommonType);

            var instance = Activator.CreateInstance(type);

            //вернуть при изменении логики ерп
            //ExportDisposalPrepareDataTask.SetPropertyValue(instance, type, "houseGuid", inspectionGjiRealityObject.RealityObject.FiasAddress?.HouseGuid?.ToString().ToUpper());
            DisposalPrepareDataTask.SetPropertyValue(instance, type, "addressTypeId", "2");
            DisposalPrepareDataTask.SetPropertyValue(instance, type, "iObjectTypeId", "4");
            DisposalPrepareDataTask.SetPropertyValue(instance, type, "address", inspectionGjiRealityObject.RealityObject.Address);
            if (!isUpdate)
            {
                DisposalPrepareDataTask.SetPropertyValue(instance, type, "iGuid", disposal.ErpGuid.ToUpper());
            }

            DisposalPrepareDataTask.SetPropertyValue(instance,
                type,
                "numGuid",
                isUpdate ? inspectionGjiRealityObject.ErpGuid?.ToUpper() : this.GenGuid(inspectionGjiRealityObject, typeof(InspectionGjiRealityObject)));

            return (ObjectUpdateInspectionCommonType) instance;
        }

        private List<ClassificationLbCommonType> GetIClassificationLbs(bool isUpdate, TatarstanDisposal disposal)
        {
            var type = isUpdate
                ? typeof(ClassificationLbUpdateInspectionUpdateCommonType)
                : typeof(ClassificationLbUpdateInspectionInsertCommonType);

            return this.inspFoundationChecks
                .WhereIf(isUpdate, y => !string.IsNullOrWhiteSpace(y.ErpGuid))
                .WhereIf(!isUpdate, y => string.IsNullOrWhiteSpace(y.ErpGuid))
                .Select(x =>
                {
                    var instance = Activator.CreateInstance(type);
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "lbDocumentName", x.InspFoundationCheck.Name);
                    if (!isUpdate)
                    {
                        DisposalPrepareDataTask.SetPropertyValue(instance, type, "iGuid", disposal.ErpGuid.ToUpper());
                    }

                    DisposalPrepareDataTask.SetPropertyValue(instance,
                        type,
                        "numGuid",
                        isUpdate ? x.ErpGuid?.ToUpper() : this.GenGuid(x, typeof(DisposalInspFoundationCheck)));

                    return (ClassificationLbCommonType) instance;
                }).ToList();
        }

        private List<AuthorityJointlyFrguCommonType> GetIAuthorityJointlyFrgus(bool isUpdate, TatarstanDisposal disposal)
        {
            var type = isUpdate
                ? typeof(AuthorityJointlyFrguUpdateInspectionUpdateCommonType)
                : typeof(AuthorityJointlyFrguUpdateInspectionInsertCommonType);

            return this.inspectionContragents
                .WhereIf(isUpdate, y => !string.IsNullOrWhiteSpace(y.ErpGuid))
                .WhereIf(!isUpdate, y => string.IsNullOrWhiteSpace(y.ErpGuid))
                .Select(x =>
                {
                    var instance = Activator.CreateInstance(type);
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "frguOrgGuid", this.frguParticipantId.ToUpper());
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "iGuid", disposal.ErpGuid.ToUpper());
                    DisposalPrepareDataTask.SetPropertyValue(instance,
                        type,
                        isUpdate ? "frguJointlyGuid" : "idBk",
                        isUpdate ? x.ErpGuid.ToUpper() : x.FrguOrgNumber);
                    return (AuthorityJointlyFrguCommonType) instance;
                }).ToList();
        }

        private List<InspectorCommonType> GetIInspectors(bool isUpdate, TatarstanDisposal disposal, DocumentGjiInspector[] inspectors)
        {
            var type = isUpdate
                ? typeof(InspectorUpdateInspectionUpdateCommonType)
                : typeof(InspectorUpdateInspectionInsertCommonType);

            return inspectors
                .WhereIf(isUpdate, x => !string.IsNullOrWhiteSpace(x.ErpGuid))
                .WhereIf(!isUpdate, x => string.IsNullOrWhiteSpace(x.ErpGuid))
                .Select(x =>
                {
                    var instance = Activator.CreateInstance(type);

                    if (!isUpdate)
                    {
                        DisposalPrepareDataTask.SetPropertyValue(instance, type, "iGuid", disposal.ErpGuid.ToUpper());
                        DisposalPrepareDataTask.SetPropertyValue(instance, type, "frguOrgGuid", this.frguSupervisoryBodyId.ToUpper());
                    }

                    DisposalPrepareDataTask.SetPropertyValue(instance,
                        type,
                        "numGuid",
                        isUpdate ? x.ErpGuid?.ToUpper() : this.GenGuid(x, typeof(DocumentGjiInspector)));
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "inspectorTypeId", "1");
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "fullName", x.Inspector.Fio);
                    DisposalPrepareDataTask.SetPropertyValue(instance, type, "position", x.Inspector.Position);

                    return (InspectorCommonType) instance;
                }).ToList();
        }

        private static void SetPropertyValue(object instance, Type typeOfInstance, string propertyName, object value)
        {
            var property = typeOfInstance.GetProperty(propertyName);

            if (property == null)
            {
                return;
            }

            property.SetValue(instance, value);
        }
    }
}