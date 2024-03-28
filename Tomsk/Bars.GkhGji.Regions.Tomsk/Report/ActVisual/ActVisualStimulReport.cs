namespace Bars.GkhGji.Regions.Tomsk.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using B4;
    using B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using B4.Utils;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.GkhGji.Report;
    using Gkh.Entities;
    using Gkh.Overhaul.Entities;
    using Gkh.Report;
    using Gkh.Utils;
    using GkhGji.Entities;
    using Entities;

    public class ActVisualStimulReport : GjiBaseStimulReport
    {
        private readonly Dictionary<int, string> _floorsWords = new Dictionary<int, string>
        {
            {1, "одноэтажный"},
            {2, "двухэтажный"},
            {3, "трежэтажный"},
            {4, "четырехэтажный"},
            {5, "пятиэтажный"},
            {6, "шестиэтажный"},
            {7, "семиэтажный"},
            {8, "восьмиэтажный"},
            {9, "девятиэтажный"},
            {10, "десятиэтажный"},
            {11, "одиннадцатиэтажный"},
            {12, "двенадцатиэтажный"},
            {13, "тринадцатиэтажный"},
            {14, "четырнадцатиэтажный"},
            {15, "пятнадцатиэтажный"},
            {16, "шестнадцатиэтажный"},
            {17, "семнадцатиэтажный"},
            {18, "восемнадцатиэтажный"},
            {19, "девятнадцатиэтажный"},
            {20, "двадцатиэтажный"},
            {21, "двадцатиодноэтажный"},
            {22, "двадцатидвухэтажный"},
            {23, "двадцатитрехэтажный"},
            {24, "двадцатичетырехэтажный"},
            {25, "двадцатипятиэтажный}"}
        };

        public ActVisualStimulReport() : base(new ReportTemplateBinary(Properties.Resources.ActVisual))
        {

        }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<RealityObjectStructuralElement> RobjectSeDomain { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        protected long ActVisualId { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            ActVisualId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var actVisual = Container.ResolveDomain<ActVisual>().Get(ActVisualId);

            if (actVisual == null)
            {
                return;
            }

            FillCommonFields(actVisual);

            var documentGjiInspector = DocumentInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == actVisual.Id)
                .Select(x => new
                {
                    x.Inspector.Id,
                    x.Inspector.Fio
                })
                .FirstOrDefault();

            var actVisualDate = actVisual.DocumentDate;
            var realObjAddress = actVisual.ReturnSafe(x => x.RealityObject.Address);
            var inspectionResult = actVisual.InspectionResult;
            var conclusion = actVisual.Conclusion;
            var rameVerification = string.Empty;
            var rameVerificationCode = string.Empty;

            if (actVisual.FrameVerification != null)
            {
                rameVerification = actVisual.FrameVerification.Name;
                rameVerificationCode = actVisual.FrameVerification.Code;
            }

            var flatNum = actVisual.Flat;
            var actHour = actVisual.Hour ?? 0;
            var actMinute = actVisual.Minute ?? 0;

            var acrVisualNum = actVisual.DocumentNumber;

            var objectCreateDate = actVisual.Inspection.ObjectCreateDate.ToShortDateString();
            var inspectionNumber = actVisual.Inspection.InspectionNumber ?? string.Empty;

            this.ReportParams["ДатаАктаВизОсм"] = actVisualDate.ToDateString("«dd» MMMM yyyy");
            this.ReportParams["ВремяАктаВизОсм"] = string.Format("{0} час. {1} мин.", actHour.ToString("D2"), actMinute.ToString("D2"));

            if (documentGjiInspector != null)
            {
                this.ReportParams["ИнспекторАктаВизОсм"] = CutFio(documentGjiInspector.Fio ?? string.Empty);
            }

            if (actVisual.RealityObject != null)
            {
                var floor = actVisual.RealityObject.Floors;

                if (floor.HasValue)
                {
                    this.ReportParams["ЭтажейПропис"] = _floorsWords.Get(floor.Value);
                }

                if (actVisual.RealityObject.WallMaterial != null)
                {
                    this.ReportParams["Стена"] = actVisual.RealityObject.WallMaterial.Name;
                }
            }

            var dispId = actVisual.Inspection.Id;

            var disposalData = DisposalDomain.GetAll()
                .Where(x => x.Inspection.Id == dispId)
                .Select(x => new
                {
                    x.DocumentNumber,
                    x.DocumentDate
                })
                .FirstOrDefault();

            if (disposalData != null)
            {
                this.ReportParams["НомерПриказа"] = disposalData.DocumentNumber;
                this.ReportParams["ДатаПриказа"] = disposalData.DocumentDate.ToDateString();
            }

            if (rameVerificationCode == "2")
            {
                var docNum = string.Empty;
                var docDate = string.Empty;
                if (disposalData != null)
                {
                    docNum = disposalData.DocumentNumber;
                    docDate = disposalData.DocumentDate.ToDateString();
                }
                this.ReportParams["РамкиПроверкиАктаВизОсм"] =
                    string.Format("в рамках проверки (распоряжение № {0} от {1})",
                        docNum,
                        docDate);
            }
            else if (rameVerificationCode == "11")
            {
                this.ReportParams["РамкиПроверкиАктаВизОсм"] =
                    string.Format("по требованию прокуратуры Томской области/ (вх от {0} № {1} жалоба)",
                        objectCreateDate,
                        inspectionNumber);
            }
            else
            {
                this.ReportParams["РамкиПроверкиАктаВизОсм"] = rameVerification ?? string.Empty;
            }

            this.ReportParams["НомерКвартиры"] = flatNum;

            if (!realObjAddress.IsEmpty())
            {
                if (!flatNum.IsEmpty())
                {
                    this.ReportParams["ДомИлиКвартира"] = "квартиры № ";
                }
                else
                {
                    this.ReportParams["ДомИлиКвартира"] = "жилого дома";
                }
            }

            this.ReportParams["АдресДомаАктаВизОсм"] = realObjAddress ?? string.Empty;
            this.ReportParams["КвАдресДомаАктаВизОсм"] = flatNum ?? string.Empty;
            this.ReportParams["РезультатПроверкиАктаВизОсм"] = inspectionResult ?? string.Empty;
            this.ReportParams["ВыводыАктаВизОсм"] = conclusion ?? string.Empty;

            this.ReportParams["ДатаСозданияНомерОбращения"] = objectCreateDate + " " + inspectionNumber;

            this.ReportParams["НомерАктаВизОсм"] = acrVisualNum;

            var inspectorData = InspectorDomain.GetAll()
                .Where(x => x.Id == documentGjiInspector.Id)
                .Select(x => new
                {
                    x.PositionAblative,
                    x.PositionDative,
                    x.Position,
                    x.FioDative,
                    x.FioAblative
                })
                .FirstOrDefault();

            if (inspectorData != null)
            {
                this.ReportParams["ДолжностьИнспектораАктаВизОсмДП"] =
                    inspectorData.PositionDative.IsEmpty()
                        ? inspectorData.Position
                        : inspectorData.PositionDative;

                this.ReportParams["ДолжностьИнспектораАктаВизОсмТП"] =
                    inspectorData.PositionAblative.IsEmpty()
                        ? inspectorData.Position
                        : inspectorData.PositionAblative;

                this.ReportParams["ИнспекторАктаВизОсмДП"] =
                    inspectorData.FioDative.IsEmpty()
                        ? documentGjiInspector.Fio
                        : CutFio(inspectorData.FioDative);

                this.ReportParams["ИнспекторАктаВизОсмТП"] =
                    inspectorData.FioAblative.IsEmpty()
                        ? documentGjiInspector.Fio
                        : CutFio(inspectorData.FioAblative);

                this.ReportParams["ДолжностьИнспектораАктаВизОсм"] = inspectorData.Position;
            }

            if (!realObjAddress.IsEmpty() && flatNum.IsEmpty())
            {
                this.ReportParams["РасположениеСклонение"] = "расположенного ";
            }
            else
            {
                this.ReportParams["РасположениеСклонение"] = "расположенной ";
            }

            FillCeoInfo(actVisual);
        }

        

        private void FillCeoInfo(ActVisual actvisual)
        {
            if (actvisual.RealityObject == null)
            {
                return;
            }

            var roCeo = RobjectSeDomain.GetAll()
                .Where(x => x.RealityObject.Id == actvisual.RealityObject.Id)
                .Select(x => x.StructuralElement.Group.CommonEstateObject.Name)
                .Where(x => x != null)
                .AsEnumerable()
                //приводим к нормированному виду, дабы лишние пробелы и регистр не портили картину
                .Select(x => x.ToUpper().Replace(" ", ""))
                .Distinct();

            var addStr = new List<string>();

            foreach (var ceo in roCeo)
            {
                switch (ceo)
                {
                    case "ХВС":
                        addStr.Add("холодного водоснабжения");
                        break;
                    case "ГВС":
                        addStr.Add("горячего водоснабжения");
                        break;
                    case "СИСТЕМАВОДООТВЕДЕНИЯМКД":
                        addStr.Add("водоотведения");
                        break;
                    case "ЭЛЕКТРОСНАБЖЕНИЕ":
                        addStr.Add("электроснабжения");
                        break;
                    case "ОТОПЛЕНИЕ":
                        addStr.Add("теплоснабжения");
                        break;
                }
            }

            this.ReportParams["ЦентрализованныеСистемы"] = addStr.AggregateWithSeparator(", ");
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "ActVisual_Stimul",
                    Name = "ActVisual",
                    Description = "Акт визуального осмотра",
                    Template = Properties.Resources.ActVisual
                }
            };
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        public override string Name
        {
            get { return "Акт виз-го осмотра"; }
        }

        public override string Description
        {
            get { return "Акт виз-го осмотра"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string Id
        {
            get { return "ActVisualStimul"; }
        }

        public override string CodeForm
        {
            get { return "ActVisual"; }
        }
    }
}