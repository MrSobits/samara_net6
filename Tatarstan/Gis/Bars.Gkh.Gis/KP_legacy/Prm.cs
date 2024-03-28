// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces.Prm
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Prm// : ParamCommon
    {
        private string _dat_when;
        private string _dat_del;
        private string _name_link;
        [DataMember]
        public bool callFromFindPrm;

        [DataMember]
        public string cnt { get; set; }

        [DataMember]
        public int month_ { get; set; }

        [DataMember]
        public int year_ { get; set; }

        [DataMember]
        public string spis_prm { get; set; }

        [DataMember]
        public string dopprm { get; set; }

        [DataMember]
        public int nzp_key { get; set; }

        [DataMember]
        public string val_prm_po { get; set; }

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
        public string dat_when_po { get; set; }

        [DataMember]
        public int nzp_user_when { get; set; }

        [DataMember]
        public string user_name { get; set; }

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
        public int user_del { get; set; }

        [DataMember]
        public string delname { get; set; }

        [DataMember]
        public string name_link
        {
            get
            {
                return Utils.ENull(this._name_link);
            }
            set
            {
                this._name_link = value;
            }
        }

        [DataMember]
        public int visible { get; set; }

        //[DataMember]
        //public enCriteria criteria { get; set; }

        [DataMember]
        public int nzp_serv { get; set; }

        [DataMember]
        public string service { get; set; }

        [DataMember]
        public int nzp_frm { get; set; }

        [DataMember]
        public string name_frm { get; set; }

        [DataMember]
        public string measure { get; set; }

        [DataMember]
        public int is_edit { get; set; }

        [DataMember]
        public Param param { get; set; }

        [DataMember]
        public bool isLoadParamInfo { get; set; }

        [DataMember]
        public int nzp_reg { get; set; }

        [DataMember]
        public int nzp_prm_calc { get; set; }

        [DataMember]
        public Decimal tarif { get; set; }

        [DataMember]
        public DateTime arx9_dt { get; set; }

        [DataMember]
        public string arx9_ktr { get; set; }

        [DataMember]
        public int arx9_kkst { get; set; }

        [DataMember]
        public int arx9_nzp_conv_db { get; set; }

        [DataMember]
        public int nzp_trfl { get; set; }

        [DataMember]
        public int nzp_tarif { get; set; }

        public Prm()
        {
            //this.criteria = enCriteria.equal;
            this.cnt = "";
            this.month_ = 0;
            this.year_ = 0;
            this.spis_prm = "";
            this.dopprm = "";
            this.nzp_key = 0;
            this.val_prm_po = "";
            this.is_actual = 0;
            this.dat_when = "";
            this.dat_when_po = "";
            this.nzp_user_when = 0;
            this.user_name = "";
            this.dat_del = "";
            this.user_del = 0;
            this.delname = "";
            this.name_link = "";
            this.callFromFindPrm = false;
            this.visible = 1;
            this.nzp_serv = 0;
            this.service = "";
            this.nzp_frm = 0;
            this.name_frm = "";
            this.is_edit = -999987654;
            this.measure = "";
            this.param = (Param)null;
            this.isLoadParamInfo = false;
            this.nzp_reg = 0;
        }
    }
}
