namespace Bars.GkhGji.Regions.Smolensk.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Smolensk.Entities;
    using Bars.GkhGji.Regions.Smolensk.Entities.Resolution;

    using Slepov.Russian.Morpher;

    public class ResolutionGjiStimulReport : GjiBaseStimulReport
    {
        #region .ctor
        public ResolutionGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.Resolution))
        {
        }
        #endregion


        #region Properties
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        public override string Id
        {
            get { return "Resolution"; }
        }

        public override string CodeForm
        {
            get { return "Resolution"; }
        }

        public override string Name
        {
            get { return "Постановление"; }
        }

        public override string Description
        {
            get { return "Постановление"; }
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        protected void GetCodeTemplate()
        {
            CodeTemplate = "Resolution";
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "Resolution",
                            Name = "Постановление",
                            Description = "Постановление",
                            Template = Properties.Resources.Resolution
                        }
                };
        }

        protected override string CodeTemplate { get; set; }

        #endregion


        #region Fields

        private long DocumentId { get; set; }

        #endregion


        #region DomainServices

        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomain { get; set; }
        public IDomainService<Resolution> ResolutionDomain { get; set; }
        public IDomainService<ResolutionLongDescription> ResolutionLongDescriptionDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }
        public IDomainService<ProtocolViolation> ProtocolViolationDomain { get; set; }
        public IDomainService<ResolutionDefinitionSmol> ResolutionDefinitionSmolDomain { get; set; }
        public IDomainService<ContragentContact> ContragentContactDomain { get; set; }
        public IDomainService<ContragentBank> ContragentBankDomain { get; set; }
        public IDomainService<DocumentGJIPhysPersonInfo> DocumentGjiPhysPersonInfoDomain { get; set; }

        #endregion

        public override void PrepareReport(ReportParams reportParams)
        {
            var resolution = ResolutionDomain.Load(DocumentId);

            if (resolution == null)
            {
                throw new ReportProviderException("Не удалось получить постановление");
            }

            if (resolution.Sanction == null)
            {
                throw new ReportProviderException("Не указана санкция");
            }

            FillCommonFields(resolution);

            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            this.ReportParams["НомерПостановления"] = resolution.DocumentNumber;

            this.ReportParams["ДатаПостановления"] = resolution.DocumentDate.HasValue
                    ? resolution.DocumentDate.Value.ToString("d MMMM yyyy")
                    : string.Empty;

            this.ReportParams["ДатаВручения"] = resolution.DeliveryDate.HasValue
                    ? resolution.DeliveryDate.Value.ToString("d MMMM yyyy")
                    : string.Empty;

            if (resolution.Official != null)
            {
                this.ReportParams["КодДлВынесшегоПостановление"] = resolution.Official.Position;
                this.ReportParams["ФИОДлВынесшегоПостановление"] = resolution.Official.Fio;
                this.ReportParams["ФИОРуководителя"] = resolution.Official.ShortFio;
            }

            var resolLong =
                ResolutionLongDescriptionDomain.GetAll()
                    .FirstOrDefault(x => x.Resolution.Id == resolution.Id);

            var longDescription = string.Empty;

            if (resolLong != null)
            {
                longDescription = Encoding.UTF8.GetString(resolLong.Description);
            }

            this.ReportParams["СоставАП"] = longDescription.IsNotEmpty()
                ? longDescription
                : resolution.Description;

            var protocol = (GkhGji.Entities.Protocol)GkhGji.Utils.Utils.GetParentDocumentByType(DocumentGjiChildrenDomain, resolution, TypeDocumentGji.Protocol);

            if (protocol != null)
            {
                this.ReportParams["ИсполнительныйДокумент"] = string.Format(
                    "{0} № {1}", 
                    protocol.DocumentDate.HasValue
                        ? protocol.DocumentDate.Value.ToString("d MMMM yyyy")
                        : string.Empty, 
                    protocol.DocumentNumber);
                var protocolArticleLawList = ProtocolArticleLawDomain.GetAll()
                    .Where(x => x.Protocol.Id == protocol.Id)
                    .Select(x => new
                    {
                        x.ArticleLaw.Name,
                        x.ArticleLaw.Description
                    })
                    .ToList();

                var firstProtocolArticleLaw = protocolArticleLawList.FirstOrDefault();

                this.ReportParams["ОбластьПравонарушения"] = firstProtocolArticleLaw != null
                                                      ? firstProtocolArticleLaw.Name
                                                      : string.Empty;

                this.ReportParams["СтатьяИсполнительногоДокумента"] =
                    protocolArticleLawList.Select(x => x.Name).AggregateWithSeparator(", ");

                this.ReportParams["ОписаниеСтатьи"] =
                    protocolArticleLawList.Select(x => x.Name + " - " + x.Description).AggregateWithSeparator("; ");

                var protocolViolation =
                    ProtocolViolationDomain.GetAll().FirstOrDefault(x => x.Document.Id == protocol.Id);

                this.ReportParams["НаселенныйПункт"] = protocolViolation != null
                    ? protocolViolation.InspectionViolation.RealityObject.FiasAddress.PlaceName
                    : string.Empty;
            }

            var resolDefinitionList =
                ResolutionDefinitionSmolDomain.GetAll()
                    .Where(x => x.Resolution.Id == DocumentId)
                    .Select(x => new
                    {
                        x.DocumentDate,
                        x.DocumentNum,
                        x.TypeDefinition
                    })
                    .ToList();

            var resolDefinitionFirst =
                resolDefinitionList.FirstOrDefault(
                    x => x.TypeDefinition == TypeDefinitionResolution.AppointmentPlaceTime);

            this.ReportParams["Определение"] = resolDefinitionFirst != null
                                        ? string.Format(
                                            "{0} № {1}",
                                            resolDefinitionFirst.DocumentDate.HasValue
                                                ? resolDefinitionFirst.DocumentDate.Value.ToString("d MMMM yyyy")
                                                : string.Empty,
                                            resolDefinitionFirst.DocumentNum)
                                        : string.Empty;

            this.ReportParams["Вотношении"] = resolution.Executant != null ? resolution.Executant.Code : string.Empty;

            this.ReportParams["ВидСанкции"] = resolution.Sanction != null ? resolution.Sanction.Code : string.Empty;

            this.ReportParams["СуммаШтрафа"] = resolution.PenaltyAmount.HasValue ? resolution.PenaltyAmount.Value.ToString("#.##") : string.Empty;

            this.ReportParams["ФизЛицо"] = resolution.PhysicalPerson;
            if (!resolution.PhysicalPerson.IsEmpty())
            {
                var physPersonAllCases = склонятель.Проанализировать(resolution.PhysicalPerson);
                this.ReportParams["ФизЛицоРП"] = physPersonAllCases.Родительный;
                this.ReportParams["ФизЛицоДП"] = physPersonAllCases.Дательный;
            }

            var physPersonInfo =
                DocumentGjiPhysPersonInfoDomain.GetAll().FirstOrDefault(x => x.Document.Id == DocumentId);

            if (physPersonInfo != null)
            {
                this.ReportParams["АдресТелефон"] = physPersonInfo.PhysPersonAddress;
                this.ReportParams["МестоРаботы"] = physPersonInfo.PhysPersonJob;
                this.ReportParams["Должность"] = physPersonInfo.PhysPersonPosition;

                if (!physPersonInfo.PhysPersonPosition.IsEmpty())
                {
                    var physPersonInfoPositionAllCases = склонятель.Проанализировать(physPersonInfo.PhysPersonPosition);
                    this.ReportParams["ДолжностьРП"] = physPersonInfoPositionAllCases.Родительный;
                }

                this.ReportParams["ДатаМестоРождения"] = physPersonInfo.PhysPersonBirthdayAndPlace;
                this.ReportParams["ДокументУдостовЛичность"] = physPersonInfo.PhysPersonDocument;
            }

            var contragent = resolution.Contragent;

            if (contragent != null)
            {
                var contragentContact =
                    ContragentContactDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == contragent.Id);
                this.ReportParams["Наименование"] = contragent.Name;
                this.ReportParams["КонтрагентСокр"] = contragent.ShortName;
                if (contragentContact != null)
                {
                    this.ReportParams["ДолжностьКонтр"] = contragentContact.Position.Name;
                    this.ReportParams["ФИО"] = contragentContact.FullName;
                }
                this.ReportParams["АдресЮР"] = contragent.FiasJuridicalAddress.AddressName;
                this.ReportParams["АдресФакт"] = contragent.FiasFactAddress.AddressName;
                this.ReportParams["ИНН"] = contragent.Inn;
                this.ReportParams["КПП"] = contragent.Kpp;
                this.ReportParams["ОГРН"] = contragent.Ogrn;

                var contragentBank = ContragentBankDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == contragent.Id);
                if (contragentBank != null)
                {
                    this.ReportParams["РасСчет"] = contragentBank.SettlementAccount;
                    this.ReportParams["КорСчет"] = contragentBank.CorrAccount;
                    this.ReportParams["Банк"] = contragentBank.Name;
                    this.ReportParams["Бик"] = contragentBank.Bik;
                }
            }

            var disposal = (Disposal)GkhGji.Utils.Utils.GetParentDocumentByType(DocumentGjiChildrenDomain, resolution, TypeDocumentGji.Disposal);

            if (disposal != null)
            {
                this.ReportParams["ДатаПриказа"] = disposal.DocumentDate.HasValue
                    ? disposal.DocumentDate.Value.ToString("d MMMM yyyy")
                    : string.Empty;
                this.ReportParams["НомерПриказа"] = disposal.DocumentNumber;

                this.ReportParams["ПериодОбследования"] = string.Format(
                    "с {0} по {1}",
                    disposal.DateStart.HasValue ? disposal.DateStart.Value.ToShortDateString() : string.Empty,
                    disposal.DateEnd.HasValue ? disposal.DateEnd.Value.ToShortDateString() : string.Empty);

                if (disposal.KindCheck != null && !disposal.KindCheck.Name.IsEmpty())
                {
                    var kindCheckAllCases = склонятель.Проанализировать(disposal.KindCheck.Name);
                    this.ReportParams["ВидПроверки"] = kindCheckAllCases.Родительный;
                }
            }
        }
    }
}