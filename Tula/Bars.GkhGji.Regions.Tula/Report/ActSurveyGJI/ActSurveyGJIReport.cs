namespace Bars.GkhGji.Regions.Tula.Report
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;

    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tula.Entities;
    using GkhGji.Report;
    using Stimulsoft.Report;

    public class ActSurveyGjiReport : GjiBaseStimulReport
    {
        #region .ctor
        public ActSurveyGjiReport()
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
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<ActSurveyRealityObject> ActSurveyRealityObjectDomain { get; set; }
        public IDomainService<ActSurveyOwner> ActSurveyOwnerDomain { get; set; }
        public IDomainService<ActSurveyAnnex> ActSurveyAnnexDomain { get; set; }
        public IDomainService<ActSurveyPhoto> ActSurveyPhotoDomain { get; set; }
        public IDomainService<Disposal> DisposalDomain { get; set; }
        public IDomainService<TulaActSurvey> TulaActSurveyDomain { get; set; }
        public IDomainService<ActSurveyLongDescription> ActSurveyLongDescriptionDomain { get; set; }
        public IFileManager FileManager { get; set; }

        #endregion

        public override void PrepareReport(ReportParams reportParams)
        {
            var actSurvey = TulaActSurveyDomain.Load(DocumentId);
            if (actSurvey == null)
            {
                throw new ReportProviderException("Не удалось получить акт обследования");
            }
            FillCommonFields(actSurvey);

            Disposal disposal = null;

            var parentDisposal = GkhGji.Utils.Utils.GetParentDocumentByType(DocumentGjiChildrenDomain, actSurvey, TypeDocumentGji.Disposal);

            if (parentDisposal != null)
            {
                disposal = DisposalDomain.GetAll().FirstOrDefault(x => x.Id == parentDisposal.Id);
            }

            Report["НомерАкта"] = actSurvey.DocumentNumber;
            Report["ДатаАкта"] = actSurvey.DocumentDate.HasValue
                    ? actSurvey.DocumentDate.Value.ToString("d MMMM yyyy")
                    : string.Empty;

            var actSurvLong =
                ActSurveyLongDescriptionDomain.GetAll()
                    .FirstOrDefault(x => x.ActSurvey.Id == actSurvey.Id);

            var longDescription = string.Empty;

            if (actSurvLong != null)
            {
                longDescription = actSurvey.Description != null ? Encoding.UTF8.GetString(actSurvLong.Description) : string.Empty;
            }

            Report["Описание"] = longDescription.IsNotEmpty()
                ? longDescription
                : actSurvey.Description;

            Report["ВремяОкончания"] = actSurvey.TimeEnd.HasValue
                ? string.Format("{0} час. {1} мин.", actSurvey.TimeEnd.Value.Hour, actSurvey.TimeEnd.Value.Minute)
                : string.Empty;

            Report["ДатаПроверки"] = actSurvey.DateOf.HasValue
                ? actSurvey.DateOf.Value.ToString("dd MMMM yyyy")
                : string.Empty;

            Report["ВремяНачала"] = actSurvey.TimeStart.HasValue
                ? string.Format("{0} час. {1} мин.", actSurvey.TimeStart.Value.Hour, actSurvey.TimeStart.Value.Minute)
                : string.Empty;

            Report["ДатаПриказа"] = disposal != null 
                ? disposal.DocumentDate.ToDateTime().ToString("d MMMM yyyy")
                : string.Empty;
            Report["НомерПриказа"] = disposal != null ? disposal.DocumentNumber : string.Empty;

            var realObjs = ActSurveyRealityObjectDomain.GetAll().Where(x => x.ActSurvey.Id == actSurvey.Id).Select(x => new { x.RealityObject.FiasAddress, x.RealityObject.Address }).ToList();

            Report["АдресОбъектаПравонарушения"] = realObjs.Select(x => x.Address).AggregateWithSeparator("; ");

            Report["НаселенныйПункт"] = string.Empty;
            if (realObjs.Count > 0)
            {
                var fiasAddress = realObjs.Select(x => x.FiasAddress).FirstOrDefault(x => x != null);
                if (fiasAddress != null)
                {
                    Report["НаселенныйПункт"] = fiasAddress.PlaceName;
                }
            }

            // Инспекторы
            var inspectors =
                DocumentGjiInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == actSurvey.Id)
                    .Select(x => new
                    {
                        x.Inspector.Position,
                        x.Inspector.Fio,
                        x.Inspector.ShortFio
                    })
                    .ToList();

            var inspector = inspectors.FirstOrDefault();

            Report["ДолжностьИнспектор"] = inspectors.AggregateWithSeparator(x => x.Fio + " - " + x.Position, ", ");

            Report["Инспектор"] = inspector != null ? inspector.Fio : string.Empty;
            Report["Должность"] = inspector != null ? inspector.Position : string.Empty;

            // Лица, присутствующие при обследовании
            var actOwners = ActSurveyOwnerDomain.GetAll()
                .Where(x => x.ActSurvey.Id == actSurvey.Id)
                .Select(x => new
                {
                    x.Fio,
                    x.Position,
                    x.WorkPlace,
                    x.DocumentName
                })
                .ToList();

            Report["Присутствовавшие"] = actOwners.Any()
                ? actOwners
                    .Select(x => "{0} - {1}, {2}, {3}"
                    .FormatUsing(x.Fio, x.Position, x.WorkPlace, x.DocumentName))
                    .AggregateWithSeparator("; ") 
                : string.Empty;

            var filenames = ActSurveyAnnexDomain.GetAll()
                .Where(x => x.ActSurvey.Id == actSurvey.Id)
                .AsEnumerable()
                .Select(x => new 
                {
                    x.Name,
                    DocumentDate = x.DocumentDate.HasValue
                        ? x.DocumentDate.Value.ToShortDateString()
                        : string.Empty
                })
                .ToList();

            Report["Приложение"] = filenames.AggregateWithSeparator(x => x.Name + " от " + x.DocumentDate, "; ");
            
            // Фото
            var fileInfos =
                ActSurveyPhotoDomain.GetAll()
                    .Where(x => x.ActSurvey.Id == actSurvey.Id && x.IsPrint)
                    .Select(x => x.File)
                    .ToList();

            Report["Чекбокс"] = fileInfos.Any() ? "1" : "0";

            var i = 0;
            var photos = new List<Photo>();
            foreach (var fileInfo in fileInfos)
            {
                var image = new Photo
                {
                    Num = ++i
                };

                var fileStream = FileManager.GetFile(fileInfo);

                if (fileStream != null)
                {
                    image.Image = Image.FromStream(fileStream);
                }

                photos.Add(image);
            }

            Report.RegData("Photos", photos);
        }

        protected class Photo
        {
            public int Num { get; set; }

            public Image Image { get; set; }
        }
    }
}