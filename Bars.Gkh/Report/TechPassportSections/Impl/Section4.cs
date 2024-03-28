namespace Bars.Gkh.Report.TechPassportSections
{
    using System.Collections.Generic;

    using System.Linq;

    public class Section4 : BaseTechPassportSectionReport
    {
        protected override void PrepareComponentIds()
        {
            //---4.1. Общие сведения---
            GenerateCellCodes("Form_4_1", 1, 4, 4, 4);
            GenerateCellCodes("Form_4_1_1", 1, 7, 3, 3);

            //---4.2. Общие сведения о лифтах---
            GenerateCellCodes("Form_4_2", 1, 2);
            CellCodesByComponentCodes["Form_4_2_1"] = new List<string>();
        }

        protected override void AfterPlaceData()
        {
            if (!this.TechPassportValues.ContainsKey("Form_4_2_1"))
            {
                return;
            }

            var section = this.ReportParams.ComplexReportParams.ДобавитьСекцию("sectionForm_4_2_1");

            var dataDict =
                this.TechPassportValues["Form_4_2_1"]
                    .Select(x =>
                            {
                                var components = x.Key.Split(':');
                                var row = components.Length > 0 ? components.First() : "0";
                                var col = components.Length > 1 ? components[1] : "0";
                                return new { row, col, x.Value };
                            })
                    .GroupBy(x => x.row)
                    .ToDictionary(
                        x => x.Key, x => x.ToDictionary(y => y.col, y => y.Value));

            foreach (var row in dataDict)
            {
                section.ДобавитьСтроку();

                foreach (var val in row.Value)
                {
                    section[string.Format("Column{0}", val.Key)] = val.Value;
                }
            }
        }
    }
}