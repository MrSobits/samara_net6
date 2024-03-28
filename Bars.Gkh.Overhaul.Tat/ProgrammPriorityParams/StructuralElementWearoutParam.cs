using System.Collections.Generic;

namespace Bars.Gkh.Overhaul.Tat.ProgrammPriorityParams
{

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class StructuralElementWearoutParam : IProgrammPriorityParam
    {
        public static string Code
        {
            get
            {
                return "StructuralElementWearout";
            }
        }

        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgramm> Stage1Domain { get; set; }

        public RealityObject RealityObject { get; set; }

        public long Id { get; set; }

        public Dictionary<long, int> YearWithLifetimes { get; set; }

        public Dictionary<long, int> Lifetimes { get; set; }

        public bool Asc { get { return false; } }

        public string Name
        {
            get
            {
                return "Недоремонт (износ элемента)";
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
            var config = Container.GetGkhConfig<OverhaulTatConfig>();
            var startYear = config.ProgrammPeriodStart;

            int planYear = 0;

            var id = (Id > 0 && stage3 == null) ? Id : stage3.Id;
            if (YearWithLifetimes != null)
            {
                YearWithLifetimes.TryGetValue(id, out planYear);
            }

            /* Непонятно зачем все это делается в списке YearWithLifetimes и так есть все записи по 3 этапу и в else мы никогда непопадем
               поэтому смысла в этом коде невижу
            else
            {
                var tempData = Stage1Domain
                                 .GetAll()
                                 .Where(x => Stage1Domain.GetAll().Any(y => y.Stage2.Stage3.Id == id
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
                                         x.StructuralElement.LastOverhaulYear,
                                         x.StructuralElement.RealityObject.BuildYear,
                                         x.Year
                                     })
                                 .ToList();

                var structElem = tempData.Where(x => x.Id == id).Select(x => new
                                                          {
                                                              LastOverhaulYear = tempData.Any(y => x.RealityObjectId == y.RealityObjectId && y.StrElId == x.StrElId && y.Year < x.Year)
                                                                ? tempData.OrderByDescending(y => y.Year).First(y => x.RealityObjectId == y.RealityObjectId && y.StrElId == x.StrElId && y.Year < x.Year).Year
                                                                : x.LastOverhaulYear,
                                                              LifeTime = x.LifeTimeAfterRepair > 0 && tempData.Any(y => x.RealityObjectId == y.RealityObjectId && y.StrElId == x.StrElId && y.Year < x.Year)
                                                                ? x.LifeTimeAfterRepair
                                                                : x.LifeTime,
                                                              x.BuildYear
                                                          }).ToList();

                if (structElem.Any())
                {
                    var lastOverhaulYears = structElem.Where(x => x.LastOverhaulYear > 0).ToList();

                    if (lastOverhaulYears.Any())
                    {
                        planYear = lastOverhaulYears.Max(x => x.LastOverhaulYear > 0 ? x.LastOverhaulYear : x.BuildYear.ToInt() + x.LifeTime);
                    }
                    else
                    {
                        planYear = ro.BuildYear.HasValue ? ro.BuildYear.Value : startYear;
                    }
                }
            }
            */

            var result = startYear - planYear;

            /*
             Тут необходимо было что бы отрицательные значения занулялись но оказалось что это ненужно
             надо показывать именно с отрицательными значениями
            if (result < 0)
            {
                return 0;
            }
            */

            return result;
        }
    }
}