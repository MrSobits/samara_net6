namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;
    using Gkh.DomainService.Dict.RealEstateType;
    using Gkh.Entities;
    using Gkh.Entities.RealEstateType;
    using Gkh.Utils;

    public class PlanOwnerCollectionReport : BasePrintForm
    {
        #region .ctor

        public PlanOwnerCollectionReport() 
            : base(new ReportTemplateBinary(Properties.Resources.PlanOwnerCollectionReport))
        {
        }

        #endregion .ctor

        #region DiProperties

        public IWindsorContainer Container { get; set; }

        public IRepository<RealityObject> RobjectRepository { get; set; }

        public IDomainService<ShortProgramRecord> ShortRecordDomain { get; set; }

        //ретро, винтаж, олдскул
        public IDomainService<RealEstateTypeRealityObject> RetRoDomain { get; set; }

        public IDomainService<RealEstateTypeRate> RetRateDomain { get; set; }

        public IRepository<Municipality> MuRepository { get; set; }

        #endregion

        #region Properties

        public override string Name
        {
            get { return "Планируемая собираемость по домам"; }
        }

        public override string Desciption
        {
            get { return "Планируемая собираемость по домам"; }
        }

        public override string GroupName
        {
            get { return "Долгосрочная программа"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.PlanOwnerCollection"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhOverhaul.PlanOwnerCollectionReport"; }
        }

        #endregion Properties

        #region Fields

        private long[] _muIds;

        private int _year;

        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            _muIds = baseParams.Params.GetAs<string>("muIds").ToLongArray();
            _year = baseParams.Params.GetAs<int>("year");
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var retTariffParam = config.RateCalcTypeArea;

            var roQuery = RobjectRepository.GetAll()
                .WhereIf(_muIds.Any(), x => _muIds.Contains(x.Municipality.Id) || _muIds.Contains(x.Municipality.ParentMo.Id))
                .Where(y => ShortRecordDomain.GetAll()
                    .Where(x => x.Year == _year)
                    .Any(x => x.RealityObject.Id == y.Id));

            var groupedRoInfo = roQuery
                .OrderBy(x => x.Municipality.Name)
                .ThenBy(x => x.Address)
                .Select(x => new
                {
                    MuId = x.Municipality.Id,
                    RoId = x.Id,
                    x.Address,
                    x.AreaLiving,
                    x.AreaMkd,
                    x.AreaLivingNotLivingMkd
                })
                .AsEnumerable()
                .GroupBy(x => x.MuId);

            var dictMo = MuRepository.GetAll()
                .WhereIf(_muIds.Any(), x => _muIds.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .AsEnumerable()
                .ToDictionary(x => x.Id, y => y.Name);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            //словарь соответствия типа дома и социально допустимого тарифа
            var dictRetTariff = RetRateDomain.GetAll()
                .Where(x => x.Year == _year)
                .Where(x => x.RealEstateType != null)
                .Select(x => new
                {
                    x.RealEstateType.Id,
                    x.SociallyAcceptableRate
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.SociallyAcceptableRate).FirstOrDefault());

            IDictionary<long, List<long>> roRealEstTypesDict;

            var retService = Container.Resolve<IRealEstateTypeService>();

            using (Container.Using(retService))
            {
                roRealEstTypesDict = retService.GetRealEstateTypes(roQuery);
            }

            var chargedMonthTotal = 0m;
            var chargedYearTotal = 0m;
            var charged30YearsTotal = 0m;

            foreach (var municipality in groupedRoInfo)
            {
                section.ДобавитьСтроку();

                decimal muChargedMonth = 0m;
                decimal muChargedYear = 0m;
                decimal muCharged30Years = 0m;

                var sectionMu = section.ДобавитьСекцию("sectionMu");

                foreach (var ro in municipality)
                {
                    sectionMu.ДобавитьСтроку();

                    sectionMu["Mu"] = dictMo.Get(municipality.Key);
                    sectionMu["Address"] = ro.Address;

                    decimal tariff = 0m;

                    if (roRealEstTypesDict.ContainsKey(ro.RoId))
                    {
                        foreach (var roRet in roRealEstTypesDict[ro.RoId])
                        {
                            tariff = Math.Max(tariff, dictRetTariff.Get(roRet) ?? 0);
                        } 
                    }

                    sectionMu["Tariff"] = tariff;

                    var area = 0m;
                    switch (retTariffParam)
                    {
                        case RateCalcTypeArea.AreaLiving:
                            area = ro.AreaLiving.GetValueOrDefault();
                            break;
                        case RateCalcTypeArea.AreaLivingNotLiving:
                            area = ro.AreaLivingNotLivingMkd.GetValueOrDefault();
                            break;
                        case RateCalcTypeArea.AreaMkd:
                            area = ro.AreaMkd.GetValueOrDefault();
                            break;
                    }

                    sectionMu["Area"] = area;

                    decimal chargedMonth = area * tariff;
                    decimal chargedYear = chargedMonth * 12;
                    decimal charged30Years = chargedYear * 30;

                    sectionMu["ChargedMonth"] = chargedMonth;
                    sectionMu["ChargedYear"] = chargedYear;
                    sectionMu["Charged30Years"] = charged30Years;

                    muChargedMonth += chargedMonth;
                    muChargedYear += chargedYear;
                    muCharged30Years += charged30Years;
                }

                section["MuChargedMonth"] = muChargedMonth;
                section["MuChargedYear"] = muChargedYear;
                section["MuCharged30Years"] = muCharged30Years;

                chargedMonthTotal += muChargedMonth;
                chargedYearTotal += muChargedYear;
                charged30YearsTotal += muCharged30Years;
            }

            reportParams.SimpleReportParams["TotalChargedMonth"] = chargedMonthTotal;
            reportParams.SimpleReportParams["TotalChargedYear"] = chargedYearTotal;
            reportParams.SimpleReportParams["TotalCharged30Years"] = charged30YearsTotal;
        }
    }
}