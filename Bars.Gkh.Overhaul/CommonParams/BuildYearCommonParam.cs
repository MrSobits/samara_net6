namespace Bars.Gkh.Overhaul.CommonParams
{
    using B4.Utils;
    using Gkh.Entities;
    using Enum;

    public class BuildYearCommonParam: ICommonParam
    {
        public static string Code
        {
            get { return "BuildYear"; }
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
            get { return "Год постройки"; }
        }

        public object GetValue(RealityObject rObj)
        {
            return rObj.BuildYear;
        }

        public bool Validate(RealityObject obj, object min, object max)
        {
            var maxInt = max.ToInt();
            var minInt = min.ToInt();
            var value = obj.BuildYear.ToInt();

            if (maxInt > 0)
            {
                return value >= minInt && value <= maxInt;
            }

            return value >= minInt;
        }
    }
}