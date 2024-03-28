namespace Bars.GkhGji.Regions.Tula.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tula.Entities;
    using GkhGji.Report;
    using Slepov.Russian.Morpher;
    using Stimulsoft.Report;

    using Utils = Bars.GkhGji.Utils.Utils;

    /// <summary> 
    /// Уведомление из постановления
    /// </summary>
    public class ResolutionNotificationGjiStimulReport : GjiBaseStimulReport
    {
        #region .ctor
        public ResolutionNotificationGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.ResolutionNotificationGjiStimulReport))
        {
        }
        #endregion

        #region Properties
        public override string Id
        {
            get { return "ResolutionNotification"; }
        }

        public override string Name
        {
            get { return "Уведомление о составлении протокола"; }
        }

        public override string Description
        {
            get { return "Уведомление о составлении протокола (из постановления)"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string CodeForm
        {
            get { return "Resolution"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }
        #endregion

        #region Params
        private long documentId;

        private Resolution resolution;
        #endregion

        #region DomainServices
        public IDomainService<Protocol> ProtocolDomain { get; set; }
        public IDomainService<ContragentContact> ContragentContactDomain { get; set; } 
        public IDomainService<Resolution> ResolutionDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<DocumentGJIPhysPersonInfo> DocumentGjiPhysPersonInfoDomain { get; set; }
        public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }
        public IDomainService<ProtocolDefinition> ProtocolDefinitionDomain { get; set; }
        #endregion

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            documentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            resolution = ResolutionDomain.Load(documentId);
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "ResolutionNotificationGjiStimulReport",
                    Name = "ResolutionNotificationGjiStimulReport",
                    Description = "Уведомление о составлении протокола",
                    Template = Properties.Resources.ResolutionNotificationGjiStimulReport
                }
            };
        }

        private void GetCodeTemplate()
        {
            CodeTemplate = "ResolutionNotificationGjiStimulReport";
        }

        public override Stream GetTemplate()
        {
            GetCodeTemplate();
            return base.GetTemplate();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            if (resolution == null)
            {
                throw new ReportProviderException("Не удалось получить постановление");
            }

            Protocol protocol = null;

            var parentProtocol = Utils.GetParentDocumentByType(DocumentGjiChildrenDomain, resolution, TypeDocumentGji.Protocol);

            if (parentProtocol != null)
            {
                protocol = ProtocolDomain.Get(parentProtocol.Id);
            }

            var physPersonInfo = DocumentGjiPhysPersonInfoDomain.GetAll().FirstOrDefault(x => x.Document.Id == protocol.Id);

            FillCommonFields(resolution);
            Report["НомерПостановления"] = resolution.DocumentNumber;

            Report["ДатаПостановления"] = resolution.DocumentDate.HasValue
                ? resolution.DocumentDate.Value.ToShortDateString()
                : string.Empty;

            if (protocol != null)
            {
                var contragent = protocol.Contragent;

                Report["УправОРГ"] = contragent.Name;

                var protocolDefinition =
                    ProtocolDefinitionDomain.GetAll()
                        .Where(x => x.Protocol.Id == protocol.Id)
                        .Where(x => x.DocumentDate.HasValue)
                        .OrderByDescending(x => x.DocumentDate.Value)
                        .FirstOrDefault();

                Report["ДатаВремяНазнач"] = protocolDefinition != null 
                    ? protocolDefinition.DocumentDate.Value.ToShortDateString() 
                    : string.Empty;

                var articleLaws = ProtocolArticleLawDomain.GetAll()
                    .Where(x => x.Protocol.Id == protocol.Id)
                    .Select(x => x.ArticleLaw.Name)
                    .ToList();

                Report["Статья"] = articleLaws.AggregateWithSeparator(", ");

                if (new List<string> { "0", "9", "11", "8", "15", "18", "4", "17", "1", "10", "12", "13", "16", "19", "5" }
                    .Contains(protocol.Executant.Code))
                {
                    if (contragent != null)
                    {
                        Report["АдресМестаНахождения"] = contragent.JuridicalAddress;
                    }
                }

                else if (new List<string> { "6", "7", "14", "20", "2", "3" }.Contains(protocol.Executant.Code))
                {
                    Report["АдресМестаНахождения"] = physPersonInfo != null
                                                         ? physPersonInfo.PhysPersonAddress
                                                         : string.Empty;
                }

                if (protocol.Executant != null)
                {
                    if (new List<string> { "0", "9", "11", "8", "15", "18", "4", "17" }
                        .Contains(protocol.Executant.Code))
                    {
                        if (contragent != null)
                        {
                            var contragentName = contragent.Name;
                            
                            var headContragent = ContragentContactDomain.GetAll()
                            .Where(x => x.Contragent.Id == protocol.Contragent.Id && x.DateStartWork.HasValue
                                && (x.DateStartWork.Value <= DateTime.Now && (!x.DateEndWork.HasValue || x.DateEndWork.Value >= DateTime.Now)))
                            .FirstOrDefault(x => x.Position != null && (x.Position.Code == "1" || x.Position.Code == "4"));

                            string headFullName = string.Empty,
                                headPosition = string.Empty;
                            if (headContragent != null)
                            {
                                headPosition = headContragent.Position.NameDative;
                                var headPatronymic = !headContragent.Patronymic.IsEmpty() ? headContragent.Patronymic[0] + ". " : string.Empty;
                                var headName = !headContragent.Name.IsEmpty() ? headContragent.Name[0] + ". " : string.Empty;
                                var headSurname = headContragent.SurnameDative;

                                headFullName = headName + headPatronymic + headSurname;

                                Report["ФИОРукОрг"] = headContragent.FullName;
                            }

                            Report["Кого"] = (!headPosition.IsEmpty() ? headPosition + Environment.NewLine : string.Empty)
                                + (!contragentName.IsEmpty() ? contragentName + Environment.NewLine : string.Empty)
                                + (!headFullName.IsEmpty() ? headFullName + Environment.NewLine : string.Empty);
                        }
                    }

                    else if (
                        new List<string> { "6", "7", "14", "20", "2", "3", "1", "10", "12", "13", "16", "19", "5" }
                            .Contains(protocol.Executant.Code))
                    {
                        var physPersonName = склонятель.Проанализировать(protocol.PhysicalPerson);
                        Report["Кого"] = physPersonName.Дательный;
                        Report["ФизЛицоРП"] = physPersonName.Родительный;
                        Report["ФизЛицо"] = protocol.PhysicalPerson;
                    }
                }
            }
        }
    }
}