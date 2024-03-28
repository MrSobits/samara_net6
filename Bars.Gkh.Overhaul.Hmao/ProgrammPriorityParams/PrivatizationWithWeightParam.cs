namespace Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams
{
    using System;

    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class PrivatizationWithWeightParam : IProgrammPriorityParam
    {
        public static string Code 
        {
            get { return "PrivatizationWithWeightParam"; }
        }

        public bool Asc 
        {
            get { return true; }
        }

        public string Name 
        {
            get { return "Критерий приватизации (с весами)"; }
        }

        string IProgrammPriorityParam.Code
        {
            get { return Code; }
        }

        public int? BuildYear { get; set; }

        public DateTime? PrivatizDate { get; set; }

        public decimal GetValue(IStage3Entity stage3)
        {
            var builYear = BuildYear.ToInt();
            var privatizYear = PrivatizDate.HasValue ? PrivatizDate.Value.Year : builYear;
            return (builYear + (0.7 * (privatizYear - builYear))).ToDecimal().RoundDecimal(0);
        }
    }
}