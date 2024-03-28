namespace Bars.Gkh.Overhaul.Tat.ProgrammPriorityParams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;

    using Castle.Windsor;

    public class NeedOverhaulParam : IProgrammPriorityParam
    {
        public static string Code
        {
            get
            {
                return "NeedOverhaul";
            }
        }

        public IWindsorContainer Container { get; set; }

        public RealityObject RealityObject { get; set; }

        public Dictionary<long, int> OvrhlYears { get; set; }

        public Dictionary<long, int> Lifetimes { get; set; }

        public Dictionary<long, int> YearWithLifetimes { get; set; }

        public Dictionary<long, int> Years { get; set; }

        public long Id { get; set; }

        public bool Asc { get { return false; } }

        public string Name
        {
            get
            {
                return "Потребность в проведении кап.ремонта на дату приватизации первого жилого помещения";
            }
        }

        string IProgrammPriorityParam.Code
        {
            get
            {
                return Code;
            }
        }

        public decimal GetValue(RealityObjectStructuralElementInProgrammStage3 stage3)
        {
            if (RealityObject == null)
            {
                return 0;
            }

            var privDate = PrivatizationDateFirstApartment(stage3);
            var id = ((Id > 0 && stage3 == null) ? Id : stage3.Id);
            int val;

            if (Years.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value).ContainsKey(id))
            {
                var idsArr = Years.Keys.ToArray();
                var index = Array.IndexOf(idsArr, id);

                if (index > 0)
                {
                    var valArr = Years.Values.ToArray();
                    val = valArr[index - 1];
                }
                else
                {
                    YearWithLifetimes.TryGetValue(id, out val);
                }
            }
            else
            {
                YearWithLifetimes.TryGetValue(id, out val);
            }

            return val > privDate ? 0m : 1m;
        }

        private int PrivatizationDateFirstApartment(RealityObjectStructuralElementInProgrammStage3 stage3)
        {
            var ro = RealityObject ?? stage3.RealityObject;
            return ro.PrivatizationDateFirstApartment.HasValue ? ro.PrivatizationDateFirstApartment.Value.Year : 0;
        }

        /* Не знаю зчем эти методы нужны
        private int BuildYear(RealityObjectStructuralElementInProgrammStage3 stage3)
        {
            var ro = RealityObject ?? stage3.RealityObject;
            int maxYear = 0;
            var id = ((Id > 0 && stage3 == null) ? Id : stage3.Id);
            if (OvrhlYears != null)
            {
                OvrhlYears.TryGetValue(id, out maxYear);
            }
            else
            {
                var stage1Service = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>();

                var tempOvrhlYears = stage1Service
                             .GetAll()
                             .Where(x => stage1Service.GetAll().Any(y => y.Stage2.Stage3.Id == id &&
                                 x.StructuralElement.Id == y.StructuralElement.Id && x.StructuralElement.RealityObject.Id == y.StructuralElement.RealityObject.Id))
                             .Select(
                                 x =>
                                 new
                                     {
                                         RealityObjectId = x.StructuralElement.RealityObject.Id,
                                         StrElId = x.StructuralElement.Id,
                                         x.Stage2.Stage3.Id,
                                         x.StructuralElement.LastOverhaulYear,
                                         x.Year
                                     })
                             .ToList();

                var ovrhlYears = tempOvrhlYears.Where(x => x.Id == id).Select(
                    x => tempOvrhlYears.Any(y => x.RealityObjectId == y.RealityObjectId && y.StrElId == x.StrElId && y.Year < x.Year)
                            ? tempOvrhlYears.OrderByDescending(y => y.Year).First(y => x.RealityObjectId == y.RealityObjectId && y.StrElId == x.StrElId && y.Year < x.Year).Year
                            : x.LastOverhaulYear
                    )
                .ToList();

                maxYear = ovrhlYears.Any() ? ovrhlYears.Max() : 0;
            }

            if (maxYear == 0 && ro.BuildYear.HasValue)
            {
                maxYear = ro.BuildYear.Value;
            }

            return maxYear;
        }

        private int RepairTime(RealityObjectStructuralElementInProgrammStage3 stage3)
        {
            var id = ((Id > 0 && stage3 == null) ? Id : stage3.Id);
            if (Lifetimes != null)
            {
                int val = 0;
                Lifetimes.TryGetValue(id, out val);

                return val;
            }

            var stage1Service = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>();

            var tempLifeTimes = stage1Service
                             .GetAll()
                             .Where(x => stage1Service.GetAll().Any(y => y.Stage2.Stage3.Id == id
                                        && x.StructuralElement.Id == y.StructuralElement.Id && x.StructuralElement.RealityObject.Id == y.StructuralElement.RealityObject.Id))
                             .Select(
                                 x =>
                                 new
                                 {
                                     RealityObjectId = x.StructuralElement.RealityObject.Id,
                                     StrElId = x.StructuralElement.Id,
                                     x.Stage2.Stage3.Id,
                                     x.StructuralElement.StructuralElement.LifeTime,
                                     x.StructuralElement.StructuralElement.LifeTimeAfterRepair,
                                     x.Year
                                 })
                             .ToList();

            var lifetimes = tempLifeTimes.Where(x => x.Id == id).Select(
                x => x.LifeTimeAfterRepair > 0 && tempLifeTimes.Any(y => x.RealityObjectId == y.RealityObjectId && y.StrElId == x.StrElId && y.Year < x.Year)
                        ? x.LifeTimeAfterRepair
                        : x.LifeTime
                )
            .ToList();

            return lifetimes.Any() ? lifetimes.Max() : 0;
        }
        */
    }
}