namespace Bars.Gkh.Overhaul.Nso.ProgrammPriorityParams
{
    using System.Collections.Generic;
    using B4.Utils;
    using Entities;

    using Castle.Windsor;

    public class PointsParam : IProgrammPriorityParam
    {
        public IWindsorContainer Container { get; set; }

        public Dictionary<long, decimal> DictPoints { get; set; }

        public static string Code
        {
            get
            {
                return "PointsParam";
            }
        }

        public bool Asc
        {
            get
            {
                return false; // по убыванию поинтов
            }
        }

        public string Name
        {
            get
            {
                return "Баллы";
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
            return DictPoints.Get(stage3.Id);
        }
    }
}