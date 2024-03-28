namespace Bars.Gkh.Gis.CommonParams.Impl
{
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.Enum;
    using Gkh.Entities;

    public class WallMaterialCommonParam : IGisCommonParam
    {
        public static string Code
        {
            get
            {
                return TypeCommonParamsCodes.WallMaterial.GetEnumMeta().Display;
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
                return Enum.CommonParamType.WallMaterial;
            }
        }

        public string Name
        {
            get
            {
                return "Материал стен";
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
            return rObj.WallMaterial;
        }
    }
}
