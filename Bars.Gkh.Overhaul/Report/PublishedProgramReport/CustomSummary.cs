using System.Linq;
using Bars.B4.Modules.Pivot.EventArgs;
using System.Collections.Generic;
using System.Data;

namespace Bars.Gkh.Overhaul.Report.PublishedProgramReport
{
    public class CustomSummary
    {
        public static int GetValue(PivotCustomSummaryEventArgs e)
        {
            switch (e.FieldName)
            {
                case "HouseCount":
                {
                    if (e.ColumnField == null && e.RowField != null)
                    {
                        var dictonary = new Dictionary<string, int>();
                        foreach (var year in e.GroupedRows.GroupBy(x => (string)x["Year"]).ToDictionary(x => x.Key))
                        {
                            var value = year.Value.Select(x => (string)x["Address"]).Distinct().Count();
                            dictonary.Add(year.Key, value);
                        }

                        return dictonary.Values.Sum();
                    }

                    if (e.ColumnField == null && e.RowField == null)
                    {
                        var dictonary = new Dictionary<string, int>();
                        foreach (var year in e.GroupedRows.GroupBy(x => (string)x["Year"]).ToDictionary(x => x.Key))
                        {
                            var value = year.Value.Select(x => (string) x["Address"]).Distinct().Count();
                            dictonary.Add(year.Key, value);
                        }

                        return dictonary.Values.Sum();
                    }

                    return GetCount(e.GroupedRows, "HouseCount");
                }
            }

            return 0;
        }

        private static int GetCount(IEnumerable<DataRow> data, string field)
        {
            return data.Where(x => (int)x[field] > 0).Select(x => (string)x["Address"]).Distinct().Count();
        }
    }
}
