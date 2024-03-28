namespace Bars.GkhGji.Regions.Saha.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Saha.Entities;
    using GkhGji.Report;

    public class ResolutionGjiStimulReport : GjiBaseStimulReport
    {
        public ResolutionGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.SahaResolution))
        {
        }

        private long DocumentId { get; set; }

        private Resolution Document { get; set; }

        #region Properties

        public override string Id
        {
            get
            {
                return "Resolution";
            }
        }

        public override string CodeForm
        {
            get
            {
                return "Resolution";
            }
        }

        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
        }

        public override string Name
        {
            get
            {
                return "Постановление";
            }
        }

        public override string Description
        {
            get
            {
                return "Постановление";
            }
        }

        protected override string CodeTemplate { get; set; }

        #endregion

        #region Templates

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Code = "SahaResolution_1",
                                   Name = "SahaResolution_1",
                                   Description =
                                       "Постановление предупреждение (Вид санкции: 'Предупреждение''\''устное замечание')",
                                   Template = Properties.Resources.SahaResolution
                               },
                           new TemplateInfo
                               {
                                   Code = "SahaResolution_2",
                                   Name = "SahaResolution_2",
                                   Description =
                                       "Постановление о назначении (Вид санкции: 'Административный штраф')",
                                   Template = Properties.Resources.SahaResolution
                               },
                           new TemplateInfo
                               {
                                   Code = "SahaResolution_3",
                                   Name = "SahaResolution_3",
                                   Description =
                                       "Постановление о прекращении (Вид санкции: 'Прекращено' и определения имеются)",
                                   Template = Properties.Resources.SahaResolution
                               },
                           new TemplateInfo
                               {
                                   Code = "SahaResolution_4",
                                   Name = "SahaResolution_4",
                                   Description =
                                       "Постановление о прекращении без определений (Вид санкции: 'Прекращено' и определения отсутсвуют)",
                                   Template = Properties.Resources.SahaResolution
                               }
                       };
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        private void GetCodeTemplate()
        {
            var resolDefDomain = Container.Resolve<IDomainService<ResolutionDefinition>>();

            try
            {
                CodeTemplate = "SahaResolution_1";

                if (Document.Sanction != null)
                {
                    switch (Document.Sanction.Name)
                    {
                        case "Предупреждение":
                            CodeTemplate = "SahaResolution_1";
                            break;

                        case "Административный штраф":
                            CodeTemplate = "SahaResolution_2";
                            break;

                        case "Прекращено":
                            {
                                CodeTemplate = "SahaResolution_3";
    
                                // Если у постановления с видом санкции прекращено отсутсвуют определения то выводим шаблон для отсутсвующих определений
                                if (!resolDefDomain.GetAll().Any(x => x.Resolution.Id == Document.Id))
                                {
                                    CodeTemplate = "SahaResolution_4";
                                }
                            }
                            break;

                    }
                }
            }
            finally 
            {
                Container.Release(resolDefDomain);
            }
            
        }

        #endregion

        #region DomainServices

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DispInspectorsDomain { get; set; }

        public IDomainService<ContragentContact> CtrContactDomain { get; set; }

        public IDomainService<InspectionGjiRealityObject> InspRoDomain { get; set; }

        public IDomainService<InspectionGji> InspectionGjiDomain { get; set; }

        public IDomainService<DocumentGJIPhysPersonInfo> PhysInfoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocumentDomain { get; set; }

        public IDomainService<ProtocolArticleLaw> ProtocolArtLawDomain { get; set; }

        #endregion

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            Document = ResolutionDomain.GetAll().FirstOrDefault(x => x.Id == DocumentId);

            if (Document == null)
            {
                throw new ReportProviderException("Не удалось получить постановление");
            }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var dateFormat = "«dd» MMMM yyyy г.";

            FillCommonFields(Document);
            this.ReportParams["Дата"] = Document.DocumentDate.HasValue
                                 ? Document.DocumentDate.Value.ToString(dateFormat)
                                 : string.Empty;

            this.ReportParams["Номер"] = Document.DocumentNumber;

            this.ReportParams["Штраф"] = Document.PenaltyAmount.HasValue
                                  ? Document.PenaltyAmount.Value.RoundDecimal(2).ToString()
                                  : "0";

            this.ReportParams["ФизЛицо"] = Document.PhysicalPerson;

            var firstPhysPersonInfo =
                PhysInfoDomain.GetAll()
                              .Where(x => x.Document.Id == Document.Id)
                              .Select(
                                  x => new { x.PhysPersonAddress, x.PhysPersonPosition, x.PhysPersonBirthdayAndPlace })
                              .FirstOrDefault();

            this.ReportParams["Должность"] = "";
            this.ReportParams["ДатаМестоРожд"] = "";
            this.ReportParams["АдресПрож"] = "";

            if (firstPhysPersonInfo != null)
            {
                this.ReportParams["Должность"] = firstPhysPersonInfo.PhysPersonPosition;
                this.ReportParams["ДатаМестоРожд"] = firstPhysPersonInfo.PhysPersonBirthdayAndPlace;
                this.ReportParams["АдресПрож"] = firstPhysPersonInfo.PhysPersonAddress;
            }

            if (Document.Official != null)
            {
                this.ReportParams["КодДлВынесшегоПостановление"] = Document.Official.Position;
                this.ReportParams["ФИОДлВынесшегоПостановление"] = Document.Official.Fio;
                this.ReportParams["ФИОДлВынесшегоПостановлениеСокр"] = Document.Official.ShortFio;
            }

            this.ReportParams["Контрагент"] = "";
            this.ReportParams["КонтрагентСокр"] = "";
            this.ReportParams["ЮрАдрес"] = "";
            this.ReportParams["АдресФакт"] = "";
            this.ReportParams["ОГРН"] = "";
            this.ReportParams["ИНН"] = "";
            this.ReportParams["КПП"] = "";
            this.ReportParams["РуководительКонтрагент"] = "";

            if (Document.Contragent != null)
            {
                this.ReportParams["Контрагент"] = Document.Contragent.Name;
                this.ReportParams["КонтрагентСокр"] = Document.Contragent.ShortName;
                this.ReportParams["ЮрАдрес"] = Document.Contragent.FiasJuridicalAddress != null ? Document.Contragent.FiasJuridicalAddress.AddressName : string.Empty;
                this.ReportParams["АдресФакт"] = Document.Contragent.FiasFactAddress != null ? Document.Contragent.FiasFactAddress.AddressName : string.Empty;
                this.ReportParams["ОГРН"] = Document.Contragent.Ogrn;
                this.ReportParams["ИНН"] = Document.Contragent.Inn;
                this.ReportParams["КПП"] = Document.Contragent.Kpp;

                var headPerson = this.CtrContactDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == this.Document.Contragent.Id && x.Position.Code == "1");

                if (headPerson != null)
                {
                    this.ReportParams["РуководительКонтрагент"] = string.Format("{0} {1}", headPerson.Position.Name, headPerson.FullName);
                }
            }

            // получаем родительский протокол  
            var parentProtocol = this.GetParentDocument(Document, TypeDocumentGji.Protocol);

            if (parentProtocol != null)
            {
                this.ReportParams["ДатаПротокол"] = parentProtocol.DocumentDate.HasValue
                                             ? parentProtocol.DocumentDate.Value.ToString(dateFormat)
                                             : string.Empty;

                this.ReportParams["НомерПротокол"] = parentProtocol.DocumentNumber;
                this.ReportParams["СтатьяЗакона"] =
                    ProtocolArtLawDomain.GetAll()
                                        .Where(x => x.Protocol.Id == parentProtocol.Id)
                                        .Select(x => x.ArticleLaw.Name )
                                        .AsEnumerable()
                                        .AggregateWithSeparator(x => x, ", ");
            }

            var codesJurPerson = new[] { "0", "2", "4", "8", "9", "11", "15", "18" };
            var codesOfficialPerson = new[] { "1", "3", "5", "10", "12", "13", "16", "19" };
            var codesPhysPerson = new[] { "6", "7", "14" };

            if (Document.Executant != null)
            {
                if (codesJurPerson.Contains(Document.Executant.Code))
                {
                    this.ReportParams["ТипЛица"] = "юридического";
                    this.ReportParams["Кого"] =
                        string.Format(
                            "юридического лица - {0} (далее - {1}), юридический адрес: {2}, фактическое местонахождение: {3}, ОГРН: {4}, ИНН/КПП: {5}/{6}, {7}",
                            this.ReportParams["Контрагент"],
                            this.ReportParams["КонтрагентСокр"],
                            this.ReportParams["ЮрАдрес"],
                            this.ReportParams["АдресФакт"],
                            this.ReportParams["ОГРН"],
                            this.ReportParams["ИНН"],
                            this.ReportParams["КПП"],
                            this.ReportParams["РуководительКонтрагент"]);
                }
                else if (codesOfficialPerson.Contains(Document.Executant.Code))
                {
                    this.ReportParams["ТипЛица"] = "должностного";
                    this.ReportParams["Кого"] =
                        string.Format(
                            "должностного лица - {0} {1} (далее - {2}) {3}, {4}, место жительства: {5}",
                            this.ReportParams["Должность"],
                            this.ReportParams["Контрагент"],
                            this.ReportParams["КонтрагентСокр"],
                            this.ReportParams["ФизЛицо"],
                            this.ReportParams["ДатаМестоРожд"],
                            this.ReportParams["АдресПрож"]);
                }
                else if (codesPhysPerson.Contains(Document.Executant.Code))
                {
                    this.ReportParams["ТипЛица"] = "физического";
                    this.ReportParams["Кого"] =
                        string.Format(
                            "{0}, {1}, место жительства: {2}",
                            this.ReportParams["ФизЛицо"],
                            this.ReportParams["ДатаМестоРожд"],
                            this.ReportParams["АдресПрож"]);
                }
            }

            if (Document.Sanction != null)
            {
                this.ReportParams["ВидСанкции"] = Document.Sanction.Code;
            }
        }

        public DocumentGji GetParentDocument(DocumentGji document, TypeDocumentGji type)
        {
            var docChildrenDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            try
            {
                var result = document;

                if (document.TypeDocumentGji != type)
                {
                    var docs = docChildrenDomain.GetAll()
                                        .Where(x => x.Children.Id == document.Id)
                                        .Select(x => x.Parent)
                                        .ToList();

                    foreach (var doc in docs)
                    {
                        result = GetParentDocument(doc, type);
                    }
                }

                if (result != null)
                {
                    return result.TypeDocumentGji == type ? result : null;
                }

                return null;
            }
            finally 
            {
                Container.Release(docChildrenDomain);
            }
            
        }
    }
}