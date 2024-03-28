namespace Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;

    using Castle.Windsor;

    public class LastOverhaulYearParam : IProgrammPriorityParam
    {
        public static string Code
        {
            get { return "LastOverhaulYearParam"; }
        }

        public IWindsorContainer Container { get; set; }

        public IEnumerable<int> OverhaulYears { get; set; }

        public bool Asc
        {
            get { return true; }
        }

        public string Name
        {
            get { return "Год последнего капремонта"; }
        }

        string IProgrammPriorityParam.Code
        {
            get { return Code; }
        }

        public decimal GetValue(IStage3Entity stage3)
        {
            return OverhaulYears.Any(x => x < stage3.Year) ? OverhaulYears.Where(x => x < stage3.Year).Max() : stage3.Year;
        }
    }
}