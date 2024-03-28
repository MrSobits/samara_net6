namespace Bars.Gkh.Gis.DomainService.CalcVerification.Impl
{
    using System.Data;
    using B4;

    using Bars.Gkh.Gis.KP_legacy;

    using Entities.CalcVerification;
    using Intf;

    public class CalcChargeHandler : ICalcCharge
    {
        protected IDbConnection Connection;
        protected BillingInstrumentary BillingInstrumentary;
        protected CalcVerificationParams Param;


        public CalcChargeHandler(IDbConnection connection, BillingInstrumentary billingInstrumentary, CalcVerificationParams param)
        {
            Connection = connection;
            BillingInstrumentary = billingInstrumentary;
            Param = param;
        }

        /// <summary>
        /// Сохранение начислений
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="TableMOCharges"></param>
        /// <returns></returns>
        public IDataResult CalcCharge(ref CalcTypes.ParamCalc Params, string TableMOCharges)
        {
            var chargeInfo = new CalcTypes.ChargeXX(Params);
            var targetTable = chargeInfo.charge_xx.Replace(chargeInfo.charge_tab, "chd_" + chargeInfo.charge_tab);
            var servOdnTable = string.Format("{0}_kernel.serv_odn", Param.Pref);
            var gkuTable = string.Format("{0}_charge_{1}.chd_calc_gku_{2}", Params.pref, Params.calc_yy % 1000, Params.calc_mm.ToString("00"));

            var sql = string.Format(" INSERT INTO {0}(billing_house_code,chd_point_id,nzp_dom,nzp_kvar,nzp_serv,nzp_supp," +
                                 "   nzp_frm,is_device,tarif,c_calc,rsum_tarif,sum_nedop,sum_real,reval,real_charge,sum_insaldo,sum_money," +
                                 "   sum_outsaldo,sum_charge, gil, squ1, rash_norm_one," +
                                 "   tarif_chd,c_calc_chd,rsum_tarif_chd,sum_nedop_chd,sum_real_chd,reval_chd,sum_outsaldo_chd, rash_norm_one_chd) " +
                                 " SELECT {1},0,ch.nzp_dom,ch.nzp_kvar,ch.nzp_serv,ch.nzp_supp,ch.nzp_frm," +

                                 " (CASE " +
                                 "      WHEN so.nzp_serv IS NOT NULL THEN 3 " +
                                 "      WHEN so.nzp_serv IS NULL AND ch.is_device IN (1,3,5,7) THEN 2 " +
                                 "      ELSE 1 " +
                                 " END) as is_device," +

                                 "   ch.tarif, g.rashod,ch.rsum_tarif,ch.sum_nedop,ch.sum_real,ch.reval,ch.real_charge,ch.sum_insaldo," +
                                 "   ch.sum_money,ch.sum_outsaldo,ch.sum_charge, g.gil, g.squ1,g.rash_norm_one," +
                //блок ЦХД
                                 "   g.tarif_chd,g.rashod_chd,g.tarif_chd * g.rashod_chd as rsum_tarif_chd, " +
                                 "   ch.sum_nedop as sum_nedop_chd," +
                                 "   g.tarif_chd * g.rashod_chd - ch.sum_nedop as sum_real_chd," +
                                 "   ch.reval as reval_chd," +
                                 "   ch.sum_insaldo + (g.tarif_chd * g.rashod_chd - ch.sum_nedop) + ch.reval + ch.real_charge - ch.sum_money," +
                                 "   g.rash_norm_one as rash_norm_one_chd" +
                                 " FROM {3} g, {2} ch left outer join {4} so on so.nzp_serv = ch.nzp_serv  " +
                                 " WHERE ch.nzp_kvar = g.nzp_kvar " +
                                 " AND ch.nzp_serv = g.nzp_serv " +
                                 " AND ch.nzp_supp = g.nzp_supp " +
                                 " AND ch.nzp_frm=g.nzp_frm ",
                                 targetTable, Param.BillingHouseCode, TableMOCharges, gkuTable, servOdnTable);
            BillingInstrumentary.ExecSQL(sql);

            return new BaseDataResult();
        }
    }

    public class CalcChargeHandlerOverride : ICalcCharge
    {
        protected IDbConnection Connection;
        protected BillingInstrumentary BillingInstrumentary;
        protected CalcVerificationParams Param;


        public CalcChargeHandlerOverride(IDbConnection connection, BillingInstrumentary billingInstrumentary, CalcVerificationParams param)
        {
            Connection = connection;
            BillingInstrumentary = billingInstrumentary;
            Param = param;
        }

        /// <summary>
        /// Сохранение начислений
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="TableMOCharges"></param>
        /// <returns></returns>
        public IDataResult CalcCharge(ref CalcTypes.ParamCalc Params, string TableMOCharges)
        {
            var chargeInfo = new CalcTypes.ChargeXX(Params);
            chargeInfo.charge_xx = string.Format("{0}{1}{2}", Param.Pref, DBManager.tableDelimiter, chargeInfo.charge_tab);
            var targetTable = chargeInfo.charge_xx.Replace(chargeInfo.charge_tab, "chd_" + chargeInfo.charge_tab);
            var servOdnTable = string.Format("{0}.serv_odn", Param.Pref);
            var gkuTable = string.Format("{0}.chd_calc_gku_{1}", Params.pref, Params.calc_mm.ToString("00"));

            var sql = string.Format(" INSERT INTO {0}(billing_house_code,chd_point_id,nzp_dom,nzp_kvar,nzp_serv,nzp_supp," +
                                 "   nzp_frm,is_device,tarif,c_calc,rsum_tarif,sum_nedop,sum_real,reval,real_charge,sum_insaldo,sum_money," +
                                 "   sum_outsaldo,sum_charge, gil, squ1, rash_norm_one," +
                                 "   tarif_chd,c_calc_chd,rsum_tarif_chd,sum_nedop_chd,sum_real_chd,reval_chd,sum_outsaldo_chd, rash_norm_one_chd) " +
                                 " SELECT {1},0,ch.nzp_dom,ch.nzp_kvar,ch.nzp_serv,ch.nzp_supp,ch.nzp_frm," +

                                 " (CASE " +
                                 "      WHEN so.nzp_serv IS NOT NULL THEN 3 " +
                                 "      WHEN so.nzp_serv IS NULL AND ch.is_device IN (1,3,5,7) THEN 2 " +
                                 "      ELSE 1 " +
                                 " END) as is_device," +

                                 "   ch.tarif, g.rashod,ch.rsum_tarif,ch.sum_nedop,ch.sum_real,ch.reval,ch.real_charge,ch.sum_insaldo," +
                                 "   ch.sum_money,ch.sum_outsaldo,ch.sum_charge, g.gil, g.squ1,g.rash_norm_one," +
                //блок ЦХД
                                 "   g.tarif_chd,g.rashod_chd,g.tarif_chd * g.rashod_chd as rsum_tarif_chd, " +
                                 "   ch.sum_nedop as sum_nedop_chd," +
                                 "   g.tarif_chd * g.rashod_chd - ch.sum_nedop as sum_real_chd," +
                                 "   ch.reval as reval_chd," +
                                 "   ch.sum_insaldo + (g.tarif_chd * g.rashod_chd - ch.sum_nedop) + ch.reval + ch.real_charge - ch.sum_money," +
                                 "   g.rash_norm_one as rash_norm_one_chd" +
                                 " FROM {3} g, {2} ch left outer join {4} so on so.nzp_serv = ch.nzp_serv  " +
                                 " WHERE ch.nzp_kvar = g.nzp_kvar " +
                                 " AND ch.nzp_serv = g.nzp_serv " +
                                 " AND ch.nzp_supp = g.nzp_supp " +
                                 " AND ch.nzp_frm=g.nzp_frm ",
                                 targetTable, Param.BillingHouseCode, TableMOCharges, gkuTable, servOdnTable);
            BillingInstrumentary.ExecSQL(sql);

            return new BaseDataResult();
        }
    }
}
