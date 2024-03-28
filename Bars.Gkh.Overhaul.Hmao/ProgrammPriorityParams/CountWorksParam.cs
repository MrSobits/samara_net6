namespace Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams
{
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class CountWorksParam : IProgrammPriorityParam
    {
        public static string Code 
        {
            get { return "CountWorksParam"; }
        }

        public bool Asc 
        {
            get { return false; }
        }

        public string Name 
        {
            get { return "Количество работ в рамках комплексного ремонта"; }
        }

        string IProgrammPriorityParam.Code
        {
            get { return Code; }
        }

        public int CountWorks { get; set; }

        public decimal GetValue(IStage3Entity stage3)
        {
            return CountWorks;
        }
    }
}