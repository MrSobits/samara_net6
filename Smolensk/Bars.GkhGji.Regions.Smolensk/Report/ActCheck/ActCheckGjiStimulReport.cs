namespace Bars.GkhGji.Regions.Smolensk.Report.ActCheck
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    public class ActCheckGjiStimulReport : GjiBaseStimulReport
    {
        #region .ctor
        public ActCheckGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.ActCheck))
        {
        }
        #endregion

        #region Properties
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        public override string Id
        {
            get { return "ActCheck"; }
        }

        public override string CodeForm
        {
            get { return "ActCheck"; }
        }

        public override string Name
        {
            get { return "Акт проверки"; }
        }

        public override string Description
        {
            get { return "Акт проверки"; }
        }

        protected override string CodeTemplate { get; set; }
        #endregion

        #region Fields
        private long DocumentId { get; set; }
        #endregion

        #region DomainServices
        public IDomainService<ActCheckRealityObject> ActCheckRealObjDomain { get; set; }

        public IDomainService<DocumentViolGroup> DocumentViolGroupDomain { get; set; }

        public IDomainService<GkhGji.Entities.ActCheck> ActCheckDomain { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDomainService<Protocol> ProtocolDomain { get; set; }

        public IDomainService<DisposalTypeSurvey> TypeSurveyDomain { get; set; }
        #endregion

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
                    Code = "SmolenskActCheck",
                    Name = "SmolenskActCheck",
                    Description = "Акт проверки",
                    Template = Properties.Resources.ActCheck
                },

                new TemplateInfo
                {
                    Code = "SmolenskActCheckPlaned",
                    Name = "SmolenskActCheckPlaned",
                    Description = "Акт проверки если объект проверки Физ. лицо или ДЛ",
                    Template = Properties.Resources.ActCheckPlaned
                }
            };
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        private void GetCodeTemplate()
        {
            var act = ActCheckDomain.FirstOrDefault(x => x.Id == DocumentId);

            var personInspection = act == null ? PersonInspection.PhysPerson : act.Return(x => x.Inspection.PersonInspection, PersonInspection.PhysPerson);

            switch (personInspection)
            {
                case PersonInspection.PhysPerson:
                case PersonInspection.Official:
                    CodeTemplate = "SmolenskActCheckPlaned";
                    break;
                case PersonInspection.Organization:
                    CodeTemplate = "SmolenskActCheck";
                    break;
            }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var contragentContactDomain = Container.Resolve<IDomainService<ContragentContact>>();
            var actRoDomain = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var typeSurveyGoalDomain = Container.Resolve<IDomainService<TypeSurveyGoalInspGji>>();
            var actAnnexDomain = Container.Resolve<IDomainService<ActCheckAnnex>>();
            var disposalExpertDomain = Container.Resolve<IDomainService<DisposalExpert>>();
            var disposalProvidedDocDomain = this.Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var actCheckWitnessDomain = Container.Resolve<IDomainService<ActCheckWitness>>();
            var actCheckViolationDomain = this.Container.Resolve<IDomainService<ActCheckViolation>>();
            var actCheckPeriodDomain = Container.Resolve<IDomainService<ActCheckPeriod>>();
            var docInspDomain = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var zonalInspectorDomain = Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var actCheckRobjectDescriptionDomain = Container.ResolveDomain<ActCheckRealityObjectDescription>();
            var longTextDomain = Container.ResolveDomain<DocumentViolGroupLongText>();
            var violPiontDomain = Container.Resolve<IDomainService<DocumentViolGroupPoint>>();

            try
            {
                var act = ActCheckDomain.FirstOrDefault(x => x.Id == DocumentId);
                if (act == null)
                {
                    throw new ReportProviderException("Не удалось получить акт проверки");
                }
                FillCommonFields(act);

                Disposal disposal = null;

                var parentDisposal = GetParentDocument(act, TypeDocumentGji.Disposal);

                if (parentDisposal != null)
                {
                    disposal = DisposalDomain.GetAll().FirstOrDefault(x => x.Id == parentDisposal.Id);
                }

                Protocol protocol = null;

                var childProtocol = GetChildDocument(act, TypeDocumentGji.Protocol);

                if (childProtocol != null)
                {
                    protocol = ProtocolDomain.GetAll().FirstOrDefault(x => x.Id == childProtocol.Id);
                }

                var queryTypeSurveys = TypeSurveyDomain.GetAll().Where(x => x.Disposal.Id == disposal.Id);

                this.ReportParams["ДолжностьРук"] = string.Empty;

                this.ReportParams["ДатаАкта"] = act.DocumentDate.HasValue
                    ? act.DocumentDate.Value.ToString("d MMMM yyyy")
                    : string.Empty;
                this.ReportParams["НомерАкта"] = act.DocumentNumber;
                this.ReportParams["ПроверПлощадь"] = act.Area.HasValue ? act.Area.Value.ToStr() : string.Empty;

                // Такую фигню делаю чтобы мутить с датами поскольку данные будут на сервере
                var actCheckPeriods = actCheckPeriodDomain.GetAll()
                    .Where(x => x.ActCheck.Id == act.Id)
                    .Select(x => new ActCheckPeriodProxy
                    {
                        DateCheck = x.DateCheck,
                        DateStart = x.DateStart,
                        DateEnd = x.DateEnd
                    })
                    .OrderBy(x => x.DateCheck)
                    .ThenBy(x => x.DateEnd)
                    .ToList();

                var lastCheckDate = actCheckPeriods.LastOrDefault(x => x.DateEnd.HasValue);
                this.ReportParams["ВремяСоставленияАкта"] = lastCheckDate != null
                    ? lastCheckDate.DateEnd.Value.ToString("HH-mm")
                    : string.Empty;

                this.ReportParams["ДатаВремяПроверки"] = actCheckPeriods.Select(x =>
                {
                    var getTime = new Func<DateTime?, string>(
                        date => date.HasValue
                            ? date.Value.ToString("HH час. mm мин.")
                            : string.Empty);

                    var getIntervalTime = new Func<DateTime?, DateTime?, string>(
                        (dateStart, dateEnd) =>
                        {
                            if (!dateStart.HasValue || !dateEnd.HasValue)
                            {
                                return "0 час. 00 мин.";
                            }

                            if (dateStart.Value >= dateEnd.Value)
                            {
                                return "0 час. 00 мин.";
                            }

                            var time = dateEnd.Value - dateStart.Value;

                            return string.Format("{0} час. {1} мин.", time.Hours.ToString(), time.ToString("mm"));
                        });

                    return string.Format(
                        "{0} с {1} до {2} Продолжительность {3}",
                        x.DateCheck.HasValue ? x.DateCheck.Value.ToString("«dd» MMMM yyyy г.") : string.Empty,
                        getTime(x.DateStart),
                        getTime(x.DateEnd),
                        getIntervalTime(x.DateStart, x.DateEnd));
                })
                    .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

                this.DurationCheck(actCheckPeriods);

                this.ReportParams["ИнспекторыИКоды"] = docInspDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == DocumentId)
                    .Select(x => new { x.Inspector.Fio, x.Inspector.Code })
                    .ToList()
                    .Aggregate(
                        string.Empty,
                        (x, y) =>
                        {
                            if (!string.IsNullOrEmpty(x))
                            {
                                x += ", ";
                            }

                            x += y.Fio;

                            if (!string.IsNullOrEmpty(y.Code))
                            {
                                x += " - " + y.Code;
                            }

                            return x;
                        });

                this.ReportParams["ИнспекторыАкта"] = docInspDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == DocumentId)
                    .Select(x => new { x.Inspector.Fio, x.Inspector.Position })
                    .ToList()
                    .Aggregate(
                        string.Empty,
                        (x, y) =>
                        {
                            if (!string.IsNullOrEmpty(x))
                            {
                                x += ", ";
                            }

                            x += y.Fio;

                            if (!string.IsNullOrEmpty(y.Position))
                            {
                                x += " - " + y.Position;
                            }

                            return x;
                        });

                var inspectorIds = docInspDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == DocumentId)
                    .Select(x => x.Inspector.Id)
                    .ToList();

                if (inspectorIds.Any())
                {
                    var zonalInspection = zonalInspectorDomain.GetAll()
                        .Where(x => inspectorIds.Contains(x.Inspector.Id))
                        .Where(x => x.ZonalInspection != null
                                    && x.ZonalInspection.Name != string.Empty
                                    && x.ZonalInspection.Name != null)
                        .Select(x => x.ZonalInspection.Name)
                        .FirstOrDefault();

                    if (zonalInspection != null)
                    {
                        this.ReportParams["Отдел"] = zonalInspection;
                    }
                }

                if (act.Inspection.Contragent != null)
                {
                    var contragent = act.Inspection.Contragent;

                    this.ReportParams["УправОрг"] = contragent.Name;
                    this.ReportParams["ИННУправОрг"] = contragent.Inn;

                    if (contragent.OrganizationForm != null)
                    {
                        this.ReportParams["ТипЛица"] = contragent.OrganizationForm.Code != "91"
                            ? "юридического лица"
                            : "индивидуального предпринимателя";
                    }

                    this.ReportParams["УправОргРП"] = contragent.Name;
                    this.ReportParams["СокрУправОрг"] = contragent.ShortName;
                    this.ReportParams["ЮрАдресКонтрагента"] = contragent.JuridicalAddress;

                    var headContragent = contragentContactDomain.GetAll()
                        .Where(x => x.Contragent.Id == contragent.Id)
                        .Where(x => x.DateStartWork.HasValue)
                        .Where(x => x.DateStartWork.Value <= DateTime.Now)
                        .Where(x => !x.DateEndWork.HasValue || x.DateEndWork.Value >= DateTime.Now)
                        .FirstOrDefault(x => x.Position != null && (x.Position.Code == "1" || x.Position.Code == "4"));

                    if (headContragent != null)
                    {
                        this.ReportParams["РуководительЮЛДолжность"] = string.Format(
                            "{0} {1} {2} {3}",
                            headContragent.Position != null ? headContragent.Position.Name : string.Empty,
                            headContragent.Surname,
                            headContragent.Name,
                            headContragent.Patronymic);
                        this.ReportParams["РуководительЮЛ"] = string.Format(
                            "{0} {1} {2}",
                            headContragent.Surname,
                            headContragent.Name,
                            headContragent.Patronymic);
                        this.ReportParams["ДолжностьОрг"] = headContragent.Position != null
                            ? headContragent.Position.Name
                            : string.Empty;
                        this.ReportParams["ДолжностьОргДП"] = headContragent.Position != null
                            ? headContragent.Position.NameDative
                            : string.Empty;
                        this.ReportParams["ФИООрг"] = headContragent.FullName;
                    }

                    this.ReportParams["АдресКонтрагентаФакт"] = contragent.FiasFactAddress.Return(x => x.AddressName);
                }

                var queryTypeSurveyIds = queryTypeSurveys.Select(x => x.TypeSurvey.Id);
                var strGoalInspGji = typeSurveyGoalDomain.GetAll()
                    .Where(x => queryTypeSurveyIds.Contains(x.TypeSurvey.Id))
                    .Select(x => x.SurveyPurpose.Name)
                    .AsEnumerable()
                    .Aggregate(string.Empty, (t, n) => t + (!string.IsNullOrEmpty(t) ? ("; " + n) : n));

                this.ReportParams["Цель"] = strGoalInspGji;

                var actCheckAnnex = actAnnexDomain.GetAll()
                    .Where(x => x.ActCheck.Id == DocumentId)
                    .Select(x => x.Name)
                    .AsEnumerable()
                    .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

                this.ReportParams["ПрилагаемыеДокументы"] = actCheckAnnex;

                // Дома акта проверки
                var actCheckRealtyObjects = actRoDomain.GetAll()
                    .Where(x => x.ActCheck.Id == DocumentId)
                    .Select(x => new { x.Id, x.RealityObject, x.HaveViolation, x.Description, x.NotRevealedViolations })
                    .ToArray();

                var actCheckRealityObjectIds = actCheckRealtyObjects.Select(x => x.Id).ToArray();
                var descriptions = actCheckRobjectDescriptionDomain.GetAll()
                    .Where(x => actCheckRealityObjectIds.Contains(x.ActCheckRealityObject.Id))
                    .Select(x => new { x.ActCheckRealityObject.Id, x.Description })
                    .AsEnumerable()
                    .ToDictionary(x => x.Id, x => Encoding.UTF8.GetString(x.Description));

                var longTexts = longTextDomain.GetAll()
                    .Where(x => x.ViolGroup.Document.Id == DocumentId).ToArray()
                    .GroupBy(x => x.ViolGroup.Id).ToDictionary(x => x.Key, x => x.Select(y => new
                    {
                        y.Description,
                        y.Action
                    }).First());

                if (actCheckRealtyObjects.All(x => x.HaveViolation == YesNoNotSet.No))
                {
                    this.ReportParams["НеВыявлено"] = actCheckRealtyObjects
                        .Select(x => descriptions.Get(x.Id) ?? x.Description)
                        .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? "; " + y : y));
                }

                this.ReportParams["НевыявленныеНарушения"] = string.Join(", ",
                    actCheckRealtyObjects.Select(x => x.NotRevealedViolations).ToArray());

                var actRoList = actCheckRealtyObjects.Where(x => x.RealityObject != null).ToList();

                if (disposal != null)
                {
                    if (disposal.IssuedDisposal != null)
                    {
                        this.ReportParams["ДолжностьРукРП"] = disposal.IssuedDisposal.PositionGenitive;
                        this.ReportParams["ФИОРукРП"] = disposal.IssuedDisposal.FioGenitive;
                    }
                }

                if (CodeTemplate == "SmolenskActCheck")
                {
                    if (actRoList.Count > 0)
                    {
                        var firstRecord = actRoList.First();
                        var realtyObject = firstRecord.RealityObject;
                        if (realtyObject.FiasAddress != null)
                        {
                            this.ReportParams["НомерДома"] = realtyObject.FiasAddress.House;
                            this.ReportParams["АдресДома"] = realtyObject.FiasAddress.PlaceName + ", " +
                                                  realtyObject.FiasAddress.StreetName;
                            this.ReportParams["НаселенныйПункт"] = realtyObject.FiasAddress.PlaceName;
                        }

                        if (actRoList.Count == 1)
                        {
                            var housing = !string.IsNullOrEmpty(realtyObject.FiasAddress.Housing)
                                ? ", корп. " + realtyObject.FiasAddress.Housing
                                : string.Empty;

                            this.ReportParams["УлицаДом"] = string.Format(
                                "{0}, {1}, д.{2}{3} ",
                                realtyObject.FiasAddress.PlaceName,
                                realtyObject.FiasAddress.StreetName,
                                realtyObject.FiasAddress.House,
                                housing);
                            this.ReportParams["ДомаИАдреса"] = realtyObject.FiasAddress.AddressName;
                            this.ReportParams["Описание"] = descriptions.Get(firstRecord.Id) ?? firstRecord.Description;
                        }
                        else
                        {
                            var realObjs = new StringBuilder();

                            foreach (var realityObject in actRoList.Select(x => x.RealityObject))
                            {
                                if (realObjs.Length > 0)
                                {
                                    realObjs.Append("; ");
                                }

                                var housing = !string.IsNullOrEmpty(realtyObject.FiasAddress.Housing)
                                    ? ", корп. " + realtyObject.FiasAddress.Housing
                                    : string.Empty;

                                realObjs.AppendFormat(
                                    "{0}, д.{1}{2}",
                                    realityObject.FiasAddress.StreetName,
                                    realityObject.FiasAddress.House,
                                    housing);
                            }

                            this.ReportParams["УлицаДом"] = string.Format("{0}.", realObjs);
                            this.ReportParams["ДомаИАдреса"] = string.Format("{0}, {1}. ",
                                actRoList.FirstOrDefault().RealityObject.FiasAddress.PlaceName, realObjs);
                        }
                    }
                }
                else
                {
                    this.ReportParams["РеквизитыПротокола"] = protocol != null
                        ? string.Format(
                            "составлен №{0} от {1}",
                            protocol.DocumentNumber,
                            protocol.DocumentDate.HasValue
                                ? protocol.DocumentDate.Value.ToShortDateString()
                                : string.Empty)
                        : "не составлен";

                    var protocolObj = string.Empty;

                    if (protocol != null)
                    {
                        var listTypeContragent = new List<string> { "0", "9", "11", "8", "15", "18", "4", "2" };
                        var listTypeContrPhysicalPerson = new List<string> { "1", "10", "12", "13", "16", "19", "5", "3" };
                        var listTypePhysicalPerson = new List<string> { "6", "7", "14" };

                        var executant = protocol.Return(x => x.Executant);

                        if (listTypeContragent.Contains(executant.Return(x => x.Code)))
                        {
                            protocolObj = protocol.Contragent.Name;
                        }
                        else if (listTypeContrPhysicalPerson.Contains(executant.Return(x => x.Code)))
                        {
                            protocolObj = protocol.Contragent.Name + ", " + protocol.PhysicalPerson;
                        }
                        else if (listTypePhysicalPerson.Contains(executant.Return(x => x.Code)))
                        {
                            protocolObj = protocol.PhysicalPerson;
                        }
                    }

                    this.ReportParams["НаКогоСоставлен"] = protocolObj;

                    // Секция
                    var houseList = new List<House>();

                    if (actRoList.Count > 0)
                    {
                        houseList.AddRange(actRoList.Select(x => x.RealityObject).Select(realtyObject => new House
                        {
                            ТипДома = realtyObject.TypeHouse.GetEnumMeta().Display,
                            АдресДома =
                                realtyObject.FiasAddress != null
                                    ? realtyObject.FiasAddress.PlaceName + ", " + realtyObject.FiasAddress.StreetName
                                    : string.Empty,
                            НомерДома = realtyObject.FiasAddress != null ? realtyObject.FiasAddress.House : string.Empty,
                            ФормаСобственности =
                                realtyObject.TypeOwnership != null ? realtyObject.TypeOwnership.Name : string.Empty,
                            ХарактеристикаДома = string.Format(
                                "год постройки {0}, этажность {1}, материал стен {2}, кровли {3}",
                                realtyObject.BuildYear != null ? realtyObject.BuildYear.ToString() : string.Empty,
                                realtyObject.MaximumFloors != null
                                    ? realtyObject.MaximumFloors.ToString()
                                    : string.Empty,
                                realtyObject.WallMaterial != null ? realtyObject.WallMaterial.Name : string.Empty,
                                realtyObject.RoofingMaterial != null ? realtyObject.RoofingMaterial.Name : string.Empty)
                        }));

                        this.DataSources.Add(new MetaData
                        {
                            SourceName = "Дома",
                            MetaType = nameof(House),
                            Data = houseList
                        });
                    }
                }

                var sum = actRoList.Select(x => x.RealityObject.AreaMkd).Sum();
                if (sum.HasValue)
                {
                    this.ReportParams["ОбщаяПлощадьСумма"] = sum.Value.ToString();
                }

                var listDisposalProvDocs = new List<DispProvidedDoc>();

                if (disposal != null)
                {
                    this.ReportParams["ДатаРаспоряжения"] = disposal.DocumentDate.HasValue
                        ? disposal.DocumentDate.Value.ToString("d MMMM yyyy")
                        : string.Empty;

                    this.ReportParams["НомерРаспоряжения"] = disposal.DocumentNumber;

                    if (disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.NotRequiresAgreement)
                    {
                        this.ReportParams["ДатаИНомерРешенияПрокурора"] = "Не требуется";
                    }

                    var kindCheckName = disposal.KindCheck != null ? disposal.KindCheck.Name : string.Empty;
                    this.ReportParams["ВидПроверки"] = kindCheckName;
                    this.ReportParams["ВидОбследования"] = kindCheckName;

                    if (disposal.IssuedDisposal != null)
                    {
                        if (!string.IsNullOrEmpty(disposal.IssuedDisposal.PositionGenitive))
                        {
                            this.ReportParams["КодИнспектораРП"] = disposal.IssuedDisposal.PositionGenitive.ToLower();
                            this.ReportParams["КодИнспектораТП"] = disposal.IssuedDisposal.PositionAblative.ToLower();
                        }

                        this.ReportParams["ДолжностьРукРП"] = disposal.IssuedDisposal.PositionGenitive;
                        this.ReportParams["ФИОРукРП"] = disposal.IssuedDisposal.FioGenitive;
                        this.ReportParams["РуководительТП"] = disposal.IssuedDisposal.FioAblative;
                    }

                    this.ReportParams["ВидОбследованияРП"] = GetTypeCheckAblative(disposal.KindCheck);

                    if (disposal.DateStart.HasValue && disposal.DateEnd.HasValue)
                    {
                        var startDate = disposal.DateStart;

                        int countDays = 0;

                        while (startDate.Value.Date != disposal.DateEnd.Value.Date)
                        {
                            if (startDate.Value.DayOfWeek != DayOfWeek.Sunday &&
                                startDate.Value.DayOfWeek != DayOfWeek.Saturday)
                            {
                                countDays++;
                            }

                            startDate = startDate.Value.AddDays(1);
                        }

                        this.ReportParams["ПродолжительностьПроверки"] = countDays.ToString();
                    }

                    this.ReportParams["Эксперты"] = disposalExpertDomain.GetAll()
                        .Where(x => x.Disposal.Id == disposal.Id)
                        .Select(x => x.Expert)
                        .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Name : y.Name));

                    if (disposal.IssuedDisposal != null)
                    {
                        this.ReportParams["КодРуководителяФИО"] = string.Format(
                            "{0} - {1}",
                            disposal.IssuedDisposal.Code,
                            string.IsNullOrEmpty(disposal.IssuedDisposal.ShortFio)
                                ? disposal.IssuedDisposal.Fio
                                : disposal.IssuedDisposal.ShortFio);
                    }

                    this.ReportParams["НачалоПериода"] = disposal.DateStart.HasValue
                        ? disposal.DateStart.Value.ToShortDateString()
                        : string.Empty;
                    this.ReportParams["ОкончаниеПериода"] = disposal.DateEnd.HasValue
                        ? disposal.DateEnd.Value.ToShortDateString()
                        : string.Empty;

                    listDisposalProvDocs.AddRange(disposalProvidedDocDomain.GetAll()
                        .Where(x => x.Disposal.Id == disposal.Id)
                        .Select(x => new DispProvidedDoc
                        {
                            Наименование = x.ProvidedDoc.Name
                        })
                        .ToList());

                    this.DataSources.Add(new MetaData
                    {
                        SourceName = "ПредоставляемыеДокументы",
                        MetaType = nameof(DispProvidedDoc),
                        Data = listDisposalProvDocs
                    });

                    // через запятую выводим все предоставляемые документы
                    this.ReportParams["ПредоставляемыеДокументыСтрокой"] = listDisposalProvDocs
                        .Select(x => x.Наименование)
                        .AsEnumerable()
                        .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));
                }

                // Не используется для Смоленска

                //var actProvidedDocs = actCheckProvidedDocDomain.GetAll()
                //        .Where(x => x.ActCheck.Id == act.Id)
                //        .Select(x => new ActProvidedDoc
                //        {
                //            Наименование = x.ProvidedDoc.Name
                //        })
                //        .ToList();

                //var listProvDocs = actProvidedDocs.Where(x => listDisposalProvDocs.Any(y => y.Наименование == x.Наименование)).ToList();

                //Report.RegData("ПредоставленныеДокументы", listProvDocs);

                //// через запятую выводим все предоставленные документы
                //Report["ПредоставленныеДокументыСтрокой"] = listProvDocs.Select(x => x.Наименование).AsEnumerable().Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

                //var actOtherProvidedDocs = actProvidedDocs.Where(x => !listDisposalProvDocs.Any(y => y.Наименование == x.Наименование)).ToList();

                //Report.RegData("ДопПредоставленныеДокументы", actOtherProvidedDocs);

                //// через запятую выводим все предоставленные документы
                //Report["ДопПредоставленныеДокументыСтрокой"] = actOtherProvidedDocs.Select(x => x.Наименование).AsEnumerable().Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

                var witness = actCheckWitnessDomain.GetAll()
                    .Where(x => x.ActCheck.Id == DocumentId)
                    .Select(x => new { x.Fio, x.Position, x.IsFamiliar })
                    .ToArray();

                var allWitness = witness
                    .Aggregate(string.Empty,
                        (x, y) =>
                            x +
                            (!string.IsNullOrEmpty(x) ? ", " + y.Position + " - " + y.Fio : y.Position + " - " + y.Fio));
                var witnessIsFamiliar = witness
                    .Where(x => x.IsFamiliar)
                    .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Fio : y.Fio));

                this.ReportParams["ДЛприПроверке"] = allWitness;
                this.ReportParams["Ознакомлен"] = !string.IsNullOrEmpty(witnessIsFamiliar)
                    ? witnessIsFamiliar.TrimEnd(new[] { ',', ' ' })
                    : string.Empty;
                this.ReportParams["ЧислоЛицПривлеченныеЛица"] = witness.Length > 0 ? "Лица, привлеченные" : "Лицо, привлеченное";

                // получаем ссылки на статьи
                var violList = actCheckViolationDomain.GetAll()
                    .Where(x => x.ActObject.ActCheck.Id == DocumentId)
                    .Select(
                        x => new
                        {
                            x.InspectionViolation.Violation.Id,
                            x.InspectionViolation.Violation.PpRf25,
                            x.InspectionViolation.Violation.PpRf170,
                            x.InspectionViolation.Violation.PpRf307,
                            x.InspectionViolation.Violation.PpRf491,
                            x.InspectionViolation.Violation.OtherNormativeDocs
                        })
                    .ToList();
                var linksViolation = string.Empty;

                foreach (var link in violList)
                {
                    linksViolation += string.Format(
                        "{0},{1},{2},{3},{4},",
                        link.PpRf25,
                        link.PpRf170,
                        link.PpRf307,
                        link.PpRf491,
                        link.OtherNormativeDocs);
                }

                linksViolation = linksViolation.Split(',')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

                this.ReportParams["СсылкиНаПунктыНормативныхАктов"] = linksViolation;

                var actCheckViolInfoQuery = ActCheckRealObjDomain.GetAll().Where(x => x.ActCheck.Id == DocumentId);

                var actCheckViolInfo = actCheckViolInfoQuery
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        TypeHouse = x.RealityObject != null
                            ? x.RealityObject.TypeHouse
                            : 0,
                        Address = x.RealityObject != null
                            ? string.Format(
                                "№{0}{1}/{2} по {3} {4}",
                                x.RealityObject.FiasAddress.House,
                                x.RealityObject.FiasAddress.Letter,
                                x.RealityObject.FiasAddress.Housing,
                                x.RealityObject.FiasAddress.StreetName,
                                x.RealityObject.FiasAddress.PlaceName)
                            : string.Empty,
                        WallMaterial = x.RealityObject != null
                            ? x.RealityObject.WallMaterial != null
                                ? x.RealityObject.WallMaterial.Name
                                : string.Empty
                            : string.Empty,
                        NumberEntrances = x.RealityObject != null
                            ? x.RealityObject.NumberEntrances
                            : null,
                        Floors = x.RealityObject != null
                            ? x.RealityObject.Floors
                            : null,
                        TypeRoof = x.RealityObject != null
                            ? x.RealityObject.TypeRoof.GetEnumMeta().Display
                            : string.Empty,
                        Description = descriptions.Get(x.Id) ?? x.Description,
                        AreaMkd = x.RealityObject != null
                            ? x.RealityObject.AreaMkd.HasValue
                                ? decimal.Round(x.RealityObject.AreaMkd.Value, 2)
                                : 0m
                            : 0m,
                        RealtyId = x.RealityObject != null ? x.RealityObject.Id : 0
                    })
                    .ToList();

                var docViolGroupNoRealty =
                    DocumentViolGroupDomain.GetAll()
                        .Where(x => x.RealityObject == null)
                        .Where(x => x.Document.Id == DocumentId);

                var docViolGroup =
                    DocumentViolGroupDomain.GetAll()
                        .Where(x => actCheckViolInfoQuery.Select(y => y.RealityObject.Id).Contains(x.RealityObject.Id))
                        .Where(x => x.Document.Id == DocumentId)
                        .ToArray()
                        .GroupBy(x => x.RealityObject.Id)
                        .ToDictionary(x => x.Key);

                var actCheckViols = actCheckViolationDomain.GetAll()
                    .Where(x => actCheckViolInfoQuery.Select(y => y.Id).Contains(x.ActObject.Id)).ToArray();

                var gjiViolsTexts = actCheckViols.GroupBy(x => x.ActObject.Id).
                    ToDictionary(x => x.Key,
                        x => x.Select(v => v.InspectionViolation.Violation.Name).AggregateWithSeparator(", "));

                var violWithoutRealty = actCheckViols.FirstOrDefault(x => x.ActObject.RealityObject == null);

                RegJurPersonViolCheckResult(violWithoutRealty);

                if (actCheckViolInfoQuery.Count() == 1 && actCheckViolInfoQuery.FirstOrDefault().RealityObject == null)
                {
                    this.ReportParams["Чекбокс"] = "0";
                }
                else if (!actCheckViolInfoQuery.Any())
                {
                    this.ReportParams["Чекбокс"] = "1";
                }
                else
                {
                    this.ReportParams["Чекбокс"] = "2";
                }

                this.ReportParams["ОбщаяПлощадь"] = actCheckViolInfo.Any()
                    ? actCheckViolInfo.SafeSum(x => x.AreaMkd).ToStr()
                    : string.Empty;

                var violDescr = new List<ActCheckViolationRecord>();
                var docViols =
                    DocumentViolGroupDomain.GetAll()
                        .Where(x => actCheckViolInfoQuery.Select(y => y.RealityObject.Id).Contains(x.RealityObject.Id))
                        .Where(x => x.Document.Id == DocumentId)
                        .Select(x => new
                                     {
                                         x.Id,
                                         x.Description
                                     })
                        .ToList();

                var violPoints =
                    violPiontDomain.GetAll()
                                   .Where(x => x.ViolGroup.Document.Id == DocumentId)
                                   .Select(
                                       x =>
                                       new
                                       {
                                           x.ViolStage.InspectionViolation.Violation.CodePin,
                                           ViolStageId = x.ViolStage.Id,
                                           violGroupId = x.ViolGroup.Id
                                       })
                                   .AsEnumerable()
                                   .GroupBy(x => x.violGroupId)
                                   .ToDictionary(
                                        x => x.Key,
                                        y => new
                                        {
                                            PointCodes = y.Select(z => z.CodePin)
                                                        .Aggregate((str, result) => !string.IsNullOrEmpty(result) ? result + "," + str : str)
                                        });

                foreach (var viol in docViols)
                {
                    var record = new ActCheckViolationRecord();
                    
                    record.ТекстНарушения = longTexts.ContainsKey(viol.Id) && longTexts[viol.Id].Description != null
                            ? Encoding.UTF8.GetString(longTexts[viol.Id].Description).ToString(CultureInfo.GetCultureInfo("ru-RU"))
                            : viol.Description;

                    record.Пункт = violPoints.ContainsKey(viol.Id) ? violPoints[viol.Id].PointCodes : string.Empty;

                    violDescr.Add(record);
                }

                this.DataSources.Add(new MetaData
                {
                    SourceName = "НарушенияОписание",
                    MetaType = nameof(ActCheckViolationRecord),
                    Data = violDescr
                });

                var i = 0;
                var dataViol = new List<ActCheckViolationRecord>();

                foreach (var viol in actCheckViolInfo)
                {
                    var record = new ActCheckViolationRecord
                    {
                        НомерПп = ++i,
                        АдресДома = !viol.Address.IsEmpty()
                            ? viol.Address
                            : string.Empty,
                        МатериалСтен = !viol.WallMaterial.IsEmpty()
                            ? "материал стен: " + viol.WallMaterial + ", "
                            : string.Empty,
                        КоличествоПодъездов = viol.NumberEntrances.HasValue
                            ? "количество подъездов: " + viol.NumberEntrances.Value.ToStr() + ", "
                            : string.Empty,
                        КоличествоЭтажей = viol.Floors.HasValue
                            ? viol.Floors.Value.ToStr()
                            : string.Empty,
                        ТипКровли = !viol.TypeRoof.IsEmpty()
                            ? "тип кровли: " + viol.TypeRoof
                            : string.Empty
                    };

                    if (viol.RealtyId > 0)
                    {
                        record.ТекстНарушения = record.ОписаниеНарушений =
                            docViolGroup.ContainsKey(viol.RealtyId)
                                ? docViolGroup[viol.RealtyId].AggregateWithSeparator(x => x.Description, ", \n")
                                : string.Empty;

                        var violIds = docViolGroup.ContainsKey(viol.RealtyId)
                            ? docViolGroup[viol.RealtyId].Select(x => x.Id).ToArray()
                            : new long[0];

                        var result = new StringBuilder();

                        foreach (var violId in violIds)
                        {
                            var descr = longTexts.ContainsKey(violId) && longTexts[violId].Description != null
                                ? Encoding.UTF8.GetString(longTexts[violId].Description).ToString(CultureInfo.GetCultureInfo("ru-RU"))
                                : viol.Description;
                            if (!string.IsNullOrWhiteSpace(descr))
                            {
                                if (result.Length > 0)
                                {
                                    result.Append(", \n");
                                }
                                
                                result.Append(descr);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(result.ToString())) record.ТекстНарушения = record.ОписаниеНарушений = result.ToString();
                    }
                    else
                    {
                        record.ТекстНарушения =
                            record.ОписаниеНарушений = docViolGroupNoRealty.AggregateWithSeparator(x => x.Description, ", \n");

                        var violIds = docViolGroupNoRealty.Select(x => x.Id).ToArray();

                        var result = new StringBuilder();

                        foreach (var violId in violIds)
                        {
                            var descr = longTexts.ContainsKey(violId) && longTexts[violId].Description != null
                                ? Encoding.UTF8.GetString(longTexts[violId].Description).ToString(CultureInfo.GetCultureInfo("ru-RU"))
                                : viol.Description;
                            if (!string.IsNullOrWhiteSpace(descr))
                            {
                                if (result.Length > 0)
                                {
                                    result.Append(", \n");
                                }

                                result.Append(descr);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(result.ToString())) record.ТекстНарушения = record.ОписаниеНарушений = result.ToString();
                    }

                    if (viol.TypeHouse == TypeHouse.BlockedBuilding
                        || viol.TypeHouse == TypeHouse.Individual
                        || viol.TypeHouse == TypeHouse.ManyApartments)
                    {
                        record.ТипДома = viol.TypeHouse.GetEnumMeta().Display + " дом";
                    }
                    else if (viol.TypeHouse == TypeHouse.SocialBehavior)
                    {
                        record.ТипДома = viol.TypeHouse.GetEnumMeta().Display;
                    }
                    else
                    {
                        record.ТипДома = string.Empty;
                    }

                    dataViol.Add(record);
                }

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Нарушения",
                    MetaType = nameof(ActCheckViolationRecord),
                    Data = dataViol
                });
            }
            finally
            {
                Container.Release(contragentContactDomain);
                Container.Release(actRoDomain);
                Container.Release(typeSurveyGoalDomain);
                Container.Release(actAnnexDomain);
                Container.Release(disposalExpertDomain);
                Container.Release(disposalProvidedDocDomain);
                Container.Release(actCheckWitnessDomain);
                Container.Release(actCheckViolationDomain);
                Container.Release(actCheckPeriodDomain);
                Container.Release(docInspDomain);
                Container.Release(zonalInspectorDomain);
                Container.Release(actCheckRobjectDescriptionDomain);
                Container.Release(longTextDomain);
                Container.Release(violPiontDomain);
            }
        }

        private void RegJurPersonViolCheckResult(ActCheckViolation violWithoutRealty)
        {
            var result = new РезультатПроверкиЮЛ();

            if (violWithoutRealty == null)
            {
                result.Вступление = 0;
                this.DataSources.Add(new MetaData
                {
                    SourceName = "РезультатПроверкиЮЛ",
                    MetaType = nameof(РезультатПроверкиЮЛ),
                    Data = result
                });
                return;
            }

            var domain = Container.ResolveDomain<ViolationNormativeDocItemGji>();
            var longTextDomain = Container.ResolveDomain<DocumentViolGroupLongText>();
            using (Container.Using(domain, longTextDomain))
            {
                string description;
                var longTexts = longTextDomain.GetAll().Where(x => x.ViolGroup.Document.Id == DocumentId).ToList();
                if (longTexts.Any())
                {
                    description = longTexts.AggregateWithSeparator(x => Encoding.UTF8.GetString(x.Description), "\n");
                }
                else
                {
                    var docViolGroups = DocumentViolGroupDomain.GetAll().Where(x => x.Document.Id == DocumentId).ToList();
                    description = docViolGroups.AggregateWithSeparator(x => x.Description, ";\n");
                }

                result.ОписаниеНарушенияЮЛ = description;
                try
                {
                    var normativeDocs =
                        domain.GetAll()
                            .Where(x => x.ViolationGji.Id == violWithoutRealty.InspectionViolation.Violation.Id)
                            .Select(x => x.NormativeDocItem.NormativeDoc)
                            .ToList();
                    if (normativeDocs.Any() && normativeDocs.All(x => x.Code == 731))
                    {
                        result.Вступление = 1;
                    }
                }
                catch
                {
                    // ignored
                }

                this.DataSources.Add(new MetaData
                {
                    SourceName = "РезультатПроверкиЮЛ",
                    MetaType = nameof(РезультатПроверкиЮЛ),
                    Data = result
                });
            }
        }

        /// <summary>
        /// Добавляет источник ДатаВремяПровПроверки и формирует общую продолжительность проверки.
        /// </summary>
        /// <remarks>
        /// Общая продожительность проверки выводится в виде "1 д., 4 ч., 42 мин." (если затрачено 12ч 42мин)
        /// То есть 1 день = 8 часов
        /// </remarks>
        /// <param name="actCheckPeriods">
        /// The act check periods.
        /// </param>
        private void DurationCheck(List<ActCheckPeriodProxy> actCheckPeriods)
        {
            var ci = new System.Globalization.CultureInfo("ru-RU");
            var dtfi = ci.DateTimeFormat.MonthGenitiveNames;

            var dateTimeVerification = actCheckPeriods
                .Where(x => x.DateCheck.HasValue && x.DateStart.HasValue && x.DateEnd.HasValue)
                .Select(x => new
                            {
                                ВремяПроверки = string.Format(
                                    "\"{0}\" {1} {2} г. с {3} час. {4:D2} мин. до {5} час. {6:D2} мин. Продолжительность {7:D2} ч. {8:D2} мин.",
                                    x.DateCheck.Value.Day,
                                    dtfi[x.DateCheck.Value.Month],
                                    x.DateCheck.Value.Year,
                                    x.DateStart.Value.Hour,
                                    x.DateStart.Value.Minute,
                                    x.DateEnd.Value.Hour,
                                    x.DateEnd.Value.Minute,
                                    (x.DateEnd.Value - x.DateStart.Value).Hours,
                                    (x.DateEnd.Value - x.DateStart.Value).Minutes)
                            });


            this.DataSources.Add(new MetaData
            {
                SourceName = "ДатаВремяПровПроверки",
                MetaType = nameof(Object),
                Data = dateTimeVerification.ToList()
            });

            var periodTimeSum = new TimeSpan();

            foreach (var actCheckPeriod in actCheckPeriods)
            {
                if (actCheckPeriod.DateStart.HasValue && actCheckPeriod.DateEnd.HasValue)
                {
                    periodTimeSum += actCheckPeriod.DateEnd.Value - actCheckPeriod.DateStart.Value;
                }
            }

            var duration = new StringBuilder();
            var separator = string.Empty;

            var countDays = periodTimeSum.Hours / 8;
            if (countDays > 0)
            {
                duration.AppendFormat("{0} д.", countDays);
                separator = ", ";
            }

            var countHours = periodTimeSum.Hours % 8;
            if (countHours > 0)
            {
                duration.AppendFormat("{0}{1:D2} ч.", separator, countHours);
                separator = ", ";
            }

            var countMinutes = periodTimeSum.Minutes;
            if (countMinutes > 0)
            {
                duration.AppendFormat("{0}{1:D2} мин", separator, countMinutes);
            }

            this.ReportParams["ПродолжительностьПроверкиДЧ"] = duration.ToString();
        }

        protected class ActCheckViolationRecord
        {
            public int НомерПп { get; set; }
            public string ТипДома { get; set; }
            public string АдресДома { get; set; }
            public string МатериалСтен { get; set; }
            public string КоличествоПодъездов { get; set; }
            public string КоличествоЭтажей { get; set; }
            public string ТипКровли { get; set; }
            public string ОписаниеНарушений { get; set; }
            public string ТекстНарушения { get; set; }
            public string Пункт { get; set; }
        }

        private string GetTypeCheckAblative(KindCheckGji kindCheck)
        {
            var result = string.Empty;

            var dictTypeCheckAblative = new Dictionary<TypeCheck, string>
                                            {
                                                { TypeCheck.PlannedExit, "плановой выездной" },
                                                { TypeCheck.NotPlannedExit, "внеплановой выездной" },
                                                { TypeCheck.PlannedDocumentation, "плановой документарной" },
                                                { TypeCheck.NotPlannedDocumentation, "внеплановой документарной" },
                                                { TypeCheck.InspectionSurvey, "внеплановой выездной" },
                                                { TypeCheck.PlannedDocumentationExit, "плановой документарной и выездной" },
                                                { TypeCheck.VisualSurvey, "о внеплановой проверке технического состояния жилого помещения" },
                                                { TypeCheck.NotPlannedDocumentationExit, "внеплановой документарной и выездной" }
                                            };
            if (kindCheck != null)
            {
                result = dictTypeCheckAblative[kindCheck.Code];
            }

            return result;
        }

        public DocumentGji GetParentDocument(DocumentGji document, TypeDocumentGji type)
        {
            var childrenService = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            try
            {
                var result = document;

                if (document.TypeDocumentGji != type)
                {
                    var docs = childrenService.GetAll()
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
            finally
            {
                Container.Release(childrenService);
            }
        }

        protected DocumentGji GetChildDocument(DocumentGji document, TypeDocumentGji seachingTypeDoc)
        {
            var childrenService = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            try
            {
                var result = document;

                if (document.TypeDocumentGji != seachingTypeDoc)
                {
                    var docs = childrenService.GetAll()
                        .Where(x => x.Parent.Id == document.Id)
                        .Select(x => x.Children)
                        .ToList();

                    foreach (var doc in docs)
                    {
                        result = GetChildDocument(doc, seachingTypeDoc);
                        if (result != null)
                        {
                            break;
                        }
                    }
                }

                if (result != null)
                {
                    return result.TypeDocumentGji == seachingTypeDoc ? result : null;
                }

                return null;
            }
            finally
            {
                Container.Release(childrenService);
            }
        }

        private class DispProvidedDoc
        {
            public string Наименование { get; set; }
        }

        private class House
        {
            public string ТипДома { get; set; }
            public string АдресДома { get; set; }
            public string НомерДома { get; set; }
            public string ФормаСобственности { get; set; }
            public string ХарактеристикаДома { get; set; }
        }

        private class ActCheckPeriodProxy
        {
            /// <summary>
            /// Дата.
            /// </summary>
            public DateTime? DateCheck { get; set; }

            /// <summary>
            /// Дата начала.
            /// </summary>
            public DateTime? DateStart { get; set; }

            /// <summary>
            /// Дата окончания.
            /// </summary>
            public DateTime? DateEnd { get; set; }
        }

        private class РезультатПроверкиЮЛ
        {
            public РезультатПроверкиЮЛ()
            {
                Вступление = 2;
                ОписаниеНарушенияЮЛ = string.Empty;
            }

            public РезультатПроверкиЮЛ(int v)
                : this()
            {
                Вступление = v;
            }

            /// <summary>
            /// 0 - если нет нарушения, не привязанного к дому
            /// 1 - если нарушение ссылается только на нормативный документ с кодом 731
            /// 2 - в остальных случаях
            /// </summary>
            public int Вступление { get; set; }

            public string ОписаниеНарушенияЮЛ { get; set; }
        }
    }
}
