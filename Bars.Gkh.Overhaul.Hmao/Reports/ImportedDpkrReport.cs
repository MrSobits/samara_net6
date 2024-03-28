namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.FIAS;
    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.B4.IoC;
    using Castle.Windsor;
    using Entities;
    using Enums;
    using Gkh.Entities;
    using Overhaul.Entities;

    public class ImportedDpkrReport : BasePrintForm
    {
        #region .ctor

        public ImportedDpkrReport(IWindsorContainer container) : base(new ReportTemplateBinary(Properties.Resources.ImportedDpkrReport))
        {
            _container = container;
            _muIds = new long[0];
        }

        #endregion

        #region Properties

        public IWindsorContainer Container
        {
            get
            {
                return _container;
            }
        }

        public override string Name
        {
            get { return "Отчет по ДПКР для импорта"; }
        }

        public override string Desciption
        {
            get { return "Отчет по ДПКР для импорта"; }
        }

        public override string GroupName
        {
            get { return "Долгосрочная программа"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ImportedDpkrReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhOverhaul.ImportedDpkrReport"; }
        }

        #endregion

        #region Fields

        private readonly IWindsorContainer _container;

        private long[] _muIds;

        private DataSource _dataSource;

        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            if (!baseParams.Params.GetAs<string>("muIds").IsEmpty())
            {
                _muIds = baseParams.Params.GetAs<string>("muIds").Split(',').Select(x => x.ToLong()).ToArray();
            }

            _dataSource = baseParams.Params.GetAs<DataSource>("dataSource");
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            int maxCount;

            var query = _dataSource == DataSource.Dpkr
                ? GetDpkrData(out maxCount)
                : GetVersionData(out maxCount);

            var dataSe = query
                .Select(x => new
                {
                    x.RoSeId,
                    x.Year
                })
                .AsEnumerable()
                .GroupBy(x => x.RoSeId)
                .ToDictionary(x => x.Key, y => y.OrderBy(x => x.Year).Select(x => x.Year));

            var dataRo = Container.ResolveRepository<RealityObject>().GetAll()
                .Where(y => query.Any(x => x.RoId == y.Id))
                .Select(x => new
                {
                    x.Id,
                    MuId = x.Municipality.Id,
                    x.FiasAddress.House,
                    x.FiasAddress.Letter,
                    x.FiasAddress.Housing,
                    x.FiasAddress.Building,
                    x.FiasAddress.PlaceGuidId,
                    x.FiasAddress.StreetGuidId,
                    Municipality = x.Municipality.Name,
                    x.Address,

                    x.BuildYear,
                    x.Floors,
                    x.NumberEntrances,
                    x.NumberApartments,
                    x.AreaLiving,
                    x.AreaNotLivingFunctional,
                    x.AreaCommonUsage
                });

            Dictionary<long, long> seJob;

            var seworkDomain = Container.ResolveDomain<StructuralElementWork>();

            using (Container.Using(seworkDomain))
            {
                seJob = Container.ResolveDomain<StructuralElementWork>().GetAll()
                    .Select(x => new
                    {
                        JobId = x.Job.Id,
                        SeId = x.StructuralElement.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.SeId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.JobId).First());
            }

            var workPrices = GetDictWorkPrice();

            var fiasRepos = Container.ResolveRepository<Fias>();

            var fiasStreetName = fiasRepos.GetAll()
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .Where(y => dataRo.Any(x => x.StreetGuidId == y.AOGuid))
                .GroupBy(x => x.AOGuid)
                .ToDictionary(x => x.Key, y => y.Select(x => x.OffName + " " + x.ShortName).First());

            var fiasPlaceName = fiasRepos.GetAll()
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .Where(y => dataRo.Any(x => x.PlaceGuidId == y.AOGuid))
                .GroupBy(x => x.AOGuid)
                .ToDictionary(x => x.Key, y => y.Select(x => x.OffName + " " + x.ShortName).First());

            var roDict = dataRo.ToDictionary(x => x.Id);

            var resultData = query
                .AsEnumerable()
                .GroupBy(x => x.RoSeId)
                .Select(x => x.First());

            var vSection = reportParams.ComplexReportParams.ДобавитьСекцию("verticalSection");

            for (int i = 1; i <= maxCount; i++)
            {
                vSection.ДобавитьСтроку();

                vSection["YearNumber"] = "Год" + i;
                vSection["Year"] = string.Format("$year{0}$", i);
            }

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var item in resultData)
            {
                if (!dataSe.ContainsKey(item.RoSeId) || !roDict.ContainsKey(item.RoId))
                {
                    continue;
                }

                section.ДобавитьСтроку();

                var ro = roDict[item.RoId];

                section["indexNumber"] = item.IndexNumber;
                section["municipality"] = ro.Municipality;
                section["placement"] = ro.PlaceGuidId != null ? fiasPlaceName.Get(ro.PlaceGuidId) : null;
                section["street"] = ro.StreetGuidId != null ? fiasStreetName.Get(ro.StreetGuidId) : null;
                section["house"] = ro.House;
                section["letter"] = ro.Letter;
                section["housing"] = ro.Housing;
                section["building"] = ro.Building;
                section["buildYear"] = ro.BuildYear;
                section["floors"] = ro.Floors;
                section["numberEntrances"] = ro.NumberEntrances;
                section["numberApartments"] = ro.NumberApartments;
                section["areaLiving"] = ro.AreaLiving;
                section["areaNotLivingFunctional"] = ro.AreaNotLivingFunctional;
                section["areaCommonUsage"] = ro.AreaCommonUsage;
                section["ceo"] = item.CeoName;
                section["se"] = item.SeName;
                section["seCode"] = item.SeCode;
                section["seVolume"] = item.Volume;
                section["lifetimeAfterRepair"] = item.LifeTimeAfterRepair;
                section["lastOverhaulYear"] = item.LastOverhaulYear;
                section["sum"] = item.Sum;
                section["points"] = item.Point;

                int i = 1;

                foreach (var year in dataSe[item.RoSeId])
                {
                    if (i == 1)
                    {
                        section["firstYear"] = year;
                    }

                    section["year" + i] = year;

                    i++;
                }

                if (seJob.ContainsKey(item.SeId)
                    && workPrices.ContainsKey(ro.MuId)
                    && workPrices[ro.MuId].ContainsKey(seJob[item.SeId]))
                {
                    decimal? price = null;

                    switch (item.CalculateBy)
                    {
                        case PriceCalculateBy.Volume:
                            price = workPrices[ro.MuId][seJob[item.SeId]].NormativeCost;
                            break;
                        case PriceCalculateBy.LivingArea:
                        case PriceCalculateBy.TotalArea:
                        case PriceCalculateBy.AreaLivingNotLivingMkd:
                            price = workPrices[ro.MuId][seJob[item.SeId]].SquareMeterCost;
                            break;
                    }

                    section["price"] = price;
                }
            }
        }

        protected IQueryable<Data> GetDpkrData(out int maxCount)
        {
            var roseInProgDomain = Container.ResolveDomain<RealityObjectStructuralElementInProgramm>();

            using (Container.Using(roseInProgDomain))
            {
                var data = roseInProgDomain.GetAll()
                    .WhereIf(!_muIds.IsEmpty(),
                        x => _muIds.Contains(x.Stage2.RealityObject.Municipality.Id)
                             || _muIds.Contains(x.Stage2.RealityObject.Municipality.ParentMo.Id))
                    .Where(x => x.Stage2.RealityObject.Municipality != null);

                maxCount = data
                    .GroupBy(x => x.StructuralElement.Id)
                    .Select(x => x.Count())
                    .OrderByDescending(x => x)
                    .FirstOrDefault();

                return data
                    .Select(x => new Data
                    {
                        RoId = x.StructuralElement.RealityObject.Id,
                        RoSeId = x.StructuralElement.Id,
                        SeId = x.StructuralElement.StructuralElement.Id,
                        CeoName = x.StructuralElement.StructuralElement.Group.CommonEstateObject.Name,
                        SeName = x.StructuralElement.StructuralElement.Name,
                        SeCode = x.StructuralElement.StructuralElement.Code,
                        LifeTimeAfterRepair = x.StructuralElement.StructuralElement.LifeTimeAfterRepair,
                        CalculateBy = x.StructuralElement.StructuralElement.CalculateBy,

                        Volume = x.StructuralElement.Volume,
                        LastOverhaulYear = x.StructuralElement.LastOverhaulYear,

                        Sum = x.Stage2.Stage3.Sum,
                        Point = x.Stage2.Stage3.Point,
                        IndexNumber = x.Stage2.Stage3.IndexNumber,
                        Year = x.Stage2.Stage3.Year
                    });
            }
        }

        protected IQueryable<Data> GetVersionData(out int maxCount)
        {
            var versionst1Domain = Container.ResolveDomain<VersionRecordStage1>();

            using (Container.Using(versionst1Domain))
            {
                var data = versionst1Domain.GetAll()
                    .WhereIf(!_muIds.IsEmpty(),
                        x => _muIds.Contains(x.Stage2Version.Stage3Version.ProgramVersion.Municipality.Id)
                             || _muIds.Contains(x.Stage2Version.Stage3Version.ProgramVersion.Municipality.ParentMo.Id))
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Municipality != null)
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain);

                maxCount = data
                    .GroupBy(x => x.StructuralElement.Id)
                    .Select(x => x.Count())
                    .OrderByDescending(x => x)
                    .FirstOrDefault();

                return data
                    .Select(x => new Data
                    {
                        RoId = x.RealityObject.Id,
                        RoSeId = x.StructuralElement.Id,
                        SeId = x.StructuralElement.StructuralElement.Id,
                        CeoName = x.StructuralElement.StructuralElement.Group.CommonEstateObject.Name,
                        SeName = x.StructuralElement.StructuralElement.Name,
                        SeCode = x.StructuralElement.StructuralElement.Code,
                        LifeTimeAfterRepair = x.StructuralElement.StructuralElement.LifeTimeAfterRepair,
                        CalculateBy = x.StructuralElement.StructuralElement.CalculateBy,

                        Volume = x.StructuralElement.Volume,
                        LastOverhaulYear = x.StructuralElement.LastOverhaulYear,

                        Sum = x.Stage2Version.Stage3Version.Sum,
                        Point = x.Stage2Version.Stage3Version.Point,
                        IndexNumber = x.Stage2Version.Stage3Version.IndexNumber,
                        Year = x.Stage2Version.Stage3Version.Year
                    });
            }
        }

        protected Dictionary<long, Dictionary<long, CostProxy>> GetDictWorkPrice()
        {
            var workpriceDomain = Container.ResolveDomain<WorkPrice>();
            using (Container.Using(workpriceDomain))
            {
                return workpriceDomain.GetAll()
                .WhereIf(!_muIds.IsEmpty(), x => _muIds.Contains(x.Municipality.Id) || _muIds.Contains(x.Municipality.ParentMo.Id))
                .Where(x => x.Municipality != null)
                .Where(x => x.Job != null)
                .Select(x => new
                {
                    JobId = x.Job.Id,
                    MuId = x.Municipality.Id,
                    x.SquareMeterCost,
                    x.NormativeCost
                })
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key,
                    y => y
                        .GroupBy(x => x.JobId)
                        .ToDictionary(x => x.Key, z => new CostProxy
                        {
                            NormativeCost = z.Average(x => x.NormativeCost),
                            SquareMeterCost = z.Average(x => x.SquareMeterCost).GetValueOrDefault()
                        }));
            }
        }

        protected class Data
        {
            public long RoId { get; set; }
            public long RoSeId { get; set; }
            public long SeId { get; set; }
            public string CeoName { get; set; }
            public string SeName { get; set; }
            public string SeCode { get; set; }
            public int? LifeTimeAfterRepair { get; set; }
            public PriceCalculateBy CalculateBy { get; set; }
            public decimal Volume { get; set; }
            public int LastOverhaulYear { get; set; }
            public decimal Sum { get; set; }
            public decimal Point { get; set; }
            public int IndexNumber { get; set; }
            public int Year { get; set; }
        }

        protected class CostProxy
        {
            public decimal NormativeCost { get; set; }

            public decimal SquareMeterCost { get; set; }
        }

        // при изменении значения енумов проверить, что значения здесь и на панели отчета идентичны
        protected enum DataSource
        {
            Version = 0,

            Dpkr = 1
        }
    }
}