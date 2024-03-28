namespace Bars.GkhGji.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ResolutionGjiReport : GjiBaseReport
    {
        protected long DocumentId { get; set; }

        protected override string CodeTemplate { get; set; }

        public ResolutionGjiReport() : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_Resolution))
        {
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

        /// <summary>
        /// Расширение
        /// </summary>
        public override string Extention => "mrt";

        /// <summary>
        /// Генератор отчетов
        /// </summary>
        public override string ReportGeneratorName => "StimulReportGenerator";

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
                            Description = "Вид санкции с кодом 0,1,5",
                            Template = Properties.Resources.BlockGJI_Resolution
                        },
                    new TemplateInfo
                        {
                            Name = "ResolutionGJI",
                            Code = "BlockGJI_Resolution_1",
                            Description = "Вид санкции с кодом 2",
                            Template = Properties.Resources.BlockGJI_Resolution_1
                        },
                    new TemplateInfo
                        {
                            Name = "ResolutionGJI",
                            Code = "BlockGJI_Resolution_2",
                            Description = "Вид санкции с кодом 3",
                            Template = Properties.Resources.BlockGJI_Resolution_2
                        },
                    new TemplateInfo
                        {
                            Name = "ResolutionGJI",
                            Code = "BlockGJI_Resolution_3",
                            Description = "Вид санкции с кодом 4",
                            Template = Properties.Resources.BlockGJI_Resolution_3
                        },
                    new TemplateInfo
                        {
                            Name = "ResolutionGJI",
                            Code = "BlockGJI_Resolution_4",
                            Description = "Вид санкции с кодом 1 и тип исполнителя с кодом 1,5,6,7,10,12,13,14,16,19",
                            Template = Properties.Resources.BlockGJI_Resolution
                        }
                };
        }

        protected virtual void SelectCodeTemlate(string sanctionCode, string executantCode)
        {
            switch (sanctionCode)
            {
                case "0":
                case "5":
                    CodeTemplate = "BlockGJI_Resolution";
                    break;
                case "2":
                    CodeTemplate = "BlockGJI_Resolution_1";
                    break;
                case "3":
                    CodeTemplate = "BlockGJI_Resolution_2";
                    break;
                case "4":
                    CodeTemplate = "BlockGJI_Resolution_3";
                    break;
                case "1":
                    if (new[] { "1", "5", "6", "7", "10", "12", "13", "14", "16", "19" }.Contains(executantCode))
                    {
                        CodeTemplate = "BlockGJI_Resolution_4";
                    }
                    else
                    {
                        CodeTemplate = "BlockGJI_Resolution";
                    }
                    break;
            }
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

            SelectCodeTemlate(resolution.Sanction.Code, resolution.Executant != null ? resolution.Executant.Code : string.Empty);
            FillCommonFields(reportParams, resolution);

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
                    reportParams.SimpleReportParams["ЗональноеНаименование1ГосЯзык"] = zonalInspection.BlankName;
                    reportParams.SimpleReportParams["ЗональноеНаименование2ГосЯзык"] = zonalInspection.BlankNameSecond;
                    reportParams.SimpleReportParams["Адрес1ГосЯзык"] = zonalInspection.Address;
                    reportParams.SimpleReportParams["Адрес2ГосЯзык"] = zonalInspection.AddressSecond;
                    reportParams.SimpleReportParams["Телефон"] = zonalInspection.Phone;
                    reportParams.SimpleReportParams["Email"] = zonalInspection.Email;
                    reportParams.SimpleReportParams["ОКАТО"] = firstRo.Municipality.Okato;
                }
            }

            reportParams.SimpleReportParams["Дата"] = resolution.DocumentDate.HasValue
                                                          ? resolution.DocumentDate.Value.ToShortDateString()
                                                          : string.Empty;

            reportParams.SimpleReportParams["Номер"] = resolution.DocumentNumber;

            if (resolution.Official != null)
            {
                reportParams.SimpleReportParams["КодДлВынесшегоПостановление"] = resolution.Official.Position;
                reportParams.SimpleReportParams["ФИОДлВынесшегоПостановление"] = resolution.Official.Fio;
                reportParams.SimpleReportParams["ФИОСокрДлПостановление"] = resolution.Official.ShortFio;
                reportParams.SimpleReportParams["КодРуководителяФИО(сИнициалами)"] = resolution.Official.ShortFio;
            }

            if (resolution.Executant != null)
            {
                switch (resolution.Executant.Code)
                {
                    case "0":
                    case "2":
                    case "4":
                    case "10":
                    case "12":
                    case "6":
                    case "7":
                    case "15":
                    case "21": //ИП
                        {
                            var contragentName = resolution.Contragent != null ? resolution.Contragent.Name : string.Empty;
                            reportParams.SimpleReportParams["Кого"] = contragentName;
                            reportParams.SimpleReportParams["Кого(РП)"] = contragentName;
                        }
                        
                        break;
                    case "1":
                    case "3":
                    case "5":
                    case "11":
                    case "13":
                    case "16":
                    case "18":
                    case "19":
                        {
                            var result = resolution.Contragent != null ? resolution.Contragent.Name : string.Empty;

                            if (!string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(resolution.PhysicalPerson))
                            {
                                result += ", ";
                            }

                            result += resolution.PhysicalPerson;

                            reportParams.SimpleReportParams["Кого"] = result;
                            reportParams.SimpleReportParams["Кого(РП)"] = result;
                        }

                        break;
                    case "8":
                    case "9":
                    case "14":
                        {
                            reportParams.SimpleReportParams["Кого"] = resolution.PhysicalPerson;
                            reportParams.SimpleReportParams["Кого(РП)"] = resolution.PhysicalPerson;
                        }
                       
                        break;
                }

                switch (resolution.Executant.Code)
                {
                    case "1":
                    case "3":
                    case "5":
                    case "11":
                    case "13":
                    case "16":
                    case "18":
                    case "19":
                        reportParams.SimpleReportParams["Присутствие"] = "в его присутствии";
                        break;
                    default:
                        reportParams.SimpleReportParams["Присутствие"] = "в присутствии представителя по договоренности";
                        break;
                }

                switch (resolution.Executant.Code)
                {
                    case "0":
                    case "2":
                    case "4":
                    case "10":
                    case "12":
                    case "6":
                    case "7":
                    case "15":
                    case "21": //ИП
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

                            reportParams.SimpleReportParams["ЮрАдресКонтрагента"] = newAddr;
                        }

                        reportParams.SimpleReportParams["Местонахождение"] = "(Место нахождения: $ЮрАдресКонтрагента$)";
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
                reportParams.SimpleReportParams["НомерПротокола"] = protocol.DocumentNumber;
                reportParams.SimpleReportParams["ДатаПротокола"] =
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

                reportParams.SimpleReportParams["ЧастьРаздельно"] = articles.Distinct(x => x.Part).AggregateWithSeparator(x => x.Part, ",");
                reportParams.SimpleReportParams["СтатьяРаздельно"] = articles.Distinct(x => x.Article).AggregateWithSeparator(x => x.Article, ",");
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

                reportParams.SimpleReportParams["Обращение"] = appealsBaseStatement;
            }

            var dispDoc = GetParentDocument(resolution, TypeDocumentGji.Disposal);

            if (dispDoc != null)
            {
                var disposal = Container.Resolve<IDomainService<Disposal>>().Load(dispDoc.Id);

                reportParams.SimpleReportParams["ВидОбследования"] = disposal.KindCheck != null ? disposal.KindCheck.Name : "";
                reportParams.SimpleReportParams["Распоряжение"] = string.Format(
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
                                reportParams.SimpleReportParams["ТипОбследования"] =
                                    "в связи с обращением граждан была проведена внеплановая проверка";
                            }

                            if (typeSurveys.Any(x => x.Code == "3"))
                            {
                                reportParams.SimpleReportParams["ТипОбследования"] =
                                    "по согласованию с прокуратурой ______________ была проведена внеплановая проверка";
                            }

                            break;
                        case TypeCheck.PlannedExit:
                            reportParams.SimpleReportParams["ГодИзДатыДокумента"] = disposal.DocumentYear;
                            reportParams.SimpleReportParams["ТипОбследования"] =
                                "в ходе проведения плановой проверки, в соотвествии с планом проверок жилищного фонда на $ГодИзДатыДокумента$ г.";
                            break;
                    }
                }

                if (disposal.IssuedDisposal != null)
                {
                    reportParams.SimpleReportParams["КодРуководителяФИО(сИнициалами)"] = string.Format("{0} {1}",
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

                reportParams.SimpleReportParams["НаселПунктОтдела"] = string.Join("; ", listLocality);
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

                            reportParams.SimpleReportParams["Дом1"] = firstRo.FiasAddress.House + housing;
                            reportParams.SimpleReportParams["Улица1"] = firstRo.FiasAddress.StreetName;
                        }
                    }
                }
            }

            var actDoc = GetParentDocument(resolution, TypeDocumentGji.ActCheck);

            if (actDoc != null)
            {
                reportParams.SimpleReportParams["Акт"] = string.Format(
                    "{0} от {1}",
                    actDoc.DocumentNumber,
                    actDoc.DocumentDate.HasValue ? actDoc.DocumentDate.Value.ToShortDateString() : string.Empty);
            }

            var protDoc = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                                    .Where(x => x.Children.Id == resolution.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.ProtocolMvd)
                                    .Select(x => x.Parent)
                                    .FirstOrDefault();

            if (protDoc != null)
            {
                reportParams.SimpleReportParams["ДатаИсполнительногоДокумента"] = protDoc.DocumentDate.HasValue
                    ? protDoc.DocumentDate.Value.ToShortDateString()
                    : string.Empty;

                var realityObjs = Container.Resolve<IDomainService<ProtocolMvdRealityObject>>().GetAll()
                    .Where(x => x.ProtocolMvd.Id == protDoc.Id)
                    .Where(x => x.RealityObject != null)
                    .Select(x => x.RealityObject)
                    .ToArray()
                    .Distinct(x => x.Id).ToArray();

                if (realityObjs.Any())
                {
                    var protocolFirstRo = realityObjs.FirstOrDefault();

                    if (protocolFirstRo.Municipality != null)
                    {
                        reportParams.SimpleReportParams["НаселенныйПункт"] = protocolFirstRo.Municipality.Name;
                    }

                    if (realityObjs.Count() == 1 && protocolFirstRo.FiasAddress != null)
                    {
                        var housing = !string.IsNullOrEmpty(protocolFirstRo.FiasAddress.Housing) ?
                                        ", корп. " + protocolFirstRo.FiasAddress.Housing :
                                        string.Empty;

                        reportParams.SimpleReportParams["НаселенныйПункт"] = protocolFirstRo.FiasAddress.PlaceName;
                        reportParams.SimpleReportParams["Дом"] = protocolFirstRo.FiasAddress.House + housing;
                        reportParams.SimpleReportParams["Улица"] = protocolFirstRo.FiasAddress.StreetName;
                    }
                }

                var artLaws = Container.Resolve<IDomainService<ProtocolMvdArticleLaw>>().GetAll()
                    .Where(x => x.ProtocolMvd.Id == protDoc.Id)
                    .Select(x => x.ArticleLaw)
                    .ToArray();

                if (artLaws.Any())
                {
                    reportParams.SimpleReportParams["СтатьяИсполнительногоДокумента"] =
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

                    reportParams.SimpleReportParams["СтатьяЗакона"] = result.ToStr();
                }

                #region Секция нарушений

                var protocolViol = Container.Resolve<IDomainService<ProtocolViolation>>().GetAll()
                                            .Where(x => x.Document.Id == protDoc.Id)
                                            .ToArray();

                var protocolRoAdr = protocolViol.Where(x => x.InspectionViolation != null && x.InspectionViolation.RealityObject != null)
                                                .Select(x => x.InspectionViolation.RealityObject.Address)
                                                .Distinct()
                                                .ToArray();

                reportParams.SimpleReportParams["АдресОбъектаПравонарушения"] = protocolRoAdr.Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

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

                reportParams.SimpleReportParams["ТекстНарушения"] = violText.ToString();
                reportParams.SimpleReportParams["Мероприятие"] = descrText.ToString();
                reportParams.SimpleReportParams["НарушениеПримечание"] = viols.ToString();

                #endregion
            }

            if (resolution.PenaltyAmount.HasValue)
            {
                reportParams.SimpleReportParams["Штраф"] = resolution.PenaltyAmount.Value.RoundDecimal(2);
                reportParams.SimpleReportParams["ШтрафПрописью"] = resolution.PenaltyAmount.Value.RoundDecimal(2);
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
                    reportParams.SimpleReportParams["НаименованиеБанка"] = contragentBank.Name;
                    reportParams.SimpleReportParams["КорСчет"] = contragentBank.CorrAccount;
                    reportParams.SimpleReportParams["РасчетныйСчет"] = contragentBank.SettlementAccount;
                    reportParams.SimpleReportParams["БИК"] = contragentBank.Bik;
                }

                reportParams.SimpleReportParams["КПП"] = resolution.Contragent.Kpp;
                reportParams.SimpleReportParams["ИНН"] = resolution.Contragent.Inn;

                reportParams.SimpleReportParams["АдресКонтрагента"] = resolution.Contragent.FiasJuridicalAddress != null
                                                                          ? resolution.Contragent.FiasJuridicalAddress.AddressName
                                                                          : string.IsNullOrEmpty(resolution.Contragent.AddressOutsideSubject)
                                                                                ? resolution.Contragent.AddressOutsideSubject
                                                                                : string.Empty;
            }

            reportParams.SimpleReportParams["СоставАдминПрав"] = resolution.Description;
            var listExecutantCode = new List<string>() { "1", "10", "12", "13", "16", "14", "6", "7", "19", "5" };
            reportParams.SimpleReportParams["ФизЛицо"] = listExecutantCode.Contains(resolution.Executant.Code)
                                                             ? resolution.PhysicalPerson
                                                             : string.Empty;

            reportParams.SimpleReportParams["ОКТМО"] = resolution.Municipality != null
                                                           ? resolution.Municipality.Oktmo
                                                           : null;

            this.FillRegionParams(reportParams, resolution);
        }
    }
}