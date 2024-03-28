using System.Linq;
using Bars.B4;
using Bars.B4.DataAccess;

using Bars.B4.Modules.Reports;
using Bars.B4.Utils;
using Bars.Gkh.Domain;
using Bars.GkhGji.Entities;
using Castle.Windsor;

namespace Bars.GkhGji.Regions.Nso.Report
{
    class NsoBusinessActivityReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long[] municipalityIds;

        public NsoBusinessActivityReport()
            : base(new ReportTemplateBinary(Properties.Resources.NsoBusinessActivityReport))
        {
        }

        public override string Name
        {
            get
            {
                return "Реестр уведомлений о начале осуществления предпринимательской деятельности (НСО)"; 
            }
        }

        public override string Desciption
        {
            get
            {
                return "Реестр уведомлений о начале осуществления предпринимательской деятельности (НСО)"; 
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отчет ГЖИ";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.NsoBusinessActivityReport";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.NsoBusinessActivityReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            municipalityIds = baseParams.Params.GetAs("municipalityIds", string.Empty).ToLongArray();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var businessActivityDomain = Container.ResolveDomain<BusinessActivity>();
           
            var data = businessActivityDomain .GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Contragent.Municipality.Id))
                .Select(x => new
                {
                    x.RegNum,
                    x.DateRegistration,
                    Contragent = x.Contragent.Name,
                    x.Contragent.ShortName,
                    OrganizationForm = x.Contragent.OrganizationForm.Name,
                    x.Contragent.Ogrn,
                    x.Contragent.Inn,
                    x.Contragent.JuridicalAddress,
                    x.Contragent.FactAddress,
                    Municipality = x.Contragent.Municipality.Name,
                    x.TypeKindActivity,
                    x.DateBegin
                })
                .OrderBy(x => x.Municipality)
                .ThenBy(x => x.Contragent)
                .ToList();
            
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var rec in data)
            {
                section.ДобавитьСтроку();
                section["RegNum"] = rec.RegNum;
                section["RegDate"] = rec.DateRegistration.HasValue ? rec.DateRegistration.Value.ToShortDateString() : string.Empty;
                section["ContragentName"] = rec.Contragent;
                section["ContragentShortName"] = rec.ShortName;
                section["OrgForm"] = rec.OrganizationForm;
                section["Ogrn"] = rec.Ogrn;
                section["Inn"] = rec.Inn;
                section["JuridicalAddress"] = rec.JuridicalAddress;
                section["Municipality"] = rec.Municipality;
                section["FactAddress"] = rec.FactAddress;
                section["KindActivity"] = rec.TypeKindActivity.GetEnumMeta().Display;
                section["DateActivity"] = rec.DateBegin.HasValue ? rec.DateBegin.Value.ToShortDateString() : string.Empty;
            }
        }
    }
}
