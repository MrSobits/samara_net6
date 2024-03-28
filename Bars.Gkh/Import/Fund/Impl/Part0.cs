namespace Bars.Gkh.Import.Fund.Impl
{
    using System.Collections.Generic;

    public class Part0 : BaseTechPassportPartImport, ITechPassportPartImport
    {
        public static string Code
        {
            get
            {
                return "RASP_PART_0";
            }
        }

        string ITechPassportPartImport.Code
        {
            get
            {
                return Code;
            }
        }

        protected override string ImportCode
        {
            get
            {
                return Code + "-Техпаспорт";
            }
        }

        protected override void InitDictCodes()
        {
            dictCodes = new Dictionary<string, KeyValuePair<string, string>>
            {
                { "COUNT_ENTRANCE", new KeyValuePair<string, string>("Form_1", "12:1") },
                { "COUNT_PEOPLE", new KeyValuePair<string, string>("Form_1", "13:1") },
                { "INV_N", new KeyValuePair<string, string>("Form_1", "18:1") },
                { "SQV", new KeyValuePair<string, string>("Form_1", "6:1") },
                { "SQV_GIL", new KeyValuePair<string, string>("Form_1", "7:1") },
                { "SQV_GOS", new KeyValuePair<string, string>("Form_1", "26:1") },
                { "SQV_GR", new KeyValuePair<string, string>("Form_1", "24:1") },
                { "SQV_MUN", new KeyValuePair<string, string>("Form_1", "25:1") }
            };
        }
        
        protected override TehPassportValueProxy GetCorrectValue(string formCode, string cellCode, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                // Сюда попадем, только если ReplaceData == true
                return new TehPassportValueProxy { FormCode = formCode, CellCode = cellCode, Value = value };
            }

            var formCodeCellCode = formCode + "#" + cellCode;

            bool isValid = true;

            switch (formCodeCellCode)
            {
                case "Form_1#12:1":
                case "Form_1#13:1":
                    isValid = this.isInt(value);
                    break;

                case "Form_1#6:1":
                case "Form_1#7:1":
                case "Form_1#24:1":
                case "Form_1#25:1":
                case "Form_1#26:1":
                    isValid = this.isDecimal(value);
                    break;
            }

            if (!isValid)
            {
                return null;
            } 

            return new TehPassportValueProxy { FormCode = formCode, CellCode = cellCode, Value = value };
        }
    }
}