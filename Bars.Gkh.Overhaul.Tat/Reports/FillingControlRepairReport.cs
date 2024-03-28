namespace Bars.Gkh.Overhaul.Tat.Reports
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Tat.Properties;

    using Castle.Windsor;

    public class FillingControlRepairReport : BasePrintForm
    {
        public FillingControlRepairReport()
            : base(new ReportTemplateBinary(Resources.FillingControlRepairReport))
        {
        }

        private long[] municipalityIds;
        public IWindsorContainer Container { get; set; }
        public override string Name
        {
            get { return "Контроль заполнения показателей для определения  очередности проведения ремонта"; }
        }

        public override string Desciption
        {
            get { return "Контроль заполнения показателей для определения  очередности проведения ремонта"; }
        }

        public override string GroupName
        {
            get { return "ДПКР"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.FillingControlRepairReport";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Ovrhl.FillingControlRepairReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var data = Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                .Where(x => x.TypeHouse == TypeHouse.ManyApartments)
                .Where(x => x.ConditionHouse == ConditionHouse.Serviceable || x.ConditionHouse == ConditionHouse.Dilapidated)
                .Select(x => new
                {
                    MuName = x.Municipality.Name,
                    x.Address,
                    x.BuildYear,
                    x.DateCommissioning,
                    x.PrivatizationDateFirstApartment,
                    x.HasPrivatizedFlats,
                    x.ProjectDocs,
                    x.EnergyPassport,
                    x.ConfirmWorkDocs
                })
                .AsEnumerable()
                .GroupBy(x => x.MuName)
                .ToDictionary(x => x.Key, x => x.ToList());

            var num = 1;

            var sectionMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMunicipality");

            var section = sectionMunicipality.ДобавитьСекцию("section");

            foreach (var municipality in data.OrderBy(x => x.Key))
            {
                sectionMunicipality.ДобавитьСтроку();
                
                foreach (var realObj in municipality.Value.OrderBy(x => x.Address))
                {
                    section.ДобавитьСтроку();

                    section["Num"] = num++;
                    section["Mun"] = realObj.MuName;
                    section["Address"] = realObj.Address;
                    section["BuildYear"] = realObj.BuildYear;
                    section["DeliverDate"] = realObj.DateCommissioning;
                    section["FirstPrivatizDate"] = realObj.PrivatizationDateFirstApartment.HasValue
                            ? realObj.PrivatizationDateFirstApartment.Value.ToShortDateString()
                            : string.Empty;
                    section["HasPrivatizDate"] = realObj.HasPrivatizedFlats.HasValue ? (realObj.HasPrivatizedFlats.Value ? "Да" : "Нет") : "Не задано";
                    section["AvailDoc"] = realObj.ProjectDocs.GetEnumMeta().Display;
                    section["AvalPassport"] = realObj.EnergyPassport.GetEnumMeta().Display;
                    section["AvailCadastralDoc"] = realObj.ConfirmWorkDocs.GetEnumMeta().Display;
                }

                sectionMunicipality["MunName"] = municipality.Key;
                sectionMunicipality["BuildYearMoTotal"] = municipality.Value.Count(x => x.BuildYear == null);
                sectionMunicipality["DeliverDateMoTotal"] = municipality.Value.Count(x => x.DateCommissioning == null);
                sectionMunicipality["FirstPrivatizDateMoTotal"] = municipality.Value.Count(x => !x.PrivatizationDateFirstApartment.HasValue);
                sectionMunicipality["HasPrivatizDateMoTotal"] = municipality.Value.Count(x => !x.HasPrivatizedFlats.HasValue || !x.HasPrivatizedFlats.Value);
                sectionMunicipality["AvailDocMoTotal"] = municipality.Value.Count(x => x.ProjectDocs == TypePresence.NotSet);
                sectionMunicipality["AvalPassportMoTotal"] = municipality.Value.Count(x => x.ProjectDocs == TypePresence.NotSet);
                sectionMunicipality["AvailCadastralDocMoTotal"] = municipality.Value.Count(x => x.ProjectDocs == TypePresence.NotSet);
            }

            reportParams.SimpleReportParams["BuildYearTotal"] = data.Values.Sum(y => y.Count(x => x.BuildYear == null));
            reportParams.SimpleReportParams["DeliverDateTotal"] = data.Values.Sum(y => y.Count(x => x.DateCommissioning == null));
            reportParams.SimpleReportParams["FirstPrivatizDateTotal"] = data.Values.Sum(y => y.Count(x => !x.PrivatizationDateFirstApartment.HasValue));
            reportParams.SimpleReportParams["HasPrivatizDateTotal"] = data.Values.Sum(y => y.Count(x => !x.HasPrivatizedFlats.HasValue || !x.HasPrivatizedFlats.Value));
            reportParams.SimpleReportParams["AvailDocTotal"] = data.Values.Sum(y => y.Count(x => x.ProjectDocs == TypePresence.NotSet));
            reportParams.SimpleReportParams["AvalPassportTotal"] = data.Values.Sum(y => y.Count(x => x.ProjectDocs == TypePresence.NotSet));
            reportParams.SimpleReportParams["AvailCadastralDocTotal"] = data.Values.Sum(y => y.Count(x => x.ProjectDocs == TypePresence.NotSet));
        }
    }
}
