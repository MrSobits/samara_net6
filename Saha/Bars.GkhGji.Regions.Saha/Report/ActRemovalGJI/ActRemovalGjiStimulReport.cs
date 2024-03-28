namespace Bars.GkhGji.Regions.Saha.Report.ActRemovalGJI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Report;

    public class ActRemovalGjiStimulReport : ActBaseStimulReport
    {
        private long DocumentId { get; set; }

        protected override string CodeTemplate { get; set; }

        public ActRemovalGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_ActSurvey_Prescription))
        {
            ExportFormat = StiExportFormat.Word2007;
        }

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

        public override void PrepareReport(ReportParams reportParams)
        {
            var act = Container.Resolve<IDomainService<ActRemoval>>().Get(DocumentId);

            if (act == null)
            {
                throw new ReportProviderException("Не удалось получить акт проверки");
            }

            CodeTemplate = "BlockGJI_ActSurvey_Prescription";

            // Заполняем общие поля
            FillCommonFields(act);

            var dsDocumentChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            var prescriptionId = dsDocumentChildren.GetAll()
                .Where(x => x.Children.Id == DocumentId && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                .Select(x => x.Parent.Id)
                .FirstOrDefault();


            var prescription =
                Container.Resolve<IDomainService<Prescription>>().FirstOrDefault(x => x.Id == prescriptionId);
            if (prescription != null)
            {
                this.ReportParams["НомерПредписания"] = prescription.DocumentNumber;
                this.ReportParams["ДатаПредписания"] = DateToString(prescription.DocumentDate);
                if (prescription.Contragent != null)
                {
                    this.ReportParams["Контрагент"] = prescription.Contragent.Name;
                }

                var prescrActCheck =
                    Container.ResolveDomain<DocumentGjiChildren>().GetAll().Where(x => x.Children.Id == prescriptionId
                        && x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck).Select(x => x.Parent).FirstOrDefault();
                if (prescrActCheck != null)
                {
                    this.ReportParams["ДатаАктаПроверки"] = prescrActCheck.DocumentDate.HasValue
                        ? prescrActCheck.DocumentDate.Value.ToString("dd.MM.yyyy")
                        : string.Empty;
                    this.ReportParams["НомерАктаПроверки"] = prescrActCheck.DocumentNumber;
                }
            }

            this.ReportParams["ТипКонтрагента"] = act.Inspection.TypeJurPerson.GetEnumMeta().Display;
            this.ReportParams["Площадь"] = act.Area.ToString();

            var actViols = Container.Resolve<IDomainService<ActRemovalViolation>>().GetAll()
                .Where(x => x.Document.Id == act.Id)
                .Select(x => x.InspectionViolation)
                .ToArray();

            this.ReportParams["ДатаАкта"] = DateToString(act.DocumentDate);
            this.ReportParams["ВремяСоставленияАкта"] = DateTimeToTimeString(act.DocumentDate);
            this.ReportParams["НомерАкта"] = act.DocumentNumber;
            this.ReportParams["Описание"] = act.Description;

            #region Нарушения

            var violPiontDomain = Container.Resolve<IDomainService<Entities.DocumentViolGroupPoint>>();
            var docViolGroupLongTextDomain = Container.Resolve<IDomainService<Entities.DocumentViolGroupLongText>>();

            var docViolGroups = Container.Resolve<IDomainService<Entities.DocumentViolGroup>>().GetAll()
                    .Where(x => x.Document.Id == act.Id);
            
            var violPoints = violPiontDomain.GetAll()
                .Where(x => docViolGroups.Any(y => y.Id == x.ViolGroup.Id))
                .Select(x => new
                    {
                        x.ViolStage.InspectionViolation.Violation.CodePin,
                        violGroupId = x.ViolGroup.Id
                    })
                .AsEnumerable()
                .GroupBy(x => x.violGroupId)
                .ToDictionary(
                    x => x.Key,
                    y => new
                    {
                        PointCodes = y.Select(z => z.CodePin).Distinct()
                            .Aggregate((str, result) => !string.IsNullOrEmpty(result) ? result + "," + str : str)
                    });

            // описание нарушений (пнд, кратное описание)
            var data = docViolGroups
                    .Select(x => new
                    {
                        x.Id,
                        x.Description,
                        x.DatePlanRemoval,
                        x.DateFactRemoval
                    })
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        x.Description,
                        x.DatePlanRemoval,
                        x.DateFactRemoval,
                        PointCodes = violPoints.ContainsKey(x.Id) ? violPoints[x.Id].PointCodes : string.Empty
                    })
                    .ToArray();

            // полное описание нарушений
            var longTexts = docViolGroupLongTextDomain.GetAll()
                .Where(x => docViolGroups.Any(y => y.Id == x.ViolGroup.Id))
                .Select(x => new { x.ViolGroup.Id, x.Description })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.Select(x => new
                    {
                        Description = x.Description != null ? Encoding.UTF8.GetString(x.Description) : string.Empty
                    }).First());

            // если нарушения не устранены
            if (data.Length > 0)
            {
                var actViolsData = data.Select(x =>
                    {
                        var text = string.Empty;

                        if (longTexts.ContainsKey(x.Id) && longTexts[x.Id].Description.IsNotEmpty())
                        {
                            text = longTexts[x.Id].Description;
                        }
                        else
                        {
                            text = x.Description;
                        }

                        return new
                        {
                            Текст = text,
                            ПНД = x.PointCodes,
                            СрокУстранения = x.DatePlanRemoval.HasValue 
                                ? x.DatePlanRemoval.Value.ToString("dd.MM.yyyy") 
                                : string.Empty,
                            ДатаФакт = x.DateFactRemoval.HasValue 
                                ? x.DateFactRemoval.Value.ToString("dd.MM.yyyy") 
                                : null,
                        };
                    })
                    .ToList();
                this.DataSources.Add(new MetaData
                {
                    SourceName = "Нарушения",
                    MetaType = nameof(Object),
                    Data = actViolsData
                });
            }

            #endregion

            this.ReportParams["Адреса"] = actViols
                .Select(x => new { x.RealityObject.Id, x.RealityObject.Address })
                .Distinct(x => x.Id)
                .ToList()
                .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? "; " + y.Address : y.Address));


            var parentActCheck =
                Container.Resolve<IDomainService<DocumentGjiChildren>>()
                    .GetAll()
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


            var baseDisposal = GetParentDocument(act, TypeDocumentGji.Disposal);
            var queryDisposal = Container.Resolve<IDomainService<Disposal>>().GetAll()
                .Where(x => x.Id == baseDisposal.Id);
            var kindCheckDisposal = queryDisposal.Select(x => x.KindCheck).FirstOrDefault();

            this.ReportParams["ВидПроверки"] = kindCheckDisposal != null ? kindCheckDisposal.Name : string.Empty;


            if (baseDisposal != null)
            {
                this.ReportParams["ДатаРаспоряжения"] = baseDisposal.DocumentDate.HasValue
                    ? baseDisposal.DocumentDate.ToDateTime().ToShortDateString()
                    : string.Empty;
                this.ReportParams["НомерРаспоряжения"] = baseDisposal.DocumentNumber;
            }


            var inspectors = Container.Resolve<IDomainService<DocumentGjiInspector>>()
                .GetAll()
                .Where(x => x.DocumentGji.Id == act.Id)
                .Select(x => x.Inspector)
                .ToList();
            RegInspectorsDataSource(inspectors);

            var realObjs = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                .Where(x => x.Inspection.Id == baseDisposal.Inspection.Id)
                .Select(x => x.RealityObject)
                .ToList();

            var realObjAddresses = new StringBuilder();

            if (realObjs.Count > 0)
            {
                //realObjsWithMunicipality.AppendFormat("{0}, ", realObjs.FirstOrDefault().FiasAddress.PlaceName);
                foreach (var realityObject in realObjs)
                {
                    if (realObjAddresses.Length > 0)
                    {
                        realObjAddresses.Append("; ");
                    }

                    realObjAddresses.AppendFormat("{0}, д.{1}", realityObject.FiasAddress.StreetName,
                        realityObject.FiasAddress.House);
                }

                this.ReportParams["УлицаДом"] = String.Format("{0}.", realObjAddresses);
                this.ReportParams["ДомАдрес"] = String.Format("{0}, {1}. ", realObjs.FirstOrDefault().FiasAddress.PlaceName,
                    realObjAddresses);
            }

            var firstRealObjPlaceName = Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
                .Where(x => x.Document.Id == act.Id)
                .Select(x => x.InspectionViolation.RealityObject.FiasAddress.PlaceName)
                .FirstOrDefault();

            this.ReportParams["НаселенныйПункт"] = firstRealObjPlaceName;


            this.ReportParams["ПризнакВыполнения"] = act.TypeRemoval == YesNoNotSet.Yes ? "Да" : "Нет";


            var disposalText = Container.Resolve<IDisposalText>().SubjectiveCase;
            this.ReportParams["Распоряжение"] = string.Format(
                "{0} {1} от {2}",
                disposalText,
                baseDisposal.DocumentNum,
                baseDisposal.DocumentDate.HasValue ? baseDisposal.DocumentDate.Value.ToShortDateString() : string.Empty);

            // пока пусто
            this.ReportParams["Мероприятия"] = string.Empty;
            this.ReportParams["СрокИсполнения"] = string.Empty;

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

            this.ReportParams["РеквизитыОбращения"] = appealAddress.ToString();


        }

        private string GetTypeCheckAblative(KindCheckGji kindCheck)
        {
            var result = "";

            var dictTypeCheckAblative = new Dictionary<TypeCheck, string>()
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
    }
}