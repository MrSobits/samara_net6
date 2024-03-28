namespace Bars.Gkh.Decisions.Nso.Entities
{
    using System.Collections.Generic;

    using Bars.Gkh.Overhaul.Entities;

    public class JobYearDecision : UltimateDecision
    {
        public virtual List<RealtyJobYear> JobYears { get; set; }

        public JobYearDecision()
        {
            JobYears = new List<RealtyJobYear>();
        }
    }

    public class RealtyJobYear
    {
        public Job Job { get; set; }

        public int PlanYear { get; set; }

        public int? UserYear { get; set; }
    }
}