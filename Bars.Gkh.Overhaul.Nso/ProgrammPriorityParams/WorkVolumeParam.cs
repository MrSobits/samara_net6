namespace Bars.Gkh.Overhaul.Nso.ProgrammPriorityParams
{
    using System.Collections.Generic;
    using System.Linq;

    using Entities;

    using Castle.Windsor;

    public class WorkVolumeParam : IProgrammPriorityParam
    {
        public static string Code
        {
            get
            {
                return "WorkVolume";
            }
        }

        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Веса ООИ
        /// </summary>
        public IEnumerable<int> Weights { get; set; }

        public bool Asc { get { return false; } }

        public string Name
        {
            get
            {
                return "Значимость кап.ремонта конструктивных элементов в плановом году";
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
            return Weights.Sum();
        }
    }
}