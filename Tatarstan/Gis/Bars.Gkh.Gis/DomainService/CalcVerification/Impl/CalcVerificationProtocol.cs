namespace Bars.Gkh.Gis.DomainService.CalcVerification.Impl
{
    using B4;
    using Castle.Windsor;
    using Intf;

    public class CalcVerificationProtocol : ICalcVerificationProtocol
    {
        protected IWindsorContainer Container;
        protected BillingInstrumentary BillingInstrumentary;
        public CalcVerificationProtocol(IWindsorContainer container, BillingInstrumentary billingInstrumentary)
        {
            Container = container;
            BillingInstrumentary = billingInstrumentary;
        }

        public string GetProtocol(BaseParams param)
        {
            var nzp_kvar = (int)param.Params.GetAs<long>("personalAccountId");
            var nzp_serv = (int)param.Params.GetAs<long>("serviceId");
            var nzp_supp = (int)param.Params.GetAs<long>("supplierId");
            var billing_house_id = param.Params.GetAs<long>("billingHouseId");
            var isChd = param.Params.GetAs<bool>("isGis");
            var year = param.Params.GetAs<int>("year");
            var month = param.Params.GetAs<int>("month");
            var TableCharge = "chd_charge_" + (year % 2000) + ".chd_charge_" + month.ToString("00");

            var measure = BillingInstrumentary.ExecScalar<decimal>(string.Format("SELECT nzp_measure FROM {0} " +
                    " WHERE billing_house_code={1} " +
                    " AND nzp_kvar={2} AND nzp_serv={3} AND nzp_supp={4}",
                    TableCharge, billing_house_id, nzp_kvar, nzp_serv, nzp_supp));

            decimal tariff = 0;
            decimal consumption = 0;
            if (isChd)
            {
                tariff = BillingInstrumentary.ExecScalar<decimal>(string.Format("SELECT tarif_chd FROM {0} " +
                    " WHERE billing_house_code={1} " +
                    " AND nzp_kvar={2} AND nzp_serv={3} AND nzp_supp={4}",
                    TableCharge, billing_house_id, nzp_kvar, nzp_serv, nzp_supp));
                consumption = BillingInstrumentary.ExecScalar<decimal>(string.Format("SELECT c_calc_chd FROM {0} " +
                " WHERE billing_house_code={1} " +
                " AND nzp_kvar={2} AND nzp_serv={3} AND nzp_supp={4}",
                TableCharge, billing_house_id, nzp_kvar, nzp_serv, nzp_supp));
            }
            else
            {
                tariff = BillingInstrumentary.ExecScalar<decimal>(string.Format("SELECT tarif FROM {0} " +
                 " WHERE billing_house_code={1} " +
                 " AND nzp_kvar={2} AND nzp_serv={3} AND nzp_supp={4}",
                 TableCharge, billing_house_id, nzp_kvar, nzp_serv, nzp_supp));
                consumption = BillingInstrumentary.ExecScalar<decimal>(string.Format("SELECT c_calc FROM {0} " +
                " WHERE billing_house_code={1} " +
                " AND nzp_kvar={2} AND nzp_serv={3} AND nzp_supp={4}",
                TableCharge, billing_house_id, nzp_kvar, nzp_serv, nzp_supp));
            }
            var range = measure == 4 ? 6 : 2;
            var res = "Начислено (" + decimal.Round(tariff * consumption, range) + " руб.)  = Тариф (" + decimal.Round(tariff, range) + ") * Расход (" + decimal.Round(consumption, range) + ") ";

            return res;
        }
    }
}
