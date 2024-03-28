using Bars.B4.DataAccess;

namespace Bars.GkhGji.Report
{
    using Bars.Gkh.Utils;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Contracts;

    /// <summary>
    /// Отчет "Протокол"
    /// </summary>
    public class ProtocolGjiReport : ProtocolGjiReport<Protocol>
    {
    }

    /// <summary>
    /// Базовый класс отчета "Протокол"
    /// </summary>
    public class ProtocolGjiReport<TProtocol> : GjiBaseReport
        where TProtocol : Protocol
    {
        private long DocumentId { get; set; }

        /// <inheritdoc />
        protected override string CodeTemplate { get; set; }

        public ProtocolGjiReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_ExecutiveDocProtocol))
        {
        }

        /// <inheritdoc />
        public override string Id => "Protocol";

        /// <inheritdoc />
        public override string CodeForm => "Protocol";

        /// <inheritdoc />
        public override string Name => "Протокол";

        /// <inheritdoc />
        public override string Description => "Протокол";

        /// <inheritdoc />
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "BlockGJI_ExecutiveDocProtocol_1",
                            Name = "ProtocolGJI",
                            Description = "Тип обследования с кодом 22",
                            Template = Properties.Resources.BlockGJI_ExecutiveDocProtocol_11
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_ExecutiveDocProtocol",
                            Name = "ProtocolGJI",
                            Description = "Любой другой случай",
                            Template = Properties.Resources.BlockGJI_ExecutiveDocProtocol1
                        }
                };
        }

        /// <inheritdoc />
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            var protocolDomain = Container.ResolveDomain<TProtocol>();
            var disposalTextServ = Container.Resolve<IDisposalText>();
            var disposalTypeSurveyDomain = Container.ResolveDomain<DisposalTypeSurvey>();
            var disposalDomain = Container.ResolveDomain<Disposal>();
            var docInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            var zonInspInspectorDomain = Container.ResolveDomain<ZonalInspectionInspector>();
            var zonInspMuDomain = Container.ResolveDomain<ZonalInspectionMunicipality>();
            var contragentBankDomain = Container.ResolveDomain<ContragentBank>();
            var protocolArticleLawDomain = Container.ResolveDomain<ProtocolArticleLaw>();
            var protocolViolDomain = Container.ResolveDomain<ProtocolViolation>();
            var actCheckDomain = Container.ResolveDomain<ActCheck>();
            var actCheckPeriodsDomain = Container.ResolveDomain<ActCheckPeriod>();
            var docChildrenDomain = Container.ResolveDomain<DocumentGjiChildren>();
            var prescriptionViolDomain = Container.ResolveDomain<PrescriptionViol>();
            var violActRemovDomain = Container.ResolveDomain<ViolationActionsRemovGji>();
            var inspectionRealObjDomain = Container.ResolveDomain<InspectionGjiRealityObject>();

            try
            {
                var protocol = protocolDomain.Load(this.DocumentId);

                if (protocol == null)
                {
                    throw new ReportProviderException("Не удалось получить протокол");
                }

                // Заполняем общие поля
                FillCommonFields(reportParams, protocol);

                var cultureInfo = new CultureInfo("ru-RU");

                CodeTemplate = "BlockGJI_ExecutiveDocProtocol";

                var disposal = GetParentDocument(protocol, TypeDocumentGji.Disposal);

                if (disposal != null)
                {
                    var disposalText = disposalTextServ.SubjectiveCase;
                    reportParams.SimpleReportParams["Распоряжение"] = string.Format(
                        "{0} № {1} от {2}",
                        disposalText,
                        disposal.DocumentNumber,
                        disposal.DocumentDate.HasValue ? disposal.DocumentDate.Value.ToShortDateString() : "");

                    // если у распоряжения есть тип проверки с кодом 22
                    if (disposalTypeSurveyDomain.GetAll().Any(x => x.Disposal.Id == disposal.Id && x.TypeSurvey.Code == "22"))
                    {
                        CodeTemplate = "BlockGJI_ExecutiveDocProtocol_1";
                    }

                    var issuedDisposal = disposalDomain.GetAll().Where(x => x.Id == disposal.Id).Select(x => x.IssuedDisposal).FirstOrDefault();

                    if (issuedDisposal != null)
                    {
                        reportParams.SimpleReportParams["РуководительФИО"] = string.Format(
                            "{0} - {1}",
                            issuedDisposal.Position,
                            string.IsNullOrEmpty(issuedDisposal.ShortFio) ? issuedDisposal.Fio : issuedDisposal.ShortFio);


                        reportParams.SimpleReportParams["ДолжностьРуководителя"] = issuedDisposal.Code;
                    }

                    var queryInspectorId = docInspectorDomain.GetAll()
                                                         .Where(x => x.DocumentGji.Id == disposal.Id)
                                                         .Select(x => x.Inspector.Id);

                    var listLocality = zonInspInspectorDomain.GetAll()
                                        .Where(x => queryInspectorId.Contains(x.Inspector.Id))
                                        .Select(x => x.ZonalInspection.Locality)
                                        .Distinct()
                                        .ToList();

                    reportParams.SimpleReportParams["НаселПунктОтдела"] = string.Join("; ", listLocality);
                }

                reportParams.SimpleReportParams["ДатаПротокола"] = protocol.DocumentDate.HasValue
                                                                       ? protocol.DocumentDate.Value.ToString("D", cultureInfo)
                                                                       : string.Empty;

                reportParams.SimpleReportParams["Номер"] = protocol.DocumentNumber;

                reportParams.SimpleReportParams["РеквизитыФизЛица"] = protocol.PhysicalPersonInfo;

                var parentDoc = base.GetParentDocument(protocol, TypeDocumentGji.ActCheck) as ActCheck;

                if (parentDoc != null)
                {
                    reportParams.SimpleReportParams["Кв"] = parentDoc.Flat;
                }

                var contragent = protocol.Contragent;

                if (contragent != null)
                {
                    if (contragent.FiasJuridicalAddress != null)
                    {
                        var subStr = contragent.FiasJuridicalAddress.AddressName.Split(',');

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

                        reportParams.SimpleReportParams["АдресКонтрагента"] = newAddr;
                    }
                    else
                    {
                        reportParams.SimpleReportParams["АдресКонтрагента"] = contragent.AddressOutsideSubject;
                    }

                    var contragentBank = contragentBankDomain
                                                    .GetAll()
                                                    .Where(x => x.Contragent.Id == contragent.Id)
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

                    reportParams.SimpleReportParams["КПП"] = contragent.Kpp;
                    reportParams.SimpleReportParams["ИНН"] = contragent.Inn;
                    reportParams.SimpleReportParams["ДатаРегистрации"] = contragent.DateRegistration.HasValue ? contragent.DateRegistration.Value.ToShortDateString() : string.Empty;
                }

                reportParams.SimpleReportParams["СоставАдминПрав"] = protocol.Description;

                if (protocol.Executant != null)
                {
                    switch (protocol.Executant.Code)
                    {
                        case "1":
                        case "5":
                        case "6":
                        case "7":
                        case "10":
                        case "12":
                        case "13":
                        case "14":
                        case "16":
                        case "19":
                            reportParams.SimpleReportParams["ФизЛицо"] = protocol.PhysicalPerson;
                            reportParams.SimpleReportParams["Реквизиты"] = protocol.PhysicalPersonInfo;
                            break;
                    }

                    if ((new List<string> { "0", "2", "4", "8", "9", "11", "15", "18" }).Contains(protocol.Executant.Code))
                    {
                        reportParams.SimpleReportParams["Местонахождение"] = protocol.Contragent != null
                                                                                 ? protocol.Contragent.JuridicalAddress
                                                                                 : string.Empty;
                    }
                    else
                        if ((new List<string> { "6", "7", "14" }).Contains(protocol.Executant.Code))
                        {
                            reportParams.SimpleReportParams["Местонахождение"] = protocol.PhysicalPersonInfo;
                        }
                }

                var protocolArticleLaws = protocolArticleLawDomain.GetAll()
                    .Where(x => x.Protocol.Id == protocol.Id)
                    .Select(x => new { x.ArticleLaw.Name, x.ArticleLaw.Description })
                    .ToList();

                reportParams.SimpleReportParams["СтатьяЗакона"] = protocolArticleLaws.Select(x => x.Name)
                             .AggregateWithSeparator(", ");

                reportParams.SimpleReportParams["Описание"] = protocolArticleLaws.Select(x => x.Description)
                             .AggregateWithSeparator(", ");

                var realityObjs = protocolViolDomain.GetAll()
                         .Where(x => x.Document.Id == protocol.Id)
                         .Select(x => x.InspectionViolation.RealityObject)
                         .AsEnumerable()
                         .Where(x => x != null)
                         .Distinct(x => x.Id)
                         .ToArray();

                if (realityObjs.Any())
                {
                    reportParams.SimpleReportParams["АдресОбъектаПравонарушения"] =
                        realityObjs.AggregateWithSeparator(x => x.Return(y => y.Address), "; ");

                    reportParams.SimpleReportParams["АдресОбъектаПравонарушенияПолный"] = realityObjs.Aggregate(
                        string.Empty,
                        (x, y) => x + (!string.IsNullOrEmpty(x)
                            ? "; " + y.FiasAddress.AddressName.Substring(y.FiasAddress.AddressName.IndexOf(',') + 1)
                            : y.FiasAddress.AddressName.Substring(y.FiasAddress.AddressName.IndexOf(',') + 2)));

                    reportParams.SimpleReportParams["Район"] = realityObjs[0].Municipality.Name;
                    reportParams.SimpleReportParams["НаселенныйПункт"] = realityObjs[0].FiasAddress.PlaceName;

                    if (realityObjs.Count() == 1)
                    {
                        var firstRoProtocol = realityObjs.FirstOrDefault();

                        if (firstRoProtocol != null && firstRoProtocol.FiasAddress != null)
                        {
                            reportParams.SimpleReportParams["Улица"] = firstRoProtocol.FiasAddress.StreetName;
                            reportParams.SimpleReportParams["Дом"] = firstRoProtocol.FiasAddress.House;
                            reportParams.SimpleReportParams["Корпус"] = firstRoProtocol.FiasAddress.Housing;
                        }
                    }
                }

                var listInspectors = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == protocol.Id)
                    .Select(x => new
                    {
                        x.Inspector.Fio,
                        x.Inspector.ShortFio,
                        x.Inspector.FioAblative,
                        x.Inspector.Position,
                        x.Inspector.PositionAblative
                    })
                    .ToList();

                var firstInspector = listInspectors.FirstOrDefault();

                if (firstInspector != null && !string.IsNullOrEmpty(firstInspector.ShortFio))
                {
                    reportParams.SimpleReportParams["КодИнспектора"] = firstInspector.Position;
                    reportParams.SimpleReportParams["Инспектор"] = firstInspector.Fio;
                    reportParams.SimpleReportParams["ИнспекторФамИО"] = firstInspector.ShortFio;
                    reportParams.SimpleReportParams["ИнспекторФамИОТП"] = string.Format("{0} {1}", firstInspector.FioAblative.Split(' ')[0], firstInspector.ShortFio.Substring(firstInspector.ShortFio.IndexOf(' ')));
                    reportParams.SimpleReportParams["ДолжностьИнсТП"] = firstInspector.PositionAblative;

                    reportParams.SimpleReportParams["ДолжностьИнспектора"] = firstInspector.Position;
                    reportParams.SimpleReportParams["ИнспекторФИОСокр"] = firstInspector.ShortFio;
                }

                if (listInspectors.Count > 0)
                {
                    reportParams.SimpleReportParams["ИнспекторДолжность(ТворП)"] =
                    listInspectors.AggregateWithSeparator(
                        x => string.Format("{0} - {1}", x.FioAblative, x.PositionAblative), ", ");

                    reportParams.SimpleReportParams["ДолжностьТворП"] = listInspectors.AggregateWithSeparator(x => x.PositionAblative, ", ");
                    reportParams.SimpleReportParams["ИнспекторТворП"] = listInspectors.AggregateWithSeparator(x => x.FioAblative, ", ");
                }

                if (protocol.Contragent != null)
                {
                    reportParams.SimpleReportParams["Контрагент"] = protocol.Contragent.Name;
                    reportParams.SimpleReportParams["УправОргСокр"] = protocol.Contragent.ShortName;
                    reportParams.SimpleReportParams["АдресКонтрагентаФакт"] = protocol.Contragent.FiasFactAddress != null ? protocol.Contragent.FiasFactAddress.AddressName : string.Empty;
                    reportParams.SimpleReportParams["АдресКонтрагентаЮр"] = protocol.Contragent.FiasJuridicalAddress != null ? protocol.Contragent.FiasJuridicalAddress.AddressName : string.Empty;
                }

                var actDoc = GetParentDocument(protocol, TypeDocumentGji.ActCheck);

                if (actDoc != null)
                {
                    var act = actCheckDomain.Load(actDoc.Id);

                    reportParams.SimpleReportParams["Акт"] = string.Format(
                        "Акт №{0} от {1}",
                        act.DocumentNumber,
                        act.DocumentDate.HasValue ? act.DocumentDate.Value.ToShortDateString() : "");

                    reportParams.SimpleReportParams["НомерАктаПроверки"] = actDoc.DocumentNumber;
                    reportParams.SimpleReportParams["ДатаАктаПроверки"] = actDoc.DocumentDate.HasValue ? actDoc.DocumentDate.Value.ToShortDateString() : string.Empty;
                    reportParams.SimpleReportParams["ВремяСоставленияАкта"] =
                        actCheckPeriodsDomain.GetAll()
                            .Where(x => x.ActCheck.Id == act.Id)
                            .Select(x => new { x.DateCheck, x.DateEnd })
                            .OrderBy(x => x.DateCheck)
                            .ThenBy(x => x.DateEnd)
                            .ToArray()
                            .LastOrDefault(x => x.DateEnd.HasValue)
                            .Return(x => x.DateEnd.ToDateString("HH-mm"));
                    reportParams.SimpleReportParams["НомерАкта"] = actDoc.DocumentNumber;
                    reportParams.SimpleReportParams["ДатаАкта"] = actDoc.DocumentDate.HasValue ? actDoc.DocumentDate.Value.ToShortDateString() : string.Empty;
                }

                // проверяем создан ли протокол на основе предписания
                var basePrescription =
                    docChildrenDomain.GetAll()
                        .Where(x => x.Children.Id == protocol.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                        .Select(x => x.Parent)
                        .FirstOrDefault();

                if (basePrescription != null)
                {
                    reportParams.SimpleReportParams["ПредДокумент"] = string.Format(
                        "{0} от {1}",
                        basePrescription.DocumentNumber,
                        basePrescription.DocumentDate.HasValue ? basePrescription.DocumentDate.Value.ToShortDateString() : string.Empty);

                    var robjectMunicipalityId = prescriptionViolDomain.GetAll()
                                                     .Where(x => x.Document.Id == basePrescription.Id)
                                                     .Select(x => x.InspectionViolation.RealityObject.Municipality.Id)
                                                     .FirstOrDefault();

                    if (robjectMunicipalityId > 0)
                    {
                        var zonalInspection = zonInspMuDomain.GetAll()
                            .Where(x => x.Municipality.Id == robjectMunicipalityId)
                            .Select(x => x.ZonalInspection)
                            .FirstOrDefault();

                        if (zonalInspection != null)
                        {
                            reportParams.SimpleReportParams["ЗональноеНаименование1ГосЯзык"] = zonalInspection.BlankName;
                            reportParams.SimpleReportParams["ЗональноеНаименование2ГосЯзык"] = zonalInspection.BlankNameSecond;
                            reportParams.SimpleReportParams["АдресЗЖИ"] = zonalInspection.Address;
                        }
                    }
                }

                #region Секция нарушений

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияНарушений");

                var violations = protocolViolDomain.GetAll()
                                          .Where(x => x.Document.Id == protocol.Id)
                                          .ToList();

                var actRemovByViol = violActRemovDomain.GetAll()
                    .Select(x => new
                    {
                        x.ViolationGji.Id,
                        x.ActionsRemovViol.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Name));

                reportParams.SimpleReportParams["Мероприятие"] =
                    actRemovByViol.Where(x => violations.Any(y => y.InspectionViolation.Violation.Id == x.Key))
                        .Select(x => x.Value)
                        .SelectMany(x => x)
                        .Distinct()
                        .AggregateWithSeparator(", ");

                var i = 0;
                var linksViolations = new List<string>();
                foreach (var viol in violations)
                {
                    section.ДобавитьСтроку();
                    section["Номер1"] = ++i;
                    section["Код"] = viol.InspectionViolation.Violation.CodePin;
                    section["ТекстНарушения"] = viol.InspectionViolation.Violation.Name;
                    section["СрокУстранения"] = viol.InspectionViolation.DateFactRemoval.HasValue
                                                    ? viol.InspectionViolation.DateFactRemoval.Value.ToShortDateString()
                                                    : string.Empty;

                    section["ПП_РФ_170"] = viol.InspectionViolation.Violation.PpRf170;
                    section["ПП_РФ_25"] = viol.InspectionViolation.Violation.PpRf25;
                    section["ПП_РФ_307"] = viol.InspectionViolation.Violation.PpRf307;
                    section["ПП_РФ_491"] = viol.InspectionViolation.Violation.PpRf491;
                    section["Прочие_норм_док"] = viol.InspectionViolation.Violation.OtherNormativeDocs;

                    var linksViolation = new[]
                                             {
                                                 viol.InspectionViolation.Violation.PpRf170, viol.InspectionViolation.Violation.PpRf25, viol.InspectionViolation.Violation.PpRf307,
                                                 viol.InspectionViolation.Violation.PpRf491
                                             };
                    section["СсылкиНаПунктыНормативныхАктов"] = string.Join(", ", linksViolation);
                    linksViolations.AddRange(linksViolation);
                }

                reportParams.SimpleReportParams["СсылкиНаПунктыНормативныхАктов"] = string.Join(", ", linksViolations.Distinct().Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim()).ToArray());

                #endregion

                reportParams.SimpleReportParams["ДатаИВремяРассмотренияДела"] = string.Format("{0} в {1} час. {2} мин.",
                                                                                                protocol.DateOfProceedings != null
                                                                                                    ? protocol.DateOfProceedings.ToDateTime().ToShortDateString()
                                                                                                    : string.Empty,
                                                                                                protocol.HourOfProceedings,
                                                                                                protocol.MinuteOfProceedings);

                reportParams.SimpleReportParams["МестоСоставления"] = inspectionRealObjDomain.GetAll().Where(x => x.Inspection.Id == protocol.Inspection.Id)
                    .Select(x => x.RealityObject.Municipality.Name)
                    .FirstOrDefault();

                if (protocol.DocumentDate != null)
                {
                    reportParams.SimpleReportParams["Времясовершенияправонарушения"] = protocol.DocumentDate.ToDateTime().AddDays(-1).ToShortDateString();
                }

                this.FillRegionParams(reportParams, protocol);
            }
            finally
            {
                Container.Release(protocolDomain);
                Container.Release(disposalTextServ);
                Container.Release(disposalTypeSurveyDomain);
                Container.Release(disposalDomain);
                Container.Release(docInspectorDomain);
                Container.Release(zonInspInspectorDomain);
                Container.Release(zonInspMuDomain);
                Container.Release(contragentBankDomain);
                Container.Release(protocolArticleLawDomain);
                Container.Release(protocolViolDomain);
                Container.Release(actCheckDomain);
                Container.Release(actCheckPeriodsDomain);
                Container.Release(docChildrenDomain);
                Container.Release(prescriptionViolDomain);
                Container.Release(violActRemovDomain);
                Container.Release(inspectionRealObjDomain);
            }
        }
    }
}