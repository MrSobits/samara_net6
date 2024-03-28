namespace Bars.Gkh.Import.Fund.Impl
{
    using System.Collections.Generic;

    public class Part6 : BaseTechPassportPartImport, ITechPassportPartImport
    {
        public static string Code
        {
            get
            {
                return "RASP_PART_6";
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
                {"AVG_RASHOD_1", new KeyValuePair<string, string>("Form_6_4", "11:4")},
                {"AVG_RASHOD_2", new KeyValuePair<string, string>("Form_6_4", "11:5")},
                {"TEPL_ENERGY", new KeyValuePair<string, string>("Form_6_2", "1:4")},
                {"EL_11", new KeyValuePair<string, string>("Form_6_5", "2:3")},
                {"EL_12", new KeyValuePair<string, string>("Form_6_5", "2:4")},
                {"EL_13", new KeyValuePair<string, string>("Form_6_5", "2:5")},
                {"EL_14", new KeyValuePair<string, string>("Form_6_5", "2:6")},
                {"EL_21", new KeyValuePair<string, string>("Form_6_5", "3:3")},
                {"EL_22", new KeyValuePair<string, string>("Form_6_5", "3:4")},
                {"EL_23", new KeyValuePair<string, string>("Form_6_5", "3:5")},
                {"EL_24", new KeyValuePair<string, string>("Form_6_5", "3:6")},
                {"EL_31", new KeyValuePair<string, string>("Form_6_5", "5:3")},
                {"EL_32", new KeyValuePair<string, string>("Form_6_5", "5:4")},
                {"EL_33", new KeyValuePair<string, string>("Form_6_5", "5:5")},
                {"EL_34", new KeyValuePair<string, string>("Form_6_5", "5:6")},
                {"EL_41", new KeyValuePair<string, string>("Form_6_5", "6:3")},
                {"EL_42", new KeyValuePair<string, string>("Form_6_5", "6:4")},
                {"EL_43", new KeyValuePair<string, string>("Form_6_5", "6:5")},
                {"EL_44", new KeyValuePair<string, string>("Form_6_5", "6:6")},
                {"EL_51", new KeyValuePair<string, string>("Form_6_5", "7:3")},
                {"EL_52", new KeyValuePair<string, string>("Form_6_5", "7:4")},
                {"EL_53", new KeyValuePair<string, string>("Form_6_5", "7:5")},
                {"EL_54", new KeyValuePair<string, string>("Form_6_5", "7:6")},
                {"EL_61", new KeyValuePair<string, string>("Form_6_5", "8:3")},
                {"EL_62", new KeyValuePair<string, string>("Form_6_5", "8:4")},
                {"EL_63", new KeyValuePair<string, string>("Form_6_5", "8:5")},
                {"EL_64", new KeyValuePair<string, string>("Form_6_5", "8:6")},
                {"EL_71", new KeyValuePair<string, string>("Form_6_5", "9:3")},
                {"EL_72", new KeyValuePair<string, string>("Form_6_5", "9:4")},
                {"EL_73", new KeyValuePair<string, string>("Form_6_5", "9:5")},
                {"EL_74", new KeyValuePair<string, string>("Form_6_5", "9:6")},
                {"VODOPR_WATER", new KeyValuePair<string, string>("Form_6_2", "10:4")},
                {"EM_11", new KeyValuePair<string, string>("Form_6_4", "7:4")},
                {"EM_12", new KeyValuePair<string, string>("Form_6_4", "7:5")},
                {"EM_21", new KeyValuePair<string, string>("Form_6_4", "8:4")},
                {"EM_22", new KeyValuePair<string, string>("Form_6_4", "8:5")},
                {"EM_31", new KeyValuePair<string, string>("Form_6_4", "9:4")},
                {"EM_32", new KeyValuePair<string, string>("Form_6_4", "9:5")},
                {"EM_41", new KeyValuePair<string, string>("Form_6_4", "10:4")},
                {"EM_42", new KeyValuePair<string, string>("Form_6_4", "10:5")},
                {"GRAD_SUT", new KeyValuePair<string, string>("Form_6_1", "6:4")},
                {"HAR_ZDAN_1", new KeyValuePair<string, string>("Form_6_4", "20:4")},
                {"HAR_ZDAN_2", new KeyValuePair<string, string>("Form_6_4", "20:5")},
                /*{"HOT_SNABJ", new KeyValuePair<string, string>("Form_6_4", "3:4")}, пока не грузим*/
                {"HOURS_LONG", new KeyValuePair<string, string>("Form_6_1", "5:4")},
                {"OTOP_VENT", new KeyValuePair<string, string>("Form_6_2", "2:4")},
                {"EL_ENERGY", new KeyValuePair<string, string>("Form_6_2", "4:4")},
                {"COMM_VEDOM", new KeyValuePair<string, string>("Form_6_2", "5:4")},
                {"LIFT_EQUIP", new KeyValuePair<string, string>("Form_6_2", "6:4")},
                {"SSR_11", new KeyValuePair<string, string>("Form_6_4", "13:4")},
                {"SSR_12", new KeyValuePair<string, string>("Form_6_4", "13:5")},
                {"SSR_21", new KeyValuePair<string, string>("Form_6_4", "14:4")},
                {"SSR_22", new KeyValuePair<string, string>("Form_6_4", "14:5")},
                {"SSR_31", new KeyValuePair<string, string>("Form_6_4", "15:4")},
                {"SSR_32", new KeyValuePair<string, string>("Form_6_4", "15:5")},
                {"SSR_41", new KeyValuePair<string, string>("Form_6_4", "16:4")},
                {"SSR_42", new KeyValuePair<string, string>("Form_6_4", "16:5")},
                {"TEMP_AVG_OUT", new KeyValuePair<string, string>("Form_6_1", "4:4")},
                {"TEMP_CALC", new KeyValuePair<string, string>("Form_6_1", "2:4")},
                {"TEMP_CALC_OUT", new KeyValuePair<string, string>("Form_6_1", "3:4")},
                {"TEMP_INNER", new KeyValuePair<string, string>("Form_6_1", "1:4")},
                {"OTOPL_VENT_EL", new KeyValuePair<string, string>("Form_6_2", "7:4")},
                {"TM_11", new KeyValuePair<string, string>("Form_6_4", "2:4")},
                {"TM_12", new KeyValuePair<string, string>("Form_6_4", "2:5")},
                {"TM_21", new KeyValuePair<string, string>("Form_6_4", "3:4")},
                {"TM_22", new KeyValuePair<string, string>("Form_6_4", "3:5")},
                {"TM_31", new KeyValuePair<string, string>("Form_6_4", "4:4")},
                {"TM_32", new KeyValuePair<string, string>("Form_6_4", "4:5")},
                {"TM_41", new KeyValuePair<string, string>("Form_6_4", "5:4")},
                {"TM_42", new KeyValuePair<string, string>("Form_6_4", "5:5")},
                {"UMH_11", new KeyValuePair<string, string>("Form_6_4", "18:4")},
                {"UMH_12", new KeyValuePair<string, string>("Form_6_4", "18:5")},
                {"UMH_21", new KeyValuePair<string, string>("Form_6_4", "19:4")},
                {"UMH_22", new KeyValuePair<string, string>("Form_6_4", "19:5")},
                /*{"UR_11", new KeyValuePair<string, string>("", "")},
                {"UR_12", new KeyValuePair<string, string>("", "")},  пока не грузим */
                {"UR_21", new KeyValuePair<string, string>("Form_6_3", "1:4")},
                {"UR_22", new KeyValuePair<string, string>("Form_6_3", "1:5")},
                {"UR_31", new KeyValuePair<string, string>("Form_6_3", "2:4")},
                {"UR_32", new KeyValuePair<string, string>("Form_6_3", "2:5")},
                {"UR_41", new KeyValuePair<string, string>("Form_6_3", "3:4")},
                {"UR_42", new KeyValuePair<string, string>("Form_6_3", "3:5")},
                {"UR_51", new KeyValuePair<string, string>("Form_6_3", "4:4")},
                {"UR_52", new KeyValuePair<string, string>("Form_6_3", "4:5")},
                {"UR_61", new KeyValuePair<string, string>("Form_6_3", "5:4")},
                {"UR_62", new KeyValuePair<string, string>("Form_6_3", "5:5")},
                {"UR_71", new KeyValuePair<string, string>("Form_6_3", "6:4")},
                {"UR_72", new KeyValuePair<string, string>("Form_6_3", "6:5")},
                {"UR_81", new KeyValuePair<string, string>("Form_6_3", "7:4")},
                {"UR_82", new KeyValuePair<string, string>("Form_6_3", "7:5")},
                {"VOD_KAN", new KeyValuePair<string, string>("Form_6_2", "8:4")},
                {"NATURE_GAZ", new KeyValuePair<string, string>("Form_6_2", "9:4")}
            };
        }

        protected override TehPassportValueProxy GetCorrectValue(string formCode, string cellCode, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                // Сюда попадем, только если ReplaceData == true
                return new TehPassportValueProxy {FormCode = formCode, CellCode = cellCode, Value = value};
            }

            var formCodeCellCode = formCode + "#" + cellCode;

            bool isValid = true;
            switch (formCodeCellCode)
            {
                #region Decimal
                case "Form_6_4#11:4":
                case "Form_6_4#11:5":
                case "Form_6_2#1:4":
                case "Form_6_2#10:4":
                case "Form_6_4#7:4":
                case "Form_6_4#7:5":
                case "Form_6_4#8:4":
                case "Form_6_4#8:5":
                case "Form_6_4#9:4":
                case "Form_6_4#9:5":
                case "Form_6_4#10:4":
                case "Form_6_4#10:5":
                case "Form_6_1#6:4":
                case "Form_6_4#20:4":
                case "Form_6_4#20:5":
                case "Form_6_1#5:4":
                case "Form_6_2#2:4":
                case "Form_6_2#4:4":
                case "Form_6_2#5:4":
                case "Form_6_2#6:4":
                case "Form_6_4#13:4":
                case "Form_6_4#13:5":
                case "Form_6_4#14:4":
                case "Form_6_4#14:5":
                case "Form_6_4#15:4":
                case "Form_6_4#15:5":
                case "Form_6_4#16:4":
                case "Form_6_4#16:5":
                case "Form_6_1#4:4":
                case "Form_6_1#2:4":
                case "Form_6_1#3:4":
                case "Form_6_1#1:4":
                case "Form_6_2#7:4":
                case "Form_6_4#2:4":
                case "Form_6_4#2:5":
                case "Form_6_4#3:4":
                case "Form_6_4#3:5":
                case "Form_6_4#4:4":
                case "Form_6_4#4:5":
                case "Form_6_4#5:4":
                case "Form_6_4#5:5":
                case "Form_6_4#18:4":
                case "Form_6_4#18:5":
                case "Form_6_4#19:4":
                case "Form_6_4#19:5":
                case "Form_6_3#1:4":
                case "Form_6_3#1:5":
                case "Form_6_3#2:4":
                case "Form_6_3#2:5":
                case "Form_6_3#3:4":
                case "Form_6_3#3:5":
                case "Form_6_3#4:4":
                case "Form_6_3#4:5":
                case "Form_6_3#5:4":
                case "Form_6_3#5:5":
                case "Form_6_3#6:4":
                case "Form_6_3#6:5":
                case "Form_6_3#7:4":
                case "Form_6_3#7:5":
                case "Form_6_2#8:4":
                case "Form_6_2#9:4":
                #endregion
                    isValid = this.isDecimal(value);
                    break;

                #region Int
                case "Form_6_5#2:3":
                case "Form_6_5#2:5":
                case "Form_6_5#3:3":
                case "Form_6_5#3:5":
                case "Form_6_5#5:3":
                case "Form_6_5#5:5":
                case "Form_6_5#6:3":
                case "Form_6_5#6:5":
                case "Form_6_5#7:3":
                case "Form_6_5#7:5":
                case "Form_6_5#8:3":
                case "Form_6_5#8:5":
                case "Form_6_5#9:3":
                case "Form_6_5#9:5":
                #endregion
                    isValid = this.isInt(value);
                    break;
            }

            if (!isValid)
            {
                return null;
            }

            return new TehPassportValueProxy {FormCode = formCode, CellCode = cellCode, Value = value};
        }
    }
}