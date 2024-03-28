namespace Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams
{
    using System;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class DateInventoryParam : IProgrammPriorityParam
    {
        public static string Code
        {
            get { return "DateInventoryParam"; }
        }

        public bool Asc
        {
            get { return true; }
        }

        public string Name
        {
            get { return "Дата инвентаризации"; }
        }

        string IProgrammPriorityParam.Code
        {
            get { return Code; }
        }

        public DateTime? DateTechInspection { get; set; }

        public decimal GetValue(IStage3Entity stage3)
        {
            return DateTechInspection.HasValue ? (DateTechInspection.Value.Date - DateTime.MinValue).Days : Decimal.MaxValue;
        }
    }
}