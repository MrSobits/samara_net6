namespace Bars.GkhGji.Regions.Archangelsk.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using GkhGji.Report;
    using Stimulsoft.Report;

    public class DisposalGjiMotivatedRequestReport : GjiBaseStimulReport
    {
        private long DocumentId { get; set; }

        public DisposalGjiMotivatedRequestReport()
            : base(new ReportTemplateBinary(Properties.Resources.DisposalGjiMotivatedRequest))
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
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "ArchangelskMotivatedRequest",
                            Name = "MotivatedRequest",
                            Description = "Мотивированный запрос",
                            Template = Properties.Resources.DisposalGjiMotivatedRequest
                        }
                };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var disposalDomain = Container.Resolve<IDomainService<Disposal>>();
            var zonalInspectionInspectorDomain = Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var contrAgentContactDomain = Container.Resolve<IDomainService<ContragentContact>>();

            try
            {
                var disposal = disposalDomain.GetAll().FirstOrDefault(x => x.Id == DocumentId);
                FillCommonFields(disposal);
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

                Report["НаименованиеОтдела"] = zonalInspectionName;
                Report["НомерЗапроса"] = ""; // пока не реализовано
                Report["ДатаЗапроса"] = DateTime.Today.ToShortDateString();
                Report["ДолжностьРуководителяДатПадеж"] = contrAgentContact != null
                    ? contrAgentContact.Position.NameDative
                    : string.Empty;
                Report["ФиоСокрДатПадеж"] = contrAgentContact != null
                    ? string.Format("{0} {1}.{2}.", contrAgentContact.SurnameDative, contrAgentContact.Name[0], contrAgentContact.Patronymic[0])
                    : string.Empty;
                Report["АдресОрганизации"] = contrAgent.JuridicalAddress;
                Report["ИНН"] = contrAgent.Inn;
                Report["КраткоеНаименованиеОрганизации"] = contrAgent.ShortName;
                Report["ДолжностьОтветственного"] = disposal.ResponsibleExecution != null
                    ? disposal.ResponsibleExecution.Position
                    : string.Empty;
                Report["ТелефонОтветственного"] = disposal.ResponsibleExecution != null
                    ? disposal.ResponsibleExecution.Phone
                    : string.Empty;
                Report["ФиоИнспектораСокр"] = disposal.ResponsibleExecution != null
                    ? disposal.ResponsibleExecution.ShortFio
                    : string.Empty;
            }
            finally
            {
                Container.Release(disposalDomain);
                Container.Release(zonalInspectionInspectorDomain);
                Container.Release(contrAgentContactDomain);
            }
        }
    }
}