namespace Bars.Gkh.Report
{
    using Bars.B4;
    using System.Linq;
    using Bars.B4.Utils;
    using B4.Modules.Reports;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Properties;
    

    class ContragentReport : BasePrintForm
    {
        private long[] municipalityIds;

        public IDomainService<Contragent> ContragentService { get; set; }

        public ContragentReport()
            : base(new ReportTemplateBinary(Resources.ContragentsReport))
        {

        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
        }

        public override void PrepareReport(ReportParams reportParams)
        {

            var allContragents = ContragentService.GetAll()
                .WhereIf(municipalityIds.Any(),
                    x => x.Municipality != null && municipalityIds.Contains(x.Municipality.Id));

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRow");

            foreach (var agent in allContragents)
            {
                section.ДобавитьСтроку();
                section["guidУлицы"] = agent.FiasFactAddress != null ? agent.FiasFactAddress.PlaceGuidId : "";
                section["НомерДома"] = agent.FiasFactAddress != null ? agent.FiasFactAddress.House : "";
                section["НомерКорпуса"] = agent.FiasFactAddress != null ? agent.FiasFactAddress.Housing : "";
                section["ИНН"] = agent.Inn;
                section["КПП"] = agent.Kpp;
                section["ОГРН"] = agent.Ogrn;
                section["Телефон"] = agent.Phone;
                section["ПризнакФилиала"] = agent.OrganizationForm != null ? 
                                               agent.OrganizationForm.Code == "90" ? "Да" : "" 
                                               : "";
                section["ДатаНачалаДоговора"] = agent.DateRegistration.HasValue ?agent.DateRegistration.Value.ToShortDateString() : "" ;
                section["ДатаОкончанияДоговора"] = agent.DateTermination.HasValue ? agent.DateTermination.Value.ToShortDateString() : "";
            }
        }

        public override string Name
        {
            get { return "Отчет по контрагентам"; }
        }

        public override string Desciption
        {
            get { return "Отчет по контрагентам"; }
        }

        public override string GroupName
        {
            get { return "Технический паспорт дома"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ContragentReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GKH.ContragentReport";}
        }
    }
}
