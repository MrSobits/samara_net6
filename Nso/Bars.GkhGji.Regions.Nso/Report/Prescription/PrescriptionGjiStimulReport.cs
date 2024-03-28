namespace Bars.GkhGji.Regions.Nso.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Nso.Entities;
    using Bars.GkhGji.Regions.Nso.Entities.Disposal;
    using GkhGji.Report;

	/// <summary>
	/// Отчет для предписания
	/// </summary>
	public class PrescriptionGjiStimulReport : GjiBaseStimulReport
    {
        private long DocumentId { get; set; }

		/// <summary>
		/// Конструктор
		/// </summary>
        public PrescriptionGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.NsoPrescription))
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
		/// Идентификатор отчета
		/// </summary>
		public override string Id
        {
            get { return "NsoPrescriptionGji"; }
        }

		/// <summary>
		/// Код формы
		/// </summary>
        public override string CodeForm
        {
            get { return "Prescription"; }
        }

		/// <summary>
		/// Наименование
		/// </summary>
        public override string Name
        {
            get { return "Предписание"; }
        }
        
		/// <summary>
		/// Описание
		/// </summary>
        public override string Description
        {
            get { return "Предписание"; }
        }

		/// <summary>
		/// Код шаблона
		/// </summary>
        protected override string CodeTemplate { get; set; }

		/// <summary>
		/// Установить пользовательские параметры
		/// </summary>
		/// <param name="userParamsValues">Значения пользовательских параметров</param>
		public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

		/// <summary>
		/// Получить информацию о шаболне
		/// </summary>
		/// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Code = "NsoPrescriptionGji",
                                   Name = "Prescription",
                                   Description = "Предписание",
                                   Template = Properties.Resources.NsoPrescription
                               }
                       };
        }

		/// <summary>
		/// Подготовить отчет
		/// </summary>
		/// <param name="reportParams">Параметры отчета</param>
        public override void PrepareReport(ReportParams reportParams)
        {

            var prescriptionDomain = Container.ResolveDomain<NsoPrescription>();
            var disposalDomain = Container.ResolveDomain<Disposal>();
            var inspectionRoDomain = Container.ResolveDomain<InspectionGjiRealityObject>();
            var prescrViolDomain = Container.ResolveDomain<PrescriptionViol>();
            var docInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            var childrenDomain = Container.ResolveDomain<DocumentGjiChildren>();
            var dispVerSubjectDomain = Container.ResolveDomain<DisposalVerificationSubject>();
            var prescActivDirectDomain = Container.ResolveDomain<PrescriptionActivityDirection>();
            var prescBaseDocDomain = Container.ResolveDomain<PrescriptionBaseDocument>();
            var violNormativeDocItemDomain = Container.ResolveDomain<ViolationNormativeDocItemGji>();
            var actCheckViolationDomain = Container.ResolveDomain<ActCheckViolation>();

            try
            {
                var prescription = prescriptionDomain.GetAll().FirstOrDefault(x => x.Id == DocumentId);

                if (prescription == null)
                    return;

                if (prescription.Stage == null)
                    return;

                if (prescription.Stage.Parent == null)
                    return;

                var disposal = disposalDomain.GetAll()
                    .FirstOrDefault(x => x.Stage.Id == prescription.Stage.Parent.Id);

                if (disposal == null)
                    return;
                
                FillCommonFields(prescription);

                this.ReportParams["ДатаПредписания"] = prescription.DocumentDate.HasValue
                                                                     ? prescription.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU"))
                                                                     : string.Empty;

                this.ReportParams["НомерПредписания"] = prescription.DocumentNumber;

                this.ReportParams["ФизическоеЛицо"] = prescription.PhysicalPerson;

                this.ReportParams["РеквизитыФизлица"] = prescription.PhysicalPersonInfo;
                this.ReportParams["МестоСоставления"] = prescription.DocumentPlace;
                this.ReportParams["ВремяCоставления"] = prescription.DocumentTime.HasValue ? prescription.DocumentTime.Value.ToString("HH-mm") : "";
                this.ReportParams["ТипИсполнителя"] = prescription.Executant != null ? prescription.Executant.Name : "";

                FillContragentInfo(prescription);

                if (prescription.Executant != null)
                {
                    var shortName = prescription.Contragent != null ? prescription.Contragent.ShortName : string.Empty;
                    var physicalPerson = prescription.PhysicalPerson;

                    switch (prescription.Executant.Code)
                    {
                        case "0":
                        case "9":
                        case "2":
                        case "4":
                        case "8":
                        case "18":
                            this.ReportParams["КомуВыдано"] = shortName;
                            break;
                        case "10":
                        case "1":
                        case "3":
                        case "5":
                        case "19":
                            this.ReportParams["КомуВыдано"] = shortName + ", " + physicalPerson;
                            break;
                        case "6":
                        case "7":
                            this.ReportParams["КомуВыдано"] = physicalPerson;
                            break;
                    }
                }

                var normativeDocDict = violNormativeDocItemDomain
                        .GetAll()
                        .Where(x => prescrViolDomain.GetAll()
                            .Any(y => y.Document.Id == prescription.Id && y.InspectionViolation.Violation.Id == x.ViolationGji.Id))
                        .Select(x => new
                        {
                            x.ViolationGji.Id,
                            x.NormativeDocItem.Number,
                            x.NormativeDocItem.NormativeDoc.Name
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.Number).Union(y.Select(x => x.Name)).AggregateWithSeparator(", "));

                var violations = prescrViolDomain.GetAll()
                                      .Where(x => x.Document.Id == prescription.Id)
                                      .Select(x => new
                                      {
                                          ViolationId = x.InspectionViolation.Violation.Id,
                                          x.InspectionViolation.Violation.CodePin,
                                          x.Action,
                                          x.InspectionViolation.DatePlanRemoval,
                                          Address = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Municipality.Name + ", " + x.InspectionViolation.RealityObject.Address : null,
                                      })
                                      // Сортировка должна быть такая же как в печатке Акт проверки предписания
                                      .OrderBy(x => x.DatePlanRemoval)
                                      .ThenBy(x => x.CodePin)
                                      .ThenBy(x => x.Action)
                                      .ToList();

                var dataViol = new List<PrescriptionViolRecord>();
                foreach (var viol in violations)
                {
                   dataViol.Add( new PrescriptionViolRecord
                            {
                                Адрес = viol.Address,
                                ПунктыНПА = normativeDocDict.Get(viol.ViolationId) ?? string.Empty,
								Мероприятие = viol.Action,
                                СрокМероприятия = viol.DatePlanRemoval.HasValue
                                                    ? viol.DatePlanRemoval.Value.ToShortDateString()
                                                    : string.Empty
                            });
                }

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Нарушения",
                    MetaType = nameof(PrescriptionViolRecord),
                    Data = dataViol
                });

                var insp = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == DocumentId)
                    .Select(x => new
                    {
                        x.Inspector.Fio,
                        x.Inspector.Position,
                        x.Inspector.ShortFio,
                        x.Inspector.FioAblative
                    })
                    .ToArray();

                if (insp.Length > 0)
                {
                    var firstInsp = insp.First();
                    this.ReportParams["ДолжностьИнспектора"] = firstInsp.Position;

                    if (firstInsp != null && !string.IsNullOrEmpty(firstInsp.ShortFio))
                    {
                        this.ReportParams["ИнспекторФамИО"] = firstInsp.ShortFio;
                    }
                }

                var actDoc = childrenDomain.GetAll()
                .Where(x => x.Children.Id == prescription.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                .Select(x => x.Parent)
                .FirstOrDefault();

                if (actDoc != null)
                {
                    this.ReportParams["НомерАктаПроверки"] = actDoc.DocumentNumber;
                    this.ReportParams["ДатаАктаПроверки"] = actDoc.DocumentDate.HasValue ? actDoc.DocumentDate.Value.ToShortDateString() : string.Empty;


                    var actCheckNormativeDocs = violNormativeDocItemDomain
                        .GetAll()
                        .Where(x => actCheckViolationDomain.GetAll()
                            .Any(y => y.Document.Id == actDoc.Id && y.InspectionViolation.Violation.Id == x.ViolationGji.Id))
                        .Select(x => new
                        {
                            ПолноеНаименование = x.NormativeDocItem.NormativeDoc.FullName,
                            СокращенноеНаименование = x.NormativeDocItem.NormativeDoc.Name,
                        })
                        .Distinct()
                        .ToList();

                    this.DataSources.Add(new MetaData
                    {
                        SourceName = "ПереченьНПА",
                        MetaType = nameof(Object),
                        Data = actCheckNormativeDocs
                    });
                }

                var realityObj = prescrViolDomain.GetAll()
                                      .Where(x => x.Document.Id == prescription.Id)
                                      .Where(x => x.InspectionViolation.RealityObject != null)
                                      .Select(x => x.InspectionViolation.RealityObject)
                                      .ToList()
                                      .Distinct(x => x.Id);

                // если дом один, то выводим адрес дома и номер дома, если домов нет или больше 1 - ничего не выводим
                if (realityObj.Any())
                {
                    var firstPrescriptionRo = realityObj.FirstOrDefault();

                    if (firstPrescriptionRo != null)
                    {
                        if (firstPrescriptionRo.FiasAddress != null)
                        {
                            var fias = firstPrescriptionRo.FiasAddress;

                            this.ReportParams["НаселенныйПункт"] = fias.PlaceName;
                            this.ReportParams["НомерДома"] = fias.House;

                            if (!string.IsNullOrEmpty(fias.Housing))
                            {
                                this.ReportParams["НомерДома"] = fias.House + ", корп. " + fias.Housing;   
                            }

                            this.ReportParams["Улица"] = fias.StreetName;
                        }
                    }
                }

                var surveySubjects = dispVerSubjectDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => new { Наименование = x.SurveySubject.Name } )
                    .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ПредметыПроверки",
                    MetaType = nameof(Object),
                    Data = surveySubjects
                });

                var activityDirections = prescActivDirectDomain.GetAll()
                    .Where(x => x.Prescription.Id == prescription.Id)
                    .Select(x => new { Наименование = x.ActivityDirection.Name })
                    .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "НаправленияДеятельности",
                    MetaType = nameof(Object),
                    Data = activityDirections
                });

                var prescBaseDocs = prescBaseDocDomain.GetAll()
                    .Where(x => x.Prescription.Id == prescription.Id)
                    .Select(x => new
                    {
                        KindBaseDocument = x.KindBaseDocument.Name,
                        x.NumDoc,
                        x.DateDoc
                    })
                    .ToList()
                    .Select(x => new
                    {
                        ДокументОснование = x.KindBaseDocument,
                        ДатаДокумента = x.DateDoc.HasValue ? x.DateDoc.Value.ToShortDateString() : string.Empty,
                        НомерДокумента  = x.NumDoc
                    })
                    .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ДокументыОснования",
                    MetaType = nameof(Object),
                    Data = prescBaseDocs
                });
            }
            finally 
            {
                Container.Release(prescriptionDomain);
                Container.Release(disposalDomain);
                Container.Release(inspectionRoDomain);
                Container.Release(prescrViolDomain);
                Container.Release(docInspectorDomain);
                Container.Release(childrenDomain);
                Container.Release(dispVerSubjectDomain);
                Container.Release(prescActivDirectDomain);
                Container.Release(prescBaseDocDomain);
                Container.Release(violNormativeDocItemDomain);
                Container.Release(actCheckViolationDomain);
            }

        }

        private void FillContragentInfo(Prescription prescription)
        {

            try
            {
                var contragent = prescription.Contragent;

                if (contragent != null)
                {                                  
                    this.ReportParams["ИНН"] = contragent.Inn;
                    this.ReportParams["КПП"] = contragent.Kpp;

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
                                var mu = rec.Replace("р-н.", "") + " район";
                                newAddr.Append(mu);
                                continue;
                            }

                            newAddr.Append(rec);
                        }

                        this.ReportParams["АдресКонтрагента"] = newAddr.ToString();
                    }
                    else
                    {
                        this.ReportParams["АдресКонтрагента"] = contragent.JuridicalAddress;
                    }
                }
            }
            finally 
            {
                
            }
        }


        protected class PrescriptionViolRecord
        {
            public string Адрес { get; set; }

            public string ПунктыНПА { get; set; }

            public string СрокМероприятия { get; set; }

            public string Мероприятие { get; set; }
        }
    }

}