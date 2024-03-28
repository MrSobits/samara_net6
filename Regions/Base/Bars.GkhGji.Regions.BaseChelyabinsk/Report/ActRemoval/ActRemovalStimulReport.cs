namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report.ActRemoval
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhCalendar.Entities;
    using Bars.GkhCalendar.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;

    /// <summary>
	/// Отчет акта проверки предписания
	/// </summary>
	public class ActRemovalStimulReport : GkhBaseStimulReport
    {
        private long DocumentId { get; set; }

		/// <summary>
		/// Конструктор
		/// </summary>
        public ActRemovalStimulReport()
            : base(new ReportTemplateBinary(Resources.ActRemoval))
        {
        }

		/// <summary>
		/// Формат экспорта
		/// </summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

		/// <summary>
		/// Настройки экспорта
		/// </summary>
		public override Dictionary<string, string> ExportSettings =>
            new()
            {
                { "RemoveEmptySpaceAtBottom", "false" }
            };

        /// <summary>
		/// Идентификатор отчета
		/// </summary>
		public override string Id
        {
            get { return "ActRemoval"; }
        }

		/// <summary>
		/// Код формы
		/// </summary>
        public override string CodeForm
        {
            get { return "ActRemoval"; }
        }

		/// <summary>
		/// Наименование
		/// </summary>
        public override string Name
        {
            get { return "Акт проверки предписания"; }
        }

		/// <summary>
		/// Описание
		/// </summary>
        public override string Description
        {
            get { return "Акт проверки предписания"; }
        }

		/// <summary>
		/// Код шаблона
		/// </summary>
        protected override string CodeTemplate { get; set; }

		/// <summary>
		/// Установить пользовательские параметры
		/// </summary>
		/// <param name="userParamsValues">Значени пользовательских параметров</param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

		/// <summary>
		/// Получить информацию о шаблоне
		/// </summary>
		/// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                    {
                        Code = "ChelyabinskActRemoval",
                        Name = "ChelyabinskActRemoval",
                        Description = "Акт проверки предписания",
                        Template = Resources.ActRemoval
                    }
            };
        }

		/// <summary>
		/// Подготовить отчет
		/// </summary>
		/// <param name="reportParams">Параметры отчета</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var actRemovalDomain = this.Container.Resolve<IDomainService<ActRemoval>>();
            var disposalDomain = this.Container.Resolve<IDomainService<Disposal>>();
            var prescriptionDomain = this.Container.Resolve<IDomainService<Prescription>>();
            var contragentContactDomain = this.Container.Resolve<IDomainService<ContragentContact>>();
            var disposalProvidedDocDomain = this.Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var actRemovalWitnessDomain = this.Container.Resolve<IDomainService<ActRemovalWitness>>();
            var actRemovalProvidedDocDomain = this.Container.Resolve<IDomainService<ActRemovalProvidedDoc>>();
            var violStageDomain = this.Container.Resolve<IDomainService<InspectionGjiViolStage>>();
            var zonalInspDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var docInspDomain = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var prescriptionViolDomain = this.Container.Resolve<IDomainService<PrescriptionViol>>();
            var actRemovalPeriodDomain = this.Container.Resolve<IDomainService<ActRemovalPeriod>>();
            var dayDomain = this.Container.Resolve<IDomainService<Day>>();

            try
            {
                var actRemoval = actRemovalDomain.FirstOrDefault(x => x.Id == this.DocumentId);
                if (actRemoval == null)
                {
                    throw new ReportProviderException("Не удалось получить акт проверки предписания");
                }

                this.ReportParams["Id"] = this.DocumentId.ToString();
                this.ReportParams["ИдентификаторДокументаГЖИ"] = actRemoval.Id.ToString();
                this.ReportParams["СтрокаПодключениякБД"] = this.Container.Resolve<IDbConfigProvider>().ConnectionString;

                Disposal disposal = null;
                Prescription prescription = null;

                var parentDisposal = this.GetParentDocument(actRemoval, TypeDocumentGji.Disposal);
                if (parentDisposal != null)
                {
                    disposal = disposalDomain.GetAll().FirstOrDefault(x => x.Id == parentDisposal.Id );
                }

                var parentPrescription = this.GetParentDocument(actRemoval, TypeDocumentGji.Prescription);
                if (parentPrescription != null)
                {
                    prescription = prescriptionDomain.GetAll().FirstOrDefault(x => x.Id == parentPrescription.Id);
                }

                this.ReportParams["ДатаАкта"] = actRemoval.DocumentDate.HasValue
                    ? actRemoval.DocumentDate.Value.ToString("d MMMM yyyy")
                    : string.Empty;

                this.ReportParams["НомерАкта"] = actRemoval.DocumentNumber;
                this.ReportParams["Описание"] = actRemoval.Description;

                var violData =
                    violStageDomain.GetAll()
                                   .Where(x => x.Document.Id == actRemoval.Id)
                                   .Select(
                                       x =>
                                       new
                                           {
                                               x.Id,
                                               x.InspectionViolation.Violation.PpRf25,
                                               x.InspectionViolation.Violation.PpRf170,
                                               x.InspectionViolation.Violation.PpRf307,
                                               x.InspectionViolation.Violation.PpRf491,
                                               x.InspectionViolation.Violation.OtherNormativeDocs,
                                               RealityObject =
                                           x.InspectionViolation.RealityObject != null
                                               ? x.InspectionViolation.RealityObject
                                               : null,
                                               x.InspectionViolation.DateFactRemoval
                                           })
                                   .ToList();

                this.ReportParams["НаселенныйПункт"] = violData.Where(x => x.RealityObject != null && x.RealityObject.FiasAddress != null).Select(x => x.RealityObject.FiasAddress.PlaceName).FirstOrDefault();

                var roList =
                    violData.Where(x => x.RealityObject != null)
                            .Select(x => new {x.RealityObject.Id, x.RealityObject.Address})
                            .Distinct(x => x.Id)
                            .Select(x => x.Address)
                            .ToList();

                if (roList.Count > 0)
                {
                    this.ReportParams["Адреса"] = roList.Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? "; " + y : y));    
                }


                var linksViolation = string.Empty;
                foreach (var viol in violData.Where(x => !x.DateFactRemoval.HasValue || (x.DateFactRemoval.HasValue && x.DateFactRemoval.Value > DateTime.MinValue)))
                {
                    linksViolation += string.Format(
                        "{0},{1},{2},{3},{4},",
                        viol.PpRf25,
                        viol.PpRf170,
                        viol.PpRf307,
                        viol.PpRf491,
                        viol.OtherNormativeDocs);
                }

                linksViolation = linksViolation.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim()).Distinct()
                                                .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

                this.ReportParams["НомераНевыполненныхПунктов"] = linksViolation;

                this.FillPrescription(actRemoval, prescription);

                var inspectorsQuery = docInspDomain.GetAll().Where(x => x.DocumentGji.Id == this.DocumentId);

                this.ReportParams["ИнспекторыИКоды"] = inspectorsQuery
                                        .Select(x => new { x.Inspector.Fio, x.Inspector.Code})
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
                                        .Select(x => new { x.Inspector.Fio, x.Inspector.Position })
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

                // Получаем Отдел Инспеторов
                this.ReportParams["Отдел"] = zonalInspDomain.GetAll()
                                   .Where(x => inspectorsQuery.Any(y => y.Inspector.Id == x.Inspector.Id))
                                   .Select(x => x.ZonalInspection.Name)
                                   .FirstOrDefault();


                var listDisposalProvDocs = new List<DispProvidedDoc>();
                
                // Если есть распоряжение то берем поля из него
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
                    
                    var kindCheckName = disposal.KindCheck != null ? disposal.KindCheck.Name : "";
                    this.ReportParams["ВидПроверки"] = kindCheckName;


                    if (disposal.IssuedDisposal != null)
                    {
                        this.ReportParams["ДЛВынесшееРаспоряжение"] = disposal.IssuedDisposal.Position;
                        this.ReportParams["ФИОВынесшееПриказ"] = disposal.IssuedDisposal.Fio;
                        this.ReportParams["РуководительРП"] = (!string.IsNullOrEmpty(disposal.IssuedDisposal.PositionGenitive) ? 
                                                        disposal.IssuedDisposal.PositionGenitive + " " : disposal.IssuedDisposal.Position) + 
                                                    (!string.IsNullOrEmpty(disposal.IssuedDisposal.FioGenitive) ?
                                                        disposal.IssuedDisposal.FioGenitive + " " : disposal.IssuedDisposal.Fio);
                    }

                    this.ReportParams["ВидОбследованияРП"] = this.GetTypeCheckAblative(disposal.KindCheck);
                    if (disposal.DateStart.HasValue && disposal.DateEnd.HasValue)
                    {
                        var startDate = disposal.DateStart;

                        int countDays = 0;

                        while (startDate.Value.Date != disposal.DateEnd.Value.Date)
                        {
                            if (startDate.Value.DayOfWeek != DayOfWeek.Sunday && startDate.Value.DayOfWeek != DayOfWeek.Saturday)
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
                
                var witness = actRemovalWitnessDomain.GetAll()
					.Where(x => x.ActRemoval.Id == actRemoval.Id)
                    .Select(x => new { x.Fio, x.Position, x.IsFamiliar })
                    .ToArray();

                var allWitness = witness
                    .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Position + " - " + y.Fio : y.Position + " - " + y.Fio));

                var witnessIsFamiliar = witness
                    .Where(x => x.IsFamiliar)
                    .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Fio : y.Fio));

                this.ReportParams["ДЛприПроверке"] = allWitness;
                this.ReportParams["Ознакомлен"] = !string.IsNullOrEmpty(witnessIsFamiliar)
                                                                ? witnessIsFamiliar.TrimEnd(new[] { ',', ' ' })
                                                                : string.Empty;

                var actProvidedDocs = actRemovalProvidedDocDomain.GetAll()
					.Where(x => x.ActRemoval.Id == actRemoval.Id)
                    .Select(x => new
                    {
                        Наименование = x.ProvidedDoc.Name
                    })
                    .ToList();

                var listProvDocs = actProvidedDocs.Where(x => listDisposalProvDocs.Any(y => y.Наименование == x.Наименование)).ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ПредоставленныеДокументы",
                    MetaType = nameof(Object),
                    Data = listProvDocs
                });

                var actOtherProvidedDocs = actProvidedDocs.Where(x => !listDisposalProvDocs.Any(y => y.Наименование == x.Наименование)).ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ДопПредоставленныеДокументы",
                    MetaType = nameof(Object),
                    Data = actOtherProvidedDocs
                });

                //Собираем дату и время проведения проверки

                this.ReportParams["ДатаВремяПроверки"] = actRemovalPeriodDomain.GetAll()
											.Where(x => x.ActRemoval.Id == actRemoval.Id)
                                            .Select(x => new { x.DateCheck, x.DateStart, x.DateEnd })
                                            .OrderBy(x => x.DateStart)
                                            .AsEnumerable() // Такую фигню делаю чтобы мутить с датами поскольку данные будут на сервере
                                            .Select(
                                                x =>
                                                {
                                                    var getTime = new Func<DateTime?, string>(
                                                        date => date.HasValue
                                                                    ? date.Value.ToString("h час. mm мин.")
                                                                    : string.Empty);

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

                                                                return string.Format("{0} час. {1} мин.", time.Hours.ToString(), time.ToString("mm")); 
                                                            });

                                                    return
                                                        string.Format(
                                                            "{0} с {1} до {2} Продолжительность {3}",
                                                            x.DateCheck.HasValue
                                                                ? x.DateCheck.Value.ToString(
                                                                    "«dd» MMMM yyyy г.")
                                                                : string.Empty,
                                                            getTime(x.DateStart),
                                                            getTime(x.DateEnd),
                                                            getIntervalTime(x.DateStart, x.DateEnd));
                                                })
                                            .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

                var checkDatesList =
                    actRemovalPeriodDomain.GetAll()
					.Where(x => x.ActRemoval.Id == actRemoval.Id)
                    .Select(x => new { x.DateStart, x.DateEnd })
                    .ToList();

                if (checkDatesList.Count > 0)
                {
                    var checkTime = checkDatesList.Where(x => x.DateEnd.HasValue).Max(x => x.DateEnd.Value) - checkDatesList.Where(x => x.DateStart.HasValue).Min(x => x.DateStart.Value);

                    // полчаса и более округляем до часа
                    var delta = Math.Round(checkTime.TotalHours - (int)checkTime.TotalHours, 1);
                    var hours = delta >= 0.5 ? checkTime.Hours + 1 : checkTime.Hours;

                    this.ReportParams["ПродолжительностьПроверкиДЧ"] = checkTime.Days > 0
                        ? string.Format("{0} д. {1} ч.", checkTime.Days, hours)
                        : string.Format("{0} ч.", hours);
                }

                var actCheckPeriods =
                    actRemovalPeriodDomain.GetAll()
						.Where(x => x.ActRemoval.Id == actRemoval.Id)
                        .Select(x => new { x.DateCheck, x.DateStart, x.DateEnd })
                        .OrderBy(x => x.DateCheck)
                        .ThenBy(x => x.DateEnd)
                        .ToList();

                if (actCheckPeriods.Count > 0)
                {
                    var firstDay = actCheckPeriods.First();
                    var lastDay = actCheckPeriods.Last();

                    var days = dayDomain.GetAll()
                        .Where(x => x.DayDate >= firstDay.DateCheck && x.DayDate <= lastDay.DateCheck)
                        .Where(x => x.DayType == DayType.Workday)
                        .ToDictionary(x => x.DayDate);

                    var workDays = days.Count;

                    var interval = new TimeSpan();

                    if (days.ContainsKey(firstDay.DateCheck.ToDateTime()))
                    {
                        interval = this.GetInteval(firstDay.DateStart.ToDateTime(), firstDay.DateEnd.ToDateTime());
                        workDays--;
                    }

                    if (actCheckPeriods.Count > 1 && days.ContainsKey(lastDay.DateCheck.ToDateTime()))
                    {
                        interval += this.GetInteval(lastDay.DateStart.ToDateTime(), lastDay.DateEnd.ToDateTime());
                        workDays--;
                    }

                    var hours = (int)interval.TotalHours;
                    workDays += Math.Min(hours / 8, 2);
                    hours -= Math.Min(hours / 8, 2) * 8;

                    this.ReportParams["ОбщаяПродолжительностьРабДни"] = "{0} дней, {1} часов".FormatUsing(workDays, hours);
                }
                
                if (prescription != null)
                {
					if (prescription.Inspection.Contragent != null)
					{
						this.ReportParams["УправОрг"] = prescription.Inspection.Contragent.Name;
						this.ReportParams["ИННУправОрг"] = prescription.Inspection.Contragent.Inn;
						this.ReportParams["УправОргРП"] = prescription.Inspection.Contragent.Name;
						this.ReportParams["СокрУправОрг"] = prescription.Inspection.Contragent.ShortName;

						var headContragent = contragentContactDomain.GetAll()
							.Where(x => x.Contragent.Id == prescription.Inspection.Contragent.Id)
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

						if (prescription.Inspection.Contragent.FiasJuridicalAddress != null)
						{
							this.ReportParams["ЮрАдресКонтрагента"] = prescription.Inspection.Contragent.JuridicalAddress;
						}

						if (prescription.Inspection.Contragent.FiasFactAddress != null)
						{
							this.ReportParams["АдресКонтрагентаФакт"] = prescription.Inspection.Contragent.FiasFactAddress.Return(x => x.AddressName);
						}
					}

                    this.ReportParams["ДатаПредписания"] = prescription.DocumentDate.HasValue
                                                            ? prescription.DocumentDate.Value.ToShortDateString()
                                                            : string.Empty;

                    this.ReportParams["НомерПредписания"] = prescription.DocumentNumber;

                    var i = 0;
                    var prescriptionViols = prescriptionViolDomain.GetAll()
                                              .Where(x => x.Document.Id == prescription.Id)
                                              .Select(
                                                  x =>
                                                  new
                                                      {
                                                          ViolationId = x.InspectionViolation.Violation.Id,
                                                          x.InspectionViolation.Violation.CodePin,
                                                          x.Action,
                                                          x.Description,
                                                          x.InspectionViolation.DatePlanRemoval,
                                                          x.InspectionViolation.DateFactRemoval,
                                                          x.InspectionViolation.Violation.Name
                                                      })
                                               // Сортировка должна быть такая же как в Печатке Предписания
                                              .OrderBy(x => x.DatePlanRemoval)
                                              .ThenBy(x => x.CodePin)
                                              .ThenBy(x => x.Action)
                                              .AsEnumerable()
                                              .Select(
                                                  x =>
                                                  new
                                                      {
                                                          НомерПП = ++i,
                                                          Мероприятие = x.Action,
                                                          Основание = x.Name,
                                                          ДатаВыполнения =
                                                      x.DateFactRemoval.HasValue
                                                          ? x.DateFactRemoval.Value.ToShortDateString()
                                                          : "Не выполнено"
                                                      })
                                              .ToList();

                    this.DataSources.Add(new MetaData
                    {
                        SourceName = "Нарушения",
                        MetaType = nameof(Object),
                        Data = prescriptionViols
                    });


                }
            }
            finally 
            {
                this.Container.Release(disposalDomain);
                this.Container.Release(actRemovalProvidedDocDomain);
                this.Container.Release(contragentContactDomain);
                this.Container.Release(disposalProvidedDocDomain);
                this.Container.Release(actRemovalWitnessDomain);
                this.Container.Release(actRemovalDomain);
                this.Container.Release(prescriptionDomain);
                this.Container.Release(violStageDomain);
                this.Container.Release(docInspDomain);
                this.Container.Release(prescriptionViolDomain);
                this.Container.Release(actRemovalPeriodDomain);
                this.Container.Release(zonalInspDomain);
            }
        }

        private void FillPrescription(ActRemoval actRemoval, Prescription prescription)
        {
            if (prescription == null) return;

            var prescrViolDomain = this.Container.ResolveDomain<PrescriptionViol>();
            var actRemovalDomain = this.Container.ResolveDomain<ActRemovalViolation>();

            try
            {
                this.ReportParams["НомерПредписания"] = prescription.DocumentNumber;
                this.ReportParams["ДатаПредписания"] = prescription.DocumentDate.ToDateString();

                var viols =
                    prescrViolDomain.GetAll()
                                    .Where(x => x.Document.Id == prescription.Id)
                                    .Select(
                                        x =>
                                        new
                                        {
                                            x.Id,
                                            InspViolId = x.InspectionViolation.Id,
                                            ViolationId = x.InspectionViolation.Violation.Id,
                                            x.InspectionViolation.Violation.CodePin,
                                            x.Action,
                                            x.Description,
                                            x.InspectionViolation.DatePlanRemoval,
                                            x.InspectionViolation.DateFactRemoval,
                                            Municipality = x.InspectionViolation.RealityObject.Municipality.Name,
                                            RealityObject = x.InspectionViolation.RealityObject.Address,
                                        })
                                    .OrderBy(x => x.Id)
                                    .AsEnumerable()
                                    .Select(
                                        (x, n) =>
                                        new
                                        {
                                            Num = n + 1,
                                            x.InspViolId,
                                            x.ViolationId,
                                            x.CodePin,
                                            x.Action,
                                            x.Description,
                                            x.DateFactRemoval,
                                            x.DatePlanRemoval
                                        })
                                    .ToArray();

                this.ReportParams["НомераНевыполненныхПунктовПредписания"] =
                    viols.Where(x => !x.DateFactRemoval.HasValue)
                         .Select(x => x.Num.ToString())
                         .AggregateWithSeparator(", ");

                if (actRemoval != null)
                {
                    var actViols = actRemovalDomain.GetAll()
                        .Where(x => x.Document.Id == actRemoval.Id)
                        .Select(x => new
                        {
                            x.CircumstancesDescription,
                            x.DateFactRemoval,
                            InspViolId = x.InspectionViolation.Id
                        })
                        .ToArray();

                    var result = viols
                        .Where(x => !x.DateFactRemoval.HasValue)
                        .Join(actViols, x => x.InspViolId, x => x.InspViolId, (x, y) => new
                        {
                            x.Num,
                            x.Action,
                            y.CircumstancesDescription,
                            y.DateFactRemoval
                        })
                        .Select(x => new
                        {
                            НомерПункта = x.Num,
                            Мероприятие = x.Action,
                            ОписаниеОбстоятельств = x.CircumstancesDescription,
                            ДатаФактИсполнения =
                                x.DateFactRemoval.HasValue
                                    ? x.DateFactRemoval.ToDateString()
                                    : "Не выполнено"
                        })
                        .ToArray();

                    this.DataSources.Add(new MetaData
                    {
                        SourceName = "ВыполнениеПредписания",
                        MetaType = nameof(Object),
                        Data = result
                    });
                }
            }
            finally
            {
                this.Container.Release(prescrViolDomain);
                this.Container.Release(actRemovalDomain);
            }
        }

        private string GetTypeCheckAblative(KindCheckGji kindCheck)
        {
            var result = "";

            var dictTypeCheckAblative = new Dictionary<TypeCheck, string>()
                {
                    { TypeCheck.PlannedExit, "плановой выездной"},
                    { TypeCheck.NotPlannedExit, "внеплановой выездной"},
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
            var childrenService = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
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
                this.Container.Release(childrenService);
            }
        }
        
        private class DispProvidedDoc
        {
            public string Наименование { get; set; }
        }

        private DateTime ChangeTime(DateTime date, int hours, int minutes)
        {
            return new DateTime(
                date.Year,
                date.Month,
                date.Day,
                hours,
                minutes,
                date.Second,
                date.Millisecond,
                date.Kind);
        }

        private TimeSpan GetInteval(DateTime dateStart, DateTime dateEnd)
        {
            var inteval = dateEnd - dateStart;

            var startLunch = this.ChangeTime(dateStart.ToDateTime(), 12, 0);
            var endLunch = this.ChangeTime(dateStart.ToDateTime(), 13, 0);

            if (startLunch > dateStart)
            {
                if (endLunch < dateEnd)
                {
                    inteval -= endLunch - startLunch;
                }
                else if (dateEnd > startLunch)
                {
                    inteval -= dateEnd - startLunch;
                }
            }
            else
            {
                if (endLunch < dateEnd && endLunch > dateStart)
                {
                    inteval -= endLunch - dateStart;
                }
            }

            return inteval > TimeSpan.Zero ? inteval : TimeSpan.Zero;
        }
    }
}