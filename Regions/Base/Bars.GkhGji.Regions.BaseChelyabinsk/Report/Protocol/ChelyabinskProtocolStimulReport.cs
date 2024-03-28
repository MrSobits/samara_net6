namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report.Protocol
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using B4.Modules.Reports;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Report;

    public class ChelyabinskProtocolStimulReport : GjiBaseStimulReport
    {
        public ChelyabinskProtocolStimulReport()
            : base(new ReportTemplateBinary(Resources.BlockGJI_ExecutiveDocProtocol_1))
        {
            ExportFormat = StiExportFormat.Word2007;
        }

        private long DocumentId { get; set; }

        public override string Id
        {
            get { return "Protocol"; }
        }

        public override string CodeForm
        {
            get { return "Protocol"; }
        }

        public override string Name
        {
            get { return "Протокол"; }
        }

        public override string Description
        {
            get { return "Протокол"; }
        }

        protected override string CodeTemplate { get; set; }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "BlockGJI_ExecutiveDocProtocol_1",
                            Name = "ProtocolGJI",
                            Description = "Любой случай",
                            Template = Resources.BlockGJI_ExecutiveDocProtocol_1
                        }
                };
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var protocolDomain = Container.ResolveDomain<Protocol>();
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
                var protocol = protocolDomain.Load(DocumentId);

                if (protocol == null)
                {
                    throw new ReportProviderException("Не удалось получить протокол");
                }

                // Заполняем общие поля
                FillCommonFields(protocol);

                var cultureInfo = new CultureInfo("ru-RU");

                CodeTemplate = "BlockGJI_ExecutiveDocProtocol_1";

                var disposal = GetParentDocument(protocol, TypeDocumentGji.Disposal);

                if (disposal != null)
                {
                    var disposalText = disposalTextServ.SubjectiveCase;
                    this.ReportParams["Id"] = this.DocumentId.ToString();
                    this.ReportParams["Распоряжение"] = string.Format(
                        "{0} №{1} от {2}",
                        disposalText,
                        disposal.DocumentNum,
                        disposal.DocumentDate.HasValue ? disposal.DocumentDate.Value.ToShortDateString() : "");

                    // если у распоряжения есть тип проверки с кодом 22
                    if (disposalTypeSurveyDomain.GetAll().Any(x => x.Disposal.Id == disposal.Id && x.TypeSurvey.Code == "22"))
                    {
                        CodeTemplate = "BlockGJI_ExecutiveDocProtocol_1";
                    }

                    var issuedDisposal = disposalDomain.GetAll().Where(x => x.Id == disposal.Id).Select(x => x.IssuedDisposal).FirstOrDefault();

                    if (issuedDisposal != null)
                    {
                        this.ReportParams["РуководительФИО"] = string.Format(
                            "{0} - {1}",
                            issuedDisposal.Position,
                            string.IsNullOrEmpty(issuedDisposal.ShortFio) ? issuedDisposal.Fio : issuedDisposal.ShortFio);


                        this.ReportParams["ДолжностьРуководителя"] = issuedDisposal.Code;
                    }

                    var queryInspectorId = docInspectorDomain.GetAll()
                                                         .Where(x => x.DocumentGji.Id == disposal.Id)
                                                         .Select(x => x.Inspector.Id);

                    var listLocality = zonInspInspectorDomain.GetAll()
                                        .Where(x => queryInspectorId.Contains(x.Inspector.Id))
                                        .Select(x => x.ZonalInspection.Locality)
                                        .Distinct()
                                        .ToList();

                    this.ReportParams["НаселПунктОтдела"] = string.Join("; ", listLocality);
                }

                this.ReportParams["ДатаПротокола"] = protocol.DocumentDate.HasValue
                                                                       ? protocol.DocumentDate.Value.ToString("D", cultureInfo)
                                                                       : string.Empty;

                this.ReportParams["Номер"] = protocol.DocumentNumber;

                this.ReportParams["РеквизитыФизЛица"] = protocol.PhysicalPersonInfo;

                this.ReportParams["ДатаРассмотрения"] = protocol.DateOfProceedings.HasValue
                    ? protocol.DateOfProceedings.Value.ToString("dd MMMM yyyy") : string.Empty;
                this.ReportParams["ВремРассмотрения"] = protocol.HourOfProceedings.ToString();

                var parentDoc = base.GetParentDocument(protocol, TypeDocumentGji.ActCheck) as ActCheck;

                if (parentDoc != null)
                {
                    this.ReportParams["Кв"] = parentDoc.Flat;
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

                        this.ReportParams["АдресКонтрагента"] = newAddr.ToString();
                    }
                    else
                    {
                        this.ReportParams["АдресКонтрагента"] = contragent.AddressOutsideSubject;
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
                        this.ReportParams["НаименованиеБанка"] = contragentBank.Name;
                        this.ReportParams["КорСчет"] = contragentBank.CorrAccount;
                        this.ReportParams["РасчетныйСчет"] = contragentBank.SettlementAccount;
                        this.ReportParams["БИК"] = contragentBank.Bik;
                    }

                    this.ReportParams["КПП"] = contragent.Kpp;
                    this.ReportParams["ИНН"] = contragent.Inn;
                    this.ReportParams["ДатаРегистрации"] = contragent.DateRegistration.HasValue ? contragent.DateRegistration.Value.ToShortDateString() : string.Empty;
                }

                this.ReportParams["СоставАдминПрав"] = protocol.Description;

                if (protocol.Executant != null)
                {
                    this.ReportParams["ТипИсполнителя"] = protocol.Executant.Code;
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
                            this.ReportParams["ФизЛицо"] = protocol.PhysicalPerson;
                            this.ReportParams["Реквизиты"] = protocol.PhysicalPersonInfo;
                            break;
                    }

                    if ((new List<string> { "0", "2", "4", "8", "9", "11", "15", "18" }).Contains(protocol.Executant.Code))
                    {
                        this.ReportParams["Местонахождение"] = protocol.Contragent != null
                                                                                 ? protocol.Contragent.JuridicalAddress
                                                                                 : string.Empty;
                    }
                    else
                        if ((new List<string> { "6", "7", "14" }).Contains(protocol.Executant.Code))
                    {
                        this.ReportParams["Местонахождение"] = protocol.PhysicalPersonInfo;
                    }
                }

                var protocolArticleLaws = protocolArticleLawDomain.GetAll()
                    .Where(x => x.Protocol.Id == protocol.Id)
                    .Select(x => new { x.ArticleLaw.Name, x.ArticleLaw.Description })
                    .ToList();

                this.ReportParams["СтатьяЗакона"] = protocolArticleLaws.Select(x => x.Name)
                             .AggregateWithSeparator(", ");

                this.ReportParams["Описание"] = protocolArticleLaws.Select(x => x.Description)
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
                    this.ReportParams["АдресОбъектаПравонарушения"] =
                        realityObjs.AggregateWithSeparator(x => x.Return(y => y.Address), "; ");

                    this.ReportParams["АдресОбъектаПравонарушенияПолный"] = realityObjs.Aggregate(
                        string.Empty,
                        (x, y) => x + (!string.IsNullOrEmpty(x)
                            ? "; " + y.FiasAddress.AddressName.Substring(y.FiasAddress.AddressName.IndexOf(',') + 1)
                            : y.FiasAddress.AddressName.Substring(y.FiasAddress.AddressName.IndexOf(',') + 2)));

                    this.ReportParams["Район"] = realityObjs[0].Municipality.Name;
                    this.ReportParams["НаселенныйПункт"] = realityObjs[0].FiasAddress.PlaceName;

                    if (realityObjs.Count() == 1)
                    {
                        var firstRoProtocol = realityObjs.FirstOrDefault();

                        if (firstRoProtocol != null && firstRoProtocol.FiasAddress != null)
                        {
                            this.ReportParams["Улица"] = firstRoProtocol.FiasAddress.StreetName;
                            this.ReportParams["Дом"] = firstRoProtocol.FiasAddress.House;
                            this.ReportParams["Корпус"] = firstRoProtocol.FiasAddress.Housing;
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
                    this.ReportParams["КодИнспектора"] = firstInspector.Position;
                    this.ReportParams["Инспектор"] = firstInspector.Fio;
                    this.ReportParams["ИнспекторФамИО"] = firstInspector.ShortFio;
                    this.ReportParams["ИнспекторФамИОТП"] = string.Format("{0} {1}", firstInspector.FioAblative?.Split(' ')[0], firstInspector.ShortFio.Substring(firstInspector.ShortFio.IndexOf(' ')));
                    this.ReportParams["ДолжностьИнсТП"] = firstInspector.PositionAblative;

                    this.ReportParams["ДолжностьИнспектора"] = firstInspector.Position;
                    this.ReportParams["ИнспекторФИОСокр"] = firstInspector.ShortFio;
                }

                if (listInspectors.Count > 0)
                {
                    this.ReportParams["ИнспекторДолжность(ТворП)"] =
                    listInspectors.AggregateWithSeparator(
                        x => string.Format("{0} - {1}", x.FioAblative, x.PositionAblative), ", ");

                    this.ReportParams["ДолжностьТворП"] = listInspectors.AggregateWithSeparator(x => x.PositionAblative, ", ");
                    this.ReportParams["ИнспекторТворП"] = listInspectors.AggregateWithSeparator(x => x.FioAblative, ", ");
                }

                if (protocol.Contragent != null)
                {
                    this.ReportParams["Контрагент"] = protocol.Contragent.Name;
                    this.ReportParams["УправОргСокр"] = protocol.Contragent.ShortName;
                    this.ReportParams["АдресКонтрагентаФакт"] = protocol.Contragent.FiasFactAddress != null ? protocol.Contragent.FiasFactAddress.AddressName : string.Empty;
                }

                var actDoc = GetParentDocument(protocol, TypeDocumentGji.ActCheck);

                if (actDoc != null)
                {
                    var act = actCheckDomain.Load(actDoc.Id);

                    this.ReportParams["Акт"] = string.Format(
                        "Акт №{0} от {1}",
                        act.DocumentNumber,
                        act.DocumentDate.HasValue ? act.DocumentDate.Value.ToShortDateString() : "");

                    this.ReportParams["НомерАктаПроверки"] = actDoc.DocumentNumber;
                    this.ReportParams["ДатаАктаПроверки"] = actDoc.DocumentDate.HasValue ? actDoc.DocumentDate.Value.ToShortDateString() : string.Empty;
                    this.ReportParams["ВремяСоставленияАкта"] =
                        actCheckPeriodsDomain.GetAll()
                            .Where(x => x.ActCheck.Id == act.Id)
                            .Select(x => new { x.DateCheck, x.DateEnd })
                            .OrderBy(x => x.DateCheck)
                            .ThenBy(x => x.DateEnd)
                            .ToArray()
                            .LastOrDefault(x => x.DateEnd.HasValue)
                            .Return(x => x.DateEnd.ToDateString("HH-mm"));
                    this.ReportParams["НомерАкта"] = actDoc.DocumentNumber;
                    this.ReportParams["ДатаАкта"] = actDoc.DocumentDate.HasValue ? actDoc.DocumentDate.Value.ToShortDateString() : string.Empty;
                }

                // проверяем создан ли протокол на основе предписания
                var basePrescription =
                    docChildrenDomain.GetAll()
                        .Where(x => x.Children.Id == protocol.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                        .Select(x => x.Parent)
                        .FirstOrDefault();

                if (basePrescription != null)
                {
                    this.ReportParams["ПредДокумент"] = string.Format(
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
                            this.ReportParams["ЗональноеНаименование1ГосЯзык"] = zonalInspection.BlankName;
                            this.ReportParams["ЗональноеНаименование2ГосЯзык"] = zonalInspection.BlankNameSecond;
                            this.ReportParams["АдресЗЖИ"] = zonalInspection.Address;
                        }
                    }
                }

                #region Секция нарушений
                var violations = protocolViolDomain.GetAll()
                                          .Where(x => x.Document.Id == protocol.Id)
                                          .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Нарушения",
                    MetaType = nameof(Object),
                    Data = violations.Select(
                        x => new
                        {
                            Наименование = x.InspectionViolation.Violation.Name,
                            ПИН = x.InspectionViolation.Violation.CodePin,
                            ПП170 = x.InspectionViolation.Violation.PpRf170,
                            ПП25 = x.InspectionViolation.Violation.PpRf25,
                            ПП307 = x.InspectionViolation.Violation.PpRf307,
                            ПП491 = x.InspectionViolation.Violation.PpRf491,
                            Прочее = x.InspectionViolation.Violation.OtherNormativeDocs,
                            ЖКРФ = x.InspectionViolation.Violation.GkRf
                        }).ToList()
                });

                var actRemovByViol = violActRemovDomain.GetAll()
                    .Select(x => new
                    {
                        x.ViolationGji.Id,
                        x.ActionsRemovViol.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Name));

                this.ReportParams["Мероприятие"] =
                    actRemovByViol.Where(x => violations.Any(y => y.InspectionViolation.Violation.Id == x.Key))
                        .Select(x => x.Value)
                        .SelectMany(x => x)
                        .Distinct()
                        .AggregateWithSeparator(", ");

                var linksViolations = new List<string>();
                this.ReportParams["СсылкиНаПунктыНормативныхАктов"] = string.Join(", ", linksViolations.Distinct().Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim()).ToArray());

                #endregion

                this.ReportParams["ДатаИВремяРассмотренияДела"] = string.Format("{0} в {1} час. {2} мин.",
                                                                                                protocol.DateOfProceedings != null
                                                                                                    ? protocol.DateOfProceedings.ToDateTime().ToShortDateString()
                                                                                                    : string.Empty,
                                                                                                protocol.HourOfProceedings,
                                                                                                protocol.MinuteOfProceedings);

                this.ReportParams["МестоСоставления"] = inspectionRealObjDomain.GetAll().Where(x => x.Inspection.Id == protocol.Inspection.Id)
                    .Select(x => x.RealityObject.Municipality.Name)
                    .FirstOrDefault();

                if (protocol.DocumentDate != null)
                {
                    this.ReportParams["Времясовершенияправонарушения"] = protocol.DocumentDate.ToDateTime().AddDays(-1).ToShortDateString();
                }


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
