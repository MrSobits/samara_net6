namespace Bars.GkhGji.Regions.Smolensk.Report.ActRemoval
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using B4;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    using Gkh.Entities;
    using Gkh.Enums;
    using Gkh.Report;
    using GkhGji.Entities;
    using GkhGji.Enums;
    using Gkh.Utils;

    public class ActRemovalStimulReport : GjiBaseStimulReport
    {
        private long DocumentId { get; set; }

        public ActRemovalStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.ActRemoval))
        {
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        public override string Id
        {
            get { return "ActRemoval"; }
        }

        public override string CodeForm
        {
            get { return "ActRemoval"; }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            if (CodeTemplate == "SmolenskActRemovalJurPerson")
            {

                PrepareJurPersonReport();
            }
            else
            {
                PrepareReport();
            }
        }

        public override string Name
        {
            get { return "Акт проверки предписания"; }
        }

        public override string Description
        {
            get { return "Акт проверки предписания"; }
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
                    Code = "SmolenskActRemoval",
                    Name = "SmolenskActRemoval",
                    Description = "Акт проверки предписания для физ и ДЛ",
                    Template = Properties.Resources.ActRemoval
                },
                new TemplateInfo
                {
                    Code = "SmolenskActRemovalJurPerson",
                    Name = "SmolenskActRemovalJurPerson",
                    Description = "Акт проверки предписания для Юр лица",
                    Template = Properties.Resources.ActCheck
                }
            };
        }

        public override Stream GetTemplate()
        {
            GetCodeTemplate();
            return base.GetTemplate();
        }

        private void GetCodeTemplate()
        {
            var actRemovalDomain = Container.Resolve<IDomainService<ActRemoval>>();
            var actRemoval = actRemovalDomain.FirstOrDefault(x => x.Id == DocumentId);

            var personInspection = actRemoval == null
                ? PersonInspection.PhysPerson
                : actRemoval.Return(x => x.Inspection.PersonInspection, PersonInspection.PhysPerson);

            switch (personInspection)
            {
                case PersonInspection.PhysPerson:
                case PersonInspection.Official:
                    CodeTemplate = "SmolenskActRemoval";
                    break;
                case PersonInspection.Organization:
                    CodeTemplate = "SmolenskActRemovalJurPerson";
                    break;
            }
        }



        private void PrepareReport()
        {
            var actCheckDomain = Container.Resolve<IDomainService<ActCheck>>();
            var actRemovalDomain = Container.Resolve<IDomainService<ActRemoval>>();
            var disposalDomain = Container.Resolve<IDomainService<Disposal>>();
            var prescriptionDomain = Container.Resolve<IDomainService<Prescription>>();
            var contragentContactDomain = Container.Resolve<IDomainService<ContragentContact>>();
            var disposalProvidedDocDomain = Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var actCheckWitnessDomain = Container.Resolve<IDomainService<ActCheckWitness>>();
            var violStageDomain = Container.Resolve<IDomainService<InspectionGjiViolStage>>();
            var zonalInspDomain = Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var docInspDomain = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var actCheckPeriodDomain = Container.Resolve<IDomainService<ActCheckPeriod>>();

            try
            {
                var actRemoval = actRemovalDomain.FirstOrDefault(x => x.Id == DocumentId);
                if (actRemoval == null)
                {
                    throw new ReportProviderException("Не удалось получить акт проверки предписания");
                }
                FillCommonFields(actRemoval);

                Disposal disposal = null;
                ActCheck act = null;
                Prescription prescription = null;

                var parentDisposal = GetParentDocument(actRemoval, TypeDocumentGji.Disposal);

                if (parentDisposal != null)
                {
                    disposal = disposalDomain.GetAll().FirstOrDefault(x => x.Id == parentDisposal.Id);
                }

                var parentAct = GetParentDocument(actRemoval, TypeDocumentGji.ActCheck);

                if (parentAct != null)
                {
                    act = actCheckDomain.GetAll().FirstOrDefault(x => x.Id == parentAct.Id);
                }

                var parentPrescription = GetParentDocument(actRemoval, TypeDocumentGji.Prescription);

                if (parentPrescription != null)
                {
                    prescription = prescriptionDomain.GetAll().FirstOrDefault(x => x.Id == parentPrescription.Id);
                }

                this.ReportParams["ДатаАкта"] = actRemoval.DocumentDate.ToDateString("d MMMM yyyy");

                this.ReportParams["НомерАкта"] = actRemoval.DocumentNumber;

                string prescriptionDocNum = string.Empty;
                string prescriptionDocDate = string.Empty;

                if (prescription != null)
                {
                    prescriptionDocNum = prescription.DocumentNumber;
                    prescriptionDocDate = prescription.DocumentDate.ToDateString("dd.MM.yyyy");
                }

                this.ReportParams["РеквизитыПредписаний"] = String.Format("№{0} от {1} г.", prescriptionDocNum, prescriptionDocDate);

                this.ReportParams["Описание"] = actRemoval.Description;

                var violData =
                    violStageDomain.GetAll()
                        .Where(x => x.Document.Id == actRemoval.Id)
                        .Select(x => new
                        {
                            x.Id,
                            x.InspectionViolation.Violation.PpRf25,
                            x.InspectionViolation.Violation.PpRf170,
                            x.InspectionViolation.Violation.PpRf307,
                            x.InspectionViolation.Violation.PpRf491,
                            x.InspectionViolation.Violation.OtherNormativeDocs,
                            x.InspectionViolation.RealityObject,
                            x.InspectionViolation.DateFactRemoval
                        })
                        .ToList();

                this.ReportParams["НаселенныйПункт"] = violData
                    .Where(x => x.RealityObject != null && x.RealityObject.FiasAddress != null)
                    .Select(x => x.RealityObject.FiasAddress.PlaceName)
                    .FirstOrDefault();

                var house =
                    violData.Where(x => x.RealityObject != null)
                        .Select(x => new {x.RealityObject.Id, x.RealityObject})
                        .Distinct(x => x.Id)
                        .Select(x => x.RealityObject)
                        .FirstOrDefault();

                if (house != null)
                {
                    this.ReportParams["ТипДома"] = house.TypeHouse.GetEnumMeta().Display;

                    this.ReportParams["АдресДома"] = string.Empty;
                    this.ReportParams["НомерДома"] = string.Empty;

                    
                    if (house.FiasAddress != null)
                    {
                        this.ReportParams["АдресДома"] = house.FiasAddress.PlaceName + ", " + house.FiasAddress.StreetName;

                        var houseNumber = house.FiasAddress.House;
                        if (!string.IsNullOrWhiteSpace(house.FiasAddress.Letter))
                        {
                            houseNumber = houseNumber + ", лит. " + house.FiasAddress.Letter;
                        }

                        if (!string.IsNullOrWhiteSpace(house.FiasAddress.Housing))
                        {
                            houseNumber = houseNumber + ", корп. " + house.FiasAddress.Housing;
                        }

                        this.ReportParams["НомерДома"] = houseNumber;
                    }

                    this.ReportParams["ФормаСобственности"] = house.TypeOwnership != null ? house.TypeOwnership.Name : string.Empty;
                }

                var linksViolation = string.Empty;
                foreach (
                    var viol in
                        violData.Where(x => !x.DateFactRemoval.HasValue || x.DateFactRemoval.Value > DateTime.MinValue))
                {
                    linksViolation += string.Format(
                        "{0},{1},{2},{3},{4},",
                        viol.PpRf25,
                        viol.PpRf170,
                        viol.PpRf307,
                        viol.PpRf491,
                        viol.OtherNormativeDocs);
                }

                linksViolation = linksViolation.Split(',')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .AggregateWithSeparator(", ");

                this.ReportParams["НомераНевыполненныхПунктов"] = linksViolation;

                var inspectorsQuery = docInspDomain.GetAll().Where(x => x.DocumentGji.Id == DocumentId);

                this.ReportParams["ИнспекторыИКоды"] = inspectorsQuery
                    .Select(x => new {x.Inspector.Fio, x.Inspector.Code})
                    .ToList()
                    .Aggregate(string.Empty, (x, y) =>
                    {
                        if (!string.IsNullOrEmpty(x))
                            x += ", ";

                        x += y.Fio;

                        if (!string.IsNullOrEmpty(y.Code))
                            x += " - " + y.Code;

                        return x;
                    });

                this.ReportParams["ИнспекторыАкта"] = inspectorsQuery
                    .Select(x => new {x.Inspector.Fio, x.Inspector.Position})
                    .ToList()
                    .Aggregate(string.Empty, (x, y) =>
                    {
                        if (!string.IsNullOrEmpty(x))
                            x += ", ";

                        if (!string.IsNullOrEmpty(y.Position))
                            x += y.Position + " - ";

                        x += y.Fio;

                        return x;
                    });

                //Получаем Отдел Инспеторов
                this.ReportParams["Отдел"] = zonalInspDomain.GetAll()
                    .Where(x => inspectorsQuery.Any(y => y.Inspector.Id == x.Inspector.Id))
                    .Select(x => x.ZonalInspection.Name)
                    .FirstOrDefault();

                var listDisposalProvDocs = new List<DispProvidedDoc>();

                // Если есть распоряжение то берем поля из него
                if (disposal != null)
                {
                    this.ReportParams["ДатаРаспоряжения"] = disposal.DocumentDate.ToDateString("d MMMM yyyy");

                    this.ReportParams["НомерРаспоряжения"] = disposal.DocumentNumber;

                    if (disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.NotRequiresAgreement)
                    {
                        this.ReportParams["ДатаИНомерРешенияПрокурора"] = "Не требуется";
                    }

                    var kindCheckName = disposal.KindCheck.Return(x => x.Name);
                    this.ReportParams["ВидПроверки"] = kindCheckName;

                    if (disposal.IssuedDisposal != null)
                    {
                        this.ReportParams["ДЛВынесшееРаспоряжение"] = disposal.IssuedDisposal.Position;
                        this.ReportParams["ФИОВынесшееПриказ"] = disposal.IssuedDisposal.Fio;
                        this.ReportParams["РуководительРП"] = (!string.IsNullOrEmpty(disposal.IssuedDisposal.PositionGenitive)
                            ? disposal.IssuedDisposal.PositionGenitive + " "
                            : disposal.IssuedDisposal.Position) +
                                                   (!string.IsNullOrEmpty(disposal.IssuedDisposal.FioGenitive)
                                                       ? disposal.IssuedDisposal.FioGenitive + " "
                                                       : disposal.IssuedDisposal.Fio);
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

                    listDisposalProvDocs.AddRange(disposalProvidedDocDomain.GetAll()
                        .Where(x => x.Disposal.Id == disposal.Id)
                        .Select(x => new DispProvidedDoc
                        {
                            Наименование = x.ProvidedDoc.Name
                        })
                        .ToList());
                }

                var uniqueDates = new List<DateTime>();
                TimeSpan? allTimes = null;

                // Если есть акт проверки то берем оттуда переменные
                if (act != null)
                {
                    if (act.Inspection.Contragent != null)
                    {
                        this.ReportParams["УправОрг"] = act.Inspection.Contragent.Name;
                        this.ReportParams["ИННУправОрг"] = act.Inspection.Contragent.Inn;
                        this.ReportParams["УправОргРП"] = act.Inspection.Contragent.Name;
                        this.ReportParams["СокрУправОрг"] = act.Inspection.Contragent.ShortName;

                        var headContragent = contragentContactDomain.GetAll()
                            .Where(x => x.Contragent.Id == act.Inspection.Contragent.Id)
                            .Where(x => x.DateStartWork.HasValue)
                            .Where(x => x.DateStartWork.Value <= DateTime.Today)
                            .Where(x => !x.DateEndWork.HasValue || x.DateEndWork.Value >= DateTime.Today)
                            .FirstOrDefault(
                                x => x.Position != null && (x.Position.Code == "1" || x.Position.Code == "4"));

                        if (headContragent != null)
                        {
                            this.ReportParams["РуководительЮЛДолжность"] = string.Format("{0} {1} {2} {3}",
                                headContragent.Position.Return(x => x.Name),
                                headContragent.Surname,
                                headContragent.Name,
                                headContragent.Patronymic);

                            this.ReportParams["РуководительЮЛ"] = string.Format("{0} {1} {2}",
                                headContragent.Surname,
                                headContragent.Name,
                                headContragent.Patronymic);
                        }

                        if (act.Inspection.Contragent.FiasJuridicalAddress != null)
                        {
                            this.ReportParams["ЮрАдресКонтрагента"] = act.Inspection.Contragent.JuridicalAddress;
                        }

                        this.ReportParams["АдресКонтрагентаФакт"] =
                            act.Inspection.Contragent.FiasFactAddress.Return(x => x.AddressName);
                    }

                    var witness = actCheckWitnessDomain.GetAll()
                        .Where(x => x.ActCheck.Id == act.Id)
                        .Select(x => new {x.Fio, x.Position, x.IsFamiliar})
                        .ToArray();

                    var allWitness = witness
                        .AggregateWithSeparator(x => x.Position + " - " + x.Fio, ", ");

                    var witnessIsFamiliar = witness
                        .Where(x => x.IsFamiliar)
                        .AggregateWithSeparator(x => x.Fio, ", ");

                    this.ReportParams["ДЛприПроверке"] = allWitness;
                    this.ReportParams["Ознакомлен"] = !string.IsNullOrEmpty(witnessIsFamiliar)
                        ? witnessIsFamiliar.TrimEnd(new[] {',', ' '})
                        : string.Empty;

                    //Собираем дату и время проведения проверки

                    this.ReportParams["ДатаВремяПроверки"] = actCheckPeriodDomain.GetAll()
                        .Where(x => x.ActCheck.Id == act.Id)
                        .Select(x => new {x.DateCheck, x.DateStart, x.DateEnd})
                        .OrderBy(x => x.DateStart)
                        .AsEnumerable() // Такую фигню делаю чтобы мутить с датами поскольку данные будут на сервере
                        .Select(x =>
                        {
                            var getIntervalTime =
                                new Func<DateTime?, DateTime?, string>(
                                    (dateStart, dateEnd) =>
                                    {
                                        if (!dateStart.HasValue || !dateEnd.HasValue)
                                            return "0 час. 00 мин.";

                                        if (dateStart.Value >= dateEnd.Value)
                                            return "0 час. 00 мин.";

                                        var time = dateEnd.Value - dateStart.Value;

                                        if (!uniqueDates.Contains(dateEnd.Value.Date))
                                        {
                                            uniqueDates.Add(dateEnd.Value.Date);
                                        }

                                        if (allTimes.HasValue)
                                            allTimes += time;
                                        else
                                            allTimes = time;

                                        return string.Format("{0} час. {1} мин.", time.Hours, time.ToString("mm"));
                                    });

                            return
                                string.Format(
                                    "{0} с {1} до {2} Продолжительность {3}",
                                    x.DateCheck.ToDateString("«dd» MMMM yyyy г."),
                                    x.DateStart.ToDateString("h час. mm мин."),
                                    x.DateEnd.ToDateString("h час. mm мин."),
                                    getIntervalTime(x.DateStart, x.DateEnd));
                        })
                        .AggregateWithSeparator(", ");
                }

                var allTimesResult = string.Empty;

                if (uniqueDates.Any())
                {
                    //Подсчитываем количество уникальных дней.
                    allTimesResult = uniqueDates.Count + " д.";
                }

                if (allTimes.HasValue)
                {
                    if (!string.IsNullOrEmpty(allTimesResult)) allTimesResult += ", ";

                    var hours = allTimes.Value.TotalHours;

                    var delta = Math.Round(hours - (int) hours, 1);

                    if (Math.Abs(delta - 0.5) < 0.01)
                    {
                        // Поскольку при округлении 0.5 половинка не переходит в большую сторону
                        // то к нашему числу часов добавляем 0.1/ Это нужно чтобы Roundокруглил в большую сторону
                        hours += 0.1;
                    }

                    if (allTimes.Value.TotalHours > 0) allTimesResult += (Math.Round(hours)) + " ч.";
                }

                this.ReportParams["ПродолжительностьПроверкиДЧ"] = allTimesResult;
                RegViolationsDataSource(prescription);
                RegViolationDescriptions(prescription);

            }
            finally
            {
                Container.Release(actCheckDomain);
                Container.Release(disposalDomain);
                Container.Release(contragentContactDomain);
                Container.Release(disposalProvidedDocDomain);
                Container.Release(actCheckWitnessDomain);
                Container.Release(actRemovalDomain);
                Container.Release(prescriptionDomain);
                Container.Release(violStageDomain);
                Container.Release(docInspDomain);
                Container.Release(actCheckPeriodDomain);
                Container.Release(zonalInspDomain);
            }
        }


        private string GetTypeCheckAblative(KindCheckGji kindCheck)
        {
            var result = "";

            var dictTypeCheckAblative = new Dictionary<TypeCheck, string>
            {
                {TypeCheck.PlannedExit, "плановой выездной"},
                {TypeCheck.NotPlannedExit, "внеплановой выездной"},
                {TypeCheck.PlannedDocumentation, "плановой документарной"},
                {TypeCheck.NotPlannedDocumentation, "внеплановой документарной"},
                {TypeCheck.InspectionSurvey, "внеплановой выездной"},
                {TypeCheck.PlannedDocumentationExit, "плановой документарной и выездной"},
                {TypeCheck.VisualSurvey, "о внеплановой проверке технического состояния жилого помещения"},
                {TypeCheck.NotPlannedDocumentationExit, "внеплановой документарной и выездной"}
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

        private void PrepareJurPersonReport()
        {
            var actCheckDomain = Container.Resolve<IDomainService<ActCheck>>();
            var disposalDomain = Container.Resolve<IDomainService<Disposal>>();
            var typeSurveyDomain = Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var contragentContactDomain = Container.Resolve<IDomainService<ContragentContact>>();
            var actRoDomain = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var typeSurveyGoalDomain = Container.Resolve<IDomainService<TypeSurveyGoalInspGji>>();
            var actAnnexDomain = Container.Resolve<IDomainService<ActCheckAnnex>>();
            var disposalExpertDomain = Container.Resolve<IDomainService<DisposalExpert>>();
            var disposalProvidedDocDomain = Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var actCheckWitnessDomain = Container.Resolve<IDomainService<ActCheckWitness>>();
            var actCheckViolationDomain = Container.Resolve<IDomainService<ActCheckViolation>>();
            var actCheckPeriodDomain = Container.Resolve<IDomainService<ActCheckPeriod>>();
            var docInspDomain = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var zonalInspectorDomain = Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var documentGjiDomain = Container.Resolve<IDomainService<DocumentGji>>();
            var actCheckRobjectDescriptionDomain = Container.ResolveDomain<ActCheckRealityObjectDescription>();
            var actRemovalDomain = Container.Resolve<IDomainService<ActRemoval>>();
            var prescriptionDomain = Container.Resolve<IDomainService<Prescription>>();

            try
            {
                Disposal disposal = null;
                ActCheck act = null;
                Prescription prescription = null;

                var actRemoval = actRemovalDomain.FirstOrDefault(x => x.Id == DocumentId);
                if (actRemoval == null)
                {
                    throw new ReportProviderException("Не удалось получить акт проверки предписания");
                }
                FillCommonFields(actRemoval);
                var parentAct = GetParentDocument(actRemoval, TypeDocumentGji.ActCheck);

                if (parentAct != null)
                {
                    act = actCheckDomain.GetAll().FirstOrDefault(x => x.Id == parentAct.Id);
                }

                var parentDisposal = GetParentDocument(act, TypeDocumentGji.Disposal);

                if (parentDisposal != null)
                {
                    disposal = disposalDomain.GetAll().FirstOrDefault(x => x.Id == parentDisposal.Id);
                }

                var parentPrescription = GetParentDocument(actRemoval, TypeDocumentGji.Prescription);

                if (parentPrescription != null)
                {
                    prescription = prescriptionDomain.GetAll().FirstOrDefault(x => x.Id == parentPrescription.Id);
                }

                var queryTypeSurveys = typeSurveyDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id);

                this.ReportParams["ДатаАкта"] = act.DocumentDate.ToDateString("d MMMM yyyy");
                this.ReportParams["НомерАкта"] = string.Format("{0} от {1}",
                    actRemoval.Return(x => x.DocumentNumber),
                    actRemoval.DocumentDate.ToDateString());

                this.ReportParams["ПроверПлощадь"] = act.Area.HasValue ? act.Area.Value.ToStr() : string.Empty;

                var docs = documentGjiDomain.GetAll()
                    .Where(x => x.Stage.Parent.Id == act.Stage.Parent.Id)
                    .Where(x => x.TypeDocumentGji == TypeDocumentGji.Protocol
                                || x.TypeDocumentGji == TypeDocumentGji.Prescription)
                    .ToList();

                var docPrescriptionNames = new List<string>();
                var docProtocolNames = new List<string>();
                foreach (var doc in docs)
                {
                    var date = doc.DocumentDate.HasValue ? doc.DocumentDate.Value.ToShortDateString() : string.Empty;
                    switch (doc.TypeDocumentGji)
                    {
                        case TypeDocumentGji.Prescription:
                            docPrescriptionNames.Add(string.Format("№{0} от {1}г.", doc.DocumentNumber, date));
                            break;
                        case TypeDocumentGji.Protocol:
                            docProtocolNames.Add(string.Format("составлен №{0} от {1}г.", doc.DocumentNumber, date));
                            break;
                    }
                }

                this.ReportParams["РеквизитыПредписаний"] = docPrescriptionNames.AggregateWithSeparator(", ");
                this.ReportParams["РеквизитыПротоколов"] = docProtocolNames.Count > 0
                    ? docProtocolNames.AggregateWithSeparator(", ")
                    : "не составлен";

                // Такую фигню делаю чтобы мутить с датами поскольку данные будут на сервере
                var actCheckPeriods =
                    actCheckPeriodDomain.GetAll()
                        .Where(x => x.ActCheck.Id == act.Id)
                        .Select(x => new {x.DateCheck, x.DateStart, x.DateEnd})
                        .OrderBy(x => x.DateCheck)
                        .ThenBy(x => x.DateEnd)
                        .ToList();

                var lastCheckDate = actCheckPeriods.LastOrDefault(x => x.DateEnd.HasValue);
                this.ReportParams["ВремяСоставленияАкта"] = lastCheckDate != null
                    ? lastCheckDate.DateEnd.ToDateString("HH-mm")
                    : string.Empty;

                this.ReportParams["ДатаВремяПроверки"] = actCheckPeriods
                    .Select(x =>
                    {
                        var getTime = new Func<DateTime?, string>(date => date.ToDateString("HH час. mm мин."));

                        var getIntervalTime =
                            new Func<DateTime?, DateTime?, string>(
                                (dateStart, dateEnd) =>
                                {
                                    if (!dateStart.HasValue || !dateEnd.HasValue)
                                        return "0 час. 00 мин.";

                                    if (dateStart.Value >= dateEnd.Value)
                                        return "0 час. 00 мин.";

                                    var time = dateEnd.Value - dateStart.Value;

                                    return string.Format("{0} час. {1} мин.", time.Hours, time.ToString("mm"));
                                });

                        return
                            string.Format("{0} с {1} до {2} Продолжительность {3}",
                                x.DateCheck.ToDateString("«dd» MMMM yyyy г."),
                                getTime(x.DateStart),
                                getTime(x.DateEnd),
                                getIntervalTime(x.DateStart, x.DateEnd));
                    })
                    .AggregateWithSeparator(", ");

                var daysCount = actCheckPeriods.Distinct(x => x.DateCheck).Count();
                var periodTimeSum = new TimeSpan();
                foreach (var actCheckPeriod in actCheckPeriods)
                {
                    if (actCheckPeriod.DateStart.HasValue && actCheckPeriod.DateEnd.HasValue)
                    {
                        periodTimeSum += actCheckPeriod.DateEnd.Value - actCheckPeriod.DateStart.Value;
                    }
                }

                this.ReportParams["ПродолжительностьПроверкиДЧ"] = string.Format("{0} д., {1} ч.", daysCount,
                    Math.Round(periodTimeSum.TotalHours));

                this.ReportParams["ИнспекторыИКоды"] = docInspDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == act.Id)
                    .Select(x => new {x.Inspector.Fio, x.Inspector.Code})
                    .ToList()
                    .Aggregate(string.Empty, (x, y) =>
                    {
                        if (!string.IsNullOrEmpty(x))
                            x += ", ";

                        x += y.Fio;

                        if (!string.IsNullOrEmpty(y.Code))
                            x += " - " + y.Code;

                        return x;
                    });

                this.ReportParams["ИнспекторыАкта"] = docInspDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == act.Id)
                    .Select(x => new {x.Inspector.Fio, x.Inspector.Position})
                    .ToList()
                    .Aggregate(string.Empty, (x, y) =>
                    {
                        if (!string.IsNullOrEmpty(x))
                            x += ", ";

                        x += y.Fio;

                        if (!string.IsNullOrEmpty(y.Position))
                            x += " - " + y.Position;

                        return x;
                    });

                var inspectorIds =
                    docInspDomain.GetAll().Where(x => x.DocumentGji.Id == act.Id).Select(x => x.Inspector.Id).ToList();

                if (inspectorIds.Any())
                {
                    var zonalInspection =
                        zonalInspectorDomain.GetAll()
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
                    }

                    this.ReportParams["АдресКонтрагентаФакт"] = contragent.FiasFactAddress.Return(x => x.AddressName);
                }

                var queryTypeSurveyIds = queryTypeSurveys.Select(x => x.TypeSurvey.Id);
                var strGoalInspGji = typeSurveyGoalDomain.GetAll()
                    .Where(x => queryTypeSurveyIds.Contains(x.TypeSurvey.Id))
                    .Select(x => x.SurveyPurpose.Name)
                    .AsEnumerable()
                    .AggregateWithSeparator("; ");

                this.ReportParams["Цель"] = strGoalInspGji;

                var actCheckAnnex = actAnnexDomain.GetAll()
                    .Where(x => x.ActCheck.Id == act.Id)
                    .Select(x => x.Name)
                    .AsEnumerable()
                    .AggregateWithSeparator(", ");

                this.ReportParams["ПрилагаемыеДокументы"] = actCheckAnnex;

                // Дома акта проверки
                var actCheckRealtyObjects = actRoDomain.GetAll()
                    .Where(x => x.ActCheck.Id == act.Id)
                    .Select(x => new {x.Id, x.RealityObject, x.HaveViolation, x.Description, x.NotRevealedViolations})
                    .ToArray();

                var actCheckRealtyObjectIds = actCheckRealtyObjects.Select(x => x.Id).ToList();
                var descriptions =
                    actCheckRobjectDescriptionDomain.GetAll()
                        .Where(x => actCheckRealtyObjectIds.Contains(x.ActCheckRealityObject.Id))
                        .AsEnumerable()
                        .ToDictionary(x => x.ActCheckRealityObject.Id, x => Encoding.UTF8.GetString(x.Description));

                if (actCheckRealtyObjects.All(x => x.HaveViolation == YesNoNotSet.No))
                {
                    this.ReportParams["НеВыявлено"] = actCheckRealtyObjects
                        .Select(x => descriptions.Get(x.Id) ?? x.Description)
                        .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? "; " + y : y));
                }

                this.ReportParams["НевыявленныеНарушения"] = string.Join(", ",
                    actCheckRealtyObjects.Select(x => x.NotRevealedViolations).ToArray());

                var actRoList = actCheckRealtyObjects.Where(x => x.RealityObject != null).ToList();

                if (actRoList.Count > 0)
                {
                    var firstRecord = actRoList.First();
                    var realtyObject = firstRecord.RealityObject;
                    if (realtyObject.FiasAddress != null)
                    {
                        this.ReportParams["НомерДома"] = realtyObject.FiasAddress.House;
                        this.ReportParams["АдресДома"] = realtyObject.FiasAddress.PlaceName
                                              + ", "
                                              + realtyObject.FiasAddress.StreetName;
                        this.ReportParams["НаселенныйПункт"] = realtyObject.FiasAddress.PlaceName;
                    }

                    if (actRoList.Count == 1)
                    {
                        var housing = !string.IsNullOrEmpty(realtyObject.FiasAddress.Housing)
                            ? ", корп. " + realtyObject.FiasAddress.Housing
                            : string.Empty;

                        this.ReportParams["УлицаДом"] = string.Format("{0}, {1}, д.{2}{3} ", realtyObject.FiasAddress.PlaceName,
                            realtyObject.FiasAddress.StreetName, realtyObject.FiasAddress.House, housing);
                        this.ReportParams["ДомаИАдреса"] = realtyObject.FiasAddress.AddressName;
                        this.ReportParams["Описание"] = descriptions.Get(firstRecord.Id) ?? firstRecord.Description;
                    }
                    else
                    {
                        var realObjs = new StringBuilder();

                        foreach (var realityObject in actRoList.Select(x => x.RealityObject))
                        {
                            if (realObjs.Length > 0)
                                realObjs.Append("; ");

                            var housing = !string.IsNullOrEmpty(realtyObject.FiasAddress.Housing)
                                ? ", корп. " + realtyObject.FiasAddress.Housing
                                : string.Empty;

                            realObjs.AppendFormat("{0}, д.{1}{2}",
                                realityObject.FiasAddress.StreetName,
                                realityObject.FiasAddress.House,
                                housing);
                        }

                        this.ReportParams["УлицаДом"] = string.Format("{0}.", realObjs);
                        this.ReportParams["ДомаИАдреса"] = string.Format("{0}, {1}. ",
                            actRoList.FirstOrDefault().RealityObject.FiasAddress.PlaceName, realObjs);
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
                    this.ReportParams["ДатаРаспоряжения"] = disposal.DocumentDate.ToDateString("d MMMM yyyy");

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

                        this.ReportParams["РуководительРП"] = disposal.IssuedDisposal.FioGenitive;
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

                        this.ReportParams["ПродолжительностьПроверки"] = countDays.ToStr();
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

                    this.ReportParams["ПредоставляемыеДокументыСтрокой"] =
                        listDisposalProvDocs.Select(x => x.Наименование)
                            .AsEnumerable()
                            .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));
                }


                var witness = actCheckWitnessDomain.GetAll()
                    .Where(x => x.ActCheck.Id == act.Id)
                    .Select(x => new {x.Fio, x.Position, x.IsFamiliar})
                    .ToArray();

                var allWitness = witness.AggregateWithSeparator(x => x.Position + " - " + x.Fio, ", ");

                var witnessIsFamiliar = witness
                    .Where(x => x.IsFamiliar)
                    .AggregateWithSeparator(x => x.Fio, ", ");

                this.ReportParams["ДЛприПроверке"] = allWitness;
                this.ReportParams["Ознакомлен"] = !string.IsNullOrEmpty(witnessIsFamiliar)
                    ? witnessIsFamiliar.TrimEnd(new[] {',', ' '})
                    : string.Empty;

                this.ReportParams["ЧислоЛицПривлеченныеЛица"] = witness.Length > 0 ? "Лица, привлеченные" : "Лицо, привлеченное";

                // получаем ссылки на статьи
                var violList =
                    actCheckViolationDomain.GetAll()
                        .Where(x => x.ActObject.ActCheck.Id == act.Id)
                        .Select(x => new
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

                linksViolation =
                    linksViolation.Split(',')
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Select(x => x.Trim())
                        .Distinct()
                        .AggregateWithSeparator(", ");

                this.ReportParams["СсылкиНаПунктыНормативныхАктов"] = linksViolation;

                RegViolationsDataSource(prescription);
                RegViolationDescriptions(prescription);
            }
            finally
            {
                Container.Release(actCheckDomain);
                Container.Release(disposalDomain);
                Container.Release(typeSurveyDomain);
                Container.Release(typeSurveyDomain);
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
                Container.Release(documentGjiDomain);
                Container.Release(actCheckRobjectDescriptionDomain);
                Container.Release(actRemovalDomain);
            }
        }

        #region dataSources registration methods

        private void RegViolationsDataSource(Prescription prescription)
        {
            var prescriptionViolDomain = Container.ResolveDomain<PrescriptionViol>();
            var prescrViolDescrDomain = Container.ResolveDomain<PrescriptionViolDescription>();

            using (Container.Using(prescriptionViolDomain, prescrViolDescrDomain))
            {
                if (prescription != null)
                {
                    this.ReportParams["ДатаПредписания"] = prescription.DocumentDate.ToDateString();
                    this.ReportParams["НомерПредписания"] = prescription.DocumentNumber;

                    var i = 0;
                    var prescriptionViols =
                        prescriptionViolDomain.GetAll()
                            .Where(x => x.Document.Id == prescription.Id)
                            .Select(x => new
                            {
                                x.Id,
                                x.Action,
                                x.InspectionViolation.DateFactRemoval,
                                x.InspectionViolation.Violation.Name
                            })
                            .AsEnumerable();

                    var prescriptionViolIds = prescriptionViols.Select(x => x.Id).ToArray();
                    var actions =
                        prescrViolDescrDomain.GetAll()
                            .Where(x => prescriptionViolIds.Contains(x.PrescriptionViol.Id))
                            .Select(x => new {x.PrescriptionViol.Id, x.Action})
                            .AsEnumerable()
                            .ToDictionary(x => x.Id, x => Encoding.UTF8.GetString(x.Action));

                    this.DataSources.Add(new MetaData
                    {
                        SourceName = "Нарушения",
                        MetaType = nameof(Object),
                        Data = prescriptionViols.Select(x => new
                            {
                                НомерПП = ++i,
                                Мероприятие = actions.Get(x.Id) ?? x.Action,
                                Основание = x.Name,
                                ДатаВыполнения =
                                    x.DateFactRemoval.HasValue
                                        ? x.DateFactRemoval.Value.ToShortDateString()
                                        : "Не выполнено"
                            })
                            .ToList()
                    });
                }
            }
        }

        private void RegViolationDescriptions(Prescription prescription)
        {
            var violGroupDomain = Container.ResolveDomain<DocumentViolGroup>();
            var longTextDomain = Container.ResolveDomain<DocumentViolGroupLongText>();
            var violPiontDomain = Container.Resolve<IDomainService<DocumentViolGroupPoint>>();

            using (Container.Using(violGroupDomain, violPiontDomain))
            {
                var query = violGroupDomain.GetAll().Where(x => x.Document.Id == DocumentId);

                var violPoints =
                    violPiontDomain.GetAll()
                        .Where(x => query.Any(y => y.Id == x.ViolGroup.Id))
                        .Select(x => new
                        {
                            x.ViolStage.InspectionViolation.Violation.CodePin,
                            ViolStageId = x.ViolStage.Id,
                            violGroupId = x.ViolGroup.Id
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.violGroupId)
                        .ToDictionary(x => x.Key, y => y.Select(z => z.CodePin).AggregateWithSeparator(","));

                var descriptions = longTextDomain.GetAll()
                    .Where(z => query.Any(x => x.Id == z.ViolGroup.Id))
                    .Select(x => new
                    {
                        x.ViolGroup.Id,
                        x.Description
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, z => z.Select(x => Encoding.UTF8.GetString(x.Description)).First());

                var data = query
                    .Select(x => new
                    {
                        x.Id,
                        Document = x.Document.Id,
                        Municipality = x.RealityObject != null ? x.RealityObject.Municipality.Name : null,
                        RealityObject = x.RealityObject != null ? x.RealityObject.Address : null,
                        x.DatePlanRemoval,
                        x.DateFactRemoval,
                        x.Description,
                        x.Action
                    })
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        x.DatePlanRemoval,
                        x.DateFactRemoval,
                        x.Description,
                        x.Action,
                        PointCodes = violPoints.Get(x.Id) ?? string.Empty
                    });

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ОписаниеНарушений",
                    MetaType = nameof(Object),
                    Data = data.Select(x => new
                    {
                        Пункт = x.PointCodes,
                        Описание = descriptions.Get(x.Id) ?? x.Description,
                        СрокУстранения = x.DatePlanRemoval.ToDateString(),
                        ДатаФактУстранения = x.DateFactRemoval.ToDateString(),
                        Мероприятия = x.Action
                    })
                });

            }

        }

        #endregion


        #region util classes

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
        }

        #endregion
    }
}
