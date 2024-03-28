namespace Bars.GkhCr.Report.PlannedAllocationOfWorks
{
    using System.Collections.Generic;

    internal sealed class Record
    {
        public Record(string municipal, decimal? smr)
        {
            Municipal = municipal;
            Smr = smr.HasValue ? smr.Value : 0M;

            SumTotal = decimal.Zero;
            PercentTotal = decimal.Zero;

            SumDict = new Dictionary<int, decimal>();
            PercentDict = new Dictionary<int, decimal>();
            CumulativeSum = new Dictionary<int, decimal>();
            CumulativePercent = new Dictionary<int, decimal>();

            for (var month = 1; month <= 13; month++)
            {
                SumDict.Add(month, decimal.Zero);
                PercentDict.Add(month, decimal.Zero);

                if (month < 13)
                {
                    CumulativeSum.Add(month, decimal.Zero);
                    CumulativePercent.Add(month, decimal.Zero);
                }
            }
        }

        public string Municipal { get; set; }

        public decimal Smr { get; set; }

        public decimal SumTotal { get; set; }

        public decimal PercentTotal { get; set; }

        public Dictionary<int, decimal> SumDict { get; set; }

        public Dictionary<int, decimal> PercentDict { get; set; }

        /// <summary>
        /// Нарастающая сумма
        /// </summary>
        public Dictionary<int, decimal> CumulativeSum { get; set; }

        /// <summary>
        /// Нарастающий процент
        /// </summary>
        public Dictionary<int, decimal> CumulativePercent { get; set; }

        public void AddSum(int month, decimal sum)
        {
            if (month > 0 && month <= 13)
            {
                SumDict[month] += sum;
            }
        }

        /// <summary>
        /// Подсчитывает нарастающий итог по суммам и процентам, кроме 13-го месяца(без графиков)
        /// </summary>
        public void ProcessData()
        {
            var divider = Smr == decimal.Zero ? 1 : Smr;
            foreach (var val in SumDict)
            {
                PercentDict[val.Key] = decimal.Divide(val.Value, divider);
                if (val.Key >= 13)
                {
                    continue;
                }

                this.SumTotal += val.Value;
                this.PercentTotal += this.PercentDict[val.Key];
            }

            CumulativeSum[1] = SumDict[1];
            CumulativePercent[1] = PercentDict[1];

            for (var month = 2; month < 13; month++)
            {
                CumulativeSum[month] += CumulativeSum[month - 1] + SumDict[month];
                CumulativePercent[month] += CumulativePercent[month - 1] + PercentDict[month];
            }
        }
    }
}