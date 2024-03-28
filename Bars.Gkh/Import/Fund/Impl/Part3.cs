namespace Bars.Gkh.Import.Fund.Impl
{
    using System.Collections.Generic;

    public class Part3 : BaseTechPassportPartImport, ITechPassportPartImport
    {
        public static string Code
        {
            get
            {
                return "RASP_PART_3";
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

        private void ConstructionChute(ref string value, ref bool isValid, ref string cellCode)
        {
            isValid = value == "1";

            if (!isValid)
            {
                return;
            }
            value = "-1";
            switch (cellCode)
            {
                case "100":
                    value = "1";
                    break;
                case "200":
                    value = "0";
                    break;
            }

            cellCode = "1:3";
            isValid = value != "-1";
        }

        private void TypeBool(ref string value, ref bool isValid, ref string cellCode)
        {
            isValid = value == "1";

            if (!isValid)
            {
                return;
            }
            value = "-1";
            switch (cellCode)
            {
                case "100":
                    value = "1";
                    break;
                case "200":
                    value = "0";
                    break;
            }

            cellCode = "1:3";
            isValid = value != "-1";
        }

        private void TypeColdWater(ref string value, ref bool isValid, ref string cellCode)
        {
            isValid = value == "1";

            if (!isValid)
            {
                return;
            }
            value = "-1";
            switch (cellCode)
            {
                case "100":
                    value = "1";
                    break;
                case "300":
                    value = "3";
                    break;
            }

            cellCode = "1:3";
            isValid = value != "-1";
        }

        private void TypeDrainage(ref string value, ref bool isValid, ref string cellCode)
        {
            isValid = value == "1";

            if (!isValid)
            {
                return;
            }

            value = "-1";
            switch (cellCode)
            {
                case "100":
                    value = "1";
                    break;
                case "200":
                    value = "2";
                    break;
                case "300":
                    value = "3";
                    break;
            }

            cellCode = "1:3";
            isValid = value != "-1";
        }

        private void TypeGas(ref string value, ref bool isValid, ref string cellCode)
        {
            isValid = value == "1";

            if (!isValid)
            {
                return;
            }
            value = "-1";
            switch (cellCode)
            {
                case "100":
                    value = "1";
                    break;
                case "300":
                    value = "3";
                    break;
            }

            cellCode = "1:3";
            isValid = value != "-1";
        }

        private void TypeHeating(ref string value, ref bool isValid, ref string cellCode)
        {
            isValid = value == "1";

            if (!isValid)
            {
                return;
            }
            value = "-1";
            switch (cellCode)
            {
                case "100":
                    value = "1";
                    break;
                case "200":
                    value = "2";
                    break;
                case "400":
                    value = "4";
                    break;
                case "500":
                    value = "5";
                    break;
            }

            cellCode = "1:3";
            isValid = value != "-1";
        }

        private void TypePower(ref string value, ref bool isValid, ref string cellCode)
        {
            isValid = value == "1";

            if (!isValid)
            {
                return;
            }
            value = "-1";
            switch (cellCode)
            {
                case "100":
                    value = "1";
                    break;
                case "300":
                    value = "3";
                    break;
            }

            cellCode = "1:3";
            isValid = value != "-1";
        }

        private void TypeSewage(ref string value, ref bool isValid, ref string cellCode)
        {
            isValid = value == "1";

            if (!isValid)
            {
                return;
            }

            value = "-1";
            switch (cellCode)
            {
                case "100":
                    value = "1";
                    break;
                case "300":
                    value = "3";
                    break;
            }

            cellCode = "1:3";
            isValid = value != "-1";
        }

        private void TypeVentilation(ref string value, ref bool isValid, ref string cellCode)
        {
            isValid = value == "1";

            if (!isValid)
            {
                return;
            }
            value = "-1";
            switch (cellCode)
            {
                case "100":
                    value = "1";
                    break;
                case "200":
                    value = "2";
                    break;
                case "300":
                    value = "3";
                    break;
                case "400":
                    value = "4";
                    break;
            }

            cellCode = "1:3";
            isValid = value != "-1";
        }

        protected override void InitDictCodes()
        {
            dictCodes = new Dictionary<string, KeyValuePair<string, string>>
            {
                { "AUTONOM_FLG1", new KeyValuePair<string, string>("Form_3_1", "200") },
                { "CENTRAL_FLG1", new KeyValuePair<string, string>("Form_3_1", "100") },
                { "MISSED_FLG1", new KeyValuePair<string, string>("Form_3_1", "500") },
                { "PECHN_FLG1", new KeyValuePair<string, string>("Form_3_1", "400") },
                
                { "VODIN_FLG5", new KeyValuePair<string, string>("Form_3_6", "200") },
                { "VODMISSED_FLG5", new KeyValuePair<string, string>("Form_3_6", "300") },
                { "VODOUT_FLG5", new KeyValuePair<string, string>("Form_3_6", "100") },
                
                { "SEW_ABSENT", new KeyValuePair<string, string>("Form_3_3_Water", "300") },
                { "SEW_CENTR", new KeyValuePair<string, string>("Form_3_3_Water", "100") },
                
                { "PZKL1_FLG5", new KeyValuePair<string, string>("Form_3_7_2", "100") },
                { "PZKL2_FLG5", new KeyValuePair<string, string>("Form_3_7_2", "200") },
                { "PZKL3_FLG5", new KeyValuePair<string, string>("Form_3_7_2", "300") },
                
                { "MISSED_FLG5", new KeyValuePair<string, string>("Form_3_5", "400") },
                { "PRITOK_FLG5", new KeyValuePair<string, string>("Form_3_5", "100") },
                { "PRVIT_FLG5", new KeyValuePair<string, string>("Form_3_5", "300") },
                { "VITYAJ_FLG5", new KeyValuePair<string, string>("Form_3_5", "200") },
                
                { "AUTONOM_FLG2", new KeyValuePair<string, string>("Form_3_2", "200") },
                { "CENTRAL_FLG_2", new KeyValuePair<string, string>("Form_3_2", "100") },
                { "MISSED_FLG_2", new KeyValuePair<string, string>("Form_3_2", "600") },
                { "PECHN_FLG2", new KeyValuePair<string, string>("Form_3_2", "500") },
                
                { "CENTRAL_FLG4", new KeyValuePair<string, string>("Form_3_4", "100") },
                { "MISSED_FLG4", new KeyValuePair<string, string>("Form_3_4", "300") },

                { "CENTRAL_FLG3", new KeyValuePair<string, string>("Form_3_3", "100") },
                { "MISSED_FLG3", new KeyValuePair<string, string>("Form_3_3", "300") },
                
                { "HSV_LPODV", new KeyValuePair<string, string>("Form_3_1_3", "3:1") },
                //{ "LONG_POD", new KeyValuePair<string, string>("Form_3_1_3", "3:1") },
                //{ "LONG_R", new KeyValuePair<string, string>("Form_3_1_3", "3:1") },
                
                { "HSV_LKV", new KeyValuePair<string, string>("Form_3_1_3", "4:1") },
                //{ "LONG_KV", new KeyValuePair<string, string>("Form_3_1_3", "4:1") },
                
                { "HSV_LONG", new KeyValuePair<string, string>("Form_3_1_3", "1:1") },
                //{ "LONG_H", new KeyValuePair<string, string>("Form_3_1_3", "1:1") },
                
                //{ "COUNT_V", new KeyValuePair<string, string>("Form_3_2_3", "7:1") },
                { "HSV_PVENT", new KeyValuePair<string, string>("Form_3_2_3", "7:1") },
                
                //{ "COUNT_ST", new KeyValuePair<string, string>("Form_3_1_3", "2:1") },
                { "HSV_COUNT", new KeyValuePair<string, string>("Form_3_1_3", "2:1") },
                //{ "SEW_COUNT", new KeyValuePair<string, string>("Form_3_1_3", "2:1") },

                { "TRASH_FLD5", new KeyValuePair<string, string>("Form_3_7", "200") },
                { "TRMISSED_FLG5", new KeyValuePair<string, string>("Form_3_7", "100") },

                { "HVS_ABSENT", new KeyValuePair<string, string>("Form_3_2_CW", "300") },
                { "HVS_CENTR", new KeyValuePair<string, string>("Form_3_2_CW", "100") },

                { "UU_2", new KeyValuePair<string, string>("Form_3_2_2", "2:3") },
                { "PU_E", new KeyValuePair<string, string>("Form_3_3_2", "1:3") },

                { "VODA", new KeyValuePair<string, string>("Form_3_3_3", "1:1") },
                { "COUNT_G", new KeyValuePair<string, string>("Form_3_3_2", "1:4") },
                { "COUNT_K", new KeyValuePair<string, string>("Form_3_2_3", "8:1") },
                { "COUNT_KV_R", new KeyValuePair<string, string>("Form_3_1_3", "7:1") },
                { "COUNT_L_R", new KeyValuePair<string, string>("Form_3_1_3", "6:1") },
                { "COUNT_SV_N", new KeyValuePair<string, string>("Form_3_3_3", "10:1") },
                { "COUNT_SV_DRL", new KeyValuePair<string, string>("Form_3_3_3", "11:1") },
                { "COUNT_OFF", new KeyValuePair<string, string>("Form_3_3_3", "12:1") },
                { "COUNT_ST_GOR", new KeyValuePair<string, string>("Form_3_2_3", "2:1") },
                { "GR_E", new KeyValuePair<string, string>("Form_3_3_3", "2:1") },
                { "GR_E_C", new KeyValuePair<string, string>("Form_3_3_3", "3:1") },
                { "LONG_LIGHT", new KeyValuePair<string, string>("Form_3_3_3", "4:1") },
                { "COUNT_Z", new KeyValuePair<string, string>("Form_3_2_3", "6:1") },
                { "ELEV", new KeyValuePair<string, string>("Form_3_1_3", "15:1") },
                { "LONG_LIFT", new KeyValuePair<string, string>("Form_3_3_3", "5:1") },
                { "COUNT_SCH", new KeyValuePair<string, string>("Form_3_3_3", "6:1") },
                { "HSV_LRAZV", new KeyValuePair<string, string>("Form_3_1_3", "5:1") },
                { "HSV_UPKU", new KeyValuePair<string, string>("Form_3_2CW_2", "1:3") },
                { "HSV_UUU", new KeyValuePair<string, string>("", "") },
                { "HSV_VENT", new KeyValuePair<string, string>("", "") },
                { "HSV_VODOM", new KeyValuePair<string, string>("", "") },
                { "IZOL", new KeyValuePair<string, string>("Form_3_1_3", "9:1") },
                { "KAL", new KeyValuePair<string, string>("Form_3_1_3", "10:1") },
                { "KONV", new KeyValuePair<string, string>("Form_3_1_3", "11:1") },
                { "KOR", new KeyValuePair<string, string>("Form_3_1_3", "16:1") },
                { "KRAN", new KeyValuePair<string, string>("Form_3_1_3", "14:1") },
                { "LONG_G", new KeyValuePair<string, string>("Form_3_4_2", "1:1") },
                { "LONG_GOR", new KeyValuePair<string, string>("Form_3_2_3", "1:1") },
                { "LONG_KV_GOR", new KeyValuePair<string, string>("Form_3_2_3", "4:1") },
                { "COUNT_2_TAR", new KeyValuePair<string, string>("Form_3_3_3", "7:1") },
                { "COUNT_NOM", new KeyValuePair<string, string>("Form_3_3_3", "8:1") },
                { "LONG_POD_GOR", new KeyValuePair<string, string>("Form_3_2_3", "3:1") },
                { "MUS_COUNT", new KeyValuePair<string, string>("Form_3_7_3", "1:1") },
                { "MUS_KAM", new KeyValuePair<string, string>("Form_3_7_3", "3:1") },
                { "MUS_KAM_COUNT", new KeyValuePair<string, string>("Form_3_7_3", "2:1") },
                { "MUS_VAL", new KeyValuePair<string, string>("Form_3_7_3", "4:1") },
                { "PU_1", new KeyValuePair<string, string>("Form_3_1_2", "1:3") },
                { "PU_2", new KeyValuePair<string, string>("Form_3_2_2", "1:3") },
                { "SEW_KRISHK", new KeyValuePair<string, string>("", "") },
                { "SEW_LONG", new KeyValuePair<string, string>("", "") },
                { "SEW_LSTOYAK", new KeyValuePair<string, string>("Form_3_3_Water_2", "1:1") },
                { "SEW_STOYAK", new KeyValuePair<string, string>("", "") },
                { "TEPLO", new KeyValuePair<string, string>("Form_3_1_3", "17:1") },
                { "UU_1", new KeyValuePair<string, string>("Form_3_1_2", "2:3") },
                { "VENT", new KeyValuePair<string, string>("Form_3_1_3", "13:1") },
                { "COUNT_SV", new KeyValuePair<string, string>("Form_3_3_3", "9:1") },
                { "ZADV", new KeyValuePair<string, string>("Form_3_1_3", "12:1") },
                { "ZAPOR", new KeyValuePair<string, string>("Form_3_1_3", "8:1") }
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
                case "Form_3_2CW_2#1:3":
                case "Form_3_1_2#1:3":
                case "Form_3_2_2#1:3":
                case "Form_3_1_2#2:3":
                case "Form_3_2_2#2:3":
                case "Form_3_3_2#1:3":
                    isValid = this.isBool(value);
                    break;

                case "Form_3_3_3#4:1":
                case "Form_3_3_3#5:1":
                case "Form_3_1_3#4:1":
                case "Form_3_1_3#1:1":
                case "Form_3_1_3#3:1":
                case "Form_3_1_3#5:1":
                case "Form_3_1_3#9:1":
                case "Form_3_4_2#1:1":
                case "Form_3_2_3#1:1":
                case "Form_3_2_3#4:1":
                case "Form_3_2_3#3:1":
                case "Form_3_7_3#3:1":
                case "Form_3_7_3#4:1":
                case "Form_3_3_Water_2#1:1":
                    isValid = this.isDecimal(value);
                    break;

                case "Form_3_3_3#1:1":
                case "Form_3_3_2#1:4":
                case "Form_3_2_3#8:1":
                case "Form_3_1_3#7:1":
                case "Form_3_1_3#6:1":
                case "Form_3_3_3#10:1":
                case "Form_3_3_3#11:1":
                case "Form_3_3_3#12:1":
                case "Form_3_1_3#2:1":
                case "Form_3_2_3#2:1":
                case "Form_3_3_3#2:1":
                case "Form_3_3_3#3:1":
                case "Form_3_2_3#7:1":
                case "Form_3_2_3#6:1":
                case "Form_3_1_3#15:1":
                case "Form_3_3_3#6:1":
                case "Form_3_1_3#10:1":
                case "Form_3_1_3#11:1":
                case "Form_3_1_3#16:1":
                case "Form_3_1_3#14:1":
                case "Form_3_3_3#7:1":
                case "Form_3_3_3#8:1":
                case "Form_3_7_3#1:1":
                case "Form_3_7_3#2:1":
                case "Form_3_1_3#17:1":
                case "Form_3_1_3#13:1":
                case "Form_3_3_3#9:1":
                case "Form_3_1_3#12:1":
                case "Form_3_1_3#8:1":
                    isValid = this.isInt(value);
                    break;

                case "Form_3_7_2#1:3":
                case "Form_3_7_2#100":
                case "Form_3_7_2#200":
                    ConstructionChute(ref value, ref isValid, ref cellCode);
                    break;

                case "Form_3_7#1:3":
                case "Form_3_7#100":
                case "Form_3_7#200":
                    TypeBool(ref value, ref isValid, ref cellCode);
                    break;

                case "Form_3_2_CW#1:3":
                case "Form_3_2_CW#100":
                case "Form_3_2_CW#200":
                    TypeColdWater(ref value, ref isValid, ref cellCode);
                    break;

                case "Form_3_6#1:3":
                case "Form_3_6#100":
                case "Form_3_6#200":
                case "Form_3_6#300":
                    TypeDrainage(ref value, ref isValid, ref cellCode);
                    break;

                case "Form_3_4#1:3":
                case "Form_3_4#100":
                case "Form_3_4#300":
                    TypeGas(ref value, ref isValid, ref cellCode);
                    break;

                case "Form_3_1#1:3":
                case "Form_3_1#100":
                case "Form_3_1#400":
                case "Form_3_1#500":
                case "Form_3_1#200":
                    TypeHeating(ref value, ref isValid, ref cellCode);
                    break;

                case "Form_3_2#1:3":
                case "Form_3_2#100":
                case "Form_3_2#200":
                case "Form_3_2#500":
                case "Form_3_2#600":
                    TypeHeating(ref value, ref isValid, ref cellCode);
                    break;

                case "Form_3_3#1:3":
                case "Form_3_3#100":
                case "Form_3_3#300":
                    TypePower(ref value, ref isValid, ref cellCode);
                    break;

                case "Form_3_3_Water#1:3":
                case "Form_3_3_Water#100":
                case "Form_3_3_Water#300":
                    TypeSewage(ref value, ref isValid, ref cellCode);
                    break;

                case "Form_3_5#1:3":
                case "Form_3_5#100":
                case "Form_3_5#200":
                case "Form_3_5#300":
                case "Form_3_5#400":
                    TypeVentilation(ref value, ref isValid, ref cellCode);
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