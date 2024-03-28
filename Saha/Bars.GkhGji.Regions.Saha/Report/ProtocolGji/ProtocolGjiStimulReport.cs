namespace Bars.GkhGji.Regions.Saha.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Saha.Entities;
    using GkhGji.Report;
    using Slepov.Russian.Morpher;

    public class ProtocolGjiStimulReport : GjiBaseStimulReport
    {
         #region .ctor

        public ProtocolGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.ProtocolGjiStimulReport))
        {
        }

        #endregion

        #region Properties

        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
        }

        public override string Id
        {
            get
            {
                return "Protocol";
            }
        }

        public override string CodeForm
        {
            get
            {
                return "Protocol";
            }
        }

        public override string Name
        {
            get
            {
                return "Протокол";
            }
        }

        public override string Description
        {
            get
            {
                return "Протокол";
            }
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

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

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        protected void GetCodeTemplate()
        {
            CodeTemplate = "Protocol";
        }

        protected override string CodeTemplate { get; set; }

        #endregion

        #region Fields

        private long DocumentId { get; set; }

        #endregion

        #region DomainServices

        public IDomainService<Protocol> ProtocolDomain { get; set; }
        public IDomainService<IDisposalText> DisposalTextDomain { get; set; }
        public IDomainService<DisposalTypeSurvey> DisposalTypeSurveyDomain { get; set; }
        public IDomainService<Disposal> DisposalDomain { get; set; }
        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomain { get; set; }
        public IDomainService<ZonalInspectionInspector> ZonalInspectionInspectorDomain { get; set; }
        public IDomainService<ZonalInspectionMunicipality> ZonalInspectionMunicipalityDomain { get; set; }
        public IDomainService<ContragentBank> ContragentBankDomain { get; set; }
        public IDomainService<ContragentContact> ContragentContactDomain { get; set; }
        public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }
        public IDomainService<ProtocolViolation> ProtocolViolationDomain { get; set; }
        public IDomainService<ActCheck> ActCheckDomain { get; set; }
        public IDomainService<ActCheckPeriod> ActCheckPeriodDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<PrescriptionViol> PrescriptionViolDomain { get; set; }
        public IDomainService<ViolationActionsRemovGji> ViolationActionsRemovGjiDomain { get; set; }
        public IDomainService<InspectionGjiRealityObject> InspectionGjiRealityObjectDomain { get; set; }
        public IDomainService<DocumentGJIPhysPersonInfo> DocumentGjiPhysPersonInfoDomain { get; set; }
        public IDomainService<DocumentViolGroup> DocumentViolGroupDomain { get; set; }
        public IDomainService<DocumentViolGroupLongText> DocumentViolGroupLongTextDomain { get; set; }
        public IDomainService<DocumentViolGroupPoint> DocumentViolGroupPointDomain { get; set; }

        #endregion

        public override void PrepareReport(ReportParams reportParams)
        {
            var protocol = ProtocolDomain.Get(DocumentId);
            if (protocol == null)
            {
                throw new ReportProviderException("Не удалось получить протокол");
            }
            FillCommonFields(protocol);

            this.ReportParams["Номер"] = protocol.DocumentNumber;
            this.ReportParams["ДатаПротокола"] = protocol.DocumentDate.HasValue
                                          ? protocol.DocumentDate.Value.ToShortDateString()
                                          : string.Empty;

            var realObjs = ProtocolViolationDomain.GetAll()
                .Where(x => x.Document.Id == protocol.Id && x.InspectionViolation.RealityObject != null)
                .Select(x => x.InspectionViolation.RealityObject)
                .Distinct()
                .ToList();
            var firstRealObj = realObjs.FirstOrDefault();

            this.ReportParams["НаселенныйПункт"] = firstRealObj != null ? firstRealObj.FiasAddress.PlaceName : string.Empty;
            this.ReportParams["АдресОбъекта"] = realObjs.Select(x => x.Address).AggregateWithSeparator(", ");

            var parent = DocumentGjiChildrenDomain.GetAll()
                .Where(x => x.Children.Id == DocumentId)
                .Select(x => new
                {
                    parentId = x.Parent.Id,
                    x.Parent.TypeDocumentGji,
                    x.Parent.DocumentDate,
                    x.Parent.DocumentNumber
                })
                .FirstOrDefault();

            this.ReportParams["ИсполнительныйДокумент"] = parent != null 
                ? string.Format("{0} №{1} от {2}", 
                    GkhGji.Utils.Utils.GetDocumentName(parent.TypeDocumentGji),
                    parent.DocumentNumber,
                    parent.DocumentDate.HasValue ? parent.DocumentDate.Value.ToShortDateString() : string.Empty)
                : string.Empty;

            var protocolArticleLaws = ProtocolArticleLawDomain.GetAll()
                    .Where(x => x.Protocol.Id == protocol.Id)
                    .Select(x => new { x.ArticleLaw.Name, x.ArticleLaw.Description })
                    .ToList();

            this.ReportParams["СтатьяОписание"] = protocolArticleLaws.AggregateWithSeparator(x => x.Name + " - " + x.Description, ", ");

            var executant = protocol.Executant;

            this.ReportParams["Вотношении"] = executant != null ? executant.Code : string.Empty;

            var contragent = protocol.Contragent;

            if (contragent != null)
            {
                this.ReportParams["Наименование"] = contragent.Name;
                this.ReportParams["КонтрагентСокр"] = contragent.ShortName;

                var contact = ContragentContactDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == contragent.Id);
                this.ReportParams["ДолжностьКонтр"] = contact != null ? contact.Position.Name : string.Empty;
                this.ReportParams["ФИО"] = contact != null ? contact.FullName : string.Empty;

                this.ReportParams["АдресЮР"] = contragent.JuridicalAddress;
                this.ReportParams["АдресФакт"] = contragent.FactAddress;

                var contragentBank = ContragentBankDomain.GetAll()
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
                this.ReportParams["РасСчет"] = contragentBank != null ? contragentBank.SettlementAccount : string.Empty;
                this.ReportParams["КорСчет"] = contragentBank != null ? contragentBank.CorrAccount : string.Empty;
                this.ReportParams["Банк"] = contragentBank != null ? contragentBank.Name : string.Empty;
                this.ReportParams["Бик"] = contragentBank != null ? contragentBank.Bik : string.Empty;

                this.ReportParams["ИНН"] = contragent.Inn;
                this.ReportParams["КПП"] = contragent.Kpp;
                this.ReportParams["ОГРН"] = contragent.Ogrn;
            }

            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            if (!protocol.PhysicalPerson.IsEmpty())
            {
                var physPerson = склонятель.Проанализировать(protocol.PhysicalPerson);
                this.ReportParams["ФизЛицо"] = protocol.PhysicalPerson;
                this.ReportParams["ФизЛицоРП"] = physPerson.Родительный;
                this.ReportParams["ФизЛицоДП"] = physPerson.Дательный;
            }

            var firstPhysPersonInfo =
                DocumentGjiPhysPersonInfoDomain.GetAll().FirstOrDefault(x => x.Document.Id == protocol.Id);
            if (firstPhysPersonInfo != null)
            {
                this.ReportParams["АдресТелефон"] = firstPhysPersonInfo.PhysPersonAddress;
                this.ReportParams["МестоРаботы"] = firstPhysPersonInfo.PhysPersonJob;
                this.ReportParams["Должность"] = firstPhysPersonInfo.PhysPersonPosition;

                if (!firstPhysPersonInfo.PhysPersonPosition.IsEmpty())
                {
                    var physPersPosition = склонятель.Проанализировать(firstPhysPersonInfo.PhysPersonPosition);
                    this.ReportParams["ДолжностьРП"] = physPersPosition.Родительный;
                }
                
                this.ReportParams["ДатаМестоРождения"] = firstPhysPersonInfo.PhysPersonBirthdayAndPlace;
                this.ReportParams["ДокументУдостовЛичность"] = firstPhysPersonInfo.PhysPersonDocument;
            }

            var disposal = (Disposal)GkhGji.Utils.Utils.GetParentDocumentByType(DocumentGjiChildrenDomain, protocol, TypeDocumentGji.Disposal);
            if (disposal != null)
            {
                this.ReportParams["НомерПриказа"] = disposal.DocumentNumber;
                this.ReportParams["ДатаПриказа"] = disposal.DocumentDate.HasValue ? disposal.DocumentDate.Value.ToShortDateString() : string.Empty;
                this.ReportParams["ВидПроверки"] = disposal.KindCheck != null ? disposal.KindCheck.Name : string.Empty;
            }

            var act = (ActCheck)GkhGji.Utils.Utils.GetParentDocumentByType(DocumentGjiChildrenDomain, protocol, TypeDocumentGji.ActCheck);
            if (act != null)
            {
                this.ReportParams["НомерАкта"] = act.DocumentNumber;
                this.ReportParams["ДатаАкта"] = act.DocumentDate.HasValue ? act.DocumentDate.Value.ToShortDateString() : string.Empty;
            }

            var firstInspector = DocumentGjiInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == protocol.Id)
                .Select(x => x.Inspector)
                .FirstOrDefault();

            if (firstInspector != null)
            {
                this.ReportParams["ДолжностьИнспектораТП"] = firstInspector.PositionAblative;
                this.ReportParams["ИнспекторФИОТП"] = firstInspector.FioAblative;
            }

            var realObjIds = realObjs.Select(x => x.Id).ToArray();

            var query = DocumentViolGroupDomain.GetAll()
                    .Where(x => realObjIds.Contains(x.RealityObject.Id))
                    .Where(x => x.Document.Id == act.Id);

            var violPoints =
                DocumentViolGroupPointDomain.GetAll()
                               .Where(x => query.Any(y => y.Id == x.ViolGroup.Id))
                               .Select(
                                   x =>
                                   new
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

            var longTexts = DocumentViolGroupLongTextDomain.GetAll()
                .Where(x => query.Any(y => y.Id == x.ViolGroup.Id))
                .Select(x => new
                {
                    x.ViolGroup.Id,
                    x.Description
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => new
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
                });

            this.DataSources.Add(new MetaData
            {
                SourceName = "Нарушения",
                MetaType = nameof(Violation),
                Data = data
            });
        }

        protected class Violation
        {
            public string Пункт { get; set; }
            public string Описание { get; set; }
        }
    }
}