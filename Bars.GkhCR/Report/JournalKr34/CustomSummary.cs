namespace Bars.GkhCr.Report.JournalKr34
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4.Modules.Pivot.EventArgs;

    public class CustomSummary
    {
        private const string Caption = "Municipality";

        public static int GetValue(PivotCustomSummaryEventArgs e, int typeWork)
        {
            switch (e.FieldName)
            {
                // По программе домов
                case "InProgHouse":
                    return e.GroupedRows.Select(x => (string)x["Address"]).Distinct().Count();

                // Д.б. по плану домов
                case "MustPlanHouse":

                    // вычисляем итог по строке != итогиПоМо
                    if (e.ColumnField == null && e.RowField != null && e.RowField != Caption)
                    {
                        return typeWork == 0 ? GetCount(e.GroupedRows, "MustPlanHouse") : e.GroupedRows.Select(x => (int)x["MustPlanHouse"]).Min();
                    }

                    // вычисляем итоги == итогиПоМо || ОбщиеИтоги
                    if ((e.ColumnField == null && e.RowField != null && e.RowField == Caption) || (e.ColumnField == null && e.RowField == null))
                    {
                        var maximum = typeWork == 0;
                        var dictonary = new Dictionary<string, int>();
                        foreach (var address in e.GroupedRows.GroupBy(x => (string)x["Address"]).ToDictionary(x => x.Key))
                        {
                            var values = address.Value.Select(x => (int)x["MustPlanHouse"]);
                            dictonary.Add(address.Key, maximum ? values.Max() : values.Min());
                        }

                        return dictonary.Values.Sum();
                    }

                    // вычисляем значения ячеек и итоги по столбцу
                    return GetCount(e.GroupedRows, "MustPlanHouse");

                // Д.б. и факт домов
                case "MustFactHouse":

                    if (e.ColumnField == null && e.RowField != null && e.RowField != Caption)
                    {
                        return typeWork == 0 ? GetCount(e.GroupedRows, "MustFactHouse") : e.GroupedRows.Select(x => (int)x["MustFactHouse"]).Min();
                    }

                    // вычисляем итоги == итогиПоМо || ОбщиеИтоги
                    if ((e.ColumnField == null && e.RowField != null && e.RowField == Caption) || (e.ColumnField == null && e.RowField == null))
                    {
                        var maximum = typeWork == 0;
                        var dictonary = new Dictionary<string, int>();
                        foreach (var address in e.GroupedRows.GroupBy(x => (string)x["Address"]).ToDictionary(x => x.Key))
                        {
                            var values = address.Value.Select(x => (int)x["MustFactHouse"]);
                            dictonary.Add(address.Key, maximum ? values.Max() : values.Min());
                        }

                        return dictonary.Values.Sum();
                    }

                    return GetCount(e.GroupedRows, "MustFactHouse");

                case "NotMustFactHouse":
                    // вычисляем итог по строке != итогиПоМо
                    if (e.ColumnField == null && e.RowField != null && e.RowField != Caption)
                    {
                        return e.GroupedRows.Select(x => (int)x["NotMustFactHouse"]).Min();
                    }

                    // вычисляем итоги == итогиПоМо || ОбщиеИтоги
                    if ((e.ColumnField == null && e.RowField != null && e.RowField == Caption) || (e.ColumnField == null && e.RowField == null))
                    {
                        var maximum = typeWork == 0;
                        var dictonary = new Dictionary<string, int>();
                        foreach (var address in e.GroupedRows.GroupBy(x => (string)x["Address"]).ToDictionary(x => x.Key))
                        {
                            var values = address.Value.Select(x => (int)x["NotMustFactHouse"]);
                            dictonary.Add(address.Key, maximum ? values.Max() : values.Min());
                        }

                        return dictonary.Values.Sum();
                    }

                    return GetCount(e.GroupedRows, "NotMustFactHouse");

                case "MustNotFactHouse":
                    return GetCount(e.GroupedRows, "MustNotFactHouse");

                case "NotMustNotFactHouse":

                    if (e.ColumnField == null && e.RowField != null && e.RowField != Caption)
                    {
                        return typeWork == 0
                                   ? GetCount(e.GroupedRows, "NotMustNotFactHouse")
                                   : e.GroupedRows.Select(x => (int)x["NotMustNotFactHouse"]).Min();
                    }

                    if ((e.ColumnField == null && e.RowField != null && e.RowField == Caption) || (e.ColumnField == null && e.RowField == null))
                    {
                        var maximum = typeWork == 1;
                        var dictonary = new Dictionary<string, int>();
                        foreach (var address in e.GroupedRows.GroupBy(x => (string)x["Address"]).ToDictionary(x => x.Key))
                        {
                            var values = address.Value.Select(x => (int)x["NotMustNotFactHouse"]);
                            dictonary.Add(address.Key, maximum ? values.Max() : values.Min());
                        }

                        return dictonary.Values.Sum();
                    }

                    return GetCount(e.GroupedRows, "NotMustNotFactHouse");

                case "FactHouse":
                    if (e.ColumnField == null && e.RowField != null && e.RowField != Caption)
                    {
                        return typeWork == 0
                                         ? GetCount(e.GroupedRows, "FactHouse")
                                         : e.GroupedRows.Select(x => (int)x["FactHouse"]).Min();
                    }

                    if ((e.ColumnField == null && e.RowField != null && e.RowField == Caption) || (e.ColumnField == null && e.RowField == null))
                    {
                        var maximum = typeWork == 0;
                        var dictonary = new Dictionary<string, int>();
                        foreach (var address in e.GroupedRows.GroupBy(x => (string)x["Address"]).ToDictionary(x => x.Key))
                        {
                            var values = address.Value.Select(x => (int)x["FactHouse"]);
                            dictonary.Add(address.Key, maximum ? values.Max() : values.Min());
                        }

                        return dictonary.Values.Sum();
                    }

                    return GetCount(e.GroupedRows, "FactHouse");
            }

            return 0;
        }

        private static int GetCount(IEnumerable<DataRow> data, string field)
        {
            return data.Where(x => (int)x[field] > 0).Select(x => (string)x["Address"]).Distinct().Count();
        }
    }
}
