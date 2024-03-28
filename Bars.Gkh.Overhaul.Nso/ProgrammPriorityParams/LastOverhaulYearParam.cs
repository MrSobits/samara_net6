namespace Bars.Gkh.Overhaul.Nso.ProgrammPriorityParams
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;

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

        public IEnumerable<int> OverhaulYears { get; set; }

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

        public decimal GetValue(IStage3Entity stage3)
        {
            return OverhaulYears.Any(x => x < stage3.Year) ? OverhaulYears.Where(x => x < stage3.Year).Max() : stage3.Year;


            /*var id = (Id > 0) ? Id : 0;

            var result = 0;
            if (OvrhlYears != null)
            {
                OvrhlYears.TryGetValue(id, out result);
            }*/
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

            //return result.ToDecimal();
        }
    }
}