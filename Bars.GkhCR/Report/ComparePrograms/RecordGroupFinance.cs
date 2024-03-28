namespace Bars.GkhCr.Report.ComparePrograms
{
    using System.Collections.Generic;
    using System.Linq;

    internal class RecordGroupFinance
    {
        public RecordGroupFinance(string groupSource)
        {
            GroupSource = groupSource;
            Sources = new Dictionary<string, RecordForSource>();
            Total = new Grand();
        }

        public string GroupSource { get; set; }

        public Dictionary<string, RecordForSource> Sources { get; set; }

        /// <summary>
        /// Итоги
        /// </summary>
        public Grand Total { get; set; }

        public void Add(RecordGroupFinance record)
        {
            foreach (var rec in record.Sources)
            {
                if (Sources.ContainsKey(rec.Key))
                {
                    Sources[rec.Key].Add(rec.Value.Clone());
                }
                else
                {
                    Sources.Add(rec.Key, rec.Value.Clone());
                }
            }
        }

        public RecordGroupFinance Clone()
        {
            return new RecordGroupFinance(GroupSource)
                       {
                           Sources = Sources.ToDictionary(x => x.Key, x => x.Value.Clone())
                       };
        }
    }
}
