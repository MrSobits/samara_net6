namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Modules.Reports;
    using B4.Utils;

    public class TatFormForMoscow3 : GkhCr.Report.FormForMoscow3
    {
        public TatFormForMoscow3()
            : base(new ReportTemplateBinary(Properties.Resources.TatFormForMoscow3))
        {
        }

        public override string Name
        {
            get
            {
                return "Форма для Москвы 3 (Татарстан)";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Форма для Москвы 3 (Татарстан)";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.FormForMoscow3Tat";
            }
        }

        protected override Dictionary<string, string> GetWorkGroups()
        {
            var workGroups = Enumerable.Range(1, 19).Select(x => x.ToStr()).ToDictionary(x => x, x => x);

            workGroups["15"] = "14";
            workGroups["17"] = "16";
            workGroups["21"] = "21";
            workGroups["29"] = "29";
            workGroups["30"] = "30";
            workGroups["1018"] = "1018";
            workGroups["1019"] = "1018";

            return workGroups;
        }

        protected override decimal FillSpecificFields(Section section, Dictionary<string, TypeWorkProxy> workDict, Dictionary<string, decimal> sumDict)
        {
            var workType29 = workDict.FirstOrDefault(x => x.Key == "29");
            
            var volume29 = workType29.Value != null ? workType29.Value.volume : 0;
            var costSum29 = workType29.Value != null ? workType29.Value.costSum : 0;

            Action<string, decimal?> addToSum = (x, y) =>
            {
                if (y.HasValue && y.Value != 0)
                {
                    section["col" + x] = y;
                    this.AddToSumAndWriteToReport(sumDict, x, y.Value);
                }
            };

            addToSum("59", volume29);
            addToSum("60", costSum29);

            Func<string, decimal> costSum = key => workDict.ContainsKey(key) ? workDict[key].costSum : 0;

            var costSum7 = costSum("7");
            var costSum8 = costSum("8");
            var costSum9 = costSum("9");
            var costSum10 = costSum("10");
            var costSum11 = costSum("11");

            var costSum48 = costSum7 + costSum8 + costSum9 + costSum10 + costSum11 + costSum29;

            section["costSum48"] = AsString(costSum48);


            var complex69 = ((costSum7 < 100000 || costSum8 < 100000 ? 1 : 0) * costSum9 * costSum10 * costSum11) == 0
                                ? "Нет"
                                : "Да";

            section["complex69"] = complex69;

            return costSum48;
        }
    }
}