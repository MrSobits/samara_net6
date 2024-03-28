namespace Bars.Gkh.Overhaul.Tat.ProgrammPriorityParams
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.Entities;

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

        public long Id { get; set; }

        public Dictionary<long, int> Weights { get; set; } 

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

        public decimal GetValue(RealityObjectStructuralElementInProgrammStage3 stage3)
        {
            var id = (Id > 0 && stage3 == null) ? Id : stage3.Id;

            int result;

            if (Weights != null)
            {
                Weights.TryGetValue(id, out result);
            }
            else
            {
                result = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage2>>()
                    .GetAll()
                    .FirstOrDefault(x => x.Stage3.Id == id)
                    .Return(x => x.CommonEstateObject.Weight);
            }

            return result.ToDecimal();
        }
    }
}