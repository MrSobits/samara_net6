// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces.Charge
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Charge : Saldo
    {
        private string _dat_charge;

        [DataMember]
        public string dat_month { get; set; }

        [DataMember]
        public int has_future_reval { get; set; }

        [DataMember]
        public int has_past_reval { get; set; }

        [DataMember]
        public int month_po { get; set; }

        [DataMember]
        public int nzp_frm { get; set; }

        [DataMember]
        public int isdel { get; set; }

        [DataMember]
        public string name_frm { get; set; }

        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int parent_id { get; set; }

        [DataMember]
        public string dat_charge
        {
            get
            {
                return Utils.ENull(this._dat_charge);
            }
            set
            {
                this._dat_charge = value;
            }
        }

        [DataMember]
        public Decimal tarif { get; set; }

        [DataMember]
        public Decimal tarif_p { get; set; }

        [DataMember]
        public Decimal rsum_tarif_p { get; set; }

        [DataMember]
        public Decimal rsum_lgota { get; set; }

        [DataMember]
        public Decimal sum_tarif { get; set; }

        [DataMember]
        public Decimal sum_dlt_tarif { get; set; }

        [DataMember]
        public Decimal sum_dlt_tarif_p { get; set; }

        [DataMember]
        public Decimal sum_tarif_p { get; set; }

        [DataMember]
        public Decimal sum_lgota { get; set; }

        [DataMember]
        public Decimal sum_dlt_lgota { get; set; }

        [DataMember]
        public Decimal sum_dlt_lgota_p { get; set; }

        [DataMember]
        public Decimal sum_lgota_p { get; set; }

        [DataMember]
        public Decimal sum_nedop { get; set; }

        [DataMember]
        public Decimal sum_nedop_p { get; set; }

        [DataMember]
        public Decimal real_pere { get; set; }

        [DataMember]
        public Decimal sum_pere { get; set; }

        [DataMember]
        public Decimal sum_fakt { get; set; }

        [DataMember]
        public Decimal fakt_to { get; set; }

        [DataMember]
        public Decimal fakt_from { get; set; }

        [DataMember]
        public Decimal fakt_del { get; set; }

        [DataMember]
        public int is_device { get; set; }

        [DataMember]
        public Decimal c_calc { get; set; }

        [DataMember]
        public Decimal c_calc_p { get; set; }

        [DataMember]
        public Decimal c_calc_full { get; set; }

        [DataMember]
        public Decimal c_calc_full_p { get; set; }

        [DataMember]
        public Decimal c_sn { get; set; }

        [DataMember]
        public Decimal c_okaz { get; set; }

        [DataMember]
        public Decimal c_nedop { get; set; }

        [DataMember]
        public Decimal c_reval { get; set; }

        [DataMember]
        public Decimal reval_tarif { get; set; }

        [DataMember]
        public Decimal reval_lgota { get; set; }

        [DataMember]
        public Decimal tarif_f { get; set; }

        [DataMember]
        public Decimal sum_tarif_eot { get; set; }

        [DataMember]
        public Decimal sum_tarif_sn_eot { get; set; }

        [DataMember]
        public Decimal sum_tarif_sn_f { get; set; }

        [DataMember]
        public Decimal rsum_subsidy { get; set; }

        [DataMember]
        public Decimal sum_subsidy { get; set; }

        [DataMember]
        public Decimal sum_subsidy_p { get; set; }

        [DataMember]
        public Decimal sum_subsidy_reval { get; set; }

        [DataMember]
        public Decimal sum_subsidy_all { get; set; }

        [DataMember]
        public Decimal sum_lgota_eot { get; set; }

        [DataMember]
        public Decimal sum_lgota_f { get; set; }

        [DataMember]
        public Decimal sum_smo { get; set; }

        [DataMember]
        public Decimal tarif_f_p { get; set; }

        [DataMember]
        public Decimal sum_tarif_eot_p { get; set; }

        [DataMember]
        public Decimal sum_tarif_sn_eot_p { get; set; }

        [DataMember]
        public Decimal sum_tarif_sn_f_p { get; set; }

        [DataMember]
        public Decimal sum_lgota_eot_p { get; set; }

        [DataMember]
        public Decimal sum_lgota_f_p { get; set; }

        [DataMember]
        public Decimal reval_pol { get; set; }

        [DataMember]
        public Decimal reval_otr { get; set; }

        [DataMember]
        public Decimal sum_insaldo_k { get; set; }

        [DataMember]
        public Decimal sum_insaldo_d { get; set; }

        [DataMember]
        public Decimal sum_outsaldo_k { get; set; }

        [DataMember]
        public Decimal sum_outsaldo_d { get; set; }

        [DataMember]
        public Decimal real_charge_otr { get; set; }

        [DataMember]
        public Decimal real_charge_pol { get; set; }

        [DataMember]
        public bool clickable { get; set; }

        [DataMember]
        public Decimal sum_tarif_f { get; set; }

        [DataMember]
        public Decimal sum_tarif_f_p { get; set; }

        [DataMember]
        public Decimal norma { get; set; }

        [DataMember]
        public Decimal norma_rashod { get; set; }

        [DataMember]
        public int priznak_rasch { get; set; }

        [DataMember]
        public Decimal rashod { get; set; }

        [DataMember]
        public Decimal rashod_odn { get; set; }

        [DataMember]
        public long nzp_payer_princip { get; set; }

        [DataMember]
        public string princip { get; set; }

        [DataMember]
        public long nzp_payer_agent { get; set; }

        [DataMember]
        public string agent { get; set; }

        [DataMember]
        public long nzp_payer_supp { get; set; }

        [DataMember]
        public string supp { get; set; }

        public Charge()
        {
            this.princip = this.agent = this.supp = "";
            this.nzp_payer_agent = this.nzp_payer_princip = this.nzp_payer_supp = 0L;
            this.dat_month = "";
            this.has_future_reval = 0;
            this.has_past_reval = 0;
            this.nzp_frm = 0;
            this.name_frm = "";
            this.norma = new Decimal(0);
            this.norma_rashod = new Decimal(0);
            this.rashod = this.rashod_odn = new Decimal(0);
            this.priznak_rasch = 0;
            this.id = 0;
            this.parent_id = 0;
            this.nzp_charge = 0;
            this.nzp_serv = 0;
            this.nzp_supp = 0L;
            this.nzp_frm = 0;
            this.dat_charge = "";
            this.tarif = new Decimal(0);
            this.tarif_p = new Decimal(0);
            this.rsum_tarif = new Decimal(0);
            this.rsum_tarif_p = new Decimal(0);
            this.rsum_lgota = new Decimal(0);
            this.sum_tarif = new Decimal(0);
            this.isdel = 0;
            this.sum_dlt_tarif = new Decimal(0);
            this.sum_dlt_tarif_p = new Decimal(0);
            this.sum_tarif_p = new Decimal(0);
            this.sum_lgota = new Decimal(0);
            this.sum_dlt_lgota = new Decimal(0);
            this.sum_dlt_lgota_p = new Decimal(0);
            this.sum_lgota_p = new Decimal(0);
            this.sum_nedop = new Decimal(0);
            this.sum_nedop_p = new Decimal(0);
            this.sum_fakt = new Decimal(0);
            this.fakt_to = new Decimal(0);
            this.fakt_from = new Decimal(0);
            this.fakt_del = new Decimal(0);
            this.is_device = 0;
            this.c_calc = new Decimal(0);
            this.c_calc_p = new Decimal(0);
            this.c_calc_full = new Decimal(0);
            this.c_calc_full_p = new Decimal(0);
            this.c_sn = new Decimal(0);
            this.c_okaz = new Decimal(0);
            this.c_nedop = new Decimal(0);
            this.c_reval = new Decimal(0);
            this.reval_tarif = new Decimal(0);
            this.reval_lgota = new Decimal(0);
            this.tarif_f = new Decimal(0);
            this.sum_tarif_eot = new Decimal(0);
            this.sum_tarif_sn_eot = new Decimal(0);
            this.sum_tarif_sn_f = new Decimal(0);
            this.rsum_subsidy = new Decimal(0);
            this.sum_subsidy = new Decimal(0);
            this.sum_subsidy_p = new Decimal(0);
            this.sum_subsidy_reval = new Decimal(0);
            this.sum_subsidy_all = new Decimal(0);
            this.sum_lgota_eot = new Decimal(0);
            this.sum_lgota_f = new Decimal(0);
            this.sum_smo = new Decimal(0);
            this.tarif_f_p = new Decimal(0);
            this.sum_tarif_eot_p = new Decimal(0);
            this.sum_tarif_sn_eot_p = new Decimal(0);
            this.sum_tarif_sn_f_p = new Decimal(0);
            this.sum_lgota_eot_p = new Decimal(0);
            this.sum_lgota_f_p = new Decimal(0);
            this.sum_insaldo_k = new Decimal(0);
            this.sum_insaldo_d = new Decimal(0);
            this.sum_outsaldo_k = new Decimal(0);
            this.sum_outsaldo_d = new Decimal(0);
            this.real_charge_otr = new Decimal(0);
            this.real_charge_pol = new Decimal(0);
            this.reval_pol = new Decimal(0);
            this.reval_otr = new Decimal(0);
            this.clickable = false;
            this.sum_tarif_f = new Decimal(0);
            this.sum_tarif_f_p = new Decimal(0);
        }
    }
}
