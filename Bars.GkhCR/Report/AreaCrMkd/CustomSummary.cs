namespace Bars.GkhCr.Report.AreaCrMkd
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4.Modules.Pivot.EventArgs;
    using Bars.B4.Utils;

    public class CustomSummary
    {
        private const string Caption = "Municipality";

        private static decimal GetCustomSum(PivotCustomSummaryEventArgs e, string fieldName)
        {
            var sum = e.GroupedRows
                        .Select(x => new
                        {
                            Address = x["Address"].ToStr(),
                            field = x[fieldName].ToDecimal()
                        })
                    .GroupBy(x => x.Address)
                    .Sum(x => x.Max(y => y.field));
            return sum;
        }

        public static decimal GetValue(PivotCustomSummaryEventArgs e)
        {
            switch (e.FieldName)
            {
                case "CountHouses":
                    return e.GroupedRows.Select(x => x["Address"].ToStr()).Distinct().Count();

                default:
                //case "TotalAreaMkd":
                    // вычисляем итоги == итогиПоМо || ОбщиеИтоги
                    if ((e.ColumnField == null && e.RowField != null && e.RowField == Caption) || (e.ColumnField == null && e.RowField == null))
                    {
                        var dictonary = new Dictionary<string, int>();
                        foreach (var address in e.GroupedRows.GroupBy(x => (string)x["Address"]).ToDictionary(x => x.Key))
                        {
                            var values = address.Value.Select(x => Math.Round(x[e.FieldName].ToDecimal(), 0)).Max();
                            dictonary.Add(address.Key, values.ToInt());
                        }

                        return dictonary.Values.Sum();
                    }

                    break;
            }

            return GetCustomSum(e, e.FieldName);
        }
    }
}
