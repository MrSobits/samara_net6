namespace Bars.GkhGji.Regions.Stavropol.Report.ResolPros
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
	using Bars.B4.IoC;
	using B4.Modules.Reports;
	using Bars.B4.Utils;
	using Bars.Gkh.Entities;
	using Bars.Gkh.Report;
	using Bars.Gkh.Utils;
	using Bars.GkhGji.Entities;
	using Bars.GkhGji.Enums;
	using Bars.GkhGji.Regions.Stavropol.Entities;
	using Bars.GkhGji.Regions.Stavropol.Enums;
	using Bars.GkhGji.Regions.Stavropol.Properties;
	using Bars.GkhGji.Report;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    using ResolProsDefinition = Bars.GkhGji.Regions.Stavropol.Entities.ResolProsDefinition;

    public class ResolProsGjiDefinitionStimulReport : GjiBaseStimulReport
    {
		public ResolProsGjiDefinitionStimulReport()
            : base(new ReportTemplateBinary(Resources.Stavrapol_ResolPros_Definition_1))
        {
        }

        #region Fields

        private long _definitionId;
        private ResolPros _resolPros;
        private ResolProsDefinition _definition;

        private const string StavrapolResolProsDefinition1 = "Stavrapol_ResolPros_Definition_1";
		private const string StavrapolResolProsDefinition2 = "Stavrapol_ResolPros_Definition_2";

        #endregion Fields

        #region Properties

        public override string Id
        {
			get { return "ResolProsDefinition"; }
        }

        public override string CodeForm
        {
			get { return "ResolProsDefinition"; }
        }

        public override string Name
        {
            get { return "Определение постановления прокуратуры"; }
        }

        public override string Description
        {
			get { return "Определение постановления прокуратуры"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        protected override string CodeTemplate { get; set; }

        #endregion Properties

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _definitionId = userParamsValues.GetValue<object>("DefinitionId").ToLong();

            var resolProsDefinitionDomain = Container.ResolveDomain<ResolProsDefinition>();
            using (Container.Using(resolProsDefinitionDomain))
            {
                _definition = resolProsDefinitionDomain.GetAll().FirstOrDefault(x => x.Id == _definitionId);
            }

            if (_definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение");
            }

            _resolPros = _definition.ResolPros;
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = StavrapolResolProsDefinition1,
                    Code = StavrapolResolProsDefinition1,
                    Description =
                        "Определение о назначении места и времени рассмотрения дела об административном правонарушении",
                    Template = Resources.Stavrapol_ResolPros_Definition_1
                },
                
                new TemplateInfo
                {
                    Name = StavrapolResolProsDefinition2,
                    Code = StavrapolResolProsDefinition2,
                    Description =
                        "Определение об отложении рассмотрения дела об административном правонарушении",
                    Template = Resources.Stavrapol_ResolPros_Definition_2
                }
            };
        }

        public override Stream GetTemplate()
        {
            switch (_definition.TypeDefinition)
            {
				case TypeDefinitionResolPros.TimeAndPlaceHearing:
					CodeTemplate = StavrapolResolProsDefinition1;
                    break;
                case TypeDefinitionResolPros.PostponeCase:
					CodeTemplate = StavrapolResolProsDefinition2;
                    break;
            }

            return base.GetTemplate();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            FillCommonInfo();
            switch (_definition.TypeDefinition)
            {
				case TypeDefinitionResolPros.TimeAndPlaceHearing:
					FillTimeAndPlaceHearingInfo();
                    break;
				case TypeDefinitionResolPros.PostponeCase:
					FillPostponeCaselInfo();
                    break;
            }
        }

        private void FillCommonInfo()
        {
            this.ReportParams["НомерОпределения"] = _definition.DocumentNumber != null
                ? _definition.DocumentNumber.ToString()
                : _definition.DocumentNum;
            this.ReportParams["ДатаОпределения"] = _definition.DocumentDate.HasValue
                ? _definition.DocumentDate.Value.ToShortDateString()
                : string.Empty;

            if (_definition.IssuedDefinition != null)
            {
                this.ReportParams["Руководитель"] = _definition.IssuedDefinition.Fio;
                this.ReportParams["РуководительФИО"] = _definition.IssuedDefinition.ShortFio;
                this.ReportParams["ДолжностьРуководителя"] = _definition.IssuedDefinition.Position;
            }

            this.ReportParams["ДатаПостановления"] = _resolPros.DocumentDate.HasValue
               ? _resolPros.DocumentDate.Value.ToShortDateString()
               : string.Empty;

            this.ReportParams["НомерПостановления"] = _resolPros.DocumentNumber;

            if (_resolPros.Contragent != null)
            {
                this.ReportParams["Контрагент"] = _resolPros.Contragent.Name;
                this.ReportParams["АдресКонтрагента"] = _resolPros.Contragent.FiasJuridicalAddress != null
                    ? _resolPros.Contragent.FiasJuridicalAddress.AddressName
                    : _resolPros.Contragent.AddressOutsideSubject;
            }
        }

		private void FillTimeAndPlaceHearingInfo()
		{
			// переменные точно такие же
			FillPostponeCaselInfo();
		}

		private void FillPostponeCaselInfo()
        {
            // статьи в области охраны собственности
            var protectionActricles = new[]
            {
                "ст.7.21 ч.1 КоАП РФ",
                "ст.7.21 ч.2 КоАП РФ",
                "ст.7.22 КоАП РФ",
                "ст.7.23 КоАП РФ",
                "ст. 7.23.1 КоАП РФ"
            };

            // статьи в промышленности, строительстве и энергетике
            var industryActicles = new[]
            {
                "ст.9.16 ч.4 КоАП РФ",
                "ст. 9.16 ч.5 КоАП РФ"
            };

            // статьи против порядка управления
            var againstOrderArticles = new[]
            {
                "ст.19.4 ч.1 КоАП РФ",
                "ст.19.5 ч.1 КоАП РФ",
                "ст.19.6 КоАП РФ",
                "ст.19.7 КоАП РФ"
            };

            // статьи, посягающее на общественный порядок и общественную безопасность
            var againstPublicOrderArticles = new[]
            {
                "ст. 20.25 ч.1 КоАП РФ"
            };

            var articles = GetArticleLaws();
            if (articles.Any(p => protectionActricles.Any(n => p == n)))
            {
				this.ReportParams["ОбластьПравонарушения"] = "в области охраны собственности";
            }
            else if (articles.Any(p => industryActicles.Any(n => p == n)))
            {
				this.ReportParams["ОбластьПравонарушения"] = "в промышленности, строительстве и энергетике";
            }
            else if (articles.Any(p => againstOrderArticles.Any(n => p == n)))
            {
				this.ReportParams["ОбластьПравонарушения"] = "против порядка управления";
            }
            else if (articles.Any(p => againstPublicOrderArticles.Any(n => p == n)))
            {
				this.ReportParams["ОбластьПравонарушения"] = "посягающее на общественный порядок и общественную безопасность";
            }

            if (_resolPros.Contragent != null && _resolPros.Executant != null)
            {
                string contragent = null;
                string contragentAddress = null;
                string withRespect = null; // кода "тип исполнителя" 0,9,11,8,15,18,4,17,6,7,14,20,2,3,1,10,12,13,16,19,5

                // Коды исполнителя для поля Контрагент(юр. лицо)
                var typesForJurPerson = new[] { "0", "9", "11", "8", "15", "18", "4", "17" };

                // Коды исполнителя для поля Контрагент(должностное лицо)
                var typesForPerson = new[] { "1", "10", "12", "13", "16", "19", "5" };

                // Коды исполнителя для поля Физическое лицо (должностное лицо)
                var typesForPhysPerson = new[] { "6", "7", "14", "20", "2", "3" };

                if (typesForJurPerson.Contains(_resolPros.Executant.Code))
                {
                    contragent = _resolPros.Contragent.NameGenitive;
                    contragentAddress = _resolPros.Contragent.FiasJuridicalAddress != null
                        ? _resolPros.Contragent.FiasJuridicalAddress.AddressName
                        : _resolPros.Contragent.JuridicalAddress;
                    withRespect = _resolPros.Executant.Code;
                }
                else if (typesForPerson.Contains(_resolPros.Executant.Code))
                {
                    contragent = _resolPros.PhysicalPerson;
                    contragentAddress = _resolPros.PhysicalPersonInfo;
                    withRespect = _resolPros.Executant.Code;
                }
                else if (typesForPhysPerson.Contains(_resolPros.Executant.Code))
                {
                    contragent = _resolPros.PhysicalPerson;
                    contragentAddress = _resolPros.PhysicalPersonInfo;
                    withRespect = _resolPros.Executant.Code;
                }

                if (contragent != null)
                {
                    this.ReportParams["Контрагент"] = contragent;
                }
                if (contragentAddress != null)
                {
                    this.ReportParams["АдресКонтрагента"] = contragentAddress;
                }
                if (withRespect != null)
                {
                    this.ReportParams["Вотношении"] = withRespect;
                }
            }
            var docinspDomain = Container.ResolveDomain<DocumentGjiInspector>();
            using (Container.Using(docinspDomain))
            {
                var inspectors = docinspDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == _resolPros.Id)
                    .Select(x => x.Inspector)
                    .ToArray();

                if (inspectors.Any())
                {
                    var firstInspector = inspectors.First();
                    this.ReportParams["РуководительФИО"] = firstInspector.ShortFio;
                }
            }

            var date = _definition.ExecutionDate.ToDateTime();
	        var time = _definition.TimeDefinition.ToDateTime();

            this.ReportParams["ДатаИВремяРассмотренияДела"] = string.Format("{0:dd MMMM yyyy} в {1:HH} час. {1:mm} мин.",
				date, time);
        }
		
        private IList<string> GetArticleLaws()
        {
            var articleDomain = Container.ResolveDomain<ResolProsArticleLaw>();
            using (Container.Using(articleDomain))
            {
                var articles = articleDomain.GetAll()
                    .Where(x => x.ResolPros.Id == _resolPros.Id)
                    .Select(x => x.ArticleLaw.Name)
                    .ToList();

                return articles;
            }
        }
    }
}