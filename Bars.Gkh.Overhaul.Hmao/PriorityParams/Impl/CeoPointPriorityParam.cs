namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using System;
    using System.Collections.Generic;
    using Entities;
    using Gkh.Entities.CommonEstateObject;
    using Overhaul.Entities;

    public class CeoPointPriorityParam : IPriorityParams, IMultiPriorityParam
    {
        public string Id
        {
            get { return "CeoPointPriorityParam"; }
        }

        public Type Type
        {
            get
            {
                return typeof (CommonEstateObject);
            }
        }

        public string Name
        {
            get { return "Значимость кап.ремонта ООИ"; }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Multi; }
        }

        public Dictionary<long, HashSet<long>> Stage3CeoDict { get; set; }

        public  object GetValue(IStage3Entity obj)
        {
            return null;
        }

        public bool CheckContains(IStage3Entity obj, IEnumerable<StoredMultiValue> value)
        {
            //запиливалось по таску [34348]
            //по описанию проверка будет на 1 оои, но если в дальнейшем будет нужда в наличии нескольких оои
            //здесь ничего не придется менять
            if (!Stage3CeoDict.ContainsKey(obj.Id) || value == null)
            {
                return false;
            }

            var roCeos = Stage3CeoDict[obj.Id];

            foreach (var ceo in value)
            {
                if (!roCeos.Contains(ceo.Id))
                {
                    return false;
                }
            }

            return true;
        }
    }
}