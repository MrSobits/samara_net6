namespace Bars.GkhGji.Regions.Tomsk.Report.ProtocolGji
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Report;

	/// <summary>
	/// Печатка Протокол
	/// </summary>
    public class ProtocolGjiStimulReport : GjiBaseStimulReport
    {
		/// <summary>
		/// Конструктор
		/// </summary>
        public ProtocolGjiStimulReport() : base(new ReportTemplateBinary(Properties.Resources.TomskProtocol))
        {
        }

		#region Properties

		/// <summary>
		/// Идентификатор отчета
		/// </summary>
		public override string Id
        {
            get { return "TomskProtocolGji"; }
        }

		/// <summary>
		/// Код формы
		/// </summary>
		public override string CodeForm
        {
            get { return "Protocol"; }
        }

		/// <summary>
		/// Наименование
		/// </summary>
		public override string Name
        {
            get { return "Протокол"; }
        }

		/// <summary>
		/// Описание
		/// </summary>
		public override string Description
        {
            get { return "Протокол"; }
        }

		/// <summary>
		/// Формат экспорта
		/// </summary>
		public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

		/// <summary>
		/// Домен сервис для Описание протокола
		/// </summary>
		public IDomainService<ProtocolDescription> DescriptionDomain { get; set; }

		/// <summary>
		/// Домен сервис для Определение протокола
		/// </summary>
		public IDomainService<TomskProtocolDefinition> DefinitionDomain { get; set; } 

        protected override string CodeTemplate { get; set; }

        #endregion Properties

        #region Fields

        private long documentId;

        private Protocol protocol;

        private Disposal disposal;

		#endregion Fields

		/// <summary>
		/// Установить пользовательские параметры
		/// </summary>
		public override void SetUserParams(UserParamsValues userParamsValues)
        {
	        this.documentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            // Поскольку во многих местах нужен родительский Disposal то берем его как поле объекта и получаем здесь
            var protocolDomain = this.Container.ResolveDomain<Protocol>();

            using (this.Container.Using(protocolDomain))
            {
	            this.protocol = protocolDomain.FirstOrDefault(x => x.Id == this.documentId);

                var dispParent = this.GetParentDocument(this.protocol, TypeDocumentGji.Disposal);
                if (dispParent != null)
                {
	                this.disposal = dispParent as Disposal;
                }
            }
        }

		/// <summary>
		/// Получить информация о шаблоне
		/// </summary>
		public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "TomskProtocolGji_1",
                    Name = "Protocol",
                    Description = "Тип обследования с кодом 22",
                    Template = Properties.Resources.TomskProtocol
                },
                new TemplateInfo
                {
                    Code = "TomskProtocolGji",
                    Name = "Protocol",
                    Description = "Любой другой случай",
                    Template = Properties.Resources.TomskProtocol
                }
            };
        }

		/// <summary>
		/// Получить шаблон
		/// </summary>
		public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }
        
        private void GetCodeTemplate()
        {
	        this.CodeTemplate = "TomskProtocolGji";

            if (this.disposal == null)
            {
                return;
            }

            var dispTypesurDomain = this.Container.ResolveDomain<DisposalTypeSurvey>();

            using (this.Container.Using(dispTypesurDomain))
            {
                // если у распоряжения есть тип проверки с кодом 22
                if (dispTypesurDomain.GetAll().Any(x => x.Disposal.Id == this.disposal.Id && x.TypeSurvey.Code == "22"))
                {
	                this.CodeTemplate = "TomskProtocolGji_1";
                }
            }
        }

		/// <summary>
		/// Подготовить отчет
		/// </summary>
		public override void PrepareReport(ReportParams reportParams)
        {
            if (this.protocol == null)
            {
                return;
            }

	        this.FillCommonFields(this.protocol);

	        this.FillProtocolInspectors();

	        this.FillAddress();

	        this.FillProtocolData();

	        this.FillArticlesLaw();

	        this.FillDisposalInfo();

	        this.FillActInfo();

	        this.FillPrescriptionInfo();

	        this.FillContragentBank();

	        this.FillProtocolViolations();

	        this.FillProtocolAnnex();

	        this.FillDocPhysInfo();
        }

        protected void FillDocPhysInfo()
        {
            var docPhysInfoDomain = this.Container.ResolveDomain<DocumentPhysInfo>();

            var physInfo = docPhysInfoDomain.GetAll().FirstOrDefault(x => x.Document.Id == this.documentId);

            if (physInfo != null)
            {
	            this.ReportParams["МестоРаботы"] = physInfo.PhysJob;
	            this.ReportParams["Должность"] = physInfo.PhysPosition;
	            this.ReportParams["ДатаРождения"] = physInfo.PhysBirthdayAndPlace;
	            this.ReportParams["ДУЛ"] = physInfo.PhysIdentityDoc;
            }
        }

        protected void FillProtocolAnnex()
        {
            var protocolAnnexDomain = this.Container.ResolveDomain<ProtocolAnnex>();
            try
            {
                var annexes = protocolAnnexDomain.GetAll()
                        .Where(x => x.Protocol.Id == this.documentId)
                        .Select(x => new
                        {
                            x.DocumentDate,
                            x.Name
                        })
                        .AsEnumerable()
                        .Select(x => "{0} {1}".FormatUsing(x.Name, x.DocumentDate.HasValue ? "от " +  x.DocumentDate.Value.ToShortDateString() : string.Empty))
                        .AggregateWithSeparator(", ");

	            this.ReportParams["ПрилагаемыеДокументы"] = annexes;
            }
            finally
            {
	            this.Container.Release(protocolAnnexDomain);
            }
        }

        protected void FillProtocolInspectors()
        {
            var docinspDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
            using (this.Container.Using(docinspDomain))
            {
                var inspectors = docinspDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == this.documentId)
                    .Select(x => x.Inspector)
                    .ToArray();

                if (inspectors.Any())
                {
                    var firstInspector = inspectors.First();

	                this.ReportParams["Инспектор"] = firstInspector.Fio;
	                this.ReportParams["ДолжностьИнсТП"] = firstInspector.PositionAblative;
	                this.ReportParams["ДолжностьИнспектора"] = firstInspector.Position;
                    
	                if (!string.IsNullOrEmpty(firstInspector.FioAblative) && !string.IsNullOrEmpty(firstInspector.ShortFio))
	                {
		                this.ReportParams["ИнспекторФамИОТП"] =
			                string.Format(
				                "{0} {1}",
				                firstInspector.FioAblative.Split(' ')[0],
				                firstInspector.ShortFio.Substring(firstInspector.ShortFio.IndexOf(' ')));

		                this.ReportParams["ИнспекторТворП"] = inspectors.AggregateWithSeparator(x => x.FioAblative, ", ");
		                this.ReportParams["ИнспекторФамИО"] = firstInspector.ShortFio;
		                this.ReportParams["ИнспекторФИОСокр"] = firstInspector.ShortFio;
		                this.ReportParams["ИнспекторДолжность(ТворП)"] = inspectors
				                .AggregateWithSeparator(x => string.Format("{0} - {1}", x.FioAblative, x.PositionAblative), ", ");
	                }

	                this.ReportParams["КодИнспектора"] = firstInspector.Position;
	                this.ReportParams["ДолжностьТворП"] = inspectors.AggregateWithSeparator(x => x.PositionAblative, ", ");
                    
                }
            }
        }

        protected void FillAddress()
        {
            var reqDomain = this.Container.ResolveDomain<RequirementDocument>();
            var docchildDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
            var insproDomain = this.Container.ResolveDomain<InspectionGjiRealityObject>();
            var protviolDomain = this.Container.ResolveDomain<ProtocolViolation>();

            using (this.Container.Using(reqDomain, docchildDomain, insproDomain, protviolDomain))
            {
                var ofActCheck = docchildDomain.GetAll()
                    .Where(x => x.Children.Id == this.protocol.Id)
                    .Any(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck);

                var ofRequirement = reqDomain.GetAll().Any(x => x.Document.Id == this.protocol.Id);

                var addressObjOffences = string.Empty;

                var protocolRo = protviolDomain.GetAll()
                    .Where(x => x.Document.Id == this.protocol.Id)
                    .Select(x => new
                    {
                        x.InspectionViolation.RealityObject.Id,
                        x.InspectionViolation.RealityObject.Address,
                        x.InspectionViolation.RealityObject.FiasAddress.AddressName,
                        x.InspectionViolation.RealityObject.FiasAddress.PlaceName,
                        x.InspectionViolation.RealityObject.FiasAddress.StreetName,
                        x.InspectionViolation.RealityObject.FiasAddress.House,
                        x.InspectionViolation.RealityObject.FiasAddress.Housing,
                        Municipality = x.InspectionViolation.RealityObject.Municipality.Name
                    })
                    .AsEnumerable()
                    .Distinct(x => x.Id)
                    .ToArray();

                if (protocolRo.Any())
                {
                    var firstRo = protocolRo[0];

	                this.ReportParams["АдресОбъектаПравонарушения"] = protocolRo.AggregateWithSeparator(x => x.AddressName, ", ");
	                this.ReportParams["АдресОбъектаПравонарушенияПолный"] = protocolRo.AggregateWithSeparator(x => x.AddressName, ", ");
	                this.ReportParams["Район"] = firstRo.Municipality;
	                this.ReportParams["НаселенныйПункт"] = firstRo.PlaceName;

                    if (protocolRo.Length == 1)
                    {
	                    this.ReportParams["Улица"] = firstRo.StreetName;
	                    this.ReportParams["Дом"] = firstRo.House;
	                    this.ReportParams["Корпус"] = firstRo.Housing;
                    }
                }

                if (ofRequirement && this.protocol.Contragent != null && this.protocol.Contragent.FiasJuridicalAddress != null)
                {
                    addressObjOffences = this.protocol.Contragent.FiasJuridicalAddress.AddressName;
                }

                if (ofActCheck)
                {
                    addressObjOffences = insproDomain.GetAll()
                        .Where(x => x.Inspection.Id == this.protocol.Inspection.Id)
                        .Select(x => x.RealityObject.FiasAddress.AddressName)
                        .AggregateWithSeparator(";");
                }

	            this.ReportParams["АдресОбъектаПравонарушения"] = addressObjOffences;
            }
        }

        protected void FillProtocolData()
        {
            var firstExecutantCodeList = new List<string> { "0", "9", "11", "8", "15", "18", "4" };
            var secondExecutantCodeList = new List<string> { "1", "10", "12", "13", "16", "19", "5" };
            var thirdExecutantCodeList = new List<string> { "6", "7", "14" };

	        this.ReportParams["Номер"] = this.protocol.DocumentNumber;
	        this.ReportParams["ДатаПротокола"] = this.protocol.DocumentDate.HasValue
                ? this.protocol.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU"))
                : null;


            var descr = this.DescriptionDomain.GetAll().FirstOrDefault(x => x.Protocol.Id == this.protocol.Id).Return(x => x.Description);

            var descrSet = this.DescriptionDomain.GetAll().FirstOrDefault(x => x.Protocol.Id == this.protocol.Id).Return(x => x.DescriptionSet);

	        this.ReportParams["СоставАдминПрав"] = descr != null ? Encoding.UTF8.GetString(descr) : this.protocol.Description;

	        this.ReportParams["УстановилПост"] = descrSet != null ? Encoding.UTF8.GetString(descrSet) : this.protocol.Description;
	        this.ReportParams["УстановилПодр"] = descr != null ? Encoding.UTF8.GetString(descr) : string.Empty;

            var contrContactDomain = this.Container.Resolve<IDomainService<ContragentContact>>();

            var contragent = this.protocol.Return(x => x.Contragent);
            var executant = this.protocol.Return(x => x.Executant);

            var contragentId = contragent.Return(x => x.Id);

            var contragentContact = contrContactDomain.GetAll()
                .Where(x => x.Contragent.Id == contragentId)
                .Select(x => new
                {
                    x.FullName,
                    x.Position.Name
                })
                .FirstOrDefault();

	        this.ReportParams["Вотношении"] = executant != null ? executant.Code : string.Empty;
	        this.ReportParams["ФизЛицо"] = this.protocol.PhysicalPerson;
	        this.ReportParams["Реквизиты"] = this.protocol.PhysicalPersonInfo;

            if (contragent != null)
            {
	            this.ReportParams["Контрагент"] = contragent.ShortName;
	            this.ReportParams["ЮрАдрес"] = contragent.JuridicalAddress;
	            this.ReportParams["ФактАдрес"] = contragent.FactAddress;
	            this.ReportParams["ОГРН"] = contragent.Ogrn;
	            this.ReportParams["ИНН"] = contragent.Inn;
	            this.ReportParams["КПП"] = contragent.Kpp;
            }

            if (firstExecutantCodeList.Contains(executant.Return(x => x.Code)))
            {
                var direc = string.Empty;
                if (contragentContact.ReturnSafe(x => x.Name.ToLower().Trim()) == "руководитель")
                {
                    direc = contragentContact.Return(x => x.FullName);
                }

	            this.ReportParams["ТипИсполнителя"] =
                    string.Format("{0} юридический адрес: {1} фактический адрес: {2}  ОГРН: {3} ИНН/КПП: {4} {5}",
                        contragent.Return(x => x.ShortName),
                        contragent.Return(x => x.JuridicalAddress),
                        contragent.Return(x => x.FactAddress),
                        contragent.Return(x => x.Ogrn),
                        contragent.Return(x => x.Inn) + "/" + contragent.Return(x => x.Kpp),
                        direc);
            }

            if (secondExecutantCodeList.Contains(executant.Return(x => x.Code)))
            {
	            this.ReportParams["ТипИсполнителя"] = this.protocol.PhysicalPerson + " " + contragent.Return(x => x.ShortName);
            }

            if (thirdExecutantCodeList.Contains(executant.Return(x => x.Code)))
            {
	            this.ReportParams["ТипИсполнителя"] = this.protocol.PhysicalPerson;
            }

            if (this.protocol.DocumentDate != null)
            {
	            this.ReportParams["ВремяСовершенияПравонарушения"] = this.protocol.DocumentDate.ToDateTime().AddDays(-1).ToString("dd MMMM yyyy");
            }

            var definition = this.DefinitionDomain.GetAll()
                    .FirstOrDefault(x =>
                            x.Protocol.Id == this.documentId &&
                            x.TypeDefinition == TypeDefinitionProtocol.TimeAndPlaceHearing);

            if (definition != null)
            {
	            this.ReportParams["ДатаРассмотренияДела"] = definition.DateOfProceedings.HasValue
                    ? definition.DateOfProceedings.Value.ToShortDateString()
                    : string.Empty;
	            this.ReportParams["ВремяРассмотренияДела"] = definition.TimeDefinition.HasValue 
                    ? "{0:d2}:{1:d2}".FormatUsing(definition.TimeDefinition.Value.Hour, definition.TimeDefinition.Value.Minute) 
                    : string.Empty;
	            this.ReportParams["МестоРассмотренияДела"] = definition.PlaceReview;
            }

        }

        protected void FillArticlesLaw()
        {
            var articleDomain = this.Container.ResolveDomain<ProtocolArticleLaw>();
            var tomskArticleLawDomain = this.Container.ResolveDomain<TomskArticleLawGji>();

            using (this.Container.Using(articleDomain))
            {
                var articles = articleDomain.GetAll()
                    .Where(x => x.Protocol.Id == this.protocol.Id)
                    .Where(x => tomskArticleLawDomain.GetAll()
                            .Any(a => a.Id == x.ArticleLaw.Id))
                   .Join(tomskArticleLawDomain.GetAll(), p => p.ArticleLaw.Id, a => a.Id,
                    (p, a) => new
                    {
                        p.ArticleLaw.Code,
                        p.ArticleLaw.Name,
                        p.ArticleLaw.Description,
                        p.ArticleLaw.Part,
                        p.ArticleLaw.Article,
                        a.OffPersonPenalty,
                        a.PhysPersonPenalty,
                        a.JurPersonPenalty
                    })
                    .ToArray();

	            this.ReportParams["ОтветственностьЮР"] = articles.Select(x => x.JurPersonPenalty).AggregateWithSeparator(", ");
	            this.ReportParams["ОтветственностьФиз"] = articles.Select(x => x.PhysPersonPenalty).AggregateWithSeparator(", ");
	            this.ReportParams["ОтветственностьДЛ"] = articles.Select(x => x.OffPersonPenalty).AggregateWithSeparator(", ");
	            this.ReportParams["КодСтатьи"] = articles.Select(x => x.Code).AggregateWithSeparator(", ");

                var description = articles.AggregateWithSeparator(x => x.Description, ", ");
                var articlesString = string.Empty;
                foreach (var article in articles)
                {
                    if (!string.IsNullOrEmpty(articlesString))
                    {
                        articlesString += ", ";
                    }

                    if (!string.IsNullOrEmpty(article.Part) && !string.IsNullOrEmpty(article.Article))
                    {
                        articlesString += string.Format("частью {0} статьи {1}", article.Part, article.Article);
                    }
                    else if (!string.IsNullOrEmpty(article.Article))
                    {
                        articlesString += string.Format("статьей {0}", article.Article);
                    }
                    else
                    {
                        // если ничего неуказано в справочниках, то выводим стандартное наименование
                        articlesString += article.Name;
                    }
                }

	            this.ReportParams["Статьи"] = articlesString;

                var codeList = articles.Select(x => x.Code).ToList();
                var descriptionsDict = articles.GroupBy(x => x.Code)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Description).AggregateWithSeparator(","));

                var fullStateDiscription = new List<string>();

                if (codeList.Contains("1"))
                {
                    fullStateDiscription.Add("нарушение правил пользования жилыми помещениями. " +
                                             "В указанной квартире выполнена самовольное переустройство, " +
                                             "а именно: указанные работы по переустройству организовал и выполнил___________________." +
                                             "Разрешительные документы на переустройство указанного жилого помещения, предусмотренные ст. 26 ЖК РФ, отсутствуют.");
                }
                if (codeList.Contains("2"))
                {
                    fullStateDiscription.Add("нарушение правил пользования жилыми помещениями. " +
                                             "В указанной квартире выполнена самовольная перепланировка, " +
                                             "а именно: указанные работы по перепланировке организовал и выполнил___________________. " +
                                             "Разрешительные документы на перепланировку указанного жилого помещения, предусмотренные  ст. 26 ЖК РФ, отсутствуют.");
                }
                if (codeList.Contains("3"))
                {
                    fullStateDiscription.Add("нарушение правил содержания и ремонта жилого дома, а именно: " + descriptionsDict.Get("3"));
                }
                if (codeList.Contains("4"))
                {
                    fullStateDiscription.Add("нарушение режима обеспечения населения коммунальными услугами, а именно: " + descriptionsDict.Get("4"));
                }
                if (codeList.Contains("5"))
                {
                    fullStateDiscription.Add("нарушение требований законодательства о раскрытии информации организациями, осуществляющими деятельность в сфере управления многоквартирными домами, а именно: " + descriptionsDict.Get("5"));
                }
                if (codeList.Contains("8"))
                {
                    fullStateDiscription.Add("неповиновение законному требованию должностного лица органа, осуществляющего государственный надзор (контроль), а именно: " + descriptionsDict.Get("8"));
                }
                if (codeList.Contains("10"))
                {
                    fullStateDiscription.Add("невыполнение в установленный срок законного предписания органа (должностного лица), осуществляющего государственный надзор (контроль), об устранении нарушений законодательства, а именно: " + descriptionsDict.Get("10"));
                }
                if (codeList.Contains("6") || codeList.Contains("7"))
                {
                    var code6 = descriptionsDict.Get("6");
                    var code7 = descriptionsDict.Get("7");
                    
                    fullStateDiscription.Add("нарушение законодательства об энергосбережении и о повышении энергетической эффективности, а именно: "
                          + (code6.IsNotEmpty() && code7.IsNotEmpty() ?
                                                string.Format("{0}, {1}", code6, code7) :
                                                code6.IsNotEmpty() ?
                                                        code6 :
                                                        code7.IsNotEmpty() ?
                                                            code7 :
                                                            string.Empty));
                }
                if (codeList.Contains("11"))
                {
                    fullStateDiscription.Add("непредставление или несвоевременное представление в государственный орган (должностному лицу) " +
                                             "сведений (информации), представление которых предусмотрено законом и необходимо для осуществления " +
                                             "этим органом (должностным лицом) его законной деятельности, а равно представление в государственный " +
                                             "орган таких сведений в неполном объеме или в искаженном виде, а именно: " + descriptionsDict.Get("11"));
                }
                if (codeList.Contains("9"))
                {
                    fullStateDiscription.Add("воспрепятствование законной деятельности должностного лица органа государственного контроля (надзора), повлекшие невозможность проведения или завершения проверки, а именно: " + descriptionsDict.Get("9"));
                }

	            this.ReportParams["СтатьяЗакона"] = articles.AggregateWithSeparator(x => x.Name, ", ");
	            this.ReportParams["Установил"] = fullStateDiscription.AggregateWithSeparator(";");
	            this.ReportParams["Описание"] = description;

                var state1 = "ст. 26, 29, 30 Жилищного кодекса РФ, п. 1.7.1.  Правил и норм эксплуатации жилищного фонда (утв. постановлением Госстроя РФ от 27 сентября 2003 г. № 170)";
                var state3 = "ч. 1, ч. 2.3 ст. 161 Жилищного кодекса РФ";
                var state4 = "п. 4 ст. 3 ЖК РФ; ст. 161 ЖК РФ";
                var state5 = "ч. 1, ч. 2.3 ст. 161 ЖК РФ";
                var state8 = "пп. 2п. 10, пп. а, б пп. 20 п. 9 Положения о Департаменте ЖКХ и государственного жилищного надзора Томской области, утвержденное постановлением Губернатора Томской области от 03.10.2012 № 117.";

                var articleStr = new List<string>();

                if (codeList.Contains("1") || codeList.Contains("2"))
                {
                    articleStr.Add(state1);
                }
                if (codeList.Contains("3"))
                {
                    articleStr.Add(state3);
                }
                if (codeList.Contains("4"))
                {
                    articleStr.Add(state4);
                }
                if (codeList.Contains("5"))
                {
                    articleStr.Add(state5);
                }
                if (codeList.Contains("8"))
                {
                    articleStr.Add(state8);
                }

	            this.ReportParams["НарушенныеТребования"] = articleStr.AggregateWithSeparator("; ");
            }
        }

        /// <summary>
        /// The fill act info.
        /// </summary>
        protected void FillActInfo()
        {
            var act = this.GetParentDocument(this.protocol, TypeDocumentGji.ActCheck);

            if (act != null)
            {
            var actDate = act
                .Return(x => x.DocumentDate.HasValue
                    ? x.DocumentDate.Value.ToShortDateString()
                    : null);

	            this.ReportParams["Акт"] = string.Format(
                "Акт №{0} от {1}",
                    act.DocumentNumber,
                act.DocumentDate.HasValue ? act.DocumentDate.Value.ToShortDateString() : string.Empty);

	            this.ReportParams["ДатаАкта"] = actDate;

	            this.ReportParams["АктПроверкиДата"] = actDate;
	            this.ReportParams["АктПроверкиНомер"] = act.Return(x => x.DocumentNumber);

	            this.ReportParams["ДатаАктаПроверки"] = actDate;
	            this.ReportParams["НомерАктаПроверки"] = act.Return(x => x.DocumentNumber);
        }
        }

        protected void FillDisposalInfo()
        {
            
            if (this.disposal == null)
            {
                return;
            }

            var disposalTextServ = this.Container.Resolve<IDisposalText>();
            var dispTypesurDomain = this.Container.ResolveDomain<DisposalTypeSurvey>();
            var docinspDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
            var zoninspDomain = this.Container.ResolveDomain<ZonalInspectionInspector>();

            using (this.Container.Using(disposalTextServ, dispTypesurDomain, docinspDomain))
            {
                var disposalText = disposalTextServ.SubjectiveCase;

                var dispDate = this.disposal.DocumentDate.HasValue
                        ? this.disposal.DocumentDate.Value.ToShortDateString()
                        : null;

                var dispNumberDate = string.Format("{0} №{1} от {2}", disposalText, this.disposal.DocumentNum, dispDate);

	            this.ReportParams["Распоряжение"] = dispNumberDate;
	            this.ReportParams["РаспоряжениеНомер"] = this.disposal.DocumentNumber;
	            this.ReportParams["РаспоряжениеДата"] = dispDate;
	            this.ReportParams["РаспоряжениеНомерДата"] = dispNumberDate;

                var issuedDisposal = this.disposal.IssuedDisposal;

                if (issuedDisposal != null)
                {
                    var fioPosition =
                        string.Format("{0} - {1}",
                            issuedDisposal.Position,
                            string.IsNullOrEmpty(issuedDisposal.ShortFio)
                                ? issuedDisposal.Fio
                                : issuedDisposal.ShortFio);

	                this.ReportParams["РуководительФИО"] = fioPosition;
	                this.ReportParams["РаспоряжениеДлДолжностьФио"] = fioPosition;

	                this.ReportParams["РаспоряжениеДлКод"] = issuedDisposal.Code;
	                this.ReportParams["ДолжностьРуководителя"] = issuedDisposal.Code;
                }

                var queryInspectorId =
                    docinspDomain.GetAll()
                        .Where(x => x.DocumentGji.Id == this.disposal.Id)
                        .Select(x => x.Inspector.Id);

	            this.ReportParams["НаселПунктОтдела"] =
                    zoninspDomain.GetAll()
                        .Where(x => queryInspectorId.Contains(x.Inspector.Id))
                        .Select(x => x.ZonalInspection.Locality)
                        .Distinct()
                        .AggregateWithSeparator("; ");
            }
        }

        protected void FillPrescriptionInfo()
        {
            var prescrViodDomain = this.Container.ResolveDomain<PrescriptionViol>();
            var zonalMuDomain = this.Container.ResolveDomain<ZonalInspectionMunicipality>();

            using (this.Container.Using(prescrViodDomain, zonalMuDomain))
            {
                // проверяем создан ли протокол на основе предписания
                var basePrescription = this.DocChildDomain.GetAll()
                        .Where(x => x.Children.Id == this.protocol.Id)
                        .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                        .Select(x => x.Parent)
                        .FirstOrDefault();

                if (basePrescription != null)
                {
	                this.ReportParams["ПредДокумент"] =
                        string.Format("{0} от {1}",
                            basePrescription.DocumentNumber,
                            basePrescription.DocumentDate.HasValue
                                ? basePrescription.DocumentDate.Value.ToShortDateString()
                                : string.Empty);

                    var robjectMunicipality =
                        prescrViodDomain.GetAll()
                            .Where(x => x.Document.Id == basePrescription.Id)
                            .Select(x => x.InspectionViolation.RealityObject.Municipality)
                            .FirstOrDefault();

                    if (robjectMunicipality.Return(x => x.Id) > 0)
                    {
                        var muId = robjectMunicipality.Return(x => x.Id);

                        var zonalInspection = zonalMuDomain.GetAll()
                            .Where(x => x.Municipality.Id == muId)
                            .Select(x => x.ZonalInspection)
                            .FirstOrDefault();

                        if (zonalInspection != null)
                        {
	                        this.ReportParams["ЗональноеНаименование1ГосЯзык"] = zonalInspection.BlankName;
	                        this.ReportParams["ЗональноеНаименование2ГосЯзык"] = zonalInspection.BlankNameSecond;
	                        this.ReportParams["АдресЗЖИ"] = zonalInspection.Address;
                        }
                    }
                }
            }
        }

        protected void FillExecutant()
        {
            if (this.protocol.Executant != null)
            {
                if ((new List<string> { "0", "2", "4", "8", "9", "11", "15", "18" }).Contains(this.protocol.Executant.Code))
                {
	                this.ReportParams["Местонахождение"] = this.protocol.Contragent != null
                        ? this.protocol.Contragent.JuridicalAddress
                        : string.Empty;
                }
                else if ((new List<string> { "6", "7", "14" }).Contains(this.protocol.Executant.Code))
                {
	                this.ReportParams["Местонахождение"] = this.protocol.PhysicalPersonInfo;
                }
            }
        }

        protected void FillContragentBank()
        {
            if (this.protocol.Contragent == null)
            {
                return;
            }

            var contragentBankDomain = this.Container.ResolveDomain<ContragentBank>();

            using (this.Container.Using(contragentBankDomain))
            {
                var contragentBank = contragentBankDomain.GetAll()
                    .Where(x => x.Contragent.Id == this.protocol.Contragent.Id)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Bik,
                        x.CorrAccount,
                        x.SettlementAccount
                    })
                    .FirstOrDefault();

                if (contragentBank != null)
                {
	                this.ReportParams["НаименованиеБанка"] = contragentBank.Name;
	                this.ReportParams["КорСчет"] = contragentBank.CorrAccount;
	                this.ReportParams["РасчетныйСчет"] = contragentBank.SettlementAccount;
	                this.ReportParams["БИК"] = contragentBank.Bik;
                }
            }
        }

        protected void FillProtocolViolations()
        {
            var protocolViolDomain = this.Container.ResolveDomain<ProtocolViolation>();

            using (this.Container.Using(protocolViolDomain))
            {
                var violations = protocolViolDomain.GetAll()
                    .Where(x => x.Document.Id == this.protocol.Id)
                    .Select(x => new {x.Id, x.InspectionViolation, x.InspectionViolation.Violation})
                    .Select(x => new
                    {
                        x.Id,
                        x.InspectionViolation.DateFactRemoval,
                        x.Violation.Name,
                        x.Violation.CodePin,
                        x.Violation.PpRf170,
                        x.Violation.PpRf25,
                        x.Violation.PpRf307,
                        x.Violation.PpRf491,
                        x.Violation.OtherNormativeDocs
                    })
                    .ToList();

                var protocolViol = new List<ProtocolViolProxy>();

                var i = 0;

                foreach (var viol in violations)
                {
                    var violProxy = new ProtocolViolProxy
                    {
                        Номер1 = ++i,
                        Код = viol.CodePin,
                        ТекстНарушения = viol.Name,
                        СрокУстранения = viol.DateFactRemoval.HasValue
                            ? viol.DateFactRemoval.Value.ToShortDateString()
                            : string.Empty,
                        ПП_РФ_170 = viol.PpRf170,
                        ПП_РФ_25 = viol.PpRf25,
                        ПП_РФ_307 = viol.PpRf307,
                        ПП_РФ_491 = viol.PpRf491,
                        Прочие_норм_док = viol.OtherNormativeDocs
                    };

                    protocolViol.Add(violProxy);
                }

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Нарушения",
                    MetaType = nameof(ProtocolViolProxy),
                    Data = protocolViol
                });
            }
        }

        private class ProtocolViolProxy
        {
            public int Номер1;
            public string Код;
            public string ТекстНарушения;
            public string СрокУстранения;
            public string ПП_РФ_170;
            public string ПП_РФ_25;
            public string ПП_РФ_307;
            public string ПП_РФ_491;
            public string Прочие_норм_док;
        }
    }
}