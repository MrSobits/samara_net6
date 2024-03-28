namespace Bars.GkhGji.Regions.Tula.Report
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tula.Entities;
    using GkhGji.Report;
    using Slepov.Russian.Morpher;
    using Stimulsoft.Report;

    /// <summary> Уведомление о проверке из приказа </summary>
    public class ResolutionGjiDefinitionStimulReport : GjiBaseStimulReport
    {
        #region .ctor

        public ResolutionGjiDefinitionStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.TulaResolution_Definition))
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
                    Name = "TulaResolution_Definition_1",
                    Code = "TulaResolution_Definition_1",
                    Description =
                        "о назначении времени и места рассмотрения жалобы на постановление по делу об административном правонарушении",
                    Template = Properties.Resources.TulaResolution_Definition
                },
                new TemplateInfo
                {
                    Name = "TulaResolution_Definition_2",
                    Code = "TulaResolution_Definition_2",
                    Description =
                        "о предоставлении отсрочки исполнения постановления по делу об административном правонарушении",
                    Template = Properties.Resources.TulaResolution_Definition
                },
                new TemplateInfo
                {
                    Name = "TulaResolution_Definition_3",
                    Code = "TulaResolution_Definition_3",
                    Description =
                        "о предоставлении рассрочки исполнения постановления по делу об административном правонарушении",
                    Template = Properties.Resources.TulaResolution_Definition
                },
                new TemplateInfo
                {
                    Name = "TulaResolution_Definition_4",
                    Code = "TulaResolution_Definition_4",
                    Description =
                        "об исправлении описок, опечаток и арифметических ошибок",
                    Template = Properties.Resources.TulaResolution_Definition
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
                    CodeTemplate = "TulaResolution_Definition_1";
                    break;
                case TypeDefinitionResolution.Deferment:
                    CodeTemplate = "TulaResolution_Definition_2";
                    break;
                case TypeDefinitionResolution.Installment:
                    CodeTemplate = "TulaResolution_Definition_3";
                    break;
                case TypeDefinitionResolution.CorrectionError:
                    CodeTemplate = "TulaResolution_Definition_4";
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

            var physPerson = physPersonInfoDomain.GetAll().FirstOrDefault(x => x.Document.Id == resolution.Id);

            FillCommonFields(resolution);
            Report["НомерОпределения"] = definition.DocumentNum;
            Report["ДатаОпределения"] = definition.DocumentDate.HasValue
                                                    ? definition.DocumentDate.Value.ToShortDateString()
                                                    : string.Empty;

            if (definition.IssuedDefinition != null)
            {
                Report["Руководитель"] = definition.IssuedDefinition.Fio;
                Report["ДолжностьРуководителя"] = definition.IssuedDefinition.Position;
                Report["РуководительФИОСокр"] = definition.IssuedDefinition.ShortFio;
            }

            Report["ДатаПостановления"] = resolution.DocumentDate.HasValue
                                                        ? resolution.DocumentDate.Value.ToShortDateString()
                                                        : string.Empty;

            Report["НомерПостановления"] = resolution.DocumentNumber;

            if (resolution.PenaltyAmount.HasValue)
            {
                Report["Штраф"] = resolution.PenaltyAmount.Value.RoundDecimal(2);
            }

            if (resolution.Official != null)
            {
                Report["ВынесшийПостановление"] = resolution.Official.Fio;
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

                    Report["СтатьяЗакона"] = !string.IsNullOrEmpty(artStr)
                                                ? artStr.TrimEnd(new[] { ',', ' ' })
                                                : string.Empty;

                    var descriptions = articles.Select(x => !string.IsNullOrEmpty(x.Description) ? x.Description : x.ArtDescription).Distinct().Aggregate(string.Empty, (x, y) => x + (y + ", "));

                    Report["ОписаниеСтатьи"] = !string.IsNullOrEmpty(descriptions)
                                                ? descriptions.TrimEnd(new[] { ',', ' ' })
                                                : string.Empty;
                }    
            }
            
            if (resolution.Executant != null)
            {
                Report["ТипИсполнителя"] = resolution.Executant.Code;

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
                    Report["ФизЛицоФИО"] = physPersonFio;
                    Report["ФизЛицоФИОРП"] = fio.Родительный;
                    Report["ФизЛицоФИОДП"] = fio.Дательный;    
                }
                
                if (!string.IsNullOrEmpty(physPersonPosition))
                {
                    var position = склонятель.Проанализировать(physPersonPosition);
                    Report["ФизЛицоДолжность"] = physPersonPosition;
                    Report["ФизЛицоДолжностьРП"] = position.Родительный;
                    Report["ФизЛицоДолжностьДП"] = position.Дательный;     
                }
               
                Report["ФизЛицоАдрес"] = physPersonAddress;
                Report["ФизЛицоДУЛ"] = physPersonDoc;

                var contragent = resolution.Contragent;

                if (contragent != null)
                {
                    contrName = contragent.Return(x => x.Name);
                    shortName = contragent.Return(x => x.ShortName);
                    inn = contragent.Return(x => x.Inn);
                    kpp = contragent.Return(x => x.Kpp);
                    jurAddress = contragent.FiasJuridicalAddress != null ?
                        contragent.FiasJuridicalAddress.AddressName : contragent.JuridicalAddress;

                    Report["Контрагент"] = contrName;
                    Report["КонтрагентСокр"] = shortName;
                    Report["КонтрагентИНН"] = inn;
                    Report["КонтрагентКПП"] = kpp;
                    Report["КонтрагентЮрАдрес"] = jurAddress;

                    var contact = contragentContactDomain.GetAll()
                                               .Where(x => x.Contragent.Id == contragent.Id && x.Position != null)
                                               .FirstOrDefault();

                    if (contact != null && contact.Position != null && !string.IsNullOrEmpty(contact.Position.Name))
                    {
                        var contactPosition = склонятель.Проанализировать(contact.Position.Name);
                        Report["КонтрагентКонтактДолжность"] = contact.Position.Name;
                        Report["КонтрагентКонтактДолжностьРП"] = !string.IsNullOrEmpty(contact.Position.NameGenitive) ? contact.Position.NameGenitive : contactPosition.Родительный;
                        Report["КонтрагентКонтактДолжностьДП"] = !string.IsNullOrEmpty(contact.Position.NameDative) ? contact.Position.NameDative : contactPosition.Дательный;
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
