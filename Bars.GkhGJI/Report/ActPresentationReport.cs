namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ActPresentationReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private List<long> municipalityListId = new List<long>(); 

        public ActPresentationReport() : base(new ReportTemplateBinary(Properties.Resources.ActResponsePresentation))
        {

        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.ActPresentation";
            }
        }

        public override string Desciption
        {
            get { return "Реестр представлений"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ActPresentation"; }
        }

        public override string Name
        {
            get { return "Реестр представлений"; }
        }

        private void ParseIds(List<long> list, string Ids)
        {
            list.Clear();

            if (!string.IsNullOrEmpty(Ids))
            {
                var ids = Ids.Split(',');
                foreach (var id in ids)
                {
                    long Id;
                    if (long.TryParse(id, out Id))
                    {
                        if (!list.Contains(Id))
                        {
                            list.Add(Id);
                        }
                    }
                }
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            this.ParseIds(municipalityListId, baseParams.Params["municipalityIds"].ToString());
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var data = Container.Resolve<IDomainService<Presentation>>().GetAll()
                .WhereIf(municipalityListId.Count > 0, x => municipalityListId.Contains(x.Contragent.Municipality.Id))
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .Select(x => new { x.DocumentNumber, x.DocumentDate, x.TypeInitiativeOrg })
                .ToList();

            var i = 0;

            foreach (var rec in data)
            {
                section.ДобавитьСтроку();
                section["Number1"] = ++i;
                section["DocNumber"] = rec.DocumentNumber;
                section["DocDate"] = rec.DocumentDate.HasValue ? rec.DocumentDate.Value.ToShortDateString() : string.Empty;

                var enumValue = rec.TypeInitiativeOrg;

                var memInfo = enumValue.GetType().GetMember(enumValue.ToString());
                if (memInfo.Length > 0)
                {
                    var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);

                    if (attributes.Length > 0)
                    {
                        section["Issued"] = ((DisplayAttribute)attributes[0]).Value;
                    }
                }
            }
        }
    }
}