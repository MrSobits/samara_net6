namespace Bars.Gkh.Gis.DomainService.CalcVerification.Impl
{
    using System;
    using System.Data;
    using B4;
    using Castle.Core.Internal;
    using Dapper;
    using Entities.CalcVerification;
    using Intf;

    using System.ComponentModel;

    using Bars.Gkh.Gis.KP_legacy;

    using Castle.Windsor;

    public class CalcPreparationHandler : ICalcPreparation
    {
        protected IDbConnection Connection;
        protected BillingInstrumentary BillingInstrumentary;
        protected CalcVerificationParams VerificationParams;
        private string tableMOCharges = String.Empty;
        protected CalcTypes.ParamCalc Params;
        protected TempTablesLifeTime TempTablesLifeTime;
        protected IWindsorContainer Container;

        public CalcPreparationHandler(IDbConnection connection,
            BillingInstrumentary billingInstrumentary,
            CalcVerificationParams verificationParams,
            IWindsorContainer container)
        {
            Connection = connection;
            BillingInstrumentary = billingInstrumentary;
            VerificationParams = verificationParams;
            Container = container;
            TempTablesLifeTime = Container.Resolve<TempTablesLifeTime>();
        }

        /// <summary>
        /// Подготовка расчета - определяем выборку ЛС для расчета
        /// </summary>
        /// <param name="Params"></param>
        /// <returns></returns>
        public IDataResult PrepareCalc(ref CalcTypes.ParamCalc Params)
        {
            CreatePreparationsTables();

            GetAllPersonalAccounts(Params);

            GetOpenPersonalAccounts(Params);

            #region переопределим nzp_dom для расчета одного ЛС

            //переопределим nzp_dom
            if (Params.nzp_kvar > 0)
            {
                Params.nzp_dom = Connection.ExecuteScalar<int>(string.Format(" Select nzp_dom From t_selkvar Where nzp_kvar = {0} limit 1", Params.nzp_kvar));
            }
            #endregion переопределим nzp_dom для расчета одного ЛС

            return new BaseDataResult();
        }


        /// <summary>
        /// Получить данные по начислениям их УК - необходимо для дальнейшего расчета
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetMOCharges(ref CalcTypes.ParamCalc param)
        {
            var calcCharge = new CalcTypes.ChargeXX(param);
            var TableMOCharges = "t_charge_gg_" + DateTime.Now.Ticks;
            var sql = " CREATE TEMP TABLE " + TableMOCharges + " AS " +
                      " SELECT ch.*, s.nzp_dom FROM " + calcCharge.charge_xx + " ch" +
                      " JOIN t_selkvar s ON s.nzp_kvar=ch.nzp_kvar" +
                      " WHERE ch.dat_charge IS NULL";
            TempTablesLifeTime.AddTempTable(TableMOCharges, sql);

            BillingInstrumentary.ExecSQL("CREATE INDEX ix1_" + TableMOCharges + " ON " + TableMOCharges + "(nzp_kvar, nzp_serv,nzp_supp )");
            BillingInstrumentary.ExecSQL("CREATE INDEX ix2_" + TableMOCharges + " ON " + TableMOCharges + "(nzp_kvar)");
            BillingInstrumentary.ExecSQL("CREATE INDEX ix3_" + TableMOCharges + " ON " + TableMOCharges + "(nzp_serv)");

            tableMOCharges = TableMOCharges;
            return TableMOCharges;
        }

        /// <summary>
        /// выборка открытых ЛС - t_opn
        /// </summary>
        /// <param name="Params"></param>
        private void GetOpenPersonalAccounts(CalcTypes.ParamCalc Params)
        {
            //+++++++++++++++++++++++++++++++++++++++++++++++++++
            // выбрать открытые лицевые счета по t_selkvar
            //+++++++++++++++++++++++++++++++++++++++++++++++++++

            BillingInstrumentary.ExecSQL(
                " Insert into ttt_prm_3 (nzp, nzp_dom, num_ls, dat_s, dat_po)" +
                " Select k.nzp_kvar, k.nzp_dom, k.num_ls, " +
                DBManager.sNvlWord + "(p.dat_s," + DBManager.MDY(12, 31, 1899) + "), " + DBManager.sNvlWord +
                "(p.dat_po," +
                DBManager.MDY(1, 1, 3000) + ") " +
                " From t_selkvar k," + Params.data_alias + "prm_3 p" +
                " Where k.nzp_kvar=p.nzp and p.nzp_prm=51 and p.val_prm in ('1','3') " +
                "   and p.is_actual<>100 " +
                "   and p.dat_s  <= " + Params.dat_po +
                "   and p.dat_po >= " + Params.dat_s +
                " group by 1,2,3,4,5 "
                );

            BillingInstrumentary.ExecSQL(" Create unique index ix1_ttt_prm_3 on ttt_prm_3 (nzp,nzp_dom,num_ls) ", false);
            BillingInstrumentary.ExecSQL(" Create        index ix2_ttt_prm_3 on ttt_prm_3 (nzp,dat_s,dat_po) ", false);
            BillingInstrumentary.ExecSQL(" Create        index ix3_ttt_prm_3 on ttt_prm_3 (dat_s,dat_po) ", false);

            bool bDayCalc =
                CheckValBoolPrmWithVal(Params.data_alias, 89, "10", "1", Params.dat_s, Params.dat_po);

            if (bDayCalc)
            {
                BillingInstrumentary.ExecSQL(
                    " Insert into t_opn (nzp_kvar, nzp_dom, num_ls, dat_s, dat_po, is_day_calc) " +
                    " Select k.nzp, k.nzp_dom, k.num_ls, min(k.dat_s), max(k.dat_po), 1 " +
                    " From ttt_prm_3 k " +
                    " group by 1,2,3 "
                    );

                BillingInstrumentary.ExecSQL(" Create unique index ix1_t_opn on t_opn (nzp_kvar) ", false);
            }
            else
            {
                BillingInstrumentary.ExecSQL(
                    " Insert into t_opn (nzp_kvar, nzp_dom, num_ls, dat_s, dat_po, is_day_calc) " +
                    " Select k.nzp, k.nzp_dom, k.num_ls, min(k.dat_s), max(k.dat_po), 1 " +
                    " From ttt_prm_3 k, " + Params.data_alias + "prm_1 p " +
                    " Where k.nzp=p.nzp and p.nzp_prm=90 and p.val_prm='1' " +
                    "   and p.is_actual<>100 " +
                    "   and p.dat_s  <= " + Params.dat_po +
                    "   and p.dat_po >= " + Params.dat_s +
                    " group by 1,2,3 "
                    );

                BillingInstrumentary.ExecSQL(" Create unique index ix1_t_opn on t_opn (nzp_kvar) ", false);
                BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " t_opn ");

                BillingInstrumentary.ExecSQL(
                    " Insert into t_opn (nzp_kvar, nzp_dom, num_ls, dat_s, dat_po, is_day_calc) " +
                    " Select k.nzp, k.nzp_dom, k.num_ls, min(k.dat_s), max(k.dat_po), 0 " +
                    " From ttt_prm_3 k " +
                    " where NOT EXISTS (SELECT 1 From t_opn n Where k.nzp=n.nzp_kvar ) " +
                    " group by 1,2,3 "
                    );
            }

            BillingInstrumentary.ExecSQL(" Create        index ix2_t_opn on t_opn (num_ls) ", false);
            BillingInstrumentary.ExecSQL(" Create        index ix3_t_opn on t_opn (nzp_dom) ", false);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " t_opn ");
        }

        /// <summary>
        /// Выбрать множество лицевых счетов для расчета
        /// </summary>
        /// <param name="Params"></param>
        private void GetAllPersonalAccounts(CalcTypes.ParamCalc Params)
        {
            //выбрать множество лицевых счетов
            string s_find =
                " From " + Params.data_alias + "kvar k " +
                " Where " + Params.where_z;

            if (Params.nzp_kvar > 0)
            {
                s_find =
                    " From " + Params.data_alias + "kvar k " +
                    " Where k.nzp_dom=" + Params.nzp_dom;

                // ... для расчета связанные дома ... 
                int nzp_dom_base = 0;
                try
                {
                    nzp_dom_base =
                        Connection.ExecuteScalar<int>(" Select max(nzp_dom_base) From " + Params.data_alias +
                                                      "link_dom_lit p " +
                                                      " Where nzp_dom=" + Params.nzp_dom);
                }
                catch
                {
                    nzp_dom_base = 0;
                }

                if (nzp_dom_base > 0)
                {
                    s_find =
                        " From " + Params.data_alias + "kvar k, " + Params.data_alias + "link_dom_lit l " +
                        " Where k.nzp_dom=l.nzp_dom and l.nzp_dom_base=" + nzp_dom_base;
                }
            }
            if (Params.nzp_kvar > 0)
            {
                s_find =
                    " From " + Params.data_alias + "kvar k " +
                    " Where k.nzp_kvar=" + Params.nzp_kvar;
            }
            BillingInstrumentary.ExecSQL(
                " Insert into t_selkvar (nzp_kvar,num_ls,nzp_dom, nzp_area,nzp_geu) " +
                " Select k.nzp_kvar,k.num_ls,k.nzp_dom,k.nzp_area,k.nzp_geu " + s_find
                );
            BillingInstrumentary.ExecSQL(" Create unique index ix1_t_selkvar on t_selkvar (nzp_key) ", false);
            BillingInstrumentary.ExecSQL(" Create unique index ix2_t_selkvar on t_selkvar (nzp_kvar) ", false);
            BillingInstrumentary.ExecSQL(" Create        index ix3_t_selkvar on t_selkvar (num_ls) ", false);
            BillingInstrumentary.ExecSQL(" Create        index ix4_t_selkvar on t_selkvar (nzp_dom) ", false);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " t_selkvar ");
        }

        /// <summary>
        /// Создаем таблицы для подготовка выборки ЛС
        /// </summary>
        private void CreatePreparationsTables()
        {
            TempTablesLifeTime.DropTempTable("t_opn");
            TempTablesLifeTime.DropTempTable("t_selkvar");
            TempTablesLifeTime.DropTempTable("ttt_prm_3");

            var ssql = " Create temp table ttt_prm_3 " +
                       " ( nzp      integer, " +
                       "   nzp_dom  integer, " +
                       "   num_ls   integer, " +
                       "   dat_s    date not null, " +
                       "   dat_po   date not null  " +
                       " )";
            TempTablesLifeTime.AddTempTable("ttt_prm_3", ssql);


            ssql =
                " Create temp table t_selkvar " +
                " ( nzp_key   serial not null, " +
                "   nzp_kvar  integer, " +
                "   num_ls    integer, " +
                "   nzp_dom   integer, " +
                "   nzp_area  integer, " +
                "   nzp_geu   integer,  " +
                "   need_calc   bool  " +
                " )";
            TempTablesLifeTime.AddTempTable("t_selkvar", ssql);


            ssql =
                " Create temp table t_opn " +
                " ( nzp_kvar integer," +
                "   nzp_dom  integer," +
                "   num_ls   integer, " +
                "   is_day_calc integer default 0, " +
                "   dat_s    date not null," +
                "   dat_po   date not null" +
                " )";
            TempTablesLifeTime.AddTempTable("t_opn", ssql);

        }

        #region Методы перенесенные из KP50

        #region Проверка наличия логического параметра в нужной базе, нужной таблице prm_5/10, по дате, значению параметра

        //--------------------------------------------------------------------------------
        public bool CheckValBoolPrmWithVal(string pDataAls, int pNzpPrm, string pNumPrm, string pValPrm, string pDatS,
            string pDatPo)
        //--------------------------------------------------------------------------------
        {
            var sql = " Select count(*) cnt From " + pDataAls + "prm_" + pNumPrm.Trim() + " p " +
            " Where p.nzp_prm = " + pNzpPrm + " and p.val_prm='" + pValPrm.Trim() + "' " +
            " and p.is_actual <> 100 and p.dat_s  <= " + pDatPo + " and p.dat_po >= " + pDatS + " ";
            bool bRetVal;
            try
            {
                bRetVal = Connection.ExecuteScalar<int>(sql) > 0;
            }
            catch
            {
                bRetVal = false;
            }
            return bRetVal;
        }

        #endregion Проверка наличия логического параметра в нужной базе, нужной таблице prm_5/10, по дате, значению параметра

        #endregion


        /// <summary>
        /// Загрузка параметров необходимых для расчета
        /// </summary>
        /// <param name="paramcalc"></param>
        /// <returns></returns>
        public IDataResult LoadTempTablesForMonth(ref CalcTypes.ParamCalc paramcalc)
        //--------------------------------------------------------------------------------
        {
            //параметры по лс: prm_1, prm_3
            LoadTempTablesAccountParams(paramcalc);
            //параметры по дому: prm_2
            LoadTempTablesHouseParams(paramcalc);
            //периоды t_calc_gku
            GetCalcPeriodsByAccounts(paramcalc);
            return new BaseDataResult();
        }

        private void LoadTempTablesAccountParams(CalcTypes.ParamCalc paramcalc)
        {
            string s;
            //prm_1 - параметры ЛС
            string sp;
            if (paramcalc.nzp_kvar > 0 || paramcalc.nzp_dom > 0)
                sp = "  ,t_selkvar b Where a.nzp = b.nzp_kvar ";
            else
                sp = "  Where 1 = 1 ";


            TempTablesLifeTime.AddTempTable("ttt_prm_1f",
                " CREATE TEMP TABLE ttt_prm_1f (" +
                "   nzp_key serial NOT NULL," +
                "   nzp     integer," +
                "   nzp_prm integer," +
                "   val_prm character(20)," +
                "   dat_s      date," +
                "   dat_po     date " +
                " ) "
                );

            BillingInstrumentary.ExecSQL(
                " insert into ttt_prm_1f (nzp_key,nzp,nzp_prm,val_prm,dat_s,dat_po) " +
                " Select a.nzp_key, a.nzp, a.nzp_prm," +
                " replace( " + DBManager.sNvlWord + "(a.val_prm,'0'), ',', '.') val_prm, " +
                DBManager.sNvlWord + "(a.dat_s, " + DBManager.MDY(1, 1, 1901) + ") as dat_s , " +
                DBManager.sNvlWord + "(a.dat_po, " + DBManager.MDY(1, 1, 3000) + ") as dat_po " +
                " From " + paramcalc.data_alias + "prm_1 a " +
                sp + " and a.is_actual <> 100 "
                );

            BillingInstrumentary.ExecSQL(" Create unique index ix1_ttt_prm_1f on ttt_prm_1f (nzp_key) ");
            BillingInstrumentary.ExecSQL(" Create index ix2_ttt_prm_1f on ttt_prm_1f (nzp,dat_s,dat_po) ");
            BillingInstrumentary.ExecSQL(" Create index ix3_ttt_prm_1f on ttt_prm_1f (dat_s,dat_po) ");

            //prm_1
            if (paramcalc.nzp_kvar > 0 || paramcalc.nzp_dom > 0)
                s = "  ,t_selkvar b Where a.nzp = b.nzp_kvar ";
            else
                s = "  Where 1 = 1 ";

            TempTablesLifeTime.AddTempTable("ttt_prm_1",
                " CREATE TEMP TABLE ttt_prm_1 (" +
                "   nzp_key serial NOT NULL," +
                "   nzp     integer," +
                "   nzp_prm integer," +
                "   val_prm character(20)," +
                "   dat_s      date," +
                "   dat_po     date " +
                " ) "
                );

            TempTablesLifeTime.AddTempTable("ttt_prm_1d",
                " CREATE TEMP TABLE ttt_prm_1d (" +
                "   nzp_key serial NOT NULL," +
                "   nzp     integer," +
                "   nzp_prm integer," +
                "   val_prm character(20)," +
                "   dat_s      date," +
                "   dat_po     date " +
                " ) "
                );

            BillingInstrumentary.ExecSQL(
                " insert into ttt_prm_1d (nzp_key,nzp,nzp_prm,val_prm,dat_s,dat_po) " +
                " Select min(a.nzp_key) nzp_key,a.nzp,a.nzp_prm,a.val_prm,a.dat_s,a.dat_po " +
                " From ttt_prm_1f a " +
                s +
                "   and a.dat_s  <= " + paramcalc.dat_po +
                "   and a.dat_po >= " + paramcalc.dat_s +
                " group by 2,3,4,5,6 "
                );

            BillingInstrumentary.ExecSQL(
                " Create index ix1_ttt_prm_1d on ttt_prm_1d (nzp,nzp_prm,val_prm,dat_s,dat_po) ");
            BillingInstrumentary.ExecSQL(" Create index ix2_ttt_prm_1d on ttt_prm_1d (nzp_prm) ");


            BillingInstrumentary.ExecSQL(
                " insert into ttt_prm_1 (nzp_key,nzp,nzp_prm,val_prm,dat_s,dat_po) " +
                " Select min(a.nzp_key) nzp_key,a.nzp,a.nzp_prm,max(a.val_prm) val_prm,min(a.dat_s) dat_s,max(a.dat_po) dat_po " +
                " From ttt_prm_1d a " +
                " group by 2,3 "
                );

            BillingInstrumentary.ExecSQL(" Create index ix1_ttt_prm_1 on ttt_prm_1 (nzp,nzp_prm) ");
            BillingInstrumentary.ExecSQL(" Create index ix2_ttt_prm_1 on ttt_prm_1 (nzp_prm) ");
        }

        /// <summary>
        /// Получение периодов расчета по лицевым счетам
        /// </summary>
        /// <param name="paramcalc"></param>
        private void GetCalcPeriodsByAccounts(CalcTypes.ParamCalc paramcalc)
        {
            TempTablesLifeTime.AddTempTable("t_gku_periods",
                " Create temp table t_gku_periods " +
                " ( nzp_period serial, " +
                "   nzp_kvar   integer, " +
                "   dp       " + DBManager.sDateTimeType + ", " +
                "   dp_end   " + DBManager.sDateTimeType + ", " +
                "   typ  integer, " +
                "   cntd integer," +
                "   cntd_mn integer " +
                " )"
                );

            BillingInstrumentary.ExecSQL(
                " insert into t_gku_periods (nzp_kvar,typ,dp,dp_end,cntd,cntd_mn) " +
                " select k.nzp_kvar,0," + paramcalc.dat_s + "," + paramcalc.dat_po + ", " +
                DateTime.DaysInMonth(paramcalc.calc_yy, paramcalc.calc_mm) + ", " +
                DateTime.DaysInMonth(paramcalc.calc_yy, paramcalc.calc_mm) +
                " From t_opn k " +
                " where NOT EXISTS (select 1 From t_gku_periods t Where k.nzp_kvar=t.nzp_kvar ) "
                );

            BillingInstrumentary.ExecSQL(" Create index ix1_t_gku_periods on t_gku_periods (nzp_kvar) ", false);
            BillingInstrumentary.ExecSQL(" Create unique index ix0_t_gku_periods on t_gku_periods (nzp_period) ", false);
            BillingInstrumentary.ExecSQL(" Create index ix2_t_gku_periods on t_gku_periods (nzp_kvar,dp,dp_end) ", false);
        }

        /// <summary>
        /// Домовые параметры
        /// </summary>
        /// <param name="paramcalc"></param>
        /// <param name="sp"></param>
        /// <param name="s"></param>
        private void LoadTempTablesHouseParams(CalcTypes.ParamCalc paramcalc)
        {
            #region Домовые параметры

            var s = String.Empty;
            //prm_2
            if (paramcalc.nzp_kvar > 0 || paramcalc.nzp_dom > 0)
                s = " Where Exists (select 1 from t_selkvar b Where a.nzp = b.nzp_dom) ";
            else
                s = " Where 1 = 1 ";

            var sp = String.Empty;
            //prm_2 - параметры домов
            if (paramcalc.nzp_kvar > 0 || paramcalc.nzp_dom > 0)
                sp = "  ,t_selkvar b Where a.nzp = b.nzp_dom ";
            else
                sp = "  Where 1 = 1 ";

            BillingInstrumentary.ExecSQL(
                " CREATE TEMP TABLE ttt_prm_2f (" +
                "   nzp_key serial NOT NULL," +
                "   nzp     integer," +
                "   nzp_prm integer," +
                "   val_prm character(20)," +
                "   dat_s      date," +
                "   dat_po     date " +
                " ) ");


            BillingInstrumentary.ExecSQL(
                " insert into ttt_prm_2f (nzp_key,nzp,nzp_prm,val_prm,dat_s,dat_po) " +
                " Select DISTINCT " +
                " a.nzp_key, a.nzp, a.nzp_prm," +
                " replace(COALESCE(a.val_prm,'0'), ',', '.') val_prm, " +
                "COALESCE(a.dat_s, " + BillingInstrumentary.MDY(1, 1, 1901) + ") as dat_s , " +
                "COALESCE(a.dat_po, " + BillingInstrumentary.MDY(1, 1, 3000) + " ) as dat_po " +
                " From " + paramcalc.data_alias + "prm_2 a " +
                sp + " and a.is_actual <> 100 ");


            BillingInstrumentary.ExecSQL(" Create unique index ix1_ttt_prm_2f on ttt_prm_2f (nzp_key) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix2_ttt_prm_2f on ttt_prm_2f (nzp,dat_s,dat_po) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix3_ttt_prm_2f on ttt_prm_2f (dat_s,dat_po) ", true);


            TempTablesLifeTime.AddTempTable("ttt_prm_2",
                " CREATE TEMP TABLE ttt_prm_2 (" +
                "   nzp_key serial NOT NULL," +
                "   nzp     integer," +
                "   nzp_prm integer," +
                "   val_prm character(20)," +
                "   dat_s      date," +
                "   dat_po     date " +
                " ) ");


            TempTablesLifeTime.AddTempTable("ttt_prm_2d",
                " CREATE TEMP TABLE ttt_prm_2d (" +
                "   nzp_key serial NOT NULL," +
                "   nzp     integer," +
                "   nzp_prm integer," +
                "   val_prm character(20)," +
                "   dat_s      date," +
                "   dat_po     date " +
                " ) ");


            BillingInstrumentary.ExecSQL(
                " insert into ttt_prm_2d (nzp_key,nzp,nzp_prm,val_prm,dat_s,dat_po) " +
                " Select min(a.nzp_key) nzp_key,a.nzp,a.nzp_prm,a.val_prm,a.dat_s,a.dat_po " +
                " From ttt_prm_2f a " +
                s +
                "   and a.dat_s  <= " + paramcalc.dat_po +
                "   and a.dat_po >= " + paramcalc.dat_s +
                " group by 2,3,4,5,6 "
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix1_ttt_prm_2d on ttt_prm_2d (nzp,nzp_prm,val_prm,dat_s,dat_po) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix2_ttt_prm_2d on ttt_prm_2d (nzp_prm) ", true);


            BillingInstrumentary.ExecSQL(
                " insert into ttt_prm_2 (nzp_key,nzp,nzp_prm,val_prm,dat_s,dat_po) " +
                " Select min(a.nzp_key) nzp_key,a.nzp,a.nzp_prm,max(a.val_prm) val_prm,min(a.dat_s) dat_s,max(a.dat_po) dat_po " +
                " From ttt_prm_2d a " +
                " group by 2,3 "
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix1_ttt_prm_2 on ttt_prm_2 (nzp,nzp_prm) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix2_ttt_prm_2 on ttt_prm_2 (nzp_prm) ", true);

            #endregion  Домовые параметры
        }

        /// <summary>
        /// Чистим старые расчеты
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IDataResult ClearData(ref CalcTypes.ParamCalc param)
        {
            var tenantInfo = new DbCalcCharge.GilecXX(param);
            var CalcServicesTable = string.Format("{0}_charge_{1}.chd_calc_gku_{2}", param.pref, param.calc_yy % 1000, param.calc_mm.ToString("00"));
            var ConsumptionsTable = string.Format("{0}_charge_{1}.chd_counters_{2}", param.pref, param.calc_yy % 1000, param.calc_mm.ToString("00"));
            var ChargesTable = string.Format("{0}_charge_{1}.chd_charge_{2}", param.pref, param.calc_yy % 1000, param.calc_mm.ToString("00"));


            var sql = " DELETE FROM " + tenantInfo.gil_xx + " g " +
                      " WHERE EXISTS (SELECT 1 FROM t_selkvar t " +
                      "                 WHERE g.nzp_kvar=t.nzp_kvar)";
            ExecSQLWithCheckExist(sql, param, CalcVerificationTables.TenantTable);

            sql = " DELETE FROM " + ConsumptionsTable + " c " +
                  " WHERE EXISTS (SELECT 1 FROM t_selkvar t " +
                  "                 WHERE c.nzp_dom=t.nzp_dom)";
            ExecSQLWithCheckExist(sql, param, CalcVerificationTables.ConsumptionsTable);


            sql = " DELETE FROM " + CalcServicesTable + " c " +
                  " WHERE EXISTS (SELECT 1 FROM t_selkvar t " +
                  "                 WHERE c.nzp_kvar=t.nzp_kvar)";
            ExecSQLWithCheckExist(sql, param, CalcVerificationTables.CalcServicesTable);

            sql = " DELETE FROM " + ChargesTable + " c " +
                  " WHERE EXISTS (SELECT 1 FROM t_selkvar t " +
                  "                 WHERE c.nzp_kvar=t.nzp_kvar)";
            ExecSQLWithCheckExist(sql, param, CalcVerificationTables.ChargesTable);


            return new BaseDataResult();
        }

        /// <summary>
        /// Выполнение запроса с проверкой на существование таблиц
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="tables">таблицы для проверки</param>
        private void ExecSQLWithCheckExist(string sql, CalcTypes.ParamCalc param, CalcVerificationTables tables)
        {
            try
            {
                BillingInstrumentary.ExecSQL(sql);
            }
            catch (Exception)
            {
                foreach (CalcVerificationTables calcTable in Enum.GetValues(typeof(CalcVerificationTables)))
                {
                    if (tables.HasFlag(calcTable))
                    {
                        CreateCalcVerificationTable(param, calcTable);
                    }
                }
                BillingInstrumentary.ExecSQL(sql);
            }
        }

        /// <summary>
        /// Создание структуры в базе данных по типу таблицы
        /// </summary>
        /// <param name="param"></param>
        /// <param name="table"></param>
        private void CreateCalcVerificationTable(CalcTypes.ParamCalc param, CalcVerificationTables table)
        {
            switch (table)
            {
                case CalcVerificationTables.TenantTable: CreateTenantsTable(param); break;
                case CalcVerificationTables.ConsumptionsTable: CreateConsumptionsTable(Connection, param); break;
                case CalcVerificationTables.CalcServicesTable: CreateCalcServicesTable(Connection, param); break;
                case CalcVerificationTables.ChargesTable: CreateChargesTable(Connection, param); break;
                default: throw new InvalidEnumArgumentException("Не определен тип таблицы для проверочного расчета");
            }
        }




        /// <summary>
        /// Создание таблицы calc_gku_xx
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="paramCalc"></param>
        private void CreateCalcServicesTable(IDbConnection connection, CalcTypes.ParamCalc paramCalc)
        {
            if (paramCalc.calc_yy == 0) throw new Exception("Не заполнен расченый год");
            if (paramCalc.calc_mm == 0) throw new Exception("Не заполнен расченый месяц");
            if (VerificationParams.Pref.IsNullOrEmpty()) throw new Exception("Не заполнен префикс");

            var chargeChdCalcGku = string.Format(@"{0}_charge_{1}.chd_calc_gku_{2}",
                VerificationParams.Pref, paramCalc.calc_yy % 1000, paramCalc.calc_mm.ToString("00"));
            var sql = string.Format(@"
                CREATE TABLE {0}
                (
                  chd_calc_gku_id serial NOT NULL,
                  chd_counters_id integer,
                  nzp_wp integer NOT NULL,
                  nzp_dom integer NOT NULL,
                  nzp_kvar integer NOT NULL,
                  nzp_serv integer NOT NULL,
                  nzp_supp integer NOT NULL,
                  nzp_frm integer NOT NULL DEFAULT 0,
                  stek integer NOT NULL DEFAULT 3,
                  dat_s date NOT NULL DEFAULT '1900-01-01'::date,
                  dat_po date NOT NULL DEFAULT '1900-01-01'::date,
                  is_device integer NOT NULL DEFAULT 0,
                  squ1 numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  squ2 numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  gil numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  tarif numeric(17,7) NOT NULL DEFAULT 0.00,
                  tarif_chd numeric(17,7) NOT NULL DEFAULT 0.00,
                  tarif_gkal numeric(17,7) NOT NULL DEFAULT 0.00,
                  tarif_gkal_chd numeric(17,7) NOT NULL DEFAULT 0.00,
                  rashod numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  rashod_chd numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  rash_norm_one numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  rash_norm_one_chd numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  valm numeric(15,7) NOT NULL DEFAULT 0.0000000,
                  valm_chd numeric(15,7) NOT NULL DEFAULT 0.0000000,
                  dop87 numeric(15,7) NOT NULL DEFAULT 0.0000000,
                  dop87_chd numeric(15,7) NOT NULL DEFAULT 0.0000000,
                  rashod_link numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  rashod_link_chd numeric(14,7) NOT NULL DEFAULT 0.0000000
                )
                WITH (
                  OIDS=TRUE
                );
                ", chargeChdCalcGku);
            connection.Execute(sql);

            sql = string.Format(@"CREATE UNIQUE INDEX ix1{0} ON {1} (chd_calc_gku_id);", chargeChdCalcGku.Replace(".", ""), chargeChdCalcGku);
            connection.Execute(sql);

            sql = string.Format(@"CREATE  INDEX ix2{0} ON {1} (nzp_kvar, stek, dat_s, dat_po);", chargeChdCalcGku.Replace(".", ""), chargeChdCalcGku);
            connection.Execute(sql);

            sql = string.Format(@"CREATE  INDEX ix3{0} ON {1} (nzp_kvar, nzp_frm);", chargeChdCalcGku.Replace(".", ""), chargeChdCalcGku);
            connection.Execute(sql);

            sql = string.Format(@"CREATE  INDEX ix4{0} ON {1} (nzp_dom);", chargeChdCalcGku.Replace(".", ""), chargeChdCalcGku);
            connection.Execute(sql);
        }
        /// <summary>
        /// Создание таблицы charge_xx
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="paramCalc"></param>
        public void CreateChargesTable(IDbConnection connection, CalcTypes.ParamCalc paramCalc, bool isChd = false)
        {
            if (paramCalc.calc_yy == 0) throw new Exception("Не заполнен расченый год");
            if (paramCalc.calc_mm == 0) throw new Exception("Не заполнен расченый месяц");
            if (VerificationParams.Pref.IsNullOrEmpty()) throw new Exception("Не заполнен префикс");

            string chargeChdCharge = string.Format(@"{0}_charge_{1}.chd_charge_{2}",
                isChd ? "chd" : VerificationParams.Pref, paramCalc.calc_yy % 1000, paramCalc.calc_mm.ToString("00"));
            var sql = string.Format(@"
                CREATE TABLE {0}
                (
                  chd_charge_id serial NOT NULL,
                  billing_house_code bigint NOT NULL,
                  service character varying(250),
                  supplier character varying(250),
                  measure character varying(250),
                  formula character varying(250),
                  chd_point_id integer NOT NULL,
                  squ1 numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  gil numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  rash_norm_one numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  rash_norm_one_chd numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  nzp_kvar integer,
                  nzp_dom integer,
                  num_ls integer,
                  nzp_serv integer,
                  nzp_supp integer,
                  nzp_measure integer,
                  nzp_frm integer,
                  dat_charge date,
                  is_device integer DEFAULT 0,
                  tarif numeric(14,3) DEFAULT 0,
                  tarif_chd numeric(14,3) DEFAULT 0,
                  c_calc numeric(14,5) DEFAULT 0,
                  c_calc_chd numeric(14,5) DEFAULT 0,
                  rsum_tarif numeric(14,2) DEFAULT 0,
                  rsum_tarif_chd numeric(14,2) DEFAULT 0,
                  sum_nedop numeric(14,2) DEFAULT 0,
                  sum_nedop_chd numeric(14,2) DEFAULT 0,
                  sum_real numeric(14,2) DEFAULT 0,
                  sum_real_chd numeric(14,2) DEFAULT 0,
                  reval numeric(14,2) DEFAULT 0,
                  reval_chd numeric(14,2) DEFAULT 0,
                  real_charge numeric(14,2) DEFAULT 0,
                  sum_insaldo numeric(14,2) DEFAULT 0,
                  sum_money numeric(14,2) DEFAULT 0,
                  sum_outsaldo numeric(14,2) DEFAULT 0,
                  sum_outsaldo_chd numeric(14,2) DEFAULT 0,
                  sum_charge numeric(14,2) DEFAULT 0
                )
                WITH (
                  OIDS=TRUE
                ); ", chargeChdCharge);

            connection.Execute(sql);

            sql = string.Format(@"CREATE UNIQUE INDEX ix1{0} ON {1} (chd_charge_id);", chargeChdCharge.Replace(".", ""), chargeChdCharge);
            connection.Execute(sql);

            sql = string.Format(@"CREATE INDEX ix2{0} ON {1} (num_ls, nzp_supp, nzp_serv);", chargeChdCharge.Replace(".", ""), chargeChdCharge);
            connection.Execute(sql);

            sql = string.Format(@"CREATE INDEX ix3{0} ON {1} (nzp_kvar, nzp_serv, dat_charge);", chargeChdCharge.Replace(".", ""), chargeChdCharge);
            connection.Execute(sql);

            sql = string.Format(@"CREATE INDEX ix4{0} ON {1} (nzp_kvar, nzp_serv, nzp_supp);", chargeChdCharge.Replace(".", ""), chargeChdCharge);
            connection.Execute(sql);

            sql = string.Format(@"CREATE INDEX ix5{0} ON {1} (nzp_kvar, nzp_supp, dat_charge);", chargeChdCharge.Replace(".", ""), chargeChdCharge);
            connection.Execute(sql);

            sql = string.Format(@"CREATE INDEX ix6{0} ON {1} (nzp_serv);", chargeChdCharge.Replace(".", ""), chargeChdCharge);
            connection.Execute(sql);

        }
        /// <summary>
        /// Создание таблицы counters_xx
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="paramCalc"></param>
        private void CreateConsumptionsTable(IDbConnection connection, CalcTypes.ParamCalc paramCalc)
        {
            if (paramCalc.calc_yy == 0) throw new Exception("Не заполнен расченый год");
            if (paramCalc.calc_mm == 0) throw new Exception("Не заполнен расченый месяц");
            if (VerificationParams.Pref.IsNullOrEmpty()) throw new Exception("Не заполнен префикс");

            var chargeChdCounters = string.Format(@"{0}_charge_{1}.chd_counters_{2}",
                VerificationParams.Pref, paramCalc.calc_yy % 1000, paramCalc.calc_mm.ToString("00"));

            var sql = string.Format(@"
               CREATE TABLE {0} (
                      chd_counters_id serial NOT NULL,
                      nzp_wp integer NOT NULL,
                      nzp_dom integer NOT NULL,
                      nzp_serv integer NOT NULL,
                      nzp_frmd integer NOT NULL,
                      stek integer NOT NULL DEFAULT 0,
                      nzp_type integer NOT NULL,
                      cnt_stage integer DEFAULT 0,
                      cls integer NOT NULL DEFAULT 0,
                      gil numeric(14,7) DEFAULT 0.0000000,
                      val1 numeric(14,7) DEFAULT 0.0000000,
                      val2 numeric(14,7) DEFAULT 0.0000000,
                      val2_dop numeric(14,7) DEFAULT 0.0000000,
                      val1_dop numeric(14,7) DEFAULT 0.0000000,
                      squ1_norm numeric(14,7) DEFAULT 0.0000000,
                      squ1 numeric(14,7) DEFAULT 0.0000000,
                      squ2 numeric(14,7) DEFAULT 0.0000000,
                      squ_mop numeric(14,7) DEFAULT 0.0000000,
                      norm_one numeric(15,7) NOT NULL DEFAULT 0,
                      norm_one_chd numeric(15,7) NOT NULL DEFAULT 0,
                      rashod numeric(14,7) NOT NULL DEFAULT 0.0000000,
                      rashod_chd numeric(14,7) NOT NULL DEFAULT 0.0000000,
                      dop87 numeric(15,7) NOT NULL DEFAULT 0,
                      dop87_chd numeric(15,7) NOT NULL DEFAULT 0,
                      kf307 numeric(15,7) DEFAULT 0.0000000,
                      kf307n numeric(15,7) DEFAULT 0.0000000,
                      kf307_chd numeric(15,7) DEFAULT 0.0000000,
                      kf307n_chd numeric(15,7) DEFAULT 0.0000000,
                      kf_dpu_prop numeric(15,7) DEFAULT 0.0000000,
                      kf_dpu_prop_chd numeric(15,7) DEFAULT 0.0000000,
                      kod_info integer DEFAULT 0,
                      kod_info_chd integer DEFAULT 0
                    )
                    WITH (
                      OIDS=FALSE
                    );
                ", chargeChdCounters);
            connection.Execute(sql);

            sql = string.Format(@"CREATE UNIQUE INDEX ix1{0} ON {1}  (chd_counters_id);", chargeChdCounters.Replace(".", ""), chargeChdCounters);
            connection.Execute(sql);
            sql = string.Format(@"CREATE INDEX ix2{0} ON {1}  (nzp_type, stek, nzp_dom);", chargeChdCounters.Replace(".", ""), chargeChdCounters);
            connection.Execute(sql);


        }
        /// <summary>
        /// Создание таблицы gil_xx
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="paramCalc"></param>
        void CreateTenantsTable(CalcTypes.ParamCalc paramCalc)
        {
            if (paramCalc.calc_yy == 0) throw new Exception("Не заполнен расченый год");
            if (paramCalc.calc_mm == 0) throw new Exception("Не заполнен расченый месяц");
            if (VerificationParams.Pref.IsNullOrEmpty()) throw new Exception("Не заполнен префикс");
            var gilTable = string.Format("{0}_charge_{1}.gil_{2}", VerificationParams.Pref, paramCalc.calc_yy % 1000, paramCalc.calc_mm.ToString("00"));

            BillingInstrumentary.ExecSQL(
                " Create table " + gilTable +
                " (  nzp_gx      serial        not null, " +
                "    nzp_dom     integer       not null, " +
                "    nzp_kvar    integer       default 0 not null, " +
                "    dat_charge  date, " +
                "    cur_zap     integer       default 0 not null, " +//0-текущее значение, >0 - ссылка на следующее значение (nzp_cntx)
                "    prev_zap    integer       default 0 not null, " +//0-текущее значение, >0 - ссылка на предыдыущее значение (nzp_cntx)
                "    nzp_gil     integer       default 0 not null, " + //
                "    dat_s       date, " +
                "    dat_po      date, " +
                "    stek        integer       default 0 not null, " + //
                "    cnt1        integer       default 0 not null, " + //кол-во жильцов в лс с учетом времен. выбывших
                "    cnt2        integer       default 0 not null, " + //кол-во жильцов в лс без учета времен. выбывших
                "    cnt3        integer       default 0 not null, " + //кол-во дней cnt1 x 31 (кол-во дней проживания)
                "    val1        " + DBManager.sDecimalType + "(11,7) default 0.00, " +//итоговое кол-во жильцов в лс с учетом времен. выбывших
                    "    val2        " + DBManager.sDecimalType + "(11,7) default 0.00, " + //nzp_prm = 5
                    "    val3        " + DBManager.sDecimalType + "(11,7) default 0.00, " + //nzp_prm = 131
                "    val4        " + DBManager.sDecimalType + "(11,7) default 0.00, " +//дробное кол-во жильцов в лс по kart с учетом времен. выбывших
                "    val5        " + DBManager.sDecimalType + "(11,7) default 0.00, " +//дробное кол-во времен. выбывших
                "    val6        " + DBManager.sDecimalType + "(11,7) default 0.00, " +//Количество не зарегистрированных проживающих
                "    kod_info    integer       default 0 ) ");

            BillingInstrumentary.ExecSQL(" create unique index ix1_" + gilTable.Replace(".", "") + " on " + gilTable + " (nzp_gx) ");

            BillingInstrumentary.ExecSQL(" create index ix2_" + gilTable.Replace(".", "") + " on " + gilTable + " (nzp_dom,dat_charge) ");

            BillingInstrumentary.ExecSQL(" create index ix3_" + gilTable.Replace(".", "") + " on " + gilTable + " (nzp_kvar,dat_charge,stek, dat_s,dat_po) ");

            BillingInstrumentary.ExecSQL(" create index ix4_" + gilTable.Replace(".", "") + " on " + gilTable + " (nzp_kvar,nzp_gil,dat_charge) ");

            BillingInstrumentary.ExecSQL(" create index ix5_" + gilTable.Replace(".", "") + " on " + gilTable + " (cur_zap) ");

            BillingInstrumentary.ExecSQL(" create index ix6_" + gilTable.Replace(".", "") + " on " + gilTable + " (prev_zap) ");
        }

    }

    public class CalcPreparationHandlerOverride : ICalcPreparation
    {
        protected IDbConnection Connection;
        protected BillingInstrumentary BillingInstrumentary;
        protected CalcVerificationParams VerificationParams;
        private string tableMOCharges = String.Empty;
        protected CalcTypes.ParamCalc Params;
        protected TempTablesLifeTime TempTablesLifeTime;
        protected IWindsorContainer Container;

        public CalcPreparationHandlerOverride(IDbConnection connection,
            BillingInstrumentary billingInstrumentary,
            CalcVerificationParams verificationParams,
            IWindsorContainer container)
        {
            Connection = connection;
            BillingInstrumentary = billingInstrumentary;
            VerificationParams = verificationParams;
            Container = container;
            TempTablesLifeTime = Container.Resolve<TempTablesLifeTime>();
        }

        /// <summary>
        /// Подготовка расчета - определяем выборку ЛС для расчета
        /// </summary>
        /// <param name="Params"></param>
        /// <returns></returns>
        public IDataResult PrepareCalc(ref CalcTypes.ParamCalc Params)
        {

            CreatePreparationsTables();

            GetAllPersonalAccounts(Params);

            GetOpenPersonalAccounts(Params);

            #region переопределим nzp_dom для расчета одного ЛС

            //переопределим nzp_dom
            if (Params.nzp_kvar > 0)
            {
                Params.nzp_dom = Connection.ExecuteScalar<int>(string.Format(" Select nzp_dom From t_selkvar Where nzp_kvar = {0} limit 1", Params.nzp_kvar));
            }
            #endregion переопределим nzp_dom для расчета одного ЛС

            return new BaseDataResult();
        }


        /// <summary>
        /// Получить данные по начислениям их УК - необходимо для дальнейшего расчета
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetMOCharges(ref CalcTypes.ParamCalc param)
        {
            var calcCharge = new CalcTypes.ChargeXX(param);
            var TableMOCharges = "t_charge_gg_" + DateTime.Now.Ticks;
            var sql = " CREATE TEMP TABLE " + TableMOCharges + " AS " +
                      " SELECT ch.*, s.nzp_dom FROM " + calcCharge.charge_xx + " ch" +
                      " JOIN t_selkvar s ON s.nzp_kvar=ch.nzp_kvar" +
                      " WHERE ch.dat_charge IS NULL";
            TempTablesLifeTime.AddTempTable(TableMOCharges, sql);

            BillingInstrumentary.ExecSQL("CREATE INDEX ix1_" + TableMOCharges + " ON " + TableMOCharges + "(nzp_kvar, nzp_serv,nzp_supp )");
            BillingInstrumentary.ExecSQL("CREATE INDEX ix2_" + TableMOCharges + " ON " + TableMOCharges + "(nzp_kvar)");
            BillingInstrumentary.ExecSQL("CREATE INDEX ix3_" + TableMOCharges + " ON " + TableMOCharges + "(nzp_serv)");

            tableMOCharges = TableMOCharges;
            return TableMOCharges;
        }

        /// <summary>
        /// выборка открытых ЛС - t_opn
        /// </summary>
        /// <param name="Params"></param>
        private void GetOpenPersonalAccounts(CalcTypes.ParamCalc Params)
        {
            //+++++++++++++++++++++++++++++++++++++++++++++++++++
            // выбрать открытые лицевые счета по t_selkvar
            //+++++++++++++++++++++++++++++++++++++++++++++++++++

            BillingInstrumentary.ExecSQL(
                " Insert into ttt_prm_3 (nzp, nzp_dom, num_ls, dat_s, dat_po)" +
                " Select k.nzp_kvar, k.nzp_dom, k.num_ls, " +
                DBManager.sNvlWord + "(p.dat_s," + DBManager.MDY(12, 31, 1899) + "), " + DBManager.sNvlWord +
                "(p.dat_po," +
                DBManager.MDY(1, 1, 3000) + ") " +
                " From t_selkvar k," + Params.pref + "prm_3 p" +
                " Where k.nzp_kvar=p.nzp and p.nzp_prm=51 and p.val_prm in ('1','3') " +
                "   and p.is_actual<>100 " +
                "   and p.dat_s  <= " + Params.dat_po +
                "   and p.dat_po >= " + Params.dat_s +
                " group by 1,2,3,4,5 "
                );

            BillingInstrumentary.ExecSQL(" Create unique index ix1_ttt_prm_3 on ttt_prm_3 (nzp,nzp_dom,num_ls) ", false);
            BillingInstrumentary.ExecSQL(" Create        index ix2_ttt_prm_3 on ttt_prm_3 (nzp,dat_s,dat_po) ", false);
            BillingInstrumentary.ExecSQL(" Create        index ix3_ttt_prm_3 on ttt_prm_3 (dat_s,dat_po) ", false);

            bool bDayCalc =
                CheckValBoolPrmWithVal(Params.pref, 89, "10", "1", Params.dat_s, Params.dat_po);

            if (bDayCalc)
            {
                BillingInstrumentary.ExecSQL(
                    " Insert into t_opn (nzp_kvar, nzp_dom, num_ls, dat_s, dat_po, is_day_calc) " +
                    " Select k.nzp, k.nzp_dom, k.num_ls, min(k.dat_s), max(k.dat_po), 1 " +
                    " From ttt_prm_3 k " +
                    " group by 1,2,3 "
                    );

                BillingInstrumentary.ExecSQL(" Create unique index ix1_t_opn on t_opn (nzp_kvar) ", false);
            }
            else
            {
                BillingInstrumentary.ExecSQL(
                    " Insert into t_opn (nzp_kvar, nzp_dom, num_ls, dat_s, dat_po, is_day_calc) " +
                    " Select k.nzp, k.nzp_dom, k.num_ls, min(k.dat_s), max(k.dat_po), 1 " +
                    " From ttt_prm_3 k, " + Params.pref + "prm_1 p " +
                    " Where k.nzp=p.nzp and p.nzp_prm=90 and p.val_prm='1' " +
                    "   and p.is_actual<>100 " +
                    "   and p.dat_s  <= " + Params.dat_po +
                    "   and p.dat_po >= " + Params.dat_s +
                    " group by 1,2,3 "
                    );

                BillingInstrumentary.ExecSQL(" Create unique index ix1_t_opn on t_opn (nzp_kvar) ", false);
                BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " t_opn ");

                BillingInstrumentary.ExecSQL(
                    " Insert into t_opn (nzp_kvar, nzp_dom, num_ls, dat_s, dat_po, is_day_calc) " +
                    " Select k.nzp, k.nzp_dom, k.num_ls, min(k.dat_s), max(k.dat_po), 0 " +
                    " From ttt_prm_3 k " +
                    " where NOT EXISTS (SELECT 1 From t_opn n Where k.nzp=n.nzp_kvar ) " +
                    " group by 1,2,3 "
                    );
            }

            BillingInstrumentary.ExecSQL(" Create        index ix2_t_opn on t_opn (num_ls) ", false);
            BillingInstrumentary.ExecSQL(" Create        index ix3_t_opn on t_opn (nzp_dom) ", false);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " t_opn ");
        }

        /// <summary>
        /// Выбрать множество лицевых счетов для расчета
        /// </summary>
        /// <param name="Params"></param>
        private void GetAllPersonalAccounts(CalcTypes.ParamCalc Params)
        {
            //выбрать множество лицевых счетов
            string s_find =
                " From " + Params.pref + "kvar k " +
                " Where " + Params.where_z;

            if (Params.nzp_kvar > 0)
            {
                s_find =
                    " From " + Params.pref + "kvar k " +
                    " Where k.nzp_dom=" + Params.nzp_dom;

                // ... для расчета связанные дома ... 
                int nzp_dom_base = 0;
                try
                {
                    nzp_dom_base =
                        Connection.ExecuteScalar<int>(" Select max(nzp_dom_base) From " + Params.pref +
                                                      "link_dom_lit p " +
                                                      " Where nzp_dom=" + Params.nzp_dom);
                }
                catch
                {
                    nzp_dom_base = 0;
                }

                if (nzp_dom_base > 0)
                {
                    s_find =
                        " From " + Params.pref + "kvar k, " + Params.pref + "link_dom_lit l " +
                        " Where k.nzp_dom=l.nzp_dom and l.nzp_dom_base=" + nzp_dom_base;
                }
            }
            if (Params.nzp_kvar > 0)
            {
                s_find =
                    " From " + Params.pref + "kvar k " +
                    " Where k.nzp_kvar=" + Params.nzp_kvar;
            }
            BillingInstrumentary.ExecSQL(
                " Insert into t_selkvar (nzp_kvar,num_ls,nzp_dom, nzp_area,nzp_geu) " +
                " Select k.nzp_kvar,k.num_ls,k.nzp_dom,k.nzp_area,k.nzp_geu " + s_find
                );
            BillingInstrumentary.ExecSQL(" Create unique index ix1_t_selkvar on t_selkvar (nzp_key) ", false);
            BillingInstrumentary.ExecSQL(" Create unique index ix2_t_selkvar on t_selkvar (nzp_kvar) ", false);
            BillingInstrumentary.ExecSQL(" Create        index ix3_t_selkvar on t_selkvar (num_ls) ", false);
            BillingInstrumentary.ExecSQL(" Create        index ix4_t_selkvar on t_selkvar (nzp_dom) ", false);
            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " t_selkvar ");
        }

        /// <summary>
        /// Создаем таблицы для подготовка выборки ЛС
        /// </summary>
        private void CreatePreparationsTables()
        {
            TempTablesLifeTime.DropTempTable("t_opn");
            TempTablesLifeTime.DropTempTable("t_selkvar");
            TempTablesLifeTime.DropTempTable("ttt_prm_3");

            var ssql = " Create temp table ttt_prm_3 " +
                       " ( nzp      integer, " +
                       "   nzp_dom  integer, " +
                       "   num_ls   integer, " +
                       "   dat_s    date not null, " +
                       "   dat_po   date not null  " +
                       " )";
            TempTablesLifeTime.AddTempTable("ttt_prm_3", ssql);


            ssql =
                " Create temp table t_selkvar " +
                " ( nzp_key   serial not null, " +
                "   nzp_kvar  integer, " +
                "   num_ls    integer, " +
                "   nzp_dom   integer, " +
                "   nzp_area  integer, " +
                "   nzp_geu   integer,  " +
                "   need_calc   bool  " +
                " )";
            TempTablesLifeTime.AddTempTable("t_selkvar", ssql);


            ssql =
                " Create temp table t_opn " +
                " ( nzp_kvar integer," +
                "   nzp_dom  integer," +
                "   num_ls   integer, " +
                "   is_day_calc integer default 0, " +
                "   dat_s    date not null," +
                "   dat_po   date not null" +
                " )";
            TempTablesLifeTime.AddTempTable("t_opn", ssql);

        }

        #region Методы перенесенные из KP50

        #region Проверка наличия логического параметра в нужной базе, нужной таблице prm_5/10, по дате, значению параметра

        //--------------------------------------------------------------------------------
        public bool CheckValBoolPrmWithVal(string pDataAls, int pNzpPrm, string pNumPrm, string pValPrm, string pDatS,
            string pDatPo)
        //--------------------------------------------------------------------------------
        {
            var sql = " Select count(*) cnt From " + pDataAls + "prm_" + pNumPrm.Trim() + " p " +
            " Where p.nzp_prm = " + pNzpPrm + " and p.val_prm='" + pValPrm.Trim() + "' " +
            " and p.is_actual <> 100 and p.dat_s  <= " + pDatPo + " and p.dat_po >= " + pDatS + " ";
            bool bRetVal;
            try
            {
                bRetVal = Connection.ExecuteScalar<int>(sql) > 0;
            }
            catch
            {
                bRetVal = false;
            }
            return bRetVal;
        }

        #endregion Проверка наличия логического параметра в нужной базе, нужной таблице prm_5/10, по дате, значению параметра

        #endregion


        /// <summary>
        /// Загрузка параметров необходимых для расчета
        /// </summary>
        /// <param name="paramcalc"></param>
        /// <returns></returns>
        public IDataResult LoadTempTablesForMonth(ref CalcTypes.ParamCalc paramcalc)
        //--------------------------------------------------------------------------------
        {
            //параметры по лс: prm_1, prm_3
            LoadTempTablesAccountParams(paramcalc);
            //параметры по дому: prm_2
            LoadTempTablesHouseParams(paramcalc);
            //периоды t_calc_gku
            GetCalcPeriodsByAccounts(paramcalc);
            return new BaseDataResult();
        }

        private void LoadTempTablesAccountParams(CalcTypes.ParamCalc paramcalc)
        {
            string s;
            //prm_1 - параметры ЛС
            string sp;
            if (paramcalc.nzp_kvar > 0 || paramcalc.nzp_dom > 0)
                sp = "  ,t_selkvar b Where a.nzp = b.nzp_kvar ";
            else
                sp = "  Where 1 = 1 ";


            TempTablesLifeTime.AddTempTable("ttt_prm_1f",
                " CREATE TEMP TABLE ttt_prm_1f (" +
                "   nzp_key serial NOT NULL," +
                "   nzp     integer," +
                "   nzp_prm integer," +
                "   val_prm character(20)," +
                "   dat_s      date," +
                "   dat_po     date " +
                " ) "
                );

            BillingInstrumentary.ExecSQL(
                " insert into ttt_prm_1f (nzp_key,nzp,nzp_prm,val_prm,dat_s,dat_po) " +
                " Select a.nzp_key, a.nzp, a.nzp_prm," +
                " replace( " + DBManager.sNvlWord + "(a.val_prm,'0'), ',', '.') val_prm, " +
                DBManager.sNvlWord + "(a.dat_s, " + DBManager.MDY(1, 1, 1901) + ") as dat_s , " +
                DBManager.sNvlWord + "(a.dat_po, " + DBManager.MDY(1, 1, 3000) + ") as dat_po " +
                " From " + paramcalc.pref + "prm_1 a " +
                sp + " and a.is_actual <> 100 "
                );

            BillingInstrumentary.ExecSQL(" Create unique index ix1_ttt_prm_1f on ttt_prm_1f (nzp_key) ");
            BillingInstrumentary.ExecSQL(" Create index ix2_ttt_prm_1f on ttt_prm_1f (nzp,dat_s,dat_po) ");
            BillingInstrumentary.ExecSQL(" Create index ix3_ttt_prm_1f on ttt_prm_1f (dat_s,dat_po) ");

            //prm_1
            if (paramcalc.nzp_kvar > 0 || paramcalc.nzp_dom > 0)
                s = "  ,t_selkvar b Where a.nzp = b.nzp_kvar ";
            else
                s = "  Where 1 = 1 ";

            TempTablesLifeTime.AddTempTable("ttt_prm_1",
                " CREATE TEMP TABLE ttt_prm_1 (" +
                "   nzp_key serial NOT NULL," +
                "   nzp     integer," +
                "   nzp_prm integer," +
                "   val_prm character(20)," +
                "   dat_s      date," +
                "   dat_po     date " +
                " ) "
                );

            TempTablesLifeTime.AddTempTable("ttt_prm_1d",
                " CREATE TEMP TABLE ttt_prm_1d (" +
                "   nzp_key serial NOT NULL," +
                "   nzp     integer," +
                "   nzp_prm integer," +
                "   val_prm character(20)," +
                "   dat_s      date," +
                "   dat_po     date " +
                " ) "
                );

            BillingInstrumentary.ExecSQL(
                " insert into ttt_prm_1d (nzp_key,nzp,nzp_prm,val_prm,dat_s,dat_po) " +
                " Select min(a.nzp_key) nzp_key,a.nzp,a.nzp_prm,a.val_prm,a.dat_s,a.dat_po " +
                " From ttt_prm_1f a " +
                s +
                "   and a.dat_s  <= " + paramcalc.dat_po +
                "   and a.dat_po >= " + paramcalc.dat_s +
                " group by 2,3,4,5,6 "
                );

            BillingInstrumentary.ExecSQL(
                " Create index ix1_ttt_prm_1d on ttt_prm_1d (nzp,nzp_prm,val_prm,dat_s,dat_po) ");
            BillingInstrumentary.ExecSQL(" Create index ix2_ttt_prm_1d on ttt_prm_1d (nzp_prm) ");


            BillingInstrumentary.ExecSQL(
                " insert into ttt_prm_1 (nzp_key,nzp,nzp_prm,val_prm,dat_s,dat_po) " +
                " Select min(a.nzp_key) nzp_key,a.nzp,a.nzp_prm,max(a.val_prm) val_prm,min(a.dat_s) dat_s,max(a.dat_po) dat_po " +
                " From ttt_prm_1d a " +
                " group by 2,3 "
                );

            BillingInstrumentary.ExecSQL(" Create index ix1_ttt_prm_1 on ttt_prm_1 (nzp,nzp_prm) ");
            BillingInstrumentary.ExecSQL(" Create index ix2_ttt_prm_1 on ttt_prm_1 (nzp_prm) ");
        }

        /// <summary>
        /// Получение периодов расчета по лицевым счетам
        /// </summary>
        /// <param name="paramcalc"></param>
        private void GetCalcPeriodsByAccounts(CalcTypes.ParamCalc paramcalc)
        {
            TempTablesLifeTime.AddTempTable("t_gku_periods",
                " Create temp table t_gku_periods " +
                " ( nzp_period serial, " +
                "   nzp_kvar   integer, " +
                "   dp       " + DBManager.sDateTimeType + ", " +
                "   dp_end   " + DBManager.sDateTimeType + ", " +
                "   typ  integer, " +
                "   cntd integer," +
                "   cntd_mn integer " +
                " )"
                );

            BillingInstrumentary.ExecSQL(
                " insert into t_gku_periods (nzp_kvar,typ,dp,dp_end,cntd,cntd_mn) " +
                " select k.nzp_kvar,0," + paramcalc.dat_s + "," + paramcalc.dat_po + ", " +
                DateTime.DaysInMonth(paramcalc.calc_yy, paramcalc.calc_mm) + ", " +
                DateTime.DaysInMonth(paramcalc.calc_yy, paramcalc.calc_mm) +
                " From t_opn k " +
                " where NOT EXISTS (select 1 From t_gku_periods t Where k.nzp_kvar=t.nzp_kvar ) "
                );

            BillingInstrumentary.ExecSQL(" Create index ix1_t_gku_periods on t_gku_periods (nzp_kvar) ", false);
            BillingInstrumentary.ExecSQL(" Create unique index ix0_t_gku_periods on t_gku_periods (nzp_period) ", false);
            BillingInstrumentary.ExecSQL(" Create index ix2_t_gku_periods on t_gku_periods (nzp_kvar,dp,dp_end) ", false);
        }

        /// <summary>
        /// Домовые параметры
        /// </summary>
        /// <param name="paramcalc"></param>
        /// <param name="sp"></param>
        /// <param name="s"></param>
        private void LoadTempTablesHouseParams(CalcTypes.ParamCalc paramcalc)
        {
            #region Домовые параметры

            var s = String.Empty;
            //prm_2
            if (paramcalc.nzp_kvar > 0 || paramcalc.nzp_dom > 0)
                s = " Where Exists (select 1 from t_selkvar b Where a.nzp = b.nzp_dom) ";
            else
                s = " Where 1 = 1 ";

            var sp = String.Empty;
            //prm_2 - параметры домов
            if (paramcalc.nzp_kvar > 0 || paramcalc.nzp_dom > 0)
                sp = "  ,t_selkvar b Where a.nzp = b.nzp_dom ";
            else
                sp = "  Where 1 = 1 ";

            BillingInstrumentary.ExecSQL(
                " CREATE TEMP TABLE ttt_prm_2f (" +
                "   nzp_key serial NOT NULL," +
                "   nzp     integer," +
                "   nzp_prm integer," +
                "   val_prm character(20)," +
                "   dat_s      date," +
                "   dat_po     date " +
                " ) ");


            BillingInstrumentary.ExecSQL(
                " insert into ttt_prm_2f (nzp_key,nzp,nzp_prm,val_prm,dat_s,dat_po) " +
                " Select DISTINCT " +
                " a.nzp_key, a.nzp, a.nzp_prm," +
                " replace(COALESCE(a.val_prm,'0'), ',', '.') val_prm, " +
                "COALESCE(a.dat_s, " + BillingInstrumentary.MDY(1, 1, 1901) + ") as dat_s , " +
                "COALESCE(a.dat_po, " + BillingInstrumentary.MDY(1, 1, 3000) + " ) as dat_po " +
                " From " + paramcalc.pref + "prm_2 a " +
                sp + " and a.is_actual <> 100 ");


            BillingInstrumentary.ExecSQL(" Create unique index ix1_ttt_prm_2f on ttt_prm_2f (nzp_key) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix2_ttt_prm_2f on ttt_prm_2f (nzp,dat_s,dat_po) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix3_ttt_prm_2f on ttt_prm_2f (dat_s,dat_po) ", true);


            TempTablesLifeTime.AddTempTable("ttt_prm_2",
                " CREATE TEMP TABLE ttt_prm_2 (" +
                "   nzp_key serial NOT NULL," +
                "   nzp     integer," +
                "   nzp_prm integer," +
                "   val_prm character(20)," +
                "   dat_s      date," +
                "   dat_po     date " +
                " ) ");


            TempTablesLifeTime.AddTempTable("ttt_prm_2d",
                " CREATE TEMP TABLE ttt_prm_2d (" +
                "   nzp_key serial NOT NULL," +
                "   nzp     integer," +
                "   nzp_prm integer," +
                "   val_prm character(20)," +
                "   dat_s      date," +
                "   dat_po     date " +
                " ) ");


            BillingInstrumentary.ExecSQL(
                " insert into ttt_prm_2d (nzp_key,nzp,nzp_prm,val_prm,dat_s,dat_po) " +
                " Select min(a.nzp_key) nzp_key,a.nzp,a.nzp_prm,a.val_prm,a.dat_s,a.dat_po " +
                " From ttt_prm_2f a " +
                s +
                "   and a.dat_s  <= " + paramcalc.dat_po +
                "   and a.dat_po >= " + paramcalc.dat_s +
                " group by 2,3,4,5,6 "
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix1_ttt_prm_2d on ttt_prm_2d (nzp,nzp_prm,val_prm,dat_s,dat_po) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix2_ttt_prm_2d on ttt_prm_2d (nzp_prm) ", true);


            BillingInstrumentary.ExecSQL(
                " insert into ttt_prm_2 (nzp_key,nzp,nzp_prm,val_prm,dat_s,dat_po) " +
                " Select min(a.nzp_key) nzp_key,a.nzp,a.nzp_prm,max(a.val_prm) val_prm,min(a.dat_s) dat_s,max(a.dat_po) dat_po " +
                " From ttt_prm_2d a " +
                " group by 2,3 "
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix1_ttt_prm_2 on ttt_prm_2 (nzp,nzp_prm) ", true);
            BillingInstrumentary.ExecSQL(" Create index ix2_ttt_prm_2 on ttt_prm_2 (nzp_prm) ", true);

            #endregion  Домовые параметры
        }

        /// <summary>
        /// Чистим старые расчеты
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IDataResult ClearData(ref CalcTypes.ParamCalc param)
        {
            var tenantInfo = new DbCalcCharge.GilecXX(param);
            tenantInfo.gil_xx = string.Format("{0}.{1}", param.pref, tenantInfo.gilec_tab);
            var CalcServicesTable = string.Format("{0}.chd_calc_gku_{1}", param.pref, param.calc_mm.ToString("00"));
            var ConsumptionsTable = string.Format("{0}.chd_counters_{1}", param.pref, param.calc_mm.ToString("00"));
            var ChargesTable = string.Format("{0}.chd_charge_{1}", param.pref, param.calc_mm.ToString("00"));


            var sql = " DELETE FROM " + tenantInfo.gil_xx + " g " +
                      " WHERE EXISTS (SELECT 1 FROM t_selkvar t " +
                      "                 WHERE g.nzp_kvar=t.nzp_kvar)";
            ExecSQLWithCheckExist(sql, param, CalcVerificationTables.TenantTable);

            sql = " DELETE FROM " + ConsumptionsTable + " c " +
                  " WHERE EXISTS (SELECT 1 FROM t_selkvar t " +
                  "                 WHERE c.nzp_dom=t.nzp_dom)";
            ExecSQLWithCheckExist(sql, param, CalcVerificationTables.ConsumptionsTable);


            sql = " DELETE FROM " + CalcServicesTable + " c " +
                  " WHERE EXISTS (SELECT 1 FROM t_selkvar t " +
                  "                 WHERE c.nzp_kvar=t.nzp_kvar)";
            ExecSQLWithCheckExist(sql, param, CalcVerificationTables.CalcServicesTable);

            sql = " DELETE FROM " + ChargesTable + " c " +
                  " WHERE EXISTS (SELECT 1 FROM t_selkvar t " +
                  "                 WHERE c.nzp_kvar=t.nzp_kvar)";
            ExecSQLWithCheckExist(sql, param, CalcVerificationTables.ChargesTable);


            return new BaseDataResult();
        }

        /// <summary>
        /// Выполнение запроса с проверкой на существование таблиц
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="tables">таблицы для проверки</param>
        private void ExecSQLWithCheckExist(string sql, CalcTypes.ParamCalc param, CalcVerificationTables tables)
        {
            try
            {
                BillingInstrumentary.ExecSQL(sql);
            }
            catch (Exception)
            {
                foreach (CalcVerificationTables calcTable in Enum.GetValues(typeof(CalcVerificationTables)))
                {
                    if (tables.HasFlag(calcTable))
                    {
                        CreateCalcVerificationTable(param, calcTable);
                    }
                }
                BillingInstrumentary.ExecSQL(sql);
            }
        }

        /// <summary>
        /// Создание структуры в базе данных по типу таблицы
        /// </summary>
        /// <param name="param"></param>
        /// <param name="table"></param>
        private void CreateCalcVerificationTable(CalcTypes.ParamCalc param, CalcVerificationTables table)
        {
            switch (table)
            {
                case CalcVerificationTables.TenantTable: CreateTenantsTable(param); break;
                case CalcVerificationTables.ConsumptionsTable: CreateConsumptionsTable(Connection, param); break;
                case CalcVerificationTables.CalcServicesTable: CreateCalcServicesTable(Connection, param); break;
                case CalcVerificationTables.ChargesTable: CreateChargesTable(Connection, param); break;
                default: throw new InvalidEnumArgumentException("Не определен тип таблицы для проверочного расчета");
            }
        }




        /// <summary>
        /// Создание таблицы calc_gku_xx
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="paramCalc"></param>
        private void CreateCalcServicesTable(IDbConnection connection, CalcTypes.ParamCalc paramCalc)
        {
            if (paramCalc.calc_yy == 0) throw new Exception("Не заполнен расченый год");
            if (paramCalc.calc_mm == 0) throw new Exception("Не заполнен расченый месяц");
            if (VerificationParams.Pref.IsNullOrEmpty()) throw new Exception("Не заполнен префикс");

            var chargeChdCalcGku = string.Format(@"{0}.chd_calc_gku_{1}",
                VerificationParams.Pref, paramCalc.calc_mm.ToString("00"));
            var sql = string.Format(@"
                CREATE TABLE {0}
                (
                  chd_calc_gku_id serial NOT NULL,
                  chd_counters_id integer,
                  nzp_wp integer NOT NULL,
                  nzp_dom integer NOT NULL,
                  nzp_kvar integer NOT NULL,
                  nzp_serv integer NOT NULL,
                  nzp_supp integer NOT NULL,
                  nzp_frm integer NOT NULL DEFAULT 0,
                  stek integer NOT NULL DEFAULT 3,
                  dat_s date NOT NULL DEFAULT '1900-01-01'::date,
                  dat_po date NOT NULL DEFAULT '1900-01-01'::date,
                  is_device integer NOT NULL DEFAULT 0,
                  squ1 numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  squ2 numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  gil numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  tarif numeric(17,7) NOT NULL DEFAULT 0.00,
                  tarif_chd numeric(17,7) NOT NULL DEFAULT 0.00,
                  tarif_gkal numeric(17,7) NOT NULL DEFAULT 0.00,
                  tarif_gkal_chd numeric(17,7) NOT NULL DEFAULT 0.00,
                  rashod numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  rashod_chd numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  rash_norm_one numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  rash_norm_one_chd numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  valm numeric(15,7) NOT NULL DEFAULT 0.0000000,
                  valm_chd numeric(15,7) NOT NULL DEFAULT 0.0000000,
                  dop87 numeric(15,7) NOT NULL DEFAULT 0.0000000,
                  dop87_chd numeric(15,7) NOT NULL DEFAULT 0.0000000,
                  rashod_link numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  rashod_link_chd numeric(14,7) NOT NULL DEFAULT 0.0000000
                )
                WITH (
                  OIDS=TRUE
                );
                ", chargeChdCalcGku);
            connection.Execute(sql);

            sql = string.Format(@"CREATE UNIQUE INDEX ix1{0} ON {1} (chd_calc_gku_id);", chargeChdCalcGku.Replace(".", ""), chargeChdCalcGku);
            connection.Execute(sql);

            sql = string.Format(@"CREATE  INDEX ix2{0} ON {1} (nzp_kvar, stek, dat_s, dat_po);", chargeChdCalcGku.Replace(".", ""), chargeChdCalcGku);
            connection.Execute(sql);

            sql = string.Format(@"CREATE  INDEX ix3{0} ON {1} (nzp_kvar, nzp_frm);", chargeChdCalcGku.Replace(".", ""), chargeChdCalcGku);
            connection.Execute(sql);

            sql = string.Format(@"CREATE  INDEX ix4{0} ON {1} (nzp_dom);", chargeChdCalcGku.Replace(".", ""), chargeChdCalcGku);
            connection.Execute(sql);
        }
        /// <summary>
        /// Создание таблицы charge_xx
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="paramCalc"></param>
        public void CreateChargesTable(IDbConnection connection, CalcTypes.ParamCalc paramCalc, bool isChd = false)
        {
            if (paramCalc.calc_yy == 0) throw new Exception("Не заполнен расченый год");
            if (paramCalc.calc_mm == 0) throw new Exception("Не заполнен расченый месяц");
            if (VerificationParams.Pref.IsNullOrEmpty()) throw new Exception("Не заполнен префикс");

            string chargeChdCharge = string.Format(@"{0}.chd_charge_{1}",
                isChd ? "chd" : VerificationParams.Pref, paramCalc.calc_mm.ToString("00"));
            var sql = string.Format(@"
                CREATE TABLE {0}
                (
                  chd_charge_id serial NOT NULL,
                  billing_house_code bigint NOT NULL,
                  service character varying(250),
                  supplier character varying(250),
                  measure character varying(250),
                  formula character varying(250),
                  chd_point_id integer NOT NULL,
                  squ1 numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  gil numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  rash_norm_one numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  rash_norm_one_chd numeric(14,7) NOT NULL DEFAULT 0.0000000,
                  nzp_kvar integer,
                  nzp_dom integer,
                  num_ls integer,
                  nzp_serv integer,
                  nzp_supp integer,
                  nzp_measure integer,
                  nzp_frm integer,
                  dat_charge date,
                  is_device integer DEFAULT 0,
                  tarif numeric(14,3) DEFAULT 0,
                  tarif_chd numeric(14,3) DEFAULT 0,
                  c_calc numeric(14,5) DEFAULT 0,
                  c_calc_chd numeric(14,5) DEFAULT 0,
                  rsum_tarif numeric(14,2) DEFAULT 0,
                  rsum_tarif_chd numeric(14,2) DEFAULT 0,
                  sum_nedop numeric(14,2) DEFAULT 0,
                  sum_nedop_chd numeric(14,2) DEFAULT 0,
                  sum_real numeric(14,2) DEFAULT 0,
                  sum_real_chd numeric(14,2) DEFAULT 0,
                  reval numeric(14,2) DEFAULT 0,
                  reval_chd numeric(14,2) DEFAULT 0,
                  real_charge numeric(14,2) DEFAULT 0,
                  sum_insaldo numeric(14,2) DEFAULT 0,
                  sum_money numeric(14,2) DEFAULT 0,
                  sum_outsaldo numeric(14,2) DEFAULT 0,
                  sum_outsaldo_chd numeric(14,2) DEFAULT 0,
                  sum_charge numeric(14,2) DEFAULT 0
                )
                WITH (
                  OIDS=TRUE
                ); ", chargeChdCharge);

            connection.Execute(sql);

            sql = string.Format(@"CREATE UNIQUE INDEX ix1{0} ON {1} (chd_charge_id);", chargeChdCharge.Replace(".", ""), chargeChdCharge);
            connection.Execute(sql);

            sql = string.Format(@"CREATE INDEX ix2{0} ON {1} (num_ls, nzp_supp, nzp_serv);", chargeChdCharge.Replace(".", ""), chargeChdCharge);
            connection.Execute(sql);

            sql = string.Format(@"CREATE INDEX ix3{0} ON {1} (nzp_kvar, nzp_serv, dat_charge);", chargeChdCharge.Replace(".", ""), chargeChdCharge);
            connection.Execute(sql);

            sql = string.Format(@"CREATE INDEX ix4{0} ON {1} (nzp_kvar, nzp_serv, nzp_supp);", chargeChdCharge.Replace(".", ""), chargeChdCharge);
            connection.Execute(sql);

            sql = string.Format(@"CREATE INDEX ix5{0} ON {1} (nzp_kvar, nzp_supp, dat_charge);", chargeChdCharge.Replace(".", ""), chargeChdCharge);
            connection.Execute(sql);

            sql = string.Format(@"CREATE INDEX ix6{0} ON {1} (nzp_serv);", chargeChdCharge.Replace(".", ""), chargeChdCharge);
            connection.Execute(sql);

        }
        /// <summary>
        /// Создание таблицы counters_xx
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="paramCalc"></param>
        private void CreateConsumptionsTable(IDbConnection connection, CalcTypes.ParamCalc paramCalc)
        {
            if (paramCalc.calc_yy == 0) throw new Exception("Не заполнен расченый год");
            if (paramCalc.calc_mm == 0) throw new Exception("Не заполнен расченый месяц");
            if (VerificationParams.Pref.IsNullOrEmpty()) throw new Exception("Не заполнен префикс");

            var chargeChdCounters = string.Format(@"{0}.chd_counters_{1}",
                VerificationParams.Pref, paramCalc.calc_mm.ToString("00"));

            var sql = string.Format(@"
               CREATE TABLE {0} (
                      chd_counters_id serial NOT NULL,
                      nzp_wp integer NOT NULL,
                      nzp_dom integer NOT NULL,
                      nzp_serv integer NOT NULL,
                      nzp_frmd integer NOT NULL,
                      stek integer NOT NULL DEFAULT 0,
                      nzp_type integer NOT NULL,
                      cnt_stage integer DEFAULT 0,
                      cls integer NOT NULL DEFAULT 0,
                      gil numeric(14,7) DEFAULT 0.0000000,
                      val1 numeric(14,7) DEFAULT 0.0000000,
                      val2 numeric(14,7) DEFAULT 0.0000000,
                      val2_dop numeric(14,7) DEFAULT 0.0000000,
                      val1_dop numeric(14,7) DEFAULT 0.0000000,
                      squ1_norm numeric(14,7) DEFAULT 0.0000000,
                      squ1 numeric(14,7) DEFAULT 0.0000000,
                      squ2 numeric(14,7) DEFAULT 0.0000000,
                      squ_mop numeric(14,7) DEFAULT 0.0000000,
                      norm_one numeric(15,7) NOT NULL DEFAULT 0,
                      norm_one_chd numeric(15,7) NOT NULL DEFAULT 0,
                      rashod numeric(14,7) NOT NULL DEFAULT 0.0000000,
                      rashod_chd numeric(14,7) NOT NULL DEFAULT 0.0000000,
                      dop87 numeric(15,7) NOT NULL DEFAULT 0,
                      dop87_chd numeric(15,7) NOT NULL DEFAULT 0,
                      kf307 numeric(15,7) DEFAULT 0.0000000,
                      kf307n numeric(15,7) DEFAULT 0.0000000,
                      kf307_chd numeric(15,7) DEFAULT 0.0000000,
                      kf307n_chd numeric(15,7) DEFAULT 0.0000000,
                      kf_dpu_prop numeric(15,7) DEFAULT 0.0000000,
                      kf_dpu_prop_chd numeric(15,7) DEFAULT 0.0000000,
                      kod_info integer DEFAULT 0,
                      kod_info_chd integer DEFAULT 0
                    )
                    WITH (
                      OIDS=FALSE
                    );
                ", chargeChdCounters);
            connection.Execute(sql);

            sql = string.Format(@"CREATE UNIQUE INDEX ix1{0} ON {1}  (chd_counters_id);", chargeChdCounters.Replace(".", ""), chargeChdCounters);
            connection.Execute(sql);
            sql = string.Format(@"CREATE INDEX ix2{0} ON {1}  (nzp_type, stek, nzp_dom);", chargeChdCounters.Replace(".", ""), chargeChdCounters);
            connection.Execute(sql);


        }
        /// <summary>
        /// Создание таблицы gil_xx
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="paramCalc"></param>
        void CreateTenantsTable(CalcTypes.ParamCalc paramCalc)
        {
            if (paramCalc.calc_yy == 0) throw new Exception("Не заполнен расченый год");
            if (paramCalc.calc_mm == 0) throw new Exception("Не заполнен расченый месяц");
            if (VerificationParams.Pref.IsNullOrEmpty()) throw new Exception("Не заполнен префикс");
            var gilTable = string.Format("{0}.gil_{1}", VerificationParams.Pref, paramCalc.calc_mm.ToString("00"));

            BillingInstrumentary.ExecSQL(
                " Create table " + gilTable +
                " (  nzp_gx      serial        not null, " +
                "    nzp_dom     integer       not null, " +
                "    nzp_kvar    integer       default 0 not null, " +
                "    dat_charge  date, " +
                "    cur_zap     integer       default 0 not null, " +//0-текущее значение, >0 - ссылка на следующее значение (nzp_cntx)
                "    prev_zap    integer       default 0 not null, " +//0-текущее значение, >0 - ссылка на предыдыущее значение (nzp_cntx)
                "    nzp_gil     integer       default 0 not null, " + //
                "    dat_s       date, " +
                "    dat_po      date, " +
                "    stek        integer       default 0 not null, " + //
                "    cnt1        integer       default 0 not null, " + //кол-во жильцов в лс с учетом времен. выбывших
                "    cnt2        integer       default 0 not null, " + //кол-во жильцов в лс без учета времен. выбывших
                "    cnt3        integer       default 0 not null, " + //кол-во дней cnt1 x 31 (кол-во дней проживания)
                "    val1        " + DBManager.sDecimalType + "(11,7) default 0.00, " +//итоговое кол-во жильцов в лс с учетом времен. выбывших
                    "    val2        " + DBManager.sDecimalType + "(11,7) default 0.00, " + //nzp_prm = 5
                    "    val3        " + DBManager.sDecimalType + "(11,7) default 0.00, " + //nzp_prm = 131
                "    val4        " + DBManager.sDecimalType + "(11,7) default 0.00, " +//дробное кол-во жильцов в лс по kart с учетом времен. выбывших
                "    val5        " + DBManager.sDecimalType + "(11,7) default 0.00, " +//дробное кол-во времен. выбывших
                "    val6        " + DBManager.sDecimalType + "(11,7) default 0.00, " +//Количество не зарегистрированных проживающих
                "    kod_info    integer       default 0 ) ");

            BillingInstrumentary.ExecSQL(" create unique index ix1_" + gilTable.Replace(".", "") + " on " + gilTable + " (nzp_gx) ");

            BillingInstrumentary.ExecSQL(" create index ix2_" + gilTable.Replace(".", "") + " on " + gilTable + " (nzp_dom,dat_charge) ");

            BillingInstrumentary.ExecSQL(" create index ix3_" + gilTable.Replace(".", "") + " on " + gilTable + " (nzp_kvar,dat_charge,stek, dat_s,dat_po) ");

            BillingInstrumentary.ExecSQL(" create index ix4_" + gilTable.Replace(".", "") + " on " + gilTable + " (nzp_kvar,nzp_gil,dat_charge) ");

            BillingInstrumentary.ExecSQL(" create index ix5_" + gilTable.Replace(".", "") + " on " + gilTable + " (cur_zap) ");

            BillingInstrumentary.ExecSQL(" create index ix6_" + gilTable.Replace(".", "") + " on " + gilTable + " (prev_zap) ");
        }

    }
}
