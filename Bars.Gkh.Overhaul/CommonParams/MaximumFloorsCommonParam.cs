namespace Bars.Gkh.Overhaul.CommonParams
{
    using B4.Utils;
    using Enum;
    using Gkh.Entities;

    public class MaximumFloorsCommonParam : ICommonParam
    {
        public static string Code
        {
            get { return "MaximumFloors"; }
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
            get
            {
                return "Максимальная этажность";
            }
        }

        public object GetValue(RealityObject obj)
        {
            return obj.MaximumFloors;
        }

        public bool Validate(RealityObject obj, object min, object max)
        {
            var maxInt = max.ToInt();
            var minInt = min.ToInt();
            var value = obj.MaximumFloors.ToInt();

            if (maxInt > 0)
            {
                return value >= minInt && value <= maxInt;
            }

            return value >= minInt;
        }
    }
}