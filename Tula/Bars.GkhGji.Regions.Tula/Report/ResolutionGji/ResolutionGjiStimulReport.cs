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
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tula.Entities;
    using GkhGji.Report;
    using Stimulsoft.Report;

    /// <summary>
    /// Печать отчета ГЖИ (Постановление) через Стимул.
    /// </summary>
    public class ResolutionGjiStimulReport : GjiBaseStimulReport
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public ResolutionGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.TulaResolution))
        {
        }

        #region Properties

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get
            {
                return "Resolution";
            }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get
            {
                return "Resolution";
            }
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
        }

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name
        {
            get
            {
                return "Постановление";
            }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get
            {
                return "Постановление";
            }
        }

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// ИД документа.
        /// </summary>
        private long DocumentId { get; set; }

        /// <summary>
        /// Постановление.
        /// </summary>
        private Resolution Document { get; set; }

        #endregion

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
                                   Code = "TulaResolution_1",
                                   Name = "TulaResolution_1",
                                   Description =
                                       "Постановление предупреждение (Вид санкции: 'Предупреждение''\''устное замечание')",
                                   Template = Properties.Resources.TulaResolution
                               },
                           new TemplateInfo
                               {
                                   Code = "TulaResolution_2",
                                   Name = "TulaResolution_2",
                                   Description =
                                       "Постановление о назначении (Вид санкции: 'Административный штраф')",
                                   Template = Properties.Resources.TulaResolution
                               },
                           new TemplateInfo
                               {
                                   Code = "TulaResolution_3",
                                   Name = "TulaResolution_3",
                                   Description =
                                       "Постановление о прекращении (Вид санкции: 'Прекращено' и определения имеются)",
                                   Template = Properties.Resources.TulaResolution
                               },
                           new TemplateInfo
                               {
                                   Code = "TulaResolution_4",
                                   Name = "TulaResolution_4",
                                   Description =
                                       "Постановление о прекращении без определений (Вид санкции: 'Прекращено' и определения отсутсвуют)",
                                   Template = Properties.Resources.TulaResolution
                               }
                       };
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

        #region DomainServices

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DispInspectorsDomain { get; set; }

        public IDomainService<ContragentContact> CtrContactDomain { get; set; }

        public IDomainService<InspectionGjiRealityObject> InspRoDomain { get; set; }

        public IDomainService<InspectionGji> InspectionGjiDomain { get; set; }

        public IDomainService<DocumentGJIPhysPersonInfo> PhysInfoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocumentDomain { get; set; }

        public IDomainService<ProtocolArticleLaw> ProtocolArtLawDomain { get; set; }

        public IDomainService<ResolutionLongDescription> ResolutionLongDescriptionDomain { get; set; } 

        #endregion

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues">Пользовательские параметры</param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            this.Document = this.ResolutionDomain.GetAll().FirstOrDefault(x => x.Id == this.DocumentId);

            if (this.Document == null)
            {
                throw new ReportProviderException("Не удалось получить постановление");
            }
        }

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams">Параметры отчета</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            const string DateFormat = "«dd» MMMM yyyy г.";

            FillCommonFields(Document);
            this.Report["Дата"] = this.Document.DocumentDate.HasValue
                                      ? this.Document.DocumentDate.Value.ToString(DateFormat)
                                      : string.Empty;
            this.Report["Номер"] = this.Document.DocumentNumber;
            this.Report["Штраф"] = this.Document.PenaltyAmount.HasValue
                                       ? this.Document.PenaltyAmount.Value.RoundDecimal(2)
                                       : 0m;
            this.Report["ФизЛицо"] = this.Document.PhysicalPerson;
            this.Report["ОснованиеПроверки"] = this.Document.Inspection.TypeBase;

            var whosDecided = string.Empty;
            switch (this.Document.TypeInitiativeOrg)
            {
                case TypeInitiativeOrgGji.Court:
                    whosDecided = "0";
                    break;
                case TypeInitiativeOrgGji.HousingInspection:
                    whosDecided = "1";
                    break;
            }

            this.Report["КемВынесено"] = whosDecided;
            this.Report["НомерУчастка"] = this.Document.SectorNumber;

            if (this.Document.Municipality != null)
            {
                this.Report["МестонахождениеУчастка"] = this.Document.Municipality.Name;
            }

            var administrativeOffence = string.Empty;
            
            var longDescription = this.ResolutionLongDescriptionDomain.GetAll()
                .Where(x => x.Resolution.Id == this.DocumentId)
                .Select(x => x.Description)
                .FirstOrDefault();

            if (longDescription != null)
            {
                administrativeOffence = System.Text.Encoding.UTF8.GetString(longDescription);
            }

            this.Report["СоставАП"] = !string.IsNullOrWhiteSpace(administrativeOffence)
                                          ? administrativeOffence
                                          : this.Document.Description;

            var firstPhysPersonInfo = this.PhysInfoDomain.GetAll()
                    .Where(x => x.Document.Id == this.Document.Id)
                    .Select(x => new { x.PhysPersonAddress, x.PhysPersonPosition, x.PhysPersonBirthdayAndPlace })
                    .FirstOrDefault();

            this.Report["Должность"] = string.Empty;
            this.Report["ДатаМестоРожд"] = string.Empty;
            this.Report["АдресПрож"] = string.Empty;

            if (firstPhysPersonInfo != null)
            {
                this.Report["Должность"] = firstPhysPersonInfo.PhysPersonPosition;
                this.Report["ДатаМестоРожд"] = firstPhysPersonInfo.PhysPersonBirthdayAndPlace;
                this.Report["АдресПрож"] = firstPhysPersonInfo.PhysPersonAddress;
            }

            if (this.Document.Official != null)
            {
                this.Report["КодДлВынесшегоПостановление"] = this.Document.Official.Position;
                this.Report["ФИОДлВынесшегоПостановление"] = this.Document.Official.Fio;
                this.Report["ФИОДлВынесшегоПостановлениеСокр"] = this.Document.Official.ShortFio;
            }

            this.Report["Контрагент"] = string.Empty;
            this.Report["КонтрагентСокр"] = string.Empty;
            this.Report["ЮрАдрес"] = string.Empty;
            this.Report["АдресФакт"] = string.Empty;
            this.Report["ОГРН"] = string.Empty;
            this.Report["ИНН"] = string.Empty;
            this.Report["КПП"] = string.Empty;
            this.Report["РуководительКонтрагент"] = string.Empty;

            if (this.Document.Contragent != null)
            {
                this.Report["Контрагент"] = this.Document.Contragent.Name;
                this.Report["КонтрагентСокр"] = this.Document.Contragent.ShortName;
                this.Report["ЮрАдрес"] = this.Document.Contragent.FiasJuridicalAddress != null
                                             ? this.Document.Contragent.FiasJuridicalAddress.AddressName
                                             : string.Empty;
                this.Report["АдресФакт"] = this.Document.Contragent.FiasFactAddress != null
                                               ? this.Document.Contragent.FiasFactAddress.AddressName
                                               : string.Empty;
                this.Report["ОГРН"] = this.Document.Contragent.Ogrn;
                this.Report["ИНН"] = this.Document.Contragent.Inn;
                this.Report["КПП"] = this.Document.Contragent.Kpp;

                var headPerson = this.CtrContactDomain.GetAll()
                    .FirstOrDefault(x => x.Contragent.Id == this.Document.Contragent.Id && x.Position.Code == "1");

                if (headPerson != null)
                {
                    this.Report["РуководительКонтрагент"] = string.Format("{0} {1}", headPerson.Position.Name, headPerson.FullName);
                }
            }

            // получаем родительский протокол  
            var parentProtocol = this.GetParentDocument(this.Document, TypeDocumentGji.Protocol);

            if (parentProtocol != null)
            {
                this.Report["ДатаПротокол"] = parentProtocol.DocumentDate.HasValue
                                             ? parentProtocol.DocumentDate.Value.ToString(DateFormat)
                                             : string.Empty;

                this.Report["НомерПротокол"] = parentProtocol.DocumentNumber;
                this.Report["СтатьяЗакона"] = this.ProtocolArtLawDomain.GetAll()
                        .Where(x => x.Protocol.Id == parentProtocol.Id)
                        .Select(x => x.ArticleLaw.Name)
                        .AsEnumerable()
                        .AggregateWithSeparator(x => x, ", ");
            }

            var codesJurPerson = new[] { "0", "2", "4", "8", "9", "11", "15", "18" };
            var codesOfficialPerson = new[] { "1", "3", "5", "10", "12", "13", "16", "19" };
            var codesPhysPerson = new[] { "6", "7", "14" };

            if (this.Document.Executant != null)
            {
                if (codesJurPerson.Contains(this.Document.Executant.Code))
                {
                    this.Report["ТипЛица"] = "юридического";
                    this.Report["Кого"] =
                        string.Format(
                            "юридического лица - {0} (далее - {1}), юридический адрес: {2}, фактическое местонахождение: {3}, ОГРН: {4}, ИНН/КПП: {5}/{6}, {7}",
                            this.Report["Контрагент"],
                            this.Report["КонтрагентСокр"],
                            this.Report["ЮрАдрес"],
                            this.Report["АдресФакт"],
                            this.Report["ОГРН"],
                            this.Report["ИНН"],
                            this.Report["КПП"],
                            this.Report["РуководительКонтрагент"]);
                }
                else if (codesOfficialPerson.Contains(this.Document.Executant.Code))
                {
                    this.Report["ТипЛица"] = "должностного";
                    this.Report["Кого"] =
                        string.Format(
                            "должностного лица - {0} {1} (далее - {2}) {3}, {4}, место жительства: {5}",
                            this.Report["Должность"],
                            this.Report["Контрагент"],
                            this.Report["КонтрагентСокр"],
                            this.Report["ФизЛицо"],
                            this.Report["ДатаМестоРожд"],
                            this.Report["АдресПрож"]);
                }
                else if (codesPhysPerson.Contains(this.Document.Executant.Code))
                {
                    this.Report["ТипЛица"] = "физического";
                    this.Report["Кого"] =
                        string.Format(
                            "{0}, {1}, место жительства: {2}",
                            this.Report["ФизЛицо"],
                            this.Report["ДатаМестоРожд"],
                            this.Report["АдресПрож"]);
                }
            }

            if (this.Document.Sanction != null)
            {
                this.Report["ВидСанкции"] = this.Document.Sanction.Code;
            }
        }

        /// <summary>
        /// Получить родительский документ.
        /// </summary>
        /// <param name="document">
        /// Документ ГЖИ.
        /// </param>
        /// <param name="type">
        /// Тип документа.
        /// </param>
        /// <returns>
        /// Родительский документ ГЖИ.
        /// </returns>
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
                        result = this.GetParentDocument(doc, type);
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

        /// <summary>
        /// Установить значение кода шаблона.
        /// </summary>
        private void GetCodeTemplate()
        {
            var resolDefDomain = Container.Resolve<IDomainService<ResolutionDefinition>>();

            try
            {
                this.CodeTemplate = "TulaResolution_1";

                if (this.Document.Sanction != null)
                {
                    switch (this.Document.Sanction.Name)
                    {
                        case "Предупреждение":
                            this.CodeTemplate = "TulaResolution_1";
                            break;

                        case "Административный штраф":
                            this.CodeTemplate = "TulaResolution_2";
                            break;

                        case "Прекращено":
                            {
                                this.CodeTemplate = "TulaResolution_3";

                                // Если у постановления с видом санкции прекращено отсутсвуют определения то выводим шаблон для отсутсвующих определений
                                if (!resolDefDomain.GetAll().Any(x => x.Resolution.Id == this.Document.Id))
                                {
                                    this.CodeTemplate = "TulaResolution_4";
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
    }
}