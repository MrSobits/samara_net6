namespace Bars.Gkh.Gis.CommonParams.Impl
{
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.Enum;
    using Gkh.Entities;

    public class PrivatizationDateFirstApartment: IGisCommonParam
    {
        public static string Code
        {
            get
            {
                return TypeCommonParamsCodes.PrivatizationDateFirstApartmentGis.GetEnumMeta().Display;
            }
        }

        string IGisCommonParam.Code
        {
            get { return Code; }
        }

        public Gis.Enum.CommonParamType CommonParamType
        {
            get
            {
                return Gis.Enum.CommonParamType.Date;
            }
        }

        public string Name
        {
            get
            {
                return "Дата приватизации первого жилого помещения";
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
            return rObj.PrivatizationDateFirstApartment;
        }
    }
}
