namespace Bars.Gkh.Gis.KP_legacy
{
    using System;
    using System.Threading;

    public class CalcTypes
    {

        #region параметры передачи данных в расчеты
        //----------------------------------------------------------------------------------
        public struct ParamCalc //параметры передачи данных в расчеты
        //----------------------------------------------------------------------------------
        {
            public LockMonth LockMonth { get; set; }

            public void GetLockOnCalc()
            {
                if (this.MultiThreadToken == string.Empty)
                {
                    return;
                }
                while (true)
                {
                    if (this.LockMonth.CurrentMonth == this.date_s || this.b_cur)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                }
            }

            /// <summary>
            /// если токен не пустой, то расчет многопоточный
            /// </summary>
            public string MultiThreadToken;

            /// <summary>
            /// Дата начала текущего расчета (максимальная глубина перерасчета)
            /// </summary>
            public DateTime FirstMonthCalc;

            /// <summary>
            /// Получить имя таблицы, которая может быть общей для разных потоков
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
            public string GetCommonTableName(string tableName)
            {
                if (this.MultiThreadToken == string.Empty)
                {
                    return tableName;
                }
                return $"{DBManager.sUnloggedSchema}{tableName}_{this.MultiThreadToken}";
            }

            /// <summary>
            /// Получить тип для временной таблицы (TEMP или UNLOGGED в зависимости от режима расчета)
            /// </summary>
            /// <returns></returns>
            public string GetCommonTableType()
            {
                if (this.MultiThreadToken == string.Empty)
                {
                    return "TEMP";
                }
                return "UNLOGGED";
            }

            #region Условия выволнения веток расчета
            public bool ExistsCounters;//наличие приборов учета для текущей выборки за текущий месяц
            #endregion


            const string pref_Portal = "port";
            public string per_dat_charge
            {
                get
                {
                    if (this.b_cur)
                        return " and dat_charge is null ";
                    else
                        return " and dat_charge = " + DBManager.MDY(this.cur_mm, 28, this.cur_yy);
                }
            }
            public string per_peni_date_charge
            {
                get
                {
                    if (this.b_cur)
                        return " ";
                    else
                        return " and date_charge = " + DBManager.MDY(this.cur_mm, 28, this.cur_yy);
                }
            }
            public int nzp_pack
            {
                get
                {
                    if (this.b_pack)
                        return this.nzp_dom;
                    else
                        return 0;
                }
            }
            public string ol_srv;
            public string temp_table;

            public int nzp_pack_saldo;
            public int nzp_pack_ls_saldo;
            public int nzp_reestr;

            public int nzp_kvar;
            public int nzp_user;
            public int num_ls;
            public int nzp_dom;
            public int nzp_area;

            public int count_calc_months;
            /// <summary>
            /// расчетный год
            /// </summary>
            public int calc_yy;

            /// <summary>
            /// расчетный месяц
            /// </summary>
            public int calc_mm;
            public int prev_calc_yy;
            public int prev_calc_mm;

            /// <summary>
            /// текущий расчетный год
            /// </summary>
            public int cur_yy;

            /// <summary>
            /// текущий расчетный месяц
            /// </summary>
            public int cur_mm;

            public string pref;
            public int nzp_wp;

            public bool b_dom_in; // при  выбрке дома использовать in (...)
            public bool list_dom; //признак расчета по списку домов
            public bool list_dom_saldo; //признак расчета по списку домов

            public int nzp_key; //уникальный номер задачи в текущей очереди
            //----Первый и последний день расчетного месяца

            //Таблица со списком nzp_pack-ов
            public int nzp_par_pack;
            public string dat_s
            {
                get
                {
                    return DBManager.MDY(this.calc_mm, 1, this.calc_yy);
                }
            }

            public DateTime date_s => new DateTime(this.calc_yy, this.calc_mm, 1);
            public DateTime date_po => new DateTime(this.calc_yy, this.calc_mm, DateTime.DaysInMonth(this.calc_yy, this.calc_mm));

            /// <summary>
            /// Расчет по всему банку данных
            /// </summary>
            public bool DbMode => this.nzp_kvar == 0 && this.nzp_dom == 0 && !this.list_dom;

            public string next_dat_s
            {
                get
                {
                    if (this.calc_mm + 1 == 13)
                        return DBManager.MDY(1, 1, this.calc_yy + 1);
                    else
                        return DBManager.MDY(this.calc_mm + 1, 1, this.calc_yy);
                }
            }
            public string dat_po
            {
                get
                {
                    return DBManager.MDY(this.calc_mm, DateTime.DaysInMonth(this.calc_yy, this.calc_mm), this.calc_yy);
                }
            }
            public string portal_dat_charge
            {
                get
                {
                    if (this.isPortal)
                        return DBManager.MDY(this.cur_mm, 1, this.calc_yy);
                    else
                    {
                        //в текущем расчете д.б. предыдущий месяц
                        if (this.cur_mm - 1 == 0)
                            return DBManager.MDY(1, 1, this.calc_yy - 1);
                        else
                            return DBManager.MDY(this.cur_mm - 1, 1, this.calc_yy);
                    }
                }
            }

            public bool isOneLs
            {
                get { return this.nzp_kvar > 0; }
            }

            //----Признак выборки
            public string where_z
            {
                get
                {
                    if (this.nzp_kvar > 0)
                    {
                        return "nzp_kvar = " + this.nzp_kvar;
                    }
                    if (this.list_dom) //расчет по списку домов ИЛИ расчет по домов, где есть ЛС требующим расчета
                    {
                        var tableName = DBManager.sDefaultSchema + "list_houses_for_calc";
                        return "nzp_dom in (select nzp_dom from " + tableName +
                            " where nzp_wp=" + this.nzp_wp + " and nzp_key="
                            + this.nzp_key + " and nzp_user=" + this.nzp_user + ")";
                    }
                    if (this.list_dom_saldo) //расчет сальдо по списку домов
                    {
                        var tableName = DBManager.sDefaultSchema + "t" + this.nzp_user + "_eqv_list_dom";
                        return "nzp_dom in (select distinct nzp_dom from " + tableName + ")";
                    }
                    if (this.nzp_dom > 0 && !this.b_pack) //поскольку при b_pack=true в nzp_dom лежит nzp_pack !
                    {
                        if (this.b_dom_in)
                            return "nzp_dom in (select nzp_dom from t_selkvar) ";
                        else
                            return "nzp_dom = " + this.nzp_dom;
                    }
                    if (this.nzp_area > 0)
                    {
                        return "nzp_area > 0";
                    }

                    return "nzp_dom > 0 ";
                }
            }


            //тип таблицы
            //pref_id_bill - пустой, тогда это текущий charge
            // == "port" - порталовский charge_xx
            public int id_bill; //номер записи, для Портала решил, что будет сохраняться только последний расчет в rashod_xx etc.
            public string id_bill_pref; //port - это Портал

            public bool b_again //признак параллельной таблицы:  не будем считать домовые расходы, используем для Портала или других целей
            {
                get
                {
                    return (this.id_bill_pref != "" || this.id_bill > 0);
                }
            }
            public string alias_again //alias таблицы (для reval_xx & lnk_charge_xx)
            {
                get
                {
                    string s = ""; //будет charge_04

                    if (this.b_again)
                    {
                        //s = id_bill.ToString() + id_bill_pref; //будет charge1p_04
                        s = "_" + this.id_bill_pref; //будет charge_port_04, nedo_port_04
                    }

                    return s;
                }
            }
            public string alias //полный alias таблицы
            {
                get
                {
                    string s = ""; //будет charge_04

                    if (this.b_reval || this.b_handl)
                    {
                        //перерасчетный месяц
                        //будет порталовские charge_port1206_04, nedo_port1206_04 или обычный charge1206_04, nedo1206_04
                        s = (this.cur_yy - 2000).ToString() + this.cur_mm.ToString("00");
                    }

                    return this.alias_again + s;
                }
            }
            public string kernel_alias
            {
                get
                {
#if PG
                    string s = pref.Trim() + "_kernel.";
#else
                    string s = this.pref.Trim() + "_kernel:";
#endif
                    return s;
                }
            }
            public string data_alias
            {
                get
                {
#if PG
                    string s = pref.Trim() + "_data.";
#else
                    string s = this.pref.Trim() + "_data:";
#endif
                    return s;
                }
            }
            public bool isPortal //признак вызова расчета из Портала (billpro)
            {
                get
                {
                    return (this.id_bill_pref == ParamCalc.pref_Portal);
                }
            }

            //----Признак текущего месяца
            public bool b_cur
            {
                get
                {
                    return (this.calc_yy == this.cur_yy && this.calc_mm == this.cur_mm);
                }
            }
            public bool b_data; //загрузить charge_cnts при первом расчете

            public bool b_loadtemp; //препарировать таблицы

            //0 - CalcGilXX      101
            //1 - CalcRashod     111
            //2 - CalcNedo       121
            //3 - CalcGkuXX      131
            //4 - CalcChargeXX   141
            //5 - CalcReportXX   200
            //6 - again - заново пересчитать пред. месяц с текущими изменениями (charge2_xx)
            //7 - reval - запись в перерасчетные таблицы
            //8 - must  - учесть must_calc при выборке лицевых счетов 

            //сделать через геттеры по таску!!!
            public bool b_gil;
            public bool b_rashod;
            public bool b_nedo;
            public bool b_gku;
            public bool b_charge;
            public bool b_report;
            public bool b_reval;
            public bool b_must;
            public bool b_pack, b_packOt, b_packDel;
            public bool b_handl;
            public bool benefits_norms; //учитывать нормативы по льготам
            public bool b_peni;

            public bool b_refresh;

            public DateTime DateOper;
            public string dat_oper
            {
                get
                {
                    return "'" + this.DateOper.ToShortDateString() + "'";
                }
            }
            public string between_dat_oper
            {
                get
                {
                    int y = this.DateOper.Year;
                    int m = this.DateOper.Month;
                    int days = DateTime.DaysInMonth(y, m);

                    DateTime d1 = new DateTime(y, m, 1);
                    DateTime d2 = new DateTime(y, m, days);
                    return "between '" + d1.ToShortDateString() + "' and '" + d2.ToShortDateString() + "' ";
                }
            }


            public DateTime curd;
            public CalcFonTask calcfon;

            public ParamCalc(int _nzp_kvar, int _nzp_dom_or_pack, string _pref, int _calc_yy, int _calc_mm, int _cur_yy, int _cur_mm)
            {
                this.calcfon = new CalcFonTask(0);

                this.nzp_par_pack = 0;
                this.id_bill_pref = "";
                this.temp_table = "";
                this.id_bill = 0;
                this.nzp_reestr = 0;

                this.b_gil = true;
                this.b_rashod = true;
                this.b_nedo = true;
                this.b_gku = true;
                this.b_charge = true;
                this.b_report = true;
                this.b_peni = true;

                this.b_data = false;
                this.b_reval = false;
                this.b_must = false;
                this.b_handl = false;

                this.b_pack = false;
                this.b_packOt = false;
                this.b_packDel = false;
                this.b_refresh = false;
                this.b_dom_in = false; // при  выбрке дома использовать in (...)

                this.list_dom = false;
                this.list_dom_saldo = false;
                this.curd = DateTime.Now;

                this.calc_yy = _calc_yy;
                this.calc_mm = _calc_mm;
                this.cur_yy = _cur_yy;
                this.cur_mm = _cur_mm;

                if (this.cur_yy == 0)
                {
                    this.cur_yy = Points.CalcMonth.year_;
                    this.cur_mm = Points.CalcMonth.month_;
                }

                this.prev_calc_yy = this.calc_yy;
                this.prev_calc_mm = this.calc_mm - 1;
                if (this.prev_calc_mm == 0)
                {
                    this.prev_calc_yy = this.calc_yy - 1;
                    this.prev_calc_mm = 12;
                }

                this.nzp_dom = _nzp_dom_or_pack;
                this.nzp_kvar = _nzp_kvar;
                this.nzp_user = 0;
                this.num_ls = 0;
                this.nzp_area = 0;

                this.pref = _pref;

                this.nzp_wp = 0;
                this.nzp_key = 0;
                foreach (_Point zap in Points.PointList)
                {
                    if (this.pref == zap.pref)
                    {
                        this.nzp_wp = zap.nzp_wp;
                        break;
                    }
                }
                this.ol_srv = "";
                if (Points.IsFabric)
                {
                    foreach (_Server server in Points.Servers)
                    {
                        if (Points.Point.nzp_server == server.nzp_server)
                        {
#if PG
                            ol_srv = "";
#else
                            this.ol_srv = "@" + server.ol_server;
#endif
                            break;
                        }
                    }
                }

                this.DateOper = Points.DateOper;

                this.b_loadtemp = true;
                this.nzp_pack_saldo = 0;
                this.nzp_pack_ls_saldo = 0;
                this.count_calc_months = 0;
                this.ExistsCounters = true;
                this.benefits_norms = false;
                this.MultiThreadToken = string.Empty;
                this.FirstMonthCalc = DateTime.MinValue;
                this.LockMonth = new LockMonth();
            }

            public ParamCalc(int _nzp_kvar, int _nzp_dom_or_pack, string _pref, int _calc_yy, int _calc_mm,
                int _cur_yy, int _cur_mm, int _nzp_user, int _nzp_key = 0, CalcFonTask _calcfon = null)
            {
                this.calcfon = _calcfon ?? new CalcFonTask(0);

                this.id_bill_pref = "";
                this.id_bill = 0;
                this.temp_table = "";
                this.nzp_reestr = 0;
                this.nzp_par_pack = 0;
                this.b_gil = true;
                this.b_rashod = true;
                this.b_nedo = true;
                this.b_gku = true;
                this.b_charge = true;
                this.b_report = true;
                this.b_peni = true;

                this.b_data = false;
                this.b_reval = false;
                this.b_must = false;
                this.b_handl = false;

                this.b_pack = false;
                this.b_packOt = false;
                this.b_packDel = false;
                this.b_refresh = false;
                this.b_dom_in = false; // при  выбрке дома использовать in (...)
                this.list_dom = false;
                this.list_dom_saldo = false;
                this.curd = DateTime.Now;

                this.calc_yy = _calc_yy;
                this.calc_mm = _calc_mm;
                this.cur_yy = _cur_yy;
                this.cur_mm = _cur_mm;

                if (this.cur_yy == 0)
                {
                    this.cur_yy = Points.CalcMonth.year_;
                    this.cur_mm = Points.CalcMonth.month_;
                }

                this.prev_calc_yy = this.calc_yy;
                this.prev_calc_mm = this.calc_mm - 1;
                if (this.prev_calc_mm == 0)
                {
                    this.prev_calc_yy = this.calc_yy - 1;
                    this.prev_calc_mm = 12;
                }

                this.nzp_dom = _nzp_dom_or_pack;
                this.nzp_kvar = _nzp_kvar;
                this.nzp_user = _nzp_user;
                this.num_ls = 0;
                this.nzp_area = 0;

                this.pref = _pref;
                this.nzp_wp = 0;
                this.nzp_key = _nzp_key;
                foreach (_Point zap in Points.PointList)
                {
                    if (this.pref == zap.pref)
                    {
                        this.nzp_wp = zap.nzp_wp;
                        break;
                    }
                }
                this.ol_srv = "";
                if (Points.IsFabric)
                {
                    foreach (_Server server in Points.Servers)
                    {
                        if (Points.Point.nzp_server == server.nzp_server)
                        {
#if PG
                            ol_srv = "";
#else
                            this.ol_srv = "@" + server.ol_server;
#endif
                            break;
                        }
                    }
                }

                this.DateOper = Points.DateOper;

                this.b_loadtemp = true;
                this.nzp_pack_saldo = 0;
                this.nzp_par_pack = 0;
                this.nzp_pack_ls_saldo = 0;
                this.count_calc_months = 0;
                this.ExistsCounters = true;
                this.benefits_norms = false;
                this.MultiThreadToken = string.Empty;
                this.FirstMonthCalc = DateTime.MinValue;
                this.LockMonth = new LockMonth();
            }
        }
        #endregion параметры передачи данных в расчеты

        //расчет начислений charge_xx
        //---------------------------------------------------
        public struct ChargeXX
        {
            public ParamCalc paramcalc;

            public string charge_xx;
            public string charge_xx_ishod; //с чем сравнивать при перерасчете
            public string lgcharge_xx_ishod; //с чем сравнивать при перерасчете

            public string charge_tab;
            public string prev_charge_xx;
            public string prev_charge_tab;
            public string calc_gku_xx;
            public string gil_xx;
            public string lgcharge_xx;
            public string calc_lg_xx;


            public string kvar_calc_tab;
            public string kvar_calc_xx;

            public string charge_advance_tab;
            public string charge_advance_xx;

            public string dom_calc;

            public string lnk_charge_xx;
            public string lnk_tab;

            public string lnk_reval_xx;
            public string lnk_reval_tab;

            public string reval_tab;
            public string reval_xx;
            public string delta_tab;
            public string delta_xx;
            public string reval_lg_tab;
            public string reval_lg_xx;
            public string reval_why_tab;
            public string reval_why_xx;

            public string peni_charge_tab;
            public string peni_charge_xx;
            public string peni_duty_tab;
            public string peni_duty_xx;

            public string calc_nedo_xx;
            public string fn_supplier;
            public string del_supplier;
            public string from_supplier;
            public string perekidka;
            public string report_xx;
            public string report_xx_dom;

            public string where_report, charge_cnts, charge_nedo, charge_g, charge_cnts_prev, charge_nedo_prev, counters_vals;
            public string counters_xx;

            public string where_kvar;

            public ChargeXX(ParamCalc _paramcalc)
            {
                this.paramcalc = _paramcalc;
                this.paramcalc.b_dom_in = true;
                string cur_bd = this.paramcalc.pref + "_charge_" + (this.paramcalc.cur_yy - 2000).ToString("00");
                string calc_bd = this.paramcalc.pref + "_charge_" + (this.paramcalc.calc_yy - 2000).ToString("00");

                this.lnk_tab = "lnk_charge" + this.paramcalc.alias_again + "_" + this.paramcalc.cur_mm.ToString("00");
                this.lnk_charge_xx = cur_bd + DBManager.tableDelimiter + this.lnk_tab;

                this.lnk_reval_tab = "lnk_reval_" + this.paramcalc.cur_mm.ToString("00");
                this.lnk_reval_xx = cur_bd + DBManager.tableDelimiter + this.lnk_reval_tab;

                this.reval_tab = "reval" + this.paramcalc.alias_again + "_" + this.paramcalc.cur_mm.ToString("00");
                this.reval_xx = cur_bd + DBManager.tableDelimiter + this.reval_tab;

                this.delta_tab = "delta" + this.paramcalc.alias_again + "_" + this.paramcalc.cur_mm.ToString("00");
                this.delta_xx = cur_bd + DBManager.tableDelimiter + this.delta_tab;

                this.reval_lg_tab = "reval_lg" + this.paramcalc.alias_again + "_" + this.paramcalc.cur_mm.ToString("00");
                this.reval_lg_xx = cur_bd + DBManager.tableDelimiter + this.reval_lg_tab;

                this.reval_why_tab = "reval_why_" + this.paramcalc.cur_mm.ToString("00");
                this.reval_why_xx = cur_bd + DBManager.tableDelimiter + this.reval_why_tab;

                this.counters_xx = calc_bd + DBManager.tableDelimiter + "counters" + this.paramcalc.alias + "_" + this.paramcalc.calc_mm.ToString("00");
                this.calc_nedo_xx = calc_bd + DBManager.tableDelimiter + "nedo" + this.paramcalc.alias + "_" + this.paramcalc.calc_mm.ToString("00");
                this.fn_supplier = calc_bd + DBManager.tableDelimiter + "fn_supplier" + this.paramcalc.calc_mm.ToString("00");
                this.from_supplier = calc_bd + DBManager.tableDelimiter + "from_supplier";
                this.del_supplier = calc_bd + DBManager.tableDelimiter + "del_supplier";
                this.perekidka = calc_bd + DBManager.tableDelimiter + "perekidka";

                this.prev_charge_tab = "charge_" + this.paramcalc.prev_calc_mm.ToString("00");
                this.prev_charge_xx = this.paramcalc.pref + "_charge_" + (this.paramcalc.prev_calc_yy - 2000).ToString("00") + DBManager.tableDelimiter + this.prev_charge_tab;

                this.charge_tab = "charge" + this.paramcalc.alias + "_" + this.paramcalc.calc_mm.ToString("00");
                this.charge_xx = calc_bd + DBManager.tableDelimiter + this.charge_tab;

                this.charge_xx_ishod = calc_bd + DBManager.tableDelimiter + "charge_" + this.paramcalc.calc_mm.ToString("00"); //с чем сравнивать при  перерасчете
                this.lgcharge_xx_ishod = calc_bd + DBManager.tableDelimiter + "lgcharge_" + this.paramcalc.calc_mm.ToString("00"); //с чем сравнивать при  перерасчете

                this.calc_gku_xx = calc_bd + DBManager.tableDelimiter + "calc_gku" + this.paramcalc.alias + "_" + this.paramcalc.calc_mm.ToString("00");
                this.gil_xx = calc_bd + DBManager.tableDelimiter + "gil" + this.paramcalc.alias + "_" + this.paramcalc.calc_mm.ToString("00");
                //льготы
                this.lgcharge_xx = calc_bd + DBManager.tableDelimiter + "lgcharge" + this.paramcalc.alias + "_" + this.paramcalc.calc_mm.ToString("00");
                //льготы в разрезе по стекам
                this.calc_lg_xx = calc_bd + DBManager.tableDelimiter + "calc_lg" + this.paramcalc.alias + "_" + this.paramcalc.calc_mm.ToString("00");

                this.kvar_calc_tab = "kvar_calc" + this.paramcalc.alias_again + "_" + this.paramcalc.calc_mm.ToString("00");
                this.kvar_calc_xx = calc_bd + DBManager.tableDelimiter + this.kvar_calc_tab;

                //авансовые расчеты
                this.charge_advance_tab = "charge_advance_" + this.paramcalc.calc_mm.ToString("00");
                this.charge_advance_xx = calc_bd + DBManager.tableDelimiter + this.charge_advance_tab;

                //таблицы с задолженностями
                this.peni_charge_tab = this.paramcalc.b_cur ? "peni_charge_" + this.paramcalc.calc_mm.ToString("00") : "peni_charge_recalc_" + this.paramcalc.calc_mm.ToString("00");
                this.peni_charge_xx = calc_bd + DBManager.tableDelimiter + this.peni_charge_tab;

                //таблицы с суммами по пеням
                this.peni_duty_tab = this.paramcalc.b_cur ? "peni_duty_" + this.paramcalc.calc_mm.ToString("00") : "peni_duty_recalc_" + this.paramcalc.calc_mm.ToString("00");
                this.peni_duty_xx = calc_bd + DBManager.tableDelimiter + this.peni_duty_tab;

                this.dom_calc = calc_bd + DBManager.tableDelimiter + "dom_calc";

                this.where_report = " and month_ = " + this.paramcalc.calc_mm + " and nzp_wp = " + this.paramcalc.nzp_wp;

                this.counters_vals = calc_bd + DBManager.tableDelimiter + "counters_vals ";
                this.charge_cnts = calc_bd + DBManager.tableDelimiter + "charge_cnts ";
                this.charge_nedo = calc_bd + DBManager.tableDelimiter + "charge_nedo ";
                this.charge_g = calc_bd + DBManager.tableDelimiter + "charge_g ";

                string calc_bd_prev = this.paramcalc.pref + "_charge_" + (this.paramcalc.cur_yy - 2000).ToString("00");
                if (this.paramcalc.cur_mm == 1)
                    calc_bd_prev = this.paramcalc.pref + "_charge_" + (this.paramcalc.cur_yy - 2001).ToString("00");

                this.charge_cnts_prev = calc_bd_prev + DBManager.tableDelimiter + "charge_cnts ";
                this.charge_nedo_prev = calc_bd_prev + DBManager.tableDelimiter + "charge_nedo ";

#if PG
                string ol_srv = "";
#else
                string ol_srv = "";
                if (Points.IsFabric)
                {
                    foreach (_Server server in Points.Servers)
                    {
                        if (Points.Point.nzp_server == server.nzp_server)
                        {
                            ol_srv = "@" + server.ol_server;
                            break;
                        }
                    }
                }
#endif

                this.report_xx = Points.Pref + "_fin_" + (this.paramcalc.calc_yy - 2000).ToString("00") + ol_srv + DBManager.tableDelimiter + "fn_ukrgucharge ";
                this.report_xx_dom = Points.Pref + "_fin_" + (this.paramcalc.calc_yy - 2000).ToString("00") + ol_srv + DBManager.tableDelimiter + "fn_ukrgudom ";

                this.where_kvar = " nzp_kvar in ( Select nzp_kvar From t_selkvar)";
                if (this.paramcalc.nzp_kvar > 0)
                    this.where_kvar = " nzp_kvar = " + this.paramcalc.nzp_kvar;
            }
        }


        public struct PackXX
        //---------------------------------------------------
        {
            public CalcTypes.ParamCalc paramcalc;

            public string fn_pa_tab;
            public string fn_pa_xx
            {
                get
                {
                    if (this.is_local)
#if PG
                        return paramcalc.pref + "_charge_" + (paramcalc.calc_yy - 2000).ToString("00") + paramcalc.ol_srv + "." + fn_pa_tab;
#else
                        return this.paramcalc.pref + "_charge_" + (this.paramcalc.calc_yy - 2000).ToString("00") + this.paramcalc.ol_srv + ":" + this.fn_pa_tab;
#endif
                    else
#if PG
                        return Points.Pref + "_fin_" + (paramcalc.calc_yy - 2000).ToString("00") + paramcalc.ol_srv + "." + fn_pa_tab;
#else
                        return Points.Pref + "_fin_" + (this.paramcalc.calc_yy - 2000).ToString("00") + this.paramcalc.ol_srv + ":" + this.fn_pa_tab;
#endif
                }
            }
            public string fn_distrib_prev
            {
                get
                {
                    if (this.paramcalc.DateOper.Day == 1)
                    {
                        if (this.paramcalc.DateOper.Month == 1)
#if PG
                            return Points.Pref + "_fin_" + (paramcalc.calc_yy - 2001).ToString("00") + paramcalc.ol_srv + ".fn_distrib_dom_12";
#else
                            return Points.Pref + "_fin_" + (this.paramcalc.calc_yy - 2001).ToString("00") + this.paramcalc.ol_srv + ":fn_distrib_dom_12";
#endif
                        else
#if PG
                            return Points.Pref + "_fin_" + (paramcalc.calc_yy - 2000).ToString("00") + paramcalc.ol_srv + ".fn_distrib_dom_" + (paramcalc.calc_mm - 1).ToString("00");
#else
                            return Points.Pref + "_fin_" + (this.paramcalc.calc_yy - 2000).ToString("00") + this.paramcalc.ol_srv + ":fn_distrib_dom_" + (this.paramcalc.calc_mm - 1).ToString("00");
#endif
                    }
                    else
                        return this.fn_distrib;
                }
            }

            public string fn_supplier_tab;
            public string fn_supplier;
            public string fn_operday_log;
            public string fn_distrib_tab;
            public string fn_distrib;
            public string fn_naud;
            public string fn_perc;
            public string charge_xx;
            public string fn_sended;
            public string fn_reval;

            public string s_bank;
            public string s_payer;
            public string pack;
            public string pack_log;
            public string pack_log_tab;

            public string pack_ls;
            public string where_pack_ls;
            public string where_pack;

            public int nzp_pack_ls;


            public int nzp_pack
            {
                get
                {
                    return this.paramcalc.nzp_pack;
                }
            }

            public bool is_local;
            public bool all_opermonth;
            public string where_dat_oper
            {
                get
                {
                    if (this.all_opermonth)
                        return this.paramcalc.between_dat_oper;
                    else
                        return " = " + this.paramcalc.dat_oper;
                }
            }

            public PackXX(CalcTypes.ParamCalc _paramcalc, int _nzp_pack_ls, bool _local)
            {
                this.paramcalc = _paramcalc;

                this.all_opermonth = false;

                this.nzp_pack_ls = _nzp_pack_ls;
                this.is_local = _local;

                this.fn_supplier_tab = "fn" + this.paramcalc.alias + "_supplier" + this.paramcalc.calc_mm.ToString("00");
#if PG
                fn_supplier = paramcalc.pref + "_charge_" + (paramcalc.calc_yy - 2000).ToString("00") + paramcalc.ol_srv + "." + fn_supplier_tab;
                charge_xx = paramcalc.pref + "_charge_" + (paramcalc.calc_yy - 2000).ToString("00") + ".charge_" + paramcalc.calc_mm.ToString("00");
                pack = Points.Pref + "_fin_" + (paramcalc.calc_yy - 2000).ToString("00") + paramcalc.ol_srv + ".pack ";
                pack_ls = Points.Pref + "_fin_" + (paramcalc.calc_yy - 2000).ToString("00") + paramcalc.ol_srv + ".pack_ls ";
#else
                this.fn_supplier = this.paramcalc.pref + "_charge_" + (this.paramcalc.calc_yy - 2000).ToString("00") + this.paramcalc.ol_srv + ":" + this.fn_supplier_tab;
                this.charge_xx = this.paramcalc.pref + "_charge_" + (this.paramcalc.calc_yy - 2000).ToString("00") + ":charge_" + this.paramcalc.calc_mm.ToString("00");
                this.pack = Points.Pref + "_fin_" + (this.paramcalc.calc_yy - 2000).ToString("00") + this.paramcalc.ol_srv + ":pack ";
                this.pack_ls = Points.Pref + "_fin_" + (this.paramcalc.calc_yy - 2000).ToString("00") + this.paramcalc.ol_srv + ":pack_ls ";
#endif

                this.pack_log_tab = "pack_log";
#if PG
                pack_log = Points.Pref + "_fin_" + (paramcalc.calc_yy - 2000).ToString("00") + paramcalc.ol_srv + "." + pack_log_tab;
                fn_operday_log = Points.Pref + "_fin_" + (paramcalc.calc_yy - 2000).ToString("00") + paramcalc.ol_srv + ".fn_operday_dom_mc";
#else
                this.pack_log = Points.Pref + "_fin_" + (this.paramcalc.calc_yy - 2000).ToString("00") + this.paramcalc.ol_srv + ":" + this.pack_log_tab;
                this.fn_operday_log = Points.Pref + "_fin_" + (this.paramcalc.calc_yy - 2000).ToString("00") + this.paramcalc.ol_srv + ":fn_operday_dom_mc";
#endif
                this.fn_pa_tab = "fn_pa_dom_" + (this.paramcalc.calc_mm).ToString("00");

#if PG
                fn_perc = Points.Pref + "_fin_" + (paramcalc.calc_yy - 2000).ToString("00") + paramcalc.ol_srv + ".fn_perc_dom";
                fn_naud = Points.Pref + "_fin_" + (paramcalc.calc_yy - 2000).ToString("00") + paramcalc.ol_srv + ".fn_naud_dom";
                fn_sended = Points.Pref + "_fin_" + (paramcalc.calc_yy - 2000).ToString("00") + paramcalc.ol_srv + ".fn_sended_dom";
                fn_reval = Points.Pref + "_fin_" + (paramcalc.calc_yy - 2000).ToString("00") + paramcalc.ol_srv + ".fn_reval_dom";
#else
                this.fn_perc = Points.Pref + "_fin_" + (this.paramcalc.calc_yy - 2000).ToString("00") + this.paramcalc.ol_srv + ":fn_perc_dom";
                this.fn_naud = Points.Pref + "_fin_" + (this.paramcalc.calc_yy - 2000).ToString("00") + this.paramcalc.ol_srv + ":fn_naud_dom";
                this.fn_sended = Points.Pref + "_fin_" + (this.paramcalc.calc_yy - 2000).ToString("00") + this.paramcalc.ol_srv + ":fn_sended_dom";
                this.fn_reval = Points.Pref + "_fin_" + (this.paramcalc.calc_yy - 2000).ToString("00") + this.paramcalc.ol_srv + ":fn_reval_dom";
#endif

                this.fn_distrib_tab = "fn_distrib_dom_" + (this.paramcalc.calc_mm).ToString("00");
#if PG
                fn_distrib = Points.Pref + "_fin_" + (paramcalc.calc_yy - 2000).ToString("00") + paramcalc.ol_srv + "." + fn_distrib_tab;
                s_bank = Points.Pref + "_kernel.s_bank ";
                s_payer = Points.Pref + "_kernel.s_payer ";
#else
                this.fn_distrib = Points.Pref + "_fin_" + (this.paramcalc.calc_yy - 2000).ToString("00") + this.paramcalc.ol_srv + ":" + this.fn_distrib_tab;
                this.s_bank = Points.Pref + "_kernel:s_bank ";
                this.s_payer = Points.Pref + "_kernel:s_payer ";
#endif

                this.where_pack_ls = "nzp_pack_ls > 0";
                if (this.nzp_pack_ls > 0)
                    this.where_pack_ls = "nzp_pack_ls = " + this.nzp_pack_ls;

                this.where_pack = "nzp_pack > 0";
                if (this.nzp_pack > 0)
                    this.where_pack = "nzp_pack = " + this.nzp_pack;

            }
        }

        public enum FunctionType
        {
            Payment = 1,
            Perekidki = 2,
            All = 3
        }

        public enum CalcSteps
        {
            CalcGil = 1,
            CalcRashod = 2,
            CalcNedo = 3,
            CalcGku = 4,
            CalcPeni = 5,
            CalcCharge = 6,
            PrepareParams = 7,
            PrepareMonthParams = 8,
            Complete = 9,
            CalcBenefits = 10

        }
    }

    /// <summary>
    /// Класс определяет расчитывемый месяц и указывает, что нужно считать/пересчитывать
    /// </summary>
    public class CalcMonthInfo
    {
        /// <summary>
        /// Конструктор класса CalcMonthInfo
        /// </summary>
        /// <param name="date">Месяц за который нужно произвести расчет/перерасчет</param>
        /// <param name="calcGku">Признак указывающий нужно ли считать ЖКУ</param>
        /// <param name="calcPeni">Признак указывающий нужно ли считать пени</param>
        public CalcMonthInfo(DateTime date, bool calcGku, bool calcPeni)
        {
            this.Date = date;
            this.CalcGku = calcGku;
            this.CalcPeni = calcPeni;
        }
        /// <summary>
        /// Месяц за который нужно произвести расчет/перерасчет
        /// </summary>
        public readonly DateTime Date;
        /// <summary>
        /// Признак указывающий нужно ли считать ЖКУ
        /// </summary>
        public readonly bool CalcGku;
        /// <summary>
        /// Признак указывающий нужно ли считать пени
        /// </summary>
        public readonly bool CalcPeni;
    }

    public class LockMonth
    {
        public DateTime CurrentMonth { get; set; }
    }
}