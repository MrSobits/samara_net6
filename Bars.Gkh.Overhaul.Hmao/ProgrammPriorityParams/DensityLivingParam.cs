namespace Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams
{
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class DensityLivingParam : IProgrammPriorityParam
    {
        public static string Code
        {
            get { return "DensityLiving"; }
        }

        public bool Asc
        {
            get { return false; }
        }

        public string Name
        {
            get { return "Плотность населения"; }
        }

        string IProgrammPriorityParam.Code
        {
            get { return Code; }
        }

        public decimal Density { get; set; }

        public decimal GetValue(IStage3Entity stage3)
        {
            return Density;
        }
    }
}