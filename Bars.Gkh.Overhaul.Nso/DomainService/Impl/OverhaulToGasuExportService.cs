namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.GkhCr.Entities;
    using Castle.Windsor;

    public class OverhaulToGasuExportService : IOverhaulToGasuExportService
    {
        public IWindsorContainer Container { get; set; }

        public List<OverhaulToGasuProxy> GetData(DateTime startDate, long programId)
        {

            var typeWorkCodeVolumeIndexId = new Dictionary<string, string>
            {
                {"1", "390010613"},
                {"2", "390010614"},
                {"3", "390010615"},
                {"4", "390010616"},
                {"5", "390010617"},
                {"6", "390010618"},
                {"7", "390010619"},
                {"8", "390010620"},
                {"9", "390010621"},
                {"10", "390010622"},
                {"11", "390010623"},
                {"12", "390010624"},
                {"13", "390010625"},
                {"14", "390010626"},
                {"16", "390010627"},
                {"31", "390010628"},
                {"123", "390010629"}
            };

            var typeWorkCodeSumIndexId = new Dictionary<string, string>
            {
                {"1", "390010631"},
                {"2", "390010632"},
                {"3", "390010633"},
                {"4", "390010634"},
                {"5", "390010635"},
                {"6", "390010636"},
                {"7", "390010637"},
                {"8", "390010638"},
                {"9", "390010639"},
                {"10", "390010640"},
                {"11", "390010641"},
                {"12", "390010642"},
                {"13", "390010643"},
                {"14", "390010644"},
                {"16", "390010645"},
                {"31", "390010646"},
                {"123", "390010647"}
            };

            var unitMeasureIdDict = new Dictionary<string, string>
            {
                {"ед.", "642"},
                {"кв.м", "055"}
            };

            var result = new List<OverhaulToGasuProxy>();
            var versionRecordDomain = Container.ResolveDomain<VersionRecord>();
            var typeWorkCrDomain = Container.ResolveDomain<TypeWorkCr>();

            try
            {
                var countRealObjs = versionRecordDomain.GetAll()
                    .Where(x => x.ProgramVersion.IsMain)
                    .Where(x => x.Year > startDate.Year)
                    .Select(x => new
                    {
                        MuId = x.RealityObject.Municipality.Id,
                        x.RealityObject.Municipality.Okato,
                        RoId = x.RealityObject.Id,
                        x.Year
                    })
                    .ToList()
                    .GroupBy(x => new {x.MuId, x.Year})
                    .Select(x => new OverhaulToGasuProxy
                    {
                        DateStart = new DateTime(x.Key.Year, 1, 1).ToShortDateString(),
                        FactOrPlan = (int) FactOrPlan.Plan,
                        Okato = x.Select(y => y.Okato).First(),
                        Value = x.Select(y => y.RoId).Distinct().Count().ToStr(),
                        IndexId = "390010605",
                        UnitId = "642"
                    });

                versionRecordDomain.GetAll()
                    .Where(x => x.ProgramVersion.IsMain)
                    .Where(x => x.Year > startDate.Year)
                    .Select(x => new
                    {
                        MuId = x.RealityObject.Municipality.Id,
                        x.RealityObject.Municipality.Okato,
                        x.RealityObject.MaximumFloors,
                        x.Year,
                        x.Sum
                    })
                    .ToList()
                    .GroupBy(x => new {x.MuId, x.Year})
                    .ForEach(x =>
                    {
                        var date = new DateTime(x.Key.Year, 1, 1).ToShortDateString();
                        var okato = x.Select(y => y.Okato).First();

                        result.Add(new OverhaulToGasuProxy
                        {
                            DateStart = date,
                            FactOrPlan = (int) FactOrPlan.Plan,
                            Okato = okato,
                            Value = x.Where(y => y.MaximumFloors >= 1 && y.MaximumFloors <= 3)
                                .SafeSum(y => y.Sum)
                                .RoundDecimal(2).ToFormatedString('.'),
                            IndexId = "390010607",
                            UnitId = "383"
                        });

                        result.Add(new OverhaulToGasuProxy
                        {
                            DateStart = date,
                            FactOrPlan = (int) FactOrPlan.Plan,
                            Okato = okato,
                            Value = x.Where(y => y.MaximumFloors >= 4 && y.MaximumFloors <= 6)
                                .SafeSum(y => y.Sum)
                                .RoundDecimal(2).ToFormatedString('.'),
                            IndexId = "390010608",
                            UnitId = "383"
                        });

                        result.Add(new OverhaulToGasuProxy
                        {
                            DateStart = date,
                            FactOrPlan = (int) FactOrPlan.Plan,
                            Okato = okato,
                            Value = x.Where(y => y.MaximumFloors >= 7 && y.MaximumFloors <= 10)
                                .SafeSum(y => y.Sum)
                                .RoundDecimal(2).ToFormatedString('.'),
                            IndexId = "390010609",
                            UnitId = "383"
                        });

                        result.Add(new OverhaulToGasuProxy
                        {
                            DateStart = date,
                            FactOrPlan = (int) FactOrPlan.Plan,
                            Okato = okato,
                            Value = x.Where(y => y.MaximumFloors > 10).SafeSum(y => y.Sum).RoundDecimal(2).ToFormatedString('.'),
                            IndexId = "390010610",
                            UnitId = "383"
                        });
                    });

                var sumList = new List<OverhaulToGasuProxy>();
                var volumeList = new List<OverhaulToGasuProxy>();

                var typeWorkCodes = typeWorkCodeSumIndexId.Keys.ToList();

                typeWorkCrDomain.GetAll()
                    .Where(x => x.ObjectCr.ProgramCr.Id == programId)
                    .Where(x => typeWorkCodes.Contains(x.Work.Code))
                    .Select(x => new
                    {
                        MuId = x.ObjectCr.RealityObject.Municipality.Id,
                        x.ObjectCr.RealityObject.Municipality.Okato,
                        x.Work.Code,
                        x.Volume,
                        x.CostSum,
                        x.Work.UnitMeasure.Name
                    })
                    .ToList()
                    .GroupBy(x => new {x.MuId, x.Code})
                    .ForEach(x =>
                    {
                        var date = startDate.ToShortDateString();
                        var okato = x.Select(y => y.Okato).First();
                        var umName = x.Select(y => y.Name).First();

                        volumeList.Add(new OverhaulToGasuProxy
                        {
                            DateStart = date,
                            FactOrPlan = (int) FactOrPlan.Plan,
                            Okato = okato,
                            Value = x.SafeSum(y => y.Volume.ToDecimal()).RoundDecimal(2).ToFormatedString('.'),
                            IndexId = typeWorkCodeVolumeIndexId.Get(x.Key.Code),
                            UnitId = unitMeasureIdDict.Get(umName) ?? "055"
                        });

                        sumList.Add(new OverhaulToGasuProxy
                        {
                            DateStart = date,
                            FactOrPlan = (int) FactOrPlan.Fact,
                            Okato = okato,
                            Value = x.SafeSum(y => y.CostSum.ToDecimal()).RoundDecimal(2).ToFormatedString('.'),
                            IndexId = typeWorkCodeSumIndexId.Get(x.Key.Code),
                            UnitId = "383"
                        });
                    });


                result.AddRange(countRealObjs);
                result.AddRange(volumeList);
                result.AddRange(sumList);
            }
            finally
            {
                Container.Release(versionRecordDomain);
                Container.Release(typeWorkCrDomain);
            }

            return result;
        }
    }
}