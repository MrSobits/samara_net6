namespace Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams
{
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class RoWearoutParam : IProgrammPriorityParam
    {
        public decimal? PhysicalWear { get; set; }

        public static string Code
        {
            get { return "RoWearoutParam"; }
        }

        public bool Asc
        {
            get { return false; }
        }

        public string Name 
        {
            get { return "Степень износа объекта недвижимости"; }
        }

        string IProgrammPriorityParam.Code 
        {
            get { return Code; }
        }

        public decimal GetValue(IStage3Entity stage3)
        {
            return PhysicalWear.ToDecimal();
        }
    }
}