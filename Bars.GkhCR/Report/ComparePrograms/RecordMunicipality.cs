namespace Bars.GkhCr.Report.ComparePrograms
{
    using System.Collections.Generic;
    using System.Linq;

    internal class RecordMunicipality
    {
        public RecordMunicipality(string municipality)
        {
            RealtyObjects = new Dictionary<long, RecordRealtyObject>();
            Municipality = municipality;
            Total = new RecordRealtyObject();
        }

        public string Municipality { get; set; }

        public Dictionary<long, RecordRealtyObject> RealtyObjects { get; set; }

        public RecordRealtyObject Total { get; set; }

        public void Add(RecordMunicipality record)
        {
            foreach (var rec in record.RealtyObjects)
            {
                if (RealtyObjects.ContainsKey(rec.Key))
                {
                    RealtyObjects[rec.Key].Add(rec.Value.Clone());
                }
                else
                {
                    RealtyObjects.Add(rec.Key, rec.Value.Clone());
                }
            }
        }

        /// <summary>
        /// Превышение лимита
        /// </summary>
        /// <returns></returns>
        public decimal[] OverLimit()
        {
            return new[]
                       {
                           Total.GroupFinances.Sum(x => x.Value.Sources.Sum(y => y.Value.WorksOne.Sum(j => j.Value.Sum))),
                           Total.GroupFinances.Sum(x => x.Value.Sources.Sum(y => y.Value.WorksTwo.Sum(j => j.Value.Sum)))
                       };
        }
    }
}
