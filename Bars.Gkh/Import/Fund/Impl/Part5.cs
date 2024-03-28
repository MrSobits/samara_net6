namespace Bars.Gkh.Import.Fund.Impl
{
    using System.Collections.Generic;

    public class Part5 : BaseTechPassportPartImport, ITechPassportPartImport
    {
        public static string Code
        {
            get
            {
                return "RASP_PART_5";
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
                    {"BAL_VOS", new KeyValuePair<string, string>("Form_5_10", "1:3")},
                    {"BASE_TYPE", new KeyValuePair<string, string>("Form_5_1", "100")},
                    {"CHER", new KeyValuePair<string, string>("Form_5_3_2", "3:4")},
                    {"COUNT_BUN", new KeyValuePair<string, string>("Form_5_9_1", "1:1")},
                    {"COUNT_DOM", new KeyValuePair<string, string>("Form_5_9_1", "2:1")},
                    {"COUNT_DOOR", new KeyValuePair<string, string>("Form_5_5", "3:1")},
                    {"COUNT_DOOR_MET", new KeyValuePair<string, string>("Form_5_5", "4:1")},
                    {"COUNT_GEL", new KeyValuePair<string, string>("Form_5_6_2", "15:1")},
                    {"COUNT_KASH", new KeyValuePair<string, string>("Form_5_9_1", "3:1")},
                    {"COUNT_KOLP", new KeyValuePair<string, string>("Form_5_6_2", "7:1")},
                    {"COUNT_KR_F", new KeyValuePair<string, string>("Form_5_6_2", "9:1")},
                    {"COUNT_KR_K", new KeyValuePair<string, string>("Form_5_6_2", "13:1")},
                    {"COUNT_KR_LAZ", new KeyValuePair<string, string>("Form_5_6_2", "10:1")},
                    {"COUNT_KR_P", new KeyValuePair<string, string>("Form_5_6_2", "12:1")},
                    {"COUNT_KR_VIH", new KeyValuePair<string, string>("Form_5_6_2", "11:1")},
                    {"COUNT_LES_UK", new KeyValuePair<string, string>("Form_5_9", "5:1")},
                    {"COUNT_OGD_DET", new KeyValuePair<string, string>("Form_5_9_1", "7:1")},
                    {"COUNT_PES", new KeyValuePair<string, string>("Form_5_9_1", "4:1")},
                    {"COUNT_PODV_WIN", new KeyValuePair<string, string>("Form_5_5", "10:1")},
                    {"COUNT_PR", new KeyValuePair<string, string>("Form_5_5", "14:1")},
                    {"COUNT_SK", new KeyValuePair<string, string>("Form_5_9", "6:1")},
                    {"COUNT_SKAM", new KeyValuePair<string, string>("Form_5_9_1", "5:1")},
                    {"COUNT_STR", new KeyValuePair<string, string>("Form_5_6_2", "5:1")},
                    {"COUNT_SVED", new KeyValuePair<string, string>("Form_5_9_1", "6:1")},
                    {"COUNT_VENT", new KeyValuePair<string, string>("Form_5_6_2", "8:1")},
                    {"COUNT_VOD", new KeyValuePair<string, string>("Form_5_6_2", "16:1")},
                    {"COUNT_WIND", new KeyValuePair<string, string>("Form_5_5", "1:1")},
                    {"FUND_VAL", new KeyValuePair<string, string>("Form_5_1_1", "3:4")},
                    {"KROV", new KeyValuePair<string, string>("Form_5_6", "1:3")},
                    {"LONG_LOT", new KeyValuePair<string, string>("Form_5_6_2", "20:1")},
                    {"LONG_OGR", new KeyValuePair<string, string>("Form_5_6_2", "21:1")},
                    {"LONG_O_P", new KeyValuePair<string, string>("Form_5_6_2", "6:1")},
                    {"LONG_ST", new KeyValuePair<string, string>("Form_5_6_2", "19:1")},
                    {"LONG_STOK", new KeyValuePair<string, string>(string.Empty, string.Empty)},
                    {"LONG_SV", new KeyValuePair<string, string>("Form_5_6_2", "18:1")},
                    {"LONG_VOD", new KeyValuePair<string, string>("Form_5_6_2", "17:1")},
                    {"ME", new KeyValuePair<string, string>("Form_5_3_2", "1:4")},
                    {"PEREKR_TYPE", new KeyValuePair<string, string>("Form_5_3", "1:3")},
                    {"POD", new KeyValuePair<string, string>("Form_5_3_2", "2:4")},
                    {"SKOB", new KeyValuePair<string, string>("Form_5_5", "9:1")},
                    {"SL_WIN", new KeyValuePair<string, string>("Form_5_5", "12:1")},
                    {"SQL_OBL_LIC", new KeyValuePair<string, string>("Form_5_8", "6:1")},
                    {"SQL_PVH", new KeyValuePair<string, string>(string.Empty, string.Empty)},
                    {"SQV_ASF", new KeyValuePair<string, string>("Form_5_9", "1:1")},

                    {"SQV_BAL", new KeyValuePair<string, string>("Form_5_8", "13:1")},

                    {"SQV_COK", new KeyValuePair<string, string>("Form_5_1_1", "1:4")},
                    {"SQV_DOOR", new KeyValuePair<string, string>("Form_5_5", "5:1")},
                    {"SQV_DOOR_MET", new KeyValuePair<string, string>("Form_5_5", "6:1")},
                    {"SQV_FAC", new KeyValuePair<string, string>("Form_5_8", "9:1")},
                    {"SQV_FAC_DVOR", new KeyValuePair<string, string>("Form_5_8", "11:1")},
                    {"SQV_FAC_LIC", new KeyValuePair<string, string>("Form_5_8", "10:1")},
                    {"SQV_FAC_TORC", new KeyValuePair<string, string>("Form_5_8", "12:1")},
                    {"SQV_GL", new KeyValuePair<string, string>("Form_5_5", "7:1")},
                    {"SQV_IN_DVOR", new KeyValuePair<string, string>(string.Empty, string.Empty)},
                    {"SQV_KAR", new KeyValuePair<string, string>("Form_5_8", "16:1")},
                    {"SQV_KERAM", new KeyValuePair<string, string>("Form_5_4", "3:4")},
                    {"SQV_KL_OP", new KeyValuePair<string, string>("Form_5_7", "1:4")},
                    {"SQV_KR_K", new KeyValuePair<string, string>("Form_5_6_2", "14:1")},
                    {"SQV_KR_MET", new KeyValuePair<string, string>("Form_5_6_2", "1:1")},
                    {"SQV_KR_O", new KeyValuePair<string, string>("Form_5_6_2", "4:1")},
                    {"SQV_KR_RUL", new KeyValuePair<string, string>("Form_5_6_2", "2:1")},
                    {"SQV_KR_S", new KeyValuePair<string, string>("Form_5_6_2", "3:1")},
                    {"SQV_LES_M", new KeyValuePair<string, string>("Form_5_7", "9:4")},
                    {"SQV_LK", new KeyValuePair<string, string>("Form_5_4", "1:4")},
                    {"SQV_LOW", new KeyValuePair<string, string>("Form_5_7", "10:4")},
                    {"SQV_MASH", new KeyValuePair<string, string>("Form_5_4", "8:4")},
                    {"SQV_MET", new KeyValuePair<string, string>("Form_5_8", "14:1")},
                    {"SQV_MR", new KeyValuePair<string, string>("Form_5_4", "4:4")},
                    {"SQV_MS_OP", new KeyValuePair<string, string>("Form_5_7", "2:4")},
                    {"SQV_MUS_POL", new KeyValuePair<string, string>("Form_5_4", "6:4")},
                    {"SQV_OBL", new KeyValuePair<string, string>("Form_5_8", "5:1")},
                    {"SQV_OBL_DVOR", new KeyValuePair<string, string>("Form_5_8", "7:1")},
                    {"SQV_OBL_TORC", new KeyValuePair<string, string>("Form_5_8", "8:1")},
                    {"SQV_OGR", new KeyValuePair<string, string>("Form_5_8", "20:1")},
                    {"SQV_OGR_GAS", new KeyValuePair<string, string>("Form_5_9", "7:1")},
                    {"SQV_OP_AN", new KeyValuePair<string, string>("Form_5_7", "11:4")},
                    {"SQV_OTM", new KeyValuePair<string, string>("Form_5_8", "32:1")},
                    {"SQV_O_WIN", new KeyValuePair<string, string>("Form_5_8", "19:1")},
                    {"SQV_PAR", new KeyValuePair<string, string>("Form_5_8", "15:1")},
                    {"SQV_PER", new KeyValuePair<string, string>("Form_5_2_2", "2:4")},
                    {"SQV_PO", new KeyValuePair<string, string>("Form_5_8", "17:1")},
                    {"SQV_PODV_WIN", new KeyValuePair<string, string>("Form_5_5", "11:1")},
                    {"SQV_POST", new KeyValuePair<string, string>("Form_5_7", "8:4")},
                    {"SQV_PRI", new KeyValuePair<string, string>("Form_5_4", "7:4")},
                    {"SQV_RAD_OP", new KeyValuePair<string, string>("Form_5_7", "4:4")},
                    {"SQV_RESH", new KeyValuePair<string, string>("Form_5_7", "5:4")},
                    {"SQV_RESH_WIN", new KeyValuePair<string, string>("Form_5_7", "6:4")},
                    {"SQV_SHAHT", new KeyValuePair<string, string>("Form_5_7", "7:4")},
                    {"SQV_ST", new KeyValuePair<string, string>("Form_5_2_2", "1:4")},
                    {"SQV_STEN", new KeyValuePair<string, string>("Form_5_8", "21:1")},
                    {"SQV_STUK", new KeyValuePair<string, string>("Form_5_8", "1:1")},
                    {"SQV_STUK_DVOR", new KeyValuePair<string, string>("Form_5_8", "3:1")},
                    {"SQV_STUK_LIC", new KeyValuePair<string, string>("Form_5_8", "2:1")},
                    {"SQV_STUK_TORC", new KeyValuePair<string, string>("Form_5_8", "4:1")},
                    {"SQV_T", new KeyValuePair<string, string>("Form_5_8", "18:1")},
                    {"SQV_TR", new KeyValuePair<string, string>("Form_5_7", "3:4")},
                    {"SQV_TROT", new KeyValuePair<string, string>("Form_5_9", "3:1")},
                    {"SQV_UN", new KeyValuePair<string, string>("Form_5_5", "8:1")},
                    {"SQV_WIND", new KeyValuePair<string, string>("Form_5_5", "2:1")},
                    {"SQV_ZAM", new KeyValuePair<string, string>("Form_5_9", "4:1")},
                    {"VENT_WIN", new KeyValuePair<string, string>("Form_5_5", "13:1")},
                    {"WALL_TYPE", new KeyValuePair<string, string>("Form_5_2", "1:3")}
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
                #region Decimal
                case "Form_6_4#11:4":
                case "Form_5_10#1:3":
                case "Form_5_3_2#3:4":
                case "Form_5_6_2#15:1":
                case "Form_5_9_1#7:1":
                case "Form_5_5#14:1":
                case "Form_5_1_1#3:4":
                case "Form_5_6_2#21:1":
                case "Form_5_6_2#6:1":
                case "Form_5_6_2#19:1":
                case "Form_5_6_2#18:1":
                case "Form_5_6_2#17:1":
                case "Form_5_3_2#1:4":
                case "Form_5_3_2#2:4":
                case "Form_5_5#12:1":
                case "Form_5_8#6:1":
                case "Form_5_9#1:1":
                case "Form_5_1_1#1:4":
                case "Form_5_5#5:1":
                case "Form_5_5#6:1":
                case "Form_5_8#9:1":
                case "Form_5_8#11:1":
                case "Form_5_8#10:1":
                case "Form_5_8#12:1":
                case "Form_5_5#7:1":
                case "Form_5_8#16:1":
                case "Form_5_4#3:4":
                case "Form_5_7#1:4":
                case "Form_5_6_2#14:1":
                case "Form_5_6_2#1:1":
                case "Form_5_6_2#4:1":
                case "Form_5_6_2#2:1":
                case "Form_5_6_2#3:1":
                case "Form_5_7#9:4":
                case "Form_5_4#1:4":
                case "Form_5_7#10:4":
                case "Form_5_4#8:4":
                case "Form_5_8#14:1":
                case "Form_5_4#4:4":
                case "Form_5_7#2:4":
                case "Form_5_4#6:4":
                case "Form_5_8#5:1":
                case "Form_5_8#7:1":
                case "Form_5_8#8:1":
                case "Form_5_8#20:1":
                case "Form_5_9#7:1":
                case "Form_5_7#11:4":
                case "Form_5_8#32:1":
                case "Form_5_8#19:1":
                case "Form_5_8#15:1":
                case "Form_5_2_2#2:4":
                case "Form_5_8#17:1":
                case "Form_5_5#11:1":
                case "Form_5_7#8:4":
                case "Form_5_4#7:4":
                case "Form_5_7#4:4":
                case "Form_5_7#5:4":
                case "Form_5_7#6:4":
                case "Form_5_7#7:4":
                case "Form_5_2_2#1:4":
                case "Form_5_8#21:1":
                case "Form_5_8#1:1":
                case "Form_5_8#3:1":
                case "Form_5_8#2:1":
                case "Form_5_8#4:1":
                case "Form_5_8#18:1":
                case "Form_5_7#3:4":
                case "Form_5_9#3:1":
                case "Form_5_5#8:1":
                case "Form_5_5#2:1":
                case "Form_5_9#4:1":
                case "Form_5_5#13:1":
                case "Form_5_8#13:1":
                #endregion
                    isValid = this.isDecimal(value);
                    break;

                #region Int
                case "Form_6_4#11:3":
                case "Form_5_9_1#1:1":
                case "Form_5_9_1#2:1":
                case "Form_5_5#3:1":
                case "Form_5_5#4:1":
                case "Form_5_9_1#3:1":
                case "Form_5_6_2#7:1":
                case "Form_5_6_2#9:1":
                case "Form_5_6_2#13:1":
                case "Form_5_6_2#10:1":
                case "Form_5_6_2#12:1":
                case "Form_5_6_2#11:1":
                case "Form_5_9#5:1":
                case "Form_5_9_1#4:1":
                case "Form_5_5#10:1":
                case "Form_5_9#6:1":
                case "Form_5_9_1#5:1":
                case "Form_5_6_2#5:1":
                case "Form_5_9_1#6:1":
                case "Form_5_6_2#8:1":
                case "Form_5_6_2#16:1":
                case "Form_5_5#1:1":
                case "Form_5_6_2#20:1":
                case "Form_5_5#9:1":
                #endregion
                    isValid = this.isInt(value);
                    break;

                case "Form_5_2#1:3":
                    isValid = this.isTypeWalls(value);  // стены
                    break;

                case "Form_5_6#1:3":
                    isValid = this.isTypeRoof(value);  // кровля
                    break;

                case "Form_5_3#1:3":
                    isValid = this.isTypeFloor(value); // перекрытия
                    break;

                case "Form_5_1#100":
                    isValid = this.Fund(ref value, out cellCode); // тип фундамента
                    break;
            }

            if (!isValid)
            {
                return null;
            }

            return new TehPassportValueProxy { FormCode = formCode, CellCode = cellCode, Value = value };
        }

        private bool isTypeWalls(string value)
        {
            var res = (value == "1") || (value == "2") || (value == "3") || (value == "4");
            return res;
        }

        private bool isTypeRoof(string value)
        {
            var res = (value == "1") || (value == "2");
            return res;
        }

        private bool isTypeFloor(string value)
        {
            var res = (value == "1") || (value == "2") || (value == "3");
            return res;
        }

        private bool Fund(ref string value, out string cellcode)
        {
            cellcode = string.Empty;
            switch (value)
            {
                case "1":
                    cellcode = "1:3";
                    break;
                case "5":
                    cellcode = "5:3";
                    break;
                case "4":
                    cellcode = "4:3";
                    break;
                case "3":
                    cellcode = "3:3";
                    break;
                case "2":
                    cellcode = "2:3";
                    break;
            }
            value = cellcode != string.Empty ? "1" : string.Empty;
            return value == "1";
        }
    }
}