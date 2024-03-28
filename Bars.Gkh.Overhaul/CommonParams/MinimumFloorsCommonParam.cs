namespace Bars.Gkh.Overhaul.CommonParams
{
    using B4.Utils;
    using Enum;
    using Gkh.Entities;

    public class MinimumFloorsCommonParam : ICommonParam
    {
        public static string Code
        {
            get { return "Floors"; }
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
            get { return "Минимальная этажность"; }
        }

        public object GetValue(RealityObject rObj)
        {
            return rObj.Floors;
        }

        public bool Validate(RealityObject obj, object min, object max)
        {
            var maxInt = max.ToInt();
            var minInt = min.ToInt();
            var value = obj.Floors.ToInt();

            if (maxInt > 0)
            {
                return value >= minInt && value <= maxInt;
            }

            return value >= minInt;
        }
    }
}