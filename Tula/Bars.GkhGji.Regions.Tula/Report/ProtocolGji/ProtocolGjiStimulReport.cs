namespace Bars.GkhGji.Regions.Tula.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tula.Entities;
    using GkhGji.Report;
    using Slepov.Russian.Morpher;

    using Stimulsoft.Report;

    public class ProtocolGjiStimulReport : GjiBaseStimulReport
    {
         #region .ctor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ProtocolGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.ProtocolGjiStimulReport))
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Формат печатной формы.
        /// </summary>
        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
        }

        /// <summary>
        /// Идентификатор отчета.
        /// </summary>
        public override string Id
        {
            get
            {
                return "Protocol";
            }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати.
        /// </summary>
        public override string CodeForm
        {
            get
            {
                return "Protocol";
            }
        }

        /// <summary>
        /// Наименование отчета.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Протокол";
            }
        }

        /// <summary>
        /// Описание отчета.
        /// </summary>
        public override string Description
        {
            get
            {
                return "Протокол";
            }
        }

        /// <summary>
        /// Код шаблона (файла).
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// ИД документа.
        /// </summary>
        private long DocumentId { get; set; }

        /// <summary>
        /// Установить пользовательские параметры.
        /// </summary>
        /// <param name="userParamsValues">
        /// Значения пользовательских параметров.
        /// </param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        /// <summary>
        /// Получить список шаблонов.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Code = "Protocol",
                                   Name = "Протокол",
                                   Description = "Протокол",
                                   Template = Properties.Resources.ActCheck_Common
                               }
                       };
        }

        /// <summary>
        /// Получить поток шаблона отчета (файла).
        /// </summary>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        #endregion

        #region DomainServices
        
        public IDomainService<IDisposalText> DisposalTextDomain { get; set; }
        public IDomainService<ActCheck> ActCheckDomain { get; set; }
        public IDomainService<ActCheckPeriod> ActCheckPeriodDomain { get; set; }
        public IDomainService<ContragentBank> ContragentBankDomain { get; set; }
        public IDomainService<ContragentContact> ContragentContactDomain { get; set; }
        public IDomainService<Disposal> DisposalDomain { get; set; }
        public IDomainService<DisposalTypeSurvey> DisposalTypeSurveyDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomain { get; set; }
        public IDomainService<DocumentGJIPhysPersonInfo> DocumentGjiPhysPersonInfoDomain { get; set; }
        public IDomainService<DocumentViolGroup> DocumentViolGroupDomain { get; set; }
        public IDomainService<DocumentViolGroupLongText> DocumentViolGroupLongTextDomain { get; set; }
        public IDomainService<DocumentViolGroupPoint> DocumentViolGroupPointDomain { get; set; }
        public IDomainService<InspectionGjiRealityObject> InspectionGjiRealityObjectDomain { get; set; }
        public IDomainService<PrescriptionViol> PrescriptionViolDomain { get; set; }
        public IDomainService<Protocol> ProtocolDomain { get; set; }
        public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }
        public IDomainService<ProtocolLongDescription> ProtocolLongDescriptionDomain { get; set; }
        public IDomainService<ProtocolViolation> ProtocolViolationDomain { get; set; }
        public IDomainService<ZonalInspectionInspector> ZonalInspectionInspectorDomain { get; set; }
        public IDomainService<ZonalInspectionMunicipality> ZonalInspectionMunicipalityDomain { get; set; }
        public IDomainService<ViolationActionsRemovGji> ViolationActionsRemovGjiDomain { get; set; }

        #endregion

        /// <summary>
        /// Подготовить параметры отчета.
        /// </summary>
        /// <param name="reportParams">
        /// Параметры отчета.
        /// </param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var protocol = this.ProtocolDomain.Get(this.DocumentId);
            if (protocol == null)
            {
                throw new ReportProviderException("Не удалось получить протокол");
            }

            FillCommonFields(protocol);
            this.Report["Номер"] = protocol.DocumentNumber;
            this.Report["ДатаПротокола"] = protocol.DocumentDate.HasValue
                                          ? protocol.DocumentDate.Value.ToShortDateString()
                                          : string.Empty;

            var realObjs = this.ProtocolViolationDomain.GetAll()
                .Where(x => x.Document.Id == protocol.Id && x.InspectionViolation.RealityObject != null)
                .Select(x => x.InspectionViolation.RealityObject)
                .Distinct()
                .ToList();
            var firstRealObj = realObjs.FirstOrDefault();

            this.Report["НаселенныйПункт"] = firstRealObj != null ? firstRealObj.FiasAddress.PlaceName : string.Empty;
            this.Report["АдресОбъекта"] = realObjs.Select(x => x.Address).AggregateWithSeparator(", ");

            var parent = this.DocumentGjiChildrenDomain.GetAll()
                .Where(x => x.Children.Id == this.DocumentId)
                .Select(x => new
                {
                    parentId = x.Parent.Id,
                    x.Parent.TypeDocumentGji,
                    x.Parent.DocumentDate,
                    x.Parent.DocumentNumber
                })
                .FirstOrDefault();

            this.Report["ИсполнительныйДокумент"] = parent != null 
                ? string.Format(
                    "{0} №{1} от {2}", 
                    GkhGji.Utils.Utils.GetDocumentName(parent.TypeDocumentGji),
                    parent.DocumentNumber,
                    parent.DocumentDate.HasValue ? parent.DocumentDate.Value.ToShortDateString() : string.Empty)
                : string.Empty;

            var protocolArticleLaws = this.ProtocolArticleLawDomain.GetAll()
                    .Where(x => x.Protocol.Id == protocol.Id)
                    .Select(x => new { x.ArticleLaw.Name, x.ArticleLaw.Description })
                    .ToList();

            this.Report["СтатьяОписание"] = protocolArticleLaws.AggregateWithSeparator(x => x.Name + " - " + x.Description, ", ");

            var executant = protocol.Executant;

            this.Report["Вотношении"] = executant != null ? executant.Code : string.Empty;

            var contragent = protocol.Contragent;

            if (contragent != null)
            {
                this.Report["Наименование"] = contragent.Name;
                this.Report["КонтрагентСокр"] = contragent.ShortName;

                var contact = this.ContragentContactDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == contragent.Id);
                this.Report["ДолжностьКонтр"] = contact != null ? contact.Position.Name : string.Empty;
                this.Report["ФИО"] = contact != null ? contact.FullName : string.Empty;

                this.Report["АдресЮР"] = contragent.JuridicalAddress;
                this.Report["АдресФакт"] = contragent.FactAddress;

                var contragentBank = this.ContragentBankDomain.GetAll()
                    .Where(x => x.Contragent.Id == contragent.Id)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Bik,
                        x.CorrAccount,
                        x.SettlementAccount
                    })
                    .OrderBy(x => x.Id)
                    .FirstOrDefault();
                this.Report["РасСчет"] = contragentBank != null ? contragentBank.SettlementAccount : string.Empty;
                this.Report["КорСчет"] = contragentBank != null ? contragentBank.CorrAccount : string.Empty;
                this.Report["Банк"] = contragentBank != null ? contragentBank.Name : string.Empty;
                this.Report["Бик"] = contragentBank != null ? contragentBank.Bik : string.Empty;

                this.Report["ИНН"] = contragent.Inn;
                this.Report["КПП"] = contragent.Kpp;
                this.Report["ОГРН"] = contragent.Ogrn;
            }

            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");
            
            if (!protocol.PhysicalPerson.IsEmpty())
            {
                this.Report["ФизЛицо"] = protocol.PhysicalPerson;
                var physPerson = склонятель.Проанализировать(protocol.PhysicalPerson);

                if (physPerson != null)
                {
                    this.Report["ФизЛицоРП"] = physPerson.Родительный;
                    this.Report["ФизЛицоДП"] = physPerson.Дательный;
                }
            }

            var firstPhysPersonInfo = this.DocumentGjiPhysPersonInfoDomain.GetAll()
                .FirstOrDefault(x => x.Document.Id == protocol.Id);
            
            if (firstPhysPersonInfo != null)
            {
                this.Report["АдресТелефон"] = firstPhysPersonInfo.PhysPersonAddress;
                this.Report["МестоРаботы"] = firstPhysPersonInfo.PhysPersonJob;
                this.Report["Должность"] = firstPhysPersonInfo.PhysPersonPosition;

                if (!firstPhysPersonInfo.PhysPersonPosition.IsEmpty())
                {
                    var physPersPosition = склонятель.Проанализировать(firstPhysPersonInfo.PhysPersonPosition);
                    this.Report["ДолжностьРП"] = physPersPosition.Родительный;
                }

                this.Report["ДатаМестоРождения"] = firstPhysPersonInfo.PhysPersonBirthdayAndPlace;
                this.Report["ДокументУдостовЛичность"] = firstPhysPersonInfo.PhysPersonDocument;
            }

            var disposal = (Disposal)GkhGji.Utils.Utils.GetParentDocumentByType(this.DocumentGjiChildrenDomain, protocol, TypeDocumentGji.Disposal);
            if (disposal != null)
            {
                this.Report["НомерПриказа"] = disposal.DocumentNumber;
                this.Report["ДатаПриказа"] = disposal.DocumentDate.HasValue
                                                 ? disposal.DocumentDate.Value.ToShortDateString()
                                                 : string.Empty;
                this.Report["ВидПроверки"] = disposal.KindCheck != null ? disposal.KindCheck.Name : string.Empty;
                Report["ОснованиеПроверки"] = disposal.Inspection != null
                                              ? Convert.ChangeType(
                                                  disposal.Inspection.TypeBase,
                                                  disposal.Inspection.TypeBase.GetTypeCode())
                                              : "";
            }

            var act = (ActCheck)GkhGji.Utils.Utils.GetParentDocumentByType(this.DocumentGjiChildrenDomain, protocol, TypeDocumentGji.ActCheck);
            if (act != null)
            {
                this.Report["НомерАкта"] = act.DocumentNumber;
                this.Report["ДатаАкта"] = act.DocumentDate.HasValue
                                              ? act.DocumentDate.Value.ToShortDateString()
                                              : string.Empty;
            }

            var firstInspector = this.DocumentGjiInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == protocol.Id)
                .Select(x => x.Inspector)
                .FirstOrDefault();

            if (firstInspector != null)
            {
                this.Report["ДолжностьИнспектораТП"] = firstInspector.PositionAblative;
                this.Report["ИнспекторФИОТП"] = firstInspector.FioAblative;
            }

            var query = this.DocumentViolGroupDomain.GetAll()
                .Where(x => x.Document.Id == protocol.Id);

            var violPoints = this.DocumentViolGroupPointDomain.GetAll()
                .Where(x => query.Any(y => y.Id == x.ViolGroup.Id))
                .Select(x => new
                    {
                        x.ViolStage.InspectionViolation.Violation.CodePin,
                        ViolStageId = x.ViolStage.Id,
                        violGroupId = x.ViolGroup.Id
                    })
                .AsEnumerable()
                .GroupBy(x => x.violGroupId)
                .ToDictionary(
                    x => x.Key,
                    y => new
                    {
                        PointCodes = y.Select(z => z.CodePin)
                            .Distinct()
                            .Aggregate((str, result) => !string.IsNullOrEmpty(result) ? result + "," + str : str)
                    });

            var longTexts = this.DocumentViolGroupLongTextDomain.GetAll()
                .Where(x => query.Any(y => y.Id == x.ViolGroup.Id))
                .Select(x => new
                {
                    x.ViolGroup.Id,
                    x.Description
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.Select(x => new
                        {
                            Description = x.Description != null ? Encoding.UTF8.GetString(x.Description) : string.Empty
                        }).First());

            var data = query
                .Select(x => new
                {
                    x.Id,
                    x.Description
                })
                .ToList()
                .Select(x => new Violation
                {
                    Описание = longTexts.ContainsKey(x.Id) && longTexts[x.Id].Description.IsNotEmpty()
                            ? longTexts[x.Id].Description
                            : x.Description,
                    Пункт = violPoints.ContainsKey(x.Id) ? violPoints[x.Id].PointCodes : null
                }).ToList();

            Report.RegData("Нарушения", data);

            var inspectors = this.DocumentGjiInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == protocol.Id)
                .Select(x => new
                                 {
                                     Должность = x.Inspector.Position,
                                     ДолжностьРП = x.Inspector.PositionGenitive,
                                     ДолжностьДП = x.Inspector.PositionDative,
                                     ДолжностьВП = x.Inspector.PositionAccusative,
                                     ДолжностьТП = x.Inspector.PositionAblative,
                                     ДолжностьПП = x.Inspector.PositionPrepositional,
                                     ИнспекторСокр = x.Inspector.ShortFio,
                                     Инспектор = x.Inspector.Fio,
                                     ИнспекторРП = x.Inspector.FioGenitive,
                                     ИнспекторДП = x.Inspector.FioDative,
                                     ИнспекторВП = x.Inspector.FioAccusative,
                                     ИнспекторТП = x.Inspector.FioAblative,
                                     ИнспекторПП = x.Inspector.FioPrepositional
                                 })
                .ToList();

            this.Report.RegData("Инспектор", inspectors);

            var protocolLongDescription = this.ProtocolLongDescriptionDomain.GetAll().FirstOrDefault(x => x.Protocol.Id == protocol.Id);
            if (protocolLongDescription != null)
            {
                this.Report["СоставАП"] = protocolLongDescription.Description != null
                                              ? Encoding.UTF8.GetString(protocolLongDescription.Description)
                                              : protocol.Description;
            }
        }

        /// <summary>
        /// Установить код шаблона (файла).
        /// </summary>
        protected void GetCodeTemplate()
        {
            this.CodeTemplate = "Protocol";
        }

        protected class Violation
        {
            public string Пункт { get; set; }
            public string Описание { get; set; }
        }
    }
}