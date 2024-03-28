using Bars.Gkh.Gis.Entities.CalcVerification;

namespace Bars.Gkh.Gis.DomainService.CalcVerification.Impl
{
    using System;
    using System.Data;
    using B4;

    using Bars.Gkh.Gis.KP_legacy;

    using Castle.Windsor;

    using Intf;

    /// <summary>
    /// Расчет расходов по услугам с применением формул расчета
    /// </summary>
    public class CalcServicesHandler : ICalcServices
    {
        private CalcServicesHandler() { }
        protected IDbConnection Connection;
        protected BillingInstrumentary BillingInstrumentary;
        protected IWindsorContainer Container;
        protected CalcVerificationParams CalcParams;
        protected TempTablesLifeTime TempTablesLifeTime;
        public CalcServicesHandler(IDbConnection connection, IWindsorContainer contariner, BillingInstrumentary billingInstrumentary)
        {
            Connection = connection;
            Container = contariner;
            BillingInstrumentary = billingInstrumentary;
            CalcParams = Container.Resolve<CalcVerificationParams>();
            TempTablesLifeTime = Container.Resolve<TempTablesLifeTime>();
        }

        public IDataResult CalcServices(string TableMOCharges)
        {
            var param = CalcParams.ParamCalc;
            var tenantInfo = new DbCalcCharge.GilecXX(param);
            var consumptionInfo = new CalcTypes.ChargeXX(param);
            var TargetTable = string.Format("{0}_charge_{1}.chd_calc_gku_{2}", param.pref, param.calc_yy % 1000, param.calc_mm.ToString("00"));
            var tempTableCalcServ = "t_calc_gku_" + DateTime.Now.Ticks;
            var sql = " CREATE TEMP TABLE " + tempTableCalcServ + " (LIKE " + TargetTable +
                      " INCLUDING DEFAULTS INCLUDING STORAGE)";
            TempTablesLifeTime.AddTempTable(tempTableCalcServ, sql);

            var tempParamsTable = "t_param_gil_" + DateTime.Now.Ticks;
            sql = " CREATE TEMP TABLE " + tempParamsTable + " AS " +
                  " SELECT nzp as nzp_kvar, MAX(CASE WHEN nzp_prm=4 THEN val_prm END) prm4," +
                  " MAX(CASE WHEN nzp_prm=133 THEN val_prm END) prm133 " +
                  " FROM ttt_prm_1 WHERE nzp_prm IN (4,133)" +
                  " GROUP BY nzp";
            TempTablesLifeTime.AddTempTable(tempParamsTable, sql);

            sql = "CREATE INDEX ix1_" + tempParamsTable + " ON " + tempTableCalcServ + "(nzp_kvar)";
            BillingInstrumentary.ExecSQL(sql);

            var tempTenantsTable = "t_tenants_" + DateTime.Now.Ticks;
            sql = " CREATE TEMP TABLE " + tempTenantsTable + " AS " +
                  " SELECT t.nzp_kvar, MAX(cnt2) gil " +
                  " FROM " + tenantInfo.gil_xx + " g" +
                  " JOIN t_selkvar t ON t.nzp_kvar=g.nzp_kvar " +
                  " WHERE g.stek=3 " +
                  " GROUP BY t.nzp_kvar";
            TempTablesLifeTime.AddTempTable(tempTenantsTable, sql);

            sql = "CREATE INDEX ix1_" + tempTenantsTable + " ON " + tempTenantsTable + "(nzp_kvar)";
            BillingInstrumentary.ExecSQL(sql);

            //получили Гкал.тарифы
            var TableTariffGcal = GetGcalTariffs(TableMOCharges, param);

            const string consumptionField = "(CASE WHEN mo.tarif>0 THEN mo.rsum_tarif/mo.tarif ELSE 0 END)";

            sql = string.Format(" INSERT INTO {0} (chd_counters_id,nzp_wp,nzp_dom,nzp_kvar,nzp_serv," +
                                " nzp_supp,nzp_frm,stek,dat_s,dat_po, is_device,squ1, squ2,gil, tarif, tarif_gkal, rashod,rash_norm_one,valm,dop87, rashod_link, " +
                                " tarif_chd,tarif_gkal_chd,rashod_chd,rash_norm_one_chd, valm_chd, dop87_chd,rashod_link_chd)" +
                                " SELECT 0, 0, mo.nzp_dom, mo.nzp_kvar,mo.nzp_serv,mo.nzp_supp, mo.nzp_frm, 3 stek, {1} dat_s, {2} dat_po, " +
                                " (CASE WHEN mo.is_device IN (1,3,5,7) THEN 1 ELSE 0 END ) is_device," +
                                " COALESCE(p1.prm4::numeric,0) as squ1,COALESCE(p1.prm133::numeric,0) as squ2," +
                                " g.gil, mo.tarif, COALESCE(tr.tariff,0) as tarif_gkal, " +
                                " " + consumptionField + " as rashod," +
                                " 0 rash_norm_one, " +
                                " COALESCE(c.val1 + c.val2, " + consumptionField + ")  as valm, " +
                                " " + consumptionField + " as dop87," +
                                " (CASE WHEN tr.tariff>0 AND mo.nzp_serv IN (8,9) THEN mo.rsum_tarif/tr.tariff ELSE 0 END) as rashod_link," +
                //блок ЦХД - после переопределется другими значениями
                                " mo.tarif as tarif_chd, " +
                                " COALESCE(tr.tariff,0) as tarif_gkal_chd," +
                                " " + consumptionField + " as rashod_chd," +
                                " 0 as rash_norm_one_chd," +
                                " COALESCE(c.val1 + c.val2, " + consumptionField + ") as valm_chd," +
                                " 0 as dop87_chd," +
                                " 0 rashod_link_chd" +
                                " FROM {3} mo " +
                                " JOIN {4} g ON g.nzp_kvar=mo.nzp_kvar" +
                                " JOIN {5} p1 ON p1.nzp_kvar=mo.nzp_kvar" +
                                " LEFT OUTER JOIN {6} tr ON tr.nzp_kvar=mo.nzp_kvar AND tr.nzp_serv=mo.nzp_serv AND tr.nzp_supp=mo.nzp_supp" +
                                " LEFT OUTER JOIN {7} c ON c.nzp_kvar=mo.nzp_kvar AND c.nzp_serv=mo.nzp_serv ",
                tempTableCalcServ, param.dat_s, param.dat_po,
                TableMOCharges, tempTenantsTable, tempParamsTable, TableTariffGcal, consumptionInfo.counters_xx);
            BillingInstrumentary.ExecSQL(sql);


            //применение нормативов
            Container.Resolve<INormative>().ApplyNormatives(tempTableCalcServ);

            //проставляем блок ЦХД в chd_calc_gku
            //todo CalcConsumptions

            //применение тарифов
            Container.Resolve<ITariff>().ApplyTariffs(ref param, tempTableCalcServ);

            //сохраняем данные в физ.таблицу
            sql = " INSERT INTO " + TargetTable + " (chd_counters_id,nzp_wp,nzp_dom,nzp_kvar,nzp_serv," +
                  " nzp_supp,nzp_frm,stek,dat_s,dat_po, is_device,squ1, squ2,gil, tarif, tarif_gkal, rashod,rash_norm_one,valm, rashod_link, " +
                  " tarif_chd,tarif_gkal_chd,rashod_chd,rash_norm_one_chd, valm_chd, dop87_chd,rashod_link_chd)" +
                  " SELECT chd_counters_id,nzp_wp,nzp_dom,nzp_kvar,nzp_serv," +
                  " nzp_supp,nzp_frm,stek,dat_s,dat_po, is_device,squ1, squ2,gil, tarif, tarif_gkal, rashod,rash_norm_one,valm, rashod_link, " +
                  " tarif_chd,tarif_gkal_chd,rashod_chd,rash_norm_one_chd, valm_chd, dop87_chd,rashod_link_chd " +
                  " FROM " + tempTableCalcServ;
            BillingInstrumentary.ExecSQL(sql);

            return new BaseDataResult();
        }

        /// <summary>
        /// Получить Гкалл. тарифы для услуг
        /// </summary>
        /// <param name="TableMOCharges"></param>
        /// <param name="param"></param>
        private string GetGcalTariffs(string TableMOCharges, CalcTypes.ParamCalc param)
        {
            var tableParamForBank = CalcParams.Pref + "_data.prm_5";
            var tableParamForSupplier = CalcParams.Pref + "_data.prm_11";
            var tableParamForHouse = CalcParams.Pref + "_data.prm_2";
            var tableParamForAccount = CalcParams.Pref + "_data.prm_1";
            //получаем гигакаллорные тарифы
            //для выборки наиболее приоритетного тарифа вводим приоритеты, чем больше тем, приоритетнее:
            //0 - тариф на банк
            //1 - тариф на поставщика
            //2 - тариф на дом
            //3 - тариф на лс

            //тарифы на банк
            var tempTableGcalTariffPrepare = "t_table_gcal_tariff_" + DateTime.Now.Ticks;
            var sql = " CREATE TEMP TABLE " + tempTableGcalTariffPrepare + " AS " +
                         " SELECT t.nzp_kvar,t.nzp_serv,t.nzp_supp, p.val_prm::numeric tariff, 0 as priority " +
                         " FROM " + tableParamForBank + " p, " + TableMOCharges + " t " +
                         " WHERE t.nzp_serv IN (8,9)" +
                         " AND p.is_actual<>100 AND p.nzp_prm=252 " +
                         " AND p.dat_po>=" + param.dat_s +
                         " AND p.dat_s<=" + param.dat_po +
                         " GROUP BY 1,2,3,4";
            TempTablesLifeTime.AddTempTable(tempTableGcalTariffPrepare, sql);

            //на поставщика, при условии что включен параметр "Ставки ЭОТ по поставщикам"
            sql = " SELECT count(1)>0 FROM " + tableParamForBank + " p " +
                  " WHERE p.is_actual<>100 " +
                  " AND p.nzp_prm=336 " +
                  " AND p.dat_po>=" + param.dat_s +
                  " AND p.dat_s<=" + param.dat_po;
            if (BillingInstrumentary.ExecScalar<bool>(sql))
            {
                sql = " INSERT INTO " + tempTableGcalTariffPrepare + " " +
                      " (nzp_kvar,nzp_serv,nzp_supp, tariff,priority ) " +
                      " SELECT t.nzp_kvar, t.nzp_supp,t.nzp_serv, p.val_prm::numeric tariff, 1 as priority" +
                      " FROM " + tableParamForSupplier + " p " +
                      " JOIN " + TableMOCharges + " t ON t.nzp_supp=p.nzp" +
                      " WHERE t.nzp_serv IN (8,9)" +
                      " AND p.is_actual<>100 AND p.nzp_prm=339 " +
                      " AND p.dat_po>=" + param.dat_s +
                      " AND p.dat_s<=" + param.dat_po +
                      " GROUP BY 1,2,3,4";
                BillingInstrumentary.ExecSQL(sql);
            }

            //тарифы на дом для отопления
            sql = " INSERT INTO " + tempTableGcalTariffPrepare + " " +
                  " (nzp_kvar,nzp_serv,nzp_supp, tariff,priority ) " +
                  " SELECT t.nzp_kvar, t.nzp_supp,t.nzp_serv, p.val_prm::numeric tariff, 2 as priority" +
                  " FROM " + tableParamForHouse + " p " +
                  " JOIN " + TableMOCharges + " t ON t.nzp_dom=p.nzp" +
                  " WHERE t.nzp_serv=8" +
                  " AND p.is_actual<>100 AND p.nzp_prm=1062 " +
                  " AND p.dat_po>=" + param.dat_s +
                  " AND p.dat_s<=" + param.dat_po +
                  " GROUP BY 1,2,3,4";
            BillingInstrumentary.ExecSQL(sql);

            //тарифы на дом для ГВС
            sql = " INSERT INTO " + tempTableGcalTariffPrepare + " " +
                  " (nzp_kvar,nzp_serv,nzp_supp, tariff,priority ) " +
                  " SELECT t.nzp_kvar, t.nzp_supp,t.nzp_serv, p.val_prm::numeric tariff, 2 as priority" +
                  " FROM " + tableParamForHouse + " p " +
                  " JOIN " + TableMOCharges + " t ON t.nzp_dom=p.nzp" +
                  " WHERE t.nzp_serv=9" +
                  " AND p.is_actual<>100 AND p.nzp_prm=1381 " +
                  " AND p.dat_po>=" + param.dat_s +
                  " AND p.dat_s<=" + param.dat_po +
                  " GROUP BY 1,2,3,4";
            BillingInstrumentary.ExecSQL(sql);


            //тарифы на лицевые счета, при условии что включен параметр "Ставки ЭОТ по лиц.счетам"
            sql = " SELECT count(1)>0 FROM " + tableParamForBank + " p " +
                  " WHERE p.is_actual<>100 " +
                  " AND p.nzp_prm=335 " +
                  " AND p.dat_po>=" + param.dat_s +
                  " AND p.dat_s<=" + param.dat_po;
            if (BillingInstrumentary.ExecScalar<bool>(sql))
            {
                //для отопления
                sql = " INSERT INTO " + tempTableGcalTariffPrepare + " " +
                      " (nzp_kvar,nzp_serv,nzp_supp, tariff,priority ) " +
                      " SELECT t.nzp_kvar, t.nzp_supp,t.nzp_serv, p.val_prm::numeric tariff, 3 as priority" +
                      " FROM " + tableParamForAccount + " p " +
                      " JOIN " + TableMOCharges + " t ON t.nzp_kvar=p.nzp" +
                      " WHERE t.nzp_serv=8" +
                      " AND p.is_actual<>100 AND p.nzp_prm=341 " +
                      " AND p.dat_po>=" + param.dat_s +
                      " AND p.dat_s<=" + param.dat_po +
                      " GROUP BY 1,2,3,4";
                BillingInstrumentary.ExecSQL(sql);

                //для ГВС
                sql = " INSERT INTO " + tempTableGcalTariffPrepare + " " +
                      " (nzp_kvar,nzp_serv,nzp_supp, tariff,priority ) " +
                      " SELECT t.nzp_kvar, t.nzp_supp,t.nzp_serv, p.val_prm::numeric tariff, 3 as priority" +
                      " FROM " + tableParamForAccount + " p " +
                      " JOIN " + TableMOCharges + " t ON t.nzp_kvar=p.nzp" +
                      " WHERE t.nzp_serv=8" +
                      " AND p.is_actual<>100 AND p.nzp_prm=340 " +
                      " AND p.dat_po>=" + param.dat_s +
                      " AND p.dat_s<=" + param.dat_po +
                      " GROUP BY 1,2,3,4";
                BillingInstrumentary.ExecSQL(sql);
            }

            sql = " CREATE INDEX ix1_" + tempTableGcalTariffPrepare + " ON " + tempTableGcalTariffPrepare +
                  "(nzp_kvar,nzp_serv,nzp_supp, tariff) ";
            BillingInstrumentary.ExecSQL(sql);

            var tempTableGcalTariff = "t_table_gcal_tariff_res_" + DateTime.Now.Ticks;

            sql = " CREATE TEMP TABLE " + tempTableGcalTariff + " AS " +
                  " SELECT  row_number() OVER(PARTITION BY nzp_kvar,nzp_serv,nzp_supp ORDER BY priority DESC) id," + //сортировка в порядке убывания приоритета
                  " nzp_kvar, nzp_serv, nzp_supp, tariff " +
                  " FROM " + tempTableGcalTariffPrepare + " " +
                  " GROUP BY nzp_kvar, nzp_serv, nzp_supp, tariff,priority";
            TempTablesLifeTime.AddTempTable(tempTableGcalTariff, sql);

            sql = " CREATE INDEX ix1_" + tempTableGcalTariff + " ON " + tempTableGcalTariff + "(id) ";
            BillingInstrumentary.ExecSQL(sql);


            var TableTarifGcal = "t_table_tariff_gcal_" + DateTime.Now.Ticks;

            sql = " CREATE TEMP TABLE " + TableTarifGcal + " as  " +
                  " SELECT * FROM " + tempTableGcalTariff + " t" +
                  " WHERE t.id=1"; //id=1 - самое приоритетное значение тарифа
            TempTablesLifeTime.AddTempTable(TableTarifGcal, sql);

            sql = " CREATE INDEX ix2_" + tempTableGcalTariff + " ON " + tempTableGcalTariff + "(nzp_kvar,nzp_serv,nzp_supp) ";
            BillingInstrumentary.ExecSQL(sql);

            TempTablesLifeTime.DropTempTable(tempTableGcalTariffPrepare);
            TempTablesLifeTime.DropTempTable(tempTableGcalTariff);

            return TableTarifGcal;
        }
    }


    public class CalcServicesHandlerOverride : ICalcServices
    {
        private CalcServicesHandlerOverride() { }
        protected IDbConnection Connection;
        protected BillingInstrumentary BillingInstrumentary;
        protected IWindsorContainer Container;
        protected CalcVerificationParams CalcParams;
        protected TempTablesLifeTime TempTablesLifeTime;
        public CalcServicesHandlerOverride(IDbConnection connection, IWindsorContainer contariner, BillingInstrumentary billingInstrumentary)
        {
            Connection = connection;
            Container = contariner;
            BillingInstrumentary = billingInstrumentary;
            CalcParams = Container.Resolve<CalcVerificationParams>();
            TempTablesLifeTime = Container.Resolve<TempTablesLifeTime>();
        }

        public IDataResult CalcServices(string TableMOCharges)
        {
            var param = CalcParams.ParamCalc;
            var tenantInfo = new DbCalcCharge.GilecXX(param);
            tenantInfo.gil_xx = string.Format("{0}.{1}", param.pref, tenantInfo.gilec_tab);

            var consumptionInfo = new CalcTypes.ChargeXX(param);
            consumptionInfo.charge_xx = string.Format("{0}{1}{2}", param.pref, DBManager.tableDelimiter, consumptionInfo.charge_tab);
            consumptionInfo.counters_xx = string.Format("{0}{1}counters{2}_{3}", param.pref, DBManager.tableDelimiter, param.alias, param.calc_mm.ToString("00"));

            var TargetTable = string.Format("{0}.chd_calc_gku_{1}", param.pref, param.calc_mm.ToString("00"));
            var tempTableCalcServ = "t_calc_gku_" + DateTime.Now.Ticks;
            var sql = " CREATE TEMP TABLE " + tempTableCalcServ + " (LIKE " + TargetTable +
                      " INCLUDING DEFAULTS INCLUDING STORAGE)";
            TempTablesLifeTime.AddTempTable(tempTableCalcServ, sql);

            var tempParamsTable = "t_param_gil_" + DateTime.Now.Ticks;
            sql = " CREATE TEMP TABLE " + tempParamsTable + " AS " +
                  " SELECT nzp as nzp_kvar, MAX(CASE WHEN nzp_prm=4 THEN val_prm END) prm4," +
                  " MAX(CASE WHEN nzp_prm=133 THEN val_prm END) prm133 " +
                  " FROM ttt_prm_1 WHERE nzp_prm IN (4,133)" +
                  " GROUP BY nzp";
            TempTablesLifeTime.AddTempTable(tempParamsTable, sql);

            sql = "CREATE INDEX ix1_" + tempParamsTable + " ON " + tempTableCalcServ + "(nzp_kvar)";
            BillingInstrumentary.ExecSQL(sql);

            var tempTenantsTable = "t_tenants_" + DateTime.Now.Ticks;
            sql = " CREATE TEMP TABLE " + tempTenantsTable + " AS " +
                  " SELECT t.nzp_kvar, MAX(cnt2) gil " +
                  " FROM " + tenantInfo.gil_xx + " g" +
                  " JOIN t_selkvar t ON t.nzp_kvar=g.nzp_kvar " +
                  " WHERE g.stek=3 " +
                  " GROUP BY t.nzp_kvar";
            TempTablesLifeTime.AddTempTable(tempTenantsTable, sql);

            sql = "CREATE INDEX ix1_" + tempTenantsTable + " ON " + tempTenantsTable + "(nzp_kvar)";
            BillingInstrumentary.ExecSQL(sql);

            //получили Гкал.тарифы
            var TableTariffGcal = GetGcalTariffs(TableMOCharges, param);

            const string consumptionField = "(CASE WHEN mo.tarif>0 THEN mo.rsum_tarif/mo.tarif ELSE 0 END)";

            sql = string.Format(" INSERT INTO {0} (chd_counters_id,nzp_wp,nzp_dom,nzp_kvar,nzp_serv," +
                                " nzp_supp,nzp_frm,stek,dat_s,dat_po, is_device,squ1, squ2,gil, tarif, tarif_gkal, rashod,rash_norm_one,valm,dop87, rashod_link, " +
                                " tarif_chd,tarif_gkal_chd,rashod_chd,rash_norm_one_chd, valm_chd, dop87_chd,rashod_link_chd)" +
                                " SELECT 0, 0, mo.nzp_dom, mo.nzp_kvar,mo.nzp_serv,mo.nzp_supp, mo.nzp_frm, 3 stek, {1} dat_s, {2} dat_po, " +
                                " (CASE WHEN mo.is_device IN (1,3,5,7) THEN 1 ELSE 0 END ) is_device," +
                                " COALESCE(p1.prm4::numeric,0) as squ1,COALESCE(p1.prm133::numeric,0) as squ2," +
                                " g.gil, mo.tarif, COALESCE(tr.tariff,0) as tarif_gkal, " +
                                " " + consumptionField + " as rashod," +
                                " 0 rash_norm_one, " +
                                " COALESCE(c.val1 + c.val2, " + consumptionField + ")  as valm, " +
                                " " + consumptionField + " as dop87," +
                                " (CASE WHEN tr.tariff>0 AND mo.nzp_serv IN (8,9) THEN mo.rsum_tarif/tr.tariff ELSE 0 END) as rashod_link," +
                //блок ЦХД - после переопределется другими значениями
                                " mo.tarif as tarif_chd, " +
                                " COALESCE(tr.tariff,0) as tarif_gkal_chd," +
                                " " + consumptionField + " as rashod_chd," +
                                " 0 as rash_norm_one_chd," +
                                " COALESCE(c.val1 + c.val2, " + consumptionField + ") as valm_chd," +
                                " 0 as dop87_chd," +
                                " 0 rashod_link_chd" +
                                " FROM {3} mo " +
                                " JOIN {4} g ON g.nzp_kvar=mo.nzp_kvar" +
                                " JOIN {5} p1 ON p1.nzp_kvar=mo.nzp_kvar" +
                                " LEFT OUTER JOIN {6} tr ON tr.nzp_kvar=mo.nzp_kvar AND tr.nzp_serv=mo.nzp_serv AND tr.nzp_supp=mo.nzp_supp" +
                                " LEFT OUTER JOIN {7} c ON c.nzp_kvar=mo.nzp_kvar AND c.nzp_serv=mo.nzp_serv ",
                tempTableCalcServ, param.dat_s, param.dat_po,
                TableMOCharges, tempTenantsTable, tempParamsTable, TableTariffGcal, consumptionInfo.counters_xx);
            BillingInstrumentary.ExecSQL(sql);


            //применение нормативов
            Container.Resolve<INormative>().ApplyNormatives(tempTableCalcServ);

            //проставляем блок ЦХД в chd_calc_gku
            //todo CalcConsumptions

            //применение тарифов
            Container.Resolve<ITariff>().ApplyTariffs(ref param, tempTableCalcServ);

            //сохраняем данные в физ.таблицу
            sql = " INSERT INTO " + TargetTable + " (chd_counters_id,nzp_wp,nzp_dom,nzp_kvar,nzp_serv," +
                  " nzp_supp,nzp_frm,stek,dat_s,dat_po, is_device,squ1, squ2,gil, tarif, tarif_gkal, rashod,rash_norm_one,valm, rashod_link, " +
                  " tarif_chd,tarif_gkal_chd,rashod_chd,rash_norm_one_chd, valm_chd, dop87_chd,rashod_link_chd)" +
                  " SELECT chd_counters_id,nzp_wp,nzp_dom,nzp_kvar,nzp_serv," +
                  " nzp_supp,nzp_frm,stek,dat_s,dat_po, is_device,squ1, squ2,gil, tarif, tarif_gkal, rashod,rash_norm_one,valm, rashod_link, " +
                  " tarif_chd,tarif_gkal_chd,rashod_chd,rash_norm_one_chd, valm_chd, dop87_chd,rashod_link_chd " +
                  " FROM " + tempTableCalcServ;
            BillingInstrumentary.ExecSQL(sql);

            return new BaseDataResult();
        }

        /// <summary>
        /// Получить Гкалл. тарифы для услуг
        /// </summary>
        /// <param name="TableMOCharges"></param>
        /// <param name="param"></param>
        private string GetGcalTariffs(string TableMOCharges, CalcTypes.ParamCalc param)
        {
            var tableParamForBank = CalcParams.Pref + ".prm_5";
            var tableParamForSupplier = CalcParams.Pref + ".prm_11";
            var tableParamForHouse = CalcParams.Pref + ".prm_2";
            var tableParamForAccount = CalcParams.Pref + ".prm_1";
            //получаем гигакаллорные тарифы
            //для выборки наиболее приоритетного тарифа вводим приоритеты, чем больше тем, приоритетнее:
            //0 - тариф на банк
            //1 - тариф на поставщика
            //2 - тариф на дом
            //3 - тариф на лс

            //тарифы на банк
            var tempTableGcalTariffPrepare = "t_table_gcal_tariff_" + DateTime.Now.Ticks;
            var sql = " CREATE TEMP TABLE " + tempTableGcalTariffPrepare + " AS " +
                         " SELECT t.nzp_kvar,t.nzp_serv,t.nzp_supp, p.val_prm::numeric tariff, 0 as priority " +
                         " FROM " + tableParamForBank + " p, " + TableMOCharges + " t " +
                         " WHERE t.nzp_serv IN (8,9)" +
                         " AND p.is_actual<>100 AND p.nzp_prm=252 " +
                         " AND p.dat_po>=" + param.dat_s +
                         " AND p.dat_s<=" + param.dat_po +
                         " GROUP BY 1,2,3,4";
            TempTablesLifeTime.AddTempTable(tempTableGcalTariffPrepare, sql);

            //на поставщика, при условии что включен параметр "Ставки ЭОТ по поставщикам"
            sql = " SELECT count(1)>0 FROM " + tableParamForBank + " p " +
                  " WHERE p.is_actual<>100 " +
                  " AND p.nzp_prm=336 " +
                  " AND p.dat_po>=" + param.dat_s +
                  " AND p.dat_s<=" + param.dat_po;
            if (BillingInstrumentary.ExecScalar<bool>(sql))
            {
                sql = " INSERT INTO " + tempTableGcalTariffPrepare + " " +
                      " (nzp_kvar,nzp_serv,nzp_supp, tariff,priority ) " +
                      " SELECT t.nzp_kvar, t.nzp_supp,t.nzp_serv, p.val_prm::numeric tariff, 1 as priority" +
                      " FROM " + tableParamForSupplier + " p " +
                      " JOIN " + TableMOCharges + " t ON t.nzp_supp=p.nzp" +
                      " WHERE t.nzp_serv IN (8,9)" +
                      " AND p.is_actual<>100 AND p.nzp_prm=339 " +
                      " AND p.dat_po>=" + param.dat_s +
                      " AND p.dat_s<=" + param.dat_po +
                      " GROUP BY 1,2,3,4";
                BillingInstrumentary.ExecSQL(sql);
            }

            //тарифы на дом для отопления
            sql = " INSERT INTO " + tempTableGcalTariffPrepare + " " +
                  " (nzp_kvar,nzp_serv,nzp_supp, tariff,priority ) " +
                  " SELECT t.nzp_kvar, t.nzp_supp,t.nzp_serv, p.val_prm::numeric tariff, 2 as priority" +
                  " FROM " + tableParamForHouse + " p " +
                  " JOIN " + TableMOCharges + " t ON t.nzp_dom=p.nzp" +
                  " WHERE t.nzp_serv=8" +
                  " AND p.is_actual<>100 AND p.nzp_prm=1062 " +
                  " AND p.dat_po>=" + param.dat_s +
                  " AND p.dat_s<=" + param.dat_po +
                  " GROUP BY 1,2,3,4";
            BillingInstrumentary.ExecSQL(sql);

            //тарифы на дом для ГВС
            sql = " INSERT INTO " + tempTableGcalTariffPrepare + " " +
                  " (nzp_kvar,nzp_serv,nzp_supp, tariff,priority ) " +
                  " SELECT t.nzp_kvar, t.nzp_supp,t.nzp_serv, p.val_prm::numeric tariff, 2 as priority" +
                  " FROM " + tableParamForHouse + " p " +
                  " JOIN " + TableMOCharges + " t ON t.nzp_dom=p.nzp" +
                  " WHERE t.nzp_serv=9" +
                  " AND p.is_actual<>100 AND p.nzp_prm=1381 " +
                  " AND p.dat_po>=" + param.dat_s +
                  " AND p.dat_s<=" + param.dat_po +
                  " GROUP BY 1,2,3,4";
            BillingInstrumentary.ExecSQL(sql);


            //тарифы на лицевые счета, при условии что включен параметр "Ставки ЭОТ по лиц.счетам"
            sql = " SELECT count(1)>0 FROM " + tableParamForBank + " p " +
                  " WHERE p.is_actual<>100 " +
                  " AND p.nzp_prm=335 " +
                  " AND p.dat_po>=" + param.dat_s +
                  " AND p.dat_s<=" + param.dat_po;
            if (BillingInstrumentary.ExecScalar<bool>(sql))
            {
                //для отопления
                sql = " INSERT INTO " + tempTableGcalTariffPrepare + " " +
                      " (nzp_kvar,nzp_serv,nzp_supp, tariff,priority ) " +
                      " SELECT t.nzp_kvar, t.nzp_supp,t.nzp_serv, p.val_prm::numeric tariff, 3 as priority" +
                      " FROM " + tableParamForAccount + " p " +
                      " JOIN " + TableMOCharges + " t ON t.nzp_kvar=p.nzp" +
                      " WHERE t.nzp_serv=8" +
                      " AND p.is_actual<>100 AND p.nzp_prm=341 " +
                      " AND p.dat_po>=" + param.dat_s +
                      " AND p.dat_s<=" + param.dat_po +
                      " GROUP BY 1,2,3,4";
                BillingInstrumentary.ExecSQL(sql);

                //для ГВС
                sql = " INSERT INTO " + tempTableGcalTariffPrepare + " " +
                      " (nzp_kvar,nzp_serv,nzp_supp, tariff,priority ) " +
                      " SELECT t.nzp_kvar, t.nzp_supp,t.nzp_serv, p.val_prm::numeric tariff, 3 as priority" +
                      " FROM " + tableParamForAccount + " p " +
                      " JOIN " + TableMOCharges + " t ON t.nzp_kvar=p.nzp" +
                      " WHERE t.nzp_serv=8" +
                      " AND p.is_actual<>100 AND p.nzp_prm=340 " +
                      " AND p.dat_po>=" + param.dat_s +
                      " AND p.dat_s<=" + param.dat_po +
                      " GROUP BY 1,2,3,4";
                BillingInstrumentary.ExecSQL(sql);
            }

            sql = " CREATE INDEX ix1_" + tempTableGcalTariffPrepare + " ON " + tempTableGcalTariffPrepare +
                  "(nzp_kvar,nzp_serv,nzp_supp, tariff) ";
            BillingInstrumentary.ExecSQL(sql);

            var tempTableGcalTariff = "t_table_gcal_tariff_res_" + DateTime.Now.Ticks;

            sql = " CREATE TEMP TABLE " + tempTableGcalTariff + " AS " +
                  " SELECT  row_number() OVER(PARTITION BY nzp_kvar,nzp_serv,nzp_supp ORDER BY priority DESC) id," + //сортировка в порядке убывания приоритета
                  " nzp_kvar, nzp_serv, nzp_supp, tariff " +
                  " FROM " + tempTableGcalTariffPrepare + " " +
                  " GROUP BY nzp_kvar, nzp_serv, nzp_supp, tariff,priority";
            TempTablesLifeTime.AddTempTable(tempTableGcalTariff, sql);

            sql = " CREATE INDEX ix1_" + tempTableGcalTariff + " ON " + tempTableGcalTariff + "(id) ";
            BillingInstrumentary.ExecSQL(sql);


            var TableTarifGcal = "t_table_tariff_gcal_" + DateTime.Now.Ticks;

            sql = " CREATE TEMP TABLE " + TableTarifGcal + " as  " +
                  " SELECT * FROM " + tempTableGcalTariff + " t" +
                  " WHERE t.id=1"; //id=1 - самое приоритетное значение тарифа
            TempTablesLifeTime.AddTempTable(TableTarifGcal, sql);

            sql = " CREATE INDEX ix2_" + tempTableGcalTariff + " ON " + tempTableGcalTariff + "(nzp_kvar,nzp_serv,nzp_supp) ";
            BillingInstrumentary.ExecSQL(sql);

            TempTablesLifeTime.DropTempTable(tempTableGcalTariffPrepare);
            TempTablesLifeTime.DropTempTable(tempTableGcalTariff);

            return TableTarifGcal;
        }
    }
}
