namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report.DisposalGji
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;
    using Bars.GkhGji.Report;

    public class DisposalGjiMotivatedRequestReport : GjiBaseStimulReport
    {
        private long DocumentId { get; set; }

        public DisposalGjiMotivatedRequestReport()
            : base(new ReportTemplateBinary(Resources.BlockGji_MotivatedRequest))
        {
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        public override string Id
        {
            get { return "MotivatedRequest"; }
        }

        public override string CodeForm
        {
            get { return "Disposal"; }
        }

        public override string Name
        {
            get { return "Мотивированный запрос"; }
        }

        public override string Description
        {
            get { return "Мотивированный запрос"; }
        }

        protected override string CodeTemplate { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "ChelyabinskMotivatedRequest",
                            Name = "MotivatedRequest",
                            Description = "Мотивированный запрос",
                            Template = Resources.BlockGji_MotivatedRequest
                        }
                };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var disposalDomain = this.Container.Resolve<IDomainService<ChelyabinskDisposal>>();
            var zonalInspectionInspectorDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var contrAgentContactDomain = this.Container.Resolve<IDomainService<ContragentContact>>();

            try
            {
                var disposal = disposalDomain.Get(this.DocumentId);
                this.FillCommonFields(disposal);
                string zonalInspectionName = string.Empty;
                if (disposal.ResponsibleExecution != null)
                {
                    zonalInspectionName =
                    zonalInspectionInspectorDomain.GetAll()
                        .Where(x => x.Inspector.Id == disposal.ResponsibleExecution.Id)
                        .Select(x => x.ZonalInspection.Name)
                        .FirstOrDefault();
                }
                var contrAgent = disposal.Inspection.Contragent;
                var contrAgentContact = contrAgentContactDomain.GetAll()
                    .FirstOrDefault(x => x.Contragent.Id == contrAgent.Id);

                this.ReportParams["Id"] = this.DocumentId.ToString();
                this.ReportParams["НаименованиеОтдела"] = zonalInspectionName;
                this.ReportParams["НомерЗапроса"] = disposal.MotivatedRequestNumber;
                this.ReportParams["ДатаЗапроса"] = disposal.MotivatedRequestDate.HasValue
                    ? disposal.MotivatedRequestDate.Value.ToShortDateString()
                    : string.Empty;
                this.ReportParams["ДолжностьРуководителяДатПадеж"] = contrAgentContact != null
                    ? contrAgentContact.Position.NameDative
                    : string.Empty;
                this.ReportParams["ФиоСокрДатПадеж"] = contrAgentContact != null
                    ? string.Format("{0} {1}.{2}.", contrAgentContact.SurnameDative, contrAgentContact.Name[0], contrAgentContact.Patronymic[0])
                    : string.Empty;
                this.ReportParams["АдресОрганизации"] = contrAgent.JuridicalAddress;
                this.ReportParams["ИНН"] = contrAgent.Inn;
                this.ReportParams["КраткоеНаименованиеОрганизации"] = contrAgent.ShortName;
                this.ReportParams["ДолжностьОтветственного"] = disposal.ResponsibleExecution != null
                    ? disposal.ResponsibleExecution.Position
                    : string.Empty;
                this.ReportParams["ТелефонОтветственного"] = disposal.ResponsibleExecution != null
                    ? disposal.ResponsibleExecution.Phone
                    : string.Empty;
                this.ReportParams["ФиоИнспектораСокр"] = disposal.ResponsibleExecution != null
                    ? disposal.ResponsibleExecution.ShortFio
                    : string.Empty;
            }
            finally
            {
                this.Container.Release(disposalDomain);
                this.Container.Release(zonalInspectionInspectorDomain);
                this.Container.Release(contrAgentContactDomain);
            }
        }
    }
}