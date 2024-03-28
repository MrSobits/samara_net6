namespace Bars.GkhGji.Regions.Zabaykalye.Report
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;
    using GkhGji.Report;
    using Slepov.Russian.Morpher;

    /// <summary> Уведомление о проверке из приказа </summary>
    public class ResolutionGjiDefinitionStimulReport : GjiBaseStimulReport
    {
        #region .ctor

        public ResolutionGjiDefinitionStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.ZabaykalyeResolution_Definition))
        {
        }

        #endregion .ctor

        #region Injections
        public IDomainService<ResolutionDefinition> resolDefinitionDomain { get; set; }
        public IDomainService<DocumentGjiInspector> docInspectorDomain { get; set; }
        public IDomainService<ProtocolArticleLaw> protArtLawDomain { get; set; }
        public IDomainService<DocumentGjiChildren> docChildrenDomain { get; set; }
        public IDomainService<DocumentGJIPhysPersonInfo> physPersonInfoDomain { get; set; }
        public IDomainService<ContragentContact> contragentContactDomain { get; set; }
        #endregion

        #region Fields
        private long DefinitionId { get; set; }

        private ResolutionDefinition definition;
        public ResolutionDefinition Definition
        {
            get { return definition; }
            set { definition = value; }
        }
        #endregion Fields
        #region Properties

        public override string Id
        {
            get { return "ResolutionDefinition"; }
        }

        public override string CodeForm
        {
            get { return "ResolutionDefinition"; }
        }

        public override string Name
        {
            get { return "Определение постановления"; }
        }

        public override string Description
        {
            get { return "Определение постановления"; }
        }

        protected override string CodeTemplate { get; set; }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "ZabaykalyeResolution_Definition_1",
                    Code = "ZabaykalyeResolution_Definition_1",
                    Description =
                        "о назначении времени и места рассмотрения жалобы на постановление по делу об административном правонарушении",
                    Template = Properties.Resources.ZabaykalyeResolution_Definition
                },
                new TemplateInfo
                {
                    Name = "ZabaykalyeResolution_Definition_2",
                    Code = "ZabaykalyeResolution_Definition_2",
                    Description =
                        "о предоставлении отсрочки исполнения постановления по делу об административном правонарушении",
                    Template = Properties.Resources.ZabaykalyeResolution_Definition
                },
                new TemplateInfo
                {
                    Name = "ZabaykalyeResolution_Definition_3",
                    Code = "ZabaykalyeResolution_Definition_3",
                    Description =
                        "о предоставлении рассрочки исполнения постановления по делу об административном правонарушении",
                    Template = Properties.Resources.ZabaykalyeResolution_Definition
                },
                new TemplateInfo
                {
                    Name = "ZabaykalyeResolution_Definition_4",
                    Code = "ZabaykalyeResolution_Definition_4",
                    Description =
                        "об исправлении описок, опечаток и арифметических ошибок",
                    Template = Properties.Resources.ZabaykalyeResolution_Definition
                }
            };
        }

        #endregion Properties

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DefinitionId = userParamsValues.GetValue<object>("DefinitionId").ToLong();

            Definition = resolDefinitionDomain.Load(DefinitionId);

            if (Definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение постановления");
            }
        }

        private void GetCodeTemplate()
        {
            switch (definition.TypeDefinition)
            {
                case TypeDefinitionResolution.AppointmentPlaceTime:
                    CodeTemplate = "ZabaykalyeResolution_Definition_1";
                    break;
                case TypeDefinitionResolution.Deferment:
                    CodeTemplate = "ZabaykalyeResolution_Definition_2";
                    break;
                case TypeDefinitionResolution.Installment:
                    CodeTemplate = "ZabaykalyeResolution_Definition_3";
                    break;
                case TypeDefinitionResolution.CorrectionError:
                    CodeTemplate = "ZabaykalyeResolution_Definition_4";
                    break;
            }
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            var resolution = definition.Resolution;
            FillCommonFields(resolution);

            var physPerson = physPersonInfoDomain.GetAll().FirstOrDefault(x => x.Document.Id == resolution.Id);
            
            this.ReportParams["НомерОпределения"] = definition.DocumentNum;
            this.ReportParams["ДатаОпределения"] = definition.DocumentDate.HasValue
                                                    ? definition.DocumentDate.Value.ToShortDateString()
                                                    : string.Empty;

            if (definition.IssuedDefinition != null)
            {
                this.ReportParams["Руководитель"] = definition.IssuedDefinition.Fio;
                this.ReportParams["ДолжностьРуководителя"] = definition.IssuedDefinition.Position;
                this.ReportParams["РуководительФИОСокр"] = definition.IssuedDefinition.ShortFio;
            }

            this.ReportParams["ДатаПостановления"] = resolution.DocumentDate.HasValue
                                                        ? resolution.DocumentDate.Value.ToShortDateString()
                                                        : string.Empty;

            this.ReportParams["НомерПостановления"] = resolution.DocumentNumber;

            if (resolution.PenaltyAmount.HasValue)
            {
                this.ReportParams["Штраф"] = resolution.PenaltyAmount.Value.RoundDecimal(2).ToString();
            }

            if (resolution.Official != null)
            {
                this.ReportParams["ВынесшийПостановление"] = resolution.Official.Fio;
            }

            var protocol = GetParentDocument(resolution, TypeDocumentGji.Protocol);
            if (protocol != null)
            {
                var articles = protArtLawDomain.GetAll()
                    .Where(x => x.Protocol.Id == protocol.Id)
                    .Select(x => new { x.ArticleLaw.Name, x.Description, ArtDescription = x.ArticleLaw.Description })
                    .ToList();

                if (articles.Any())
                {
                    var artStr = articles.Aggregate(string.Empty, (x, y) => x + (y.Name + ", "));

                    this.ReportParams["СтатьяЗакона"] = !string.IsNullOrEmpty(artStr)
                                                ? artStr.TrimEnd(new[] { ',', ' ' })
                                                : string.Empty;

                    var descriptions = articles.Select(x => !string.IsNullOrEmpty(x.Description) ? x.Description : x.ArtDescription).Distinct().Aggregate(string.Empty, (x, y) => x + (y + ", "));

                    this.ReportParams["ОписаниеСтатьи"] = !string.IsNullOrEmpty(descriptions)
                                                ? descriptions.TrimEnd(new[] { ',', ' ' })
                                                : string.Empty;
                }    
            }
            
            if (resolution.Executant != null)
            {
                this.ReportParams["ТипИсполнителя"] = resolution.Executant.Code;

                /* пока не пригодились
                var listTypeContragent = new List<string> { "0", "9", "11", "8", "15", "18", "4", "17", "18"};
                var listTypeContrPhysicalPerson = new List<string> { "1", "10", "12", "13", "16", "19", "5", "19" };
                var listTypePhysicalPerson = new List<string> { "6", "7", "14", "20", "2", "3" };
                */

                var contrName = string.Empty;
                var shortName = string.Empty;
                var inn = string.Empty;
                var kpp = string.Empty;
                var jurAddress = string.Empty;
                var physPersonFio = resolution.PhysicalPerson;
                var physPersonAddress = string.Empty;
                var physPersonPosition = string.Empty;
                var physPersonDoc = string.Empty;

                if (physPerson != null)
                {
                    physPersonAddress = physPerson.PhysPersonAddress;
                    physPersonPosition = physPerson.PhysPersonPosition;
                    physPersonDoc = physPerson.PhysPersonDocument;
                }

                if (!string.IsNullOrEmpty(physPersonFio))
                {
                    var fio = склонятель.Проанализировать(physPersonFio);
                    this.ReportParams["ФизЛицоФИО"] = physPersonFio;
                    this.ReportParams["ФизЛицоФИОРП"] = fio.Родительный;
                    this.ReportParams["ФизЛицоФИОДП"] = fio.Дательный;    
                }
                
                if (!string.IsNullOrEmpty(physPersonPosition))
                {
                    var position = склонятель.Проанализировать(physPersonPosition);
                    this.ReportParams["ФизЛицоДолжность"] = physPersonPosition;
                    this.ReportParams["ФизЛицоДолжностьРП"] = position.Родительный;
                    this.ReportParams["ФизЛицоДолжностьДП"] = position.Дательный;     
                }
               
                this.ReportParams["ФизЛицоАдрес"] = physPersonAddress;
                this.ReportParams["ФизЛицоДУЛ"] = physPersonDoc;

                var contragent = resolution.Contragent;

                if (contragent != null)
                {
                    contrName = contragent.Return(x => x.Name);
                    shortName = contragent.Return(x => x.ShortName);
                    inn = contragent.Return(x => x.Inn);
                    kpp = contragent.Return(x => x.Kpp);
                    jurAddress = contragent.FiasJuridicalAddress != null ?
                        contragent.FiasJuridicalAddress.AddressName : contragent.JuridicalAddress;

                    this.ReportParams["Контрагент"] = contrName;
                    this.ReportParams["КонтрагентСокр"] = shortName;
                    this.ReportParams["КонтрагентИНН"] = inn;
                    this.ReportParams["КонтрагентКПП"] = kpp;
                    this.ReportParams["КонтрагентЮрАдрес"] = jurAddress;

                    var contact = contragentContactDomain.GetAll()
                                               .Where(x => x.Contragent.Id == contragent.Id && x.Position != null)
                                               .FirstOrDefault();

                    if (contact != null && contact.Position != null && !string.IsNullOrEmpty(contact.Position.Name))
                    {
                        var contactPosition = склонятель.Проанализировать(contact.Position.Name);
                        this.ReportParams["КонтрагентКонтактДолжность"] = contact.Position.Name;
                        this.ReportParams["КонтрагентКонтактДолжностьРП"] = !string.IsNullOrEmpty(contact.Position.NameGenitive) ? contact.Position.NameGenitive : contactPosition.Родительный;
                        this.ReportParams["КонтрагентКонтактДолжностьДП"] = !string.IsNullOrEmpty(contact.Position.NameDative) ? contact.Position.NameDative : contactPosition.Дательный;
                    }
                }
            }
        }

        public DocumentGji GetParentDocument(DocumentGji document, TypeDocumentGji type)
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
    }
}
