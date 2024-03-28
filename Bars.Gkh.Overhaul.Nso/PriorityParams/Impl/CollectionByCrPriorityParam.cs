namespace Bars.Gkh.Overhaul.Nso.PriorityParams.Impl
{
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class CollectionByCrPriorityParam : IPriorityParams
    {

        private Dictionary<long, decimal> _roCollections;

        public string Id
        {
            get
            {
                return "CollectionByCr";
            }
        }

        public string Name
        {
            get
            {
                return "Собираемость по КР (%, за 12 месяцев)";
            }
        }

        public IRealityObjectCollectionService RoCollectionService { get; set; }

        public TypeParam TypeParam
        {
            get
            {
                return TypeParam.Quant;
            }
        }

        public object GetValue(IStage3Entity obj)
        {
            if (_roCollections == null)
            {
                if (RoCollectionService != null)
                {
                    _roCollections = RoCollectionService.FillRealityObjectCollectionDictionary();
                }
                else
                {
                    _roCollections = new Dictionary<long, decimal>();
                }
            }

            return _roCollections.Get(obj.RealityObject.Id).Return(x => x);
        }
    }
}