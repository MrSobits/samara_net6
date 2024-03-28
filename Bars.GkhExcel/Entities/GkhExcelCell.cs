namespace Bars.GkhExcel
{
    using System;
    //using OfficeOpenXml;

    public class GkhExcelCell
    {
        //public GkhExcelCell(ExcelRange excelRange)
        //{
        //    Value = excelRange.Text;
        //    FontBold = excelRange.Style.Font.Bold;
        //    IsMerged = excelRange.Merge;
        //}

        public GkhExcelCell(string Value, bool IsMerged, bool FontBold)
        {
            this.Value = Value;
            this.FontBold = FontBold;
            this.IsMerged = IsMerged;
        }

        public string Value { get; set; }

        public bool IsMerged { get; set; }

        public bool FontBold { get; set; }

        public DateTime? ToDateTimeNullable()
        {
            DateTime result;
            if (DateTime.TryParse(this.Value, out result))
            {
                return result;
            }

            return null;
        }

        public decimal? ToDecimalNullable()
        {
            decimal result;
            if (decimal.TryParse(this.Value, out result))
            {
                return result;
            }

            return null;
        }
    }
}
