using Bars.B4;

namespace Bars.Gkh.Gis.DomainService.CalcVerification.Impl
{
    using System;
    using Intf;
    using System.Data;

    using Bars.Gkh.Gis.KP_legacy;

    using Entities.CalcVerification;
    using Castle.Windsor;

    /// <summary>
    /// Расчет ОДН 
    /// </summary>
    public class CalcConsumptionsHandler : ICalcConsumptions
    {
        private CalcConsumptionsHandler() { }
        protected IDbConnection Connection;
        protected BillingInstrumentary BillingInstrumentary;
        protected IWindsorContainer Container;
        protected CalcVerificationParams CalcParams;
        protected TempTablesLifeTime TempTablesLifeTime;
        public CalcConsumptionsHandler(IDbConnection connection, IWindsorContainer contariner, BillingInstrumentary billingInstrumentary)
        {
            Connection = connection;
            Container = contariner;
            BillingInstrumentary = billingInstrumentary;
            CalcParams = Container.Resolve<CalcVerificationParams>();
            TempTablesLifeTime = Container.Resolve<TempTablesLifeTime>();
        }

        /// <summary>
        /// Расчет домовых расходов
        /// </summary>
        /// <param name="SourceTable"></param>
        /// <returns></returns>
        public IDataResult CalcHouseConsumptions(CalcTypes.ParamCalc param, string SourceTable)
        {
            CreateCountersSpis(param);
            string sDatUchet = "a.dat_uchet";
            var rashod = new DbCalcCharge.Rashod(param);
            rashod.counters_xx = string.Format("{0}{1}{2}", rashod.paramcalc.pref, DBManager.tableDelimiter, rashod.counters_tab.Replace(rashod.counters_tab, "chd_" + rashod.counters_tab));
            var p_dat_charge = new DateTime(param.cur_yy, param.cur_mm, 1).ToShortDateString();

            var rashod_prm = new DbCalcCharge.Rashod2(rashod.counters_xx, rashod.paramcalc);
            rashod_prm.tab = "temp_counters_dom";
            //rashod_prm.dat_s = rashod.paramcalc.dat_s;
            //rashod_prm.dat_po = rashod.paramcalc.dat_po;
            rashod_prm.p_TAB = rashod_prm.tab + " a";
            //rashod_prm.p_KEY = "a.nzp_crd";
            //rashod_prm.p_ACTUAL = " and c.is_actual <> 100";
            //rashod_prm.counters_xx = rashod.counters_xx;
            rashod_prm.p_where = rashod.where_dom;
            //rashod_prm.pref = rashod.paramcalc.pref;
            rashod_prm.p_type = "1";
            // для выборки дат снятия показаний ПУ
            rashod_prm.p_INSERT =
                " Insert into tpok (nzp_cr,nzp_kvar,nzp_dom,nzp_counter,nzp_serv,dat_uchet)" +
                " Select 0,0,a.nzp_dom,a.nzp_counter,a.nzp_serv," + sDatUchet +
                " From " + rashod_prm.tab + " a" +
                " Where a." + rashod.where_dom;
            // для выборки значений показаний ПУ
            rashod_prm.p_FROM =
                " From " + rashod_prm.tab + " a, " + rashod_prm.counters_xx + " b " +
                " Where a." + rashod.where_dom;
            rashod_prm.p_FROM =
                " From " + rashod_prm.tab + " a, t_inscnt b " +
                " Where a." + rashod.where_dom;
            // для установки расхода нежилых помещений для ОДПУ
            rashod_prm.p_UPDdt_s =
                  " ,val2 = " +
                  "( Select max(ngp_cnt+ngp_lift) From " + rashod_prm.tab + " a " +
                  " Where t_inscnt.nzp_counter = a.nzp_counter and t_inscnt.dat_po = a.dat_uchet )";
            //" Where " + rashod.counters_xx + ".nzp_counter = a.nzp_counter and " + rashod.counters_xx + ".dat_po = a.dat_uchet )";
            rashod_prm.p_UPDdt_po =
                  " ,ngp_cnt = " +
                  "( Select max(ngp_cnt+ngp_lift) From " + rashod_prm.tab + " a " +
                  " Where t_inscnt.nzp_counter = a.nzp_counter and t_inscnt.dat_po = a.dat_uchet )";
            //" Where " + rashod.counters_xx + ".nzp_counter = a.nzp_counter and " + rashod.counters_xx + ".dat_po = a.dat_uchet )";

            LoadValsNew(Connection, rashod_prm, p_dat_charge, "3");

            return new BaseDataResult();
        }

        /// <summary>
        /// Применение 
        /// </summary>
        /// <param name="TargetTable"></param>
        /// <returns></returns>
        public IDataResult ApplyHouseConsumption(string TargetTable)
        {
            throw new System.NotImplementedException();
        }

        public void CreateCountersSpis(CalcTypes.ParamCalc param)
        {
            string s = "  Where 1 = 1 ";
            string ss = "  Where 1 = 1 ";
            string sss = "  Where 1 = 1 ";
            string ssss = "  Where 1 = 1 ";
            if (param.nzp_kvar > 0 || param.nzp_dom > 0)
            {
                s = " ,t_selkvar b Where a.nzp_kvar = b.nzp_kvar ";
                ss = " ,t_selkvar b Where a.nzp = b.nzp_kvar ";
                sss = " Where exists (select 1 from t_selkvar b Where a.nzp = b.nzp_dom) ";
                //" Where 0<(select count(*) from t_selkvar b Where a.nzp = b.nzp_dom) ";
                ssss = " Where exists (select 1 from t_selkvar b ," + param.pref + "counters_link l" +
                    //" Where 0<(select count(*) from t_selkvar b ," + paramcalc.data_alias + "counters_link l" +
                                " Where l.nzp_counter=a.nzp_counter and l.nzp_kvar = b.nzp_kvar) ";
            }

            //counters_spis - описатели ПУ
            BillingInstrumentary.ExecSQL(" Drop table temp_cnt_spis", false);

            BillingInstrumentary.ExecSQL(
                " CREATE TEMP TABLE temp_cnt_spis (" +
                " nzp_counter  serial NOT NULL," +
                " nzp_type     integer NOT NULL," +
                " nzp          integer NOT NULL," +
                " nzp_serv     integer NOT NULL," +
                " nzp_cnttype  integer NOT NULL," +
                " num_cnt      character(40)," +
                " kod_pu       integer DEFAULT 0," +
                " kod_info     integer DEFAULT 0," +
                " dat_prov     date," +
                " dat_provnext date," +
                " dat_oblom    date," +
                " dat_poch     date," +
                " dat_close    date," +
                " comment      character(60)," +
                " nzp_cnt      integer," +
                " is_gkal      integer NOT NULL DEFAULT 0," +
                " is_actual    integer," +
                " nzp_user     integer," +
                " dat_when     date," +
                " is_pl        integer DEFAULT 0," +
                " cnt_ls       integer DEFAULT 0," +
                " dat_block    " + DBManager.sDateTimeType + "," +
                " user_block   integer," +
                " month_calc   date," +
                " user_del     integer," +
                " dat_del      date," +
                " dat_s        date," +
                " dat_po       date" +
                " ) " + DBManager.sUnlogTempTable
                , true);

            BillingInstrumentary.ExecSQL(
                " insert into temp_cnt_spis (" +
                " nzp_counter,nzp_type,nzp,nzp_serv,nzp_cnttype,num_cnt," +
                " kod_pu,kod_info,dat_prov,dat_provnext,dat_oblom,dat_poch,dat_close," +
                " comment,nzp_cnt,is_actual,nzp_user,dat_when,is_pl,cnt_ls," +
                " dat_block,user_block,month_calc,user_del,dat_del,dat_s,dat_po" +
                " )" +
                " Select " +
                " a.nzp_counter,a.nzp_type,a.nzp,a.nzp_serv,a.nzp_cnttype,a.num_cnt," +
                " a.kod_pu,a.kod_info,a.dat_prov,a.dat_provnext,a.dat_oblom,a.dat_poch,a.dat_close," +
                " a.comment,a.nzp_cnt,a.is_actual,a.nzp_user,a.dat_when,a.is_pl,a.cnt_ls," +
                " a.dat_block,a.user_block,a.month_calc,a.user_del,a.dat_del,a.dat_s,a.dat_po" +
                " From " + param.pref + "counters_spis a " +
                ss + " and a.nzp_type=3 and a.is_actual <> 100 "
                , true);

            BillingInstrumentary.ExecSQL(
                " insert into temp_cnt_spis (" +
                " nzp_counter,nzp_type,nzp,nzp_serv,nzp_cnttype,num_cnt," +
                " kod_pu,kod_info,dat_prov,dat_provnext,dat_oblom,dat_poch,dat_close," +
                " comment,nzp_cnt,nzp_user,dat_when,is_pl,cnt_ls," +
                " dat_block,user_block,month_calc,user_del,dat_del,dat_s,dat_po" +
                " )" +
                " Select " +
                " a.nzp_counter,a.nzp_type,a.nzp,a.nzp_serv,a.nzp_cnttype,a.num_cnt," +
                " a.kod_pu,a.kod_info,a.dat_prov,a.dat_provnext,a.dat_oblom,a.dat_poch,a.dat_close," +
                " a.comment,a.nzp_cnt,a.nzp_user,a.dat_when,a.is_pl,a.cnt_ls," +
                " a.dat_block,a.user_block,a.month_calc,a.user_del,a.dat_del,a.dat_s,a.dat_po" +
                " From " + param.pref + "counters_spis a " +
                sss + " and a.nzp_type=1 and a.is_actual <> 100 "
                , true);

            BillingInstrumentary.ExecSQL(
                " insert into temp_cnt_spis (" +
                " nzp_counter,nzp_type,nzp,nzp_serv,nzp_cnttype,num_cnt," +
                " kod_pu,kod_info,dat_prov,dat_provnext,dat_oblom,dat_poch,dat_close," +
                " comment,nzp_cnt,nzp_user,dat_when,is_pl,cnt_ls," +
                " dat_block,user_block,month_calc,user_del,dat_del,dat_s,dat_po" +
                " )" +
                " Select " +
                " a.nzp_counter,a.nzp_type,a.nzp,a.nzp_serv,a.nzp_cnttype,a.num_cnt," +
                " a.kod_pu,a.kod_info,a.dat_prov,a.dat_provnext,a.dat_oblom,a.dat_poch,a.dat_close," +
                " a.comment,a.nzp_cnt,a.nzp_user,a.dat_when,a.is_pl,a.cnt_ls," +
                " a.dat_block,a.user_block,a.month_calc,a.user_del,a.dat_del,a.dat_s,a.dat_po" +
                " From " + param.pref + "counters_spis a " +
                ssss + " and a.nzp_type in (2,4) and a.is_actual <> 100 " //ГрПУ, общ.кв.ПУ
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix1_temp_cnt_spis on temp_cnt_spis (nzp_counter) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix2_temp_cnt_spis on temp_cnt_spis (nzp, nzp_serv, nzp_counter) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix3_temp_cnt_spis on temp_cnt_spis (nzp, nzp_serv, nzp_cnttype, num_cnt) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix4_temp_cnt_spis on temp_cnt_spis (nzp_type, dat_close) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix5_temp_cnt_spis on temp_cnt_spis (nzp_cnt) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix6_temp_cnt_spis on temp_cnt_spis (nzp, nzp_type) ", true);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " temp_cnt_spis ", true);

            //counters_dom - домовые ПУ
            BillingInstrumentary.ExecSQL(" Drop table temp_counters_dom ", false);

            BillingInstrumentary.ExecSQL(
                " CREATE temp TABLE temp_counters_dom(" +
                "   nzp_crd INTEGER," +
                "   nzp_dom INTEGER," +
                "   nzp_serv INTEGER," +
                "   nzp_cnttype INTEGER," +
                "   num_cnt CHAR(40)," +
                "   dat_prov DATE," +
                "   dat_provnext DATE," +
                "   dat_uchet DATE," +
                "   val_cnt FLOAT," +
                "   kol_gil_dom INTEGER," +
                "   is_actual INTEGER," +
                "   nzp_user INTEGER," +
                "   comment CHAR(200)," +
                "   sum_pl " + DBManager.sDecimalType + "(14,2)," +
                "   is_doit INTEGER default 1 NOT NULL," +
                "   is_pl INTEGER," +
                "   sum_otopl " + DBManager.sDecimalType + "(14,2)," +
                "   cnt_ls INTEGER," +
                "   dat_when DATE," +
                "   is_uchet_ls INTEGER default 0 NOT NULL," +
                "   nzp_cntkind INTEGER default 1," +
                "   nzp_measure INTEGER," +
                "   is_gkal INTEGER," +
                "   ngp_cnt " + DBManager.sDecimalType + "(14,7) default 0.0000000," +
                "   cur_unl INTEGER default 0," +
                "   nzp_wp INTEGER default 1," +
                "   ngp_lift " + DBManager.sDecimalType + "(14,7) default 0.0000000," +
                "   dat_oblom DATE," +
                "   dat_poch DATE," +
                "   dat_close DATE," +
                "   nzp_counter INTEGER default 0 NOT NULL," +
                "   nzp_counter_parent integer NOT NULL DEFAULT 0," +
                "   month_calc DATE," +
                "   user_del INTEGER," +
                "   dat_del DATE," +
                "   dat_s DATE," +
                "   dat_po DATE " +
                "   ) " + DBManager.sUnlogTempTable
                , true);


            if (param.nzp_kvar > 0 || param.nzp_dom > 0)
                s = "  Where a.nzp_dom in (select b.nzp_dom from t_selkvar b) ";
            else
                s = "  Where 1 = 1 ";

            BillingInstrumentary.ExecSQL(
                " insert into temp_counters_dom ( " +
                " nzp_crd,nzp_dom,nzp_serv,nzp_cnttype,num_cnt,dat_prov,dat_provnext,dat_uchet,val_cnt,kol_gil_dom, " +
                " is_actual,nzp_user,comment,sum_pl,is_doit,is_pl,sum_otopl,cnt_ls,dat_when,is_uchet_ls,nzp_cntkind, " +
                " nzp_measure,is_gkal,ngp_cnt,cur_unl,nzp_wp,ngp_lift,dat_oblom,dat_poch,dat_close,nzp_counter, " +
                " month_calc,user_del,dat_del,dat_s,dat_po) " +
                " Select " +
                " a.nzp_crd,a.nzp_dom,a.nzp_serv,a.nzp_cnttype,a.num_cnt,a.dat_prov,a.dat_provnext,a.dat_uchet,a.val_cnt,a.kol_gil_dom, " +
                " a.is_actual,a.nzp_user,a.comment,a.sum_pl,a.is_doit,a.is_pl,sum_otopl,a.cnt_ls,a.dat_when,a.is_uchet_ls,a.nzp_cntkind, " +
                " a.nzp_measure,a.is_gkal,a.ngp_cnt,a.cur_unl,a.nzp_wp,a.ngp_lift,a.dat_oblom,a.dat_poch,a.dat_close,a.nzp_counter, " +
                " a.month_calc,a.user_del,a.dat_del,a.dat_s,a.dat_po " +
                " From " + param.pref + "counters_dom a, temp_cnt_spis s " +
                s + " and a.nzp_counter=s.nzp_counter and a.is_actual <> 100 and s.is_gkal=0 "
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix1_temp_counters_dom on temp_counters_dom (nzp_crd) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix2_temp_counters_dom on temp_counters_dom (nzp_dom,nzp_counter, dat_uchet) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix3_temp_counters_dom on temp_counters_dom (nzp_dom, nzp_serv, nzp_cnttype, num_cnt, dat_uchet ) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix4_temp_counters_dom on temp_counters_dom (nzp_counter, dat_uchet) ", true);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " temp_counters_dom ", true);

        }

        public bool LoadValsNew(IDbConnection conn_db, DbCalcCharge.Rashod2 rashod_prm, string p_dat_charge, string sstek)
        //--------------------------------------------------------------------------------
        {
            string sDatUchet = "a.dat_uchet";

            #region Выборка уникальных счетчиков в tpok с учетом выбранного списка temp_counters&t_selkvar

            BillingInstrumentary.ExecSQL(" Drop table tpok ", false);
            BillingInstrumentary.ExecSQL(
                " Create temp table tpok " +
                " ( nzp_cr      serial, " +
                "   nzp_kvar    integer," +
                "   nzp_dom     integer," +
                "   nzp_counter integer," +
                "   nzp_serv    integer," +
                "   dat_uchet   date    " +
                " ) " + DBManager.sUnlogTempTable
                , true);

            BillingInstrumentary.ExecSQL(
                rashod_prm.p_INSERT +
                "   and 1 > (" +
                "     Select count(*) From aid_i" + rashod_prm.pref + " n" +
                "     Where a.nzp_counter = n.nzp_counter and a.dat_uchet >= n.dat_s and a.dat_uchet <= n.dat_po" +
                "     )"
                , true);


            BillingInstrumentary.ExecSQL(" Create index ix_tpok on tpok (nzp_counter,dat_uchet) ", true);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " tpok ", true);


            #endregion Выборка уникальных счетчиков в tpok с учетом выбранного списка temp_counters&t_selkvar

            #region Подготовка таблиц ограничений периодов для выборки показаний захватывающих расчетный месяц
            //----------------------------------------------------------------
            //заполнение квартирных ПУ (только по открытым лс)
            //----------------------------------------------------------------
            //таблица показаний, где даты показаний
            //ta_mr <= dat_s
            //ta_br >= dat_s
            //ta_b  >  dat_s and < dat_po

            //tb_b  >  dat_po
            //tb_br >= dat_po
            //tb_mr <= dat_po

            BillingInstrumentary.ExecSQL(" Drop table ta_mr ", false);
            BillingInstrumentary.ExecSQL(" Create temp table ta_mr (nzp_counter integer, dat_uchet date) " + DBManager.sUnlogTempTable + " ", true);

            BillingInstrumentary.ExecSQL(
                " Insert into ta_mr (nzp_counter,dat_uchet)" +
                " Select nzp_counter,max(dat_uchet) dat_uchet " +
                " From tpok Where dat_uchet <=" + rashod_prm.dat_s +
                " Group by 1 ");

            BillingInstrumentary.ExecSQL(" Create unique index ix_ta_mr on ta_mr (nzp_counter) ", true);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " ta_mr ", true);

            //специально взято макс справа, чтобы наиболее точно апроксимировать подневной расход ???!!!
            //надо подумать, нафига так делать, ваще не надо!
            BillingInstrumentary.ExecSQL(" Drop table ta_bi ", false);
            BillingInstrumentary.ExecSQL(" Create temp table ta_bi (nzp_counter integer, dat_uchet date) " + DBManager.sUnlogTempTable + " ", true);

            BillingInstrumentary.ExecSQL(
                /*
                " Select nzp_counter,max(dat_uchet) dat_uchet From tpok "+ 
                " Where dat_uchet >=" + rashod.paramcalc.dat_s + 
                " Group by 1 into temp ta_br with no log "
                */
                " Insert into ta_bi (nzp_counter,dat_uchet)" +
                " Select nzp_counter,min(dat_uchet) dat_uchet " +
                " From tpok " +
                " Where dat_uchet > " + rashod_prm.dat_s +
                "   and dat_uchet <=" + rashod_prm.dat_po +
                " Group by 1 ");

            BillingInstrumentary.ExecSQL(" Create unique index ix_ta_bi on ta_bi (nzp_counter) ", true);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " ta_bi ", true);

            BillingInstrumentary.ExecSQL(" Drop table tb_b ", false);
            BillingInstrumentary.ExecSQL(" Create temp table tb_b (nzp_counter integer, dat_uchet date) " + DBManager.sUnlogTempTable + " ", true);

            BillingInstrumentary.ExecSQL(
                " Insert into tb_b (nzp_counter,dat_uchet)" +
                " Select nzp_counter,min(dat_uchet) dat_uchet " +
                " From tpok " +
                " Where dat_uchet > " + rashod_prm.dat_po +
                " Group by 1 "
                , true);

            BillingInstrumentary.ExecSQL(" Create unique index ix_tb_b on tb_b (nzp_counter) ", true);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " tb_b ", true);

            BillingInstrumentary.ExecSQL(" Drop table tb_br ", false);
            BillingInstrumentary.ExecSQL(" Create temp table tb_br (nzp_counter integer, dat_uchet date) " + DBManager.sUnlogTempTable + " ", true);

            BillingInstrumentary.ExecSQL(
                " Insert into tb_br (nzp_counter,dat_uchet)" +
                " Select nzp_counter,min(dat_uchet) dat_uchet " +
                " From tpok " +
                " Where dat_uchet >=" + rashod_prm.dat_po +
                " Group by 1 "
                , true);

            BillingInstrumentary.ExecSQL(" Create unique index ix_tb_br on tb_br (nzp_counter) ", true);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " tb_br ", true);

            BillingInstrumentary.ExecSQL(" Drop table tb_mr ", false);
            BillingInstrumentary.ExecSQL(" Create temp table tb_mr (nzp_counter integer, dat_uchet date) " + DBManager.sUnlogTempTable + " ", true);

            BillingInstrumentary.ExecSQL(
                " Insert into tb_mr (nzp_counter,dat_uchet)" +
                " Select nzp_counter,max(dat_uchet) dat_uchet " +
                " From tpok " +
                " Where dat_uchet <=" + rashod_prm.dat_po +
                " Group by 1 "
                , true);

            BillingInstrumentary.ExecSQL(" Create unique index ix_tb_mr on tb_mr (nzp_counter) ", true);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " tb_mr ", true);

            //показание в середине
            BillingInstrumentary.ExecSQL(" Drop table ta_b ", false);
            BillingInstrumentary.ExecSQL(" Create temp table ta_b (nzp_counter integer, dat_uchet date) " + DBManager.sUnlogTempTable + " ", true);

            BillingInstrumentary.ExecSQL(
                " Insert into ta_b (nzp_counter,dat_uchet)" +
                " Select nzp_counter,min(dat_uchet) dat_uchet " +
                " From tpok " +
                " Where dat_uchet > " + rashod_prm.dat_s +
                "   and dat_uchet < " + rashod_prm.dat_po +
                " Group by 1 "
                , true);

            BillingInstrumentary.ExecSQL(" Create unique index ix_ta_b on ta_b (nzp_counter) ", true);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " ta_b ", true);

            #endregion Подготовка таблиц ограничений периодов для выборки показаний захватывающих расчетный месяц

            #region Выборка показаний захватывающих расчетный месяц t_inscnt

            BillingInstrumentary.ExecSQL(" Drop table t_inscnt ", false);
            BillingInstrumentary.ExecSQL(
                " Create temp table t_inscnt" +
                " ( nzp_cr      serial, " +
                "   nzp_kvar    integer," +
                "   nzp_dom     integer," +
                "   nzp_counter integer," +
                "   nzp_serv    integer," +
                "   dat_s       date,   " +
                "   dat_po      date,   " +
                "   val2        " + DBManager.sDecimalType + "(15,7) default 0.00 not null, " +  // начальное показание ИПУ
                "   ngp_cnt     " + DBManager.sDecimalType + "(15,7) default 0.00 not null, " +  // начальное показание ИПУ
                "   val_s       " + DBManager.sDecimalType + "(15,7) default 0.00 not null, " +  // начальное показание ИПУ
                "   val_po      " + DBManager.sDecimalType + "(15,7) default 0.00 not null  " +  // конечное  показание ИПУ
                " )  " + DBManager.sUnlogTempTable
                , true);

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //выберем показания, которые полностью покрывают месяц!
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //           |      |
            //     ^-------------------^
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            BillingInstrumentary.ExecSQL(
                " Insert into t_inscnt (nzp_cr,nzp_kvar,nzp_dom,nzp_counter,nzp_serv,dat_s,dat_po)" +
                " Select 0,a.nzp_kvar,a.nzp_dom,a.nzp_counter,a.nzp_serv,max(a.dat_uchet),min(b.dat_uchet)" +
                " From tpok a, tpok b" +
                " Where a.nzp_counter = b.nzp_counter" +
                " and a.dat_uchet   < b.dat_uchet" +
                " and a.dat_uchet = ( Select c.dat_uchet From ta_mr c Where c.nzp_counter = a.nzp_counter )" +
                " and b.dat_uchet = ( Select p.dat_uchet From tb_b  p Where p.nzp_counter = b.nzp_counter )" +
                " Group by 1,2,3,4,5 "
                , true);

            BillingInstrumentary.ExecSQL(" Drop table ttt_aid_c1 ", false);
            BillingInstrumentary.ExecSQL(
                " Create temp table ttt_aid_c1 (nzp_counter integer) " + DBManager.sUnlogTempTable
                , true);

            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_aid_c1 (nzp_counter) Select " + DBManager.sUniqueWord + " nzp_counter From t_inscnt "
                , true);

            BillingInstrumentary.ExecSQL(" Create unique index ix_aid22_ttt1 on ttt_aid_c1 (nzp_counter) ", true);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " ttt_aid_c1 ", true);

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //выбор других интервалов, кроме уже выбранных в counters_xx
            //придеться изголяться, чтобы выбрать ближайшие показания (избежать выбора большого интервала)
            //зато понятно
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //           |      |
            //   ^-----------^
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            BillingInstrumentary.ExecSQL(
                " Insert into t_inscnt (nzp_cr,nzp_kvar,nzp_dom,nzp_counter,nzp_serv,dat_s,dat_po)" +
                " Select 0,a.nzp_kvar,a.nzp_dom,a.nzp_counter,a.nzp_serv,max(a.dat_uchet),min(b.dat_uchet)" +
                " From tpok a, tpok b" +
                " Where a.nzp_counter = b.nzp_counter" +
                "   and a.dat_uchet   < b.dat_uchet" +
                "   and not exists ( Select 1 From ttt_aid_c1 n Where a.nzp_counter = n.nzp_counter ) " +
                "   and a.dat_uchet = ( Select c.dat_uchet From ta_mr c Where c.nzp_counter = a.nzp_counter )" +
                "   and b.dat_uchet = ( Select p.dat_uchet From ta_b  p Where p.nzp_counter = b.nzp_counter)" +
                //"   and b.dat_uchet = ( Select p.dat_uchet From tb_mr p Where p.nzp_counter = b.nzp_counter)" +
                " Group by 1,2,3,4,5 "
                , true);

            BillingInstrumentary.ExecSQL(" Drop table ttt_aid_c1 ", false);
            BillingInstrumentary.ExecSQL(
                " Create temp table ttt_aid_c1 (nzp_counter integer) " + DBManager.sUnlogTempTable
                , true);

            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_aid_c1 (nzp_counter) Select " + DBManager.sUniqueWord + " nzp_counter From t_inscnt "
                , true);

            BillingInstrumentary.ExecSQL(" Create unique index ix_aid22_ttt1 on ttt_aid_c1 (nzp_counter) ", true);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " ttt_aid_c1 ", true);


            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //           |      |
            //              ^-------------^
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            BillingInstrumentary.ExecSQL(
                " Insert into t_inscnt (nzp_cr,nzp_kvar,nzp_dom,nzp_counter,nzp_serv,dat_s,dat_po)" +
                " Select 0,a.nzp_kvar,a.nzp_dom,a.nzp_counter,a.nzp_serv,max(a.dat_uchet),min(b.dat_uchet)" +
                " From tpok a, tpok b" +
                " Where a.nzp_counter = b.nzp_counter" +
                "   and a.dat_uchet   < b.dat_uchet" +
                "   and not exists ( Select 1 From ttt_aid_c1 n Where a.nzp_counter = n.nzp_counter ) " +
                //"   and a.dat_uchet = ( Select c.dat_uchet From ta_br c Where c.nzp_counter = a.nzp_counter )" +
                //"   and a.dat_uchet = ( Select c.dat_uchet From ta_b c Where c.nzp_counter = a.nzp_counter )" +
                "   and a.dat_uchet = ( Select c.dat_uchet From ta_bi c Where c.nzp_counter = a.nzp_counter )" +
                "   and b.dat_uchet = ( Select p.dat_uchet From tb_b  p Where p.nzp_counter = b.nzp_counter )" +
                //"   and b.dat_uchet = ( Select p.dat_uchet From tb_br p Where p.nzp_counter = b.nzp_counter )" +
                " Group by 1,2,3,4,5 "
                , true);

            BillingInstrumentary.ExecSQL(" Drop table ttt_aid_c1 ", false);
            BillingInstrumentary.ExecSQL(
                " Create temp table ttt_aid_c1 (nzp_counter integer) " + DBManager.sUnlogTempTable
                , true);

            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_aid_c1 (nzp_counter) Select " + DBManager.sUniqueWord + " nzp_counter From t_inscnt "
                , true);

            BillingInstrumentary.ExecSQL(" Create unique index ix_aid22_ttt1 on ttt_aid_c1 (nzp_counter) ", true);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " ttt_aid_c1 ", true);

            //Надо УБРАТЬ учет таких периодов, ибо фигня получается (ситуация не обработана!!!)
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //           |      |
            //             ^--^
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            BillingInstrumentary.ExecSQL(
                " insert into t_inscnt (nzp_cr,nzp_kvar,nzp_dom,nzp_counter,nzp_serv,dat_s,dat_po)" +
                " Select 0,a.nzp_kvar,a.nzp_dom,a.nzp_counter,a.nzp_serv,max(a.dat_uchet),min(b.dat_uchet)" +
                " From tpok a, tpok b" +
                " Where a.nzp_counter = b.nzp_counter" +
                "   and a.dat_uchet   < b.dat_uchet" +
                "   and not exists ( Select 1 From ttt_aid_c1 n Where a.nzp_counter = n.nzp_counter ) " +
                //"   and a.dat_uchet = ( Select c.dat_uchet From ta_br c Where c.nzp_counter = a.nzp_counter )" +
                "   and a.dat_uchet = ( Select c.dat_uchet From ta_b c Where c.nzp_counter = a.nzp_counter )" +
                "   and b.dat_uchet = ( Select p.dat_uchet From tb_mr p Where p.nzp_counter = b.nzp_counter )" +
                " Group by 1,2,3,4,5 "
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix0_t_inscnt on t_inscnt (nzp_kvar) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix1_t_inscnt on t_inscnt (nzp_counter,dat_s) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix2_t_inscnt on t_inscnt (nzp_counter,dat_po) ", true);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " t_inscnt ", true);

            #endregion Выборка показаний захватывающих расчетный месяц t_inscnt

            #region Выборка начального и конечного показаний ПУ
            //----------------------------------------------------------------
            //заполнение квартирных ПУ
            //----------------------------------------------------------------
            //выбрать все показания по лс
            //надо добавить индекс counters (nzp_counter,dat_uchet)

            BillingInstrumentary.ExecSQL(" Drop table tpok_s ", false);
            BillingInstrumentary.ExecSQL(" Drop table tpok_po ", false);

            BillingInstrumentary.ExecSQL(
                " Create temp table tpok_s " +
                " ( nzp_counter integer," +
                "   val_cnt " + DBManager.sDecimalType + "(16,7)," +
                "   dat_uchet   date    " +
                " ) " + DBManager.sUnlogTempTable
                , true);

            BillingInstrumentary.ExecSQL(
                " Insert into tpok_s (nzp_counter,dat_uchet,val_cnt) " +
                " Select a.nzp_counter," + sDatUchet + " as dat_uchet, max(a.val_cnt) val_cnt " +
                rashod_prm.p_FROM +
                "   and a.nzp_counter = b.nzp_counter " +
                "   and b.dat_s = " + sDatUchet +
                " group by 1,2 "
                , true);

            BillingInstrumentary.ExecSQL(
                " Create temp table tpok_po " +
                " ( nzp_counter integer," +
                "   val_cnt " + DBManager.sDecimalType + "(16,7)," +
                "   dat_uchet   date    " +
                " ) " + DBManager.sUnlogTempTable
                , true);

            BillingInstrumentary.ExecSQL(
                " Insert into tpok_po (nzp_counter,dat_uchet,val_cnt) " +
                " Select a.nzp_counter," + sDatUchet + " as dat_uchet, max(a.val_cnt) val_cnt " +
                rashod_prm.p_FROM +
                "   and a.nzp_counter = b.nzp_counter " +
                "   and b.dat_po = " + sDatUchet +
                " group by 1,2 "
                , true);

            BillingInstrumentary.ExecSQL(" Create unique index ix_tpok_s  on tpok_s  (nzp_counter,dat_uchet) ", true);
            BillingInstrumentary.ExecSQL(" Create unique index ix_tpok_po on tpok_po (nzp_counter,dat_uchet) ", true);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " tpok_s ", true);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " tpok_po ", true);

            #endregion Выборка начального и конечного показаний ПУ

            #region Установка начального и конечного показаний ПУ
            BillingInstrumentary.ExecSQL(
                " Update t_inscnt " +
                " Set val_s = ( Select max(val_cnt) From tpok_s a Where t_inscnt.nzp_counter = a.nzp_counter " +
                                                                  " and t_inscnt.dat_s = a.dat_uchet ) " +
                rashod_prm.p_UPDdt_s +
                " Where 1 = 1 "
                , true);

            BillingInstrumentary.ExecSQL(
                " Update t_inscnt " +
                " Set val_po = ( Select max(val_cnt) From tpok_po a Where t_inscnt.nzp_counter = a.nzp_counter " +
                                                                    " and t_inscnt.dat_po = a.dat_uchet ) " +
                rashod_prm.p_UPDdt_po +
                " Where 1 = 1 "
                , true);

            BillingInstrumentary.ExecSQL(" Drop table tpok_s ", false);
            BillingInstrumentary.ExecSQL(" Drop table tpok_po ", false);
            //ExecSQL(conn_db, " Drop table t_selkvar ", false); -- anes

            #endregion Установка начального и конечного показаний ПУ

            #region Заполнение постоянных таблиц с расходами rashod.counters_xx на основе t_inscnt

            string sql =
                    " Insert into " + rashod_prm.counters_xx +
                    " ( stek,nzp_type,dat_charge,nzp_kvar,nzp_dom,nzp_counter,nzp_serv,dat_s,dat_po,val_s,val_po,val2,ngp_cnt ) " +
                    " Select " + sstek + "," + rashod_prm.p_type + ", " + p_dat_charge + ",nzp_kvar,nzp_dom,nzp_counter,nzp_serv,dat_s,dat_po,val_s,val_po,val2,ngp_cnt " +
                    " From t_inscnt Where 1=1 ";

            BillingInstrumentary.ExecSQL(sql);

            #endregion Заполнение постоянных таблиц с расходами rashod.counters_xx на основе t_inscnt

            #region Удалить временные таблицы tpok t_inscnt ttt_aid_c1

            BillingInstrumentary.ExecSQL(" Drop table tpok ", false);
            BillingInstrumentary.ExecSQL(" Drop table t_inscnt ", false);
            BillingInstrumentary.ExecSQL(" Drop table ttt_aid_c1 ", false);

            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " " + rashod_prm.tab, true);

            #endregion Удалить временные таблицы tpok t_inscnt ttt_aid_c1

            return true;
        }
    }
}
