namespace Bars.Gkh.Import.Fund.Impl
{
    using System.Collections.Generic;

    class Part4 : BaseTechPassportPartImport, ITechPassportPartImport 
    {
        public static string Code
        {
            get
            {
                return "RASP_PART_4";
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
                { "ANTEN", new KeyValuePair<string, string>("Form_4_1_1", "5:3") },
                { "APPZ", new KeyValuePair<string, string>("Form_4_1_1", "1:3") },
                { "COMP", new KeyValuePair<string, string>("Form_4_1_1", "7:3") },
                { "DISP", new KeyValuePair<string, string>("Form_4_2", "2:1") },
                { "LIFTS", new KeyValuePair<string, string>("Form_4_1", "1:4") },
                { "LIFTS_COUNT", new KeyValuePair<string, string>("Form_4_2", "1:1") },
                { "LIFTS_O", new KeyValuePair<string, string>("Form_4_1", "3:4") },
                { "LIFTS_R", new KeyValuePair<string, string>("Form_4_1", "2:4") },
                { "ODS", new KeyValuePair<string, string>("Form_4_1_1", "2:3") },
                { "RADIO", new KeyValuePair<string, string>("Form_4_1_1", "4:3") },
                { "TEL", new KeyValuePair<string, string>("Form_4_1_1", "3:3") },
                { "TV", new KeyValuePair<string, string>("Form_4_1_1", "6:3") }           
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
                case "Form_4_1#1:4":
                case "Form_4_2#1:1":
                case "Form_4_1#3:4":
                case "Form_4_1#2:4":
                    isValid = this.isInt(value);
                    break;

                case "Form_4_1_1#5:3":
                case "Form_4_1_1#1:3":
                case "Form_4_1_1#7:3":
                case "Form_4_1_1#2:3":
                case "Form_4_1_1#4:3":
                case "Form_4_1_1#3:3":
                case "Form_4_1_1#6:3":
                    isValid = this.isBool(value);
                    break;

                case "Form_4_2#2:1":
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
