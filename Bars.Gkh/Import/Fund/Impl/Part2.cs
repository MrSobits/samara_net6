namespace Bars.Gkh.Import.Fund.Impl
{
    using System.Collections.Generic;

    class Part2 : BaseTechPassportPartImport, ITechPassportPartImport
    {
        public static string Code
        {
            get
            {
                return "RASP_PART_2";
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
                { "COUNT_1", new KeyValuePair<string, string>("Form_1_2", "1:3") },
                { "COUNT_2", new KeyValuePair<string, string>("Form_1_2", "2:3") },
                { "COUNT_3", new KeyValuePair<string, string>("Form_1_2", "3:3") },
                { "COUNT_4", new KeyValuePair<string, string>("Form_1_2", "4:3") },
                { "COUNT_5", new KeyValuePair<string, string>("Form_1_2", "5:3") },
                { "COUNT_6", new KeyValuePair<string, string>("Form_1_2", "6:3") },
                { "COUNT_7", new KeyValuePair<string, string>("Form_1_2", "7:3") },
                { "COUNT_KK_2", new KeyValuePair<string, string>("Form_1_2_2", "1:4") },
                { "COUNT_KK_3", new KeyValuePair<string, string>("Form_1_2_2", "2:4") },
                { "COUNT_KK_4", new KeyValuePair<string, string>("Form_1_2_2", "3:4") },
                { "COUNT_KK_5", new KeyValuePair<string, string>("Form_1_2_2", "4:4") },
                { "COUNT_KK_6", new KeyValuePair<string, string>("Form_1_2_2", "5:4") },
                { "COUNT_KK_7", new KeyValuePair<string, string>("Form_1_2_2", "6:4") },
                { "COUNT_K_2", new KeyValuePair<string, string>("Form_1_2_2", "1:3") },
                { "COUNT_K_3", new KeyValuePair<string, string>("Form_1_2_2", "2:3") },
                { "COUNT_K_4", new KeyValuePair<string, string>("Form_1_2_2", "3:3") },
                { "COUNT_K_5", new KeyValuePair<string, string>("Form_1_2_2", "4:3") },
                { "COUNT_K_6", new KeyValuePair<string, string>("Form_1_2_2", "5:3") },
                { "COUNT_K_7", new KeyValuePair<string, string>("Form_1_2_2", "6:3") },
                { "COUNT_O", new KeyValuePair<string, string>("Form_1_2_3", "1:1") },
                { "SQVG_1", new KeyValuePair<string, string>("Form_1_2", "1:5") },
                { "SQVG_2", new KeyValuePair<string, string>("Form_1_2", "2:5") },
                { "SQVG_3", new KeyValuePair<string, string>("Form_1_2", "3:5") },
                { "SQVG_4", new KeyValuePair<string, string>("Form_1_2", "4:5") },
                { "SQVG_5", new KeyValuePair<string, string>("Form_1_2", "5:5") },
                { "SQVG_6", new KeyValuePair<string, string>("Form_1_2", "6:5") },
                { "SQVG_7", new KeyValuePair<string, string>("Form_1_2", "7:5") },
                { "SQVG_K_2", new KeyValuePair<string, string>("Form_1_2_2", "1:5") },
                { "SQVG_K_3", new KeyValuePair<string, string>("Form_1_2_2", "2:5") },
                { "SQVG_K_4", new KeyValuePair<string, string>("Form_1_2_2", "3:5") },
                { "SQVG_K_5", new KeyValuePair<string, string>("Form_1_2_2", "4:5") },
                { "SQVG_K_6", new KeyValuePair<string, string>("Form_1_2_2", "5:5") },
                { "SQVG_K_7", new KeyValuePair<string, string>("Form_1_2_2", "6:5") },
                { "SQVG_O", new KeyValuePair<string, string>("Form_1_2_3", "3:1") },
                { "SQV_1", new KeyValuePair<string, string>("Form_1_2", "1:4") },
                { "SQV_2", new KeyValuePair<string, string>("Form_1_2", "2:4") },
                { "SQV_3", new KeyValuePair<string, string>("Form_1_2", "3:4") },
                { "SQV_4", new KeyValuePair<string, string>("Form_1_2", "4:4") },
                { "SQV_5", new KeyValuePair<string, string>("Form_1_2", "5:4") },
                { "SQV_6", new KeyValuePair<string, string>("Form_1_2", "6:4") },
                { "SQV_7", new KeyValuePair<string, string>("Form_1_2", "7:4") },
                { "SQV_K_2", new KeyValuePair<string, string>("Form_1_2_2", "1:6") },
                { "SQV_K_3", new KeyValuePair<string, string>("Form_1_2_2", "2:6") },
                { "SQV_K_4", new KeyValuePair<string, string>("Form_1_2_2", "3:6") },
                { "SQV_K_5", new KeyValuePair<string, string>("Form_1_2_2", "4:6") },
                { "SQV_K_6", new KeyValuePair<string, string>("Form_1_2_2", "5:6") },
                { "SQV_K_7", new KeyValuePair<string, string>("Form_1_2_2", "6:6") },
                { "SQV_O", new KeyValuePair<string, string>("Form_1_2_3", "2:1") },
                { "ANOTHER", new KeyValuePair<string, string>("Form_2", "15:3") }, 
                { "GASON", new KeyValuePair<string, string>("Form_2", "14:3") },
                { "SQVUARE", new KeyValuePair<string, string>("Form_2", "13:3") },
                { "SQV_AN", new KeyValuePair<string, string>("Form_2", "8:3") },
                { "SQV_GREEN", new KeyValuePair<string, string>("Form_2", "12:3") },
                { "SQV_GROUND_AN", new KeyValuePair<string, string>("Form_2", "11:3") },
                { "SQV_GROUND_CH", new KeyValuePair<string, string>("Form_2", "9:3") },
                { "SQV_GROUND_SPORT", new KeyValuePair<string, string>("Form_2", "10:3") },
                { "SQV_HARD", new KeyValuePair<string, string>("Form_2", "5:3") },
                { "SQV_PRO", new KeyValuePair<string, string>("Form_2", "6:3") },
                { "SQV_TERR", new KeyValuePair<string, string>("Form_2", "1:3") },
                { "SQV_TERR_FACT", new KeyValuePair<string, string>("Form_2", "2:3") },
                { "SQV_TERR_FOND", new KeyValuePair<string, string>("Form_2", "3:3") },
                { "SQV_TERR_NOTFOND", new KeyValuePair<string, string>("Form_2", "4:3") },
                { "SQV_TRO", new KeyValuePair<string, string>("Form_2", "7:3") }            
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
                case "Form_1_2#1:3":
                case "Form_1_2#2:3":
                case "Form_1_2#3:3":
                case "Form_1_2#4:3":
                case "Form_1_2#5:3":
                case "Form_1_2#6:3":
                case "Form_1_2#7:3":
                case "Form_1_2_2#1:4":
                case "Form_1_2_2#2:4":
                case "Form_1_2_2#3:4":
                case "Form_1_2_2#4:4":
                case "Form_1_2_2#5:4":
                case "Form_1_2_2#6:4":
                case "Form_1_2_2#1:3":
                case "Form_1_2_2#2:3":
                case "Form_1_2_2#3:3":
                case "Form_1_2_2#4:3":
                case "Form_1_2_2#5:3":
                case "Form_1_2_2#6:3":
                case "Form_1_2_3#1:1":
                    isValid = this.isInt(value);
                    break;

                case "Form_1_2#1:5":
                case "Form_1_2#2:5":
                case "Form_1_2#3:5":
                case "Form_1_2#4:5":
                case "Form_1_2#5:5":
                case "Form_1_2#6:5":
                case "Form_1_2#7:5":
                case "Form_1_2_2#1:5":
                case "Form_1_2_2#2:5":
                case "Form_1_2_2#3:5":
                case "Form_1_2_2#4:5":
                case "Form_1_2_2#5:5":
                case "Form_1_2_2#6:5":
                case "Form_1_2_3#3:1":
                case "Form_1_2#1:4":
                case "Form_1_2#2:4":
                case "Form_1_2#3:4":
                case "Form_1_2#4:4":
                case "Form_1_2#5:4":
                case "Form_1_2#6:4":
                case "Form_1_2#7:4":
                case "Form_1_2_2#1:6":
                case "Form_1_2_2#2:6":
                case "Form_1_2_2#3:6":
                case "Form_1_2_2#4:6":
                case "Form_1_2_2#5:6":
                case "Form_1_2_2#6:6":
                case "Form_1_2_3#2:1":
                case "Form_2#15:3":
                case "Form_2#14:3":
                case "Form_2#13:3":
                case "Form_2#8:3":
                case "Form_2#12:3":
                case "Form_2#11:3":
                case "Form_2#9:3":
                case "Form_2#10:3":
                case "Form_2#5:3":
                case "Form_2#6:3":
                case "Form_2#1:3":
                case "Form_2#2:3":
                case "Form_2#3:3":
                case "Form_2#4:3":
                case "Form_2#7:3":
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
