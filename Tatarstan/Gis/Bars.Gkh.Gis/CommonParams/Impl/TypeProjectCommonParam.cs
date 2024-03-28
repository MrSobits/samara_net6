namespace Bars.Gkh.Gis.CommonParams.Impl
{
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.Enum;
    using Gkh.Entities;

    public class TypeProjectCommonParam : IGisCommonParam
    {
        public static string Code
        {
            get
            {
                return TypeCommonParamsCodes.TypeProject.GetEnumMeta().Display;
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
                return Enum.CommonParamType.TypeProject;
            }
        }

        public string Name
        {
            get
            {
                return "Серия, тип проекта";
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
            return rObj.TypeProject;
        }
    }
}
