namespace Bars.GkhGji.Report
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActSurveyGjiReport : GjiBaseReport
    {
        private long DocumentId { get; set; }
        protected override string CodeTemplate { get; set; }

        public ActSurveyGjiReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_ActSurveyVis))
        {
        }

        public override string Id
        {
            get { return "ActSurvey"; }
        }

        public override string CodeForm
        {
            get { return "ActSurvey"; }
        }

        public override string Name
        {
            get { return "Акт обследования"; }
        }

        public override string Description
        {
            get { return "Акт обследования"; }
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
                            Code = "BlockGJI_ActSurveyVis",
                            Name = "ActSurveyGJI",
                            Description = string.Empty,
                            Template = Properties.Resources.BlockGJI_ActSurveyVis
                        }
                };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var actSurvey = Container.Resolve<IDomainService<ActSurvey>>().Load(DocumentId);
            if (actSurvey == null)
            {
                throw new ReportProviderException("Не удалось получить акт обследования");
            }

            // Заполняем общие поля
            FillCommonFields(reportParams, actSurvey);

            var dsActRobject = Container.Resolve<IDomainService<ActSurveyRealityObject>>();

            var countRo = dsActRobject.GetAll().Count(x => x.ActSurvey.Id == actSurvey.Id);

            reportParams.SimpleReportParams["НомерАкта"] = actSurvey.DocumentNumber;
            reportParams.SimpleReportParams["ДатаАкта"] = actSurvey.DocumentDate.HasValue
                                                                          ? actSurvey.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU"))
                                                                          : string.Empty;

            if (countRo == 1)
            {
                var realityObject = dsActRobject.GetAll()
                                   .Where(x => x.ActSurvey.Id == actSurvey.Id)
                                   .Select(x => x.RealityObject)
                                   .FirstOrDefault();

                reportParams.SimpleReportParams["АдресДома"] = realityObject.FiasAddress != null ? realityObject.FiasAddress.AddressName : realityObject.Address;

                reportParams.SimpleReportParams["Этажей"] = realityObject.Floors.HasValue
                                                                ? realityObject.Floors.Value.ToStr()
                                                                : string.Empty;
                reportParams.SimpleReportParams["КоличествоКвартир"] = realityObject.NumberApartments.HasValue
                                                                ? realityObject.NumberApartments.Value.ToStr()
                                                                : string.Empty;
                reportParams.SimpleReportParams["ОбщаяПлощадь"] = realityObject.AreaMkd.HasValue
                                                                ? realityObject.AreaMkd.Value.ToStr()
                                                                : string.Empty;

                reportParams.SimpleReportParams["АдресДома1"] = string.Format("{0}, {1}, д.{2} ", realityObject.FiasAddress.PlaceName, realityObject.FiasAddress.StreetName, realityObject.FiasAddress.House);

            }

            reportParams.SimpleReportParams["ИнспектируемаяЧасть"] =
                Container.Resolve<IDomainService<ActSurveyInspectedPart>>().GetAll()
                         .Where(x => x.ActSurvey.Id == actSurvey.Id)
                         .Select(x => x.InspectedPart.Name)
                         .ToList()
                         .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

            reportParams.SimpleReportParams["ОбследПлощадь"] = actSurvey.Area.HasValue ? actSurvey.Area.Value.ToString() : string.Empty;

            var listInspectors = Container.Resolve<IDomainService<DocumentGjiInspector>>()
                                .GetAll()
                                .Where(x => x.DocumentGji.Id == actSurvey.Id)
                                .Select(x => new
                                                {
                                                    x.Inspector.Fio,
                                                    x.Inspector.FioAblative,
                                                    x.Inspector.PositionAblative
                                                })
                                .ToList();

            var fioPositionAblativeStr = listInspectors.Aggregate( string.Empty, 
                                                                   (t, n) => t + n.FioAblative + " - " + n.PositionAblative + ", ", 
                                                                   x => x.Substring(0, x.Length - 2));

            var fioStr = listInspectors.Aggregate(string.Empty, 
                                                  (t, n) => t + n.Fio + ", ", 
                                                  x => x.Substring(0, x.Length - 2));

            reportParams.SimpleReportParams["ИнспекторДолжТворП"] = fioPositionAblativeStr;
            reportParams.SimpleReportParams["ИнспекторФИО"] = fioStr;

            var queryDispId = Container.Resolve<IDomainService<DocumentGjiChildren>>()
                                    .GetAll()
                                    .Where(x => x.Children.Id == actSurvey.Id)
                                    .Select(x => x.Parent.Id);

            var disposal = Container.Resolve<IDomainService<Disposal>>()
                                 .GetAll()
                                 .Where(x => queryDispId.Contains(x.Id))
                                 .OrderBy(x => x.Id)
                                 .AsEnumerable()
                                 .LastOrDefault();

            if (disposal != null)
            {
                reportParams.SimpleReportParams["ДатаПриказа"] = disposal.DocumentDate.ToDateTime().ToString("d MMMM yyyy");
                reportParams.SimpleReportParams["НомерПриказа"] = disposal.DocumentNumber;

                reportParams.SimpleReportParams["ДатаАкта1"] = disposal.DateStart.ToDateTime().ToShortDateString();

                if (disposal.IssuedDisposal != null)
                {
                    reportParams.SimpleReportParams["РуководительФИО"] =  disposal.IssuedDisposal.Fio;
                    reportParams.SimpleReportParams["КодИнспектора(ТворП)"] = disposal.IssuedDisposal.PositionAblative.ToLower();
                    reportParams.SimpleReportParams["Руководитель(ТворП)"] = disposal.IssuedDisposal.FioAblative;
                }

                reportParams.SimpleReportParams["ВидОбследования(РП)"] = GetTypeCheckAblative(disposal.KindCheck);
            }

            FillRegionParams(reportParams, actSurvey);
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