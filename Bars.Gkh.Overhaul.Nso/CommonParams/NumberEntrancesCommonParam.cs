namespace Bars.Gkh.Overhaul.Nso.CommonParams
{
    using B4.Utils;
    using Gkh.Entities;
    using Overhaul.CommonParams;
    using Overhaul.Enum;

    public class NumberEntrancesCommonParam : ICommonParam
    {
        public static string Code
        {
            get { return "NumberEntrances"; }
        }

        string ICommonParam.Code
        {
            get { return Code; }
        }

        public CommonParamType CommonParamType
        {
            get { return CommonParamType.Integer; }
        }

        public string Name
        {
            get { return "Количество подъездов"; }
        }

        public object GetValue(RealityObject rObj)
        {
            return rObj.NumberEntrances;
        }

        public bool Validate(RealityObject obj, object min, object max)
        {
            var maxInt = max.ToInt();
            var minInt = min.ToInt();
            var value = obj.NumberEntrances.ToInt();

            if (maxInt > 0)
            {
                return value >= minInt && value <= maxInt;
            }

            return value >= minInt;
        }
    }
}