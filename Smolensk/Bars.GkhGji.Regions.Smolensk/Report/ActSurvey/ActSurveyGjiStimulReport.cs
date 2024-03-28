namespace Bars.GkhGji.Regions.Smolensk.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    public class ActSurveyGjiStimulReport : GjiBaseStimulReport
    {
        #region .ctor
        public ActSurveyGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.ActSurvey))
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

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        protected void GetCodeTemplate()
        {
            CodeTemplate = "ActSurvey";
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "ActSurvey",
                            Name = "Акт обследования",
                            Description = "Акт обследования",
                            Template = Properties.Resources.ActSurvey
                        }
                };
        }

        protected override string CodeTemplate { get; set; }

        #endregion


        #region Fields

        private long DocumentId { get; set; }

        #endregion


        #region DomainServices

        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomain { get; set; }
        public IDomainService<ActSurveyRealityObject> ActSurveyRealityObjectDomain { get; set; }
        public IDomainService<ActSurvey> ActSurveyDomain { get; set; }
        public IDomainService<ActSurveyLongDescription> ActSurveyLongDescription { get; set; }

        #endregion

        public override void PrepareReport(ReportParams reportParams)
        {
            var actSurvey = ActSurveyDomain.Load(DocumentId);
            if (actSurvey == null)
            {
                throw new ReportProviderException("Не удалось получить акт обследования");
            }
            FillCommonFields(actSurvey);

            this.ReportParams["НомерАкта"] = actSurvey.DocumentNumber;
            this.ReportParams["ДатаАкта"] = actSurvey.DocumentDate.HasValue
                    ? actSurvey.DocumentDate.Value.ToString("d MMMM yyyy")
                    : string.Empty;

            var actSurvLong =
                ActSurveyLongDescription.GetAll()
                    .FirstOrDefault(x => x.ActSurvey.Id == actSurvey.Id);

            var longDescription = string.Empty;

            if (actSurvLong != null)
            {
                longDescription = Encoding.UTF8.GetString(actSurvLong.Description);
            }

            this.ReportParams["Описание"] = longDescription.IsNotEmpty()
                ? longDescription
                : actSurvey.Description;

            var houseInfo = new StringBuilder();
            var realityObject = ActSurveyRealityObjectDomain.GetAll().Where(x => x.ActSurvey.Id == actSurvey.Id).Select(x => x.RealityObject).FirstOrDefault();

            if (realityObject != null)
            {
                this.ReportParams["АдресДома1"] = realityObject.Address;

                this.ReportParams["НаселенныйПункт"] = realityObject.FiasAddress.PlaceName;


                houseInfo.Append("Тип дома: " + realityObject.TypeHouse.GetEnumMeta().Display);
                if (realityObject.BuildYear.HasValue)
                {
                    houseInfo.Append(", ");
                    houseInfo.Append("год постройки: " + realityObject.BuildYear.Value);
                }
                if (realityObject.AreaMkd.HasValue)
                {
                    houseInfo.Append(", ");
                    houseInfo.Append("общая площадь: " + realityObject.AreaMkd.Value.RoundDecimal(1));
                }
                if (realityObject.Floors.HasValue)
                {
                    houseInfo.Append(", ");
                    houseInfo.Append("этажность: " + realityObject.Floors.Value);
                }
                if (realityObject.NumberEntrances.HasValue)
                {
                    houseInfo.Append(", ");
                    houseInfo.Append("количество подъездов: " + realityObject.NumberEntrances.Value);
                }
                if (realityObject.RoofingMaterial != null)
                {
                    houseInfo.Append(", ");
                    houseInfo.Append("материал кровли: " + realityObject.RoofingMaterial.Name);
                }
                if (realityObject.WallMaterial != null)
                {
                    houseInfo.Append(", ");
                    houseInfo.Append("материал стен: " + realityObject.WallMaterial.Name);
                }
                houseInfo.Append(", ");
                houseInfo.Append("тип кровли: " + realityObject.TypeRoof.GetEnumMeta().Display);
                houseInfo.Append(", ");
                houseInfo.Append("система отопления: " + realityObject.HeatingSystem.GetEnumMeta().Display);

                this.ReportParams["Дом"] = houseInfo.ToString();
            }

            // Инспекторы
            var inspectors =
                DocumentGjiInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == actSurvey.Id)
                    .Select(x => new
                    {
                        x.Inspector.Position,
                        x.Inspector.Fio,
                        x.Inspector.PositionAblative,
                        x.Inspector.FioAblative
                    })
                    .ToList();

            this.ReportParams["ДолжностьИнспектор"] = inspectors
                .AggregateWithSeparator(x =>
                    (!x.PositionAblative.IsEmpty() ? x.PositionAblative : x.Fio) 
                    + " - " +
                    (!x.FioAblative.IsEmpty() ? x.FioAblative : x.Fio), 
                    ", ");


            var inspectorList = inspectors
                .Select(inspector => new InspectorProxy
                {
                    ФиоИнспектора = inspector.Fio,
                    ДолжностьИнспектора = inspector.Position
                })
                .ToList();

            this.DataSources.Add(new MetaData
            {
                SourceName = "Инспекторы",
                MetaType = nameof(InspectorProxy),
                Data = inspectorList
            });
        }

        protected class InspectorProxy
        {
            public string ФиоИнспектора { get; set; }
            public string ДолжностьИнспектора { get; set; }
        }
    }
}