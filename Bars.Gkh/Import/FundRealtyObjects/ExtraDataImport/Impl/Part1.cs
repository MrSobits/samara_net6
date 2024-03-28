namespace Bars.Gkh.Import.FundRealtyObjects.ExtraDataImport.Impl
{
    using System.Collections.Generic;

    using Bars.Gkh.Import.FundRealtyObjects.Impl;

    public class Part1 :  BaseExtraDataImport, IExtraDataImport
    {
        public string Code
        {
            get
            {
                return "RASP_PART_1";
            }
        }

        protected override void InitFields()
        {
            FieldNames = new List<string>
                {
                    "COUNT_ENTRANCE",
                    "MAX_ET",
                    "MIN_ET"
                };
        }

        protected override void AddData(ExtraDataProxy extraData, Dictionary<string, string> data)
        {
            int value;
            var countEntrance = FieldNames[0];
            if (data.ContainsKey(countEntrance) && int.TryParse(data[countEntrance], out value))
            {
                extraData.NumberEntrances = value;
            }

            var maxEt = FieldNames[1];
            if (data.ContainsKey(maxEt) && int.TryParse(data[maxEt], out value))
            {
                extraData.MaximumFloors = value;
            }

            var minEt = FieldNames[2];
            if (data.ContainsKey(minEt) && int.TryParse(data[minEt], out value))
            {
                extraData.Floors = value;
            }
        }
    }
}