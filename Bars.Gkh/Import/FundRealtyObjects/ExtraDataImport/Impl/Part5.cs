namespace Bars.Gkh.Import.FundRealtyObjects.ExtraDataImport.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Enums;
    using Bars.Gkh.Import.FundRealtyObjects.Impl;

    public class Part5 :  BaseExtraDataImport, IExtraDataImport
    {
        public string Code
        {
            get
            {
                return "RASP_PART_5";
            }
        }

        protected override void InitFields()
        {
            FieldNames = new List<string> { "KROV" };
        }

        protected override void AddData(ExtraDataProxy extraData, Dictionary<string, string> data)
        {
            if (data.ContainsKey(FieldNames.First()))
            {
                var value = data[FieldNames.First()];

                switch (value)
                {
                    case "1": 
                        extraData.TypeRoof = TypeRoof.Plane;
                        break;
                    case "2":
                        extraData.TypeRoof = TypeRoof.Pitched;
                        break;
                }
            }
        }
    }
}