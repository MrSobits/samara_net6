using System;

namespace Bars.Gkh.Qa.Steps.CommonSteps
{
    using System.Globalization;

    using TechTalk.SpecFlow;

    public static class ParseHelper
    {
        public static DateTime? DateParse(this string dateString, bool isNullable = false)
        {
            DateTime? result;

            switch (dateString)
            {
                case "текущая дата":
                    {
                        result = DateTime.Today;
                        break;
                    }
                case "":
                    {
                        if (!isNullable)
                        {
                            throw new SpecFlowException("Дата не может быть пустой");
                        }

                        result = null;
                        break;
                    }
                default:
                    {
                        DateTime date;

                        if (!DateTime.TryParse(dateString, out date))
                        {
                            throw new SpecFlowException("Не правильный формат даты");
                        }

                        result = date;

                        break;
                    }
            }

            return result;
        }

        public static Decimal DecimalParse(this string decimalString)
        {
            decimal result;

            decimalString = decimalString.Replace(
                ".",
                CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            decimalString = decimalString.Replace(
                ",",
                CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            if(!Decimal.TryParse(decimalString,out result))
            {
                throw new SpecFlowException("Не правильный формат действитльного числа");
            }

            return result;
        }

    }
}
