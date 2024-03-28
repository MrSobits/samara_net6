namespace Bars.Gkh.Import.Fund.Impl 
{
    using System.Collections.Generic;
    
    class Part1 : BaseTechPassportPartImport, ITechPassportPartImport
    {
        public static string Code
        {
            get
            {
                return "RASP_PART_1";
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
                { "COUNT_LES", new KeyValuePair<string, string>("Form_1", "9:1") },
                { "COUNT_NG", new KeyValuePair<string, string>("Form_1_3", "1:1") },
                { "COUNT_UB", new KeyValuePair<string, string>("Form_1_3_3", "3:1") },
                
                { "HAND_NOT_UP", new KeyValuePair<string, string>("Form_1_4", "7:4") },
                { "HAND_WITHOUT", new KeyValuePair<string, string>("Form_1_4", "10:4") },
                { "HAND_UP", new KeyValuePair<string, string>("Form_1_4", "4:4") },
                
                { "LIC_ACC", new KeyValuePair<string, string>("Form_1", "14:1") },
                { "MANS", new KeyValuePair<string, string>("Form_1", "15:1") },
                { "MAX_ET", new KeyValuePair<string, string>("Form_1", "11:1") },
                
                { "MECH_NOT_UP", new KeyValuePair<string, string>("Form_1_4", "6:4") },
                { "MECH_WITHOUT", new KeyValuePair<string, string>("Form_1_4", "9:4") },
                { "MECH_UP", new KeyValuePair<string, string>("Form_1_4", "3:4") },
                
                { "MIN_ET", new KeyValuePair<string, string>("Form_1", "10:1") },
                { "POKR_GAS", new KeyValuePair<string, string>("Form_1_4", "11:4") },
                { "POKR_NEW", new KeyValuePair<string, string>("Form_1_4", "2:4") },
                { "POKR_NONEW", new KeyValuePair<string, string>("Form_1_4", "5:4") },
                { "POKR_NOT", new KeyValuePair<string, string>("Form_1_4", "8:4") },
                { "SQV_LEST", new KeyValuePair<string, string>("Form_1_3_2", "1:4") },
                { "SQV_NG", new KeyValuePair<string, string>("Form_1_3", "2:1") },
                { "SQV_OF", new KeyValuePair<string, string>("Form_1_3_2", "3:4") },
                { "SQV_OP", new KeyValuePair<string, string>("Form_1_3_2", "2:4") }, 
                { "SQV_POD", new KeyValuePair<string, string>("Form_1_3_3", "2:1") },
                { "SQV_PR", new KeyValuePair<string, string>("Form_1_3_2", "5:4") },
                { "SQV_TEH", new KeyValuePair<string, string>("Form_1_3_2", "4:4") },
                { "SQV_UB", new KeyValuePair<string, string>("Form_1_3_3", "1:1") },
                { "TERR", new KeyValuePair<string, string>("Form_1_4", "1:4") },
                { "VAL", new KeyValuePair<string, string>("Form_1", "5:1") },
                { "YEAR_REC", new KeyValuePair<string, string>("Form_1", "3:1") }
            
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
                case "Form_1#9:1":
                case "Form_1_3#1:1":
                case "Form_1#14:1":
                case "Form_1#15:1":
                case "Form_1#11:1":
                case "Form_1#10:1":                
                    isValid = this.isInt(value);
                    break;

                case "Form_1#3:1":
                    isValid = value.Length == 4 ? this.isInt(value) : false;
                    break;

                case "Form_1_3_3#3:1":
                case "Form_1_4#4:4":
                case "Form_1_4#7:4":
                case "Form_1_4#10:4":
                case "Form_1_4#3:4":
                case "Form_1_4#6:4":
                case "Form_1_4#9:4":
                case "Form_1_4#11:4":
                case "Form_1_4#2:4":
                case "Form_1_4#5:4":
                case "Form_1_4#8:4":
                case "Form_1_3_2#1:4":
                case "Form_1_3#2:1":
                case "Form_1_3_2#2:4":
                case "Form_1_3_2#3:4":
                case "Form_1_3_3#2:1":
                case "Form_1_3_2#5:4":
                case "Form_1_3_2#4:4":
                case "Form_1_3_3#1:1":
                case "Form_1_4#1:4":
                case "Form_1#5:1":  
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
