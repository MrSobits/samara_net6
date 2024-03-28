namespace Bars.GkhCr.Report.ComparePrograms
{
    using System.Collections.Generic;
    using System.Linq;

    internal class RecordRealtyObject
    {
        public RecordRealtyObject()
        {
            GroupFinances = new Dictionary<string, RecordGroupFinance>();
            Total = new Grand();
            RecordOne = new RecordForProgram();
            RecordTwo = new RecordForProgram();
        }

        public RecordForProgram RecordOne { get; set; }

        public RecordForProgram RecordTwo { get; set; }

        public Dictionary<string, RecordGroupFinance> GroupFinances { get; set; }

        public Grand Total { get; set; }

        public Grand GetTotals()
        {
            foreach (var rec in GroupFinances)
            {
                Total.AddGrand(rec.Value.Total);
            }

            return Total;
        }

        public void Add(RecordRealtyObject record)
        {
            RecordOne.Add(record.RecordOne);
            RecordTwo.Add(record.RecordTwo);

            foreach (var rec in record.GroupFinances)
            {
                if (GroupFinances.ContainsKey(rec.Key))
                {
                    GroupFinances[rec.Key].Add(rec.Value.Clone());
                }
                else
                {
                    GroupFinances.Add(rec.Key, rec.Value.Clone());
                }
            }
        }

        public RecordRealtyObject Clone()
        {
            return new RecordRealtyObject
                       {
                           GroupFinances = GroupFinances.ToDictionary(x => x.Key, x => x.Value.Clone())
                       };
        }
    }
}
