namespace Bars.Gkh.Overhaul.Hmao.ValueResolver
{
    using System;
    using Bars.Gkh.Formulas;

    public class RoFirstPrivatizationYearResolver : FormulaParameterBase
    {
        public DateTime? PrivatizationDateFirstApartment { get; set; }

        public override string DisplayName
        {
            get { return "Дата приватизации первого жилого помещения"; }
        }

        public override string Code
        {
            get { return Id; }
        }

        public static string Id
        {
            get { return "RoFirstPrivatizationYear"; }
        }

        public override object GetValue()
        {
            return PrivatizationDateFirstApartment.HasValue ? PrivatizationDateFirstApartment.Value.Year : 0;
        }
    }
}