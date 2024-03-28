namespace Bars.Gkh.Import.FundRealtyObjects.ExtraDataImport.Impl
{
    using System.Collections.Generic;

    using Bars.Gkh.Import.FundRealtyObjects.Impl;

    using System.Linq;

    public class Part2 :  BaseExtraDataImport, IExtraDataImport
    {
        public string Code
        {
            get
            {
                return "RASP_PART_2";
            }
        }

        protected override void InitFields()
        {
            FieldNames = new List<string>
                {
                    "COUNT_1",
                    "COUNT_2",
                    "COUNT_3",
                    "COUNT_4",
                    "COUNT_5",
                    "COUNT_6",
                    "COUNT_7",
                    "COUNT_K_2",
                    "COUNT_K_3",
                    "COUNT_K_4",
                    "COUNT_K_5",
                    "COUNT_K_6",
                    "COUNT_K_7",
                    "COUNT_O"
                };
        }

        protected override void AddData(ExtraDataProxy extraData, Dictionary<string, string> data)
        {
            int value;

            var anyChange = false;
            var sum = 0;

            foreach (var field in FieldNames.GetRange(0, FieldNames.Count - 1))
            {
                if (data.ContainsKey(field) && int.TryParse(data[field], out value))
                {
                    anyChange = true;
                    sum += value;
                }
            }

            if (anyChange)
            {
                extraData.NumberApartmentsMkd = sum;
            }

            var countHostelRooms = FieldNames.Last();
            if (data.ContainsKey(countHostelRooms) && int.TryParse(data[countHostelRooms], out value))
            {
                extraData.NumberApartmentsHostel = value;
            }
        }
    }
}