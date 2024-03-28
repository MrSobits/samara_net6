namespace Bars.GkhGji.Regions.Saha.Report
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Saha.Entities;
    using GkhGji.Report;
    using Slepov.Russian.Morpher;

    /// <summary> Уведомление о проверке из приказа </summary>
    public class ProtocolGjiDefinitionStimulReport : GjiBaseStimulReport
    {
        #region .ctor

        public ProtocolGjiDefinitionStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.SahaProtocol_Definition))
        {
        }

        #endregion .ctor

        #region Properties

        public override string Id
        {
            get { return "ProtocolDefinition"; }
        }

        public override string CodeForm
        {
            get { return "ProtocolDefinition"; }
        }

        public override string Name
        {
            get { return "Определение протокола"; }
        }

        public override string Description
        {
            get { return "Определение протокола"; }
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
                    Name = "SahaProtocol_Definition_1",
                    Code = "SahaProtocol_Definition_1",
                    Description =
                        "об отложении рассмотрения дела об административном правонарушении",
                    Template = Properties.Resources.SahaProtocol_Definition
                },
                new TemplateInfo
                {
                    Name = "SahaProtocol_Definition_2",
                    Code = "SahaProtocol_Definition_2",
                    Description = "о назначении рассмотрения дела об административном правонарушении",
                    Template = Properties.Resources.SahaProtocol_Definition
                },
                new TemplateInfo
                {
                    Name = "SahaProtocol_Definition_3",
                    Code = "SahaProtocol_Definition_3",
                    Description = "о продлении срока рассмотрения дела об административном правонарушении",
                    Template = Properties.Resources.SahaProtocol_Definition
                },
                new TemplateInfo
                {
                    Name = "SahaProtocol_Definition_4",
                    Code = "SahaProtocol_Definition_4",
                    Description = "о передаче дела на рассмотрение по подведомственности",
                    Template = Properties.Resources.SahaProtocol_Definition
                },
                new TemplateInfo
                {
                    Name = "SahaProtocol_Definition_5",
                    Code = "SahaProtocol_Definition_5",
                    Description = "о возвращении протокола об административном правонарушении и других материалов дела должностному лицу",
                    Template = Properties.Resources.SahaProtocol_Definition
                }
            };
        }

        #endregion Properties

        #region Fields
        private long DefinitionId { get; set; }

        private ProtocolDefinition definition;
        public ProtocolDefinition Definition
        {
            get { return definition; }
            set { definition = value; }
        }
        #endregion Fields

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DefinitionId = userParamsValues.GetValue<object>("DefinitionId").ToLong();

            Definition = protocolDefinitionDomain.Load(DefinitionId);

            if (Definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение протокола");
            }
        }

        private void GetCodeTemplate()
        {
            switch (definition.TypeDefinition)
            {
                case TypeDefinitionProtocol.PostponeCase:
                    CodeTemplate = "SahaProtocol_Definition_1";
                    break;
                case TypeDefinitionProtocol.TimeAndPlaceHearing:
                    CodeTemplate = "SahaProtocol_Definition_2";
                    break;
                case TypeDefinitionProtocol.TermAdministrativeInfraction:
                    CodeTemplate = "SahaProtocol_Definition_3";
                    break;
                case TypeDefinitionProtocol.TransferCase:
                    CodeTemplate = "SahaProtocol_Definition_4";
                    break;
                case TypeDefinitionProtocol.ReturnProtocol:
                    CodeTemplate = "SahaProtocol_Definition_5";
                    break;
            }
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        #region Injections
        public IDomainService<ProtocolDefinition> protocolDefinitionDomain { get; set; }
        public IDomainService<DocumentGjiInspector> docInspectorDomain { get; set; }
        public IDomainService<ProtocolArticleLaw> protArtLawDomain { get; set; }
        public IDomainService<DocumentGJIPhysPersonInfo> physPersonInfoDomain { get; set; }
        public IDomainService<ProtocolViolation> protViolDomain { get; set; }
        public IDomainService<ContragentContact> contragentContactDomain { get; set; }
        #endregion

        public override void PrepareReport(ReportParams reportParams)
        {
            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            var protocol = definition.Protocol;

            var physPerson = physPersonInfoDomain.GetAll().FirstOrDefault(x => x.Document.Id == protocol.Id);

            FillCommonFields(protocol);
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

            this.ReportParams["ДатаПротокола"] = protocol.DocumentDate.HasValue
                                                                   ? protocol.DocumentDate.Value.ToShortDateString()
                                                                   : string.Empty;

            this.ReportParams["НомерПротокола"] = protocol.DocumentNumber;

            var inspector = docInspectorDomain.GetAll()
                         .Where(x => x.DocumentGji.Id == protocol.Id)
                         .OrderBy(x => x.Id)
                         .Select(x => new { x.Inspector.Fio, x.Inspector.FioGenitive, x.Inspector.FioDative })
                         .FirstOrDefault();

            if (inspector != null)
            {
                this.ReportParams["Инспектор"] = inspector.Fio;

                if (!string.IsNullOrEmpty(inspector.Fio))
                {
                    var fioInspectorDp = склонятель.Проанализировать(inspector.Fio);
                    this.ReportParams["ИнспекторРП"] = string.IsNullOrEmpty(inspector.FioGenitive) ? fioInspectorDp.Родительный : inspector.FioGenitive;
                    this.ReportParams["ИнспекторДП"] = string.IsNullOrEmpty(inspector.FioDative) ? fioInspectorDp.Дательный : inspector.FioDative;
                }
            }


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

            if (protocol.Executant != null)
            {
                this.ReportParams["ТипИсполнителя"] = protocol.Executant.Code;

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
                var physPersonFio = protocol.PhysicalPerson;
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

                if (protocol.Contragent != null)
                {
                    contrName = protocol.Contragent.Return(x => x.Name);
                    shortName = protocol.Contragent.Return(x => x.ShortName);
                    inn = protocol.Contragent.Return(x => x.Inn);
                    kpp = protocol.Contragent.Return(x => x.Kpp);
                    jurAddress = protocol.Contragent.FiasJuridicalAddress != null ?
                        protocol.Contragent.FiasJuridicalAddress.AddressName : protocol.Contragent.JuridicalAddress;

                    this.ReportParams["Контрагент"] = contrName;
                    this.ReportParams["КонтрагентСокр"] = shortName;
                    this.ReportParams["КонтрагентИНН"] = inn;
                    this.ReportParams["КонтрагентКПП"] = kpp;
                    this.ReportParams["КонтрагентЮрАдрес"] = jurAddress;

                    var contact = contragentContactDomain.GetAll()
                                               .Where(x => x.Contragent.Id == protocol.Contragent.Id && x.Position != null)
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

            var roAddress =
                protViolDomain.GetAll()
                              .Where(x => x.Document.Id == protocol.Id && x.InspectionViolation.RealityObject != null)
                              .Select(x => x.InspectionViolation.RealityObject.Address)
                              .Distinct()
                              .ToList();

            if (roAddress.Count > 0)
            {
                var str = roAddress.Aggregate(string.Empty, (x, y) => x + (y + ", "));
                this.ReportParams["АдресОбъекта"] = !string.IsNullOrEmpty(str)
                                            ? str.TrimEnd(new[] { ',', ' ' })
                                            : string.Empty;
            }
        }
    }
}
