namespace Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams
{
    using System.Collections.Generic;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.Windsor;

    public class PointsParam : IProgrammPriorityParam
    {
        public IWindsorContainer Container { get; set; }

        public Dictionary<long, decimal> DictPoints { get; set; }

        public static string Code
        {
            get { return "PointsParam"; }
        }

        public long Id { get; set; }

        public bool Asc
        {
            get { return false; }
        }

        public string Name
        {
            get { return "Баллы"; }
        }

        string IProgrammPriorityParam.Code
        {
            get { return Code; }
        }

        public decimal GetValue(IStage3Entity stage3)
        {
            return DictPoints.ContainsKey(stage3.Id) ? DictPoints[stage3.Id] : 0;
        }
    }
}