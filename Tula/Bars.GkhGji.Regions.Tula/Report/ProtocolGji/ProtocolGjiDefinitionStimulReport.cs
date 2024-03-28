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

    /// <summary> 
    /// Уведомление о проверке из приказа 
    /// </summary>
    public class ProtocolGjiDefinitionStimulReport : GjiBaseStimulReport
    {
        /// <summary>
        /// Определение протокола ГЖИ (Для Тулы).
        /// </summary>
        private TulaProtocolDefinition definition;

        #region .ctor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ProtocolGjiDefinitionStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.TulaProtocol_Definition))
        {
        }

        #endregion .ctor

        #region Properties

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get
            {
                return "ProtocolDefinition";
            }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get
            {
                return "ProtocolDefinition";
            }
        }

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name
        {
            get
            {
                return "Определение протокола";
            }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get
            {
                return "Определение протокола";
            }
        }

        /// <summary>
        /// Формат печатной формы
        /// </summary>
        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
        }

        /// <summary>
        /// Получить или установить определение протокола.
        /// </summary>
        public TulaProtocolDefinition Definition
        {
            get { return this.definition; }
            set { this.definition = value; }
        }

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// ИД документа.
        /// </summary>
        private long DefinitionId { get; set; }

        /// <summary>
        /// Получить список шаблонов
        /// </summary>
        /// <returns>Список шаблонов</returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "TulaProtocol_Definition_1",
                    Code = "TulaProtocol_Definition_1",
                    Description =
                        "об отложении рассмотрения дела об административном правонарушении",
                    Template = Properties.Resources.TulaProtocol_Definition
                },
                new TemplateInfo
                {
                    Name = "TulaProtocol_Definition_2",
                    Code = "TulaProtocol_Definition_2",
                    Description = "о назначении рассмотрения дела об административном правонарушении",
                    Template = Properties.Resources.TulaProtocol_Definition
                },
                new TemplateInfo
                {
                    Name = "TulaProtocol_Definition_3",
                    Code = "TulaProtocol_Definition_3",
                    Description = "о продлении срока рассмотрения дела об административном правонарушении",
                    Template = Properties.Resources.TulaProtocol_Definition
                },
                new TemplateInfo
                {
                    Name = "TulaProtocol_Definition_4",
                    Code = "TulaProtocol_Definition_4",
                    Description = "о передаче дела на рассмотрение по подведомственности",
                    Template = Properties.Resources.TulaProtocol_Definition
                },
                new TemplateInfo
                {
                    Name = "TulaProtocol_Definition_5",
                    Code = "TulaProtocol_Definition_5",
                    Description = "о возвращении протокола об административном правонарушении и других материалов дела должностному лицу",
                    Template = Properties.Resources.TulaProtocol_Definition
                },
                new TemplateInfo
                {
                    Name = "TulaProtocol_Definition_6",
                    Code = "TulaProtocol_Definition_6",
                    Description = "об отклонении ходатайства",
                    Template = Properties.Resources.TulaProtocol_Definition
                }
            };
        }

        #endregion Properties

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues">Значения пользовательских параметров</param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DefinitionId = userParamsValues.GetValue<object>("DefinitionId").ToLong();

            this.Definition = this.protocolDefinitionDomain.Load(this.DefinitionId);

            if (this.Definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение протокола");
            }
        }

        /// <summary>
        /// Получить поток шаблона отчета (файла)
        /// </summary>
        /// <returns>Поток</returns>
        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        #region Injections
        public IDomainService<TulaProtocolDefinition> protocolDefinitionDomain { get; set; }
        public IDomainService<DocumentGjiInspector> docInspectorDomain { get; set; }
        public IDomainService<ProtocolArticleLaw> protArtLawDomain { get; set; }
        public IDomainService<DocumentGJIPhysPersonInfo> physPersonInfoDomain { get; set; }
        public IDomainService<ProtocolViolation> protViolDomain { get; set; }
        public IDomainService<ContragentContact> contragentContactDomain { get; set; }
        #endregion

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams">Параметры отчета</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            var protocol = this.definition.Protocol;

            var physPerson = this.physPersonInfoDomain.GetAll().FirstOrDefault(x => x.Document.Id == protocol.Id);

            FillCommonFields(protocol);
            this.Report["НомерОпределения"] = this.definition.DocumentNum;
            this.Report["ДатаОпределения"] = this.definition.DocumentDate.HasValue
                                                 ? this.definition.DocumentDate.Value.ToShortDateString()
                                                 : string.Empty;

            if (this.definition.IssuedDefinition != null)
            {
                this.Report["Руководитель"] = this.definition.IssuedDefinition.Fio;
                this.Report["ДолжностьРуководителя"] = this.definition.IssuedDefinition.Position;
                this.Report["РуководительФИОСокр"] = this.definition.IssuedDefinition.ShortFio;
            }

            this.Report["ДатаПротокола"] = protocol.DocumentDate.HasValue
                                               ? protocol.DocumentDate.Value.ToShortDateString()
                                               : string.Empty;
            this.Report["НомерПротокола"] = protocol.DocumentNumber;
            this.Report["ДатаИсполнения"] = this.definition.ExecutionDate.HasValue
                                                ? this.definition.ExecutionDate.Value.ToShortDateString()
                                                : string.Empty;
            this.Report["ВремяC"] = this.definition.TimeStart.HasValue
                                        ? this.definition.TimeStart.Value.ToShortTimeString()
                                        : string.Empty;
            this.Report["ВремяПо"] = this.definition.TimeEnd.HasValue
                                         ? this.definition.TimeEnd.Value.ToShortTimeString()
                                         : string.Empty;

            var inspector = this.docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == protocol.Id)
                    .OrderBy(x => x.Id)
                    .Select(x => new { x.Inspector.Fio, x.Inspector.FioGenitive, x.Inspector.FioDative })
                    .FirstOrDefault();

            if (inspector != null)
            {
                this.Report["Инспектор"] = inspector.Fio;

                if (!string.IsNullOrEmpty(inspector.Fio))
                {
                    var fioInspectorDp = склонятель.Проанализировать(inspector.Fio);
                    this.Report["ИнспекторРП"] = string.IsNullOrEmpty(inspector.FioGenitive)
                                                     ? fioInspectorDp.Родительный
                                                     : inspector.FioGenitive;
                    this.Report["ИнспекторДП"] = string.IsNullOrEmpty(inspector.FioDative)
                                                     ? fioInspectorDp.Дательный
                                                     : inspector.FioDative;
                }
            }

            var articles = this.protArtLawDomain.GetAll()
                    .Where(x => x.Protocol.Id == protocol.Id)
                    .Select(x => new { x.ArticleLaw.Name, x.Description, ArtDescription = x.ArticleLaw.Description })
                    .ToList();

            if (articles.Any())
            {
                var artStr = articles.Aggregate(string.Empty, (x, y) => x + (y.Name + ", "));

                this.Report["СтатьяЗакона"] = !string.IsNullOrEmpty(artStr)
                                                  ? artStr.TrimEnd(new[] { ',', ' ' })
                                                  : string.Empty;

                var descriptions = articles.Select(x => !string.IsNullOrEmpty(x.Description) ? x.Description : x.ArtDescription)
                    .Distinct()
                    .Aggregate(string.Empty, (x, y) => x + (y + ", "));

                this.Report["ОписаниеСтатьи"] = !string.IsNullOrEmpty(descriptions)
                                                    ? descriptions.TrimEnd(new[] { ',', ' ' })
                                                    : string.Empty;
            }

            if (protocol.Executant != null)
            {
                this.Report["ТипИсполнителя"] = protocol.Executant.Code;

                /* пока не пригодились
                var listTypeContragent = new List<string> { "0", "9", "11", "8", "15", "18", "4", "17", "18"};
                var listTypeContrPhysicalPerson = new List<string> { "1", "10", "12", "13", "16", "19", "5", "19" };
                var listTypePhysicalPerson = new List<string> { "6", "7", "14", "20", "2", "3" };
                */

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
                    this.Report["ФизЛицоФИО"] = physPersonFio;
                    this.Report["ФизЛицоФИОРП"] = fio.Родительный;
                    this.Report["ФизЛицоФИОДП"] = fio.Дательный;    
                }
                
                if (!string.IsNullOrEmpty(physPersonPosition))
                {
                    var position = склонятель.Проанализировать(physPersonPosition);
                    this.Report["ФизЛицоДолжность"] = physPersonPosition;
                    this.Report["ФизЛицоДолжностьРП"] = position.Родительный;
                    this.Report["ФизЛицоДолжностьДП"] = position.Дательный;     
                }

                this.Report["ФизЛицоАдрес"] = physPersonAddress;
                this.Report["ФизЛицоДУЛ"] = physPersonDoc;

                if (protocol.Contragent != null)
                {
                    var contrName = protocol.Contragent.Return(x => x.Name);
                    var shortName = protocol.Contragent.Return(x => x.ShortName);
                    var inn = protocol.Contragent.Return(x => x.Inn);
                    var kpp = protocol.Contragent.Return(x => x.Kpp);
                    var jurAddress = protocol.Contragent.FiasJuridicalAddress != null
                                     ? protocol.Contragent.FiasJuridicalAddress.AddressName
                                     : protocol.Contragent.JuridicalAddress;

                    this.Report["Контрагент"] = contrName;
                    this.Report["КонтрагентСокр"] = shortName;
                    this.Report["КонтрагентИНН"] = inn;
                    this.Report["КонтрагентКПП"] = kpp;
                    this.Report["КонтрагентЮрАдрес"] = jurAddress;

                    var contact = this.contragentContactDomain.GetAll()
                            .FirstOrDefault(x => x.Contragent.Id == protocol.Contragent.Id && x.Position != null);

                    if (contact != null && contact.Position != null && !string.IsNullOrEmpty(contact.Position.Name))
                    {
                        var contactPosition = склонятель.Проанализировать(contact.Position.Name);
                        this.Report["КонтрагентКонтактДолжность"] = contact.Position.Name;
                        this.Report["КонтрагентКонтактДолжностьРП"] = !string.IsNullOrEmpty(contact.Position.NameGenitive)
                                ? contact.Position.NameGenitive
                                : contactPosition.Родительный;
                        this.Report["КонтрагентКонтактДолжностьДП"] = !string.IsNullOrEmpty(contact.Position.NameDative)
                                                                          ? contact.Position.NameDative
                                                                          : contactPosition.Дательный;
                    }
                }
            }

            var roAddress = this.protViolDomain.GetAll()
                    .Where(x => x.Document.Id == protocol.Id && x.InspectionViolation.RealityObject != null)
                    .Select(x => x.InspectionViolation.RealityObject.Address)
                    .Distinct()
                    .ToList();

            if (roAddress.Count > 0)
            {
                var str = roAddress.Aggregate(string.Empty, (x, y) => x + (y + ", "));
                this.Report["АдресОбъекта"] = !string.IsNullOrEmpty(str)
                                                  ? str.TrimEnd(new[] { ',', ' ' })
                                                  : string.Empty;
            }
        }

        /// <summary>
        /// Установить значение кода шаблона.
        /// </summary>
        private void GetCodeTemplate()
        {
            switch (this.definition.TypeDefinition)
            {
                case TypeDefinitionProtocol.PostponeCase:
                    this.CodeTemplate = "TulaProtocol_Definition_1";
                    break;
                case TypeDefinitionProtocol.TimeAndPlaceHearing:
                    this.CodeTemplate = "TulaProtocol_Definition_2";
                    break;
                case TypeDefinitionProtocol.TermAdministrativeInfraction:
                    this.CodeTemplate = "TulaProtocol_Definition_3";
                    break;
                case TypeDefinitionProtocol.TransferCase:
                    this.CodeTemplate = "TulaProtocol_Definition_4";
                    break;
                case TypeDefinitionProtocol.ReturnProtocol:
                    this.CodeTemplate = "TulaProtocol_Definition_5";
                    break;
                case TypeDefinitionProtocol.RequestDeviation:
                    this.CodeTemplate = "TulaProtocol_Definition_6";
                    break;
            }
        }
    }
}
