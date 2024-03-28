namespace Bars.Gkh.Overhaul.Nso.ProgrammPriorityParams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Entities;

    using Castle.Windsor;

    public class NeedOverhaulParam : IProgrammPriorityParam
    {
        public static string Code
        {
            get
            {
                return "NeedOverhaul";
            }
        }

        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Дата приватизации первого жилого помещения
        /// </summary>
        public DateTime? PrivatizDate { get; set; }

        /// <summary>
        /// Годы ремонта конструктивных элементов из Stage1
        /// </summary>
        public IEnumerable<int> FirstPrivYears { get; set; }

        public bool Asc { get { return false; } }

        public string Name
        {
            get
            {
                return "Потребность в проведении кап.ремонта на дату приватизации первого жилого помещения";
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
            var privatizYear = PrivatizDate.HasValue ? PrivatizDate.Value.Year : 0;
            var minYear = FirstPrivYears.Min();
            return minYear > privatizYear ? 0m : 1m;
        }
    }
}