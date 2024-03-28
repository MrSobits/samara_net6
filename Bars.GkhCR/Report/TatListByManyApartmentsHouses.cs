namespace Bars.GkhCr.Report
{
    using System.Collections.Generic;
    using B4.Utils;
    using Gkh.Domain.CollectionExtensions;

    public class TatListByManyApartmentsHouses : GkhCr.Report.ListByManyApartmentsHouses
    {
        public override string RequiredPermission
        {
            get { return "Reports.CR.ListByManyApartmentsHousesTat"; }
        }

        public override string Name
        {
            get { return "Перечень многоквартирных домов (Татарстан)"; }
        }

        public override string Desciption
        {
            get { return "Перечень многоквартирных домов (Татарстан)"; }
        }

        protected override decimal GetPartialCost(DataProxy data)
        {
            return (data.AreaLivNotLivMkd != 0M ? data.Sum / data.AreaLivNotLivMkd : data.Sum).RoundDecimal(2);
        }

        protected override decimal GetPartialCostTotal(List<DataProxy> data)
        {
            var area = data.SafeSum(x => x.AreaLivNotLivMkd);
            var sum = data.SafeSum(x => x.Sum);

            return (area != 0M ? sum / area : sum).RoundDecimal(2);
        }
    }
}
