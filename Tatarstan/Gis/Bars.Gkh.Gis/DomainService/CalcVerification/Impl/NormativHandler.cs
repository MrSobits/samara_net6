namespace Bars.Gkh.Gis.DomainService.CalcVerification.Impl
{
    using System;
    using System.Data;

    using Bars.B4;
    using Bars.Gkh.Gis.Entities.CalcVerification;
    using Bars.Gkh.Gis.KP_legacy;

    using Castle.Windsor;

    using Intf;

    /// <summary>
    /// Получить нормативы из ЦХД
    /// </summary>
    public class NormativeHandler : INormative
    {
        public IDataResult ApplyNormatives(string TargetTable)
        {
            //todo переопределяем нормативы данными из ЦХД
            throw new System.NotImplementedException();
        }
    }


    /// <summary>
    /// Взять нормативы из УК
    /// </summary>
    public class MONormativeHandler : INormative
    {
        private MONormativeHandler() { }
        protected IDbConnection Connection;
        protected BillingInstrumentary BillingInstrumentary;
        protected IWindsorContainer Container;
        protected CalcVerificationParams CalcParams;
        protected TempTablesLifeTime TempTablesLifeTime;
        public MONormativeHandler(IDbConnection connection, IWindsorContainer container, BillingInstrumentary billingInstrumentary)
        {
            Connection = connection;
            Container = container;
            BillingInstrumentary = billingInstrumentary;
            CalcParams = Container.Resolve<CalcVerificationParams>();
            TempTablesLifeTime = Container.Resolve<TempTablesLifeTime>();
        }
        /// <summary>
        /// Подсчет нормативов на услуги ЖКУ по старому алгоритму в 5.0
        /// </summary>
        /// <param name="param"></param>
        /// <param name="TargetTable"></param>
        /// <returns></returns>
        public IDataResult ApplyNormatives(string TargetTable)
        {
            var rashod = new DbCalcCharge.Rashod(CalcParams.ParamCalc);

            //cтруктура для получения 
            var TableNormatives = CreateTempNormatives(rashod, TargetTable);
            //N: электроснабжение
            CalcNormElEn(rashod);
            //N: хвс гвс канализация
            CalcRashodNormHVandKAN(rashod, TargetTable);
            //N: газ 
            CalcRashodNormGas(rashod);
            //N: электроотопление
            CalcRashodNormElOtopl();
            //N: газовое отопление
            CalcRashodNormGasOtopl(rashod);
            //N: полив
            CalcRashodNormPoliv(rashod);
            //N: отопление
            CalcRashodNormOtopl(rashod);
            //N: вода для бани
            CalcRashodNormHVforBanja();
            //N: питьевая вода
            CalcRashodNormPitHV();
            //применить полученные нормативы УК
            ApplyMONorms(TargetTable, TableNormatives);

            return new BaseDataResult();
        }

        /// <summary>
        /// применяем полученные нормативы УК
        /// </summary>
        /// <param name="TargetTable"></param>
        /// <param name="TableNormatives"></param>
        private void ApplyMONorms(string TargetTable, string TableNormatives)
        {
            //применяем полученные нормативы
            var sql = " UPDATE " + TargetTable + " t " +
                      " SET rash_norm_one=n.rash_norm_one, rash_norm_one_chd=n.rash_norm_one" +
                      " FROM  " + TableNormatives + " n " +
                      " WHERE n.nzp_kvar=t.nzp_kvar " +
                      " AND n.nzp_serv=t.nzp_serv";
            BillingInstrumentary.ExecSQL(sql);
        }

        /// <summary>
        /// Нормативы по электро-энергии
        /// </summary>
        /// <param name="rashod"></param>
        /// <param name="b_calc_kvar"></param>
        /// <returns></returns>
        private void CalcNormElEn(DbCalcCharge.Rashod rashod)
        {
            var iNzpPrmEE = 730;
            if ((new DateTime(rashod.paramcalc.calc_yy, rashod.paramcalc.calc_mm, 01)) >= (new DateTime(2012, 09, 01)))
            {
                iNzpPrmEE = 1079;
            }

            BillingInstrumentary.ExecSQL(
                " Update ttt_counters_xx " +
                " Set cnt4 = " + iNzpPrmEE + //дом не-МКД (по всем нормативным строкам лс и домам)
                " Where nzp_serv in (25,210) " +
                "   and 0 < (" +
                          " Select count(*) From ttt_prm_2 p " +
                          " Where ttt_counters_xx.nzp_dom = p.nzp " +
                          "   and p.nzp_prm = " + iNzpPrmEE +
                          " ) ");


            TempTablesLifeTime.AddTempTable("ttt_aid_c1", " create temp table ttt_aid_c1 (nzp_kvar integer) ");


            BillingInstrumentary.ExecSQL(
                  " Insert into ttt_aid_c1 (nzp_kvar)" +
                  " Select nzp as nzp_kvar " +
                  " From ttt_counters_xx a, ttt_prm_1 p " +
                  " Where a.nzp_kvar = p.nzp " +
                  "   and a.nzp_serv in (25,210) " +
                  "   and p.nzp_prm = 19 " + // электроплита
                  " Group by 1 "
                );

            TempTablesLifeTime.AddTempTable("ttt_aid_c1x",
                " create temp table ttt_aid_c1x (nzp_kvar integer) "
                );


            BillingInstrumentary.ExecSQL(
                " Insert into ttt_aid_c1x (nzp_kvar)" +
                " Select nzp_kvar From ttt_aid_c1 "
                );


            BillingInstrumentary.ExecSQL(" Create unique index ix_ttt_aid_c1x on ttt_aid_c1x (nzp_kvar) ");


            BillingInstrumentary.ExecSQL(
                  " Insert into ttt_aid_c1 (nzp_kvar)" +
                  " Select a.nzp_kvar " +
                  " From ttt_counters_xx a, ttt_prm_2 p " +
                  " Where a.nzp_dom = p.nzp " +
                  "   and a.nzp_serv in (25,210) " +
                  "   and p.nzp_prm = 28 " + // электроплита на дом
                  "   and 0 = ( Select count(*) From ttt_aid_c1x k Where a.nzp_kvar = k.nzp_kvar ) " +
                  " Group by 1 "
                );


            BillingInstrumentary.ExecSQL(" Create unique index ix_ttt_aid_c1 on ttt_aid_c1 (nzp_kvar) ");

            var iNzpResMKD = LoadIntValPrmForNorm(rashod.paramcalc.data_alias, 181, "13", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);
            var iNzpRes = LoadIntValPrmForNorm(rashod.paramcalc.data_alias, 183, "13", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);

            if ((iNzpRes != 0) && (iNzpResMKD != 0))
            {
                BillingInstrumentary.ExecSQL(
                    " Update ttt_counters_xx " +
                    " Set cnt3 = (case when cnt4 = " + iNzpPrmEE + " then " + iNzpRes + " else " + iNzpResMKD + " end) " + //лс с электроплитой (в зависимости от МКД)
                    " Where nzp_serv in (25,210) " +
                    "   and 0 < ( Select count(*) From ttt_aid_c1 k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar ) "
                    );

            }

            iNzpResMKD = LoadIntValPrmForNorm(rashod.paramcalc.data_alias, 180, "13", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);
            iNzpRes = LoadIntValPrmForNorm(rashod.paramcalc.data_alias, 182, "13", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);

            if ((iNzpRes != 0) && (iNzpResMKD != 0))
            {
                BillingInstrumentary.ExecSQL(
                    " Update ttt_counters_xx " +
                    " Set cnt3 = (case when cnt4 = " + iNzpPrmEE + " then " + iNzpRes + " else " + iNzpResMKD + " end) " + //значит остальные без электроплиты (где <> -2;-17)
                    " Where nzp_serv in (25,210) " +
                    "   and cnt3 = 0 "
                    );

            }

            var sKolGil = "cnt1";
            var sKolGil_g = "cnt1_g";
            if ((new DateTime(rashod.paramcalc.calc_yy, rashod.paramcalc.calc_mm, 01)) < (new DateTime(2012, 09, 01)))
            {
                sKolGil = "1"; sKolGil_g = "1";
            }

            #region применить спец.таблицу расходов ЭЭ в РТ - если стоит признак на базу (nzp_prm=163) и наличие ИПУ (nzp_prm=101)

            TempTablesLifeTime.DropTempTable("ttt_aid_c1x", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_c1x",
                " create temp table ttt_aid_c1x (nzp_kvar integer)"
                );


            // 101|Наличие ЛС счетчика эл/эн|1||bool||1||||
            BillingInstrumentary.ExecSQL(
                  " Insert into ttt_aid_c1x (nzp_kvar)" +
                  " Select a.nzp_kvar " +
                  " From ttt_counters_xx a, ttt_prm_1 p " +
                  " Where a.nzp_kvar = p.nzp " +
                  "   and a.nzp_serv in (25,210) " +
                  "   and p.nzp_prm = 101 " +

                  // 163|Отменить таблицу нормативов по эл/энергии Пост.№761|1||bool||5||||
                  "   and 0 < ( Select count(*) From " + rashod.paramcalc.data_alias + "prm_5 p5 " +
                       " where p5.nzp_prm=163 and p5.val_prm='1' and p5.is_actual<>100 " +
                       "   and p5.dat_s  <= " + rashod.paramcalc.dat_po + " and p5.dat_po >= " + rashod.paramcalc.dat_s +
                       "   ) Group by 1 ");


            BillingInstrumentary.ExecSQL(" Create unique index ix_ttt_aid_c1x on ttt_aid_c1x (nzp_kvar) ");

            string sql = " Update ttt_counters_xx " +
                         " Set cnt3 = 13 " +
                         " Where nzp_serv in (25,210) " +
                         "   and 0 < ( Select count(*) From ttt_aid_c1x k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar ) ";
            BillingInstrumentary.ExecSQL(sql);


            TempTablesLifeTime.DropTempTable("ttt_aid_c1x ", false);
            TempTablesLifeTime.DropTempTable("ttt_aid_c1xx ", false);

            #endregion применить спец.таблицу расходов ЭЭ в РТ - если стоит признак на базу (nzp_prm=163) и наличие ИПУ (nzp_prm=101)

            sql =
                    " Update ttt_counters_xx " +
                    " Set val1 = (COALESCE " +
                               "(( Select value From " + rashod.paramcalc.kernel_alias + "res_values " +
                                 " Where nzp_res = cnt3 " +
                                 "   and nzp_y = (case when cnt3 = 13" + //кол-во людей
                                                " then (case when cnt1 >11 then 11 else cnt1 end)" +
                                                " else (case when cnt1 > 6 then  6 else cnt1 end)" +
                                                " end) " +
                                 "   and nzp_x = (case when cnt3 = 13" + //кол-во комнат
                                                " then 1" +
                                                " else (case when cnt2 > 5 then  5 else cnt2 end)" +
                                                " end) " +
                                 " ),'0')::numeric) " +
                                 " * (case when cnt3 = 13 then 1 else " + sKolGil + " end)" +
                    " , rash_norm_one = COALESCE " +
                               "(( Select value From " + rashod.paramcalc.kernel_alias + "res_values " +
                                 " Where nzp_res = cnt3 " +
                                 "   and nzp_y = (case when cnt3 = 13" + //кол-во людей
                                                " then (case when cnt1 >11 then 11 else cnt1 end)" +
                                                " else (case when cnt1 > 6 then  6 else cnt1 end)" +
                                                " end) " +
                                 "   and nzp_x = (case when cnt3 = 13" + //кол-во комнат
                                                " then 1" +
                                                " else (case when cnt2 > 5 then  5 else cnt2 end)" +
                                                " end) " +
                                 " ),'0') ::numeric" +
                                 " / (case when cnt3 = 13 and " + sKolGil + " > 0 then " + sKolGil + " else 1 end)" +
                    " , val1_g = (COALESCE " +
                               "(( Select value From " + rashod.paramcalc.kernel_alias + "res_values " +
                                 " Where nzp_res = cnt3 " +
                                 "   and nzp_y = (case when cnt3 = 13" + //кол-во людей
                                                " then (case when cnt1_g >11 then 11 else cnt1_g end)" +
                                                " else (case when cnt1_g > 6 then  6 else cnt1_g end)" +
                                                " end) " +
                                 "   and nzp_x = (case when cnt3 = 13" + //кол-во комнат
                                                " then 1" +
                                                " else (case when cnt2 > 5 then  5 else cnt2 end)" +
                                                " end) " +
                                 " ),'0')::numeric) " +
                                 " * (case when cnt3 = 13 then 1 else " + sKolGil_g + " end)" +
                    " Where nzp_serv in (25,210) " +
                    "   and cnt1_g > 0 and cnt2 > 0 " +
                    "   and cnt3 in ( Select nzp_res From " + rashod.paramcalc.kernel_alias + "res_values ) ";
            BillingInstrumentary.ExecSQL(sql);

        }

        #region N: хвс гвс канализация
        private void CalcRashodNormHVandKAN(DbCalcCharge.Rashod rashod, string TargetTable)
        {
            //----------------------------------------------------------------
            //N: хвс гвс канализация
            //----------------------------------------------------------------

            var iNzpRes = LoadIntValPrmForNorm(rashod.paramcalc.data_alias, 172, "13", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);
            if (iNzpRes != 0)
            {
                BillingInstrumentary.ExecSQL(
                    " Update ttt_counters_xx " +
                    " Set cnt3 = " + iNzpRes +
                    " Where nzp_serv in (6,7,324) "
                    );

            }

            iNzpRes = LoadIntValPrmForNorm(rashod.paramcalc.data_alias, 177, "13", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);
            if (iNzpRes != 0)
            {
                BillingInstrumentary.ExecSQL(
                    " Update ttt_counters_xx " +
                    " Set cnt3 = " + iNzpRes +
                    " Where nzp_serv in (9,281,323) "
                    );

            }

            string sql;
            var sdn = "";

            #region  выбрать ЛС с расходом по КАН меньше суммы расхода ХВС+ГВС

            #endregion

            // выбрать КАН только по ХВ
            TempTablesLifeTime.AddTempTable("t_is339",
                " create temp table t_is339 (nzp_kvar integer) "
                );


            BillingInstrumentary.ExecSQL(
                " insert into t_is339 (nzp_kvar) " +
                " select nzp_kvar " +
                " from " + TargetTable + " " +
                " where nzp_serv in (7,324) and nzp_frm=339 " +
                " group by 1 "
                );


            BillingInstrumentary.ExecSQL(" Create index ix_t_is339 on t_is339 (nzp_kvar) ");

            // отметить КАН только по ХВ
            BillingInstrumentary.ExecSQL(
                " Update ttt_counters_xx " +
                " Set rvirt = 1" +
                " Where nzp_serv in (7,324) " +
                  " and 0<(select count(*) from t_is339 t where t.nzp_kvar=ttt_counters_xx.nzp_kvar )"
                );


            sql =
                " Update ttt_counters_xx Set " +
                // норма на 1 человека - ХВС, ГВС и КАН
                " rash_norm_one =" +

                " case when nzp_serv in (9,281,323) then " +

                // ... beg норматив на ГВС
                " (( Select COALESCE(value,'0') From " + rashod.paramcalc.kernel_alias + "res_values " +
                    "  Where nzp_res = cnt3 " +
                    "   and nzp_y = cnt2 " + //тип водоснабжения
                    "   and nzp_x = 2 " +
                    " ) ::numeric) " +
                    sdn +
                // ... end норматив на ГВС

                " else " +

                // ... beg норматив на ХВС и КАН
                " (( Select COALESCE(value,'0') From " + rashod.paramcalc.kernel_alias + "res_values " +
                    "  Where nzp_res = cnt3 " +
                    "   and nzp_y = cnt2 " + //тип водоснабжения
                    "   and nzp_x = (case when nzp_serv=6 then 1 else 3 end) " + // на нужды ХВ
                    " )::numeric) " +
                    sdn +

                " + case when nzp_serv=6 or rvirt = 1 then 0 else " +

                   "(( Select COALESCE(value,'0') From " + rashod.paramcalc.kernel_alias + "res_values " +
                       " Where nzp_res = cnt3 " +
                       "   and nzp_y = cnt2 " + //тип водоснабжения
                       "   and nzp_x = (case when nzp_serv=6 then 2 else 4 end) " + // на нужды ГВ
                       " )::numeric) " +
                       sdn +

                   " end " +
                // ... end норматив на ХВС и КАН

               " end " +

                // норма на 1 человека в части ХВС - ХВС и КАН
                ", val3 = " +
                // ... beg доля норматива на ХВС - только для ХВС и КАН - для ГВС нет
                " case when nzp_serv in (9,281,323) then 0 else " +
                " (( Select COALESCE(value,'0') From " + rashod.paramcalc.kernel_alias + "res_values " +
                    " Where nzp_res = cnt3 " +
                    "   and nzp_y = cnt2 " + //тип водоснабжения
                    "   and nzp_x = (case when nzp_serv=6 then 1 else 3 end) " + // на нужды ХВ
                    " )::numeric) " +
                    sdn + " " +
               " end " +
                // ... end доля норматива на ХВС - только для ХВС и КАН - для ГВС нет

               " Where nzp_serv in (6,7,324, 9,281,323) " +
               "   and cnt2 > 0 " +
               "   and cnt3 in ( Select nzp_res From " + rashod.paramcalc.kernel_alias + "res_values ) ";

            BillingInstrumentary.ExecSQL(sql);



            // нужды ХВ + нужды ГВ !!! для ХВ (и для КАН по ХВ -> rvirt = 1) расход ГВ НЕ добавлять !!!
            sql =
                " Update ttt_counters_xx Set " +
                " val1   = COALESCE(rash_norm_one * gil1,0) " +
                ",val1_g = COALESCE(rash_norm_one * gil1_g,0) " +
                ",val4   = COALESCE(val3 * gil1,0) " +
                " Where nzp_serv in (6,7,324, 9,281,323) " +
                "   and cnt2 > 0 ";
            BillingInstrumentary.ExecSQL(sql);

        }
        #endregion N: хвс гвс канализация

        #region N: отопление
        private void CalcRashodNormOtopl(DbCalcCharge.Rashod rashod)
        {
            //----------------------------------------------------------------
            //N: отопление
            //----------------------------------------------------------------
            var sql =
                " Update ttt_counters_xx " +
                " Set cnt3 = 61" + // норма по площади
                " Where nzp_serv in (8,322,325) ";
            BillingInstrumentary.ExecSQL(sql);


            var sKolGPl = "5";

            // норма по площади на 1 человека (Кж=cnt1)
            sql =
                " Update ttt_counters_xx " +
                " Set cnt2 = ( Select COALESCE(value,'0') From " + rashod.paramcalc.kernel_alias + "res_values " +
                             " Where nzp_res = cnt3 " +
                             "   and nzp_y = (case when cnt1 > " + sKolGPl + " then " + sKolGPl + " else cnt1 end) " + //кол-во людей
                             "   and nzp_x = 2 " + //
                             " )::int" +
                " Where nzp_serv in (8,322,325) " +
                "   and cnt1 > 0 " +
                "   and cnt3 in ( Select nzp_res From " + rashod.paramcalc.kernel_alias + "res_values ) ";
            BillingInstrumentary.ExecSQL(sql);


            //
            // ... расчет норматива на 1 кв.м ...
            //

            // признак отключения расчета норматива отопления
            var bCalcNormOtopl =
                !CheckBoolValPrmWithVal(rashod.paramcalc.data_alias, 478, "5", "1", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);

            TempTablesLifeTime.AddTempTable("t_norm_otopl",
                " create temp table t_norm_otopl ( " +
                " nzp_dom         integer, " +
                " nzp_kvar        integer, " +
                " s_otopl         NUMERIC( 8,2) default 0, " +    // отапливаемая площадь
                " rashod_gkal_m2f NUMERIC(12,8) default 0, " +    // расход в ГКал на 1 м2 фактический
                // вид учтенного расхода на л/с (1-норматив/2-ИПУ расход ГКал/3-дом.норма ГКал/4-ОДПУ расход ГКал)
                " vid_gkal_ls     integer default 0,       " +
                " rashod_gkal_dom NUMERIC(12,8) default 0, " +    // расход в ГКал на 1 м2
                " vid_gkal_dom    integer default 0,       " +    // вид учтенного домового расхода

                " rashod_gkal_m2  NUMERIC(12,8) default 0, " +    // нормативный расход в ГКал на 1 м2
                // для расчета норматива по отоплению
                " koef_god_pere   NUMERIC( 6,4) default 1, " +    // коэффициент перерасчета по итогам года
                " ugl_kv          integer default 0,       " +    // признак угловой квартиры (0-обычная/1-угловая)
                " vid_alg         integer default 1,       " +    // вид методики расчета норматива
                " tmpr_vnutr_vozd NUMERIC( 8,4) default 0, " +    // температура внутреннего воздуха (обычно = 20, для угловых = 22)
                " tmpr_vnesh_vozd NUMERIC( 8,4) default 0, " +    // средняя температура внешнего воздуха  (обычно = -5.7)
                " otopl_period     integer default 0,      " +    // продолжительность отопительного периода в сутках (обычно = 218)
                " nzp_res0         integer default 0,      " +    // таблица нормативов
                " dom_klimatz      integer default 0,      " +    // Климатическая зона
                // vid_alg=1 - памфиловская методика расчета норматива
                " dom_objem        NUMERIC(12,2) default 0, " +   // объем дома
                " dom_pol_pl       NUMERIC(12,2) default 1, " +   // полезная/отапливаемая площадь дома
                " dom_ud_otopl_har NUMERIC(12,8) default 1, " +   // удельная отопительная характеристика дома
                " dom_otopl_koef   NUMERIC( 8,4) default 1, " +   // поправочно-отопительный коэффициент для дома
                // vid_alg=2 - методика расчета норматива по Пост306 без интерполяции удельного расхода тепловой энергии
                // vid_alg=3 - методика расчета норматива по Пост306  с интерполяцией удельного расхода тепловой энергии
                // vid_alg=4 - методика расчета норматива - табличное значение от этажа и года постройки дома
                " dom_dat_postr    date default '1.1.1900', " +   // дата постройки дома
                " dom_kol_etag     integer default 0,       " +   // количество этажей дома (этажность)
                " pos_etag         integer default 0,       " +   // позиция по количеству этажей дома в таблице удельных расходов тепловой энергии
                " pos_narug_vozd   integer default 0,       " +   // позиция по температуре наружного воздуха в таблице удельных расходов тепловой энергии
                " dom_ud_tepl_en1  NUMERIC(12,8) default 0, " +   // минимальный  удельный расход тепловой энергии для дома по температуре и этажности
                " dom_ud_tepl_en2  NUMERIC(12,8) default 0, " +   // максимальный удельный расход тепловой энергии для дома по температуре и этажности
                " tmpr_narug_vozd1 NUMERIC( 8,4) default 0, " +   // минимально  близкая температура наружного воздуха в таблице
                " tmpr_narug_vozd2 NUMERIC( 8,4) default 0, " +   // максимально близкая температура наружного воздуха в таблице
                " tmpr_narug_vozd  NUMERIC( 8,4) default 0, " +   // температура наружного воздуха по проекту (паспорту) дома
                " dom_ud_tepl_en   NUMERIC(12,8) default 0,  " +   // удельный расход тепловой энергии для дома по температуре и этажности
                " norm_type_id integer,  " +
                " norm_tables_id integer " + // id норматива - по нему можно получить набор влияющих пар-в и их знач.
                " ) "
                );


            // === перечень л/с для расчета нормативов ===
            BillingInstrumentary.ExecSQL(
                  " insert into t_norm_otopl (nzp_dom,nzp_kvar,s_otopl)" +
                  " select nzp_dom,nzp_kvar,max(squ2) from ttt_counters_xx " +
                  " where nzp_serv=8 " +
                  " group by 1,2 "
                  );


            BillingInstrumentary.ExecSQL(" create index ix1_norm_otopl on t_norm_otopl (nzp_kvar) ");
            BillingInstrumentary.ExecSQL(" create index ix2_norm_otopl on t_norm_otopl (nzp_dom) ");


            int iNzpRes;
            // если разрешено рассчитывать норматив по отоплению
            if (bCalcNormOtopl)
            {
                #region N: отопление - расчет нормативов по типам

                // === параметры для всех алгоритмов расчета нормативов ===

                // нормативы от этажей (для РТ) и климатических зон (для сах РС-Я)
                iNzpRes = LoadIntValPrmForNorm(rashod.paramcalc.data_alias, 186, "13", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);
                if (iNzpRes != 0)
                {
                    BillingInstrumentary.ExecSQL(
                          " Update t_norm_otopl " +
                          " Set nzp_res0 = " + iNzpRes + " Where 1=1 "
                        );

                }

                // количество этажей дома (этажность)
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_kol_etag=" +
                      "    ( select max( replace(COALESCE(p.val_prm,'0'),',','.')::int ) from ttt_prm_2 p " +
                      "      where t_norm_otopl.nzp_dom=p.nzp " +
                             " and p.nzp_prm=37 " +
                      " )" +
                      " where exists" +
                      "    ( select 1 from ttt_prm_2 p " +
                      "      where t_norm_otopl.nzp_dom=p.nzp " +
                             " and p.nzp_prm=37 " +
                      " ) "
                    );


                //ViewTbl( " select * from t_norm_otopl where nzp_kvar=3829 ");

                // поиск указанного норматива в ГКал на 1 кв. метр. вид методики расчета норматива = 0

                TempTablesLifeTime.AddTempTable("t_otopl_m2",
                    " create temp table t_otopl_m2 ( " +
                    " nzp_dom         integer, " +
                    " rashod_gkal_m2  NUMERIC(12,8) default 0 " +    // нормативный расход в ГКал на 1 м2
                    " )  "
                     );


                // === алгоритм расчет нормативов 0 ===
                BillingInstrumentary.ExecSQL(
                      " Insert into t_otopl_m2 (nzp_dom,rashod_gkal_m2)" +
                      " Select p.nzp,max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                      " From ttt_prm_2 p " +
                      " Where p.nzp_prm=723 " +
                      "    and exists (select 1 from t_norm_otopl t where t.nzp_dom=p.nzp) " +
                      " group by 1 "
                      );


                // вид методики расчета норматива
                BillingInstrumentary.ExecSQL(
                      " Update t_norm_otopl set vid_alg=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.') ::int) " +
                           " From ttt_prm_2 p " +
                      "      Where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=709 " +
                      " )" +
                      " Where vid_alg<>0 and exists" +
                      "    ( Select 1 " +
                           " From ttt_prm_2 p " +
                      "      Where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=709 " +
                      " ) "
                    );


                // коэффициент перерасчета по итогам года
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set koef_god_pere=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from " + rashod.paramcalc.data_alias + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=108" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " )" +
                      " where exists" +
                      "    ( select 1 " +
                           " from " + rashod.paramcalc.data_alias + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=108" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s +
                      " ) "
                    );


                BillingInstrumentary.ExecSQL(" create index ix1_otopl_m2 on t_otopl_m2 (nzp_dom) ");

                BillingInstrumentary.ExecSQL(
                      " Update t_norm_otopl" +
                      " Set vid_alg=0, rashod_gkal_m2=( Select rashod_gkal_m2 From t_otopl_m2 p Where t_norm_otopl.nzp_dom=p.nzp_dom )" +
                      " Where exists ( Select 1 From t_otopl_m2 p Where t_norm_otopl.nzp_dom=p.nzp_dom ) "
                    );

                //
                #region Выбрать норму Гкал на 1 м2 для отопления на лицевой счет prm_1 nzp_prm =2463

                // выбрать параметры л/с для типа 1814 на дату расчета
                TempTablesLifeTime.DropTempTable("t_p2463 ", false);

                TempTablesLifeTime.AddTempTable("t_p2463",
                    " create temp table t_p2463 (" +
                    " nzp     integer," +
                    " vald    NUMERIC(14,7))  ");


                // Выбрать квартирный норматив гкал на м3
                BillingInstrumentary.ExecSQL(
                    " insert into t_p2463 (nzp,vald)" +
                    " select nzp,max(COALESCE(val_prm,'0')::numeric) " +
                    " from ttt_prm_1" +
                    " where nzp_prm=2463 " +
                    " group by 1 "
                    );
                BillingInstrumentary.ExecSQL(" create index ixt_p2463 on t_p2463(nzp) ");

                BillingInstrumentary.ExecSQL(
                    " update t_norm_otopl" +
                    " set vid_alg=0, rashod_gkal_m2=( select p2.vald from t_p2463 p2 where p2.nzp=t_norm_otopl.nzp_kvar ) " +
                    " where exists ( select 1 from t_p2463 p1 where p1.nzp=t_norm_otopl.nzp_kvar ) "
                    );
                TempTablesLifeTime.DropTempTable("t_p2463 ", false);

                #endregion Выбрать норму Гкал на 1 м2 для отопления на лицевой счет prm_1 nzp_prm =2463


                // === алгоритмы расчета нормативов 1, 2, 3, 4 ===

                // признак угловой квартиры (0-обычная/1-угловая)
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set ugl_kv=1" +
                      " where 0<(select count(*) from ttt_prm_1 p where t_norm_otopl.nzp_kvar=p.nzp and p.nzp_prm=310) "
                    );


                // vid_alg=1 ==================================

                // === параметры на всю БД (prm_5 - в 2.0 указаны в формуле!) ===
                // tmpr_vnutr_vozd -- температура внутреннего воздуха (обычно = 20, для угловых = 22)
                // tmpr_vnesh_vozd -- средняя температура внешнего воздуха  (обычно = -5.7)
                // otopl_period    -- продолжительность отопительного периода в сутках (обычно = 218)
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set" +
                      "  tmpr_vnutr_vozd = (case when ugl_kv=1 then 22 else 20 end)," +
                      "  tmpr_vnesh_vozd = -5.7," +
                      "  otopl_period    = 218" +
                      " where vid_alg=1 "
                    );


                // === домовые параметры (prm_2) ===
                // поправочно-отопительный коэффициент для дома
                BillingInstrumentary.ExecSQL(
                      " Update t_norm_otopl Set dom_otopl_koef=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " From ttt_prm_2 p Where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=33 " +
                      " )" +
                      " Where vid_alg=1 and 0<" +
                      "    ( Select count(*)" +
                           " From ttt_prm_2 p Where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=33 " +
                      " ) "
                    );


                // объем дома
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_objem=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=32 " +
                      " )" +
                      " where vid_alg=1 and 0<" +
                      "    ( select count(*)" +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=32 " +
                      " ) "
                    );


                // полезная/отапливаемая площадь дома
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_pol_pl=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=36 " +
                      " )" +
                      " where vid_alg=1 and 0<" +
                      "    ( select count(*)" +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=36 " +
                      " ) "
                    );


                // удельная отопительная характеристика дома
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_ud_otopl_har=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=31 " +
                      " )" +
                      " where vid_alg=1 and 0<" +
                      "    ( select count(*)" +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=31 " +
                      " ) "
                    );


                // vid_alg=2 / 3 / 4 ==================================

                // === параметры на всю БД (prm_5) ===
                // температура внутреннего воздуха (обычно = 20, для угловых = 22)
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set tmpr_vnutr_vozd=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from " + rashod.paramcalc.data_alias + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=54" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " )" +
                      " where vid_alg in (2,3) and 0<" +
                      "    ( select count(*) from " + rashod.paramcalc.data_alias + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=54" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " ) "
                    );


                // для угловых
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set tmpr_vnutr_vozd=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from " + rashod.paramcalc.data_alias + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=713" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " )" +
                      " where vid_alg in (2,3) and ugl_kv=1 and 0<" +
                      "    ( select count(*) from " + rashod.paramcalc.data_alias + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=713" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " ) "
                    );

                // средняя температура внешнего воздуха  (обычно = -5.7)
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set tmpr_vnesh_vozd=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from " + rashod.paramcalc.data_alias + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=710" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " )" +
                      " where vid_alg in (2,3) and 0<" +
                      "    ( select count(*) from " + rashod.paramcalc.data_alias + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=710" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " ) "
                    );


                // продолжительность отопительного периода в сутках (обычно = 218)
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set otopl_period=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from " + rashod.paramcalc.data_alias + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=712" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " )" +
                      " where vid_alg in (2,3) and 0<" +
                      "    ( select count(*) from " + rashod.paramcalc.data_alias + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=712" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " ) "
                    );


                // === домовые параметры (prm_2) ===
                // дата постройки дома
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_dat_postr=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::date) " +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=150 " +
                      " )" +
                      " where vid_alg in (2,3,4) and 0<" +
                      "    ( select count(*)" +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=150 " +
                      " ) "
                    );


                // количество этажей дома (этажность)
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_kol_etag=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::int) " +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=37 " +
                      " )" +
                      " where vid_alg in (2,3,4) and 0<" +
                      "    ( select count(*)" +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=37 " +
                      " ) "
                    );


                // температура наружного воздуха по проекту (паспорту) дома
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set tmpr_narug_vozd=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=711 " +
                      " )" +
                      " where vid_alg in (2,3) and 0<" +
                      "    ( select count(*)" +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=711 " +
                      " ) "
                    );



                TempTablesLifeTime.AddTempTable("t_etag1999",
                    " create temp table t_etag1999(y1 int,val1 char(20),etag1 integer,etag2 integer) "
                    );

                const string sposfun = "strpos";


                string ssql = " insert into t_etag1999(y1,val1,etag1,etag2) " +
                              " select " +
                              " r.nzp_y y1,r.value val1" +
                              ",(case when " + sposfun + "(r.value,'-')=0" +
                              "       then (r.value::int) else substr(r.value,1," +
                              sposfun + "(r.value,'-')-1)::int" +
                              "  end) etag1 " +
                              ",COALESCE((case when " + sposfun + "(b.value,'-')=0" +
                              "       then (b.value::int) else substr(b.value,1," +
                              sposfun + "(b.value,'-')-1)::int" +
                              "  end),9999) etag2 " +
                              " from " + rashod.paramcalc.kernel_alias + "res_values r " +

                              " left outer join " + rashod.paramcalc.kernel_alias + "res_values b " +
                              " on r.nzp_y=b.nzp_y-1 and b.nzp_res=9996 and b.nzp_x=1 where 1=1" +

                              " and r.nzp_res=9996 and r.nzp_x=1  ";
                BillingInstrumentary.ExecSQL(ssql);


                BillingInstrumentary.ExecSQL(" create index ix_etag1999 on t_etag1999 (etag1,etag2) ");

                // таблица диапазонов этажей для таблицы удельных расходов тепловой энергии (Пост.306) после 1999 года
                TempTablesLifeTime.AddTempTable("t_etag",
                    " create temp table t_etag(y1 int,val1 char(20),etag1 integer,etag2 integer) "
                    );


                ssql =
                    " insert into t_etag(y1,val1,etag1,etag2) " +
                    " select " +
                    " r.nzp_y y1,r.value val1" +
                    ",(case when " + sposfun + "(r.value,'-')=0" +
                    "       then (r.value::int) else substr(r.value,1," +
                    sposfun + "(r.value,'-')-1)::int" +
                    "  end) etag1 " +
                    ",COALESCE((case when " + sposfun + "(b.value,'-')=0" +
                    "       then (b.value::int) else substr(b.value,1," +
                    sposfun + "(b.value,'-')-1)::int" +
                    "       end),9999) etag2 " +
                    " from " + rashod.paramcalc.kernel_alias + "res_values r " +

 " left outer join " + rashod.paramcalc.kernel_alias + "res_values b" +
                    " on r.nzp_y=b.nzp_y-1 and b.nzp_res=9997 and b.nzp_x=1 where 1=1 " +

 " and r.nzp_res=9997 and r.nzp_x=1 ";
                BillingInstrumentary.ExecSQL(ssql);


                BillingInstrumentary.ExecSQL(" create index ix_etag on t_etag (etag1,etag2) ");

                // таблица диапазонов температур для таблицы удельных расходов тепловой энергии (Пост.306)
                BillingInstrumentary.ExecSQL(
                    " select  r.nzp_y y1,r.value val1" +
                    ",(case when strpos(r.value,'-')=0" +
                    "       then (r.value::int) else substring(r.value,1,strpos(r.value,'-')-1)::int" +
                    "  end) tmpr1 " +
                    ",coalesce((case when strpos(b.value,'-')=0" +
                    "       then (b.value::int) else substring(b.value,1,strpos(b.value,'-')-1)::int" +
                    "       end),9999) tmpr2 " +
                    " into temp t_tmpr  " +
                    " from " + rashod.paramcalc.pref + "_kernel.res_values r " +
                    " left outer join " + rashod.paramcalc.pref +
                    "_kernel.res_values b on r.nzp_y=b.nzp_y-1 and b.nzp_res=9991 and b.nzp_x=1 " +
                    " where r.nzp_res=9991 and r.nzp_x=1 ");


                BillingInstrumentary.ExecSQL(" create index ix_tmpr on t_tmpr (tmpr1,tmpr2) ");


                // позиция по количеству этажей дома в таблице удельных расходов тепловой энергии до 1999
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set pos_etag=" +
                      "    (select max(b.y1) from t_etag1999 b where t_norm_otopl.dom_kol_etag>=b.etag1 and t_norm_otopl.dom_kol_etag<b.etag2)" +
                      " where vid_alg in (2,3) and dom_dat_postr<=" + BillingInstrumentary.MDY(1, 1, 1999)
                    );


                // позиция по количеству этажей дома в таблице удельных расходов тепловой энергии после 1999
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set pos_etag=" +
                      "    (select max(b.y1) from t_etag b where t_norm_otopl.dom_kol_etag>=b.etag1 and t_norm_otopl.dom_kol_etag<b.etag2)" +
                      " where vid_alg in (2,3) and dom_dat_postr>" + BillingInstrumentary.MDY(1, 1, 1999)
                    );


                // позиция по количеству этажей дома в таблице нормативных расходов тепловой энергии с 09.2012г в РТ 
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set pos_etag=" +
                      "    (case when dom_kol_etag>16 then 16 else dom_kol_etag end)" +
                      " where vid_alg=4 "
                    );


                //ViewTbl( " select * from t_norm_otopl where nzp_kvar=3829 ");

                // pos_narug_vozd   - позиция по температуре наружного воздуха в таблице удельных расходов тепловой энергии
                // tmpr_narug_vozd1 - минимально  близкая температура наружного воздуха в таблице
                // tmpr_narug_vozd2 - максимально близкая температура наружного воздуха в таблице
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set (pos_narug_vozd,tmpr_narug_vozd1,tmpr_narug_vozd2)=" +
                      "    ((select max(b.y1) from t_tmpr b " +
                      "      where abs(t_norm_otopl.tmpr_narug_vozd)>=b.tmpr1 and abs(t_norm_otopl.tmpr_narug_vozd)<b.tmpr2)," +
                           "(select max(abs(b.tmpr1)) from t_tmpr b " +
                      "      where abs(t_norm_otopl.tmpr_narug_vozd)>=b.tmpr1 and abs(t_norm_otopl.tmpr_narug_vozd)<b.tmpr2)," +
                           "(select max(abs(b.tmpr2)) from t_tmpr b " +
                      "      where abs(t_norm_otopl.tmpr_narug_vozd)>=b.tmpr1 and abs(t_norm_otopl.tmpr_narug_vozd)<b.tmpr2))" +
                      " where vid_alg in (2,3) "
                    );


                // минимальный  удельный расход тепловой энергии для дома по температуре и этажности
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_ud_tepl_en1=" +
                      "    (select max(replace(COALESCE(r.value,'0'),',','.')::numeric) " +
                      " from " + rashod.paramcalc.kernel_alias + "res_values r" +
                      "     where r.nzp_res=9996 and t_norm_otopl.pos_etag=r.nzp_y" +
                            " and (case when t_norm_otopl.pos_narug_vozd>=2 then t_norm_otopl.pos_narug_vozd else 2 end)=r.nzp_x)" +
                      " where vid_alg in (2,3) and dom_dat_postr<=" + BillingInstrumentary.MDY(1, 1, 1999)
                    );


                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_ud_tepl_en1=" +
                      "    (select max(replace(COALESCE(r.value,'0'),',','.')::numeric) " +
                      " from " + rashod.paramcalc.kernel_alias + "res_values r" +
                      "     where r.nzp_res=9997 and t_norm_otopl.pos_etag=r.nzp_y" +
                            " and (case when t_norm_otopl.pos_narug_vozd>=2 then t_norm_otopl.pos_narug_vozd else 2 end)=r.nzp_x)" +
                      " where vid_alg in (2,3) and dom_dat_postr> " + BillingInstrumentary.MDY(1, 1, 1999)
                    );


                // максимальный удельный расход тепловой энергии для дома по температуре и этажности
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_ud_tepl_en2=" +
                      "    (select max(replace(COALESCE(r.value,'0'),',','.')::numeric) " +
                      " from " + rashod.paramcalc.kernel_alias + "res_values r" +
                      "     where r.nzp_res=9996 and t_norm_otopl.pos_etag=r.nzp_y and t_norm_otopl.pos_narug_vozd+1=r.nzp_x)" +
                      " where vid_alg in (2,3) and dom_dat_postr<=" + BillingInstrumentary.MDY(1, 1, 1999)
                    );


                BillingInstrumentary.ExecSQL(
                      " Update t_norm_otopl set dom_ud_tepl_en2=" +
                      "    (select max(replace(COALESCE(r.value,'0'),',','.')::numeric) " +
                      " From " + rashod.paramcalc.kernel_alias + "res_values r" +
                      "     where r.nzp_res=9997 and t_norm_otopl.pos_etag=r.nzp_y and t_norm_otopl.pos_narug_vozd+1=r.nzp_x)" +
                      " Where vid_alg in (2,3) and dom_dat_postr> " + BillingInstrumentary.MDY(1, 1, 1999)
                    );


                // dom_ud_tepl_en - удельный расход тепловой энергии для дома по температуре и этажности
                // без интерполяции
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_ud_tepl_en=dom_ud_tepl_en1" +
                      " where vid_alg=2 "
                    );


                // с интерполяцией
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_ud_tepl_en=dom_ud_tepl_en1+" +
                      "   (dom_ud_tepl_en2-dom_ud_tepl_en1)*(abs(tmpr_narug_vozd)-tmpr_narug_vozd1)/(tmpr_narug_vozd2-tmpr_narug_vozd1)" +
                      " where vid_alg=3 and abs(tmpr_narug_vozd2-tmpr_narug_vozd1)>0.0001 "
                    );


                // === расчет нормативов в ГКал на 1 кв.м - vid_alg=1 ===
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set rashod_gkal_m2 = 0.98 * dom_ud_otopl_har * dom_objem / dom_pol_pl" +
                      " where vid_alg=1 and dom_pol_pl> 0.0001"
                    );


                // === расчет нормативов в ГКал на 1 кв.м - vid_alg in (2,3) ===
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set rashod_gkal_m2 = dom_ud_tepl_en / (tmpr_vnutr_vozd - tmpr_narug_vozd)" +
                      " where vid_alg in (2,3) and abs(tmpr_vnutr_vozd - tmpr_narug_vozd)> 0.0001 "
                    );


                // === расчет нормативов в ГКал на 1 кв.м - общее для vid_alg in (1,2,3) ===
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set rashod_gkal_m2 =" +
                      " rashod_gkal_m2 * (tmpr_vnutr_vozd - tmpr_vnesh_vozd) * otopl_period * 24 * 0.000001 / 12 * koef_god_pere" +
                      " where vid_alg in (1,2,3) "
                    );


                // === установить норматив в ГКал на 1 кв.м - из таблицы для vid_alg=4 ===
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set rashod_gkal_m2=" +
                      "    (select max(replace(COALESCE(r.value,'0'),',','.')::numeric) " +
                      " from " + rashod.paramcalc.kernel_alias + "res_values r" +
                      "     where r.nzp_res=t_norm_otopl.nzp_res0 and t_norm_otopl.pos_etag=r.nzp_y" +
                      " and r.nzp_x=(case when dom_dat_postr<=" + BillingInstrumentary.MDY(1, 1, 1999) + " then 1 else 2 end) )" +
                      " where vid_alg=4 "
                    );


                #endregion N: отопление - расчет нормативов по типам
            }
            else
            {
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set vid_alg=0,rashod_gkal_m2=0 "
                    );

            }
            //ViewTbl( " select * from t_norm_otopl where nzp_kvar=3829 ");

            BillingInstrumentary.ExecSQL(
                  " update t_norm_otopl set rashod_gkal_m2=0 where rashod_gkal_m2 is null  "
                );


            // ===  установка норматива по отоплению в counters_xx ===
            sql =
                " Update ttt_counters_xx " +
                " Set (val1,val3,rash_norm_one,kod_info) = " +
                "((Select rashod_gkal_m2 * ttt_counters_xx.squ2 From t_norm_otopl k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar)," +
                "(Select rashod_gkal_m2 From t_norm_otopl k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar)," +
                "(Select rashod_gkal_m2 From t_norm_otopl k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar)," +
                "(Select vid_alg From t_norm_otopl k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar)) " +
                " Where nzp_serv=8 " +
                "   and 0 < ( Select count(*) From t_norm_otopl k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar and k.vid_alg in (0,1,2,3,4,5) ) ";
            BillingInstrumentary.ExecSQL(sql);

        }
        #endregion N: отопление

        #region N: электроотопление

        private void CalcRashodNormElOtopl()
        //--------------------------------------------------------------------------------
        {
            //----------------------------------------------------------------
            //N: электроотопление
            //----------------------------------------------------------------
            TempTablesLifeTime.AddTempTable("t_norm_eeot",
                " create temp table t_norm_eeot ( " +
                //" create table are.t_norm_otopl ( " +
                " nzp_dom       integer, " +
                " nzp_kvar      integer, " +
                " s_otopl       NUMERIC( 8,2) default 0, " + // отапливаемая площадь
                " rashod_kvt_m2 NUMERIC(12,8) default 0, " + // нормативный расход в квт*час на 1 м2
                // для расчета норматива по отоплению
                " nzp_res0      integer default 0,      " + // таблица нормативов
                " dom_klimatz   integer default 0,      " + // Климатическая зона
                " dom_kol_etag  integer default 0,       " + // количество этажей дома (этажность)
                " pos_etag      integer default 0        " +
                // позиция по количеству этажей дома в таблице удельных расходов тепловой энергии
                " ) "
                );


            // === перечень л/с для расчета нормативов ===
            BillingInstrumentary.ExecSQL(
                " insert into t_norm_eeot (nzp_dom,nzp_kvar,s_otopl)" +
                " select nzp_dom,nzp_kvar,max(squ2) from ttt_counters_xx " +
                " where nzp_serv=322 " +
                " group by 1,2 "
                );
            BillingInstrumentary.ExecSQL(" create index ix1_norm_eeot on t_norm_eeot (nzp_kvar) ");

            // поиск указанного норматива в Квт*час на 1 кв. метр. вид методики расчета норматива = 0
            // домовые
            BillingInstrumentary.ExecSQL(
                " update t_norm_eeot set rashod_kvt_m2= t.val_prm::numeric " +
                " from ttt_prm_2 t where t.nzp = t_norm_eeot.nzp_dom and nzp_prm = 1479"
                );

            // квартирные
            BillingInstrumentary.ExecSQL(
                " update t_norm_eeot set rashod_kvt_m2=t.val_prm::numeric " +
                "  from ttt_prm_1 t where t.nzp = t_norm_eeot.nzp_kvar and nzp_prm = 1480"
                );


            BillingInstrumentary.ExecSQL(
                " update t_norm_eeot set rashod_kvt_m2=0 where rashod_kvt_m2 is null  "
                );


            // ===  установка норматива по электроотоплению в counters_xx ===
            BillingInstrumentary.ExecSQL(
                " Update ttt_counters_xx " +
                " Set (val1,val3) = " +
                "((Select rashod_kvt_m2 * s_otopl From t_norm_eeot k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar)," +
                " (Select rashod_kvt_m2 From t_norm_eeot k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar)) " +
                " Where nzp_serv=322 " +
                "   and EXISTS ( Select 1 From t_norm_eeot k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar ) "
                );
        }

        #endregion N: электроотопление

        #region N: газовое отопление
        private void CalcRashodNormGasOtopl(DbCalcCharge.Rashod rashod)
        //--------------------------------------------------------------------------------
        {
            //----------------------------------------------------------------
            //N: газовое отопление
            //----------------------------------------------------------------
            TempTablesLifeTime.AddTempTable("t_norm_gasot",
                " create temp table t_norm_gasot ( " +
                " nzp_dom       integer, " +
                " nzp_kvar      integer, " +
                " s_otopl       NUMERIC( 8,2) default 0, " +    // отапливаемая площадь
                " rashod_kbm_m2 NUMERIC(12,8) default 0  " +    // нормативный расход в куб.м газа на 1 м2
                " )  "
                );


            // === перечень л/с для расчета нормативов ===
            BillingInstrumentary.ExecSQL(
                  " insert into t_norm_gasot (nzp_dom,nzp_kvar,s_otopl)" +
                  " select nzp_dom,nzp_kvar,max(squ2) from ttt_counters_xx " +
                  " where nzp_serv=325 " +
                  " group by 1,2 "
                  );


            BillingInstrumentary.ExecSQL(" create index ix1_norm_gasot on t_norm_gasot (nzp_kvar) ");

            BillingInstrumentary.ExecSQL(
                  " update t_norm_gasot set rashod_kbm_m2=" +
                  " (Select max(val_prm::numeric)" +
                   " From " + rashod.paramcalc.data_alias + "prm_5 " +
                   " Where nzp_prm = 169 and is_actual <> 100" +
                   " and dat_s  <= " + rashod.paramcalc.dat_po + " and dat_po >= " + rashod.paramcalc.dat_s + " ) "
                  );


            BillingInstrumentary.ExecSQL(
                  " update t_norm_gasot set rashod_kbm_m2=0 Where rashod_kbm_m2 is null "
                  );


            // ===  установка норматива по газовому отоплению в counters_xx ===
            BillingInstrumentary.ExecSQL(
                " Update ttt_counters_xx " +
                " Set (val1,val3) = " +
                "((Select rashod_kbm_m2 * s_otopl From t_norm_gasot k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar)," +
                " (Select rashod_kbm_m2 From t_norm_gasot k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar)) " +
                " Where nzp_serv=325 " +
                "   and 0 < ( Select count(*) From t_norm_gasot k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar ) "
                );

        }
        #endregion N: газовое отопление

        #region N: газ
        private void CalcRashodNormGas(DbCalcCharge.Rashod rashod)
        {
            //----------------------------------------------------------------
            //N: газ 
            //----------------------------------------------------------------

            TempTablesLifeTime.AddTempTable("t_norm_gas",
                " create temp table t_norm_gas ( " +
                " nzp_dom    integer, " +
                " nzp_kvar   integer, " +
                " rashod_kbm NUMERIC(12,8) default 0, " +    // нормативный расход в куб.м газа на 1 человека
                " nzp_res0   integer default 0,       " +    // таблица нормативов
                " is_gp      integer default 0,      " +   // Климатическая зона
                " is_gvs     integer default 0,      " +   // количество этажей дома (этажность)
                " is_gk      integer default 0       " +   // позиция по количеству этажей дома в таблице удельных расходов тепловой энергии
                " ) "
                );


            // === перечень л/с для расчета нормативов ===
            BillingInstrumentary.ExecSQL(
                " insert into t_norm_gas (nzp_dom,nzp_kvar)" +
                " select nzp_dom,nzp_kvar from ttt_counters_xx " +
                " where nzp_serv=10 " +
                " group by 1,2 "
                );


            BillingInstrumentary.ExecSQL(" create index ix1_norm_gas on t_norm_gas (nzp_kvar) ");

            // поиск указанного норматива в куб.м на 1 человека

            // нормативы
            var iNzpRes = LoadIntValPrmForNorm(rashod.paramcalc.data_alias, 173, "13", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);
            if (iNzpRes != 0)
            {
                BillingInstrumentary.ExecSQL(
                      " Update t_norm_gas " +
                      " Set nzp_res0 = " + iNzpRes + " Where 1=1 "
                    );

            }
            // наличие газовой плиты
            BillingInstrumentary.ExecSQL(
                  " update t_norm_gas set is_gp=1" +
                  " where exists( select 1 from ttt_prm_1 p where t_norm_gas.nzp_kvar=p.nzp and p.nzp_prm=551 ) "
                );


            // наличие газовой колонки (водонагревателя)
            BillingInstrumentary.ExecSQL(
                  " update t_norm_gas set is_gk=1" +
                  " where exists( select 1 from ttt_prm_1 p where t_norm_gas.nzp_kvar=p.nzp and p.nzp_prm=1 ) "
                );


            // наличие ГВС
            var sql =
                " update t_norm_gas set is_gvs=1" +
                " where exists( select 1 from ttt_prm_1 p where t_norm_gas.nzp_kvar=p.nzp and p.nzp_prm=7 " +
                " and p.val_prm::int in (";
            if (Points.IsSmr)
            {
                sql = sql.Trim() + "10";
            }
            else
            {
                sql = sql.Trim() + "05,07,08,09,14,15,16,17";
            }
            sql = sql.Trim() + ") ) ";

            BillingInstrumentary.ExecSQL(sql);


            BillingInstrumentary.ExecSQL(
                " Update t_norm_gas Set " +
                "  rashod_kbm = ( Select max(r2.value::numeric)" +
                " From " + rashod.paramcalc.kernel_alias + "res_values r1, " + rashod.paramcalc.kernel_alias + "res_values r2 " +
                    "  Where r1.nzp_res = nzp_res0 and r2.nzp_res = nzp_res0 " +
                    "   and r1.nzp_x = 1 and r2.nzp_x = 2 and r1.nzp_y=r2.nzp_y " +
                    "   and trim(r1.value) = (" +
                        " (case when is_gp =1 then '1' else '0' end) ||" +
                        " (case when is_gvs=1 then '1' else '0' end) ||" +
                        " (case when is_gk =1 then '1' else '0' end)" +
                    ") " +
                    " ) " +
                " Where (is_gp > 0 or is_gk>0) " +
                "   and nzp_res0 in ( Select nzp_res From " + rashod.paramcalc.kernel_alias + "res_values ) "
                );


            BillingInstrumentary.ExecSQL(
                " Update t_norm_gas Set rashod_kbm = 0 Where rashod_kbm is null "
                );

            sql =
                " UPDATE ttt_counters_xx " +
                " SET val1   = n.rashod_kbm * gil1   * (cntd * 1.00 / cntd_mn), " +
                "     val1_g = n.rashod_kbm * gil1_g * (cntd * 1.00 / cntd_mn), " +
                "     val3          = n.rashod_kbm, " +
                "     rash_norm_one = n.rashod_kbm " +
                " FROM t_norm_gas n " +
                " WHERE ttt_counters_xx.nzp_kvar = n.nzp_kvar AND ttt_counters_xx.nzp_serv = 10 ";
            BillingInstrumentary.ExecSQL(sql);

        }
        #endregion N: газ

        #region N: полив
        private void CalcRashodNormPoliv(DbCalcCharge.Rashod rashod)
        //--------------------------------------------------------------------------------
        {
            //----------------------------------------------------------------
            //N: полив
            //----------------------------------------------------------------
            TempTablesLifeTime.DropTempTable("ttt_aid_c1 ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_c1",
                " Create temp table ttt_aid_c1 " +
                " ( nzp_kvar integer, " +
                "   ival1 integer default 0, " +
                "   rval1  NUMERIC(12,4) default 0.00," +
                "   rval2  NUMERIC(12,4) default 0.00," +
                "   rval3  NUMERIC(12,4) default 0.00," +
                "   rval4  NUMERIC(12,4) default 0.00 " +
                " )  "
                );


            // для РТ нормативный расход полива = объем на сотку * кол-во соток
            var sNorm200 =
            ",1 as ival1 " +
            ",max(case when nzp_prm=262 then COALESCE(p.val_prm,'0')::numeric else 0 end) as rval1 " +
            ",max(case when nzp_prm=390 then COALESCE(p.val_prm,'0')::numeric else 0 end) as rval2 " +
                // для садов в Туле!
            ",max(case when nzp_prm=2466 then COALESCE(p.val_prm,'0')::numeric else 0 end) as rval3 " +
            ",max(case when nzp_prm=2467 then COALESCE(p.val_prm,'0')::numeric else 0 end) as rval4 ";

            var sFlds200 = "262,390,2466,2467";
            if (Points.IsSmr)
            {
                // для Самары нормативный расход полива = кол-во поливок * площадь полива в кв.м * объем на 1 кв.м
                sNorm200 =
                ",max(case when nzp_prm=2044 then COALESCE(p.val_prm,'0')::numeric else 0 end) as ival1 " +
                ",max(case when nzp_prm=2011 then COALESCE(p.val_prm,'0')::numeric else 0 end) as rval1 " +
                ",max(case when nzp_prm=2043 then COALESCE(p.val_prm,'0')::numeric else 0 end) as rval2 " +
                ",0 as rval3 " +
                ",0 as rval4 ";
                sFlds200 = "2011,2043,2044";
            }

            BillingInstrumentary.ExecSQL(
                  " insert into ttt_aid_c1 (nzp_kvar, ival1, rval1, rval2, rval3, rval4) " +
                  " Select nzp as nzp_kvar" + sNorm200 +
                  " From ttt_counters_xx a, ttt_prm_1 p " +
                  " Where a.nzp_kvar = p.nzp " +
                  "   and a.nzp_serv=200 " +
                  "   and p.nzp_prm in (" + sFlds200 + ") " +
                  " Group by 1 "
                );


            BillingInstrumentary.ExecSQL(" Create unique index ix_ttt_aid_c1 on ttt_aid_c1 (nzp_kvar) ");

            BillingInstrumentary.ExecSQL(
                " Update ttt_counters_xx " +
                " Set val1 =" +
                " ( Select k.ival1 * (k.rval1 * k.rval2 + k.rval3 * k.rval4)" +
                  " From ttt_aid_c1 k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar ) " +
                " , rash_norm_one =" +
                " ( Select k.ival1 * (k.rval2 + k.rval4)" +
                  " From ttt_aid_c1 k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar ) " +
                " Where nzp_serv=200 " +
                "   and 0 < ( Select count(*)" +
                  " From ttt_aid_c1 k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar ) "
                );

        }
        #endregion N: полив

        #region N: вода для бани
        private void CalcRashodNormHVforBanja()
        //--------------------------------------------------------------------------------
        {
            //----------------------------------------------------------------
            //N: вода для бани
            //----------------------------------------------------------------
            TempTablesLifeTime.DropTempTable("ttt_aid_c1 ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_c1",
                " Create temp table ttt_aid_c1 " +
                " ( nzp_kvar integer, " +
                "   gil1   NUMERIC(12,4) default 0.00," +
                "   gil1_g NUMERIC(12,4) default 0.00," +
                "   dat_s date not null," +
                "   dat_po date not null," +
                "   norm   NUMERIC(12,4) default 0.00 " +
                " )  "
                );


            // для РТ нормативный расход вода для бани = объем на Кж * Норма на 1 чел.
            var sFlds200 = "268";
            BillingInstrumentary.ExecSQL(
                  " insert into ttt_aid_c1 (nzp_kvar, gil1, gil1_g,dat_s,dat_po, norm) " +
                  " Select nzp as nzp_kvar, gil1, gil1_g, a.dat_s,a.dat_po, max(COALESCE(p.val_prm,'0')::numeric) " +
                  " From ttt_counters_xx a, ttt_prm_1 p " +
                  " Where a.nzp_kvar = p.nzp " +
                  "   and a.nzp_serv=203 " +
                  "   and p.nzp_prm in (" + sFlds200 + ") " +
                  "   and p.dat_s<=a.dat_po and p.dat_po>=a.dat_s" +
                  " Group by 1,2,3,4,5 "
                );


            BillingInstrumentary.ExecSQL(" Create index ix_ttt_aid_c1 on ttt_aid_c1 (nzp_kvar) ");

            BillingInstrumentary.ExecSQL(
                 " Update ttt_counters_xx t " +
                 " Set" +
                   " cnt1 = " + sFlds200 +
                   ",val1 =  k.gil1 * k.norm " +
                   ",val1_g =  k.gil1_g * k.norm " +
                   ",rash_norm_one =  k.norm " +
                 " From ttt_aid_c1 k " +
                 " Where t.nzp_kvar = k.nzp_kvar " +
                 " and t.nzp_serv=203 " +
                 " and t.dat_s=k.dat_s and t.dat_po=k.dat_po"
                );

        }
        #endregion N: вода для бани

        #region N: питьевая вода
        private void CalcRashodNormPitHV()
        //--------------------------------------------------------------------------------
        {
            //----------------------------------------------------------------
            //N: питьевая вода
            //----------------------------------------------------------------
            TempTablesLifeTime.DropTempTable("ttt_aid_c1 ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_c1",
                " Create temp table ttt_aid_c1 " +
                " ( nzp_kvar integer, " +
                "   ival1 integer default 0, " +
                "   rval1  NUMERIC(12,4) default 0.00," +
                "   rval1_g NUMERIC(12,4) default 0.00," +
                "   rval2  NUMERIC(12,4) default 0.00 " +
                " ) "
                );


            // для РТ нормативный расход пит.воды = кол-во жильцов * кол-во литров (норма на дом)
            BillingInstrumentary.ExecSQL(
                " insert into ttt_aid_c1 (nzp_kvar, ival1, rval1, rval1_g, rval2) " +
                " Select nzp_kvar,max(cnt1),max(gil1),max(gil1_g),max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric )" +
                " From ttt_counters_xx a " +
                " left outer join ttt_prm_2 p on a.nzp_dom=p.nzp and p.nzp_prm=705 " +
                " Where 1=1 " +
                " and a.nzp_serv=253 " +
                " Group by 1 ");


            BillingInstrumentary.ExecSQL(" Create unique index ix_ttt_aid_c1 on ttt_aid_c1 (nzp_kvar) ");

            BillingInstrumentary.ExecSQL(
                " Update ttt_counters_xx " +
                " Set (val1,val1_g,rash_norm_one) =" +
                " (( Select k.rval1 * k.rval2" +
                   " From ttt_aid_c1 k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar )," +
                "  ( Select k.rval1_g * k.rval2" +
                   " From ttt_aid_c1 k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar )," +
                "  ( Select k.rval2" +
                   " From ttt_aid_c1 k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar )) " +
                " Where nzp_serv=253 " +
                "   and 0 < ( Select count(*) From ttt_aid_c1 k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar ) "
                );

        }
        #endregion N: питьевая вода


        /// <summary>
        /// создание ttt_counters_xx - структуры для получения нормативов
        /// </summary>
        /// <param name="rashod"></param>
        public string CreateTempNormatives(DbCalcCharge.Rashod rashod, string TargetTable)
        {

            var TableNormatives = "ttt_counters_xx";
            TempTablesLifeTime.AddTempTable(TableNormatives, " Create Temp table " + TableNormatives +
                  " (  nzp_cntx    serial        not null, " +
                  "    nzp_dom     integer       not null, " +
                  "    nzp_kvar    integer       default 0 not null , " +
                  "    nzp_type    integer       not null, " +               //1,2,3
                  "    nzp_serv    integer       not null, " +
                  "    dat_charge  date, " +
                  "    cur_zap     integer       default 0 not null, " +     //0-текущее значение, >0 - ссылка на следующее значение (nzp_cntx)
                  "    nzp_counter integer       default 0 , " +             //счетчик или вариатны расходов для stek=3
                  "    cnt_stage   integer       default 0 , " +             //разрядность
                  "    mmnog       NUMERIC(15,7) default 1.00 , " +             //масшт. множитель
                  "    stek        integer       default 0 not null, " +     //3-итого по лс,дому; 1-счетчик; 2,3,4,5 - стек расходов
                  "    rashod      NUMERIC(15,7) default 0.00 not null, " +  //общий расход в зависимости от stek
                  "    dat_s       date          not null, " +               //"дата с" - для ПУ, для по-дневного расчета период в месяце (dp)
                  "    val_s       NUMERIC(15,7) default 0.00 not null, " +  //значение (а также коэф-т)
                  "    dat_po      date not null, " +                        //"дата по"- для ПУ, для по-дневного расчета период в месяце (dp_end)
                  "    val_po      NUMERIC(15,7) default 0.00 not null, " +  //значение
                  "    ngp_cnt       NUMERIC(14,7) default 0.0000000, " +  // расход на нежилые помещения
                  "    rash_norm_one NUMERIC(14,7) default 0.0000000, " +  // норматив на 1 человека
                  "    val1_g      NUMERIC(15,7) default 0.00 not null, " +  //расход по счетчику nzp_counter или нормативные расходы в расчетном месяце без учета вр.выбывших
                  "    val1        NUMERIC(15,7) default 0.00 not null, " +  //расход по счетчику nzp_counter или нормативные расходы в расчетном месяце
                  "    val2        NUMERIC(15,7) default 0.00 not null, " +  //дом: расход КПУ
                  "    val3        NUMERIC(15,7) default 0.00         , " +  //дом: расход нормативщики
                  "    val4        NUMERIC(15,7) default 0.00         , " +  //общий расход по счетчику nzp_counter
                  "    rvirt       NUMERIC(15,7) default 0.00         , " +  //вирт. расход
                  "    squ1        NUMERIC(15,7) default 0.00         , " +  //площадь лс, дома (по всем лс)
                  "    squ2        NUMERIC(15,7) default 0.00         , " +  //площадь лс без КПУ (для домовых строк)
                  "    nzp_prm_squ2 integer default 133, " +                              // по умолчанию как squ2 берется nzp_prm=133 отапливаемая площадь
                  "    cls1        integer       default 0 not null   , " +  //количество лс дома по услуге
                  "    cls2        integer       default 0 not null   , " +  //количество лс без КПУ (для домовых строк)
                  "    gil1_g      NUMERIC(15,7) default 0.00         , " +  //кол-во жильцов в лс без учета вр.выбывших
                  "    gil1        NUMERIC(15,7) default 0.00         , " +  //кол-во жильцов в лс
                  "    gil2        NUMERIC(15,7) default 0.00         , " +  //кол-во жильцов в лс
                  "    cnt1_g      integer       default 0 not null, " +     //кол-во жильцов в лс (нормативное) без учета вр.выбывших
                  "    cnt1        integer       default 0 not null, " +     //кол-во жильцов в лс (нормативное)
                  "    cnt2        integer       default 0 not null, " +     //кол-во комнат в лс
                  "    cnt3        integer       default 0, " +              //тип норматива в зависимости от услуги (ссылка на resolution.nzp_res)
                  "    cnt4        integer       default 0, " +              //1-дом не-МКД (0-МКД)
                  "    cnt5        integer       default 0, " +              //резерв
                  "    dop87       NUMERIC(15,7) default 0.00         , " +  //доп.значение 87 постановления (7кВт или добавок к нормативу  (87 П) )
                  "    pu7kw       NUMERIC(15,7) default 0.00         , " +  //7 кВт для КПУ (откорректированный множитель)
                  "    gl7kw       NUMERIC(15,7) default 0.00         , " +  //7 кВт КПУ * gil1 (учитывая корректировку)
                  "    vl210       NUMERIC(15,7) default 0.00         , " +  //расход 210 для nzp_type = 6
                  "    kf307       NUMERIC(15,7) default 0.00         , " +  //коэфициент 307 для КПУ или коэфициент 87 для нормативщиков
                  "    kf307n      NUMERIC(15,7) default 0.00         , " +  //коэфициент 307 для нормативщиков
                  "    kf307f9     NUMERIC(15,7) default 0.00         , " +  //коэфициент 307 по формуле 9
                  "    kf_dpu_kg   NUMERIC(15,7) default 0.00         , " +  //коэфициент ДПУ для распределения пропорционально кол-ву жильцов
                  "    kf_dpu_plob NUMERIC(15,7) default 0.00         , " +  //коэфициент ДПУ для распределения пропорционально сумме общих площадей
                  "    kf_dpu_plot NUMERIC(15,7) default 0.00         , " +  //коэфициент ДПУ для распределения пропорционально сумме отапливаемых площадей
                  "    kf_dpu_ls   NUMERIC(15,7) default 0.00         , " +  //коэфициент ДПУ для распределения пропорционально кол-ву л/с
                  "    dlt_in      NUMERIC(15,7) default 0.00         , " +  //входящии нераспределенный расход (остаток)
                  "    dlt_cur     NUMERIC(15,7) default 0.00         , " +  //текущая дельта
                  "    dlt_reval   NUMERIC(15,7) default 0.00         , " +  //перерасчет дельты за прошлые месяцы
                  "    dlt_real_charge NUMERIC(15,7) default 0.00     , " +  //перерасчет дельты за прошлые месяцы
                  "    dlt_calc    NUMERIC(15,7) default 0.00         , " +  //распределенный (учтенный) расход
                  "    dlt_out     NUMERIC(15,7) default 0.00         , " +  //исходящии нераспределенный расход (остаток)
                  "    kod_info    integer default 0," +
                  "    sqgil       NUMERIC(15,7) default 0.00         ," +  //жилая площадь лс
                  "    is_day_calc integer not null, " +
                  "    is_use_knp integer default 0, " +
                  "    is_use_ctr integer default 0," +//Количество временно выбывших
                  "    nzp_period  integer not null, " +
                  "    cntd integer," +
                  "    cntd_mn integer, " +
                  "    nzp_measure integer, " + // ед.измерения 
                  "    norm_type_id integer, " + // id типа норматива - для нового режима введения нормативов
                  "    norm_tables_id integer, " + // id норматива - по нему можно получить набор влияющих пар-в и их знач.
                  "    val1_source NUMERIC(15,7) default 0.00 not null, " +  //val1 без учета повышающего коэффициента
                  "    val4_source NUMERIC(15,7) default 0.00 not null, " +  //val4 без учета повышающего коэффициента
                  "    up_kf NUMERIC(15,7) default 1.00 not null " +   //повышающий коэффициент для нормативного расхода
                  " ) ");

            // только открытые лс и выборка услуг
            TempTablesLifeTime.AddTempTable("ttt_cnt_uni", " Create temp table ttt_cnt_uni " +
                " ( nzp_kvar integer," +
                "   nzp_dom  integer," +
                "   nzp_serv integer, " +
                "   nzp_measure integer, " +
                "   is_day_calc integer not null," +
                "   is_use_knp integer default 0, " +
                "   is_use_ctr integer default 0" +//Количество временно выбывших
                " )  ");

            // только открытые лс и выборка услуг
            BillingInstrumentary.ExecSQL(" Insert into ttt_cnt_uni (nzp_kvar,nzp_dom,nzp_serv,nzp_measure,is_day_calc,is_use_knp,is_use_ctr) " +
                " Select k.nzp_kvar, k.nzp_dom, t.nzp_serv,nzp_measure,max(k.is_day_calc),0 as is_use_knp,1 as is_use_ctr" +
                " From " + TargetTable + " t, t_opn k, " + rashod.paramcalc.kernel_alias + "s_counts s " +
                " Where  " +
                "   k.nzp_kvar = t.nzp_kvar and t.nzp_serv=s.nzp_serv " +
                "   and t." + rashod.where_dom + rashod.where_kvarK +
                " Group by 1,2,3,4 "
                , true);


            BillingInstrumentary.ExecSQL(" Create index ix_ttt_cnt_uni on ttt_cnt_uni (nzp_kvar) ", true);


            // только открытые лс и выборка услуг
            BillingInstrumentary.ExecSQL(
                " Insert into " + TableNormatives +
                " (nzp_kvar,nzp_dom,nzp_serv,is_day_calc,nzp_period,dat_s,dat_po,cntd,cntd_mn, stek,nzp_type,mmnog,dat_charge,nzp_measure,is_use_knp,is_use_ctr) " +
                " Select t.nzp_kvar, t.nzp_dom, t.nzp_serv, t.is_day_calc," +
                " k.nzp_period,k.dp,k.dp_end,k.cntd,k.cntd_mn," +
                " 10,3,-100, null::date as dat_charge,t.nzp_measure,t.is_use_knp,t.is_use_ctr" +
                " From ttt_cnt_uni t,t_gku_periods k " +
                " Where t.nzp_kvar = k.nzp_kvar "
                , true);

            BillingInstrumentary.ExecSQL(" Create unique index ix_aid00_sq on " + TableNormatives + " (nzp_cntx) ", true);
            BillingInstrumentary.ExecSQL(" Create        index ix_aid11_sq on " + TableNormatives + " (nzp_kvar,nzp_serv,kod_info) ", true);
            BillingInstrumentary.ExecSQL(" Create        index ix_aid22_sq on " + TableNormatives + " (nzp_dom,nzp_serv) ", true);
            BillingInstrumentary.ExecSQL(" Create        index ix_aid33_sq on " + TableNormatives + " (nzp_serv,cnt2) ", true);
            BillingInstrumentary.ExecSQL(" Create        index ix_aid44_sq on " + TableNormatives + " (nzp_kvar,nzp_serv,dat_s,dat_po) ", true);
            BillingInstrumentary.ExecSQL(" Create        index ix_aid55_sq on " + TableNormatives + " (nzp_kvar,nzp_serv,stek,cnt_stage) ", true);
            BillingInstrumentary.ExecSQL(" Create        index ix_aid66_sq on " + TableNormatives + " (nzp_kvar,nzp_prm_squ2) ", true);

            TempTablesLifeTime.DropTempTable("ttt_cnt_uni ", false);

            return TableNormatives;
        }

        /// <summary>
        /// Получить целочисленное значения параметра для нормативов
        /// </summary>
        /// <param name="conn_db"></param>
        /// <param name="pDataAls"></param>
        /// <param name="pNzpPrm"></param>
        /// <param name="pNumPrm"></param>
        /// <param name="pDatS"></param>
        /// <param name="pDatPo"></param>
        /// <returns></returns>
        public int LoadIntValPrmForNorm(string pDataAls, int pNzpPrm, string pNumPrm, string pDatS, string pDatPo)
        //--------------------------------------------------------------------------------
        {
            var sql = " SELECT MAX(val_prm::int) val_prm FROM " + pDataAls + "prm_" + pNumPrm.Trim() +
                      " WHERE nzp_prm = " + pNzpPrm + " AND is_actual <> 100 AND dat_s  <= " + pDatPo +
                      " AND dat_po >= " + pDatS + " ";
            return BillingInstrumentary.ExecScalar<int>(sql);
        }


        /// <summary>
        /// Проверка наличия логического параметра в нужной базе, нужной таблице prm_5/10, по дате, значению параметра
        /// </summary>
        /// <param name="conn_db"></param>
        /// <param name="pDataAls"></param>
        /// <param name="pNzpPrm"></param>
        /// <param name="pNumPrm"></param>
        /// <param name="pValPrm"></param>
        /// <param name="pDatS"></param>
        /// <param name="pDatPo"></param>
        /// <returns></returns>
        public bool CheckBoolValPrmWithVal(string pDataAls, int pNzpPrm, string pNumPrm, string pValPrm, string pDatS, string pDatPo)
        //--------------------------------------------------------------------------------
        {
            var sql = " SELECT COUNT(1)>0 cnt FROM " + pDataAls + "prm_" + pNumPrm.Trim() + " p " +
                      " WHERE p.nzp_prm = " + pNzpPrm + " AND p.val_prm='" + pValPrm.Trim() + "' " +
                      " AND p.is_actual <> 100 AND p.dat_s  <= " + pDatPo + " AND p.dat_po >= " + pDatS + " ";
            return BillingInstrumentary.ExecScalar<bool>(sql);
        }

    }

    public class MONormativeHandlerOverride : INormative
    {
        private MONormativeHandlerOverride() { }
        protected IDbConnection Connection;
        protected BillingInstrumentary BillingInstrumentary;
        protected IWindsorContainer Container;
        protected CalcVerificationParams CalcParams;
        protected TempTablesLifeTime TempTablesLifeTime;
        public MONormativeHandlerOverride(IDbConnection connection, IWindsorContainer container, BillingInstrumentary billingInstrumentary)
        {
            Connection = connection;
            Container = container;
            BillingInstrumentary = billingInstrumentary;
            CalcParams = Container.Resolve<CalcVerificationParams>();
            TempTablesLifeTime = Container.Resolve<TempTablesLifeTime>();
        }
        /// <summary>
        /// Подсчет нормативов на услуги ЖКУ по старому алгоритму в 5.0
        /// </summary>
        /// <param name="param"></param>
        /// <param name="TargetTable"></param>
        /// <returns></returns>
        public IDataResult ApplyNormatives(string TargetTable)
        {
            var rashod = new DbCalcCharge.Rashod(CalcParams.ParamCalc);

            //cтруктура для получения 
            var TableNormatives = CreateTempNormatives(rashod, TargetTable);
            //N: электроснабжение
            CalcNormElEn(rashod);
            //N: хвс гвс канализация
            CalcRashodNormHVandKAN(rashod, TargetTable);
            //N: газ 
            CalcRashodNormGas(rashod);
            //N: электроотопление
            CalcRashodNormElOtopl();
            //N: газовое отопление
            CalcRashodNormGasOtopl(rashod);
            //N: полив
            CalcRashodNormPoliv(rashod);
            //N: отопление
            CalcRashodNormOtopl(rashod);
            //N: вода для бани
            CalcRashodNormHVforBanja();
            //N: питьевая вода
            CalcRashodNormPitHV();
            //применить полученные нормативы УК
            ApplyMONorms(TargetTable, TableNormatives);

            return new BaseDataResult();
        }

        /// <summary>
        /// применяем полученные нормативы УК
        /// </summary>
        /// <param name="TargetTable"></param>
        /// <param name="TableNormatives"></param>
        private void ApplyMONorms(string TargetTable, string TableNormatives)
        {
            //применяем полученные нормативы
            var sql = " UPDATE " + TargetTable + " t " +
                      " SET rash_norm_one=n.rash_norm_one, rash_norm_one_chd=n.rash_norm_one" +
                      " FROM  " + TableNormatives + " n " +
                      " WHERE n.nzp_kvar=t.nzp_kvar " +
                      " AND n.nzp_serv=t.nzp_serv";
            BillingInstrumentary.ExecSQL(sql);
        }

        /// <summary>
        /// Нормативы по электро-энергии
        /// </summary>
        /// <param name="rashod"></param>
        /// <param name="b_calc_kvar"></param>
        /// <returns></returns>
        private void CalcNormElEn(DbCalcCharge.Rashod rashod)
        {
            var iNzpPrmEE = 730;
            if ((new DateTime(rashod.paramcalc.calc_yy, rashod.paramcalc.calc_mm, 01)) >= (new DateTime(2012, 09, 01)))
            {
                iNzpPrmEE = 1079;
            }

            BillingInstrumentary.ExecSQL(
                " Update ttt_counters_xx " +
                " Set cnt4 = " + iNzpPrmEE + //дом не-МКД (по всем нормативным строкам лс и домам)
                " Where nzp_serv in (25,210) " +
                "   and 0 < (" +
                          " Select count(*) From ttt_prm_2 p " +
                          " Where ttt_counters_xx.nzp_dom = p.nzp " +
                          "   and p.nzp_prm = " + iNzpPrmEE +
                          " ) ");


            TempTablesLifeTime.AddTempTable("ttt_aid_c1", " create temp table ttt_aid_c1 (nzp_kvar integer) ");


            BillingInstrumentary.ExecSQL(
                  " Insert into ttt_aid_c1 (nzp_kvar)" +
                  " Select nzp as nzp_kvar " +
                  " From ttt_counters_xx a, ttt_prm_1 p " +
                  " Where a.nzp_kvar = p.nzp " +
                  "   and a.nzp_serv in (25,210) " +
                  "   and p.nzp_prm = 19 " + // электроплита
                  " Group by 1 "
                );

            TempTablesLifeTime.AddTempTable("ttt_aid_c1x",
                " create temp table ttt_aid_c1x (nzp_kvar integer) "
                );


            BillingInstrumentary.ExecSQL(
                " Insert into ttt_aid_c1x (nzp_kvar)" +
                " Select nzp_kvar From ttt_aid_c1 "
                );


            BillingInstrumentary.ExecSQL(" Create unique index ix_ttt_aid_c1x on ttt_aid_c1x (nzp_kvar) ");


            BillingInstrumentary.ExecSQL(
                  " Insert into ttt_aid_c1 (nzp_kvar)" +
                  " Select a.nzp_kvar " +
                  " From ttt_counters_xx a, ttt_prm_2 p " +
                  " Where a.nzp_dom = p.nzp " +
                  "   and a.nzp_serv in (25,210) " +
                  "   and p.nzp_prm = 28 " + // электроплита на дом
                  "   and 0 = ( Select count(*) From ttt_aid_c1x k Where a.nzp_kvar = k.nzp_kvar ) " +
                  " Group by 1 "
                );


            BillingInstrumentary.ExecSQL(" Create unique index ix_ttt_aid_c1 on ttt_aid_c1 (nzp_kvar) ");

            var iNzpResMKD = LoadIntValPrmForNorm(rashod.paramcalc.pref, 181, "13", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);
            var iNzpRes = LoadIntValPrmForNorm(rashod.paramcalc.pref, 183, "13", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);

            if ((iNzpRes != 0) && (iNzpResMKD != 0))
            {
                BillingInstrumentary.ExecSQL(
                    " Update ttt_counters_xx " +
                    " Set cnt3 = (case when cnt4 = " + iNzpPrmEE + " then " + iNzpRes + " else " + iNzpResMKD + " end) " + //лс с электроплитой (в зависимости от МКД)
                    " Where nzp_serv in (25,210) " +
                    "   and 0 < ( Select count(*) From ttt_aid_c1 k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar ) "
                    );

            }

            iNzpResMKD = LoadIntValPrmForNorm(rashod.paramcalc.pref, 180, "13", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);
            iNzpRes = LoadIntValPrmForNorm(rashod.paramcalc.pref, 182, "13", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);

            if ((iNzpRes != 0) && (iNzpResMKD != 0))
            {
                BillingInstrumentary.ExecSQL(
                    " Update ttt_counters_xx " +
                    " Set cnt3 = (case when cnt4 = " + iNzpPrmEE + " then " + iNzpRes + " else " + iNzpResMKD + " end) " + //значит остальные без электроплиты (где <> -2;-17)
                    " Where nzp_serv in (25,210) " +
                    "   and cnt3 = 0 "
                    );

            }

            var sKolGil = "cnt1";
            var sKolGil_g = "cnt1_g";
            if ((new DateTime(rashod.paramcalc.calc_yy, rashod.paramcalc.calc_mm, 01)) < (new DateTime(2012, 09, 01)))
            {
                sKolGil = "1"; sKolGil_g = "1";
            }

            #region применить спец.таблицу расходов ЭЭ в РТ - если стоит признак на базу (nzp_prm=163) и наличие ИПУ (nzp_prm=101)

            TempTablesLifeTime.DropTempTable("ttt_aid_c1x", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_c1x",
                " create temp table ttt_aid_c1x (nzp_kvar integer)"
                );


            // 101|Наличие ЛС счетчика эл/эн|1||bool||1||||
            BillingInstrumentary.ExecSQL(
                  " Insert into ttt_aid_c1x (nzp_kvar)" +
                  " Select a.nzp_kvar " +
                  " From ttt_counters_xx a, ttt_prm_1 p " +
                  " Where a.nzp_kvar = p.nzp " +
                  "   and a.nzp_serv in (25,210) " +
                  "   and p.nzp_prm = 101 " +

                  // 163|Отменить таблицу нормативов по эл/энергии Пост.№761|1||bool||5||||
                  "   and 0 < ( Select count(*) From " + rashod.paramcalc.pref + "prm_5 p5 " +
                       " where p5.nzp_prm=163 and p5.val_prm='1' and p5.is_actual<>100 " +
                       "   and p5.dat_s  <= " + rashod.paramcalc.dat_po + " and p5.dat_po >= " + rashod.paramcalc.dat_s +
                       "   ) Group by 1 ");


            BillingInstrumentary.ExecSQL(" Create unique index ix_ttt_aid_c1x on ttt_aid_c1x (nzp_kvar) ");

            string sql = " Update ttt_counters_xx " +
                         " Set cnt3 = 13 " +
                         " Where nzp_serv in (25,210) " +
                         "   and 0 < ( Select count(*) From ttt_aid_c1x k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar ) ";
            BillingInstrumentary.ExecSQL(sql);


            TempTablesLifeTime.DropTempTable("ttt_aid_c1x ", false);
            TempTablesLifeTime.DropTempTable("ttt_aid_c1xx ", false);

            #endregion применить спец.таблицу расходов ЭЭ в РТ - если стоит признак на базу (nzp_prm=163) и наличие ИПУ (nzp_prm=101)

            sql =
                    " Update ttt_counters_xx " +
                    " Set val1 = (COALESCE " +
                               "(( Select value From " + rashod.paramcalc.pref + "res_values " +
                                 " Where nzp_res = cnt3 " +
                                 "   and nzp_y = (case when cnt3 = 13" + //кол-во людей
                                                " then (case when cnt1 >11 then 11 else cnt1 end)" +
                                                " else (case when cnt1 > 6 then  6 else cnt1 end)" +
                                                " end) " +
                                 "   and nzp_x = (case when cnt3 = 13" + //кол-во комнат
                                                " then 1" +
                                                " else (case when cnt2 > 5 then  5 else cnt2 end)" +
                                                " end) " +
                                 " ),'0')::numeric) " +
                                 " * (case when cnt3 = 13 then 1 else " + sKolGil + " end)" +
                    " , rash_norm_one = COALESCE " +
                               "(( Select value From " + rashod.paramcalc.pref + "res_values " +
                                 " Where nzp_res = cnt3 " +
                                 "   and nzp_y = (case when cnt3 = 13" + //кол-во людей
                                                " then (case when cnt1 >11 then 11 else cnt1 end)" +
                                                " else (case when cnt1 > 6 then  6 else cnt1 end)" +
                                                " end) " +
                                 "   and nzp_x = (case when cnt3 = 13" + //кол-во комнат
                                                " then 1" +
                                                " else (case when cnt2 > 5 then  5 else cnt2 end)" +
                                                " end) " +
                                 " ),'0') ::numeric" +
                                 " / (case when cnt3 = 13 and " + sKolGil + " > 0 then " + sKolGil + " else 1 end)" +
                    " , val1_g = (COALESCE " +
                               "(( Select value From " + rashod.paramcalc.pref + "res_values " +
                                 " Where nzp_res = cnt3 " +
                                 "   and nzp_y = (case when cnt3 = 13" + //кол-во людей
                                                " then (case when cnt1_g >11 then 11 else cnt1_g end)" +
                                                " else (case when cnt1_g > 6 then  6 else cnt1_g end)" +
                                                " end) " +
                                 "   and nzp_x = (case when cnt3 = 13" + //кол-во комнат
                                                " then 1" +
                                                " else (case when cnt2 > 5 then  5 else cnt2 end)" +
                                                " end) " +
                                 " ),'0')::numeric) " +
                                 " * (case when cnt3 = 13 then 1 else " + sKolGil_g + " end)" +
                    " Where nzp_serv in (25,210) " +
                    "   and cnt1_g > 0 and cnt2 > 0 " +
                    "   and cnt3 in ( Select nzp_res From " + rashod.paramcalc.pref + "res_values ) ";
            BillingInstrumentary.ExecSQL(sql);

        }

        #region N: хвс гвс канализация
        private void CalcRashodNormHVandKAN(DbCalcCharge.Rashod rashod, string TargetTable)
        {
            //----------------------------------------------------------------
            //N: хвс гвс канализация
            //----------------------------------------------------------------

            var iNzpRes = LoadIntValPrmForNorm(rashod.paramcalc.pref, 172, "13", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);
            if (iNzpRes != 0)
            {
                BillingInstrumentary.ExecSQL(
                    " Update ttt_counters_xx " +
                    " Set cnt3 = " + iNzpRes +
                    " Where nzp_serv in (6,7,324) "
                    );

            }

            iNzpRes = LoadIntValPrmForNorm(rashod.paramcalc.pref, 177, "13", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);
            if (iNzpRes != 0)
            {
                BillingInstrumentary.ExecSQL(
                    " Update ttt_counters_xx " +
                    " Set cnt3 = " + iNzpRes +
                    " Where nzp_serv in (9,281,323) "
                    );

            }

            string sql;
            var sdn = "";

            #region  выбрать ЛС с расходом по КАН меньше суммы расхода ХВС+ГВС

            #endregion

            // выбрать КАН только по ХВ
            TempTablesLifeTime.AddTempTable("t_is339",
                " create temp table t_is339 (nzp_kvar integer) "
                );


            BillingInstrumentary.ExecSQL(
                " insert into t_is339 (nzp_kvar) " +
                " select nzp_kvar " +
                " from " + TargetTable + " " +
                " where nzp_serv in (7,324) and nzp_frm=339 " +
                " group by 1 "
                );


            BillingInstrumentary.ExecSQL(" Create index ix_t_is339 on t_is339 (nzp_kvar) ");

            // отметить КАН только по ХВ
            BillingInstrumentary.ExecSQL(
                " Update ttt_counters_xx " +
                " Set rvirt = 1" +
                " Where nzp_serv in (7,324) " +
                  " and 0<(select count(*) from t_is339 t where t.nzp_kvar=ttt_counters_xx.nzp_kvar )"
                );


            sql =
                " Update ttt_counters_xx Set " +
                // норма на 1 человека - ХВС, ГВС и КАН
                " rash_norm_one =" +

                " case when nzp_serv in (9,281,323) then " +

                // ... beg норматив на ГВС
                " (( Select COALESCE(value,'0') From " + rashod.paramcalc.pref + "res_values " +
                    "  Where nzp_res = cnt3 " +
                    "   and nzp_y = cnt2 " + //тип водоснабжения
                    "   and nzp_x = 2 " +
                    " ) ::numeric) " +
                    sdn +
                // ... end норматив на ГВС

                " else " +

                // ... beg норматив на ХВС и КАН
                " (( Select COALESCE(value,'0') From " + rashod.paramcalc.pref + "res_values " +
                    "  Where nzp_res = cnt3 " +
                    "   and nzp_y = cnt2 " + //тип водоснабжения
                    "   and nzp_x = (case when nzp_serv=6 then 1 else 3 end) " + // на нужды ХВ
                    " )::numeric) " +
                    sdn +

                " + case when nzp_serv=6 or rvirt = 1 then 0 else " +

                   "(( Select COALESCE(value,'0') From " + rashod.paramcalc.pref + "res_values " +
                       " Where nzp_res = cnt3 " +
                       "   and nzp_y = cnt2 " + //тип водоснабжения
                       "   and nzp_x = (case when nzp_serv=6 then 2 else 4 end) " + // на нужды ГВ
                       " )::numeric) " +
                       sdn +

                   " end " +
                // ... end норматив на ХВС и КАН

               " end " +

                // норма на 1 человека в части ХВС - ХВС и КАН
                ", val3 = " +
                // ... beg доля норматива на ХВС - только для ХВС и КАН - для ГВС нет
                " case when nzp_serv in (9,281,323) then 0 else " +
                " (( Select COALESCE(value,'0') From " + rashod.paramcalc.pref + "res_values " +
                    " Where nzp_res = cnt3 " +
                    "   and nzp_y = cnt2 " + //тип водоснабжения
                    "   and nzp_x = (case when nzp_serv=6 then 1 else 3 end) " + // на нужды ХВ
                    " )::numeric) " +
                    sdn + " " +
               " end " +
                // ... end доля норматива на ХВС - только для ХВС и КАН - для ГВС нет

               " Where nzp_serv in (6,7,324, 9,281,323) " +
               "   and cnt2 > 0 " +
               "   and cnt3 in ( Select nzp_res From " + rashod.paramcalc.pref + "res_values ) ";

            BillingInstrumentary.ExecSQL(sql);



            // нужды ХВ + нужды ГВ !!! для ХВ (и для КАН по ХВ -> rvirt = 1) расход ГВ НЕ добавлять !!!
            sql =
                " Update ttt_counters_xx Set " +
                " val1   = COALESCE(rash_norm_one * gil1,0) " +
                ",val1_g = COALESCE(rash_norm_one * gil1_g,0) " +
                ",val4   = COALESCE(val3 * gil1,0) " +
                " Where nzp_serv in (6,7,324, 9,281,323) " +
                "   and cnt2 > 0 ";
            BillingInstrumentary.ExecSQL(sql);

        }
        #endregion N: хвс гвс канализация

        #region N: отопление
        private void CalcRashodNormOtopl(DbCalcCharge.Rashod rashod)
        {
            //----------------------------------------------------------------
            //N: отопление
            //----------------------------------------------------------------
            var sql =
                " Update ttt_counters_xx " +
                " Set cnt3 = 61" + // норма по площади
                " Where nzp_serv in (8,322,325) ";
            BillingInstrumentary.ExecSQL(sql);


            var sKolGPl = "5";

            // норма по площади на 1 человека (Кж=cnt1)
            sql =
                " Update ttt_counters_xx " +
                " Set cnt2 = ( Select COALESCE(value,'0') From " + rashod.paramcalc.pref + "res_values " +
                             " Where nzp_res = cnt3 " +
                             "   and nzp_y = (case when cnt1 > " + sKolGPl + " then " + sKolGPl + " else cnt1 end) " + //кол-во людей
                             "   and nzp_x = 2 " + //
                             " )::int" +
                " Where nzp_serv in (8,322,325) " +
                "   and cnt1 > 0 " +
                "   and cnt3 in ( Select nzp_res From " + rashod.paramcalc.pref + "res_values ) ";
            BillingInstrumentary.ExecSQL(sql);


            //
            // ... расчет норматива на 1 кв.м ...
            //

            // признак отключения расчета норматива отопления
            var bCalcNormOtopl =
                !CheckBoolValPrmWithVal(rashod.paramcalc.pref, 478, "5", "1", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);

            TempTablesLifeTime.AddTempTable("t_norm_otopl",
                " create temp table t_norm_otopl ( " +
                " nzp_dom         integer, " +
                " nzp_kvar        integer, " +
                " s_otopl         NUMERIC( 8,2) default 0, " +    // отапливаемая площадь
                " rashod_gkal_m2f NUMERIC(12,8) default 0, " +    // расход в ГКал на 1 м2 фактический
                // вид учтенного расхода на л/с (1-норматив/2-ИПУ расход ГКал/3-дом.норма ГКал/4-ОДПУ расход ГКал)
                " vid_gkal_ls     integer default 0,       " +
                " rashod_gkal_dom NUMERIC(12,8) default 0, " +    // расход в ГКал на 1 м2
                " vid_gkal_dom    integer default 0,       " +    // вид учтенного домового расхода

                " rashod_gkal_m2  NUMERIC(12,8) default 0, " +    // нормативный расход в ГКал на 1 м2
                // для расчета норматива по отоплению
                " koef_god_pere   NUMERIC( 6,4) default 1, " +    // коэффициент перерасчета по итогам года
                " ugl_kv          integer default 0,       " +    // признак угловой квартиры (0-обычная/1-угловая)
                " vid_alg         integer default 1,       " +    // вид методики расчета норматива
                " tmpr_vnutr_vozd NUMERIC( 8,4) default 0, " +    // температура внутреннего воздуха (обычно = 20, для угловых = 22)
                " tmpr_vnesh_vozd NUMERIC( 8,4) default 0, " +    // средняя температура внешнего воздуха  (обычно = -5.7)
                " otopl_period     integer default 0,      " +    // продолжительность отопительного периода в сутках (обычно = 218)
                " nzp_res0         integer default 0,      " +    // таблица нормативов
                " dom_klimatz      integer default 0,      " +    // Климатическая зона
                // vid_alg=1 - памфиловская методика расчета норматива
                " dom_objem        NUMERIC(12,2) default 0, " +   // объем дома
                " dom_pol_pl       NUMERIC(12,2) default 1, " +   // полезная/отапливаемая площадь дома
                " dom_ud_otopl_har NUMERIC(12,8) default 1, " +   // удельная отопительная характеристика дома
                " dom_otopl_koef   NUMERIC( 8,4) default 1, " +   // поправочно-отопительный коэффициент для дома
                // vid_alg=2 - методика расчета норматива по Пост306 без интерполяции удельного расхода тепловой энергии
                // vid_alg=3 - методика расчета норматива по Пост306  с интерполяцией удельного расхода тепловой энергии
                // vid_alg=4 - методика расчета норматива - табличное значение от этажа и года постройки дома
                " dom_dat_postr    date default '1.1.1900', " +   // дата постройки дома
                " dom_kol_etag     integer default 0,       " +   // количество этажей дома (этажность)
                " pos_etag         integer default 0,       " +   // позиция по количеству этажей дома в таблице удельных расходов тепловой энергии
                " pos_narug_vozd   integer default 0,       " +   // позиция по температуре наружного воздуха в таблице удельных расходов тепловой энергии
                " dom_ud_tepl_en1  NUMERIC(12,8) default 0, " +   // минимальный  удельный расход тепловой энергии для дома по температуре и этажности
                " dom_ud_tepl_en2  NUMERIC(12,8) default 0, " +   // максимальный удельный расход тепловой энергии для дома по температуре и этажности
                " tmpr_narug_vozd1 NUMERIC( 8,4) default 0, " +   // минимально  близкая температура наружного воздуха в таблице
                " tmpr_narug_vozd2 NUMERIC( 8,4) default 0, " +   // максимально близкая температура наружного воздуха в таблице
                " tmpr_narug_vozd  NUMERIC( 8,4) default 0, " +   // температура наружного воздуха по проекту (паспорту) дома
                " dom_ud_tepl_en   NUMERIC(12,8) default 0,  " +   // удельный расход тепловой энергии для дома по температуре и этажности
                " norm_type_id integer,  " +
                " norm_tables_id integer " + // id норматива - по нему можно получить набор влияющих пар-в и их знач.
                " ) "
                );


            // === перечень л/с для расчета нормативов ===
            BillingInstrumentary.ExecSQL(
                  " insert into t_norm_otopl (nzp_dom,nzp_kvar,s_otopl)" +
                  " select nzp_dom,nzp_kvar,max(squ2) from ttt_counters_xx " +
                  " where nzp_serv=8 " +
                  " group by 1,2 "
                  );


            BillingInstrumentary.ExecSQL(" create index ix1_norm_otopl on t_norm_otopl (nzp_kvar) ");
            BillingInstrumentary.ExecSQL(" create index ix2_norm_otopl on t_norm_otopl (nzp_dom) ");


            int iNzpRes;
            // если разрешено рассчитывать норматив по отоплению
            if (bCalcNormOtopl)
            {
                #region N: отопление - расчет нормативов по типам

                // === параметры для всех алгоритмов расчета нормативов ===

                // нормативы от этажей (для РТ) и климатических зон (для сах РС-Я)
                iNzpRes = LoadIntValPrmForNorm(rashod.paramcalc.pref, 186, "13", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);
                if (iNzpRes != 0)
                {
                    BillingInstrumentary.ExecSQL(
                          " Update t_norm_otopl " +
                          " Set nzp_res0 = " + iNzpRes + " Where 1=1 "
                        );

                }

                // количество этажей дома (этажность)
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_kol_etag=" +
                      "    ( select max( replace(COALESCE(p.val_prm,'0'),',','.')::int ) from ttt_prm_2 p " +
                      "      where t_norm_otopl.nzp_dom=p.nzp " +
                             " and p.nzp_prm=37 " +
                      " )" +
                      " where exists" +
                      "    ( select 1 from ttt_prm_2 p " +
                      "      where t_norm_otopl.nzp_dom=p.nzp " +
                             " and p.nzp_prm=37 " +
                      " ) "
                    );


                //ViewTbl( " select * from t_norm_otopl where nzp_kvar=3829 ");

                // поиск указанного норматива в ГКал на 1 кв. метр. вид методики расчета норматива = 0

                TempTablesLifeTime.AddTempTable("t_otopl_m2",
                    " create temp table t_otopl_m2 ( " +
                    " nzp_dom         integer, " +
                    " rashod_gkal_m2  NUMERIC(12,8) default 0 " +    // нормативный расход в ГКал на 1 м2
                    " )  "
                     );


                // === алгоритм расчет нормативов 0 ===
                BillingInstrumentary.ExecSQL(
                      " Insert into t_otopl_m2 (nzp_dom,rashod_gkal_m2)" +
                      " Select p.nzp,max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                      " From ttt_prm_2 p " +
                      " Where p.nzp_prm=723 " +
                      "    and exists (select 1 from t_norm_otopl t where t.nzp_dom=p.nzp) " +
                      " group by 1 "
                      );


                // вид методики расчета норматива
                BillingInstrumentary.ExecSQL(
                      " Update t_norm_otopl set vid_alg=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.') ::int) " +
                           " From ttt_prm_2 p " +
                      "      Where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=709 " +
                      " )" +
                      " Where vid_alg<>0 and exists" +
                      "    ( Select 1 " +
                           " From ttt_prm_2 p " +
                      "      Where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=709 " +
                      " ) "
                    );


                // коэффициент перерасчета по итогам года
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set koef_god_pere=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from " + rashod.paramcalc.pref + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=108" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " )" +
                      " where exists" +
                      "    ( select 1 " +
                           " from " + rashod.paramcalc.pref + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=108" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s +
                      " ) "
                    );


                BillingInstrumentary.ExecSQL(" create index ix1_otopl_m2 on t_otopl_m2 (nzp_dom) ");

                BillingInstrumentary.ExecSQL(
                      " Update t_norm_otopl" +
                      " Set vid_alg=0, rashod_gkal_m2=( Select rashod_gkal_m2 From t_otopl_m2 p Where t_norm_otopl.nzp_dom=p.nzp_dom )" +
                      " Where exists ( Select 1 From t_otopl_m2 p Where t_norm_otopl.nzp_dom=p.nzp_dom ) "
                    );

                //
                #region Выбрать норму Гкал на 1 м2 для отопления на лицевой счет prm_1 nzp_prm =2463

                // выбрать параметры л/с для типа 1814 на дату расчета
                TempTablesLifeTime.DropTempTable("t_p2463 ", false);

                TempTablesLifeTime.AddTempTable("t_p2463",
                    " create temp table t_p2463 (" +
                    " nzp     integer," +
                    " vald    NUMERIC(14,7))  ");


                // Выбрать квартирный норматив гкал на м3
                BillingInstrumentary.ExecSQL(
                    " insert into t_p2463 (nzp,vald)" +
                    " select nzp,max(COALESCE(val_prm,'0')::numeric) " +
                    " from ttt_prm_1" +
                    " where nzp_prm=2463 " +
                    " group by 1 "
                    );
                BillingInstrumentary.ExecSQL(" create index ixt_p2463 on t_p2463(nzp) ");

                BillingInstrumentary.ExecSQL(
                    " update t_norm_otopl" +
                    " set vid_alg=0, rashod_gkal_m2=( select p2.vald from t_p2463 p2 where p2.nzp=t_norm_otopl.nzp_kvar ) " +
                    " where exists ( select 1 from t_p2463 p1 where p1.nzp=t_norm_otopl.nzp_kvar ) "
                    );
                TempTablesLifeTime.DropTempTable("t_p2463 ", false);

                #endregion Выбрать норму Гкал на 1 м2 для отопления на лицевой счет prm_1 nzp_prm =2463


                // === алгоритмы расчета нормативов 1, 2, 3, 4 ===

                // признак угловой квартиры (0-обычная/1-угловая)
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set ugl_kv=1" +
                      " where 0<(select count(*) from ttt_prm_1 p where t_norm_otopl.nzp_kvar=p.nzp and p.nzp_prm=310) "
                    );


                // vid_alg=1 ==================================

                // === параметры на всю БД (prm_5 - в 2.0 указаны в формуле!) ===
                // tmpr_vnutr_vozd -- температура внутреннего воздуха (обычно = 20, для угловых = 22)
                // tmpr_vnesh_vozd -- средняя температура внешнего воздуха  (обычно = -5.7)
                // otopl_period    -- продолжительность отопительного периода в сутках (обычно = 218)
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set" +
                      "  tmpr_vnutr_vozd = (case when ugl_kv=1 then 22 else 20 end)," +
                      "  tmpr_vnesh_vozd = -5.7," +
                      "  otopl_period    = 218" +
                      " where vid_alg=1 "
                    );


                // === домовые параметры (prm_2) ===
                // поправочно-отопительный коэффициент для дома
                BillingInstrumentary.ExecSQL(
                      " Update t_norm_otopl Set dom_otopl_koef=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " From ttt_prm_2 p Where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=33 " +
                      " )" +
                      " Where vid_alg=1 and 0<" +
                      "    ( Select count(*)" +
                           " From ttt_prm_2 p Where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=33 " +
                      " ) "
                    );


                // объем дома
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_objem=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=32 " +
                      " )" +
                      " where vid_alg=1 and 0<" +
                      "    ( select count(*)" +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=32 " +
                      " ) "
                    );


                // полезная/отапливаемая площадь дома
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_pol_pl=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=36 " +
                      " )" +
                      " where vid_alg=1 and 0<" +
                      "    ( select count(*)" +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=36 " +
                      " ) "
                    );


                // удельная отопительная характеристика дома
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_ud_otopl_har=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=31 " +
                      " )" +
                      " where vid_alg=1 and 0<" +
                      "    ( select count(*)" +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=31 " +
                      " ) "
                    );


                // vid_alg=2 / 3 / 4 ==================================

                // === параметры на всю БД (prm_5) ===
                // температура внутреннего воздуха (обычно = 20, для угловых = 22)
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set tmpr_vnutr_vozd=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from " + rashod.paramcalc.pref + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=54" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " )" +
                      " where vid_alg in (2,3) and 0<" +
                      "    ( select count(*) from " + rashod.paramcalc.pref + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=54" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " ) "
                    );


                // для угловых
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set tmpr_vnutr_vozd=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from " + rashod.paramcalc.pref + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=713" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " )" +
                      " where vid_alg in (2,3) and ugl_kv=1 and 0<" +
                      "    ( select count(*) from " + rashod.paramcalc.pref + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=713" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " ) "
                    );

                // средняя температура внешнего воздуха  (обычно = -5.7)
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set tmpr_vnesh_vozd=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from " + rashod.paramcalc.pref + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=710" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " )" +
                      " where vid_alg in (2,3) and 0<" +
                      "    ( select count(*) from " + rashod.paramcalc.pref + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=710" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " ) "
                    );


                // продолжительность отопительного периода в сутках (обычно = 218)
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set otopl_period=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from " + rashod.paramcalc.pref + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=712" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " )" +
                      " where vid_alg in (2,3) and 0<" +
                      "    ( select count(*) from " + rashod.paramcalc.pref + "prm_5 p" +
                      "      where p.is_actual<>100 and p.nzp_prm=712" +
                      "        and p.dat_s  <= " + rashod.paramcalc.dat_po +
                      "        and p.dat_po >= " + rashod.paramcalc.dat_s + " ) "
                    );


                // === домовые параметры (prm_2) ===
                // дата постройки дома
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_dat_postr=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::date) " +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=150 " +
                      " )" +
                      " where vid_alg in (2,3,4) and 0<" +
                      "    ( select count(*)" +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=150 " +
                      " ) "
                    );


                // количество этажей дома (этажность)
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_kol_etag=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::int) " +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=37 " +
                      " )" +
                      " where vid_alg in (2,3,4) and 0<" +
                      "    ( select count(*)" +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=37 " +
                      " ) "
                    );


                // температура наружного воздуха по проекту (паспорту) дома
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set tmpr_narug_vozd=" +
                      "    ( Select max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric) " +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=711 " +
                      " )" +
                      " where vid_alg in (2,3) and 0<" +
                      "    ( select count(*)" +
                           " from ttt_prm_2 p where t_norm_otopl.nzp_dom=p.nzp and p.nzp_prm=711 " +
                      " ) "
                    );



                TempTablesLifeTime.AddTempTable("t_etag1999",
                    " create temp table t_etag1999(y1 int,val1 char(20),etag1 integer,etag2 integer) "
                    );

                const string sposfun = "strpos";


                string ssql = " insert into t_etag1999(y1,val1,etag1,etag2) " +
                              " select " +
                              " r.nzp_y y1,r.value val1" +
                              ",(case when " + sposfun + "(r.value,'-')=0" +
                              "       then (r.value::int) else substr(r.value,1," +
                              sposfun + "(r.value,'-')-1)::int" +
                              "  end) etag1 " +
                              ",COALESCE((case when " + sposfun + "(b.value,'-')=0" +
                              "       then (b.value::int) else substr(b.value,1," +
                              sposfun + "(b.value,'-')-1)::int" +
                              "  end),9999) etag2 " +
                              " from " + rashod.paramcalc.pref + "res_values r " +

                              " left outer join " + rashod.paramcalc.pref + "res_values b " +
                              " on r.nzp_y=b.nzp_y-1 and b.nzp_res=9996 and b.nzp_x=1 where 1=1" +

                              " and r.nzp_res=9996 and r.nzp_x=1  ";
                BillingInstrumentary.ExecSQL(ssql);


                BillingInstrumentary.ExecSQL(" create index ix_etag1999 on t_etag1999 (etag1,etag2) ");

                // таблица диапазонов этажей для таблицы удельных расходов тепловой энергии (Пост.306) после 1999 года
                TempTablesLifeTime.AddTempTable("t_etag",
                    " create temp table t_etag(y1 int,val1 char(20),etag1 integer,etag2 integer) "
                    );


                ssql =
                    " insert into t_etag(y1,val1,etag1,etag2) " +
                    " select " +
                    " r.nzp_y y1,r.value val1" +
                    ",(case when " + sposfun + "(r.value,'-')=0" +
                    "       then (r.value::int) else substr(r.value,1," +
                    sposfun + "(r.value,'-')-1)::int" +
                    "  end) etag1 " +
                    ",COALESCE((case when " + sposfun + "(b.value,'-')=0" +
                    "       then (b.value::int) else substr(b.value,1," +
                    sposfun + "(b.value,'-')-1)::int" +
                    "       end),9999) etag2 " +
                    " from " + rashod.paramcalc.pref + "res_values r " +

 " left outer join " + rashod.paramcalc.pref + "res_values b" +
                    " on r.nzp_y=b.nzp_y-1 and b.nzp_res=9997 and b.nzp_x=1 where 1=1 " +

 " and r.nzp_res=9997 and r.nzp_x=1 ";
                BillingInstrumentary.ExecSQL(ssql);


                BillingInstrumentary.ExecSQL(" create index ix_etag on t_etag (etag1,etag2) ");

                // таблица диапазонов температур для таблицы удельных расходов тепловой энергии (Пост.306)
                BillingInstrumentary.ExecSQL(
                    " select  r.nzp_y y1,r.value val1" +
                    ",(case when strpos(r.value,'-')=0" +
                    "       then (r.value::int) else substring(r.value,1,strpos(r.value,'-')-1)::int" +
                    "  end) tmpr1 " +
                    ",coalesce((case when strpos(b.value,'-')=0" +
                    "       then (b.value::int) else substring(b.value,1,strpos(b.value,'-')-1)::int" +
                    "       end),9999) tmpr2 " +
                    " into temp t_tmpr  " +
                    " from " + rashod.paramcalc.pref + ".res_values r " +
                    " left outer join " + rashod.paramcalc.pref +
                    ".res_values b on r.nzp_y=b.nzp_y-1 and b.nzp_res=9991 and b.nzp_x=1 " +
                    " where r.nzp_res=9991 and r.nzp_x=1 ");


                BillingInstrumentary.ExecSQL(" create index ix_tmpr on t_tmpr (tmpr1,tmpr2) ");


                // позиция по количеству этажей дома в таблице удельных расходов тепловой энергии до 1999
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set pos_etag=" +
                      "    (select max(b.y1) from t_etag1999 b where t_norm_otopl.dom_kol_etag>=b.etag1 and t_norm_otopl.dom_kol_etag<b.etag2)" +
                      " where vid_alg in (2,3) and dom_dat_postr<=" + BillingInstrumentary.MDY(1, 1, 1999)
                    );


                // позиция по количеству этажей дома в таблице удельных расходов тепловой энергии после 1999
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set pos_etag=" +
                      "    (select max(b.y1) from t_etag b where t_norm_otopl.dom_kol_etag>=b.etag1 and t_norm_otopl.dom_kol_etag<b.etag2)" +
                      " where vid_alg in (2,3) and dom_dat_postr>" + BillingInstrumentary.MDY(1, 1, 1999)
                    );


                // позиция по количеству этажей дома в таблице нормативных расходов тепловой энергии с 09.2012г в РТ 
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set pos_etag=" +
                      "    (case when dom_kol_etag>16 then 16 else dom_kol_etag end)" +
                      " where vid_alg=4 "
                    );


                //ViewTbl( " select * from t_norm_otopl where nzp_kvar=3829 ");

                // pos_narug_vozd   - позиция по температуре наружного воздуха в таблице удельных расходов тепловой энергии
                // tmpr_narug_vozd1 - минимально  близкая температура наружного воздуха в таблице
                // tmpr_narug_vozd2 - максимально близкая температура наружного воздуха в таблице
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set (pos_narug_vozd,tmpr_narug_vozd1,tmpr_narug_vozd2)=" +
                      "    ((select max(b.y1) from t_tmpr b " +
                      "      where abs(t_norm_otopl.tmpr_narug_vozd)>=b.tmpr1 and abs(t_norm_otopl.tmpr_narug_vozd)<b.tmpr2)," +
                           "(select max(abs(b.tmpr1)) from t_tmpr b " +
                      "      where abs(t_norm_otopl.tmpr_narug_vozd)>=b.tmpr1 and abs(t_norm_otopl.tmpr_narug_vozd)<b.tmpr2)," +
                           "(select max(abs(b.tmpr2)) from t_tmpr b " +
                      "      where abs(t_norm_otopl.tmpr_narug_vozd)>=b.tmpr1 and abs(t_norm_otopl.tmpr_narug_vozd)<b.tmpr2))" +
                      " where vid_alg in (2,3) "
                    );


                // минимальный  удельный расход тепловой энергии для дома по температуре и этажности
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_ud_tepl_en1=" +
                      "    (select max(replace(COALESCE(r.value,'0'),',','.')::numeric) " +
                      " from " + rashod.paramcalc.pref + "res_values r" +
                      "     where r.nzp_res=9996 and t_norm_otopl.pos_etag=r.nzp_y" +
                            " and (case when t_norm_otopl.pos_narug_vozd>=2 then t_norm_otopl.pos_narug_vozd else 2 end)=r.nzp_x)" +
                      " where vid_alg in (2,3) and dom_dat_postr<=" + BillingInstrumentary.MDY(1, 1, 1999)
                    );


                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_ud_tepl_en1=" +
                      "    (select max(replace(COALESCE(r.value,'0'),',','.')::numeric) " +
                      " from " + rashod.paramcalc.pref + "res_values r" +
                      "     where r.nzp_res=9997 and t_norm_otopl.pos_etag=r.nzp_y" +
                            " and (case when t_norm_otopl.pos_narug_vozd>=2 then t_norm_otopl.pos_narug_vozd else 2 end)=r.nzp_x)" +
                      " where vid_alg in (2,3) and dom_dat_postr> " + BillingInstrumentary.MDY(1, 1, 1999)
                    );


                // максимальный удельный расход тепловой энергии для дома по температуре и этажности
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_ud_tepl_en2=" +
                      "    (select max(replace(COALESCE(r.value,'0'),',','.')::numeric) " +
                      " from " + rashod.paramcalc.pref + "res_values r" +
                      "     where r.nzp_res=9996 and t_norm_otopl.pos_etag=r.nzp_y and t_norm_otopl.pos_narug_vozd+1=r.nzp_x)" +
                      " where vid_alg in (2,3) and dom_dat_postr<=" + BillingInstrumentary.MDY(1, 1, 1999)
                    );


                BillingInstrumentary.ExecSQL(
                      " Update t_norm_otopl set dom_ud_tepl_en2=" +
                      "    (select max(replace(COALESCE(r.value,'0'),',','.')::numeric) " +
                      " From " + rashod.paramcalc.pref + "res_values r" +
                      "     where r.nzp_res=9997 and t_norm_otopl.pos_etag=r.nzp_y and t_norm_otopl.pos_narug_vozd+1=r.nzp_x)" +
                      " Where vid_alg in (2,3) and dom_dat_postr> " + BillingInstrumentary.MDY(1, 1, 1999)
                    );


                // dom_ud_tepl_en - удельный расход тепловой энергии для дома по температуре и этажности
                // без интерполяции
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_ud_tepl_en=dom_ud_tepl_en1" +
                      " where vid_alg=2 "
                    );


                // с интерполяцией
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set dom_ud_tepl_en=dom_ud_tepl_en1+" +
                      "   (dom_ud_tepl_en2-dom_ud_tepl_en1)*(abs(tmpr_narug_vozd)-tmpr_narug_vozd1)/(tmpr_narug_vozd2-tmpr_narug_vozd1)" +
                      " where vid_alg=3 and abs(tmpr_narug_vozd2-tmpr_narug_vozd1)>0.0001 "
                    );


                // === расчет нормативов в ГКал на 1 кв.м - vid_alg=1 ===
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set rashod_gkal_m2 = 0.98 * dom_ud_otopl_har * dom_objem / dom_pol_pl" +
                      " where vid_alg=1 and dom_pol_pl> 0.0001"
                    );


                // === расчет нормативов в ГКал на 1 кв.м - vid_alg in (2,3) ===
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set rashod_gkal_m2 = dom_ud_tepl_en / (tmpr_vnutr_vozd - tmpr_narug_vozd)" +
                      " where vid_alg in (2,3) and abs(tmpr_vnutr_vozd - tmpr_narug_vozd)> 0.0001 "
                    );


                // === расчет нормативов в ГКал на 1 кв.м - общее для vid_alg in (1,2,3) ===
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set rashod_gkal_m2 =" +
                      " rashod_gkal_m2 * (tmpr_vnutr_vozd - tmpr_vnesh_vozd) * otopl_period * 24 * 0.000001 / 12 * koef_god_pere" +
                      " where vid_alg in (1,2,3) "
                    );


                // === установить норматив в ГКал на 1 кв.м - из таблицы для vid_alg=4 ===
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set rashod_gkal_m2=" +
                      "    (select max(replace(COALESCE(r.value,'0'),',','.')::numeric) " +
                      " from " + rashod.paramcalc.pref + "res_values r" +
                      "     where r.nzp_res=t_norm_otopl.nzp_res0 and t_norm_otopl.pos_etag=r.nzp_y" +
                      " and r.nzp_x=(case when dom_dat_postr<=" + BillingInstrumentary.MDY(1, 1, 1999) + " then 1 else 2 end) )" +
                      " where vid_alg=4 "
                    );


                #endregion N: отопление - расчет нормативов по типам
            }
            else
            {
                BillingInstrumentary.ExecSQL(
                      " update t_norm_otopl set vid_alg=0,rashod_gkal_m2=0 "
                    );

            }
            //ViewTbl( " select * from t_norm_otopl where nzp_kvar=3829 ");

            BillingInstrumentary.ExecSQL(
                  " update t_norm_otopl set rashod_gkal_m2=0 where rashod_gkal_m2 is null  "
                );


            // ===  установка норматива по отоплению в counters_xx ===
            sql =
                " Update ttt_counters_xx " +
                " Set (val1,val3,rash_norm_one,kod_info) = " +
                "((Select rashod_gkal_m2 * ttt_counters_xx.squ2 From t_norm_otopl k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar)," +
                "(Select rashod_gkal_m2 From t_norm_otopl k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar)," +
                "(Select rashod_gkal_m2 From t_norm_otopl k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar)," +
                "(Select vid_alg From t_norm_otopl k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar)) " +
                " Where nzp_serv=8 " +
                "   and 0 < ( Select count(*) From t_norm_otopl k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar and k.vid_alg in (0,1,2,3,4,5) ) ";
            BillingInstrumentary.ExecSQL(sql);

        }
        #endregion N: отопление

        #region N: электроотопление

        private void CalcRashodNormElOtopl()
        //--------------------------------------------------------------------------------
        {
            //----------------------------------------------------------------
            //N: электроотопление
            //----------------------------------------------------------------
            TempTablesLifeTime.AddTempTable("t_norm_eeot",
                " create temp table t_norm_eeot ( " +
                //" create table are.t_norm_otopl ( " +
                " nzp_dom       integer, " +
                " nzp_kvar      integer, " +
                " s_otopl       NUMERIC( 8,2) default 0, " + // отапливаемая площадь
                " rashod_kvt_m2 NUMERIC(12,8) default 0, " + // нормативный расход в квт*час на 1 м2
                // для расчета норматива по отоплению
                " nzp_res0      integer default 0,      " + // таблица нормативов
                " dom_klimatz   integer default 0,      " + // Климатическая зона
                " dom_kol_etag  integer default 0,       " + // количество этажей дома (этажность)
                " pos_etag      integer default 0        " +
                // позиция по количеству этажей дома в таблице удельных расходов тепловой энергии
                " ) "
                );


            // === перечень л/с для расчета нормативов ===
            BillingInstrumentary.ExecSQL(
                " insert into t_norm_eeot (nzp_dom,nzp_kvar,s_otopl)" +
                " select nzp_dom,nzp_kvar,max(squ2) from ttt_counters_xx " +
                " where nzp_serv=322 " +
                " group by 1,2 "
                );
            BillingInstrumentary.ExecSQL(" create index ix1_norm_eeot on t_norm_eeot (nzp_kvar) ");

            // поиск указанного норматива в Квт*час на 1 кв. метр. вид методики расчета норматива = 0
            // домовые
            BillingInstrumentary.ExecSQL(
                " update t_norm_eeot set rashod_kvt_m2= t.val_prm::numeric " +
                " from ttt_prm_2 t where t.nzp = t_norm_eeot.nzp_dom and nzp_prm = 1479"
                );

            // квартирные
            BillingInstrumentary.ExecSQL(
                " update t_norm_eeot set rashod_kvt_m2=t.val_prm::numeric " +
                "  from ttt_prm_1 t where t.nzp = t_norm_eeot.nzp_kvar and nzp_prm = 1480"
                );


            BillingInstrumentary.ExecSQL(
                " update t_norm_eeot set rashod_kvt_m2=0 where rashod_kvt_m2 is null  "
                );


            // ===  установка норматива по электроотоплению в counters_xx ===
            BillingInstrumentary.ExecSQL(
                " Update ttt_counters_xx " +
                " Set (val1,val3) = " +
                "((Select rashod_kvt_m2 * s_otopl From t_norm_eeot k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar)," +
                " (Select rashod_kvt_m2 From t_norm_eeot k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar)) " +
                " Where nzp_serv=322 " +
                "   and EXISTS ( Select 1 From t_norm_eeot k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar ) "
                );
        }

        #endregion N: электроотопление

        #region N: газовое отопление
        private void CalcRashodNormGasOtopl(DbCalcCharge.Rashod rashod)
        //--------------------------------------------------------------------------------
        {
            //----------------------------------------------------------------
            //N: газовое отопление
            //----------------------------------------------------------------
            TempTablesLifeTime.AddTempTable("t_norm_gasot",
                " create temp table t_norm_gasot ( " +
                " nzp_dom       integer, " +
                " nzp_kvar      integer, " +
                " s_otopl       NUMERIC( 8,2) default 0, " +    // отапливаемая площадь
                " rashod_kbm_m2 NUMERIC(12,8) default 0  " +    // нормативный расход в куб.м газа на 1 м2
                " )  "
                );


            // === перечень л/с для расчета нормативов ===
            BillingInstrumentary.ExecSQL(
                  " insert into t_norm_gasot (nzp_dom,nzp_kvar,s_otopl)" +
                  " select nzp_dom,nzp_kvar,max(squ2) from ttt_counters_xx " +
                  " where nzp_serv=325 " +
                  " group by 1,2 "
                  );


            BillingInstrumentary.ExecSQL(" create index ix1_norm_gasot on t_norm_gasot (nzp_kvar) ");

            BillingInstrumentary.ExecSQL(
                  " update t_norm_gasot set rashod_kbm_m2=" +
                  " (Select max(val_prm::numeric)" +
                   " From " + rashod.paramcalc.pref + "prm_5 " +
                   " Where nzp_prm = 169 and is_actual <> 100" +
                   " and dat_s  <= " + rashod.paramcalc.dat_po + " and dat_po >= " + rashod.paramcalc.dat_s + " ) "
                  );


            BillingInstrumentary.ExecSQL(
                  " update t_norm_gasot set rashod_kbm_m2=0 Where rashod_kbm_m2 is null "
                  );


            // ===  установка норматива по газовому отоплению в counters_xx ===
            BillingInstrumentary.ExecSQL(
                " Update ttt_counters_xx " +
                " Set (val1,val3) = " +
                "((Select rashod_kbm_m2 * s_otopl From t_norm_gasot k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar)," +
                " (Select rashod_kbm_m2 From t_norm_gasot k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar)) " +
                " Where nzp_serv=325 " +
                "   and 0 < ( Select count(*) From t_norm_gasot k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar ) "
                );

        }
        #endregion N: газовое отопление

        #region N: газ
        private void CalcRashodNormGas(DbCalcCharge.Rashod rashod)
        {
            //----------------------------------------------------------------
            //N: газ 
            //----------------------------------------------------------------

            TempTablesLifeTime.AddTempTable("t_norm_gas",
                " create temp table t_norm_gas ( " +
                " nzp_dom    integer, " +
                " nzp_kvar   integer, " +
                " rashod_kbm NUMERIC(12,8) default 0, " +    // нормативный расход в куб.м газа на 1 человека
                " nzp_res0   integer default 0,       " +    // таблица нормативов
                " is_gp      integer default 0,      " +   // Климатическая зона
                " is_gvs     integer default 0,      " +   // количество этажей дома (этажность)
                " is_gk      integer default 0       " +   // позиция по количеству этажей дома в таблице удельных расходов тепловой энергии
                " ) "
                );


            // === перечень л/с для расчета нормативов ===
            BillingInstrumentary.ExecSQL(
                " insert into t_norm_gas (nzp_dom,nzp_kvar)" +
                " select nzp_dom,nzp_kvar from ttt_counters_xx " +
                " where nzp_serv=10 " +
                " group by 1,2 "
                );


            BillingInstrumentary.ExecSQL(" create index ix1_norm_gas on t_norm_gas (nzp_kvar) ");

            // поиск указанного норматива в куб.м на 1 человека

            // нормативы
            var iNzpRes = LoadIntValPrmForNorm(rashod.paramcalc.pref, 173, "13", rashod.paramcalc.dat_s, rashod.paramcalc.dat_po);
            if (iNzpRes != 0)
            {
                BillingInstrumentary.ExecSQL(
                      " Update t_norm_gas " +
                      " Set nzp_res0 = " + iNzpRes + " Where 1=1 "
                    );

            }
            // наличие газовой плиты
            BillingInstrumentary.ExecSQL(
                  " update t_norm_gas set is_gp=1" +
                  " where exists( select 1 from ttt_prm_1 p where t_norm_gas.nzp_kvar=p.nzp and p.nzp_prm=551 ) "
                );


            // наличие газовой колонки (водонагревателя)
            BillingInstrumentary.ExecSQL(
                  " update t_norm_gas set is_gk=1" +
                  " where exists( select 1 from ttt_prm_1 p where t_norm_gas.nzp_kvar=p.nzp and p.nzp_prm=1 ) "
                );


            // наличие ГВС
            var sql =
                " update t_norm_gas set is_gvs=1" +
                " where exists( select 1 from ttt_prm_1 p where t_norm_gas.nzp_kvar=p.nzp and p.nzp_prm=7 " +
                " and p.val_prm::int in (";
            if (Points.IsSmr)
            {
                sql = sql.Trim() + "10";
            }
            else
            {
                sql = sql.Trim() + "05,07,08,09,14,15,16,17";
            }
            sql = sql.Trim() + ") ) ";

            BillingInstrumentary.ExecSQL(sql);


            BillingInstrumentary.ExecSQL(
                " Update t_norm_gas Set " +
                "  rashod_kbm = ( Select max(r2.value::numeric)" +
                " From " + rashod.paramcalc.pref + "res_values r1, " + rashod.paramcalc.pref + "res_values r2 " +
                    "  Where r1.nzp_res = nzp_res0 and r2.nzp_res = nzp_res0 " +
                    "   and r1.nzp_x = 1 and r2.nzp_x = 2 and r1.nzp_y=r2.nzp_y " +
                    "   and trim(r1.value) = (" +
                        " (case when is_gp =1 then '1' else '0' end) ||" +
                        " (case when is_gvs=1 then '1' else '0' end) ||" +
                        " (case when is_gk =1 then '1' else '0' end)" +
                    ") " +
                    " ) " +
                " Where (is_gp > 0 or is_gk>0) " +
                "   and nzp_res0 in ( Select nzp_res From " + rashod.paramcalc.pref + "res_values ) "
                );


            BillingInstrumentary.ExecSQL(
                " Update t_norm_gas Set rashod_kbm = 0 Where rashod_kbm is null "
                );

            sql =
                " UPDATE ttt_counters_xx " +
                " SET val1   = n.rashod_kbm * gil1   * (cntd * 1.00 / cntd_mn), " +
                "     val1_g = n.rashod_kbm * gil1_g * (cntd * 1.00 / cntd_mn), " +
                "     val3          = n.rashod_kbm, " +
                "     rash_norm_one = n.rashod_kbm " +
                " FROM t_norm_gas n " +
                " WHERE ttt_counters_xx.nzp_kvar = n.nzp_kvar AND ttt_counters_xx.nzp_serv = 10 ";
            BillingInstrumentary.ExecSQL(sql);

        }
        #endregion N: газ

        #region N: полив
        private void CalcRashodNormPoliv(DbCalcCharge.Rashod rashod)
        //--------------------------------------------------------------------------------
        {
            //----------------------------------------------------------------
            //N: полив
            //----------------------------------------------------------------
            TempTablesLifeTime.DropTempTable("ttt_aid_c1 ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_c1",
                " Create temp table ttt_aid_c1 " +
                " ( nzp_kvar integer, " +
                "   ival1 integer default 0, " +
                "   rval1  NUMERIC(12,4) default 0.00," +
                "   rval2  NUMERIC(12,4) default 0.00," +
                "   rval3  NUMERIC(12,4) default 0.00," +
                "   rval4  NUMERIC(12,4) default 0.00 " +
                " )  "
                );


            // для РТ нормативный расход полива = объем на сотку * кол-во соток
            var sNorm200 =
            ",1 as ival1 " +
            ",max(case when nzp_prm=262 then COALESCE(p.val_prm,'0')::numeric else 0 end) as rval1 " +
            ",max(case when nzp_prm=390 then COALESCE(p.val_prm,'0')::numeric else 0 end) as rval2 " +
                // для садов в Туле!
            ",max(case when nzp_prm=2466 then COALESCE(p.val_prm,'0')::numeric else 0 end) as rval3 " +
            ",max(case when nzp_prm=2467 then COALESCE(p.val_prm,'0')::numeric else 0 end) as rval4 ";

            var sFlds200 = "262,390,2466,2467";
            if (Points.IsSmr)
            {
                // для Самары нормативный расход полива = кол-во поливок * площадь полива в кв.м * объем на 1 кв.м
                sNorm200 =
                ",max(case when nzp_prm=2044 then COALESCE(p.val_prm,'0')::numeric else 0 end) as ival1 " +
                ",max(case when nzp_prm=2011 then COALESCE(p.val_prm,'0')::numeric else 0 end) as rval1 " +
                ",max(case when nzp_prm=2043 then COALESCE(p.val_prm,'0')::numeric else 0 end) as rval2 " +
                ",0 as rval3 " +
                ",0 as rval4 ";
                sFlds200 = "2011,2043,2044";
            }

            BillingInstrumentary.ExecSQL(
                  " insert into ttt_aid_c1 (nzp_kvar, ival1, rval1, rval2, rval3, rval4) " +
                  " Select nzp as nzp_kvar" + sNorm200 +
                  " From ttt_counters_xx a, ttt_prm_1 p " +
                  " Where a.nzp_kvar = p.nzp " +
                  "   and a.nzp_serv=200 " +
                  "   and p.nzp_prm in (" + sFlds200 + ") " +
                  " Group by 1 "
                );


            BillingInstrumentary.ExecSQL(" Create unique index ix_ttt_aid_c1 on ttt_aid_c1 (nzp_kvar) ");

            BillingInstrumentary.ExecSQL(
                " Update ttt_counters_xx " +
                " Set val1 =" +
                " ( Select k.ival1 * (k.rval1 * k.rval2 + k.rval3 * k.rval4)" +
                  " From ttt_aid_c1 k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar ) " +
                " , rash_norm_one =" +
                " ( Select k.ival1 * (k.rval2 + k.rval4)" +
                  " From ttt_aid_c1 k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar ) " +
                " Where nzp_serv=200 " +
                "   and 0 < ( Select count(*)" +
                  " From ttt_aid_c1 k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar ) "
                );

        }
        #endregion N: полив

        #region N: вода для бани
        private void CalcRashodNormHVforBanja()
        //--------------------------------------------------------------------------------
        {
            //----------------------------------------------------------------
            //N: вода для бани
            //----------------------------------------------------------------
            TempTablesLifeTime.DropTempTable("ttt_aid_c1 ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_c1",
                " Create temp table ttt_aid_c1 " +
                " ( nzp_kvar integer, " +
                "   gil1   NUMERIC(12,4) default 0.00," +
                "   gil1_g NUMERIC(12,4) default 0.00," +
                "   dat_s date not null," +
                "   dat_po date not null," +
                "   norm   NUMERIC(12,4) default 0.00 " +
                " )  "
                );


            // для РТ нормативный расход вода для бани = объем на Кж * Норма на 1 чел.
            var sFlds200 = "268";
            BillingInstrumentary.ExecSQL(
                  " insert into ttt_aid_c1 (nzp_kvar, gil1, gil1_g,dat_s,dat_po, norm) " +
                  " Select nzp as nzp_kvar, gil1, gil1_g, a.dat_s,a.dat_po, max(COALESCE(p.val_prm,'0')::numeric) " +
                  " From ttt_counters_xx a, ttt_prm_1 p " +
                  " Where a.nzp_kvar = p.nzp " +
                  "   and a.nzp_serv=203 " +
                  "   and p.nzp_prm in (" + sFlds200 + ") " +
                  "   and p.dat_s<=a.dat_po and p.dat_po>=a.dat_s" +
                  " Group by 1,2,3,4,5 "
                );


            BillingInstrumentary.ExecSQL(" Create index ix_ttt_aid_c1 on ttt_aid_c1 (nzp_kvar) ");

            BillingInstrumentary.ExecSQL(
                 " Update ttt_counters_xx t " +
                 " Set" +
                   " cnt1 = " + sFlds200 +
                   ",val1 =  k.gil1 * k.norm " +
                   ",val1_g =  k.gil1_g * k.norm " +
                   ",rash_norm_one =  k.norm " +
                 " From ttt_aid_c1 k " +
                 " Where t.nzp_kvar = k.nzp_kvar " +
                 " and t.nzp_serv=203 " +
                 " and t.dat_s=k.dat_s and t.dat_po=k.dat_po"
                );

        }
        #endregion N: вода для бани

        #region N: питьевая вода
        private void CalcRashodNormPitHV()
        //--------------------------------------------------------------------------------
        {
            //----------------------------------------------------------------
            //N: питьевая вода
            //----------------------------------------------------------------
            TempTablesLifeTime.DropTempTable("ttt_aid_c1 ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_c1",
                " Create temp table ttt_aid_c1 " +
                " ( nzp_kvar integer, " +
                "   ival1 integer default 0, " +
                "   rval1  NUMERIC(12,4) default 0.00," +
                "   rval1_g NUMERIC(12,4) default 0.00," +
                "   rval2  NUMERIC(12,4) default 0.00 " +
                " ) "
                );


            // для РТ нормативный расход пит.воды = кол-во жильцов * кол-во литров (норма на дом)
            BillingInstrumentary.ExecSQL(
                " insert into ttt_aid_c1 (nzp_kvar, ival1, rval1, rval1_g, rval2) " +
                " Select nzp_kvar,max(cnt1),max(gil1),max(gil1_g),max( replace(COALESCE(p.val_prm,'0'),',','.')::numeric )" +
                " From ttt_counters_xx a " +
                " left outer join ttt_prm_2 p on a.nzp_dom=p.nzp and p.nzp_prm=705 " +
                " Where 1=1 " +
                " and a.nzp_serv=253 " +
                " Group by 1 ");


            BillingInstrumentary.ExecSQL(" Create unique index ix_ttt_aid_c1 on ttt_aid_c1 (nzp_kvar) ");

            BillingInstrumentary.ExecSQL(
                " Update ttt_counters_xx " +
                " Set (val1,val1_g,rash_norm_one) =" +
                " (( Select k.rval1 * k.rval2" +
                   " From ttt_aid_c1 k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar )," +
                "  ( Select k.rval1_g * k.rval2" +
                   " From ttt_aid_c1 k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar )," +
                "  ( Select k.rval2" +
                   " From ttt_aid_c1 k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar )) " +
                " Where nzp_serv=253 " +
                "   and 0 < ( Select count(*) From ttt_aid_c1 k Where ttt_counters_xx.nzp_kvar = k.nzp_kvar ) "
                );

        }
        #endregion N: питьевая вода


        /// <summary>
        /// создание ttt_counters_xx - структуры для получения нормативов
        /// </summary>
        /// <param name="rashod"></param>
        public string CreateTempNormatives(DbCalcCharge.Rashod rashod, string TargetTable)
        {

            var TableNormatives = "ttt_counters_xx";
            TempTablesLifeTime.AddTempTable(TableNormatives, " Create Temp table " + TableNormatives +
                  " (  nzp_cntx    serial        not null, " +
                  "    nzp_dom     integer       not null, " +
                  "    nzp_kvar    integer       default 0 not null , " +
                  "    nzp_type    integer       not null, " +               //1,2,3
                  "    nzp_serv    integer       not null, " +
                  "    dat_charge  date, " +
                  "    cur_zap     integer       default 0 not null, " +     //0-текущее значение, >0 - ссылка на следующее значение (nzp_cntx)
                  "    nzp_counter integer       default 0 , " +             //счетчик или вариатны расходов для stek=3
                  "    cnt_stage   integer       default 0 , " +             //разрядность
                  "    mmnog       NUMERIC(15,7) default 1.00 , " +             //масшт. множитель
                  "    stek        integer       default 0 not null, " +     //3-итого по лс,дому; 1-счетчик; 2,3,4,5 - стек расходов
                  "    rashod      NUMERIC(15,7) default 0.00 not null, " +  //общий расход в зависимости от stek
                  "    dat_s       date          not null, " +               //"дата с" - для ПУ, для по-дневного расчета период в месяце (dp)
                  "    val_s       NUMERIC(15,7) default 0.00 not null, " +  //значение (а также коэф-т)
                  "    dat_po      date not null, " +                        //"дата по"- для ПУ, для по-дневного расчета период в месяце (dp_end)
                  "    val_po      NUMERIC(15,7) default 0.00 not null, " +  //значение
                  "    ngp_cnt       NUMERIC(14,7) default 0.0000000, " +  // расход на нежилые помещения
                  "    rash_norm_one NUMERIC(14,7) default 0.0000000, " +  // норматив на 1 человека
                  "    val1_g      NUMERIC(15,7) default 0.00 not null, " +  //расход по счетчику nzp_counter или нормативные расходы в расчетном месяце без учета вр.выбывших
                  "    val1        NUMERIC(15,7) default 0.00 not null, " +  //расход по счетчику nzp_counter или нормативные расходы в расчетном месяце
                  "    val2        NUMERIC(15,7) default 0.00 not null, " +  //дом: расход КПУ
                  "    val3        NUMERIC(15,7) default 0.00         , " +  //дом: расход нормативщики
                  "    val4        NUMERIC(15,7) default 0.00         , " +  //общий расход по счетчику nzp_counter
                  "    rvirt       NUMERIC(15,7) default 0.00         , " +  //вирт. расход
                  "    squ1        NUMERIC(15,7) default 0.00         , " +  //площадь лс, дома (по всем лс)
                  "    squ2        NUMERIC(15,7) default 0.00         , " +  //площадь лс без КПУ (для домовых строк)
                  "    nzp_prm_squ2 integer default 133, " +                              // по умолчанию как squ2 берется nzp_prm=133 отапливаемая площадь
                  "    cls1        integer       default 0 not null   , " +  //количество лс дома по услуге
                  "    cls2        integer       default 0 not null   , " +  //количество лс без КПУ (для домовых строк)
                  "    gil1_g      NUMERIC(15,7) default 0.00         , " +  //кол-во жильцов в лс без учета вр.выбывших
                  "    gil1        NUMERIC(15,7) default 0.00         , " +  //кол-во жильцов в лс
                  "    gil2        NUMERIC(15,7) default 0.00         , " +  //кол-во жильцов в лс
                  "    cnt1_g      integer       default 0 not null, " +     //кол-во жильцов в лс (нормативное) без учета вр.выбывших
                  "    cnt1        integer       default 0 not null, " +     //кол-во жильцов в лс (нормативное)
                  "    cnt2        integer       default 0 not null, " +     //кол-во комнат в лс
                  "    cnt3        integer       default 0, " +              //тип норматива в зависимости от услуги (ссылка на resolution.nzp_res)
                  "    cnt4        integer       default 0, " +              //1-дом не-МКД (0-МКД)
                  "    cnt5        integer       default 0, " +              //резерв
                  "    dop87       NUMERIC(15,7) default 0.00         , " +  //доп.значение 87 постановления (7кВт или добавок к нормативу  (87 П) )
                  "    pu7kw       NUMERIC(15,7) default 0.00         , " +  //7 кВт для КПУ (откорректированный множитель)
                  "    gl7kw       NUMERIC(15,7) default 0.00         , " +  //7 кВт КПУ * gil1 (учитывая корректировку)
                  "    vl210       NUMERIC(15,7) default 0.00         , " +  //расход 210 для nzp_type = 6
                  "    kf307       NUMERIC(15,7) default 0.00         , " +  //коэфициент 307 для КПУ или коэфициент 87 для нормативщиков
                  "    kf307n      NUMERIC(15,7) default 0.00         , " +  //коэфициент 307 для нормативщиков
                  "    kf307f9     NUMERIC(15,7) default 0.00         , " +  //коэфициент 307 по формуле 9
                  "    kf_dpu_kg   NUMERIC(15,7) default 0.00         , " +  //коэфициент ДПУ для распределения пропорционально кол-ву жильцов
                  "    kf_dpu_plob NUMERIC(15,7) default 0.00         , " +  //коэфициент ДПУ для распределения пропорционально сумме общих площадей
                  "    kf_dpu_plot NUMERIC(15,7) default 0.00         , " +  //коэфициент ДПУ для распределения пропорционально сумме отапливаемых площадей
                  "    kf_dpu_ls   NUMERIC(15,7) default 0.00         , " +  //коэфициент ДПУ для распределения пропорционально кол-ву л/с
                  "    dlt_in      NUMERIC(15,7) default 0.00         , " +  //входящии нераспределенный расход (остаток)
                  "    dlt_cur     NUMERIC(15,7) default 0.00         , " +  //текущая дельта
                  "    dlt_reval   NUMERIC(15,7) default 0.00         , " +  //перерасчет дельты за прошлые месяцы
                  "    dlt_real_charge NUMERIC(15,7) default 0.00     , " +  //перерасчет дельты за прошлые месяцы
                  "    dlt_calc    NUMERIC(15,7) default 0.00         , " +  //распределенный (учтенный) расход
                  "    dlt_out     NUMERIC(15,7) default 0.00         , " +  //исходящии нераспределенный расход (остаток)
                  "    kod_info    integer default 0," +
                  "    sqgil       NUMERIC(15,7) default 0.00         ," +  //жилая площадь лс
                  "    is_day_calc integer not null, " +
                  "    is_use_knp integer default 0, " +
                  "    is_use_ctr integer default 0," +//Количество временно выбывших
                  "    nzp_period  integer not null, " +
                  "    cntd integer," +
                  "    cntd_mn integer, " +
                  "    nzp_measure integer, " + // ед.измерения 
                  "    norm_type_id integer, " + // id типа норматива - для нового режима введения нормативов
                  "    norm_tables_id integer, " + // id норматива - по нему можно получить набор влияющих пар-в и их знач.
                  "    val1_source NUMERIC(15,7) default 0.00 not null, " +  //val1 без учета повышающего коэффициента
                  "    val4_source NUMERIC(15,7) default 0.00 not null, " +  //val4 без учета повышающего коэффициента
                  "    up_kf NUMERIC(15,7) default 1.00 not null " +   //повышающий коэффициент для нормативного расхода
                  " ) ");

            // только открытые лс и выборка услуг
            TempTablesLifeTime.AddTempTable("ttt_cnt_uni", " Create temp table ttt_cnt_uni " +
                " ( nzp_kvar integer," +
                "   nzp_dom  integer," +
                "   nzp_serv integer, " +
                "   nzp_measure integer, " +
                "   is_day_calc integer not null," +
                "   is_use_knp integer default 0, " +
                "   is_use_ctr integer default 0" +//Количество временно выбывших
                " )  ");

            // только открытые лс и выборка услуг
            BillingInstrumentary.ExecSQL(" Insert into ttt_cnt_uni (nzp_kvar,nzp_dom,nzp_serv,nzp_measure,is_day_calc,is_use_knp,is_use_ctr) " +
                " Select k.nzp_kvar, k.nzp_dom, t.nzp_serv,nzp_measure,max(k.is_day_calc),0 as is_use_knp,1 as is_use_ctr" +
                " From " + TargetTable + " t, t_opn k, " + rashod.paramcalc.pref + "s_counts s " +
                " Where  " +
                "   k.nzp_kvar = t.nzp_kvar and t.nzp_serv=s.nzp_serv " +
                "   and t." + rashod.where_dom + rashod.where_kvarK +
                " Group by 1,2,3,4 "
                , true);


            BillingInstrumentary.ExecSQL(" Create index ix_ttt_cnt_uni on ttt_cnt_uni (nzp_kvar) ", true);


            // только открытые лс и выборка услуг
            BillingInstrumentary.ExecSQL(
                " Insert into " + TableNormatives +
                " (nzp_kvar,nzp_dom,nzp_serv,is_day_calc,nzp_period,dat_s,dat_po,cntd,cntd_mn, stek,nzp_type,mmnog,dat_charge,nzp_measure,is_use_knp,is_use_ctr) " +
                " Select t.nzp_kvar, t.nzp_dom, t.nzp_serv, t.is_day_calc," +
                " k.nzp_period,k.dp,k.dp_end,k.cntd,k.cntd_mn," +
                " 10,3,-100, null::date as dat_charge,t.nzp_measure,t.is_use_knp,t.is_use_ctr" +
                " From ttt_cnt_uni t,t_gku_periods k " +
                " Where t.nzp_kvar = k.nzp_kvar "
                , true);

            BillingInstrumentary.ExecSQL(" Create unique index ix_aid00_sq on " + TableNormatives + " (nzp_cntx) ", true);
            BillingInstrumentary.ExecSQL(" Create        index ix_aid11_sq on " + TableNormatives + " (nzp_kvar,nzp_serv,kod_info) ", true);
            BillingInstrumentary.ExecSQL(" Create        index ix_aid22_sq on " + TableNormatives + " (nzp_dom,nzp_serv) ", true);
            BillingInstrumentary.ExecSQL(" Create        index ix_aid33_sq on " + TableNormatives + " (nzp_serv,cnt2) ", true);
            BillingInstrumentary.ExecSQL(" Create        index ix_aid44_sq on " + TableNormatives + " (nzp_kvar,nzp_serv,dat_s,dat_po) ", true);
            BillingInstrumentary.ExecSQL(" Create        index ix_aid55_sq on " + TableNormatives + " (nzp_kvar,nzp_serv,stek,cnt_stage) ", true);
            BillingInstrumentary.ExecSQL(" Create        index ix_aid66_sq on " + TableNormatives + " (nzp_kvar,nzp_prm_squ2) ", true);

            TempTablesLifeTime.DropTempTable("ttt_cnt_uni ", false);

            return TableNormatives;
        }

        /// <summary>
        /// Получить целочисленное значения параметра для нормативов
        /// </summary>
        /// <param name="conn_db"></param>
        /// <param name="pDataAls"></param>
        /// <param name="pNzpPrm"></param>
        /// <param name="pNumPrm"></param>
        /// <param name="pDatS"></param>
        /// <param name="pDatPo"></param>
        /// <returns></returns>
        public int LoadIntValPrmForNorm(string pDataAls, int pNzpPrm, string pNumPrm, string pDatS, string pDatPo)
        //--------------------------------------------------------------------------------
        {
            var sql = " SELECT MAX(val_prm::int) val_prm FROM " + pDataAls + "prm_" + pNumPrm.Trim() +
                      " WHERE nzp_prm = " + pNzpPrm + " AND is_actual <> 100 AND dat_s  <= " + pDatPo +
                      " AND dat_po >= " + pDatS + " ";
            return BillingInstrumentary.ExecScalar<int>(sql);
        }


        /// <summary>
        /// Проверка наличия логического параметра в нужной базе, нужной таблице prm_5/10, по дате, значению параметра
        /// </summary>
        /// <param name="conn_db"></param>
        /// <param name="pDataAls"></param>
        /// <param name="pNzpPrm"></param>
        /// <param name="pNumPrm"></param>
        /// <param name="pValPrm"></param>
        /// <param name="pDatS"></param>
        /// <param name="pDatPo"></param>
        /// <returns></returns>
        public bool CheckBoolValPrmWithVal(string pDataAls, int pNzpPrm, string pNumPrm, string pValPrm, string pDatS, string pDatPo)
        //--------------------------------------------------------------------------------
        {
            var sql = " SELECT COUNT(1)>0 cnt FROM " + pDataAls + "prm_" + pNumPrm.Trim() + " p " +
                      " WHERE p.nzp_prm = " + pNzpPrm + " AND p.val_prm='" + pValPrm.Trim() + "' " +
                      " AND p.is_actual <> 100 AND p.dat_s  <= " + pDatPo + " AND p.dat_po >= " + pDatS + " ";
            return BillingInstrumentary.ExecScalar<bool>(sql);
        }

    }

}
