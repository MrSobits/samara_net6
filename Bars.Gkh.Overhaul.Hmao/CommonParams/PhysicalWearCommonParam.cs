namespace Bars.Gkh.Overhaul.Hmao.CommonParams
{
    using B4.Utils;
    using Gkh.Entities;
    using Overhaul.CommonParams;
    using Overhaul.Enum;

    public class PhysicalWearCommonParam : ICommonParam
    {
        public static string Code
        {
            get { return "PhysicalWear"; }
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
            get { return "Физический износ"; }
        }

        public object GetValue(RealityObject obj)
        {
            return obj.PhysicalWear;
        }

        public bool Validate(RealityObject obj, object min, object max)
        {
            var maxDec = max.ToDecimal();
            var minDec = min.ToDecimal();
            var value = obj.PhysicalWear.ToDecimal();

            if (maxDec > 0)
            {
                return value >= minDec && value <= maxDec;
            }

            return value >= minDec;
        }
    }
}