namespace Bars.Gkh.RegOperator.Domain.ImportExport
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Utils;

    public class ImportResult<T>
    {
        public IEnumerable<ImportRow<T>> Rows { get; set; }

        [Display("Дата файла")]
        public DateTime FileDate { get; set; }

        [Display("Номер файла")]
        public string FileNumber { get; set; }

        public DynamicDictionary GeneralData { get; set; }

        public ImportResult()
        {
            GeneralData = DynamicDictionary.Create();
            Rows = new List<ImportRow<T>>();
        }
    }
}