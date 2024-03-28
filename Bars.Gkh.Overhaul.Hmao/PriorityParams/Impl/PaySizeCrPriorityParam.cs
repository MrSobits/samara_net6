namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class PaySizeCrPriorityParam : IPriorityParams
    {

        private Dictionary<long, decimal> _roPaySizes;

        public string Id
        {
            get
            {
                return "PaySizeCr";
            }
        }

        public string Name
        {
            get
            {
                return "Размер взноса на КР";
            }
        }

        public IRealityObjectTariffService RoTariffService { get; set; }

        public TypeParam TypeParam
        {
            get
            {
                return TypeParam.Quant;
            }
        }

        public object GetValue(IStage3Entity obj)
        {
            if (_roPaySizes == null)
            {
                if (RoTariffService != null)
                {
                    _roPaySizes = RoTariffService.FillRealityObjectTariffDictionary();
                }
                else
                {
                    _roPaySizes = new Dictionary<long, decimal>();
                }
            }

            return _roPaySizes.Get(obj.RealityObject.Id).Return(x => x);
        }
    }
}