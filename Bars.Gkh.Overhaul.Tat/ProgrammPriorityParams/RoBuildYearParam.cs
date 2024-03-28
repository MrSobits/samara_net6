namespace Bars.Gkh.Overhaul.Tat.ProgrammPriorityParams
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;

    using Castle.Windsor;

    public class RoBuildYearParam : IProgrammPriorityParam
    {
        public static string Code
        {
            get
            {
                return "RoBuildYearParam";
            }
        }

        public IWindsorContainer Container { get; set; }

        public RealityObject RealityObject { get; set; }

        public bool Asc { get { return true; } }

        public string Name
        {
            get
            {
                return "Год постройки";
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
            var ro = RealityObject ?? stage3.RealityObject;
            var val = ro.BuildYear;

            //если есть год постройки то возвращаем его, иначе 0
            return val.HasValue ? val.Value : 0;
        }
    }
}