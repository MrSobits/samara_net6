
namespace Bars.Gkh.Gis.CommonParams.Impl
{
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.Enum;
    using Gkh.Entities;

    public class NumberEntrancesCommonParam : IGisCommonParam
    {
        public static string Code
        {
            get
            {
                return TypeCommonParamsCodes.NumberEntrances.GetEnumMeta().Display;
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
                return Enum.CommonParamType.Integer;
            }
        }

        public string Name
        {
            get
            {
                return "Количество подъездов";
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
            return rObj.NumberEntrances;
        }
    }
}
