namespace Bars.GkhGji.Regions.Tula.Report.ActRemovalGJI
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tula.Entities;
    using Bars.GkhGji.Report;
    using Stimulsoft.Report;

    public class ActRemovalGjiStimulReport : ActBaseStimulReport
    {
        #region .ctor & properties

        public ActRemovalGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_ActSurvey_Prescription))
        {
            ExportFormat = StiExportFormat.Word2007;
        }

        protected override string CodeTemplate { get; set; }

        public override string Id
        {
            get { return "ActRemoval"; }
        }

        public override string CodeForm
        {
            get { return "ActRemoval"; }
        }

        public override string Name
        {
            get { return "Акт проверки предписания"; }
        }

        public override string Description
        {
            get { return "Акт проверки предписания"; }
        }

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
                    Code = "BlockGJI_ActSurvey_Prescription",
                    Name = "ActRemoval",
                    Description = "Акт проверки предписания",
                    Template = Properties.Resources.BlockGJI_ActSurvey_Prescription
                }
            };
        }

        #endregion

        #region DomainServices

        public IDomainService<ActRemoval> ActRemovalDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<Prescription> PrescriptionDomain { get; set; }
        public IDomainService<DocumentViolGroup> DocumentViolGroupDomain { get; set; }
        public IDomainService<DocumentViolGroupPoint> DocumentViolGroupPointDomain { get; set; }
        public IDomainService<DocumentViolGroupLongText> DocumentViolGroupLongTextDomain { get; set; }

        #endregion

        private long DocumentId { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var act = ActRemovalDomain.Get(DocumentId);

            if (act == null)
            {
                throw new ReportProviderException("Не удалось получить акт проверки");
            }

            CodeTemplate = "BlockGJI_ActSurvey_Prescription";

            // Заполняем общие поля
            FillCommonFields(act);

            var prescriptionId = DocumentGjiChildrenDomain.GetAll()
                .Where(x => x.Children.Id == DocumentId && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                .Select(x => x.Parent.Id)
                .FirstOrDefault();

            var prescription = PrescriptionDomain.FirstOrDefault(x => x.Id == prescriptionId);
            if (prescription != null)
            {
                Report["НомерПредписания"] = prescription.DocumentNumber;
                Report["ДатаПредписания"] = DateToString(prescription.DocumentDate);
                if (prescription.Contragent != null)
                {
                    Report["Контрагент"] = prescription.Contragent.Name;
                    Report["ИНН"] = prescription.Contragent.Inn;
                    Report["ОГРН"] = prescription.Contragent.Ogrn;
                    Report["АдресЮР"] = prescription.Contragent.JuridicalAddress;
                }

                var prescrActCheck =
                    DocumentGjiChildrenDomain.GetAll().Where(x => x.Children.Id == prescriptionId
                        && x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck).Select(x => x.Parent).FirstOrDefault();
                if (prescrActCheck != null)
                {
                    Report["ДатаАктаПроверки"] = prescrActCheck.DocumentDate.HasValue
                        ? prescrActCheck.DocumentDate.Value.ToString("dd.MM.yyyy")
                        : string.Empty;
                    Report["НомерАктаПроверки"] = prescrActCheck.DocumentNumber;
                }
            }

            Report["ТипКонтрагента"] = act.Inspection.TypeJurPerson.GetEnumMeta().Display;
            Report["Площадь"] = act.Area;

            var actViols = Container.Resolve<IDomainService<ActRemovalViolation>>().GetAll()
                .Where(x => x.Document.Id == act.Id)
                .Select(x => x.InspectionViolation)
                .ToArray();

            Report["ДатаАкта"] = DateToString(act.DocumentDate);
            Report["ВремяСоставленияАкта"] = DateTimeToTimeString(act.DocumentDate);
            Report["НомерАкта"] = act.DocumentNumber;
            Report["Описание"] = act.Description;

            if (act.Inspection.TypeBase == TypeBase.PlanAction)
            {
                var basePlanAction = Container.Resolve<IDomainService<BasePlanAction>>().GetAll()
                    .FirstOrDefault(x => x.Id == act.Inspection.Id);

                if (basePlanAction != null)
                {
                    this.Report["План"] = basePlanAction.Plan != null ? basePlanAction.Plan.Name : string.Empty;
                }
            }

            var query = DocumentViolGroupDomain.GetAll()
                    .Where(x => x.Document.Id == DocumentId);

            var violPoints =
                DocumentViolGroupPointDomain.GetAll()
                               .Where(x => query.Any(y => y.Id == x.ViolGroup.Id))
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

            var longTexts = DocumentViolGroupLongTextDomain.GetAll()
                .Where(x => query.Any(y => y.Id == x.ViolGroup.Id))
                .Select(x => new
                {
                    x.ViolGroup.Id,
                    x.Description
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => new
                {
                    Description = x.Description != null ? Encoding.UTF8.GetString(x.Description).ToString(new CultureInfo("ru-RU")) : string.Empty
                }).First());

            var data = query
                .Select(x => new
                {
                    СрокУстранения = x.DatePlanRemoval.HasValue
                        ? x.DatePlanRemoval.Value.ToShortDateString()
                        : string.Empty,
                    ДатаФакт = x.DateFactRemoval.HasValue
                        ? x.DateFactRemoval.Value.ToShortDateString()
                        : string.Empty,
                    ПНД = violPoints.ContainsKey(x.Id) ? violPoints[x.Id].PointCodes : null,
                    Текст = longTexts.ContainsKey(x.Id) ? longTexts[x.Id].Description : x.Description
                })
                .ToList();

            Report.RegData("Нарушения", data);

            //if (actViols.Length > 0)
            //{
            //    Report.RegData("Нарушения", actViols.Where(x => x.DateFactRemoval != null).Select(x => new
            //    {
            //        Текст = x.Violation.Name,
            //        ПНД = x.Violation.CodePin,
            //        СрокУстранения = x.DatePlanRemoval.HasValue ? x.DatePlanRemoval.Value.ToString("dd.MM.yyyy") : string.Empty,
            //        ДатаФакт = x.DateFactRemoval.Value.ToString("dd.MM.yyyy"),
            //    }));
            //}

            Report["Адреса"] = actViols.Where(x => x.RealityObject != null)
                .Select(x => new { x.RealityObject.Id, x.RealityObject.Address })
                .Distinct(x => x.Id)
                .ToList()
                .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? "; " + y.Address : y.Address));


            var parentActCheck = DocumentGjiChildrenDomain.GetAll()
                    .Where(
                        x => x.Children.Id == DocumentId && x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck).Select(x => x.Parent).FirstOrDefault();
            if (parentActCheck != null)
            {
                #region Лица, присутствовавшие при проверке

                var witnesses = Container.Resolve<IDomainService<ActCheckWitness>>().GetAll()
                    .Where(x => x.ActCheck.Id == parentActCheck.Id)
                    .ToList();


                RegWitnessesDataSource(witnesses);
                RegWitnessesDataSource(witnesses.Where(x => !x.IsFamiliar), "Отказ");

                #endregion

                #region ПрилагаемыеДокументы

                var annexDomain = Container.ResolveDomain<ActCheckAnnex>();
                using (Container.Using(annexDomain))
                {
                    RegAttachmentsDataSource(annexDomain.GetAll().Where(x => x.ActCheck.Id == parentActCheck.Id));
                }

                #endregion

                #region Периоды проверки

                var actCheckPeriodDomain = Container.ResolveDomain<ActCheckPeriod>();
                using (Container.Using(actCheckPeriodDomain))
                {
                    var periods =
                        actCheckPeriodDomain.GetAll()
                            .Where(x => x.ActCheck.Id == parentActCheck.Id)
                            .Where(x => x.DateEnd.HasValue && x.DateStart.HasValue)
                            .ToArray();
                    RegPeriodDataSource(periods, "ПериодПроверки");
                    RegDurationDataSource(periods.Where(x => x.DateStart.HasValue).Where(x => x.DateEnd.HasValue).Select(x => new Period
                    {
                        End = x.DateEnd.Value,
                        Start = x.DateStart.Value
                    }));
                }

                #endregion
            }


            var baseDisposal = (Disposal)GetParentDocument(act, TypeDocumentGji.Disposal);
            
            if (baseDisposal != null)
            {
                var queryDisposal = Container.Resolve<IDomainService<Disposal>>().GetAll()
                    .Where(x => x.Id == baseDisposal.Id);
                var kindCheckDisposal = queryDisposal.Select(x => x.KindCheck).FirstOrDefault();

                Report["ВидПроверки"] = kindCheckDisposal != null ? kindCheckDisposal.Name : string.Empty;
                Report["ОснованиеПроверки"] = baseDisposal.Return(x => x.Inspection) != null
                                                  ? Convert.ChangeType(
                                                      baseDisposal.Inspection.TypeBase,
                                                      baseDisposal.Inspection.TypeBase.GetTypeCode())
                                                  : string.Empty;

                Report["ДатаРаспоряжения"] = baseDisposal.DocumentDate.HasValue
                    ? baseDisposal.DocumentDate.ToDateTime().ToShortDateString()
                    : string.Empty;
                Report["НомерРаспоряжения"] = baseDisposal.DocumentNumber;
                Report["ДолжностьПриказРП"] = baseDisposal.IssuedDisposal != null
                    ? !string.IsNullOrEmpty(baseDisposal.IssuedDisposal.PositionGenitive)
                        ? baseDisposal.IssuedDisposal.PositionGenitive
                        : baseDisposal.IssuedDisposal.Position
                    : string.Empty;

                Report["РуководительПриказРП"] = baseDisposal.IssuedDisposal != null
                    ? !string.IsNullOrEmpty(baseDisposal.IssuedDisposal.FioGenitive)
                        ? baseDisposal.IssuedDisposal.FioGenitive
                        : baseDisposal.IssuedDisposal.Fio
                    : string.Empty;
            }

            var inspectors = Container.Resolve<IDomainService<DocumentGjiInspector>>()
                .GetAll()
                .Where(x => x.DocumentGji.Id == act.Id)
                .Select(x => x.Inspector)
                .ToList();
            RegInspectorsDataSource(inspectors);

            if (baseDisposal != null)
            {
                var realObjs = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                    .Where(x => x.Inspection.Id == baseDisposal.Inspection.Id)
                    .Select(x => x.RealityObject)
                    .ToList();

                var realObjAddresses = new StringBuilder();

                if (realObjs.Count > 0)
                {
                    foreach (var realityObject in realObjs)
                    {
                        if (realObjAddresses.Length > 0)
                        {
                            realObjAddresses.Append("; ");
                        }

                        realObjAddresses.AppendFormat(
                            "{0}, д.{1}", 
                            realityObject.FiasAddress.StreetName,
                            realityObject.FiasAddress.House);
                    }

                    Report["УлицаДом"] = string.Format("{0}.", realObjAddresses);
                    Report["ДомАдрес"] = string.Format(
                        "{0}, {1}. ", 
                        realObjs.FirstOrDefault().FiasAddress.PlaceName,
                        realObjAddresses);
                }

                var disposalText = Container.Resolve<IDisposalText>().SubjectiveCase;
                Report["Распоряжение"] = string.Format(
                    "{0} {1} от {2}",
                    disposalText,
                    baseDisposal.DocumentNum,
                    baseDisposal.DocumentDate.HasValue 
                        ? baseDisposal.DocumentDate.Value.ToShortDateString() 
                        : string.Empty);
            }

            var firstRealObjPlaceName = Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
                .Where(x => x.Document.Id == act.Id)
                .Select(x => x.InspectionViolation.RealityObject.FiasAddress.PlaceName)
                .FirstOrDefault();

            Report["НаселенныйПункт"] = firstRealObjPlaceName;

            Report["ПризнакВыполнения"] = act.TypeRemoval == YesNoNotSet.Yes ? "Да" : "Нет";


            // пока пусто
            Report["Мероприятия"] = string.Empty;
            Report["СрокИсполнения"] = string.Empty;

            var quryAppeals = Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                .Where(x => x.Inspection.Id == act.Inspection.Id);

            var appeals = quryAppeals.Select(x => new
            {
                appealId = x.AppealCits.Id,
                x.AppealCits.Correspondent,
                x.AppealCits.DocumentNumber,
                x.AppealCits.DateFrom
            })
                .ToList();

            var quryAppealsId = quryAppeals.Select(x => x.AppealCits.Id);

            var appealsRo = Container.Resolve<IDomainService<AppealCitsRealityObject>>()
                .GetAll()
                .Where(x => quryAppealsId.Contains(x.AppealCits.Id))
                .Select(x => new
                {
                    appId = x.AppealCits.Id,
                    muName = x.RealityObject.Municipality.Name,
                    address = x.RealityObject.Address
                })
                .ToList();

            var appealAddress = new StringBuilder();
            foreach (var appeal in appeals)
            {
                var addressRo = string.Empty;
                var appealRo = appealsRo.Where(x => x.appId == appeal.appealId).ToList();
                if (appealRo.Count > 0)
                {
                    var item = appealRo.First();
                    addressRo = string.Format("{0}, {1}", item.muName, item.address);
                }

                var appealsDocNumDateStr = string.Empty;
                if (!string.IsNullOrEmpty(appeal.DocumentNumber))
                {
                    appealsDocNumDateStr = string.Format("{0} от {1}",
                        appeal.DocumentNumber,
                        appeal.DateFrom.HasValue ? appeal.DateFrom.Value.ToShortDateString() : string.Empty);
                }

                if (!string.IsNullOrEmpty(appeal.Correspondent))
                {
                    if (appealAddress.Length > 0) appealAddress.Append("; ");

                    appealAddress.AppendFormat("{0} (вх. {1}), проживающего по адресу {2}", appeal.Correspondent,
                        appealsDocNumDateStr, addressRo);
                }
            }

            Report["РеквизитыОбращения"] = appealAddress.ToString();
        }
    }
}