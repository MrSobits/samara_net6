namespace Bars.GkhCr.Report.ComparePrograms
{
    using System.Collections.Generic;
    using System.Linq;

    internal class RecordForSource
    {
        public RecordForSource(string source)
        {
            WorksOne = new Dictionary<int, RecordWorks>();
            WorksTwo = new Dictionary<int, RecordWorks>();
            BudgetOne = new Budget();
            BudgetTwo = new Budget();
            Source = source;
        }

        public Budget BudgetOne { get; set; }

        public Budget BudgetTwo { get; set; }

        public string Source { get; set; }

        public Dictionary<int, RecordWorks> WorksOne { get; set; }

        public Dictionary<int, RecordWorks> WorksTwo { get; set; }

        public decimal SumWorksOne
        {
            get { return WorksOne.Sum(x => x.Value.Sum); }
        }

        public decimal SumWorksTwo
        {
            get { return WorksTwo.Sum(x => x.Value.Sum); }
        }

        /// <summary>
        /// Получить разницу по средствам
        /// </summary>
        /// <returns></returns>
        public Budget GetDifferenceForFunds()
        {
            return new Budget(BudgetTwo.BudgetRf - BudgetOne.BudgetRf, BudgetTwo.BudgetRt - BudgetOne.BudgetRt, BudgetTwo.BudgetMu - BudgetOne.BudgetMu, BudgetTwo.BudgetOwners - BudgetOne.BudgetOwners);
        }

        public RecordForSource Clone()
        {
            return new RecordForSource(Source)
                                              {
                                                  WorksOne = WorksOne.ToDictionary(x => x.Key, x => x.Value.Clone()), 
                                                  WorksTwo = WorksTwo.ToDictionary(x => x.Key, x => x.Value.Clone()),
                                                  BudgetOne = BudgetOne.Clone(),
                                                  BudgetTwo = BudgetTwo.Clone(),
                                              };
        }

        public void Add(RecordForSource record)
        {
            foreach (var rec in record.WorksOne)
            {
                AddWorksOne(rec.Key, rec.Value.Clone());
            }

            foreach (var rec in record.WorksTwo)
            {
                AddWorksTwo(rec.Key, rec.Value.Clone());
            }

            BudgetOne.Add(record.BudgetOne);
            BudgetTwo.Add(record.BudgetTwo);
        }

        public void AddWorksOne(int code, RecordWorks record)
        {
            if (!WorksOne.ContainsKey(code))
            {
                WorksOne.Add(code, record);
            }
            else
            {
                WorksOne[code] += record;
            }
        }

        public void AddWorksTwo(int code, RecordWorks record)
        {
            if (code == 999)
            {
                code = 24;
            }

            if (!WorksTwo.ContainsKey(code))
            {
                WorksTwo.Add(code, record);
            }
            else
            {
                WorksTwo[code] += record;
            }
        }
    }
}
