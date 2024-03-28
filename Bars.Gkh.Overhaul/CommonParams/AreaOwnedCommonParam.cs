namespace Bars.Gkh.Overhaul.CommonParams
{
    using B4.Utils;
    using Gkh.Entities;
    using Enum;

    public class AreaOwnedCommonParam : ICommonParam
    {
        public static string Code
        {
            get { return "AreaOwned"; }
        }

        string ICommonParam.Code
        {
            get { return Code; }
        }

        public CommonParamType CommonParamType
        {
            get { return CommonParamType.Decimal; }
        }

        public string Name
        {
            get { return "Площадь частной собственности (кв.м.)"; }
        }

        public object GetValue(RealityObject obj)
        {
            return obj.AreaOwned;
        }

        public bool Validate(RealityObject obj, object min, object max)
        {
            var maxDec = max.ToDecimal();
            var minDec = min.ToDecimal();
            var value = obj.AreaOwned.ToDecimal();

            if (maxDec > 0)
            {
                return value >= minDec && value <= maxDec;
            }

            return value >= minDec;
        }
    }
}