namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class RoPercentDebtPriorityParam : IPriorityParams
    {
        public string Id
        {
            get { return "RoPercentDebt"; }
        }

        public string Name
        {
            get { return "Собираемость взносов на КР(%)"; }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Quant; }
        }

        public  object GetValue(IStage3Entity obj)
        {
            return obj.RealityObject.PercentDebt;
        }
    }
}