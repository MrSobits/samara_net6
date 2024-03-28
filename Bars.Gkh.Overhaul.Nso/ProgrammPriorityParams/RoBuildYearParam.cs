namespace Bars.Gkh.Overhaul.Nso.ProgrammPriorityParams
{
    using Entities;

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

        public int? BuildYear { get; set; }

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

        public decimal GetValue(IStage3Entity stage3)
        {
            return BuildYear.HasValue ? BuildYear.Value : 0;
        }
    }
}