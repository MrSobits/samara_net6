namespace Bars.Gkh.Gis.CommonParams.Impl
{
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.Enum;
    using Gkh.Entities;

    public class PhysicalWearCommonParam : IGisCommonParam
    {
        public static string Code
        {
            get
            {
                return TypeCommonParamsCodes.PhysicalWear.GetEnumMeta().Display;
            }
        }

        string IGisCommonParam.Code
        {
            get { return Code; }
        }

        public Enum.CommonParamType CommonParamType
        {
            get
            {
                return Enum.CommonParamType.Decimal;
            }
        }

        public string Name
        {
            get
            {
                return "Физический износ";
            }
        }

        public bool IsPrecision
        {
            get
            {
                return false;
            }
        }

        public object GetValue(RealityObject rObj)
        {
            return rObj.PhysicalWear;
        }
    }
}
