namespace Bars.Gkh.Import.FundRealtyObjects.ExtraDataImport.Impl
{
    using System.Collections.Generic;

    using Bars.Gkh.Enums;
    using Bars.Gkh.Import.FundRealtyObjects.Impl;

    using System.Linq;

    public class Part3 :  BaseExtraDataImport, IExtraDataImport
    {
        public string Code
        {
            get
            {
                return "RASP_PART_3";
            }
        }

        protected override void InitFields()
        {
            FieldNames = new List<string>
                {
                    "AUTONOM_FLG1",
                    "CENTRAL_FLG1"
                };
        }

        protected override void AddData(ExtraDataProxy extraData, Dictionary<string, string> data)
        {
            if (data.ContainsKey(FieldNames.First()))
            {
                var value = data[FieldNames.First()];

                if (value == "1" || value.ToUpper() == "TRUE")
                {
                    extraData.HeatingSystem = HeatingSystem.Individual;
                }
            }

            if (data.ContainsKey(FieldNames.Last()))
            {
                var value = data[FieldNames.Last()];

                if (value == "1" || value.ToUpper() == "TRUE")
                {
                    extraData.HeatingSystem = HeatingSystem.Centralized;
                }
            }
        }
    }
}