namespace Bars.Gkh.Overhaul.CommonParams
{
    using B4.Utils;
    using Gkh.Entities;
    using Enum;

    public class NumberLiftsCommonParam : ICommonParam
    {
        public static string Code
        {
            get { return "NumberLifts"; }
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
            get { return "Количество лифтов"; }
        }

        public object GetValue(RealityObject rObj)
        {
            return rObj.NumberLifts;
        }

        public bool Validate(RealityObject obj, object min, object max)
        {
            var maxInt = max.ToInt();
            var minInt = min.ToInt();
            var value = obj.NumberLifts.ToInt();

            if (maxInt > 0)
            {
                return value >= minInt && value <= maxInt;
            }

            return value >= minInt;
        }
    }
}