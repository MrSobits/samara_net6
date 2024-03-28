namespace Bars.GkhGji.Regions.Zabaykalye.Report
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
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;
    using Bars.GkhGji.Report;

    using Slepov.Russian.Morpher;

    /// <summary>
    /// Печать отчета ГЖИ (Предписание) через Стимул
    /// </summary>
    public class PrescriptionGjiStimulReport : GjiBaseStimulReport
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrescriptionGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.ZabaykalyePrescription))
        {
        }

        /// <summary>
        /// Формат печатной формы.
        /// </summary>
        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get
            {
                return "ZabaykalyePrescriptionGji";
            }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get
            {
                return "Prescription";
            }
        }

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name
        {
            get
            {
                return "Предписание";
            }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get
            {
                return "Предписание";
            }
        }

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// ИД документа.
        /// </summary>
        private long DocumentId { get; set; }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues">Пользовательские параметры</param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        /// <summary>
        /// Получить список шаблонов
        /// </summary>
        /// <returns>
        /// Список шаблонов.
        /// </returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Code = "ZabaykalyePrescriptionGji",
                                   Name = "Prescription",
                                   Description = "Предписание",
                                   Template = Properties.Resources.ZabaykalyePrescription
                               }
                       };
        }

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams">Параметры отчета</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var prescriptionDomain = Container.ResolveDomain<Prescription>();
            var disposalDomain = Container.ResolveDomain<Disposal>();
            var inspectionRoDomain = Container.ResolveDomain<InspectionGjiRealityObject>();
            var prescrViolDomain = Container.ResolveDomain<PrescriptionViol>();
            var docInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            var childrenDomain = Container.ResolveDomain<DocumentGjiChildren>();
            var violPiontDomain = Container.ResolveDomain<DocumentViolGroupPoint>();
            var docViolGroupDomain = Container.ResolveDomain<DocumentViolGroup>();
            var docViolGroupLongDomain = Container.ResolveDomain<DocumentViolGroupLongText>();
            var disposalTypeSurveyDomain = Container.ResolveDomain<DisposalTypeSurvey>();
            var typeSurveyGoalDomain = Container.ResolveDomain<TypeSurveyGoalInspGji>();
            var inspAppealCitsDomain = Container.ResolveDomain<InspectionAppealCits>();
            var actCheckWitnessDomain = Container.ResolveDomain<ActCheckWitness>();

            try
            {
                var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

                var prescription = prescriptionDomain.GetAll().FirstOrDefault(x => x.Id == this.DocumentId);

                if (prescription == null)
                {
                    return;
                }

                if (prescription.Stage == null)
                {
                    return;
                }

                if (prescription.Stage.Parent == null)
                {
                    return;
                }

                FillCommonFields(prescription);

                this.ReportParams["ДатаПредписания"] = prescription.DocumentDate.HasValue
                    ? prescription.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU"))
                    : string.Empty;
                this.ReportParams["НомерПредписания"] = prescription.DocumentNumber;
                this.ReportParams["УправОрг"] = prescription.Contragent != null ? prescription.Contragent.Name : string.Empty;
                this.ReportParams["КомуВыдано"] = string.Empty;

                this.ReportParams["ОснованиеПроверки"] = this.GetTypeBase(prescription.Inspection.TypeBase);
                this.ReportParams["Примечание"] = prescription.Description;

                if (prescription.Executant != null)
                {
                    var executant = склонятель.Проанализировать(prescription.Executant.Name ?? string.Empty);

                    var shortName = prescription.Contragent != null ? prescription.Contragent.ShortName : string.Empty;
                    var physicalPerson = склонятель.Проанализировать(prescription.PhysicalPerson ?? string.Empty); 

                    switch (prescription.Executant.Code)
                    {
                        case "0":
                        case "9":
                        case "2":
                        case "4":
                        case "8":
                        case "18":
                            this.ReportParams["КомуВыдано"] = "{0} {1}".FormatUsing(executant.Дательный, shortName);
                            break;
                        case "10":
                        case "1":
                        case "3":
                        case "5":
                        case "19":
                            this.ReportParams["КомуВыдано"] = "{0} {1} {2}".FormatUsing(executant.Дательный, shortName, physicalPerson.Дательный);
                            break;
                        case "6":
                        case "7":
                            this.ReportParams["КомуВыдано"] = "{0} {1} {2}".FormatUsing(
                                executant.Дательный, 
                                physicalPerson.Дательный,
                                prescription.PhysicalPersonInfo);
                            break;
                    }
                }
                
                var docViolGroupsQuery = docViolGroupDomain.GetAll()
                    .Where(x => x.Document.Id == prescription.Id);

                var violPoints =
                    violPiontDomain.GetAll()
                                   .Where(x => docViolGroupsQuery.Any(y => y.Id == x.ViolGroup.Id))
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
                                                        .Distinct()
                                                        .Aggregate((str, result) => !string.IsNullOrEmpty(result) ? result + "," + str : str)                                                       
                                        });

                var longTexts = docViolGroupLongDomain.GetAll()
                    .Where(x => docViolGroupsQuery.Any(y => y.Id == x.ViolGroup.Id))
                    .Select(x => new
                    {
                        x.ViolGroup.Id,
                        x.Action,
                        x.Description
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(
                        x => x.Key, 
                        y => y.Select(x => new
                        {
                            Action = x.Action != null ? Encoding.UTF8.GetString(x.Action) : string.Empty,
                            Description = x.Description != null ? Encoding.UTF8.GetString(x.Description) : string.Empty
                        })
                    .First());

                var violations = docViolGroupsQuery
                         .Select(x => new
                         {
                             x.Id,
                             x.DatePlanRemoval,
                             x.Description,
                             x.Action
                         })
                         .ToList()
                         .Select(x => new
                         {
                             x.Id,
                             x.DatePlanRemoval,
                             Description = longTexts.ContainsKey(x.Id) && longTexts[x.Id].Description.IsNotEmpty() ? longTexts[x.Id].Description : x.Description,
                             Action = longTexts.ContainsKey(x.Id) && longTexts[x.Id].Action.IsNotEmpty() ? longTexts[x.Id].Action : x.Action,
                             PointCodes = violPoints.ContainsKey(x.Id) ? violPoints[x.Id].PointCodes : null
                         })
                         .ToList();

                var i = 0;
                var dataViol = new List<PrescriptionViolRecord>();
                foreach (var viol in violations)
                {
                    var rec = new PrescriptionViolRecord
                                  {
                                      НомерПП = ++i,
                                      Описание = viol.Description,
                                      Пункты = viol.PointCodes,
                                      Мероприятие = viol.Action,
                                      ДатаВыполнения = viol.DatePlanRemoval.HasValue
                                                    ? viol.DatePlanRemoval.Value.ToShortDateString()
                                                    : string.Empty
                                  };

                    dataViol.Add(rec);
                }

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Нарушения",
                    MetaType = nameof(PrescriptionViolRecord),
                    Data = dataViol
                });

                // Инспекторы
                var inspectors = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == this.DocumentId)
                    .Select(x => new
                    {
                        x.Inspector.Fio,
                        x.Inspector.Position,
                        x.Inspector.ShortFio,
                        x.Inspector.FioAblative,
                        x.Inspector.IsHead
                    })
                    .ToArray();
                this.ReportParams["ИнспекторыИКоды"] = inspectors.AggregateWithSeparator(x => x.Fio + " - " + x.Position, ", ");

                var headInsp = inspectors.FirstOrDefault();
                this.ReportParams["ДолжностьИнспектора"] = headInsp != null ? headInsp.Position : string.Empty;
                this.ReportParams["КодРуководителяФИО"] = headInsp != null ? headInsp.ShortFio : string.Empty;
                
                // Акт проверки
                var actDoc = GetParentDocument(prescription, TypeDocumentGji.ActCheck);

                this.ReportParams["НомерАктаПроверки"] = string.Empty;
                this.ReportParams["ДатаАктаПроверки"] = string.Empty;
                if (actDoc != null)
                {
                    this.ReportParams["НомерАктаПроверки"] = actDoc.DocumentNumber;
                    this.ReportParams["ДатаАктаПроверки"] = actDoc.DocumentDate.HasValue ? actDoc.DocumentDate.Value.ToString("от dd MMMM yyyy") : string.Empty;

                    var witnesses = actCheckWitnessDomain.GetAll()
                        .Where(x => x.ActCheck.Id == actDoc.Id)
                        .Select(x => new
                        {
                            ФИО = x.Fio
                        })
                        .ToList();

                    this.DataSources.Add(new MetaData
                    {
                        SourceName = "ПрисутствующиеЛица",
                        MetaType = nameof(Object),
                        Data = witnesses
                    });
                }               

                // Дома и адреса, распоряжение
                var disposal = disposalDomain.GetAll()
                    .FirstOrDefault(x => x.Stage.Id == prescription.Stage.Parent.Id);

                if (disposal != null && disposal.Inspection != null)
                {
                    this.ReportParams["Распоряжение"] = string.Format(
                        "{0} от {1}",
                        disposal.DocumentNumber,
                        disposal.DocumentDate.HasValue ? disposal.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU")) : string.Empty);
                }

                if (disposal != null)
                {
                    var goals = disposalTypeSurveyDomain.GetAll()
                        .Join(
                            typeSurveyGoalDomain.GetAll(),
                            x => x.TypeSurvey.Id,
                            y => y.TypeSurvey.Id,
                            (x, y) => new { x.Disposal.Id, y.SurveyPurpose.Name })
                        .Where(x => x.Id == disposal.Id)
                        .Select(x => x.Name)
                        .ToList()
                        .AggregateWithSeparator(", ");

                    this.ReportParams["Цель"] = goals;
                }

                var listRealityObjs = prescrViolDomain.GetAll()
                     .Where(x => x.Document.Id == prescription.Id && x.InspectionViolation.RealityObject != null)
                     .Select(x => x.InspectionViolation.RealityObject)
                     .Distinct()
                     .ToList();

                if (listRealityObjs.Count > 0)
                {
                    this.ReportParams["ДомаИАдреса"] = listRealityObjs.Select(x => x.Address).ToList().AggregateWithSeparator(", ");

                    var realObjData = listRealityObjs.Select(x => new
                                                                      {
                                                                          АдресДома = x.Address,
                                                                          ГодПостройки = x.BuildYear.HasValue
                                                                            ? x.BuildYear.Value.ToString()
                                                                            : string.Empty,
                                                                          Этажность = x.MaximumFloors.HasValue
                                                                            ? x.MaximumFloors.Value.ToString()
                                                                            : string.Empty,
                                                                          МатериалСтен = x.WallMaterial != null
                                                                            ? x.WallMaterial.Name
                                                                            : string.Empty,
                                                                          Площадь = x.AreaMkd,
                                                                          ТипКровли = x.TypeRoof.GetEnumMeta().Display,
                                                                          ПлощадьОбщая = x.AreaLivingNotLivingMkd,
                                                                          КоличествоПодъездов = x.NumberEntrances.HasValue
                                                                            ? x.NumberEntrances.Value.ToString()
                                                                            : string.Empty
                                                                      }).ToList();
                    this.DataSources.Add(new MetaData
                    {
                        SourceName = "ЖилойДом",
                        MetaType = nameof(Object),
                        Data = realObjData
                    });
                }

                if (prescription.Contragent != null)
                {
                    this.ReportParams["Контрагент"] = prescription.Contragent.Name;
                    this.ReportParams["ИНН"] = prescription.Contragent.Inn;
                }

                var appCitsInfo = inspAppealCitsDomain.GetAll()
                    .Where(x => x.Inspection.Id == prescription.Inspection.Id)
                    .Select(x => new
                    {
                        x.AppealCits.NumberGji,
                        x.AppealCits.DateFrom,
                        x.AppealCits.Correspondent
                    })
                    .ToList()
                    .Select(x => new
                    {
                        НомерОбращения = string.Format("№ {0}", x.NumberGji),
                        ДатаОбращения = x.DateFrom.HasValue 
                            ? string.Format("от {0}", x.DateFrom.Value.ToShortDateString()) 
                            : string.Empty,
                        Корреспондент = x.Correspondent,
                    }).ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Обращения",
                    MetaType = nameof(Object),
                    Data = appCitsInfo
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
            }
        }

        /// <summary>
        /// Возвращаем код оснований проверки
        /// </summary>
        /// <param name="typeBase">Тип основания проверки</param>
        /// <returns>Код основания</returns>
        private string GetTypeBase(TypeBase typeBase)
        {
            switch (typeBase)
            {
                case TypeBase.CitizenStatement:
                    return "20";
                case TypeBase.PlanJuridicalPerson:
                    return "30";
                case TypeBase.DisposalHead:
                    return "40";
                case TypeBase.ProsecutorsClaim:
                    return "50";
                case TypeBase.PlanAction:
                    return "110";
            }

            return string.Empty;
        }

        protected class PrescriptionViolRecord
        {
            public int НомерПП { get; set; }

            public string Описание { get; set; }

            public string Пункты { get; set; }

            public string Мероприятие { get; set; }

            public string ДатаВыполнения { get; set; }
        }

        protected sealed class RealObjData
        {
            /// <summary>
            /// АдресДома.
            /// </summary>
            public string АдресДома { get; set; }

            public string ГодПостройки { get; set; }

            public string Этажность { get; set; }

            public string МатериалСтен { get; set; }

            public string Площадь { get; set; }

            public string ТипКровли { get; set; }

            public string ПлощадьОбщая { get; set; }
        }
    }
}