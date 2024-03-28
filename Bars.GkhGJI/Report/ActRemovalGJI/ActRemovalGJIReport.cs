namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Contracts;
    /// <summary>
    /// Акт проверки предписания
    /// </summary>
    public class ActRemovalGjiReport : GjiBaseReport
    {
        private long DocumentId { get; set; }
        /// <summary>
        /// Код шаблона
        /// </summary>
        protected override string CodeTemplate { get; set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        public ActRemovalGjiReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_ActSurvey_Prescription))
        {
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
                                   Name = "ActRemovalGJI",
                                   Description = "Акт проверки предписания",
                                   Template = Properties.Resources.BlockGJI_ActSurvey_Prescription
                               }
                       };
        }
        /// <summary>
        /// Подготовка данных для отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var act = Container.Resolve<IDomainService<ActRemoval>>().Get(DocumentId);

            if (act == null)
            {
                throw new ReportProviderException("Не удалось получить акт проверки");
            }

            CodeTemplate = "BlockGJI_ActSurvey_Prescription";

            // Заполняем общие поля
            FillCommonFields(reportParams, act);

            var dsDocumentChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            var prescriptionId = dsDocumentChildren.GetAll()
                    .Where(x => x.Children.Id == DocumentId && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                    .Select(x => x.Parent.Id)
                    .FirstOrDefault();

            var contragent = Container.Resolve<IDomainService<Prescription>>().GetAll()
                    .Where(x => x.Id == prescriptionId)
                    .Select(x => x.Contragent)
                    .FirstOrDefault();

            reportParams.SimpleReportParams["УправОрг(РП)"] = contragent != null ? contragent.NameGenitive : "";

            reportParams.SimpleReportParams["Площадь"] = act.Area;

            var actViols = Container.Resolve<IDomainService<ActRemovalViolation>>().GetAll()
                                .Where(x => x.Document.Id == act.Id)
                                .Select(x => x.InspectionViolation)
                                .ToArray();

            #region Секция нарушений

            //если нарушения не устранены
            if (act.TypeRemoval == YesNoNotSet.No && actViols.Length > 0)
            {
                var section = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияНарушений");

                var i = 0;

                var addedViols = new List<long>();

                foreach (var viol in actViols.Where(viol => !addedViols.Contains(viol.Id) && viol.DateFactRemoval == null))
                {
                    addedViols.Add(viol.Id);

                    section.ДобавитьСтроку();
                    section["Пункт"] = viol.Violation.CodePin;
                    section["ТекстНарушения"] = viol.Violation.Name;
                }
            }

            #endregion

            reportParams.SimpleReportParams["Адреса"] = actViols
                                .Select(x => new { x.RealityObject.Id, x.RealityObject.Address })
                                .Distinct(x => x.Id)
                                .ToList()
                                .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? "; " + y.Address : y.Address));

            reportParams.SimpleReportParams["ИнспекторыИКоды"] = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                                .Where(x => x.DocumentGji.Id == DocumentId)
                                .Select(x => new { x.Inspector.Fio, x.Inspector.Code })
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

            var parentActCheck =
                Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                         .Where(x => x.Children.Id == DocumentId && x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                         .Select(x => x.Parent.Id)
                         .FirstOrDefault();

            var witnesses = Container.Resolve<IDomainService<ActCheckWitness>>().GetAll()
                                     .Where(x => x.ActCheck.Id == parentActCheck)
                                     .Select(x => new { x.Fio, x.Position })
                                     .ToList();

            var positions = new StringBuilder();
            var fios = new StringBuilder();

            var fioPosition = new StringBuilder();

            foreach (var witness in witnesses)
            {
                if (!string.IsNullOrEmpty(witness.Position))
                {
                    if (positions.Length > 0)
                    {
                        positions.Append(", ");
                    }

                    positions.Append(witness.Position);
                }

                if (!string.IsNullOrEmpty(witness.Fio))
                {
                    if (fios.Length > 0)
                    {
                        fios.Append(", ");
                    }

                    fios.Append(witness.Fio);
                }

                var fioPos = string.Format("{0} {1}", witness.Position, witness.Fio);

                if (fioPos != " ")
                {
                    if (fioPosition.Length > 0)
                    {
                        fioPosition.Append(", ");
                    }

                    fioPosition.Append(fioPos);
                }
            }

            var baseDisposal = GetParentDocument(act, TypeDocumentGji.Disposal);
            var queryDisposal = Container.Resolve<IDomainService<Disposal>>().GetAll()
                                         .Where(x => x.Id == baseDisposal.Id);
            var issuedDisposal = queryDisposal.Select(x => x.IssuedDisposal).FirstOrDefault();
            var kindCheckDisposal = queryDisposal.Select(x => x.KindCheck).FirstOrDefault();

            if (issuedDisposal != null)
            {
                reportParams.SimpleReportParams["КодРуководителяФИО"] = string.Format(
                    "{0} {1}",
                    issuedDisposal.Position,
                    string.IsNullOrEmpty(issuedDisposal.ShortFio) ? issuedDisposal.Fio : issuedDisposal.ShortFio);

                if (!String.IsNullOrEmpty(issuedDisposal.PositionGenitive))
                    {
                        reportParams.SimpleReportParams["КодИнспектора(РодП)"] = issuedDisposal.PositionGenitive.ToLower();
                        reportParams.SimpleReportParams["КодИнспектора(ТворП)"] = issuedDisposal.PositionAblative.ToLower();
                    }

                reportParams.SimpleReportParams["Руководитель(РодП)"] = issuedDisposal.FioGenitive; 
                reportParams.SimpleReportParams["Руководитель(ТворП)"] = issuedDisposal.FioAblative;
                reportParams.SimpleReportParams["ВидОбследования(РП)"] = GetTypeCheckAblative(kindCheckDisposal);
            }

            if (baseDisposal != null)
            {
                reportParams.SimpleReportParams["ДатаРаспоряжения"] = baseDisposal.DocumentDate.HasValue
                                                                          ? baseDisposal.DocumentDate.ToDateTime().ToShortDateString()
                                                                          : string.Empty;
                reportParams.SimpleReportParams["НомерРаспоряжения"] = baseDisposal.DocumentNumber;
            }


            var inspectors = Container.Resolve<IDomainService<InspectionGjiInspector>>()
                                                           .GetAll()
                                                           .Where(x => x.Inspection.Id == baseDisposal.Inspection.Id)
                                                           .Select(x => x.Inspector)
                                                           .ToList();

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

                    realObjAddresses.AppendFormat("{0}, д.{1}", realityObject.FiasAddress.StreetName, realityObject.FiasAddress.House);
                }

                reportParams.SimpleReportParams["УлицаДом"] = String.Format("{0}.", realObjAddresses);
                reportParams.SimpleReportParams["ДомаИАдреса"] = String.Format("{0}, {1}. ", realObjs.FirstOrDefault().FiasAddress.PlaceName, realObjAddresses);
            }
          
            var firstRealObjPlaceName = Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
                                         .Where(x => x.Document.Id == act.Id)
                                         .Select(x => x.InspectionViolation.RealityObject.FiasAddress.PlaceName)
                                         .FirstOrDefault();

            reportParams.SimpleReportParams["НаселенныйПункт"] = firstRealObjPlaceName;

            reportParams.SimpleReportParams["ДЛприПроверкеДолжность"] = positions.ToString();
            reportParams.SimpleReportParams["ДЛприПроверкеФИО"] = fios.ToString();
            reportParams.SimpleReportParams["ДЛприПроверкеФИОДолжность"] = fioPosition.ToString();
            reportParams.SimpleReportParams["ДЛприПроверке"] = fioPosition.ToString();

            reportParams.SimpleReportParams["ПризнакВыполнения"] = act.TypeRemoval == YesNoNotSet.Yes ? "Да" : "Нет";


            var disposalText = Container.Resolve<IDisposalText>().SubjectiveCase;
            reportParams.SimpleReportParams["НомерАкта"] = act.DocumentNumber;
            reportParams.SimpleReportParams["Распоряжение"] = string.Format(
                    "{0} {1} от {2}",
                    disposalText,
                    baseDisposal.DocumentNum,
                    baseDisposal.DocumentDate.HasValue ? baseDisposal.DocumentDate.Value.ToShortDateString() : string.Empty);

            reportParams.SimpleReportParams["УправОрг"] = contragent != null ? contragent.Name : "";
            reportParams.SimpleReportParams["ИНН"] = contragent != null ? contragent.Inn : "";

            // пока пусто
            reportParams.SimpleReportParams["Мероприятия"] = string.Empty;
            reportParams.SimpleReportParams["СрокИсполнения"] = string.Empty;

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

                    appealAddress.AppendFormat("{0} (вх. {1}), проживающего по адресу {2}", appeal.Correspondent, appealsDocNumDateStr, addressRo);
                }
            }

            reportParams.SimpleReportParams["РеквизитыОбращения"] = appealAddress.ToString();

            var strPurposeDetails = string.Empty;
            var dispTypeDisposal = queryDisposal.Select(x => x.TypeDisposal).FirstOrDefault();
            if (act.Inspection.TypeBase == TypeBase.CitizenStatement && dispTypeDisposal != TypeDisposalGji.DocumentGji)
            {
                strPurposeDetails = reportParams.SimpleReportParams["РеквизитыОбращения"].ToString();
            }
            else
            {
                strPurposeDetails = reportParams.SimpleReportParams["ДомаИАдреса"].ToString();
            }

            reportParams.SimpleReportParams["ЦельРеквизиты"] = strPurposeDetails;

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
    }
}