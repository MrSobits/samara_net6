namespace Bars.Gkh.Gis.CommonParams.Impl
{
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.Enum;
    using Gkh.Entities;

    public class TypeRoofCommonParam : IGisCommonParam
    {
        public static string Code
        {
            get
            {
                return TypeCommonParamsCodes.TypeRoof.GetEnumMeta().Display;
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
                return Enum.CommonParamType.TypeRoof;
            }
        }

        public string Name
        {
            get
            {
                return "Тип кровли";
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
            return rObj.TypeRoof;
        }
    }
}
