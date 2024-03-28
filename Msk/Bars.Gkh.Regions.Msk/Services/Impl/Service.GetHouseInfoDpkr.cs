namespace Bars.Gkh.Regions.Msk.Services.Impl
{
    using System;
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;
    using Enums;
    using Gkh.Entities;
    using Overhaul.Nso.Entities;
    using Overhaul.Services.DataContracts;

    public partial class Service
    {
        public GetHouseInfoDpkrResponse GetHouseInfoDpkr(string id)
        {
            var realityObjectDomain = Container.ResolveDomain<RealityObject>();
            var stage1Domain = Container.ResolveDomain<RealityObjectStructuralElementInProgramm>();
            var tehPassportValueDomain = Container.ResolveDomain<TehPassportValue>();

            try
            {
                var currentYear = DateTime.Now.Year;

                var result = new GetHouseInfoDpkrResponse();

                var realObj = realityObjectDomain.Get(id.ToLong());

                if (realObj == null)
                {
                    result.Result = new GetHouseInfoDpkrResult
                    {
                        Kod = "01",
                        Name = "MKD_NOT_FOUND"
                    };

                    return result;
                }

                result.Result = new GetHouseInfoDpkrResult
                {
                    Kod = "00",
                    Name = "NO_ERRORS"
                };

                result.House = new House
                {
                    Address = realObj.Address,
                    YearBuilding = realObj.BuildYear.ToStr(),
                    TotalArea = realObj.AreaMkd.ToDecimal().ToString("G29"),
                    Serial = realObj.TypeProject != null ? realObj.TypeProject.Name : string.Empty,
                    LivingArea = realObj.AreaLiving.ToDecimal().ToString("G29"),
                    NoLivingArea = realObj.AreaNotLivingFunctional.ToDecimal().ToString("G29"),
                    Floor = realObj.NumberApartments.ToStr(),
                    Porch = realObj.NumberEntrances.ToStr(),
                    Flat = realObj.MaximumFloors.ToStr(),
                    Point = realObj.Points.ToInt()
                };

                var dpkrInfos = stage1Domain.GetAll()
                    .Where(x => x.StructuralElement.RealityObject.Id == realObj.Id)
                    .OrderBy(x => x.Year)
                    .Select(x => new
                    {
                        CeoName = x.StructuralElement.StructuralElement.Group.CommonEstateObject.Name,
                        StructelName = x.StructuralElement.StructuralElement.Name,
                        x.StructuralElement.Condition,
                        x.LastOverhaulYear,
                        x.StructuralElement.StructuralElement.LifeTime,
                        x.Stage2.Stage3.Point,
                        x.Year
                    })
                    .ToArray()
                    .Select(x => new Ceo
                    {
                        CeoName = x.CeoName,
                        StructElName = x.StructelName,
                        Status = x.Condition == ConditionStructElement.Unsatisfactory ? 1 :
                                x.Condition == ConditionStructElement.Emergency ? 2: 0,
                        Delay = (x.LastOverhaulYear + x.LifeTime - currentYear).ToStr(),
                        LifeTime = x.LifeTime,
                        LastYear = x.LastOverhaulYear,
                        Period = "{0}-{1}".FormatUsing(x.Year - 1, x.Year),
                        Point = Convert.ToInt32(x.Point)
                    })
                    .ToArray();

                result.Ceos = dpkrInfos;

                var liftInfos = tehPassportValueDomain.GetAll()
                    .Where(x => x.TehPassport.RealityObject.Id == realObj.Id)
                    .Where(x => x.FormCode == "Form_4_2_1")
                    .Select(x => new
                    {
                        x.CellCode,
                        x.Value
                    })
                    .ToArray()
                    .Select(x =>
                    {
                        var array = x.CellCode.Split(':');

                        return new
                            {
                                Num = array[0].Trim(),
                                Cell = array[1].Trim(),
                                x.Value
                            };
                    })
                    .GroupBy(y => y.Num)
                    .Select(y => new Lift
                        {
                            LiftNum = y.Where(z => z.Cell == "2").Select(z => z.Value).FirstOrDefault(),
                            PorchNum = y.Where(z => z.Cell == "1").Select(z => z.Value).FirstOrDefault(),
                            Capacity = y.Where(z => z.Cell == "5").Select(z => z.Value).FirstOrDefault(),
                            Stop = y.Where(z => z.Cell == "7").Select(z => z.Value).FirstOrDefault(),
                            YearInstallation = y.Where(z => z.Cell == "9").Select(z => z.Value).FirstOrDefault(),
                            LifeTime = y.Where(z => z.Cell == "17").Select(z => z.Value).FirstOrDefault(),
                            Period = y.Where(z => z.Cell == "18").Select(z => z.Value).FirstOrDefault(),
                        })
                    .ToArray();

                result.Lifts = liftInfos;

                return result;
            }
            finally
            {
                Container.Release(realityObjectDomain);
                Container.Release(stage1Domain);
                Container.Release(tehPassportValueDomain);
            }
        }
    }
}