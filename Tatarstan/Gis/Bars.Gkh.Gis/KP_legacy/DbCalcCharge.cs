// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.DataBase.DbCalcCharge
// Assembly: Bars.KP50.Calc, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 24B3ECB6-EBCB-4E02-90D3-EEB42F8A60A5
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\Bars.KP50.Calc.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System;

    public class DbCalcCharge : DataBaseHead
    {
        public struct CalcGkuVals
        {
            public Decimal tarif;
            public Decimal rashod;
            public Decimal rashod_full;
            public int nzp_frm;
            public string name_frm;
            public int nzp_frm_typ;
            public int nzp_frm_typrs;
            public int nzp_prm_tarif;
            public int num_prm_tarif;
            public string name_prm_tarif;
            public int nzp_prm_rashod;
            public int num_prm_rashod;
            public string name_prm_rashod;
            public DateTime dat_s;
            public DateTime dat_po;
            public string measure;
            public int is_device;
            public int cntd;
            public int cntd_mn;
            public Decimal trf1;
            public Decimal trf2;
            public Decimal trf3;
            public Decimal trf4;
            public Decimal rsh1;
            public Decimal rsh2;
            public Decimal rsh3;
            public Decimal valm;
            public Decimal dop87;
            public Decimal dlt_reval;
            public Decimal gil;
            public Decimal squ;
            public Decimal rash_norm_one;
            public Decimal rash_norm;
            public string body_calc;
            public string body_text;
        }

        public struct GilecXX
        {
            public CalcTypes.ParamCalc paramcalc;
            public string gil_xx;
            public string gilec_tab;

            public GilecXX(CalcTypes.ParamCalc _paramcalc)
            {
                this.paramcalc = _paramcalc;
                this.paramcalc.b_dom_in = true;
                this.gilec_tab = "gil" + this.paramcalc.alias + "_" + this.paramcalc.calc_mm.ToString("00");
                this.gil_xx = this.paramcalc.pref + "_charge_" + (this.paramcalc.calc_yy - 2000).ToString("00") + DataBaseHead.tableDelimiter + this.gilec_tab;
            }
        }

        public struct Gku
        {
            public CalcTypes.ParamCalc paramcalc;
            public string calc_charge_bd;
            public string cur_charge_bd;
            public string calc_gku_tab;
            public string calc_gku_xx;
            public string counters_xx;
            public string gil_xx;
            public string perekidka_xx;
            public string prevSaldoMon_charge;
            public string calc_tosupplXX;
            public string calc_lnkchargeXX;
            public string curSaldoMon_charge;

            public string dat_s
            {
                get
                {
                    return this.paramcalc.dat_s;
                }
            }

            public string dat_po
            {
                get
                {
                    return this.paramcalc.calc_mm != 12 ? DataBaseHead.MDY(this.paramcalc.calc_mm + 1, 1, this.paramcalc.calc_yy) : DataBaseHead.MDY(1, 1, this.paramcalc.calc_yy + 1);
                }
            }

            public Gku(CalcTypes.ParamCalc _paramcalc)
            {
                this.paramcalc = _paramcalc;
                this.paramcalc.b_dom_in = true;
                this.calc_charge_bd = this.paramcalc.pref + "_charge_" + (this.paramcalc.calc_yy - 2000).ToString("00");
                this.cur_charge_bd = this.paramcalc.pref + "_charge_" + (this.paramcalc.cur_yy - 2000).ToString("00");
                this.calc_gku_tab = "calc_gku" + this.paramcalc.alias + "_" + this.paramcalc.calc_mm.ToString("00");
                this.calc_gku_xx = this.calc_charge_bd + DataBaseHead.tableDelimiter + this.calc_gku_tab;
                this.counters_xx = this.calc_charge_bd + DataBaseHead.tableDelimiter + "counters" + this.paramcalc.alias + "_" + this.paramcalc.calc_mm.ToString("00");
                this.gil_xx = this.calc_charge_bd + DataBaseHead.tableDelimiter + "gil" + this.paramcalc.alias + "_" + this.paramcalc.calc_mm.ToString("00");
                this.perekidka_xx = this.cur_charge_bd + DataBaseHead.tableDelimiter + "perekidka";
                this.prevSaldoMon_charge = this.paramcalc.pref + "_charge_" + (this.paramcalc.prev_calc_yy - 2000).ToString("00") + DataBaseHead.tableDelimiter + "charge_" + this.paramcalc.prev_calc_mm.ToString("00");
                this.curSaldoMon_charge = this.paramcalc.pref + "_charge_" + (this.paramcalc.calc_yy - 2000).ToString("00") + DataBaseHead.tableDelimiter + "charge_" + this.paramcalc.calc_mm.ToString("00");
                this.calc_tosupplXX = this.paramcalc.pref + "_charge_" + (this.paramcalc.calc_yy - 2000).ToString("00") + DataBaseHead.tableDelimiter + "to_supplier" + this.paramcalc.calc_mm.ToString("00");
                this.calc_lnkchargeXX = this.paramcalc.pref + "_charge_" + (this.paramcalc.calc_yy - 2000).ToString("00") + DataBaseHead.tableDelimiter + "lnk_charge_" + this.paramcalc.calc_mm.ToString("00");
            }
        }

        public struct Rashod
        {
            public CalcTypes.ParamCalc paramcalc;
            public string counters_xx;
            public string counters_tab;
            public string countlnk_xx;
            public string countlnk_tab;
            public string gil_xx;
            public string charge_xx;
            public string delta_xx_cur;
            public string where_dom;
            public string where_kvar;
            public string where_kvarK;
            public string where_kvarA;
            public bool calcv;
            public bool k307;
            public int nzp_type_alg;

            public Rashod(CalcTypes.ParamCalc _paramcalc)
            {
                this.paramcalc = _paramcalc;
                this.calcv = false;
                this.k307 = false;
                this.nzp_type_alg = 6;
                this.counters_tab = "counters" + this.paramcalc.alias + "_" + this.paramcalc.calc_mm.ToString("00");
                this.counters_xx = this.paramcalc.pref + "_charge_" + (this.paramcalc.calc_yy - 2000).ToString("00") + DataBaseHead.tableDelimiter + this.counters_tab;
                this.countlnk_tab = "countlnk" + this.paramcalc.alias + "_" + this.paramcalc.calc_mm.ToString("00");
                this.countlnk_xx = this.paramcalc.pref + "_charge_" + (this.paramcalc.calc_yy - 2000).ToString("00") + DataBaseHead.tableDelimiter + this.countlnk_tab;
                this.gil_xx = this.paramcalc.pref + "_charge_" + (this.paramcalc.calc_yy - 2000).ToString("00") + DataBaseHead.tableDelimiter + "gil" + this.paramcalc.alias + "_" + this.paramcalc.calc_mm.ToString("00");
                this.charge_xx = this.paramcalc.pref + "_charge_" + (this.paramcalc.calc_yy - 2000).ToString("00") + DataBaseHead.tableDelimiter + "charge" + this.paramcalc.alias + "_" + this.paramcalc.calc_mm.ToString("00");
                this.delta_xx_cur = this.paramcalc.pref + "_charge_" + (this.paramcalc.calc_yy - 2000).ToString("00") + DataBaseHead.tableDelimiter + "delta_" + this.paramcalc.calc_mm.ToString("00");
                this.where_dom = "nzp_dom " + (!this.paramcalc.b_reval ? (this.paramcalc.nzp_dom <= 0 ? " > 0 " : " in ( Select nzp_dom From t_selkvar) ") : " in ( Select nzp_dom From t_selkvar) ");
                this.where_kvar = "";
                this.where_kvarK = "";
                this.where_kvarA = "";
                if (this.paramcalc.nzp_kvar <= 0)
                    return;
                this.where_kvar = " and nzp_kvar   = " + (object)this.paramcalc.nzp_kvar;
                this.where_kvarK = " and k.nzp_kvar = " + (object)this.paramcalc.nzp_kvar;
                this.where_kvarA = " and a.nzp_kvar = " + (object)this.paramcalc.nzp_kvar;
            }
        }

        public struct Rashod2
        {
            public string tab;
            public string dat_s;
            public string dat_po;
            public string p_TAB;
            public string p_KEY;
            public string p_INSERT;
            public string p_ACTUAL;
            public string counters_xx;
            public string pref;
            public string p_where;
            public string p_type;
            public string p_FROM;
            public string p_UPDdt_s;
            public string p_UPDdt_po;
            public CalcTypes.ParamCalc paramcalc;

            public Rashod2(string _counters_xx, CalcTypes.ParamCalc _paramcalc)
            {
                this.paramcalc = _paramcalc;
                this.counters_xx = _counters_xx;
                this.tab = "";
                this.dat_s = "";
                this.dat_po = "";
                this.p_TAB = "";
                this.p_KEY = "";
                this.p_INSERT = "";
                this.p_ACTUAL = "";
                this.pref = "";
                this.p_where = "";
                this.p_type = "";
                this.p_FROM = "";
                this.p_UPDdt_s = "";
                this.p_UPDdt_po = "";
            }
        }

        public struct PeriodMustCalc
        {
            public DateTime dat_s;
            public DateTime dat_po;
        }
    }
}
