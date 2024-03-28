namespace Bars.Gkh.Overhaul.Tat.ValueResolver
{
    using System;
    using Bars.Gkh.Formulas;
    using Castle.Windsor;

    public class RoFirstPrivatizationYearResolver : FormulaParameterBase
    {
        public IWindsorContainer Container { get; set; }

        public DateTime? PrivatizDate { get; set; }

        public override string DisplayName
        {
            get
            {
                return "Дата приватизации первого жилого помещения";
            }
        }

        public override string Code
        {
            get
            {
                return Id;
            }
        }

        public static string Id
        {
            get
            {
                return "RoFirstPrivatizationYear";
            }
        }

        public override object GetValue()
        {
            return PrivatizDate.HasValue ? PrivatizDate.Value.Year : 0;
        }
    }
}