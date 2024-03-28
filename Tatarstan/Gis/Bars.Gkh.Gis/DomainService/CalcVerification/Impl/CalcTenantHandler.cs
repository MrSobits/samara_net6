using Bars.Gkh.Gis.Entities.CalcVerification;

namespace Bars.Gkh.Gis.DomainService.CalcVerification.Impl
{
    using System;
    using System.Data;
    using B4;

    using Bars.Gkh.Gis.KP_legacy;

    using Castle.Windsor;
    using Intf;

    public class CalcTenantHandler : ICalcTenant
    {
        protected IDbConnection Connection;
        protected IWindsorContainer Container;
        protected BillingInstrumentary BillingInstrumentary;
        protected TempTablesLifeTime TempTablesLifeTime;
        public CalcTenantHandler(IDbConnection connection, IWindsorContainer container, BillingInstrumentary billingInstrumentary)
        {
            Connection = connection;
            Container = container;
            BillingInstrumentary = billingInstrumentary;
            TempTablesLifeTime = Container.Resolve<TempTablesLifeTime>();
        }

        public IDataResult CalcTenant(ref CalcTypes.ParamCalc Params)
        {
            var gilec = new DbCalcCharge.GilecXX(Params);

            //ограничение на дату расчета: текущий = dat_charge is null, иначе dat_charge = dateCalc
            var dateChargeConstraint = gilec.paramcalc.b_cur
                ? " and a.dat_charge is null and b.dat_charge is null "
                : " and a.dat_charge = b.dat_charge and a.dat_charge = " +
                  DBManager.MDY(gilec.paramcalc.cur_mm, 28, gilec.paramcalc.cur_yy);

            var pDatCharge = DBManager.DateNullString;
            if (!gilec.paramcalc.b_cur)
                pDatCharge = DBManager.MDY(gilec.paramcalc.cur_mm, 28, gilec.paramcalc.cur_yy);

            CalcTenantsWithPeriodsFromPasp(gilec, pDatCharge);

            CalcTenantsInPeriodTemporaryDeparture(gilec, pDatCharge, dateChargeConstraint);

            //вычислить кол-во дней перекрытия периода убытия и занести в cnt2
            CalcGilToUseGilPeriods(dateChargeConstraint, gilec.gil_xx, gilec.paramcalc.dat_s, gilec.paramcalc.dat_po,
                gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge, gilec.paramcalc.data_alias);

            //посчитать кол-во жильцов stek =3 & nzp_gil = 1
            CalcGilItogInStek(gilec, dateChargeConstraint, gilec.gil_xx, gilec.paramcalc.dat_s, gilec.paramcalc.dat_po,
                gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge, DateTime.DaysInMonth(Params.calc_yy, Params.calc_mm).ToString(), "15", "3");

            // Выборка параметров из prm_1 для учета кол-ва жильцов цифрой
            GetParamsForCalcTenants();

            SaveParamsForTenantsInStack3(gilec, pDatCharge);

            CalcTenantsOnDays(gilec, dateChargeConstraint);

            SaveParamsForTenantsInStack4(Params, gilec, pDatCharge);

            SetTenantsCount(gilec);

            return new BaseDataResult();
        }

        /// <summary>
        /// учесть кол-во жильцов - stek = 3 & 4
        /// </summary>
        /// <param name="gilec"></param>
        private void SetTenantsCount(DbCalcCharge.GilecXX gilec)
        {
            //считать по аис паспортистка kod_info = 1
            var bIsGood = BillingInstrumentary.ExecScalar<bool>(
                " Select count(*)>0 cnt From " + gilec.paramcalc.data_alias + "prm_10 p Where p.nzp_prm = 89 " +
                " and p.is_actual <> 100 and p.dat_s  <= " + gilec.paramcalc.dat_po + " and p.dat_po >= " +
                gilec.paramcalc.dat_s + " "
                , true);
            if (bIsGood)
            {
                // если установлен параметр в prm_10 - nzp_prm=89 'Разрешить подневной расчет - подключена АИС Паспортистка ЖЭУ'
                BillingInstrumentary.ExecSQL(" Update " + gilec.gil_xx + " Set kod_info = 1 " +
                        " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge + " and stek in (3,4) ", true);
            }
            else
            {
                bIsGood = BillingInstrumentary.ExecScalar<bool>(
                    " Select count(*)=0 cnt From " + gilec.paramcalc.data_alias + "prm_10 p Where p.nzp_prm = 129 " +
                    " and p.is_actual <> 100 and p.dat_s  <= " + gilec.paramcalc.dat_po + " and p.dat_po >= " +
                    gilec.paramcalc.dat_s + " "
                    , true);

                if (bIsGood)
                {
                    // если НЕ установлен параметр в prm_10 - nzp_prm=129 'Учет льгот без жильцов' и есть льготы
                    BillingInstrumentary.ExecSQL(
                        " Update " + gilec.gil_xx + " Set kod_info = 1 " +
                        " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge + " and stek in (3,4) " +
                        "   and EXISTS ( Select 1 From " + gilec.paramcalc.data_alias + "lgots l " +
                        " Where " + gilec.gil_xx + ".nzp_kvar = l.nzp_kvar and l.is_actual <> 100" +
                        " ) ", true);
                }

                // если установлен параметр в prm_1 - nzp_prm= 90 'Разрешить подневной расчет для лицевого счета'
                // если установлен параметр в prm_1 - nzp_prm=130 'Считать количество жильцов по АИС Паспортистка ЖЭУ'
                BillingInstrumentary.ExecSQL(
                    " Update " + gilec.gil_xx + " Set kod_info = 1 " +
                    " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge + " and stek in (3,4) " +
                    "   and EXISTS ( Select 1 From " + gilec.paramcalc.data_alias + "prm_1 p " +
                    "   Where " + gilec.gil_xx + ".nzp_kvar = p.nzp and p.nzp_prm in (130)" + //(90,130) - for RT!
                    "   and p.is_actual <> 100 and p.dat_s  <= " + gilec.paramcalc.dat_po + " and p.dat_po >= " +
                    gilec.paramcalc.dat_s +
                    "   ) ", true);
            }

            // val4 - кол-во жильцов по данным АИС Пасспортистка - до этого - val1 ! Сохраним !
            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx + " Set val4 = val1 " +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge + " and stek in (3,4) ", true);

            // val1 - итоговое кол-во жильцов по данным АИС Пасспортистка или параметру nzp_prm=5 (val2) с учетом врем. выбытия (val5)
            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx + " Set val1 = (case when val2<val5 then 0 else val2 - val5 end), cnt2 = round(val2) " +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge + " and stek in (3,4) and kod_info <> 1 ",
                true);

            // cnt1 - итоговое целое кол-во жильцов по данным АИС Пасспортистка или параметру nzp_prm=5
            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx + " Set cnt1 = round(val1) " +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge + " and stek in (3,4) ", true);
        }

        /// <summary>
        /// загнать параметры жильцов в stek = 4 (nzp_prm: 5,131,1395 / val2,val3,val6)
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="gilec"></param>
        /// <param name="pDatCharge"></param>
        private void SaveParamsForTenantsInStack4(CalcTypes.ParamCalc Params, DbCalcCharge.GilecXX gilec, string pDatCharge)
        {
            BillingInstrumentary.ExecSQL(
                " Update ttt_aid_prm_pd " +
                " Set kod = 1 " +
                " Where exists ( Select 1 From " + gilec.gil_xx + " a " +
                " Where ttt_aid_prm_pd.nzp_kvar = a.nzp_kvar and a.stek = 4 " +
                " ) "
                , true);


            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set (val2,val3,val6) = (" +
                "(Select max(a.val5) " +
                " From ttt_aid_prm_pd a " +
                " Where " + gilec.gil_xx + ".nzp_kvar = a.nzp_kvar and a.kod = 1" +
                "   and " + gilec.gil_xx + ".dat_s <= a.dp_end and " + gilec.gil_xx + ".dat_po >= a.dp)," +
                "(Select max(a.val131) " +
                " From ttt_aid_prm_pd a " +
                " Where " + gilec.gil_xx + ".nzp_kvar = a.nzp_kvar and a.kod = 1" +
                "   and " + gilec.gil_xx + ".dat_s <= a.dp_end and " + gilec.gil_xx + ".dat_po >= a.dp), " +
                "(Select max(a.val1395) " +
                " From ttt_aid_prm_pd a " +
                " Where " + gilec.gil_xx + ".nzp_kvar = a.nzp_kvar and a.kod = 1" +
                "   and " + gilec.gil_xx + ".dat_s <= a.dp_end and " + gilec.gil_xx + ".dat_po >= a.dp) " +
                " ) " +
                " Where exists ( Select 1 From ttt_aid_prm_pd a " +
                " Where " + gilec.gil_xx + ".nzp_kvar = a.nzp_kvar and a.kod = 1" +
                "   and " + gilec.gil_xx + ".dat_s <= a.dp_end and " + gilec.gil_xx + ".dat_po >= a.dp " +
                " ) " +
                "   and " + gilec.gil_xx + "." + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and " + gilec.gil_xx + ".stek = 4 ", true);


            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set val5 = " +
                "(Select (case when max(a.val5) > max(a.val10) then max(a.val10) else max(a.val5) end) as val10 " +
                " From ttt_aid_prm_pd a " +
                " Where " + gilec.gil_xx + ".nzp_kvar = a.nzp_kvar and a.kod = 1 " +
                "   and " + gilec.gil_xx + ".dat_s <= a.dp_end and " + gilec.gil_xx + ".dat_po >= a.dp) " +
                " Where exists ( Select 1 From ttt_aid_prm_pd a " +
                " Where " + gilec.gil_xx + ".nzp_kvar = a.nzp_kvar and a.kod = 1 " +
                "   and " + gilec.gil_xx + ".dat_s <= a.dp_end and " + gilec.gil_xx + ".dat_po >= a.dp " +
                " ) " +
                "   and " + gilec.gil_xx + "." + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and " + gilec.gil_xx + ".stek = 4 " +
                "   and not exists ( Select 1 From ttt_prm_1 a " +
                " Where " + gilec.gil_xx + ".nzp_kvar = a.nzp and a.nzp_prm=130 and a.val_prm='1'" +
                " ) ", true);


            //вставить строки с kod = 0 (нет данных паспортистки, но есть жильцовые параметры)

            //посчитать кол-во жильцов stek =3 & nzp_gil = 1, кот. не было в ПАССПОРТИСТКЕ
            TempTablesLifeTime.DropTempTable("ttt_itog", false);

            TempTablesLifeTime.AddTempTable("ttt_itog",
                " CREATE TEMP TABLE ttt_itog (" +
                "   nzp_kvar  integer," +
                "   nzp_dom   integer," +
                "   stek      integer," +
                "   dat_s     date," +
                "   dat_po    date," +
                "   val2      " + DBManager.sDecimalType + "(11,7) default 0.00, " +
                "   val3      " + DBManager.sDecimalType + "(11,7) default 0.00, " +
                "   val5      " + DBManager.sDecimalType + "(11,7) default 0.00,  " +
                "   val6      " + DBManager.sDecimalType + "(11,7) default 0.00,  " +
                "   val2d     " + DBManager.sDecimalType + "(11,7) default 0.00, " +
                "   val3d     " + DBManager.sDecimalType + "(11,7) default 0.00, " +
                "   val5d     " + DBManager.sDecimalType + "(11,7) default 0.00,  " +
                "   val6d     " + DBManager.sDecimalType + "(11,7) default 0.00,  " +
                "   cntd_mn   integer " +
                " ) "
                , true);


            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_itog (nzp_kvar,nzp_dom,stek,dat_s,dat_po,val2,val3,val5,val6,cntd_mn) " +
                " Select a.nzp_kvar,a.nzp_dom,4 stek, b.dp,b.dp_end, " +
                " max(b.val5)," +
                " max(b.val131)," +
                " (case when max(b.val5) > max(b.val10) then max(b.val10) else max(b.val5) end) as val10," +
                " max(b.val1395)," +
                DateTime.DaysInMonth(Params.calc_yy, Params.calc_mm) +
                " From t_opn a, ttt_aid_prm_pd b " +
                " Where a.nzp_kvar = b.nzp_kvar and a.is_day_calc = 1 " +
                "   and exists (select 1 from ttt_aid_prm p where a.nzp_kvar = p.nzp_kvar and p.kod = 0)" +
                " Group by 1,2,3,4,5 "
                , true);


            string sDlt =
                "EXTRACT('days' from " +
                "(" +
                "(case when dat_po > " + gilec.paramcalc.dat_po + " then " + gilec.paramcalc.dat_po +
                " else dat_po + interval '1 day' end)" +
                " - " +
                "(case when dat_s  < " + gilec.paramcalc.dat_s + " then " + gilec.paramcalc.dat_s + " else dat_s end) " +
                ") " +
                " ) * 1.00 / cntd_mn ";

            BillingInstrumentary.ExecSQL(
                " Update ttt_itog " +
                " Set val2d = val2 * " + sDlt + " , val3d = val3 * " + sDlt + ", val5d = val5 * " + sDlt + ", val6d = val6 * " +
                sDlt +
                " Where 1=1 "
                , true);


            BillingInstrumentary.ExecSQL(
                " Insert into " + gilec.gil_xx +
                " ( nzp_kvar,nzp_dom,dat_charge,nzp_gil,stek, dat_s,dat_po, cnt1,cnt2,cnt3,val1,val2,val3,val5,val6 ) " +
                " Select nzp_kvar,nzp_dom," + pDatCharge + ",1 nzp_gil,4 stek, dat_s,dat_po, 0,0,0,0, val2, val3, val5, val6 " +
                " From ttt_itog "
                , true);


            BillingInstrumentary.ExecSQL(
                " Insert into " + gilec.gil_xx +
                " ( nzp_kvar,nzp_dom,dat_charge,nzp_gil,stek, dat_s,dat_po, cnt1,cnt2,cnt3,val1,val2,val3,val5,val6 ) " +
                " Select nzp_kvar,nzp_dom," + pDatCharge + ",1 nzp_gil,3 stek, " + gilec.paramcalc.dat_s + "," +
                gilec.paramcalc.dat_po + ", 0,0,0,0," +
                " sum(val2d), sum(val3d), sum(val5d), sum(val6d) " +
                " From ttt_itog " +
                " Group by 1,2 "
                , true);

            TempTablesLifeTime.DropTempTable("ttt_itog", false);
        }

        /// <summary>
        /// посчитать кол-во жильцов для по-дневного расчета - stek = 4 & nzp_gil = 1
        /// </summary>
        /// <param name="gilec"></param>
        /// <param name="st1"></param>
        private void CalcTenantsOnDays(DbCalcCharge.GilecXX gilec, string st1)
        {
            TempTablesLifeTime.DropTempTable("ttt_gils_pd ", false);

            TempTablesLifeTime.AddTempTable("ttt_gils_pd",
                " CREATE TEMP TABLE ttt_gils_pd (" +
                "   nzp_gx    serial, " +
                "   nzp_kvar  integer," +
                "   nzp_dom   integer," +
                "   nzp_gil   integer," +
                "   stek      integer," +
                "   cur_zap   integer default 0," +
                "   dat_charge date," +
                "   dat_s      date," +
                "   dat_po     date," +
                "   nzp_period integer," +
                "   dp         date," +
                "   dp_end     date," +
                "   cntd      integer," +
                "   cntd_mn   integer," +
                "   cnt1      integer," +
                "   cnt2      integer," +
                "   cnt3      integer," +
                "   val1      " + DBManager.sDecimalType + "(11,7) default 0.00, " +
                "   val2      " + DBManager.sDecimalType + "(11,7) default 0.00, " +
                "   val3      " + DBManager.sDecimalType + "(11,7) default 0.00, " +
                "   val5      " + DBManager.sDecimalType + "(11,7) default 0.00,  " +
                "   val6      " + DBManager.sDecimalType + "(11,7) default 0.00  " +
                " ) " + DBManager.sUnlogTempTable
                , true);


            //поскольку уже при вставке в stek=2 проверяется, что временое выбытие > 5 дней, то второй раз проверять вредно!
            // st2 = " (case when cnt1 >= 15 then cnt1 else 0 end) - (case when cnt2>=0 then cnt2 else 0 end )"; //сколько прожил дней с учетом врем. выбытия (м.б.<0)
            // stv = " (case when cnt2>=0 then cnt2 else 0 end )"; //сколько дней врем. выбытия

            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_gils_pd (nzp_kvar,nzp_dom,nzp_gil,dat_charge,dat_s,dat_po,nzp_period,dp,dp_end,cntd,cntd_mn,cnt1,cnt2,cnt3,val1,val2,val3,val5,val6,stek) " +
                " Select " + DBManager.sUniqueWord +
                " g.nzp_kvar, g.nzp_dom, g.nzp_gil, g.dat_charge, g.dat_s, g.dat_po, p.nzp_period, p.dp, p.dp_end, p.cntd, p.cntd_mn, " +
                " 0 as cnt1, " + // кол-во жильцов с учетом врем. выбывших
                " 0 as cnt2, " + // кол-во жителей без врем. убытия
                " 0 as cnt3, " + // сколько прожил дней с учетом врем. выбытия (м.б.<0)
                " 0 as val1, " + // доля бытия в месяце
                " 0 as val2, " + // кол-во жильцов по параметру 5
                " 0 as val3, " + // кол-во верм.выбывших по параметру 10
                " 0 as val5, " + // доля врем. выбытия в месяце
                " 0 as val6, " +
                " 1 stek " +
                " From " + gilec.gil_xx + " g, t_opn k,t_gku_periods p " +
                " Where g.nzp_kvar=k.nzp_kvar and k.nzp_kvar=p.nzp_kvar and k." + gilec.paramcalc.where_z +
                gilec.paramcalc.per_dat_charge +
                "   and g.stek = 1 and k.is_day_calc = 1 " +
                "   and g.cur_zap <> -1 "
                , true);


            //кол-во дней бытия в месяце
            BillingInstrumentary.ExecSQL(
                " Update ttt_gils_pd" +
                " Set cnt1 = " +
                " EXTRACT('days' from" +
                " (case when dat_po > dp_end then dp_end + interval '1 day' " +
                " else dat_po+ interval '1 day' end) - " +
                " (case when dat_s < dp then dp else dat_s end) )  " +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and dat_po >= " + gilec.paramcalc.dat_s +
                "   and dat_s  <= " + gilec.paramcalc.dat_po
                , true);


            CalcTenantsWithPeriodsDepartureOnDays(gilec, st1, "1=1");


            string sCntDays = "EXTRACT('days' from(dp_end + interval '1 day' - dp))";

            //посчитать кол-во жильцов stek =4 & nzp_gil = 1
            CalcGilItogInStek(gilec, st1, "ttt_gils_pd", "dp", "dp_end",
                gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge, sCntDays, "1", "4");
        }

        /// <summary>
        /// загнать параметры жильцов в stek = 3 (nzp_prm: 5,131,1395 / val2,val3,val6)
        /// </summary>
        /// <param name="gilec"></param>
        /// <param name="pDatCharge"></param>
        private void SaveParamsForTenantsInStack3(DbCalcCharge.GilecXX gilec, string pDatCharge)
        {
            BillingInstrumentary.ExecSQL(
                " Update ttt_aid_prm " +
                " Set kod = 1 " +
                " Where exists ( Select 1 From " + gilec.gil_xx + " a " +
                " Where ttt_aid_prm.nzp_kvar = a.nzp_kvar and a.stek = 3 " +
                " ) "
                , true);


            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set" +
                " val2 = a.val5," +
                " val3 = a.val131, " +
                " val6 = a.val1395 " +
                " From ttt_aid_prm a " +
                " Where " + gilec.gil_xx + "." + gilec.paramcalc.where_z +
                "   and " + gilec.gil_xx + ".stek = 3 " +
                "   and " + gilec.gil_xx + ".nzp_kvar = a.nzp_kvar and a.kod = 1 ", true);


            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set" +
                " val5 = a.val10 " +
                " From ttt_aid_prm a Where " + gilec.gil_xx + ".nzp_kvar = a.nzp_kvar and a.kod = 1 " +
                "   and " + gilec.gil_xx + "." + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and " + gilec.gil_xx + ".stek = 3 " +
                "   and not exists ( Select 1 From ttt_prm_1 a " +
                " Where " + gilec.gil_xx + ".nzp_kvar = a.nzp and a.nzp_prm=130 and a.val_prm='1'" +
                " ) ", true);


            //вставить строки с kod = 0 (нет данных паспортистки, но есть жильцовые параметры)
            BillingInstrumentary.ExecSQL(
                " Insert into " + gilec.gil_xx +
                " ( nzp_kvar,nzp_dom,dat_charge,nzp_gil,stek, dat_s,dat_po, cnt1,cnt2,cnt3,val1,val2,val3,val5,val6 ) " +
                " Select a.nzp_kvar,a.nzp_dom," + pDatCharge + ",0 nzp_gil,3 stek, " + gilec.paramcalc.dat_s + "," +
                gilec.paramcalc.dat_po + ", 0,0,0,0, " +
                " max(b.val5)," +
                " max(b.val131)," +
                " (case when max(b.val5) > max(b.val10) then max(b.val10) else max(b.val5) end) as val10, " +
                " max(b.val1395) " +
                " From t_opn a, ttt_aid_prm b " +
                " Where a.nzp_kvar = b.nzp_kvar " +
                "   and b.kod = 0 and a.is_day_calc=0 " +
                " Group by 1,2 "
                , true);
        }

        /// <summary>
        /// заполнить stek = 2 периодами врем.выбытия
        /// </summary>
        /// <param name="gilec"></param>
        /// <param name="pDatCharge"></param>
        /// <param name="st1"></param>
        private void CalcTenantsInPeriodTemporaryDeparture(DbCalcCharge.GilecXX gilec, string pDatCharge, string st1)
        {
            //загнать gil_periods в stek = 2
            BillingInstrumentary.ExecSQL(
                " Insert into " + gilec.gil_xx + " ( nzp_kvar,nzp_dom,dat_charge,nzp_gil, dat_s,dat_po, stek, cnt1 ) " +
                " Select " + DBManager.sUniqueWord + " k.nzp_kvar,k.nzp_dom, " + pDatCharge +
                ", nzp_gilec, g.dat_s, g.dat_po, 2, " +
                " EXTRACT('days' from  (case when g.dat_po > " + gilec.paramcalc.dat_po +
                " then " + gilec.paramcalc.dat_po + " +interval '1 day' " +
                " else g.dat_po+interval '1 day' end) - (case when g.dat_s < " + gilec.paramcalc.dat_s +
                " then " + gilec.paramcalc.dat_s + " else g.dat_s end)  ) " +
                " From " + gilec.paramcalc.data_alias + "gil_periods g, t_opn k " +
                " Where g.nzp_kvar = k.nzp_kvar " +
                "   and g.is_actual <> 100 " +
                "   and g.dat_s  <= " + gilec.paramcalc.dat_po +
                "   and g.dat_po >= " + gilec.paramcalc.dat_s +
                "   and g.dat_po + 1 - g.dat_s > 5 " //где убыл не менее 6 дней
                , true);

            //сначала объединим пересекающиеся интервалы gil_periods
            TempTablesLifeTime.DropTempTable("ttt_aid_uni ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_uni",
                " CREATE TEMP TABLE ttt_aid_uni (" +
                "   nzp_kvar  integer," +
                "   nzp_dom   integer," +
                "   nzp_gil   integer," +
                "   stek      integer," +
                "   dat_charge date," +
                "   dat_s      date," +
                "   dat_po     date " +
                " ) "
                , true);

            //#optimized
            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_aid_uni (nzp_kvar,nzp_dom,nzp_gil,stek,dat_charge,dat_s,dat_po) " +
                " Select a.nzp_kvar, a.nzp_dom, a.nzp_gil, a.stek, a.dat_charge, min(a.dat_s) as dat_s, max(a.dat_po) as dat_po " +
                " From " + gilec.gil_xx + " a, " + gilec.gil_xx + " b, t_selkvar t " +
                " Where a.nzp_kvar = b.nzp_kvar " +
                "   and a.nzp_kvar= t.nzp_kvar" +
                "   and a.nzp_gil  = b.nzp_gil " +
                "   and a.stek     = b.stek " +
                "   and a.nzp_gx <>  b.nzp_gx " +
                "   and a.stek     = 2 " +
                "   and a.dat_s  <= b.dat_po " +
                "   and a.dat_po >= b.dat_s  " +
                st1 +
                " Group by 1,2,3,4,5 "
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_uni1 on ttt_aid_uni (nzp_kvar, nzp_gil) ", true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_uni2 on ttt_aid_uni (nzp_dom) ", true);

            //удалим измененные строки (пока скинем в архив для отладки)
            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set cur_zap = -1 " +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and EXISTS ( Select 1 From ttt_aid_uni b " +
                " Where " + gilec.gil_xx + ".nzp_kvar = b.nzp_kvar " +
                "   and " + gilec.gil_xx + ".nzp_gil  = b.nzp_gil " +
                "   and " + gilec.gil_xx + ".stek     = b.stek " +
                " ) "
                , true);

            //и введем измененную строку
            BillingInstrumentary.ExecSQL(
                " Insert into " + gilec.gil_xx + " ( nzp_kvar,nzp_dom,dat_charge,nzp_gil,stek, dat_s,dat_po, cur_zap,cnt1 ) " +
                " Select nzp_kvar,nzp_dom,dat_charge,nzp_gil,stek, dat_s,dat_po, 1 , " +
                "EXTRACT('days' from " +
                "(" +
                "(case when dat_po > " + gilec.paramcalc.dat_po + " then " + gilec.paramcalc.dat_po +
                " else dat_po + interval '1 day' end)" +
                " - " +
                "(case when dat_s  < " + gilec.paramcalc.dat_s + " then " + gilec.paramcalc.dat_s + " else dat_s end) " +
                ") " +
                " ) " +
                " From ttt_aid_uni "
                , true);

            TempTablesLifeTime.DropTempTable("ttt_aid_uni ", false);
        }

        /// <summary>
        /// заполнить stek = 1 периодами проживания каждого жильца без учета врем.выбытия и значения параметра 5
        /// </summary>
        /// <param name="gilec"></param>
        /// <param name="pDatCharge"></param>
        private void CalcTenantsWithPeriodsFromPasp(DbCalcCharge.GilecXX gilec, string pDatCharge)
        {
            //выбрать все карточки по дому
            TempTablesLifeTime.DropTempTable("ttt_aid_gx ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_gx",
                " CREATE TEMP TABLE ttt_aid_gx (" +
                "   nzp_kvar  integer," +
                "   nzp_dom   integer," +
                "   nzp_gil   integer," +
                "   nzp_tkrt  integer," +
                "   dat_ofor  date," +
                "   dat_oprp  date " +
                " ) "
                , true);

            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_aid_gx (nzp_kvar,nzp_dom,nzp_gil,nzp_tkrt,dat_ofor,dat_oprp) " +
                " Select " + DBManager.sUniqueWord + " k.nzp_kvar,k.nzp_dom, g.nzp_gil, g.nzp_tkrt, " +
                DBManager.sNvlWord + "(g.dat_ofor, " + DBManager.MDY(1, 1, 1901) + ") as dat_ofor, g.dat_oprp " +
                " From " + gilec.paramcalc.data_alias + "kart g, t_opn k " +
                " Where k.nzp_kvar = g.nzp_kvar " +
                "   and g.nzp_tkrt is not null and " + DBManager.sNvlWord + "(g.neuch,'0')<>'1' "
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_gx1 on ttt_aid_gx (nzp_kvar, nzp_gil, nzp_tkrt, dat_ofor) ", true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_gx2 on ttt_aid_gx (nzp_tkrt) ", true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_gx3 on ttt_aid_gx (nzp_gil) ", true);


            //карточки прибытия
            BillingInstrumentary.ExecSQL(
                " Insert into " + gilec.gil_xx + " ( nzp_kvar,nzp_dom,dat_charge,nzp_gil, dat_s,dat_po, stek ) " +
                " Select " + DBManager.sUniqueWord + " nzp_kvar,nzp_dom, " + pDatCharge + ", nzp_gil, dat_ofor, dat_oprp, 1  " +
                " From ttt_aid_gx g " +
                " Where nzp_tkrt <> 2 " + //прибытие
                //все даты внутри периоды и ближайший предыдущий до периода
                "   and ( dat_ofor between " + gilec.paramcalc.dat_s + " and " + gilec.paramcalc.dat_po +
                "   or dat_ofor = " +
                " ( Select max(g1.dat_ofor) From ttt_aid_gx g1 " +
                "  Where g.nzp_kvar = g1.nzp_kvar " +
                "  and g.nzp_gil  = g1.nzp_gil " +
                "  and g1.nzp_tkrt <> 2 " +
                "  and g1.dat_ofor < " + gilec.paramcalc.dat_s +
                " ) " +
                ")", true);

            //карточки убытия - ищем ближайший dat_ofor после dat_s
            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set dat_po = ( Select min(dat_ofor) From ttt_aid_gx g " +
                " Where " + gilec.gil_xx + ".nzp_kvar = g.nzp_kvar " +
                "   and " + gilec.gil_xx + ".nzp_gil  = g.nzp_gil " +
                "   and g.nzp_tkrt = 2 " +
                "   and " + gilec.gil_xx + ".dat_s < g.dat_ofor " +
                " ) " +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and EXISTS ( Select 1 From ttt_aid_gx g " +
                " Where " + gilec.gil_xx + ".nzp_kvar = g.nzp_kvar " +
                "   and " + gilec.gil_xx + ".nzp_gil  = g.nzp_gil " +
                "   and g.nzp_tkrt = 2 " +
                "   and " + gilec.gil_xx + ".dat_s < g.dat_ofor " +
                " ) ", true);

            //вставляем одинокие карточки убытия
            TempTablesLifeTime.DropTempTable("ttt_aid_ub ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_ub",
                " CREATE TEMP TABLE ttt_aid_ub (" +
                "   nzp_kvar  integer," +
                "   nzp_dom   integer," +
                "   nzp_gil   integer," +
                "   nzp_tkrt  integer," +
                "   dat_ofor  date " +
                " ) "
                , true);

            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_aid_ub (nzp_kvar,nzp_dom,nzp_gil,nzp_tkrt,dat_ofor) " +
                " Select nzp_kvar, nzp_dom, nzp_gil, nzp_tkrt, dat_ofor " +
                " From ttt_aid_gx g " +
                " Where g.nzp_tkrt = 2 " +
                "   and g.dat_ofor<= " + gilec.paramcalc.dat_po +
                //не были выбраны
                "   and NOT EXISTS ( Select 1 From " + gilec.gil_xx + " gx " +
                " Where g.nzp_kvar = gx.nzp_kvar " +
                "   and g.nzp_gil  = gx.nzp_gil  " + gilec.paramcalc.per_dat_charge +
                " ) " +
                //самую минимальную дату убытия после начала периода
                "   and dat_ofor = " +
                " ( Select min(g1.dat_ofor) From ttt_aid_gx g1 " +
                " Where g1.nzp_kvar = g.nzp_kvar " +
                "   and g1.nzp_gil  = g.nzp_gil " +
                "   and g1.nzp_tkrt = 2 " +
                "   and g1.dat_ofor >= " + gilec.paramcalc.dat_s +
                ")"
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_ub1 on ttt_aid_ub (nzp_dom) ", true);

            BillingInstrumentary.ExecSQL(
                " Insert into " + gilec.gil_xx + " ( nzp_kvar,nzp_dom,dat_charge,nzp_gil, dat_po, stek ) " +
                " Select nzp_kvar,nzp_dom, " + pDatCharge + ", nzp_gil, dat_ofor, 1  " +
                " From ttt_aid_ub Where 1=1 ", true
                );

            TempTablesLifeTime.DropTempTable("ttt_aid_ub ", false);

            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set dat_s = " + DBManager.MDY(1, 1, 1901) +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and dat_s is null ", true);

            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set dat_po = " + DBManager.sDefaultSchema + "MDY(1,1,3000) " +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and dat_po is null ", true);

            //кол-во дней бытия в месяце
            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set cnt1 = " +
                " EXTRACT('days' from" +
                " (case when dat_po > " + gilec.paramcalc.dat_po + " then " + gilec.paramcalc.dat_po + " + interval '1 day' " +
                " else dat_po+ interval '1 day' end) - " +
                " (case when dat_s < " + gilec.paramcalc.dat_s + " then " + gilec.paramcalc.dat_s + " else dat_s end) )  " +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and dat_po >= " + gilec.paramcalc.dat_s +
                "   and dat_s  <= " + gilec.paramcalc.dat_po, true);

        }

        /// <summary>
        /// вычислить кол-во дней перекрытия периода убытия и занести в cnt2
        /// </summary>
        /// <param name="st1"></param>
        /// <param name="sTabName"></param>
        /// <param name="pDatS"></param>
        /// <param name="pDatPo"></param>
        /// <param name="pWhere"></param>
        /// <param name="pDataAlias"></param>
        /// <returns></returns>
        public bool CalcGilToUseGilPeriods(string st1, string sTabName, string pDatS, string pDatPo, string pWhere, string pDataAlias)
        {
            //вычислить кол-во дней перекрытия периода убытия и занести в cnt2
            TempTablesLifeTime.DropTempTable("ttt_aid_cnt ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_cnt",
                " CREATE TEMP TABLE ttt_aid_cnt (" +
                "   nzp_kvar  integer," +
                "   nzp_gil   integer," +
                "   cnt_del2 interval hour  ) "
                , true);

            var sIntervalDay = "interval '1 day'";

            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_aid_cnt (nzp_kvar,nzp_gil,cnt_del2) " +
                " Select distinct a.nzp_kvar,a.nzp_gil, " +
                //     [---------------] a. gil_periods
                //  [-----------]        b. прибытие - урезаем
                        " case when " + ReplaceFromDate("b.dat_s", pDatS) + " <= " + ReplaceFromDate("a.dat_s", pDatS) + " and " +
                                        ReplaceEndDate("b.dat_po", pDatPo) + " < " + ReplaceEndDate("a.dat_po", pDatPo) +
                        " then (" + ReplaceEndDate("b.dat_po", pDatPo) + " + " + sIntervalDay + " - " + ReplaceFromDate("a.dat_s", pDatS) + ") " +
                        " else " +
                //  [---------------]    a.
                //        [-----------]  b.
                        " case when " + ReplaceFromDate("b.dat_s", pDatS) + " >= " + ReplaceFromDate("a.dat_s", pDatS) + " and " +
                                        ReplaceEndDate("b.dat_po", pDatPo) + " > " + ReplaceEndDate("a.dat_po", pDatPo) +
                        " then (" + ReplaceEndDate("a.dat_po", pDatPo) + " + " + sIntervalDay + " - " + ReplaceFromDate("b.dat_s", pDatS) + ") " +
                        " else " +
                //      [----------]     a.
                //   [---------------]   b.
                        " case when " + ReplaceFromDate("b.dat_s", pDatS) + " <= " + ReplaceFromDate("a.dat_s", pDatS) + " and " +
                                        ReplaceEndDate("b.dat_po", pDatPo) + " >= " + ReplaceEndDate("a.dat_po", pDatPo) +
                        " then (" + ReplaceEndDate("a.dat_po", pDatPo) + " + " + sIntervalDay + " - " + ReplaceFromDate("a.dat_s", pDatS) + ") " +
                        " else " +
                //  [---------------]    a.
                //    [----------]       b.
                             " (" + ReplaceEndDate("b.dat_po", pDatPo) + " + " + sIntervalDay + " - " + ReplaceFromDate("b.dat_s", pDatS) + ")" +
                " end end end as cnt_del2 " +
                " From " + sTabName + " a, " + sTabName + " b " +
                " Where 1 = 1 " + st1 +
                "   and a.nzp_kvar = b.nzp_kvar " +
                "   and a.nzp_gil = b.nzp_gil " +
                "   and a.stek = 2 " +
                "   and b.stek = 1 " +
                "   and a.cur_zap <> -1 " +
                "   and b.cur_zap <> -1 " +
                "   and a.dat_s <= b.dat_po " +
                "   and a.dat_po>= b.dat_s " +
                "   and a.dat_po >=" + pDatS +
                "   and b.dat_po >=" + pDatS
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_cn1 on ttt_aid_cnt (nzp_kvar, nzp_gil) ", true);

            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " ttt_aid_cnt ", true);

            //кол-во дней временного выбытия
            BillingInstrumentary.ExecSQL(
                " Update " + sTabName +
                " Set cnt2 = ( Select sum(" +
            "EXTRACT('days' from cnt_del2 ) " +
            ") From ttt_aid_cnt a " +
                             " Where " + sTabName + ".nzp_kvar = a.nzp_kvar " +
                             "   and " + sTabName + ".nzp_gil = a.nzp_gil " +
                           " ) " +
                " Where " + pWhere +
                "   and dat_po >= " + pDatS +
                "   and dat_s  <= " + pDatPo +
                "   and stek = 1 " +
                "   and EXISTS (SELECT 1 From ttt_aid_cnt a " +
                            " Where " + sTabName + ".nzp_kvar = a.nzp_kvar " +
                            "   and " + sTabName + ".nzp_gil = a.nzp_gil " +
                           " ) "
              , true);

            TempTablesLifeTime.DropTempTable("ttt_aid_cnt ", false);

            return true;
        }

        /// <summary>
        /// Рассчитать кол-во дней проживания жильцов по периодам с учетом периодов убытия
        /// </summary>
        /// <param name="gilec"></param>
        /// <param name="st1"></param>
        /// <param name="pWhere"></param>
        /// <returns></returns>
        private void CalcTenantsWithPeriodsDepartureOnDays(DbCalcCharge.GilecXX gilec, string st1, string pWhere)
        {
            //вычислить кол-во дней перекрытия периода убытия и занести в cnt2
            TempTablesLifeTime.DropTempTable("ttt_aid_cnt ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_cnt",
                " CREATE TEMP TABLE ttt_aid_cnt (" +
                "   nzp_kvar   integer," +
                "   nzp_gil    integer," +
                "   nzp_period integer," +
                 "   cnt_del2 interval hour " +
                 " ) "
                , true);

            var sIntervalDay = "interval '1 day'";

            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_aid_cnt (nzp_kvar,nzp_gil,nzp_period,cnt_del2) " +
                " Select distinct a.nzp_kvar,a.nzp_gil,b.nzp_period, " +
                //     [---------------] a. gil_periods
                //  [-----------]        b. прибытие - урезаем
                        " case when " + ReplaceFromDate("b.dat_s", "b.dp") + " <= " + ReplaceFromDate("a.dat_s", "b.dp") + " and " +
                                        ReplaceEndDate("b.dat_po", "b.dp_end") + " < " + ReplaceEndDate("a.dat_po", "b.dp_end") +
                        " then (" + ReplaceEndDate("b.dat_po", "b.dp_end") + " + " + sIntervalDay + " - " + ReplaceFromDate("a.dat_s", "b.dp") + ") " +
                        " else " +
                //  [---------------]    a.
                //        [-----------]  b.
                        " case when " + ReplaceFromDate("b.dat_s", "b.dp") + " >= " + ReplaceFromDate("a.dat_s", "b.dp") + " and " +
                                        ReplaceEndDate("b.dat_po", "b.dp_end") + " > " + ReplaceEndDate("a.dat_po", "b.dp_end") +
                        " then (" + ReplaceEndDate("a.dat_po", "b.dp_end") + " + " + sIntervalDay + " - " + ReplaceFromDate("b.dat_s", "b.dp") + ") " +
                        " else " +
                //      [----------]     a.
                //   [---------------]   b.
                        " case when " + ReplaceFromDate("b.dat_s", "b.dp") + " <= " + ReplaceFromDate("a.dat_s", "b.dp") + " and " +
                                        ReplaceEndDate("b.dat_po", "b.dp_end") + " >= " + ReplaceEndDate("a.dat_po", "b.dp_end") +
                        " then (" + ReplaceEndDate("a.dat_po", "b.dp_end") + " + " + sIntervalDay + " - " + ReplaceFromDate("a.dat_s", "b.dp") + ") " +
                        " else " +
                //  [---------------]    a.
                //    [----------]       b.
                             " (" + ReplaceEndDate("b.dat_po", "b.dp_end") + " + " + sIntervalDay + " - " + ReplaceFromDate("b.dat_s", "b.dp") + ")" +
                " end end end as cnt_del2 " +
                " From " + gilec.gil_xx + " a, ttt_gils_pd b " +
                " Where 1 = 1 " + st1 +
                "   and a.nzp_kvar = b.nzp_kvar " +
                "   and a.nzp_gil = b.nzp_gil " +
                "   and a.stek = 2 " +
                "   and b.stek = 1 " +
                "   and a.cur_zap <> -1 " +
                "   and b.cur_zap <> -1 " +
                "   and a.dat_s  <= b.dat_po " +
                "   and a.dat_po >= b.dat_s " +
                "   and a.dat_s  <= b.dp_end " +
                "   and a.dat_po >= b.dp "
                , true);
            BillingInstrumentary.ExecSQL(" Create index ix_aid_cn1 on ttt_aid_cnt (nzp_kvar, nzp_gil) ", true);


            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " ttt_aid_cnt ", true);


            //кол-во дней временного выбытия
            BillingInstrumentary.ExecSQL(
                " Update ttt_gils_pd " +
                " Set cnt2 = ( Select sum(" +
 "EXTRACT('days' from cnt_del2 ) " +
 ") From ttt_aid_cnt a " +
                             " Where ttt_gils_pd.nzp_kvar   = a.nzp_kvar " +
                             "   and ttt_gils_pd.nzp_gil    = a.nzp_gil " +
                             "   and ttt_gils_pd.nzp_period = a.nzp_period " +
                           " ) " +
                " Where " + pWhere +
                "   and stek = 1 " +
                "   and EXISTS (SELECT 1 From ttt_aid_cnt a " +
                             " Where ttt_gils_pd.nzp_kvar   = a.nzp_kvar " +
                             "   and ttt_gils_pd.nzp_gil    = a.nzp_gil " +
                             "   and ttt_gils_pd.nzp_period = a.nzp_period " +
                           " ) "
              , true);


            TempTablesLifeTime.DropTempTable("ttt_aid_cnt ", false);

        }


        string ReplaceFromDate(string dat, string pDatPeriodS)
        {
            return " (case when " + dat + " < " + pDatPeriodS + " then " + pDatPeriodS + " else " + dat + " end )";
        }
        string ReplaceEndDate(string dat, string pDatPeriodPo)
        {
            return " (case when " + dat + " > " + pDatPeriodPo + " then " + pDatPeriodPo + " else " + dat + " end )";
        }

        /// <summary>
        /// вычислить кол-во жильцов по периоду и вставить в стек
        /// </summary>
        /// <param name="gilec"></param>
        /// <param name="st1"></param>
        /// <param name="sTabName"></param>
        /// <param name="pDatS"></param>
        /// <param name="pDatPo"></param>
        /// <param name="pWhere"></param>
        /// <param name="sKolDayInPeriod"></param>
        /// <param name="sCntDaysMax"></param>
        /// <param name="sStek"></param>
        /// <returns></returns>
        public bool CalcGilItogInStek(DbCalcCharge.GilecXX gilec, string st1, string sTabName, string pDatS, string pDatPo, string pWhere,
            string sKolDayInPeriod, string sCntDaysMax, string sStek)
        {
            //посчитать кол-во жильцов stek =3 & nzp_gil = 1
            TempTablesLifeTime.DropTempTable("ttt_itog ", false);

            TempTablesLifeTime.AddTempTable("ttt_itog",
                " CREATE TEMP TABLE ttt_itog (" +
                "   nzp_kvar  integer," +
                "   nzp_dom   integer," +
                "   nzp_gil   integer," +
                "   dat_charge date," +
                "   dat_s      date," +
                "   dat_po     date," +
                "   cnt1      integer," +
                "   cnt2      integer," +
                "   cnt3      integer," +
                "   val1      " + DBManager.sDecimalType + "(11,7) default 0.00, " +
                "   val5      " + DBManager.sDecimalType + "(11,7) default 0.00  " +
                " ) " + DBManager.sUnlogTempTable
                , true);

            //string st2 = " (case when cnt1 >= 15 then cnt1 else 0 end) - (case when cnt2>=6 then cnt2 else 0 end )"; //сколько прожил дней с учетом врем. выбытия (м.б.<0)
            //string stv = " (case when cnt2>=6 then cnt2 else 0 end )"; //сколько дней врем. выбытия

            //поскольку уже при вставке в stek=2 проверяется, что временое выбытие > 5 дней, то второй раз проверять вредно!
            string st2 = " (case when cnt1 >= " + sCntDaysMax + " then cnt1 else 0 end) - (case when cnt2>=0 then cnt2 else 0 end )"; //сколько прожил дней с учетом врем. выбытия (м.б.<0)
            string stv = " (case when cnt2>=0 then cnt2 else 0 end )"; //сколько дней врем. выбытия

            //Calendar myCal = CultureInfo.InvariantCulture.Calendar;
            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_itog (nzp_kvar,nzp_dom,nzp_gil,dat_charge,dat_s,dat_po,cnt1,cnt2,cnt3,val1,val5) " +
                " Select " + DBManager.sUniqueWord + " nzp_kvar, nzp_dom, nzp_gil, dat_charge, " + pDatS + " as dat_s, " + pDatPo + " as dat_po, " +
                    " case when " + st2 + " >= " + sCntDaysMax + " then 1 else 0 end as cnt1, " +//нормативное кол-во жильцов с учетом врем. выбывших
                    " case when cnt1 >= " + sCntDaysMax + " then 1 else 0 end as cnt2, " +       //кол-во жителей без врем. убытия
                    st2 + " as cnt3, " +                                        //сколько прожил дней с учетом врем. выбытия (м.б.<0)
                    " case when " + st2 + " > 0 then (" + st2 + ") * 1.00/" + sKolDayInPeriod + " else 0 end as val1 " + //доля бытия в месяце
                    ",case when " + stv + " > 0 then (" + stv + ") * 1.00/" + sKolDayInPeriod + " else 0 end as val5 " + //доля врем. выбытия в месяце
                " From " + sTabName +
                " Where " + pWhere +
                "   and stek = 1 " +
                "   and cur_zap <> -1 "
               , true);

            BillingInstrumentary.ExecSQL(
                " Insert into " + gilec.gil_xx + " ( nzp_kvar,nzp_dom,dat_charge,nzp_gil,stek, dat_s,dat_po, cnt1,cnt2,cnt3,val1,val5 ) " +
                " Select nzp_kvar,nzp_dom,dat_charge,1 nzp_gil," + sStek + " stek, dat_s,dat_po, sum(cnt1),sum(cnt2),sum(cnt3),sum(val1),sum(val5) " +
                " From ttt_itog where 1=1  Group by 1,2,3,4,5,6,7", true);

            return true;
        }


        /// <summary>
        /// Выборка параметров из prm_1 для учета кол-ва жильцов цифрой
        /// </summary>
        /// <param name="gilec"></param>
        private void GetParamsForCalcTenants()
        {
            TempTablesLifeTime.DropTempTable("ttt_aid_prm ", false);
            TempTablesLifeTime.DropTempTable("ttt_aid_prm_pd ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_prm",
                 " CREATE TEMP TABLE ttt_aid_prm (" +
                 "   nzp_kvar  integer, " +
                 "   kod       integer default 0, " +
                 "   val5    " + DBManager.sDecimalType + "(11,7) not null default 0.00, " + // nzp_prm=5
                 "   val131  " + DBManager.sDecimalType + "(11,7) not null default 0.00, " + // nzp_prm=131
                 "   val10   " + DBManager.sDecimalType + "(11,7) not null default 0.00, " + // nzp_prm=10
                 "   val1395 " + DBManager.sDecimalType + "(11,7) not null default 0.00  " + // nzp_prm=1395
                 " ) " + DBManager.sUnlogTempTable
                 , true);

            TempTablesLifeTime.AddTempTable("ttt_aid_prm_pd",
                 " CREATE TEMP TABLE ttt_aid_prm_pd (" +
                 "   nzp_kvar  integer, " +
                 "   kod       integer default 0, " +
                 "   val5    " + DBManager.sDecimalType + "(11,7) not null default 0.00, " + // nzp_prm=5
                 "   val131  " + DBManager.sDecimalType + "(11,7) not null default 0.00, " + // nzp_prm=131
                 "   val10   " + DBManager.sDecimalType + "(11,7) not null default 0.00, " + // nzp_prm=10
                 "   val1395 " + DBManager.sDecimalType + "(11,7) not null default 0.00, " + // nzp_prm=1395
                 "   nzp_period integer, " +
                 "   dp         date," +
                 "   dp_end     date," +
                 "   cntd       integer, " +
                 "   cntd_mn    integer  " +
                 " ) " + DBManager.sUnlogTempTable
                 , true);

            BillingInstrumentary.ExecSQL(
               " Insert Into ttt_aid_prm_pd (nzp_kvar,nzp_period,dp,dp_end,cntd,cntd_mn,val5,val131,val10,val1395) " +
               " Select p.nzp as nzp_kvar, a.nzp_period, a.dp, a.dp_end, a.cntd, a.cntd_mn," +
               " max(case when a.dp<=p.dat_po and a.dp_end>=p.dat_s and p.nzp_prm=5    then p.val_prm" + DBManager.sConvToNum + " else 0 end) as val5," +
               " max(case when a.dp<=p.dat_po and a.dp_end>=p.dat_s and p.nzp_prm=131  then p.val_prm" + DBManager.sConvToNum + " else 0 end) as val131," +
               " max(case when a.dp<=p.dat_po and a.dp_end>=p.dat_s and p.nzp_prm=10   then p.val_prm" + DBManager.sConvToNum + " else 0 end) as val10," +
               " max(case when a.dp<=p.dat_po and a.dp_end>=p.dat_s and p.nzp_prm=1395 then p.val_prm" + DBManager.sConvToNum + " else 0 end) as val1395 " +
               " From ttt_prm_1d p,t_gku_periods a " +
               " Where a.nzp_kvar = p.nzp " +
               "   and p.nzp_prm in (5,131,10,1395) " +
               " Group by 1,2,3,4,5,6 "
               , true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_prm1_pd on ttt_aid_prm_pd (nzp_kvar,kod) ", true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_prm2_pd on ttt_aid_prm_pd (nzp_kvar,dp,dp_end) ", true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_prm3_pd on ttt_aid_prm_pd (nzp_period) ", true);

            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " ttt_aid_prm_pd ", true);

            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_aid_prm (nzp_kvar,val5,val131,val10,val1395) " +
                " Select p.nzp as nzp_kvar," +
                " max(case when p.nzp_prm=5    then p.val_prm" + DBManager.sConvToNum + " else 0 end) as val5," +
                " max(case when p.nzp_prm=131  then p.val_prm" + DBManager.sConvToNum + " else 0 end) as val131," +
                " max(case when p.nzp_prm=10   then p.val_prm" + DBManager.sConvToNum + " else 0 end) as val10," +
                " max(case when p.nzp_prm=1395 then p.val_prm" + DBManager.sConvToNum + " else 0 end) as val1395 " +
                " From ttt_prm_1d p,t_opn a " +
                " Where a.nzp_kvar = p.nzp " +
                "   and p.nzp_prm in (5,131,10,1395) " +
                " Group by 1 "
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_prm1 on ttt_aid_prm (nzp_kvar, kod) ", true);

            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " ttt_aid_prm ", true);

        }

    }

    public class CalcTenantHandlerOverride : ICalcTenant
    {
        protected IDbConnection Connection;
        protected IWindsorContainer Container;
        protected BillingInstrumentary BillingInstrumentary;
        protected TempTablesLifeTime TempTablesLifeTime;
        public CalcTenantHandlerOverride(IDbConnection connection, IWindsorContainer container, BillingInstrumentary billingInstrumentary)
        {
            Connection = connection;
            Container = container;
            BillingInstrumentary = billingInstrumentary;
            TempTablesLifeTime = Container.Resolve<TempTablesLifeTime>();
        }

        public IDataResult CalcTenant(ref CalcTypes.ParamCalc Params)
        {
            var gilec = new DbCalcCharge.GilecXX(Params);
            gilec.gil_xx = string.Format("{0}.{1}", Params.pref, gilec.gilec_tab);

            //ограничение на дату расчета: текущий = dat_charge is null, иначе dat_charge = dateCalc
            var dateChargeConstraint = gilec.paramcalc.b_cur
                ? " and a.dat_charge is null and b.dat_charge is null "
                : " and a.dat_charge = b.dat_charge and a.dat_charge = " +
                  DBManager.MDY(gilec.paramcalc.cur_mm, 28, gilec.paramcalc.cur_yy);

            var pDatCharge = DBManager.DateNullString;
            if (!gilec.paramcalc.b_cur)
                pDatCharge = DBManager.MDY(gilec.paramcalc.cur_mm, 28, gilec.paramcalc.cur_yy);

            CalcTenantsWithPeriodsFromPasp(gilec, pDatCharge);

            CalcTenantsInPeriodTemporaryDeparture(gilec, pDatCharge, dateChargeConstraint);

            //вычислить кол-во дней перекрытия периода убытия и занести в cnt2
            CalcGilToUseGilPeriods(dateChargeConstraint, gilec.gil_xx, gilec.paramcalc.dat_s, gilec.paramcalc.dat_po,
                gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge, gilec.paramcalc.data_alias);

            //посчитать кол-во жильцов stek =3 & nzp_gil = 1
            CalcGilItogInStek(gilec, dateChargeConstraint, gilec.gil_xx, gilec.paramcalc.dat_s, gilec.paramcalc.dat_po,
                gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge, DateTime.DaysInMonth(Params.calc_yy, Params.calc_mm).ToString(), "15", "3");

            // Выборка параметров из prm_1 для учета кол-ва жильцов цифрой
            GetParamsForCalcTenants();

            SaveParamsForTenantsInStack3(gilec, pDatCharge);

            CalcTenantsOnDays(gilec, dateChargeConstraint);

            SaveParamsForTenantsInStack4(Params, gilec, pDatCharge);

            SetTenantsCount(gilec);

            return new BaseDataResult();
        }

        /// <summary>
        /// учесть кол-во жильцов - stek = 3 & 4
        /// </summary>
        /// <param name="gilec"></param>
        private void SetTenantsCount(DbCalcCharge.GilecXX gilec)
        {
            //считать по аис паспортистка kod_info = 1
            var bIsGood = BillingInstrumentary.ExecScalar<bool>(
                " Select count(*)>0 cnt From " + gilec.paramcalc.data_alias + "prm_10 p Where p.nzp_prm = 89 " +
                " and p.is_actual <> 100 and p.dat_s  <= " + gilec.paramcalc.dat_po + " and p.dat_po >= " +
                gilec.paramcalc.dat_s + " "
                , true);
            if (bIsGood)
            {
                // если установлен параметр в prm_10 - nzp_prm=89 'Разрешить подневной расчет - подключена АИС Паспортистка ЖЭУ'
                BillingInstrumentary.ExecSQL(" Update " + gilec.gil_xx + " Set kod_info = 1 " +
                        " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge + " and stek in (3,4) ", true);
            }
            else
            {
                bIsGood = BillingInstrumentary.ExecScalar<bool>(
                    " Select count(*)=0 cnt From " + gilec.paramcalc.data_alias + "prm_10 p Where p.nzp_prm = 129 " +
                    " and p.is_actual <> 100 and p.dat_s  <= " + gilec.paramcalc.dat_po + " and p.dat_po >= " +
                    gilec.paramcalc.dat_s + " "
                    , true);

                if (bIsGood)
                {
                    // если НЕ установлен параметр в prm_10 - nzp_prm=129 'Учет льгот без жильцов' и есть льготы
                    BillingInstrumentary.ExecSQL(
                        " Update " + gilec.gil_xx + " Set kod_info = 1 " +
                        " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge + " and stek in (3,4) " +
                        "   and EXISTS ( Select 1 From " + gilec.paramcalc.data_alias + "lgots l " +
                        " Where " + gilec.gil_xx + ".nzp_kvar = l.nzp_kvar and l.is_actual <> 100" +
                        " ) ", true);
                }

                // если установлен параметр в prm_1 - nzp_prm= 90 'Разрешить подневной расчет для лицевого счета'
                // если установлен параметр в prm_1 - nzp_prm=130 'Считать количество жильцов по АИС Паспортистка ЖЭУ'
                BillingInstrumentary.ExecSQL(
                    " Update " + gilec.gil_xx + " Set kod_info = 1 " +
                    " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge + " and stek in (3,4) " +
                    "   and EXISTS ( Select 1 From " + gilec.paramcalc.data_alias + "prm_1 p " +
                    "   Where " + gilec.gil_xx + ".nzp_kvar = p.nzp and p.nzp_prm in (130)" + //(90,130) - for RT!
                    "   and p.is_actual <> 100 and p.dat_s  <= " + gilec.paramcalc.dat_po + " and p.dat_po >= " +
                    gilec.paramcalc.dat_s +
                    "   ) ", true);
            }

            // val4 - кол-во жильцов по данным АИС Пасспортистка - до этого - val1 ! Сохраним !
            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx + " Set val4 = val1 " +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge + " and stek in (3,4) ", true);

            // val1 - итоговое кол-во жильцов по данным АИС Пасспортистка или параметру nzp_prm=5 (val2) с учетом врем. выбытия (val5)
            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx + " Set val1 = (case when val2<val5 then 0 else val2 - val5 end), cnt2 = round(val2) " +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge + " and stek in (3,4) and kod_info <> 1 ",
                true);

            // cnt1 - итоговое целое кол-во жильцов по данным АИС Пасспортистка или параметру nzp_prm=5
            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx + " Set cnt1 = round(val1) " +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge + " and stek in (3,4) ", true);
        }

        /// <summary>
        /// загнать параметры жильцов в stek = 4 (nzp_prm: 5,131,1395 / val2,val3,val6)
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="gilec"></param>
        /// <param name="pDatCharge"></param>
        private void SaveParamsForTenantsInStack4(CalcTypes.ParamCalc Params, DbCalcCharge.GilecXX gilec, string pDatCharge)
        {
            BillingInstrumentary.ExecSQL(
                " Update ttt_aid_prm_pd " +
                " Set kod = 1 " +
                " Where exists ( Select 1 From " + gilec.gil_xx + " a " +
                " Where ttt_aid_prm_pd.nzp_kvar = a.nzp_kvar and a.stek = 4 " +
                " ) "
                , true);


            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set (val2,val3,val6) = (" +
                "(Select max(a.val5) " +
                " From ttt_aid_prm_pd a " +
                " Where " + gilec.gil_xx + ".nzp_kvar = a.nzp_kvar and a.kod = 1" +
                "   and " + gilec.gil_xx + ".dat_s <= a.dp_end and " + gilec.gil_xx + ".dat_po >= a.dp)," +
                "(Select max(a.val131) " +
                " From ttt_aid_prm_pd a " +
                " Where " + gilec.gil_xx + ".nzp_kvar = a.nzp_kvar and a.kod = 1" +
                "   and " + gilec.gil_xx + ".dat_s <= a.dp_end and " + gilec.gil_xx + ".dat_po >= a.dp), " +
                "(Select max(a.val1395) " +
                " From ttt_aid_prm_pd a " +
                " Where " + gilec.gil_xx + ".nzp_kvar = a.nzp_kvar and a.kod = 1" +
                "   and " + gilec.gil_xx + ".dat_s <= a.dp_end and " + gilec.gil_xx + ".dat_po >= a.dp) " +
                " ) " +
                " Where exists ( Select 1 From ttt_aid_prm_pd a " +
                " Where " + gilec.gil_xx + ".nzp_kvar = a.nzp_kvar and a.kod = 1" +
                "   and " + gilec.gil_xx + ".dat_s <= a.dp_end and " + gilec.gil_xx + ".dat_po >= a.dp " +
                " ) " +
                "   and " + gilec.gil_xx + "." + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and " + gilec.gil_xx + ".stek = 4 ", true);


            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set val5 = " +
                "(Select (case when max(a.val5) > max(a.val10) then max(a.val10) else max(a.val5) end) as val10 " +
                " From ttt_aid_prm_pd a " +
                " Where " + gilec.gil_xx + ".nzp_kvar = a.nzp_kvar and a.kod = 1 " +
                "   and " + gilec.gil_xx + ".dat_s <= a.dp_end and " + gilec.gil_xx + ".dat_po >= a.dp) " +
                " Where exists ( Select 1 From ttt_aid_prm_pd a " +
                " Where " + gilec.gil_xx + ".nzp_kvar = a.nzp_kvar and a.kod = 1 " +
                "   and " + gilec.gil_xx + ".dat_s <= a.dp_end and " + gilec.gil_xx + ".dat_po >= a.dp " +
                " ) " +
                "   and " + gilec.gil_xx + "." + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and " + gilec.gil_xx + ".stek = 4 " +
                "   and not exists ( Select 1 From ttt_prm_1 a " +
                " Where " + gilec.gil_xx + ".nzp_kvar = a.nzp and a.nzp_prm=130 and a.val_prm='1'" +
                " ) ", true);


            //вставить строки с kod = 0 (нет данных паспортистки, но есть жильцовые параметры)

            //посчитать кол-во жильцов stek =3 & nzp_gil = 1, кот. не было в ПАССПОРТИСТКЕ
            TempTablesLifeTime.DropTempTable("ttt_itog", false);

            TempTablesLifeTime.AddTempTable("ttt_itog",
                " CREATE TEMP TABLE ttt_itog (" +
                "   nzp_kvar  integer," +
                "   nzp_dom   integer," +
                "   stek      integer," +
                "   dat_s     date," +
                "   dat_po    date," +
                "   val2      " + DBManager.sDecimalType + "(11,7) default 0.00, " +
                "   val3      " + DBManager.sDecimalType + "(11,7) default 0.00, " +
                "   val5      " + DBManager.sDecimalType + "(11,7) default 0.00,  " +
                "   val6      " + DBManager.sDecimalType + "(11,7) default 0.00,  " +
                "   val2d     " + DBManager.sDecimalType + "(11,7) default 0.00, " +
                "   val3d     " + DBManager.sDecimalType + "(11,7) default 0.00, " +
                "   val5d     " + DBManager.sDecimalType + "(11,7) default 0.00,  " +
                "   val6d     " + DBManager.sDecimalType + "(11,7) default 0.00,  " +
                "   cntd_mn   integer " +
                " ) "
                , true);


            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_itog (nzp_kvar,nzp_dom,stek,dat_s,dat_po,val2,val3,val5,val6,cntd_mn) " +
                " Select a.nzp_kvar,a.nzp_dom,4 stek, b.dp,b.dp_end, " +
                " max(b.val5)," +
                " max(b.val131)," +
                " (case when max(b.val5) > max(b.val10) then max(b.val10) else max(b.val5) end) as val10," +
                " max(b.val1395)," +
                DateTime.DaysInMonth(Params.calc_yy, Params.calc_mm) +
                " From t_opn a, ttt_aid_prm_pd b " +
                " Where a.nzp_kvar = b.nzp_kvar and a.is_day_calc = 1 " +
                "   and exists (select 1 from ttt_aid_prm p where a.nzp_kvar = p.nzp_kvar and p.kod = 0)" +
                " Group by 1,2,3,4,5 "
                , true);


            string sDlt =
                "EXTRACT('days' from " +
                "(" +
                "(case when dat_po > " + gilec.paramcalc.dat_po + " then " + gilec.paramcalc.dat_po +
                " else dat_po + interval '1 day' end)" +
                " - " +
                "(case when dat_s  < " + gilec.paramcalc.dat_s + " then " + gilec.paramcalc.dat_s + " else dat_s end) " +
                ") " +
                " ) * 1.00 / cntd_mn ";

            BillingInstrumentary.ExecSQL(
                " Update ttt_itog " +
                " Set val2d = val2 * " + sDlt + " , val3d = val3 * " + sDlt + ", val5d = val5 * " + sDlt + ", val6d = val6 * " +
                sDlt +
                " Where 1=1 "
                , true);


            BillingInstrumentary.ExecSQL(
                " Insert into " + gilec.gil_xx +
                " ( nzp_kvar,nzp_dom,dat_charge,nzp_gil,stek, dat_s,dat_po, cnt1,cnt2,cnt3,val1,val2,val3,val5,val6 ) " +
                " Select nzp_kvar,nzp_dom," + pDatCharge + ",1 nzp_gil,4 stek, dat_s,dat_po, 0,0,0,0, val2, val3, val5, val6 " +
                " From ttt_itog "
                , true);


            BillingInstrumentary.ExecSQL(
                " Insert into " + gilec.gil_xx +
                " ( nzp_kvar,nzp_dom,dat_charge,nzp_gil,stek, dat_s,dat_po, cnt1,cnt2,cnt3,val1,val2,val3,val5,val6 ) " +
                " Select nzp_kvar,nzp_dom," + pDatCharge + ",1 nzp_gil,3 stek, " + gilec.paramcalc.dat_s + "," +
                gilec.paramcalc.dat_po + ", 0,0,0,0," +
                " sum(val2d), sum(val3d), sum(val5d), sum(val6d) " +
                " From ttt_itog " +
                " Group by 1,2 "
                , true);

            TempTablesLifeTime.DropTempTable("ttt_itog", false);
        }

        /// <summary>
        /// посчитать кол-во жильцов для по-дневного расчета - stek = 4 & nzp_gil = 1
        /// </summary>
        /// <param name="gilec"></param>
        /// <param name="st1"></param>
        private void CalcTenantsOnDays(DbCalcCharge.GilecXX gilec, string st1)
        {
            TempTablesLifeTime.DropTempTable("ttt_gils_pd ", false);

            TempTablesLifeTime.AddTempTable("ttt_gils_pd",
                " CREATE TEMP TABLE ttt_gils_pd (" +
                "   nzp_gx    serial, " +
                "   nzp_kvar  integer," +
                "   nzp_dom   integer," +
                "   nzp_gil   integer," +
                "   stek      integer," +
                "   cur_zap   integer default 0," +
                "   dat_charge date," +
                "   dat_s      date," +
                "   dat_po     date," +
                "   nzp_period integer," +
                "   dp         date," +
                "   dp_end     date," +
                "   cntd      integer," +
                "   cntd_mn   integer," +
                "   cnt1      integer," +
                "   cnt2      integer," +
                "   cnt3      integer," +
                "   val1      " + DBManager.sDecimalType + "(11,7) default 0.00, " +
                "   val2      " + DBManager.sDecimalType + "(11,7) default 0.00, " +
                "   val3      " + DBManager.sDecimalType + "(11,7) default 0.00, " +
                "   val5      " + DBManager.sDecimalType + "(11,7) default 0.00,  " +
                "   val6      " + DBManager.sDecimalType + "(11,7) default 0.00  " +
                " ) " + DBManager.sUnlogTempTable
                , true);


            //поскольку уже при вставке в stek=2 проверяется, что временое выбытие > 5 дней, то второй раз проверять вредно!
            // st2 = " (case when cnt1 >= 15 then cnt1 else 0 end) - (case when cnt2>=0 then cnt2 else 0 end )"; //сколько прожил дней с учетом врем. выбытия (м.б.<0)
            // stv = " (case when cnt2>=0 then cnt2 else 0 end )"; //сколько дней врем. выбытия

            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_gils_pd (nzp_kvar,nzp_dom,nzp_gil,dat_charge,dat_s,dat_po,nzp_period,dp,dp_end,cntd,cntd_mn,cnt1,cnt2,cnt3,val1,val2,val3,val5,val6,stek) " +
                " Select " + DBManager.sUniqueWord +
                " g.nzp_kvar, g.nzp_dom, g.nzp_gil, g.dat_charge, g.dat_s, g.dat_po, p.nzp_period, p.dp, p.dp_end, p.cntd, p.cntd_mn, " +
                " 0 as cnt1, " + // кол-во жильцов с учетом врем. выбывших
                " 0 as cnt2, " + // кол-во жителей без врем. убытия
                " 0 as cnt3, " + // сколько прожил дней с учетом врем. выбытия (м.б.<0)
                " 0 as val1, " + // доля бытия в месяце
                " 0 as val2, " + // кол-во жильцов по параметру 5
                " 0 as val3, " + // кол-во верм.выбывших по параметру 10
                " 0 as val5, " + // доля врем. выбытия в месяце
                " 0 as val6, " +
                " 1 stek " +
                " From " + gilec.gil_xx + " g, t_opn k,t_gku_periods p " +
                " Where g.nzp_kvar=k.nzp_kvar and k.nzp_kvar=p.nzp_kvar and k." + gilec.paramcalc.where_z +
                gilec.paramcalc.per_dat_charge +
                "   and g.stek = 1 and k.is_day_calc = 1 " +
                "   and g.cur_zap <> -1 "
                , true);


            //кол-во дней бытия в месяце
            BillingInstrumentary.ExecSQL(
                " Update ttt_gils_pd" +
                " Set cnt1 = " +
                " EXTRACT('days' from" +
                " (case when dat_po > dp_end then dp_end + interval '1 day' " +
                " else dat_po+ interval '1 day' end) - " +
                " (case when dat_s < dp then dp else dat_s end) )  " +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and dat_po >= " + gilec.paramcalc.dat_s +
                "   and dat_s  <= " + gilec.paramcalc.dat_po
                , true);


            CalcTenantsWithPeriodsDepartureOnDays(gilec, st1, "1=1");


            string sCntDays = "EXTRACT('days' from(dp_end + interval '1 day' - dp))";

            //посчитать кол-во жильцов stek =4 & nzp_gil = 1
            CalcGilItogInStek(gilec, st1, "ttt_gils_pd", "dp", "dp_end",
                gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge, sCntDays, "1", "4");
        }

        /// <summary>
        /// загнать параметры жильцов в stek = 3 (nzp_prm: 5,131,1395 / val2,val3,val6)
        /// </summary>
        /// <param name="gilec"></param>
        /// <param name="pDatCharge"></param>
        private void SaveParamsForTenantsInStack3(DbCalcCharge.GilecXX gilec, string pDatCharge)
        {
            BillingInstrumentary.ExecSQL(
                " Update ttt_aid_prm " +
                " Set kod = 1 " +
                " Where exists ( Select 1 From " + gilec.gil_xx + " a " +
                " Where ttt_aid_prm.nzp_kvar = a.nzp_kvar and a.stek = 3 " +
                " ) "
                , true);


            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set" +
                " val2 = a.val5," +
                " val3 = a.val131, " +
                " val6 = a.val1395 " +
                " From ttt_aid_prm a " +
                " Where " + gilec.gil_xx + "." + gilec.paramcalc.where_z +
                "   and " + gilec.gil_xx + ".stek = 3 " +
                "   and " + gilec.gil_xx + ".nzp_kvar = a.nzp_kvar and a.kod = 1 ", true);


            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set" +
                " val5 = a.val10 " +
                " From ttt_aid_prm a Where " + gilec.gil_xx + ".nzp_kvar = a.nzp_kvar and a.kod = 1 " +
                "   and " + gilec.gil_xx + "." + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and " + gilec.gil_xx + ".stek = 3 " +
                "   and not exists ( Select 1 From ttt_prm_1 a " +
                " Where " + gilec.gil_xx + ".nzp_kvar = a.nzp and a.nzp_prm=130 and a.val_prm='1'" +
                " ) ", true);


            //вставить строки с kod = 0 (нет данных паспортистки, но есть жильцовые параметры)
            BillingInstrumentary.ExecSQL(
                " Insert into " + gilec.gil_xx +
                " ( nzp_kvar,nzp_dom,dat_charge,nzp_gil,stek, dat_s,dat_po, cnt1,cnt2,cnt3,val1,val2,val3,val5,val6 ) " +
                " Select a.nzp_kvar,a.nzp_dom," + pDatCharge + ",0 nzp_gil,3 stek, " + gilec.paramcalc.dat_s + "," +
                gilec.paramcalc.dat_po + ", 0,0,0,0, " +
                " max(b.val5)," +
                " max(b.val131)," +
                " (case when max(b.val5) > max(b.val10) then max(b.val10) else max(b.val5) end) as val10, " +
                " max(b.val1395) " +
                " From t_opn a, ttt_aid_prm b " +
                " Where a.nzp_kvar = b.nzp_kvar " +
                "   and b.kod = 0 and a.is_day_calc=0 " +
                " Group by 1,2 "
                , true);
        }

        /// <summary>
        /// заполнить stek = 2 периодами врем.выбытия
        /// </summary>
        /// <param name="gilec"></param>
        /// <param name="pDatCharge"></param>
        /// <param name="st1"></param>
        private void CalcTenantsInPeriodTemporaryDeparture(DbCalcCharge.GilecXX gilec, string pDatCharge, string st1)
        {
            //загнать gil_periods в stek = 2
            BillingInstrumentary.ExecSQL(
                " Insert into " + gilec.gil_xx + " ( nzp_kvar,nzp_dom,dat_charge,nzp_gil, dat_s,dat_po, stek, cnt1 ) " +
                " Select " + DBManager.sUniqueWord + " k.nzp_kvar,k.nzp_dom, " + pDatCharge +
                ", nzp_gilec, g.dat_s, g.dat_po, 2, " +
                " EXTRACT('days' from  (case when g.dat_po > " + gilec.paramcalc.dat_po +
                " then " + gilec.paramcalc.dat_po + " +interval '1 day' " +
                " else g.dat_po+interval '1 day' end) - (case when g.dat_s < " + gilec.paramcalc.dat_s +
                " then " + gilec.paramcalc.dat_s + " else g.dat_s end)  ) " +
                " From " + gilec.paramcalc.data_alias + "gil_periods g, t_opn k " +
                " Where g.nzp_kvar = k.nzp_kvar " +
                "   and g.is_actual <> 100 " +
                "   and g.dat_s  <= " + gilec.paramcalc.dat_po +
                "   and g.dat_po >= " + gilec.paramcalc.dat_s +
                "   and g.dat_po + 1 - g.dat_s > 5 " //где убыл не менее 6 дней
                , true);

            //сначала объединим пересекающиеся интервалы gil_periods
            TempTablesLifeTime.DropTempTable("ttt_aid_uni ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_uni",
                " CREATE TEMP TABLE ttt_aid_uni (" +
                "   nzp_kvar  integer," +
                "   nzp_dom   integer," +
                "   nzp_gil   integer," +
                "   stek      integer," +
                "   dat_charge date," +
                "   dat_s      date," +
                "   dat_po     date " +
                " ) "
                , true);

            //#optimized
            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_aid_uni (nzp_kvar,nzp_dom,nzp_gil,stek,dat_charge,dat_s,dat_po) " +
                " Select a.nzp_kvar, a.nzp_dom, a.nzp_gil, a.stek, a.dat_charge, min(a.dat_s) as dat_s, max(a.dat_po) as dat_po " +
                " From " + gilec.gil_xx + " a, " + gilec.gil_xx + " b, t_selkvar t " +
                " Where a.nzp_kvar = b.nzp_kvar " +
                "   and a.nzp_kvar= t.nzp_kvar" +
                "   and a.nzp_gil  = b.nzp_gil " +
                "   and a.stek     = b.stek " +
                "   and a.nzp_gx <>  b.nzp_gx " +
                "   and a.stek     = 2 " +
                "   and a.dat_s  <= b.dat_po " +
                "   and a.dat_po >= b.dat_s  " +
                st1 +
                " Group by 1,2,3,4,5 "
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_uni1 on ttt_aid_uni (nzp_kvar, nzp_gil) ", true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_uni2 on ttt_aid_uni (nzp_dom) ", true);

            //удалим измененные строки (пока скинем в архив для отладки)
            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set cur_zap = -1 " +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and EXISTS ( Select 1 From ttt_aid_uni b " +
                " Where " + gilec.gil_xx + ".nzp_kvar = b.nzp_kvar " +
                "   and " + gilec.gil_xx + ".nzp_gil  = b.nzp_gil " +
                "   and " + gilec.gil_xx + ".stek     = b.stek " +
                " ) "
                , true);

            //и введем измененную строку
            BillingInstrumentary.ExecSQL(
                " Insert into " + gilec.gil_xx + " ( nzp_kvar,nzp_dom,dat_charge,nzp_gil,stek, dat_s,dat_po, cur_zap,cnt1 ) " +
                " Select nzp_kvar,nzp_dom,dat_charge,nzp_gil,stek, dat_s,dat_po, 1 , " +
                "EXTRACT('days' from " +
                "(" +
                "(case when dat_po > " + gilec.paramcalc.dat_po + " then " + gilec.paramcalc.dat_po +
                " else dat_po + interval '1 day' end)" +
                " - " +
                "(case when dat_s  < " + gilec.paramcalc.dat_s + " then " + gilec.paramcalc.dat_s + " else dat_s end) " +
                ") " +
                " ) " +
                " From ttt_aid_uni "
                , true);

            TempTablesLifeTime.DropTempTable("ttt_aid_uni ", false);
        }

        /// <summary>
        /// заполнить stek = 1 периодами проживания каждого жильца без учета врем.выбытия и значения параметра 5
        /// </summary>
        /// <param name="gilec"></param>
        /// <param name="pDatCharge"></param>
        private void CalcTenantsWithPeriodsFromPasp(DbCalcCharge.GilecXX gilec, string pDatCharge)
        {
            //выбрать все карточки по дому
            TempTablesLifeTime.DropTempTable("ttt_aid_gx ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_gx",
                " CREATE TEMP TABLE ttt_aid_gx (" +
                "   nzp_kvar  integer," +
                "   nzp_dom   integer," +
                "   nzp_gil   integer," +
                "   nzp_tkrt  integer," +
                "   dat_ofor  date," +
                "   dat_oprp  date " +
                " ) "
                , true);

            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_aid_gx (nzp_kvar,nzp_dom,nzp_gil,nzp_tkrt,dat_ofor,dat_oprp) " +
                " Select " + DBManager.sUniqueWord + " k.nzp_kvar,k.nzp_dom, g.nzp_gil, g.nzp_tkrt, " +
                DBManager.sNvlWord + "(g.dat_ofor, " + DBManager.MDY(1, 1, 1901) + ") as dat_ofor, g.dat_oprp " +
                " From " + gilec.paramcalc.data_alias + "kart g, t_opn k " +
                " Where k.nzp_kvar = g.nzp_kvar " +
                "   and g.nzp_tkrt is not null and " + DBManager.sNvlWord + "(g.neuch,'0')<>'1' "
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_gx1 on ttt_aid_gx (nzp_kvar, nzp_gil, nzp_tkrt, dat_ofor) ", true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_gx2 on ttt_aid_gx (nzp_tkrt) ", true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_gx3 on ttt_aid_gx (nzp_gil) ", true);


            //карточки прибытия
            BillingInstrumentary.ExecSQL(
                " Insert into " + gilec.gil_xx + " ( nzp_kvar,nzp_dom,dat_charge,nzp_gil, dat_s,dat_po, stek ) " +
                " Select " + DBManager.sUniqueWord + " nzp_kvar,nzp_dom, " + pDatCharge + ", nzp_gil, dat_ofor, dat_oprp, 1  " +
                " From ttt_aid_gx g " +
                " Where nzp_tkrt <> 2 " + //прибытие
                //все даты внутри периоды и ближайший предыдущий до периода
                "   and ( dat_ofor between " + gilec.paramcalc.dat_s + " and " + gilec.paramcalc.dat_po +
                "   or dat_ofor = " +
                " ( Select max(g1.dat_ofor) From ttt_aid_gx g1 " +
                "  Where g.nzp_kvar = g1.nzp_kvar " +
                "  and g.nzp_gil  = g1.nzp_gil " +
                "  and g1.nzp_tkrt <> 2 " +
                "  and g1.dat_ofor < " + gilec.paramcalc.dat_s +
                " ) " +
                ")", true);

            //карточки убытия - ищем ближайший dat_ofor после dat_s
            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set dat_po = ( Select min(dat_ofor) From ttt_aid_gx g " +
                " Where " + gilec.gil_xx + ".nzp_kvar = g.nzp_kvar " +
                "   and " + gilec.gil_xx + ".nzp_gil  = g.nzp_gil " +
                "   and g.nzp_tkrt = 2 " +
                "   and " + gilec.gil_xx + ".dat_s < g.dat_ofor " +
                " ) " +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and EXISTS ( Select 1 From ttt_aid_gx g " +
                " Where " + gilec.gil_xx + ".nzp_kvar = g.nzp_kvar " +
                "   and " + gilec.gil_xx + ".nzp_gil  = g.nzp_gil " +
                "   and g.nzp_tkrt = 2 " +
                "   and " + gilec.gil_xx + ".dat_s < g.dat_ofor " +
                " ) ", true);

            //вставляем одинокие карточки убытия
            TempTablesLifeTime.DropTempTable("ttt_aid_ub ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_ub",
                " CREATE TEMP TABLE ttt_aid_ub (" +
                "   nzp_kvar  integer," +
                "   nzp_dom   integer," +
                "   nzp_gil   integer," +
                "   nzp_tkrt  integer," +
                "   dat_ofor  date " +
                " ) "
                , true);

            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_aid_ub (nzp_kvar,nzp_dom,nzp_gil,nzp_tkrt,dat_ofor) " +
                " Select nzp_kvar, nzp_dom, nzp_gil, nzp_tkrt, dat_ofor " +
                " From ttt_aid_gx g " +
                " Where g.nzp_tkrt = 2 " +
                "   and g.dat_ofor<= " + gilec.paramcalc.dat_po +
                //не были выбраны
                "   and NOT EXISTS ( Select 1 From " + gilec.gil_xx + " gx " +
                " Where g.nzp_kvar = gx.nzp_kvar " +
                "   and g.nzp_gil  = gx.nzp_gil  " + gilec.paramcalc.per_dat_charge +
                " ) " +
                //самую минимальную дату убытия после начала периода
                "   and dat_ofor = " +
                " ( Select min(g1.dat_ofor) From ttt_aid_gx g1 " +
                " Where g1.nzp_kvar = g.nzp_kvar " +
                "   and g1.nzp_gil  = g.nzp_gil " +
                "   and g1.nzp_tkrt = 2 " +
                "   and g1.dat_ofor >= " + gilec.paramcalc.dat_s +
                ")"
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_ub1 on ttt_aid_ub (nzp_dom) ", true);

            BillingInstrumentary.ExecSQL(
                " Insert into " + gilec.gil_xx + " ( nzp_kvar,nzp_dom,dat_charge,nzp_gil, dat_po, stek ) " +
                " Select nzp_kvar,nzp_dom, " + pDatCharge + ", nzp_gil, dat_ofor, 1  " +
                " From ttt_aid_ub Where 1=1 ", true
                );

            TempTablesLifeTime.DropTempTable("ttt_aid_ub ", false);

            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set dat_s = " + DBManager.MDY(1, 1, 1901) +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and dat_s is null ", true);

            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set dat_po = " + DBManager.sDefaultSchema + "MDY(1,1,3000) " +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and dat_po is null ", true);

            //кол-во дней бытия в месяце
            BillingInstrumentary.ExecSQL(
                " Update " + gilec.gil_xx +
                " Set cnt1 = " +
                " EXTRACT('days' from" +
                " (case when dat_po > " + gilec.paramcalc.dat_po + " then " + gilec.paramcalc.dat_po + " + interval '1 day' " +
                " else dat_po+ interval '1 day' end) - " +
                " (case when dat_s < " + gilec.paramcalc.dat_s + " then " + gilec.paramcalc.dat_s + " else dat_s end) )  " +
                " Where " + gilec.paramcalc.where_z + gilec.paramcalc.per_dat_charge +
                "   and dat_po >= " + gilec.paramcalc.dat_s +
                "   and dat_s  <= " + gilec.paramcalc.dat_po, true);

        }

        /// <summary>
        /// вычислить кол-во дней перекрытия периода убытия и занести в cnt2
        /// </summary>
        /// <param name="st1"></param>
        /// <param name="sTabName"></param>
        /// <param name="pDatS"></param>
        /// <param name="pDatPo"></param>
        /// <param name="pWhere"></param>
        /// <param name="pDataAlias"></param>
        /// <returns></returns>
        public bool CalcGilToUseGilPeriods(string st1, string sTabName, string pDatS, string pDatPo, string pWhere, string pDataAlias)
        {
            //вычислить кол-во дней перекрытия периода убытия и занести в cnt2
            TempTablesLifeTime.DropTempTable("ttt_aid_cnt ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_cnt",
                " CREATE TEMP TABLE ttt_aid_cnt (" +
                "   nzp_kvar  integer," +
                "   nzp_gil   integer," +
                "   cnt_del2 interval hour  ) "
                , true);

            var sIntervalDay = "interval '1 day'";

            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_aid_cnt (nzp_kvar,nzp_gil,cnt_del2) " +
                " Select distinct a.nzp_kvar,a.nzp_gil, " +
                //     [---------------] a. gil_periods
                //  [-----------]        b. прибытие - урезаем
                        " case when " + ReplaceFromDate("b.dat_s", pDatS) + " <= " + ReplaceFromDate("a.dat_s", pDatS) + " and " +
                                        ReplaceEndDate("b.dat_po", pDatPo) + " < " + ReplaceEndDate("a.dat_po", pDatPo) +
                        " then (" + ReplaceEndDate("b.dat_po", pDatPo) + " + " + sIntervalDay + " - " + ReplaceFromDate("a.dat_s", pDatS) + ") " +
                        " else " +
                //  [---------------]    a.
                //        [-----------]  b.
                        " case when " + ReplaceFromDate("b.dat_s", pDatS) + " >= " + ReplaceFromDate("a.dat_s", pDatS) + " and " +
                                        ReplaceEndDate("b.dat_po", pDatPo) + " > " + ReplaceEndDate("a.dat_po", pDatPo) +
                        " then (" + ReplaceEndDate("a.dat_po", pDatPo) + " + " + sIntervalDay + " - " + ReplaceFromDate("b.dat_s", pDatS) + ") " +
                        " else " +
                //      [----------]     a.
                //   [---------------]   b.
                        " case when " + ReplaceFromDate("b.dat_s", pDatS) + " <= " + ReplaceFromDate("a.dat_s", pDatS) + " and " +
                                        ReplaceEndDate("b.dat_po", pDatPo) + " >= " + ReplaceEndDate("a.dat_po", pDatPo) +
                        " then (" + ReplaceEndDate("a.dat_po", pDatPo) + " + " + sIntervalDay + " - " + ReplaceFromDate("a.dat_s", pDatS) + ") " +
                        " else " +
                //  [---------------]    a.
                //    [----------]       b.
                             " (" + ReplaceEndDate("b.dat_po", pDatPo) + " + " + sIntervalDay + " - " + ReplaceFromDate("b.dat_s", pDatS) + ")" +
                " end end end as cnt_del2 " +
                " From " + sTabName + " a, " + sTabName + " b " +
                " Where 1 = 1 " + st1 +
                "   and a.nzp_kvar = b.nzp_kvar " +
                "   and a.nzp_gil = b.nzp_gil " +
                "   and a.stek = 2 " +
                "   and b.stek = 1 " +
                "   and a.cur_zap <> -1 " +
                "   and b.cur_zap <> -1 " +
                "   and a.dat_s <= b.dat_po " +
                "   and a.dat_po>= b.dat_s " +
                "   and a.dat_po >=" + pDatS +
                "   and b.dat_po >=" + pDatS
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_cn1 on ttt_aid_cnt (nzp_kvar, nzp_gil) ", true);

            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " ttt_aid_cnt ", true);

            //кол-во дней временного выбытия
            BillingInstrumentary.ExecSQL(
                " Update " + sTabName +
                " Set cnt2 = ( Select sum(" +
            "EXTRACT('days' from cnt_del2 ) " +
            ") From ttt_aid_cnt a " +
                             " Where " + sTabName + ".nzp_kvar = a.nzp_kvar " +
                             "   and " + sTabName + ".nzp_gil = a.nzp_gil " +
                           " ) " +
                " Where " + pWhere +
                "   and dat_po >= " + pDatS +
                "   and dat_s  <= " + pDatPo +
                "   and stek = 1 " +
                "   and EXISTS (SELECT 1 From ttt_aid_cnt a " +
                            " Where " + sTabName + ".nzp_kvar = a.nzp_kvar " +
                            "   and " + sTabName + ".nzp_gil = a.nzp_gil " +
                           " ) "
              , true);

            TempTablesLifeTime.DropTempTable("ttt_aid_cnt ", false);

            return true;
        }

        /// <summary>
        /// Рассчитать кол-во дней проживания жильцов по периодам с учетом периодов убытия
        /// </summary>
        /// <param name="gilec"></param>
        /// <param name="st1"></param>
        /// <param name="pWhere"></param>
        /// <returns></returns>
        private void CalcTenantsWithPeriodsDepartureOnDays(DbCalcCharge.GilecXX gilec, string st1, string pWhere)
        {
            //вычислить кол-во дней перекрытия периода убытия и занести в cnt2
            TempTablesLifeTime.DropTempTable("ttt_aid_cnt ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_cnt",
                " CREATE TEMP TABLE ttt_aid_cnt (" +
                "   nzp_kvar   integer," +
                "   nzp_gil    integer," +
                "   nzp_period integer," +
                 "   cnt_del2 interval hour " +
                 " ) "
                , true);

            var sIntervalDay = "interval '1 day'";

            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_aid_cnt (nzp_kvar,nzp_gil,nzp_period,cnt_del2) " +
                " Select distinct a.nzp_kvar,a.nzp_gil,b.nzp_period, " +
                //     [---------------] a. gil_periods
                //  [-----------]        b. прибытие - урезаем
                        " case when " + ReplaceFromDate("b.dat_s", "b.dp") + " <= " + ReplaceFromDate("a.dat_s", "b.dp") + " and " +
                                        ReplaceEndDate("b.dat_po", "b.dp_end") + " < " + ReplaceEndDate("a.dat_po", "b.dp_end") +
                        " then (" + ReplaceEndDate("b.dat_po", "b.dp_end") + " + " + sIntervalDay + " - " + ReplaceFromDate("a.dat_s", "b.dp") + ") " +
                        " else " +
                //  [---------------]    a.
                //        [-----------]  b.
                        " case when " + ReplaceFromDate("b.dat_s", "b.dp") + " >= " + ReplaceFromDate("a.dat_s", "b.dp") + " and " +
                                        ReplaceEndDate("b.dat_po", "b.dp_end") + " > " + ReplaceEndDate("a.dat_po", "b.dp_end") +
                        " then (" + ReplaceEndDate("a.dat_po", "b.dp_end") + " + " + sIntervalDay + " - " + ReplaceFromDate("b.dat_s", "b.dp") + ") " +
                        " else " +
                //      [----------]     a.
                //   [---------------]   b.
                        " case when " + ReplaceFromDate("b.dat_s", "b.dp") + " <= " + ReplaceFromDate("a.dat_s", "b.dp") + " and " +
                                        ReplaceEndDate("b.dat_po", "b.dp_end") + " >= " + ReplaceEndDate("a.dat_po", "b.dp_end") +
                        " then (" + ReplaceEndDate("a.dat_po", "b.dp_end") + " + " + sIntervalDay + " - " + ReplaceFromDate("a.dat_s", "b.dp") + ") " +
                        " else " +
                //  [---------------]    a.
                //    [----------]       b.
                             " (" + ReplaceEndDate("b.dat_po", "b.dp_end") + " + " + sIntervalDay + " - " + ReplaceFromDate("b.dat_s", "b.dp") + ")" +
                " end end end as cnt_del2 " +
                " From " + gilec.gil_xx + " a, ttt_gils_pd b " +
                " Where 1 = 1 " + st1 +
                "   and a.nzp_kvar = b.nzp_kvar " +
                "   and a.nzp_gil = b.nzp_gil " +
                "   and a.stek = 2 " +
                "   and b.stek = 1 " +
                "   and a.cur_zap <> -1 " +
                "   and b.cur_zap <> -1 " +
                "   and a.dat_s  <= b.dat_po " +
                "   and a.dat_po >= b.dat_s " +
                "   and a.dat_s  <= b.dp_end " +
                "   and a.dat_po >= b.dp "
                , true);
            BillingInstrumentary.ExecSQL(" Create index ix_aid_cn1 on ttt_aid_cnt (nzp_kvar, nzp_gil) ", true);


            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " ttt_aid_cnt ", true);


            //кол-во дней временного выбытия
            BillingInstrumentary.ExecSQL(
                " Update ttt_gils_pd " +
                " Set cnt2 = ( Select sum(" +
 "EXTRACT('days' from cnt_del2 ) " +
 ") From ttt_aid_cnt a " +
                             " Where ttt_gils_pd.nzp_kvar   = a.nzp_kvar " +
                             "   and ttt_gils_pd.nzp_gil    = a.nzp_gil " +
                             "   and ttt_gils_pd.nzp_period = a.nzp_period " +
                           " ) " +
                " Where " + pWhere +
                "   and stek = 1 " +
                "   and EXISTS (SELECT 1 From ttt_aid_cnt a " +
                             " Where ttt_gils_pd.nzp_kvar   = a.nzp_kvar " +
                             "   and ttt_gils_pd.nzp_gil    = a.nzp_gil " +
                             "   and ttt_gils_pd.nzp_period = a.nzp_period " +
                           " ) "
              , true);


            TempTablesLifeTime.DropTempTable("ttt_aid_cnt ", false);

        }


        string ReplaceFromDate(string dat, string pDatPeriodS)
        {
            return " (case when " + dat + " < " + pDatPeriodS + " then " + pDatPeriodS + " else " + dat + " end )";
        }
        string ReplaceEndDate(string dat, string pDatPeriodPo)
        {
            return " (case when " + dat + " > " + pDatPeriodPo + " then " + pDatPeriodPo + " else " + dat + " end )";
        }

        /// <summary>
        /// вычислить кол-во жильцов по периоду и вставить в стек
        /// </summary>
        /// <param name="gilec"></param>
        /// <param name="st1"></param>
        /// <param name="sTabName"></param>
        /// <param name="pDatS"></param>
        /// <param name="pDatPo"></param>
        /// <param name="pWhere"></param>
        /// <param name="sKolDayInPeriod"></param>
        /// <param name="sCntDaysMax"></param>
        /// <param name="sStek"></param>
        /// <returns></returns>
        public bool CalcGilItogInStek(DbCalcCharge.GilecXX gilec, string st1, string sTabName, string pDatS, string pDatPo, string pWhere,
            string sKolDayInPeriod, string sCntDaysMax, string sStek)
        {
            //посчитать кол-во жильцов stek =3 & nzp_gil = 1
            TempTablesLifeTime.DropTempTable("ttt_itog ", false);

            TempTablesLifeTime.AddTempTable("ttt_itog",
                " CREATE TEMP TABLE ttt_itog (" +
                "   nzp_kvar  integer," +
                "   nzp_dom   integer," +
                "   nzp_gil   integer," +
                "   dat_charge date," +
                "   dat_s      date," +
                "   dat_po     date," +
                "   cnt1      integer," +
                "   cnt2      integer," +
                "   cnt3      integer," +
                "   val1      " + DBManager.sDecimalType + "(11,7) default 0.00, " +
                "   val5      " + DBManager.sDecimalType + "(11,7) default 0.00  " +
                " ) " + DBManager.sUnlogTempTable
                , true);

            //string st2 = " (case when cnt1 >= 15 then cnt1 else 0 end) - (case when cnt2>=6 then cnt2 else 0 end )"; //сколько прожил дней с учетом врем. выбытия (м.б.<0)
            //string stv = " (case when cnt2>=6 then cnt2 else 0 end )"; //сколько дней врем. выбытия

            //поскольку уже при вставке в stek=2 проверяется, что временое выбытие > 5 дней, то второй раз проверять вредно!
            string st2 = " (case when cnt1 >= " + sCntDaysMax + " then cnt1 else 0 end) - (case when cnt2>=0 then cnt2 else 0 end )"; //сколько прожил дней с учетом врем. выбытия (м.б.<0)
            string stv = " (case when cnt2>=0 then cnt2 else 0 end )"; //сколько дней врем. выбытия

            //Calendar myCal = CultureInfo.InvariantCulture.Calendar;
            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_itog (nzp_kvar,nzp_dom,nzp_gil,dat_charge,dat_s,dat_po,cnt1,cnt2,cnt3,val1,val5) " +
                " Select " + DBManager.sUniqueWord + " nzp_kvar, nzp_dom, nzp_gil, dat_charge, " + pDatS + " as dat_s, " + pDatPo + " as dat_po, " +
                    " case when " + st2 + " >= " + sCntDaysMax + " then 1 else 0 end as cnt1, " +//нормативное кол-во жильцов с учетом врем. выбывших
                    " case when cnt1 >= " + sCntDaysMax + " then 1 else 0 end as cnt2, " +       //кол-во жителей без врем. убытия
                    st2 + " as cnt3, " +                                        //сколько прожил дней с учетом врем. выбытия (м.б.<0)
                    " case when " + st2 + " > 0 then (" + st2 + ") * 1.00/" + sKolDayInPeriod + " else 0 end as val1 " + //доля бытия в месяце
                    ",case when " + stv + " > 0 then (" + stv + ") * 1.00/" + sKolDayInPeriod + " else 0 end as val5 " + //доля врем. выбытия в месяце
                " From " + sTabName +
                " Where " + pWhere +
                "   and stek = 1 " +
                "   and cur_zap <> -1 "
               , true);

            BillingInstrumentary.ExecSQL(
                " Insert into " + gilec.gil_xx + " ( nzp_kvar,nzp_dom,dat_charge,nzp_gil,stek, dat_s,dat_po, cnt1,cnt2,cnt3,val1,val5 ) " +
                " Select nzp_kvar,nzp_dom,dat_charge,1 nzp_gil," + sStek + " stek, dat_s,dat_po, sum(cnt1),sum(cnt2),sum(cnt3),sum(val1),sum(val5) " +
                " From ttt_itog where 1=1  Group by 1,2,3,4,5,6,7", true);

            return true;
        }


        /// <summary>
        /// Выборка параметров из prm_1 для учета кол-ва жильцов цифрой
        /// </summary>
        /// <param name="gilec"></param>
        private void GetParamsForCalcTenants()
        {
            TempTablesLifeTime.DropTempTable("ttt_aid_prm ", false);
            TempTablesLifeTime.DropTempTable("ttt_aid_prm_pd ", false);

            TempTablesLifeTime.AddTempTable("ttt_aid_prm",
                 " CREATE TEMP TABLE ttt_aid_prm (" +
                 "   nzp_kvar  integer, " +
                 "   kod       integer default 0, " +
                 "   val5    " + DBManager.sDecimalType + "(11,7) not null default 0.00, " + // nzp_prm=5
                 "   val131  " + DBManager.sDecimalType + "(11,7) not null default 0.00, " + // nzp_prm=131
                 "   val10   " + DBManager.sDecimalType + "(11,7) not null default 0.00, " + // nzp_prm=10
                 "   val1395 " + DBManager.sDecimalType + "(11,7) not null default 0.00  " + // nzp_prm=1395
                 " ) " + DBManager.sUnlogTempTable
                 , true);

            TempTablesLifeTime.AddTempTable("ttt_aid_prm_pd",
                 " CREATE TEMP TABLE ttt_aid_prm_pd (" +
                 "   nzp_kvar  integer, " +
                 "   kod       integer default 0, " +
                 "   val5    " + DBManager.sDecimalType + "(11,7) not null default 0.00, " + // nzp_prm=5
                 "   val131  " + DBManager.sDecimalType + "(11,7) not null default 0.00, " + // nzp_prm=131
                 "   val10   " + DBManager.sDecimalType + "(11,7) not null default 0.00, " + // nzp_prm=10
                 "   val1395 " + DBManager.sDecimalType + "(11,7) not null default 0.00, " + // nzp_prm=1395
                 "   nzp_period integer, " +
                 "   dp         date," +
                 "   dp_end     date," +
                 "   cntd       integer, " +
                 "   cntd_mn    integer  " +
                 " ) " + DBManager.sUnlogTempTable
                 , true);

            BillingInstrumentary.ExecSQL(
               " Insert Into ttt_aid_prm_pd (nzp_kvar,nzp_period,dp,dp_end,cntd,cntd_mn,val5,val131,val10,val1395) " +
               " Select p.nzp as nzp_kvar, a.nzp_period, a.dp, a.dp_end, a.cntd, a.cntd_mn," +
               " max(case when a.dp<=p.dat_po and a.dp_end>=p.dat_s and p.nzp_prm=5    then p.val_prm" + DBManager.sConvToNum + " else 0 end) as val5," +
               " max(case when a.dp<=p.dat_po and a.dp_end>=p.dat_s and p.nzp_prm=131  then p.val_prm" + DBManager.sConvToNum + " else 0 end) as val131," +
               " max(case when a.dp<=p.dat_po and a.dp_end>=p.dat_s and p.nzp_prm=10   then p.val_prm" + DBManager.sConvToNum + " else 0 end) as val10," +
               " max(case when a.dp<=p.dat_po and a.dp_end>=p.dat_s and p.nzp_prm=1395 then p.val_prm" + DBManager.sConvToNum + " else 0 end) as val1395 " +
               " From ttt_prm_1d p,t_gku_periods a " +
               " Where a.nzp_kvar = p.nzp " +
               "   and p.nzp_prm in (5,131,10,1395) " +
               " Group by 1,2,3,4,5,6 "
               , true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_prm1_pd on ttt_aid_prm_pd (nzp_kvar,kod) ", true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_prm2_pd on ttt_aid_prm_pd (nzp_kvar,dp,dp_end) ", true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_prm3_pd on ttt_aid_prm_pd (nzp_period) ", true);

            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " ttt_aid_prm_pd ", true);

            BillingInstrumentary.ExecSQL(
                " Insert Into ttt_aid_prm (nzp_kvar,val5,val131,val10,val1395) " +
                " Select p.nzp as nzp_kvar," +
                " max(case when p.nzp_prm=5    then p.val_prm" + DBManager.sConvToNum + " else 0 end) as val5," +
                " max(case when p.nzp_prm=131  then p.val_prm" + DBManager.sConvToNum + " else 0 end) as val131," +
                " max(case when p.nzp_prm=10   then p.val_prm" + DBManager.sConvToNum + " else 0 end) as val10," +
                " max(case when p.nzp_prm=1395 then p.val_prm" + DBManager.sConvToNum + " else 0 end) as val1395 " +
                " From ttt_prm_1d p,t_opn a " +
                " Where a.nzp_kvar = p.nzp " +
                "   and p.nzp_prm in (5,131,10,1395) " +
                " Group by 1 "
                , true);

            BillingInstrumentary.ExecSQL(" Create index ix_aid_prm1 on ttt_aid_prm (nzp_kvar, kod) ", true);

            BillingInstrumentary.ExecSQL(DBManager.sUpdStat + " ttt_aid_prm ", true);

        }

    }
}
