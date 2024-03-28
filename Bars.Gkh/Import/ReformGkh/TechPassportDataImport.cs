namespace Bars.Gkh.Import.ReformGkh
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Import.RoImport;

    using Castle.Core;

    using NHibernate;

    public class TechPassportDataImport : ITechPassportDataImport, IInitializable
    {
        private List<TehPassport> tehPassportToCreate = new List<TehPassport>();

        private List<TehPassportValue> tehPassportValueToCreate = new List<TehPassportValue>();

        protected sealed class TehPassportValueProxy
        {
            public string FormCode;

            public string CellCode;

            public string Value;
        }

        private Dictionary<string, KeyValuePair<string, string>> techpassportMapping;

        public void Initialize()
        {
            techpassportMapping = new Dictionary<string, KeyValuePair<string, string>>
                {
                    { "П18", new KeyValuePair<string, string>("Form_1", "6:1") },
                    { "П8", new KeyValuePair<string, string>("Form_1", "18:1") },
                    { "П11", new KeyValuePair<string, string>("Form_2", "2:3") },
                    { "П19", new KeyValuePair<string, string>("Form_1", "7:1") },
                    { "П20", new KeyValuePair<string, string>("Form_1", "24:1") },
                    { "П21", new KeyValuePair<string, string>("Form_1", "25:1") },
                    { "П22", new KeyValuePair<string, string>("Form_1", "26:1") },
                    { "П23", new KeyValuePair<string, string>("Form_1_3", "2:1") },
                    { "П25", new KeyValuePair<string, string>("Form_1", "12:1") },
                    {"П65", new KeyValuePair<string, string>("Form_1_3", "3:1")},
                    {"П66", new KeyValuePair<string, string>("Form_1_3", "4:1")},
                    {"П73", new KeyValuePair<string, string>("Form_3_1_3", "20:1")},
                    {"П76", new KeyValuePair<string, string>("Form_3_1_3", "21:1")},
                    {"П77", new KeyValuePair<string, string>("Form_3_2", "1:3")},
                    {"П80", new KeyValuePair<string, string>("Form_3_2_3", "11:1")},
                    {"П81", new KeyValuePair<string, string>("Form_3_2_2", "2:4")},
                    {"П83", new KeyValuePair<string, string>("Form_3_2_3", "12:1")},
                    {"П84", new KeyValuePair<string, string>("Form_3_2_CW", "1:3")},
                    {"П87", new KeyValuePair<string, string>("Form_3_2CW_3", "3:1")},
                    {"П90", new KeyValuePair<string, string>("Form_3_2CW_3", "4:1")},
                    {"П91", new KeyValuePair<string, string>("Form_3_1", "1:3")},
                    {"П94", new KeyValuePair<string, string>("Form_3_3", "1:3")},
                    {"П97", new KeyValuePair<string, string>("Form_3_3_3", "15:1")},
                    {"П98", new KeyValuePair<string, string>("Form_3_3_2", "1:4")},
                    {"П100", new KeyValuePair<string, string>("Form_3_3_3", "16:1")},
                    {"П101", new KeyValuePair<string, string>("Form_3_4", "1:3")},
                    {"П105", new KeyValuePair<string, string>("Form_3_4_2", "7:1")},
                    {"П106", new KeyValuePair<string, string>("Form_3_4_3", "1:4")},
                    {"П108", new KeyValuePair<string, string>("Form_3_4_2", "6:1")},
                    {"П111", new KeyValuePair<string, string>("Form_6_1_2", "1:1")},
                    {"П112", new KeyValuePair<string, string>("Form_6_1_2", "2:1")},
                    {"П113", new KeyValuePair<string, string>("Form_6_1_1", "1:1")},
                    {"П114", new KeyValuePair<string, string>("Form_6_1_1", "2:1")}
                };
        }

        public string AddToSaveList(RealityObject realityObject, Dictionary<string, string> data)
        {
            var log = string.Empty;
            var codeDict = data
                .Where(x => techpassportMapping.ContainsKey(x.Key))
                .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        var mapping = techpassportMapping[x.Key];

                        return new { FormCode = mapping.Key, CellCode = mapping.Value, x.Value };
                    });

            var listToCreate = new List<TehPassportValueProxy>();

            foreach (var item in codeDict.Values)
            {
                var tech = this.GetCorrectValue(item.FormCode, item.CellCode, item.Value);

                if (tech != null)
                {
                    listToCreate.Add(tech);
                }
            }

            if (listToCreate.Count == 0)
            {
                return log;
            }

            var tehPassport = new TehPassport { RealityObject = realityObject, ObjectCreateDate = DateTime.Now, ObjectEditDate = DateTime.Now };
            tehPassportToCreate.Add(tehPassport);

            foreach (var tech in listToCreate)
            {
                var techVal = new TehPassportValue
                {
                    TehPassport = tehPassport,
                    FormCode = tech.FormCode,
                    CellCode = tech.CellCode,
                    Value = tech.Value,
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now
                };
                tehPassportValueToCreate.Add(techVal);
            }

            return log;
        }

        public void SaveData(IStatelessSession session)
        {
            this.tehPassportToCreate.ForEach(x => session.Insert(x));
            this.tehPassportValueToCreate.ForEach(x => session.Insert(x));
        }

        protected TehPassportValueProxy GetCorrectValue(string formCode, string cellCode, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var formCodeCellCode = formCode + "#" + cellCode;

            bool isValid = true;

            switch (formCodeCellCode)
            {
                #region Decimal
                case "Form_1_3#3:1":
                case "Form_6_1_2#1:1":
                case "Form_6_1_2#2:1":
                case "Form_1#6:1":
                case "Form_2#2:3":
                case "Form_1#7:1":
                case "Form_1#24:1":
                case "Form_1#25:1":
                case "Form_1#26:1":
                case "Form_1_3#2:1":
                #endregion
                    isValid = this.isDecimal(value);
                    break;

                #region Int
                case "Form_3_1_3#20:1":
                case "Form_3_2_2#2:4":
                case "Form_3_2_3#11:1":
                case "Form_3_2CW_3#3:1":
                case "Form_3_3_2#1:4":
                case "Form_3_3_3#15:1":
                case "Form_3_4_2#7:1":
                case "Form_3_4_3#1:4":
                case "Form_1#12:1":
                #endregion
                    isValid = this.isInt(value);
                    break;

                #region
                case "Form_1_3#4:1":
                case "Form_6_1_1#2:1":
                #endregion
                    isValid = this.isDate(value);
                    break;

                #region TransferResource
                case "Form_3_1_3#21:1":
                case "Form_3_2_3#12:1":
                case "Form_3_2CW_3#4:1":
                case "Form_3_3_3#16:1":
                case "Form_3_4_2#6:1":
                #endregion
                    isValid = this.isTransferResource(ref value);
                    break;

                case "Form_6_1_1#1:1":
                    isValid = this.isChooseEnergy(ref value);
                    break;


                case "Form_3_2_CW#1:3":
                    isValid = this.isTypeColdWater(ref value);
                    break;
                    
                case "Form_3_3#1:3":
                    isValid = this.isTypePower(ref value);
                    break;

                case "Form_3_4#1:3":
                    isValid = this.isTypeGas(ref value);
                    break;

                case "Form_3_1#1:3":
                    isValid = this.isTypeHeating(ref value);
                    break;

                case "Form_3_2#1:3":
                    isValid = this.isTypeHotWater(ref value);
                    break;

            }

            if (!isValid)
            {
                return null;
            }

            return new TehPassportValueProxy { FormCode = formCode, CellCode = cellCode, Value = value };
        }

        private bool isDate(string value)
        {
            DateTime date;

            return DateTime.TryParse(value, out date);
        }

        private bool isChooseEnergy(ref string value)
        {
            switch (value.ToUpper())
            {
                case "НЕ ПРИСВОЕН":
                    value = "0";
                    return true;
                case "A":
                    value = "1";
                    return true;
                case "B++":
                    value = "2";
                    return true;
                case "B+":
                    value = "3";
                    return true;
                case "B":
                    value = "4";
                    return true;
                case "C":
                    value = "5";
                    return true;
            }

            return false;
        }

        private bool isTypeColdWater(ref string value)
        {
            switch (value.ToLower())
            {
                case "централизованное":
                    value = "1";
                    return true;
                case "автономное":
                    value = "2";
                    return true;
                case "отсутствует":
                    value = "3";
                    return true;
            }

            return false;
        }

        private bool isTypeGas(ref string value)
        {
            switch (value.ToLower())
            {
                case "централизованное":
                    value = "1";
                    return true;
                case "отсутствует":
                    value = "3";
                    return true; 
            }

            return false;
        }

        private bool isTypeHeating(ref string value)
        {
            switch (value.ToLower())
            {
                case "централизованная":
                    value = "1";
                    return true;
                case "автономная":
                    value = "2";
                    return true;
                case "отсутствует":
                    value = "5";
                    return true;
            }
            return false;
        }

        private bool isTypeHotWater(ref string value)
        {
            switch (value.ToLower())
            {
                case "централизованная закрытая":
                case "централизованная открытая":
                    value = "1";
                    return true;
                case "автономная":
                    value = "2";
                    return true;
                case "поквартирная":
                    value = "3";
                    break;
                case "отсутствует":
                    value = "6";
                    return true;
            }
            return false;
        }

        private bool isTypePower(ref string value)
        {

            switch (value.ToLower())
            {
                case "централизованное":
                    value = "1";
                    return true;
                case "комбинированное":
                    value = "3";
                    return true;
                case "отсутствует":
                    value = "2";
                    return true;
            }

            return false;
        }

        private bool isTransferResource(ref string value)
        {
            switch (value.ToLower())
            {
                case "по нормативам или квартирным пу":
                    value = "1";
                    return true;
                case "по показаниям общедомовых пу":
                    value = "2";
                    return true;
                case "нет данных":
                    value = "0";
                    return true;
            }

            return false;
        }

        protected bool isInt(string value)
        {
            int intVal;

            return int.TryParse(value, out intVal);
        }

        protected bool isDecimal(string value)
        {
            decimal decimalVal;
            
            return Decimal.TryParse(
                value.Replace(
                    CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator,
                    CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator),
                out decimalVal);
        }
    }
}