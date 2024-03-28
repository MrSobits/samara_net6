namespace Bars.Gkh.Overhaul.Nso.ProgrammPriorityParams
{
    using System.Collections.Generic;
    using System.Linq;

    using Entities;

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

        public int Year { get; set; }

        public IEnumerable<int> OverhaulYearsWithLifetimes { get; set; }

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

        public decimal GetValue(IStage3Entity stage3)
        {
            return Year - OverhaulYearsWithLifetimes.Min();
        }
    }
}