namespace Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams
{
    using System;
    using Entities;

    public class RoLifetimeParam : IProgrammPriorityParam
    {
        public static string Code
        {
            get { return "RoLifetime"; }
        }

        public string Name
        {
            get { return "Срок эксплуатации МКД"; }
        }

        public bool Asc
        {
            get { return false; }
        }

        string IProgrammPriorityParam.Code
        {
            get { return Code; }
        }

        public int? BuildYear { get; set; }

        public DateTime? DateCommissioning { get; set; }

        public decimal GetValue(IStage3Entity stage3)
        {
            return DateCommissioning.HasValue
                ? (DateTime.Now.Date - DateCommissioning.Value.Date).Days
                : BuildYear.HasValue
                    ? (DateTime.Now.Date - new DateTime(BuildYear.Value, 1, 1)).Days
                    : 0m;
        }
    }
}