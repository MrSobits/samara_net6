namespace Bars.GkhGji.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.BoilerRooms;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class HeatInputInformationReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private readonly IDomainService<Municipality> _municipalityDomain;
        private readonly IDomainService<HeatInputInformation> _heatInputInfoDomain;
        private readonly IDomainService<HeatingPeriod> _boilerHeatPeriodDomain;
        private readonly IDomainService<UnactivePeriod> _boilerUnactPeriodDomain;

        private DateTime reportDate = DateTime.Now;
        private long[] municipalityIds;

        public HeatInputInformationReport(
            IDomainService<Municipality> municipalityDomain,
            IDomainService<HeatInputInformation> heatInputInfoDomain,
            IDomainService<HeatingPeriod> boilerHeatPeriodDomain,
            IDomainService<UnactivePeriod> boilerUnactPeriodDomain)
            : base(new ReportTemplateBinary(Properties.Resources.HeatInputInformationReport))
        {
            _municipalityDomain = municipalityDomain;
            _heatInputInfoDomain = heatInputInfoDomain;
            _boilerHeatPeriodDomain = boilerHeatPeriodDomain;
            _boilerUnactPeriodDomain = boilerUnactPeriodDomain;
        }

        public override string Name
        {
            get
            {
                return "Информация о подаче тепла на объекты ЖКХ и социальной сферы";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отопительный сезон";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Информация о подаче тепла на объекты ЖКХ и социальной сферы";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.HeatInputInformation";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.HeatInputInformation";
            }
        }

        public override string ReportGenerator { get; set; }

        public override void SetUserParams(BaseParams baseParams)
        {
            reportDate = baseParams.Params["reportDate"].ToDateTime();
            municipalityIds = baseParams.Params.GetAs("municipalityIds", string.Empty).ToLongArray();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["Дата"] = string.Format("{0:d MMMM yyyy}", reportDate);

            var municipalities = _municipalityDomain
                    .GetAll()
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Id))
                    .Select(x => new { x.Id, x.Name })
                    .OrderBy(x => x.Name)
                    .ToArray();

            var info = _heatInputInfoDomain.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.HeatInputPeriod.Municipality.Id))
                .Where(x => x.HeatInputPeriod.Month == reportDate.Month)
                .ToList();

            var num = 0;
            var boilerCountSum = 0;
            var boilerStartSum = 0;
            var houseCountSum = 0;
            var houseCentrSum = 0;
            var houseIndividSum = 0;
            var schoolCountSum = 0;
            var schoolCentrSum = 0;
            var schoolIndividSum = 0;
            var preSchoolCountSum = 0;
            var preSchoolCentrSum = 0;
            var preSchoolIndividSum = 0;
            var healthCountSum = 0;
            var healthCentrSum = 0;
            var healthIndividSum = 0;
            var cultureCountSum = 0;
            var cultureCentrSum = 0;
            var cultureIndividSum = 0;
            var sportCountSum = 0;
            var sportCentrSum = 0;
            var sportIndividSum = 0;
            var socialCountSum = 0;
            var socialCentrSum = 0;
            var socialIndividSum = 0;

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");

            foreach (var row in municipalities)
            {
                section.ДобавитьСтроку();
                section["НомерПП"] = ++num;
                section["МуниципальноеОбразование"] = row.Name;

                // Котельные
                var heatPeriodDomain = Container.Resolve<IDomainService<HeatingPeriod>>()
                    .GetAll()
                    .Where(x => x.BoilerRoom.Municipality.Id == row.Id)
                    .ToList();

                var unactivePeriodDomain = Container.Resolve<IDomainService<UnactivePeriod>>()
                    .GetAll()
                    .Where(x => x.BoilerRoom.Municipality.Id == row.Id)
                    .ToList();

                if (heatPeriodDomain.Any() && unactivePeriodDomain.Any())
                {
                    var boilCount =
                    unactivePeriodDomain.Count(
                        x =>
                        reportDate <= x.Start.Value.AddDays(-x.Start.Value.Day)
                        || reportDate > x.End.Value.AddDays(-x.End.Value.Day).AddMonths(1));
                    var boilStarted =
                        heatPeriodDomain.Count(
                            x =>
                            reportDate > x.Start.AddDays(-x.Start.Day)
                            || reportDate <= x.End.Value.AddDays(-x.End.Value.Day + 1).AddMonths(-1));

                    section["КотелВсего"] = boilCount;
                    section["КотелЗапущ"] = boilStarted;
                    section["КотелПроцент"] = boilCount != 0
                        ? decimal.Round((boilStarted / boilCount) * 100, 5)
                        : 0;
                    section["КотелНеЗапущ"] = boilCount - boilStarted;

                    boilerCountSum += boilCount;
                    boilerStartSum += boilStarted;
                }
                else
                {
                    section["КотелВсего"] = "-";
                    section["КотелЗапущ"] = "-";
                    section["КотелПроцент"] = "-";
                    section["КотелНеЗапущ"] = "-";
                }

                // Жилые дома
                var realityObject =
                    info.FirstOrDefault(
                        x =>
                        x.HeatInputPeriod.Municipality.Id == row.Id
                        && x.TypeHeatInputObject == TypeHeatInputObject.RealityObject);
                if (realityObject != null)
                {
                    section["ДомВсего"] = realityObject.Count;
                    section["ДомЦентрОтоп"] = realityObject.CentralHeating;
                    section["ДомИндивидОтоп"] = realityObject.IndividualHeating;
                    section["ДомПроцент"] = realityObject.Percent;
                    section["ДомБезТепла"] = realityObject.NoHeating;

                    houseCountSum += realityObject.Count;
                    houseCentrSum += realityObject.CentralHeating;
                    houseIndividSum += realityObject.IndividualHeating;
                }
                else
                {
                    section["ДомВсего"] = "-";
                    section["ДомЦентрОтоп"] = "-";
                    section["ДомИндивидОтоп"] = "-";
                    section["ДомПроцент"] = "-";
                    section["ДомБезТепла"] = "-";
                }

                // Школы
                var school =
                    info.FirstOrDefault(
                        x =>
                        x.HeatInputPeriod.Municipality.Id == row.Id
                        && x.TypeHeatInputObject == TypeHeatInputObject.School);
                if (school != null)
                {
                    section["ШколаВсего"] = school.Count;
                    section["ШколаЦентрОтоп"] = school.CentralHeating;
                    section["ШколаИндивидОтоп"] = school.IndividualHeating;
                    section["ШколаПроцент"] = school.Percent;
                    section["ШколаБезТепла"] = school.NoHeating;

                    schoolCountSum += school.Count;
                    schoolCentrSum += school.CentralHeating;
                    schoolIndividSum += school.IndividualHeating;
                }
                else
                {
                    section["ШколаВсего"] = "-";
                    section["ШколаЦентрОтоп"] = "-";
                    section["ШколаИндивидОтоп"] = "-";
                    section["ШколаПроцент"] = "-";
                    section["ШколаБезТепла"] = "-";
                }

                // Садики
                var preschool =
                    info.FirstOrDefault(
                        x =>
                        x.HeatInputPeriod.Municipality.Id == row.Id
                        && x.TypeHeatInputObject == TypeHeatInputObject.PreSchool);
                if (preschool != null)
                {
                    section["СадВсего"] = preschool.Count;
                    section["СадЦентрОтоп"] = preschool.CentralHeating;
                    section["СадИндивидОтоп"] = preschool.IndividualHeating;
                    section["СадПроцент"] = preschool.Percent;
                    section["СадБезТепла"] = preschool.NoHeating;

                    preSchoolCountSum += preschool.Count;
                    preSchoolCentrSum += preschool.CentralHeating;
                    preSchoolIndividSum += preschool.IndividualHeating;
                }
                else
                {
                    section["СадВсего"] = "-";
                    section["СадЦентрОтоп"] = "-";
                    section["СадИндивидОтоп"] = "-";
                    section["СадПроцент"] = "-";
                    section["СадБезТепла"] = "-";
                }

                // Школ + садиков
                section["ШколаИСадик"] = section["ШколаВсего"].ToInt() + section["СадВсего"].ToInt();

                // Здравоохранение
                var health =
                    info.FirstOrDefault(
                        x =>
                        x.HeatInputPeriod.Municipality.Id == row.Id
                        && x.TypeHeatInputObject == TypeHeatInputObject.HealthService);
                if (health != null)
                {
                    section["ЗдорВсего"] = health.Count;
                    section["ЗдорЦентрОтоп"] = health.CentralHeating;
                    section["ЗдорИндивидОтоп"] = health.IndividualHeating;
                    section["ЗдорПроцент"] = health.Percent;
                    section["ЗдорБезТепла"] = health.NoHeating;

                    healthCountSum += health.Count;
                    healthCentrSum += health.CentralHeating;
                    healthIndividSum += health.IndividualHeating;
                }
                else
                {
                    section["ЗдорВсего"] = "-";
                    section["ЗдорЦентрОтоп"] = "-";
                    section["ЗдорИндивидОтоп"] = "-";
                    section["ЗдорПроцент"] = "-";
                    section["ЗдорБезТепла"] = "-";
                }

                // Культура
                var culture =
                    info.FirstOrDefault(
                        x =>
                        x.HeatInputPeriod.Municipality.Id == row.Id
                        && x.TypeHeatInputObject == TypeHeatInputObject.Culture);
                if (culture != null)
                {
                    section["КультВсего"] = culture.Count;
                    section["КультЦентрОтоп"] = culture.CentralHeating;
                    section["КультИндивидОтоп"] = culture.IndividualHeating;
                    section["КультПроцент"] = culture.Percent;
                    section["КультБезТепла"] = culture.NoHeating;

                    cultureCountSum += culture.Count;
                    cultureCentrSum += culture.CentralHeating;
                    cultureIndividSum += culture.IndividualHeating;
                }
                else
                {
                    section["КультВсего"] = "-";
                    section["КультЦентрОтоп"] = "-";
                    section["КультИндивидОтоп"] = "-";
                    section["КультПроцент"] = "-";
                    section["КультБезТепла"] = "-";
                }

                // Спорт
                var sport =
                    info.FirstOrDefault(
                        x =>
                        x.HeatInputPeriod.Municipality.Id == row.Id
                        && x.TypeHeatInputObject == TypeHeatInputObject.Sport);
                if (sport != null)
                {
                    section["СпортВсего"] = sport.Count;
                    section["СпортЦентрОтоп"] = sport.CentralHeating;
                    section["СпортИндивидОтоп"] = sport.IndividualHeating;
                    section["СпортПроцент"] = sport.Percent;
                    section["СпортБезТепла"] = sport.NoHeating;

                    sportCountSum += sport.Count;
                    sportCentrSum += sport.CentralHeating;
                    sportIndividSum += sport.IndividualHeating;
                }
                else
                {
                    section["СпортВсего"] = "-";
                    section["СпортЦентрОтоп"] = "-";
                    section["СпортИндивидОтоп"] = "-";
                    section["СпортПроцент"] = "-";
                    section["СпортБезТепла"] = "-";
                }

                // Соцзащита
                var social =
                    info.FirstOrDefault(
                        x =>
                        x.HeatInputPeriod.Municipality.Id == row.Id
                        && x.TypeHeatInputObject == TypeHeatInputObject.SocialSecurity);
                if (social != null)
                {
                    section["СоцВсего"] = social.Count;
                    section["СоцЦентрОтоп"] = social.CentralHeating;
                    section["СоцИндивидОтоп"] = social.IndividualHeating;
                    section["СоцПроцент"] = social.Percent;
                    section["СоцБезТепла"] = social.NoHeating;

                    socialCountSum += social.Count;
                    socialCentrSum += social.CentralHeating;
                    socialIndividSum += social.IndividualHeating;
                }
                else
                {
                    section["СоцВсего"] = "-";
                    section["СоцЦентрОтоп"] = "-";
                    section["СоцИндивидОтоп"] = "-";
                    section["СоцПроцент"] = "-";
                    section["СоцБезТепла"] = "-";
                }
            }

            // суммы по каждому значению
            reportParams.SimpleReportParams["КотелВсегоИтог"] = boilerCountSum;
            reportParams.SimpleReportParams["КотелЗапущИтог"] = boilerStartSum;
            reportParams.SimpleReportParams["КотелПроцентИтог"] = 
                boilerCountSum != 0
                ? decimal.Round((boilerStartSum / boilerCountSum) * 100, 5)
                : 0;
            reportParams.SimpleReportParams["КотелНеЗапущИтог"] = boilerCountSum - boilerStartSum;


            reportParams.SimpleReportParams["ДомВсегоИтог"] = houseCountSum;
            reportParams.SimpleReportParams["ДомЦентрИтог"] = houseCentrSum;
            reportParams.SimpleReportParams["ДомИндивидИтог"] = houseIndividSum;
            reportParams.SimpleReportParams["ДомПроцентИтог"] =
                houseCountSum != 0
                ? decimal.Round(((houseCentrSum + houseIndividSum) / houseCountSum) * 100, 5)
                : 0;
            reportParams.SimpleReportParams["ДомБезТеплаИтог"] = houseCountSum - houseCentrSum - houseIndividSum;


            reportParams.SimpleReportParams["ШколаИСадИтого"] = schoolCountSum + preSchoolCountSum;


            reportParams.SimpleReportParams["ШколаВсегоИтог"] = schoolCountSum;
            reportParams.SimpleReportParams["ШколаЦентрИтог"] = schoolCentrSum;
            reportParams.SimpleReportParams["ШколаИндивидИтог"] = schoolIndividSum;
            reportParams.SimpleReportParams["ШколаПроцентИтог"] =
                schoolCountSum != 0
                ? decimal.Round(((schoolCentrSum + schoolIndividSum) / schoolCountSum) * 100, 5)
                : 0;
            reportParams.SimpleReportParams["ШколаБезТеплаИтог"] = schoolCountSum - schoolCentrSum - schoolIndividSum;


            reportParams.SimpleReportParams["СадВсегоИтог"] = preSchoolCountSum;
            reportParams.SimpleReportParams["СадЦентрИтог"] = preSchoolCentrSum;
            reportParams.SimpleReportParams["СадИндивидИтог"] = preSchoolIndividSum;
            reportParams.SimpleReportParams["СадПроцентИтог"] =
                preSchoolCountSum != 0
                ? decimal.Round(((preSchoolCentrSum + preSchoolIndividSum) / preSchoolCountSum) * 100, 5)
                : 0;
            reportParams.SimpleReportParams["СадБезТеплаИтог"] = preSchoolCountSum - preSchoolCentrSum - preSchoolIndividSum;


            reportParams.SimpleReportParams["ЗдорВсегоИтог"] = healthCountSum;
            reportParams.SimpleReportParams["ЗдорЦентрИтог"] = healthCentrSum;
            reportParams.SimpleReportParams["ЗдорИндивидИтог"] = healthIndividSum;
            reportParams.SimpleReportParams["ЗдорПроцентИтог"] =
                healthCountSum != 0
                ? decimal.Round(((healthCentrSum + healthIndividSum) / healthCountSum) * 100, 5)
                : 0;
            reportParams.SimpleReportParams["ЗдорБезТеплаИтог"] = healthCountSum - healthCentrSum - healthIndividSum;


            reportParams.SimpleReportParams["КультВсегоИтог"] = cultureCountSum;
            reportParams.SimpleReportParams["КультЦентрИтог"] = cultureCentrSum;
            reportParams.SimpleReportParams["КультИндивидИтог"] = cultureIndividSum;
            reportParams.SimpleReportParams["КультПроцентИтог"] =
                cultureCountSum != 0
                ? decimal.Round(((cultureCentrSum + cultureIndividSum) / cultureCountSum) * 100, 5)
                : 0;
            reportParams.SimpleReportParams["КультБезТеплаИтог"] = cultureCountSum - cultureCentrSum - cultureIndividSum;


            reportParams.SimpleReportParams["СпортВсегоИтог"] = sportCountSum;
            reportParams.SimpleReportParams["СпортЦентрИтог"] = sportCentrSum;
            reportParams.SimpleReportParams["СпортИндивидИтог"] = sportIndividSum;
            reportParams.SimpleReportParams["СпортПроцентИтог"] =
                sportCountSum != 0
                ? decimal.Round(((sportCentrSum + sportIndividSum) / sportCountSum) * 100, 5)
                : 0;
            reportParams.SimpleReportParams["СпортБезТеплаИтог"] = sportCountSum - sportCentrSum - sportIndividSum;


            reportParams.SimpleReportParams["СоцВсегоИтог"] = socialCountSum;
            reportParams.SimpleReportParams["СоцЦентрИтог"] = socialCentrSum;
            reportParams.SimpleReportParams["СоцИндивидИтог"] = socialIndividSum;
            reportParams.SimpleReportParams["СоцПроцентИтог"] =
                socialCountSum != 0
                ? decimal.Round(((socialCentrSum + socialIndividSum) / socialCountSum) * 100, 5)
                : 0;
            reportParams.SimpleReportParams["СоцБезТеплаИтог"] = socialCountSum - socialCentrSum - socialIndividSum;
        }
    }
}