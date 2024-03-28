namespace Bars.Gkh.Gis.CommonParams.Impl
{
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.Enum;
    using Gkh.Entities;

    public class TypeHouseCommonParam : IGisCommonParam
    {
        public static string Code
        {
            get
            {
                return TypeCommonParamsCodes.TypeHouse.GetEnumMeta().Display;
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
                return Enum.CommonParamType.TypeHouse;
            }
        }

        public string Name
        {
            get
            {
                return "Вид дома";
            }
        }

        public bool IsPrecision
        {
            get
            {
                return true;
            }
        }

        public object GetValue(RealityObject rObj)
        {
            return rObj.TypeHouse;
        }
    }
}
