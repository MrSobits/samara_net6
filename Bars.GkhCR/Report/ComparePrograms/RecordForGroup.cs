namespace Bars.GkhCr.Report.ComparePrograms
{
    using System.Collections.Generic;

    internal class RecordForGroup
    {
        public RecordForGroup(string group)
        {
            Municipalities = new Dictionary<string, RecordMunicipality>();
            Group = group;
            Grand = new RecordRealtyObject();
        }

        public string Group { get; set; }

        public Dictionary<string, RecordMunicipality> Municipalities { get; set; }

        /// <summary>
        /// Итоги
        /// </summary>
        public RecordRealtyObject Grand { get; set; }
    }
}
