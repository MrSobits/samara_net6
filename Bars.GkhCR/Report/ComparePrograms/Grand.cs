namespace Bars.GkhCr.Report.ComparePrograms
{
    using System.Collections.Generic;

    internal class Grand
    {
        public Grand()
        {
            Works = new Dictionary<int, RecordWorks>();
            Budget = new Budget();
        }

        public Dictionary<int, RecordWorks> Works { get; set; }

        public decimal Psd { get; set; }

        public decimal TehInspection { get; set; }

        public Budget Budget { get; set; }

        public void AddGrandWork(KeyValuePair<int, RecordWorks> rec)
        {
            AddGrandWork(rec.Key, rec.Value);
        }

        public void AddGrandWork(int code, RecordWorks rec)
        {
            if (!this.Works.ContainsKey(code))
            {
                this.Works.Add(code, rec);
            }
            else
            {
                this.Works[code] += rec;
            }
        }

        public void AddGrandsBudget(Budget budget)
        {
            Budget.Add(budget);
        }

        public void AddGrand(Grand grand)
        {
            foreach (var rec in grand.Works)
            {
                AddGrandWork(rec);
            }

            Psd += grand.Psd;
            TehInspection += grand.TehInspection;

            Budget.Add(grand.Budget);
        }
    }
}
