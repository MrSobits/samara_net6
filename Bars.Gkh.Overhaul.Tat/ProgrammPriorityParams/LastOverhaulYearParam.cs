namespace Bars.Gkh.Overhaul.Tat.ProgrammPriorityParams
{
    using System.Collections.Generic;
    
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;

    using Castle.Windsor;

    public class LastOverhaulYearParam : IProgrammPriorityParam
    {
        public static string Code
        {
            get
            {
                return "LastOverhaulYearParam";
            }
        }

        public IWindsorContainer Container { get; set; }

        public RealityObject RealityObject { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgramm> RoSeInProgramm { get; set; }

        public Dictionary<long, int> OvrhlYears { get; set; } 

        public int Id { get; set; }

        public bool Asc
        {
            get
            {
                return true;
            }
        }

        public string Name
        {
            get
            {
                return "Год последнего капремонта";
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
            var id = (Id > 0 && stage3 == null) ? Id : stage3.Id;

            var result = 0;
            if (OvrhlYears != null)
            {
                OvrhlYears.TryGetValue(id, out result);
            }
            /* незнаю зачем нужен этот код посколкьу OvrhlYears унас будет всегда
            else
            {
                var stage1Service = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>();

                var tempOvrhlYears = stage1Service
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
                                     x.StructuralElement.LastOverhaulYear,
                                     x.Year
                                 })
                             .ToList();

                var structElList = tempOvrhlYears.Where(x => x.Id == id).Select(
                    x => tempOvrhlYears.Any(y => x.RealityObjectId == y.RealityObjectId && y.StrElId == x.StrElId && y.Year < x.Year)
                            ? tempOvrhlYears.OrderByDescending(y => y.Year).First(y => x.RealityObjectId == y.RealityObjectId && y.StrElId == x.StrElId && y.Year < x.Year).Year
                            : x.LastOverhaulYear)
                .ToList();

                result = structElList.Max();
            }

            */

            /* Данное условие совсем непонятно
            if (result == 0)
            {
                result = RealityObject.BuildYear.HasValue
                    ? RealityObject.BuildYear.Value
                    : OverhaullParamProvider.GetOverhaulParam<int>("ProgrammPeriodEnd", Container) - 1;
            }
             */

            return result.ToDecimal();
        }
    }
}