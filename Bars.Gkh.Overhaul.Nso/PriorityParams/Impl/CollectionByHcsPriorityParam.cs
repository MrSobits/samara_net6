namespace Bars.Gkh.Overhaul.Nso.PriorityParams.Impl
{
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class CollectionByHcsPriorityParam : IPriorityParams
    {
        public string Id
        {
            get
            {
                return "CollectionByHcs";
            }
        }

        public string Name
        {
            get
            {
                return "Собираемость по ЖКУ (%)";
            }
        }

        public TypeParam TypeParam
        {
            get
            {
                return TypeParam.Quant;
            }
        }

        public object GetValue(IStage3Entity obj)
        {
            return obj.Return(x => x.RealityObject).Return(x => x.PercentDebt, 0)/100;
        }
    }
}