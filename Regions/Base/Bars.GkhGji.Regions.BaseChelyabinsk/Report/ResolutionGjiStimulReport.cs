namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;
    using Bars.GkhGji.Report;
    using Slepov.Russian.Morpher;

    public class ResolutionGjiStimulReport : GjiBaseStimulReport
    {

        public ResolutionGjiStimulReport()
            : base(new ReportTemplateBinary(Resources.BlockGJI_Resolution))
        {
            ExportFormat = StiExportFormat.Word2007;
        }
        protected long DocumentId { get; set; }

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

        protected override string CodeTemplate { get; set; }

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
                            Name = "ResolutionGJI",
                            Code = "BlockGJI_Resolution",
                            Description = "Постановление о назначении",
                            Template = Properties.Resources.BlockGJI_Resolution
                        }
                };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var resolution = Container.Resolve<IDomainService<Resolution>>().Load(DocumentId);

            if (resolution == null)
            {
                throw new ReportProviderException("Не удалось получить постановление");
            }

            if (resolution.Sanction == null)
            {
                throw new ReportProviderException("Не указана санкция");
            }


            FillCommonFields(resolution);

            this.ReportParams["Id"] = this.DocumentId.ToString();
            this.ReportParams["СоставАдмПравонарушения"] = resolution.Description;

            // зональную инспекцию получаем через муниципальное образование первого дома
            var firstRo = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                .Where(x => x.Inspection.Id == resolution.Inspection.Id)
                .Select(x => x.RealityObject)
                .FirstOrDefault();

            var resolType = resolution.Inspection.TypeBase;

            if (resolType == TypeBase.ProsecutorsResolution)
            {
                firstRo = Container.Resolve<IDomainService<ResolProsRealityObject>>().GetAll()
                         .Where(x => x.ResolPros.Inspection.Id == resolution.Inspection.Id)
                         .Select(x => x.RealityObject)
                         .FirstOrDefault();
            }

            if (firstRo != null)
            {
                var zonalInspection = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll()
                    .Where(x => x.Municipality.Id == firstRo.Municipality.Id)
                    .Select(x => x.ZonalInspection)
                    .FirstOrDefault();

                if (zonalInspection != null)
                {
                    this.ReportParams["ЗональноеНаименование1ГосЯзык"] = zonalInspection.BlankName;
                    this.ReportParams["ЗональноеНаименование2ГосЯзык"] = zonalInspection.BlankNameSecond;
                    this.ReportParams["Адрес1ГосЯзык"] = zonalInspection.Address;
                    this.ReportParams["Адрес2ГосЯзык"] = zonalInspection.AddressSecond;
                    this.ReportParams["Телефон"] = zonalInspection.Phone;
                    this.ReportParams["Email"] = zonalInspection.Email;
                    this.ReportParams["ОКАТО"] = firstRo.Municipality.Okato;
                }
            }

            this.ReportParams["Дата"] = resolution.DocumentDate.HasValue
                                                          ? resolution.DocumentDate.Value.ToShortDateString()
                                                          : string.Empty;

            this.ReportParams["Номер"] = resolution.DocumentNumber;

            if (resolution.Official != null)
            {
                this.ReportParams["КодДлВынесшегоПостановление"] = resolution.Official.Position;
                this.ReportParams["ФИОДлВынесшегоПостановление"] = resolution.Official.Fio;
                this.ReportParams["ФИОСокрДлПостановление"] = resolution.Official.ShortFio;
                this.ReportParams["КодРуководителяФИО(сИнициалами)"] = resolution.Official.ShortFio;
            }

            string who = string.Empty;

            if (resolution.Executant != null)
            {
                switch (resolution.Executant.Code)
                {
                    case "0":
                    case "2":
                    case "4":
                    case "8":
                    case "9":
                    case "11":
                    case "15":
                    case "18":
                        {
                            var contragentName = resolution.Contragent != null ? resolution.Contragent.Name : string.Empty;
                            who = contragentName;
                            //Report["Кого"] = contragentName;
                            //Report["Кого(РП)"] = contragentName;
                        }

                        break;
                    case "1":
                    case "3":
                    case "5":
                    case "10":
                    case "12":
                    case "13":
                    case "16":
                    case "19":
                        {
                            var result = resolution.Contragent != null ? resolution.Contragent.Name : string.Empty;

                            if (!string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(resolution.PhysicalPerson))
                            {
                                result += ", ";
                            }

                            result += resolution.PhysicalPerson;


                            who = result;
                            //Report["Кого"] = result;
                            //Report["Кого(РП)"] = result;
                        }

                        break;
                    case "6":
                    case "7":
                    case "14":
                        {
                            //Report["Кого"] = resolution.PhysicalPerson;
                            //Report["Кого(РП)"] = resolution.PhysicalPerson;
                            who = resolution.PhysicalPerson;
                        }

                        break;
                }
                if (!string.IsNullOrWhiteSpace(who))
                {
                    var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");
                    var whoAnalyzed = склонятель.Проанализировать(who);
                    this.ReportParams["Кого"] = whoAnalyzed.Именительный;
                    this.ReportParams["Кого(РП)"] = whoAnalyzed.Родительный;
                    this.ReportParams["КогоТП"] = whoAnalyzed.Творительный;
                    this.ReportParams["КогоВП"] = whoAnalyzed.Винительный;
                }

                switch (resolution.Executant.Code)
                {
                    case "1":
                    case "5":
                    case "7":
                    case "10":
                    case "12":
                    case "13":
                    case "14":
                    case "16":
                    case "19":
                        this.ReportParams["Присутствие"] = "в его присутствии";
                        break;
                    default:
                        this.ReportParams["Присутствие"] = "в присутствии представителя по договоренности";
                        break;
                }

                switch (resolution.Executant.Code)
                {
                    case "0":
                    case "2":
                    case "4":
                    case "8":
                    case "9":
                    case "11":
                    case "15":
                    case "18":
                        if (resolution.Contragent != null && resolution.Contragent.FiasJuridicalAddress != null)
                        {
                            var subStr = resolution.Contragent.FiasJuridicalAddress.AddressName.Split(',');

                            var newAddr = new StringBuilder();

                            foreach (var rec in subStr)
                            {
                                if (newAddr.Length > 0)
                                {
                                    newAddr.Append(',');
                                }

                                if (rec.Contains("р-н."))
                                {
                                    var mu = rec.Replace("р-н.", string.Empty) + " район";
                                    newAddr.Append(mu);
                                    continue;
                                }

                                newAddr.Append(rec);
                            }

                            this.ReportParams["ЮрАдресКонтрагента"] = newAddr.ToString();
                        }

                        this.ReportParams["Местонахождение"] = "(Место нахождения: $ЮрАдресКонтрагента$)";
                        break;
                }
            }

            var protocol = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                .Where(x => x.Children.Id == resolution.Id)
                .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Protocol)
                .Select(x => new
                {
                    x.Parent.Id,
                    x.Parent.DocumentNumber,
                    x.Parent.DocumentDate
                })
                .FirstOrDefault();

            if (protocol != null)
            {
                this.ReportParams["НомерПротокол"] = protocol.DocumentNumber;
                this.ReportParams["ДатаПротокола"] =
                    protocol.DocumentDate.HasValue
                        ? protocol.DocumentDate.Value.ToShortDateString()
                        : null;

                var articles = Container.Resolve<IDomainService<ProtocolArticleLaw>>().GetAll()
                    .Where(x => x.Protocol.Id == protocol.Id)
                    .Select(x => new
                    {
                        x.ArticleLaw.Part,
                        x.ArticleLaw.Article
                    })
                    .ToList();

                this.ReportParams["ЧастьРаздельно"] = articles.Distinct(x => x.Part).AggregateWithSeparator(x => x.Part, ",");
                this.ReportParams["СтатьяРаздельно"] = articles.Distinct(x => x.Article).AggregateWithSeparator(x => x.Article, ",");
            }

            if (resolution.Inspection.TypeBase == TypeBase.CitizenStatement)
            {
                // обращения граждан, связанные с основанием проверки
                var appealsBaseStatement = Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                    .Where(x => x.Inspection.Id == resolution.Inspection.Id)
                    .Select(x => new { x.AppealCits.NumberGji, x.AppealCits.DateFrom })
                    .AsEnumerable()
                    .AggregateWithSeparator(x =>
                        string.Format("{0} от {1}", x.NumberGji, x.DateFrom.ToDateTime().ToShortDateString()), ", ");

                this.ReportParams["Обращение"] = appealsBaseStatement;
            }

            var dispDoc = GetParentDocument(resolution, TypeDocumentGji.Disposal);

            if (dispDoc != null)
            {
                var disposal = Container.Resolve<IDomainService<GkhGji.Entities.Disposal>>().Load(dispDoc.Id);

                this.ReportParams["ВидОбследования"] = disposal.KindCheck != null ? disposal.KindCheck.Name : "";
                this.ReportParams["Распоряжение"] = string.Format(
                    "{0} от {1}",
                    disposal.DocumentNumber,
                    disposal.DocumentDate.HasValue ? disposal.DocumentDate.Value.ToShortDateString() : string.Empty);

                var typeSurveys =
                    Container.Resolve<IDomainService<DisposalTypeSurvey>>().GetAll()
                             .Where(x => x.Disposal.Id == disposal.Id)
                             .Select(x => x.TypeSurvey)
                             .ToArray();

                if (disposal.KindCheck != null)
                {
                    switch (disposal.KindCheck.Code)
                    {
                        case TypeCheck.NotPlannedExit:
                            if (typeSurveys.Any(x => x.Code == "1" || x.Code == "2"))
                            {
                                this.ReportParams["ТипОбследования"] =
                                    "в связи с обращением граждан была проведена внеплановая проверка";
                            }

                            if (typeSurveys.Any(x => x.Code == "3"))
                            {
                                this.ReportParams["ТипОбследования"] =
                                    "по согласованию с прокуратурой ______________ была проведена внеплановая проверка";
                            }

                            break;
                        case TypeCheck.PlannedExit:
                            this.ReportParams["ГодИзДатыДокумента"] = disposal.DocumentYear.ToString();
                            this.ReportParams["ТипОбследования"] =
                                "в ходе проведения плановой проверки, в соотвествии с планом проверок жилищного фонда на $ГодИзДатыДокумента$ г.";
                            break;
                    }
                }

                if (disposal.IssuedDisposal != null)
                {
                    this.ReportParams["КодРуководителяФИО(сИнициалами)"] = string.Format("{0} {1}",
                        disposal.IssuedDisposal.Position,
                        string.IsNullOrEmpty(disposal.IssuedDisposal.ShortFio) ? disposal.IssuedDisposal.Fio : disposal.IssuedDisposal.ShortFio);
                }

                var queryInspectorId = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                                                     .Where(x => x.DocumentGji.Id == dispDoc.Id)
                                                     .Select(x => x.Inspector.Id);

                var listLocality = Container.Resolve<IDomainService<ZonalInspectionInspector>>().GetAll()
                                    .Where(x => queryInspectorId.Contains(x.Inspector.Id))
                                    .Select(x => x.ZonalInspection.Locality)
                                    .Distinct()
                                    .ToList();

                this.ReportParams["НаселПунктОтдела"] = string.Join("; ", listLocality);
            }

            // если тип основания - постановление прокуратуры
            if (resolution.Inspection.TypeBase == TypeBase.ProsecutorsResolution)
            {
                var prosClaim = GetParentDocument(resolution, TypeDocumentGji.ResolutionProsecutor);

                if (prosClaim != null)
                {
                    var resolProsRealObj = Container.Resolve<IDomainService<ResolProsRealityObject>>().GetAll()
                                                    .Where(x => x.ResolPros.Id == prosClaim.Id)
                                                    .Select(x => x.RealityObject)
                                                    .ToArray();

                    if (resolProsRealObj.Count() == 1)
                    {
                        firstRo = resolProsRealObj.FirstOrDefault();

                        if (firstRo != null && firstRo.FiasAddress != null)
                        {
                            var housing = !string.IsNullOrEmpty(firstRo.FiasAddress.Housing) ?
                                        ", корп. " + firstRo.FiasAddress.Housing :
                                        string.Empty;

                            this.ReportParams["Дом1"] = firstRo.FiasAddress.House + housing;
                            this.ReportParams["Улица1"] = firstRo.FiasAddress.StreetName;
                        }
                    }
                }
            }

            var actDoc = GetParentDocument(resolution, TypeDocumentGji.ActCheck);

            if (actDoc != null)
            {
                this.ReportParams["Акт"] = string.Format(
                    "{0} от {1}",
                    actDoc.DocumentNumber,
                    actDoc.DocumentDate.HasValue ? actDoc.DocumentDate.Value.ToShortDateString() : string.Empty);
            }

            var protDoc = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                                    .Where(x => x.Children.Id == resolution.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Protocol)
                                    .Select(x => x.Parent)
                                    .FirstOrDefault();

            if (protDoc != null)
            {
                this.ReportParams["ДатаИсполнительногоДокумента"] = protDoc.DocumentDate.HasValue
                    ? protDoc.DocumentDate.Value.ToShortDateString()
                    : string.Empty;

				var protocolViolationDomain = Container.Resolve<IDomainService<ProtocolViolation>>();
	            using (Container.Using(protocolViolationDomain))
	            {
					var realityObjs = protocolViolationDomain.GetAll()
						.Where(x => x.Document.Id == protocol.Id)
						.Select(x => x.InspectionViolation.RealityObject)
						.AsEnumerable()
						.Where(x => x != null)
						.Distinct(x => x.Id)
						.ToArray();

					if (realityObjs.Any())
					{
						this.ReportParams["НаселенныйПункт"] = realityObjs[0].FiasAddress != null
							? realityObjs[0].FiasAddress.PlaceName
							: string.Empty;
					}
	            }

                var artLaws = Container.Resolve<IDomainService<ProtocolArticleLaw>>().GetAll()
                    .Where(x => x.Protocol.Id == protDoc.Id)
                    .Select(x => x.ArticleLaw)
                    .ToArray();

                if (artLaws.Any())
                {
                    this.ReportParams["СтатьяИсполнительногоДокумента"] =
                        artLaws.Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Name : y.Name));

                    var result = new StringBuilder();

                    foreach (var art in artLaws.Select(x => x.Name))
                    {
                        var index = art.ToUpperInvariant().IndexOf("КОАП");

                        var newStr = art;

                        if (index != -1)
                        {
                            newStr = newStr.Substring(0, index).Trim(' ');
                        }

                        newStr = newStr.Replace("ст.", string.Empty).Trim();

                        if (result.Length > 0)
                        {
                            result.Append(", ");
                        }

                        result.Append(newStr);
                    }

                    this.ReportParams["СтатьяЗакона"] = result.ToStr();
                }

                #region Секция нарушений

                var protocolViol = Container.Resolve<IDomainService<ProtocolViolation>>().GetAll()
                                            .Where(x => x.Document.Id == protDoc.Id)
                                            .ToArray();

                var protocolRoAdr = protocolViol.Where(x => x.InspectionViolation != null && x.InspectionViolation.RealityObject != null)
                                                .Select(x => x.InspectionViolation.RealityObject.Address)
                                                .Distinct()
                                                .ToArray();

                this.ReportParams["АдресОбъектаПравонарушения"] = protocolRoAdr.Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

                int i = 0;

                var violSection = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияНарушения");

                var viols = new StringBuilder();

                var listUniqViol = new List<long>();

                var violText = new StringBuilder();

                var descrText = new StringBuilder();

                foreach (var rec in protocolViol)
                {
                    violSection.ДобавитьСтроку();
                    violSection["Номер1"] = ++i;
                    violSection["ОписаниеНарушения"] = string.Format("{0} ({1})", rec.InspectionViolation.Violation.Name, rec.InspectionViolation.Violation.CodePin);

                    if (listUniqViol.Contains(rec.InspectionViolation.Violation.Id))
                        continue;

                    listUniqViol.Add(rec.InspectionViolation.Violation.Id);

                    if (viols.Length > 0)
                        viols.Append(", ");

                    viols.AppendFormat("{0} - {1}", rec.InspectionViolation.Violation.Name, rec.Description);

                    if (!string.IsNullOrEmpty(rec.InspectionViolation.Violation.Name))
                    {
                        if (violText.Length > 0)
                            violText.Append(", ");

                        violText.Append(rec.InspectionViolation.Violation.Name);
                    }

                    if (!string.IsNullOrEmpty(rec.Description))
                    {
                        if (descrText.Length > 0)
                            descrText.Append(", ");

                        descrText.Append(rec.Description);
                    }


                    violSection["ПП_РФ_170"] = rec.InspectionViolation.Violation.PpRf170;
                    violSection["ПП_РФ_25"] = rec.InspectionViolation.Violation.PpRf25;
                    violSection["ПП_РФ_307"] = rec.InspectionViolation.Violation.PpRf307;
                    violSection["ПП_РФ_491"] = rec.InspectionViolation.Violation.PpRf491;
                    violSection["Прочие_норм_док"] = rec.InspectionViolation.Violation.OtherNormativeDocs;
                }

                this.ReportParams["ТекстНарушения"] = violText.ToString();
                this.ReportParams["Мероприятие"] = descrText.ToString();
                this.ReportParams["НарушениеПримечание"] = viols.ToString();

                #endregion
            }

            if (resolution.PenaltyAmount.HasValue)
            {
                this.ReportParams["Штраф"] = resolution.PenaltyAmount.Value.RoundDecimal(2).ToString();
                this.ReportParams["ШтрафПрописью"] = resolution.PenaltyAmount.Value.RoundDecimal(2).ToString();
            }

            if (resolution.Contragent != null)
            {
                var contragentBank = Container.Resolve<IDomainService<ContragentBank>>()
                                                .GetAll()
                                                .Where(x => x.Contragent.Id == resolution.Contragent.Id)
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

                if (contragentBank != null)
                {
                    this.ReportParams["НаименованиеБанка"] = contragentBank.Name;
                    this.ReportParams["КорСчет"] = contragentBank.CorrAccount;
                    this.ReportParams["РасчетныйСчет"] = contragentBank.SettlementAccount;
                    this.ReportParams["БИК"] = contragentBank.Bik;
                }

                this.ReportParams["КПП"] = resolution.Contragent.Kpp;
                this.ReportParams["ИНН"] = resolution.Contragent.Inn;
				this.ReportParams["ОГРН"] = resolution.Contragent.Ogrn;

                this.ReportParams["АдресКонтрагента"] = resolution.Contragent.FiasJuridicalAddress != null
                                                                          ? resolution.Contragent.FiasJuridicalAddress.AddressName
                                                                          : string.IsNullOrEmpty(resolution.Contragent.AddressOutsideSubject)
                                                                                ? resolution.Contragent.AddressOutsideSubject
                                                                                : string.Empty;
            }

            this.ReportParams["СоставАдминПрав"] = resolution.Description;
            var listExecutantCode = new List<string>() { "1", "10", "12", "13", "16", "14", "6", "7", "19", "5" };
            this.ReportParams["ФизЛицо"] = listExecutantCode.Contains(resolution.Executant.Code)
                                                             ? resolution.PhysicalPerson
                                                             : string.Empty;

            this.ReportParams["ОКТМО"] = resolution.Municipality != null
                                                           ? resolution.Municipality.Oktmo
                                                           : null;
            this.ReportParams["ТипИсполнителя"] = resolution.Executant.Code;

            if (resolution.Official != null)
            {
				// Пробегаемся по зон. инспекциям и формируем итоговую строку наименований и строку идентификаторов
				var serviceZonalInspectionInspector = Container.Resolve<IDomainService<ZonalInspectionInspector>>();
	            using (Container.Using(serviceZonalInspectionInspector))
	            {
					var zonalInspId = resolution.Official.Id;
					var zonalInspectionNames = new StringBuilder();
					
					var dataZonalInspection = serviceZonalInspectionInspector.GetAll()
						.Where(x => x.Inspector.Id == zonalInspId)
						.Select(x => new
						{
							x.ZonalInspection.ZoneName
						})
						.ToArray();

					foreach (var item in dataZonalInspection)
					{
						if (!string.IsNullOrEmpty(item.ZoneName))
						{
							if (zonalInspectionNames.Length > 0)
								zonalInspectionNames.Append(", ");

							zonalInspectionNames.Append(item.ZoneName);
						}
					}

		            this.ReportParams["Инспекция"] = zonalInspectionNames.ToString();
	            }
            }
        }
    }
}
