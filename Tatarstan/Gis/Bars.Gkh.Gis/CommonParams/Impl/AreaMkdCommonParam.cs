namespace Bars.Gkh.Gis.CommonParams.Impl
{
    using Gkh.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.Enum;

    public class AreaMkdCommonParam : IGisCommonParam
    {
        public static string Code
        {
            get
            {
                return TypeCommonParamsCodes.AreaMkdGis.GetEnumMeta().Display;                
            }
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
                return "Общая площадь МКД (кв.м.)";
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
            return rObj.AreaMkd;
        }

        string IGisCommonParam.Code
        {
            get { return Code; }
        }
    }
}
