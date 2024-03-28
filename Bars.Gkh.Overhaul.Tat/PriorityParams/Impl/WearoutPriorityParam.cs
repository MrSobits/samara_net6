namespace Bars.Gkh.Overhaul.Tat.PriorityParams.Impl
{
    using Entities;

    public class WearoutPriorityParam : IPriorityParams
    {
        public decimal Wearout { get; set; }

        public string Id
        {
            get { return "Wearout"; }
        }

        public string Name
        {
            get { return "Тех. состояние (износ) МКД (%)"; }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Quant; }
        }

        public object GetValue(RealityObjectStructuralElementInProgrammStage3 obj)
        {
            return Wearout;
        }
    }
}