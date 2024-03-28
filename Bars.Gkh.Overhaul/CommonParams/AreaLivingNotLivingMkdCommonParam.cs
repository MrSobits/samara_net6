namespace Bars.Gkh.Overhaul.CommonParams
{
    using B4.Utils;
    using Gkh.Entities;
    using Enum;

    public class AreaLivingNotLivingMkdCommonParam : ICommonParam
    {
        public static string Code
        {
            get { return "AreaLivingNotLivingMkd"; }
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
            get { return "Общая площадь жилых и нежилых помещений в МКД (кв.м.)"; }
        }

        public object GetValue(RealityObject obj)
        {
            return obj.AreaLivingNotLivingMkd;
        }

        public bool Validate(RealityObject obj, object min, object max)
        {
            var maxDec = max.ToDecimal();
            var minDec = min.ToDecimal();
            var value = obj.AreaLivingNotLivingMkd.ToDecimal();

            if (maxDec > 0)
            {
                return value >= minDec && value <= maxDec;
            }

            return value >= minDec;
        }
    }
}