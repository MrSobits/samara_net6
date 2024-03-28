// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces.Service
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Service : Ls
    {
        private string _dat_s = "";
        private string _dat_po = "";
        private string _dat_when = "";
        private string _dat_del = "";
        [DataMember]
        public RecordMonth YM;

        [DataMember]
        public int month_
        {
            get
            {
                return this.YM.month_;
            }
            set
            {
                this.YM.month_ = value;
            }
        }

        [DataMember]
        public int year_
        {
            get
            {
                return this.YM.year_;
            }
            set
            {
                this.YM.year_ = value;
            }
        }

        [DataMember]
        public int nzp_serv { get; set; }

        [DataMember]
        public int nzp_groupserv { get; set; }

        [DataMember]
        public int cnt_ls { get; set; }

        [DataMember]
        public string service { get; set; }

        [DataMember]
        public long nzp_supp { get; set; }

        [DataMember]
        public string name_supp { get; set; }

        [DataMember]
        public int nzp_frm { get; set; }

        [DataMember]
        public string name_frm { get; set; }

        [DataMember]
        public int nzp_tarif { get; set; }

        [DataMember]
        public Decimal tarif { get; set; }

        [DataMember]
        public string tarif_s { get; set; }

        [DataMember]
        public string dat_s
        {
            get
            {
                return Utils.ENull(this._dat_s);
            }
            set
            {
                this._dat_s = value;
            }
        }

        [DataMember]
        public string dat_po
        {
            get
            {
                return Utils.ENull(this._dat_po);
            }
            set
            {
                this._dat_po = value;
            }
        }

        [DataMember]
        public int is_actual { get; set; }

        [DataMember]
        public string dat_when
        {
            get
            {
                return Utils.ENull(this._dat_when);
            }
            set
            {
                this._dat_when = value;
            }
        }

        [DataMember]
        public long nzp_user_when { get; set; }

        [DataMember]
        public string dat_del
        {
            get
            {
                return Utils.ENull(this._dat_del);
            }
            set
            {
                this._dat_del = value;
            }
        }

        [DataMember]
        public long nzp_user_del { get; set; }

        [DataMember]
        public int nzp_foss { get; set; }

        [DataMember]
        public int activePeriod { get; set; }

        [DataMember]
        public string service_small { get; set; }

        [DataMember]
        public string service_name { get; set; }

        [DataMember]
        public string ed_izmer { get; set; }

        [DataMember]
        public int type_lgot { get; set; }

        [DataMember]
        public int ordering { get; set; }

        [DataMember]
        public int nzp_measure { get; set; }

        [DataMember]
        public int nzp_payer_agent { get; set; }

        [DataMember]
        public int nzp_payer_princip { get; set; }

        [DataMember]
        public int nzp_payer_supp { get; set; }

        [DataMember]
        public bool one_actual_supp { get; set; }

        [DataMember]
        public List<SupplierFinder> list_supp { get; set; }

        public List<KvarPkodes> list_kvar_pkodes { get; set; }

        [DataMember]
        public string pkodes { get; set; }

        public Service()
        {
            this.YM.month_ = 0;
            this.YM.year_ = 0;
            this.nzp_serv = 0;
            this.nzp_groupserv = 0;
            this.cnt_ls = 0;
            this.service = "";
            this.nzp_supp = 0L;
            this.name_supp = "";
            this.nzp_frm = 0;
            this.name_frm = "";
            this.nzp_tarif = 0;
            this.tarif = new Decimal(0);
            this.tarif_s = "";
            this._dat_s = "";
            this._dat_po = "";
            this.is_actual = 0;
            this._dat_when = "";
            this.nzp_user_when = 0L;
            this.pkodes = "";
            this.list_supp = new List<SupplierFinder>();
            this.list_kvar_pkodes = new List<KvarPkodes>();
            this._dat_when = "";
            this.nzp_user_del = 0L;
            this.nzp_foss = 0;
            this.activePeriod = -999987654;
            this.service_small = "";
            this.service_name = "";
            this.ed_izmer = "";
            this.type_lgot = -999987654;
            this.ordering = 0;
            this.nzp_measure = 0;
            this.nzp_payer_agent = this.nzp_payer_princip = this.nzp_payer_supp = 0;
            this.one_actual_supp = true;
        }
    }
}
