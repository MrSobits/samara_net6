using System.Collections.Generic;
using System.Linq;

namespace Bars.Gkh.Regions.Nso.Report
{
    using Bars.B4;
    
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;

    using Bars.Gkh.Regions.Nso.Properties;

    using Castle.Windsor;

    public class LocalGovernmentReport: BasePrintForm
    {
        
        public IWindsorContainer Container { get; set; }

        private List<long> municipalities;

        public LocalGovernmentReport()
            : base(new ReportTemplateBinary(Resources.LocalGovernmentReport))
        {
        }

        public override string Name
        {
            get
            {
                return "Реестр органов МЖК";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Реестр органов МЖК";
            }
        }

        public override string GroupName
        {
            get
            {
                return "ГЖИ";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.LocalGovernment";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.LocalGovernmentReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            
            var municipalitiesParam = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalities = !string.IsNullOrEmpty(municipalitiesParam)
                                 ? municipalitiesParam.Split(',').Select(x => x.ToLong()).ToList()
                                 : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var service = Container.Resolve<IDomainService<NsoLocalGovernment>>();

            try
            {
                var data =
                    service.GetAll()
                           .WhereIf(
                               municipalities.Count > 0, x => municipalities.Contains(x.Contragent.Municipality.Id))
                           .Select(
                               x =>
                               new
                                   {
                                       Municipality = x.Contragent.Municipality.Name,
                                       Address = x.Contragent.JuridicalAddress,
                                       x.Fio,
                                       x.Email,
                                       x.Phone,
                                       x.RegDateNotice,
                                       x.RegNumNotice,
                                       x.NumNpa,
                                       x.DateNpa,
                                       x.NameNpa,
                                       x.InteractionMuNum,
                                       x.InteractionMuDate,
                                       x.InteractionGjiNum,
                                       x.InteractionGjiDate,
                                       x.Description
                                   })
                           .OrderBy(x => x.Municipality)
                           .ThenBy(x => x.Address)
                           .ToList();


                var section = reportParams.ComplexReportParams.ДобавитьСекцию("секция_данные");

                var i = 0;
                foreach (var rec in data)
                {
                    section.ДобавитьСтроку();
                    section["Номер"] = ++i;
                    section["МО"] = rec.Municipality;
                    section["Адрес"] = rec.Address;
                    section["Контакты"] = string.Format("{0}, {1}, {2}", rec.Fio, rec.Email, rec.Phone);
                    section["НомерУведомления"] = rec.RegNumNotice;
                    section["ДатаУведомления"] = rec.RegDateNotice.HasValue ? rec.RegDateNotice.Value.ToShortDateString() : string.Empty;
                    section["НПА"] = string.Format("{0}, {1}", rec.NumNpa, rec.DateNpa.HasValue ? rec.DateNpa.Value.ToShortDateString() : string.Empty); ;
                    section["НаименованиеНПА"] = rec.NameNpa;
                    section["НомерМО"] = rec.InteractionMuNum;
                    section["ДатаМО"] = rec.InteractionMuDate.HasValue? rec.InteractionMuDate.Value.ToShortDateString(): string.Empty;
                    section["НомерГЖИ"] = rec.InteractionGjiNum;
                    section["ДатаГЖИ"] = rec.InteractionGjiDate.HasValue ? rec.InteractionGjiDate.Value.ToShortDateString() : string.Empty;
                    section["Примечание"] = rec.Description;
                }
            }
            finally 
            {
                Container.Release(service);
            }
        }
    }
}
