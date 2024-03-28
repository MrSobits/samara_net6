namespace Bars.Gkh.Overhaul.CommonParams
{
    using System;
    using B4.Utils;
    using Gkh.Entities;
    using Enum;

    public class FirstPrivatizationDateCommonParam: ICommonParam
    {
        public static string Code
        {
            get { return "PrivatizationDateFirstApartment"; }
        }

        string ICommonParam.Code
        {
            get { return Code; }
        }

        public CommonParamType CommonParamType
        {
            get { return CommonParamType.Date; }
        }

        public string Name
        {
            get { return "Дата приватизации первого жилого помещения"; }
        }

        public object GetValue(RealityObject obj)
        {
            return obj.PrivatizationDateFirstApartment;
        }

        public bool Validate(RealityObject obj, object min, object max)
        {
            var maxDate = max.ToDateTime();
            var minDate = min.ToDateTime();
            var value = obj.PrivatizationDateFirstApartment.ToDateTime();

            if (maxDate > DateTime.MinValue)
            {
                return value >= minDate && value <= maxDate;
            }

            return value >= minDate;
        }
    }
}