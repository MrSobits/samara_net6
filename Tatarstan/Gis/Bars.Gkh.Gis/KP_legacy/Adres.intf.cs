// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces.Dom
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Улица
    /// </summary>
    [Serializable]
    [DataContract]
    public class Ulica : Rajon
    {
        string _adr;
        string _ulica;
        string _spls;
        string _ulicareg;
        string _area;
        string _geu;

        [DataMember]
        public int nzp_ul { get; set; }
        [DataMember]
        public string nzp_uls { get; set; }
        [DataMember]
        public string ulica { get { return Utils.ENull(this._ulica); } set { this._ulica = value; } }
        [DataMember]
        public string ulicareg { get { return Utils.ENull(this._ulicareg); } set { this._ulicareg = value; } }

        [DataMember]
        public int nzp_area { get; set; }

        [DataMember]
        public int area_code { set; get; }

        [DataMember]
        public string area { get { return Utils.ENull(this._area); } set { this._area = value; } }
        [DataMember]
        public List<int> list_nzp_area { get; set; }
        [DataMember]
        public List<int> list_nzp_wp { get; set; }


        [DataMember]
        public List<int> list_nzp_rajs { get; set; }

        [DataMember]
        public int nzp_geu { get; set; }
        [DataMember]
        public string geu { get { return Utils.ENull(this._geu); } set { this._geu = value; } }

        [DataMember]
        public int geuCode { set; get; }

        [DataMember]
        public string spls { get { return Utils.ENull(this._spls); } set { this._spls = value; } }
        [DataMember]
        public string adr { get { return Utils.ENull(this._adr); } set { this._adr = value; } }

        public override string getAddress()
        {
            string address = base.getAddress();
            address += (address != "" ? ", " : "") + this.ulica_short;
            return address;
        }

        public virtual string getAddressFromUlica()
        {
            return this.ulica_short;
        }

        public string ulica_short
        {
            get
            {
                return (this.ulica == null || this.ulica == "" || this.ulica == "-" ? "код " + this.nzp_ul : this.ulica) + " " + (this.ulicareg ?? "");
            }
        }

        public string ulica_full
        {
            get
            {
                return this.ulica_short.Trim() + (this.rajon.Trim() == "" ? "" : " / " + (this.rajon.Trim() ?? "")) + (this.town.Trim() == "" ? "" : " / " + (this.town.Trim() ?? ""));
            }
        }

        private string ul_short_for_dd;
        public string ulica_short_for_dd
        {
            get
            {
                return String.IsNullOrEmpty(this.ul_short_for_dd) ? this.ulica_short : this.ul_short_for_dd;
            }
            set
            {
                this.ul_short_for_dd = value;
            }
        }
        public Ulica()
            : base()
        {
            this.ul_short_for_dd = "";
            this.nzp_ul = Constants._ZERO_;
            this.nzp_raj = Constants._ZERO_;
            this.nzp_town = Constants._ZERO_;
            this.nzp_area = Constants._ZERO_;

            this.ulica = "";
            this.ulicareg = "";
            this.spls = "";
            this.adr = "";
            this._area = "";
            this.nzp_geu = Constants._ZERO_;
            this.geu = "";
            this.list_nzp_area = new List<int>();
            this.list_nzp_wp = new List<int>();
            this.list_nzp_rajs = new List<int>();
        }

        public void CopyTo(Ulica destination)
        {
            if (destination == null) return;

            base.CopyTo(destination);

            destination.nzp_ul = this.nzp_ul;
            destination._ulica = this._ulica;
            destination._ulicareg = this._ulicareg;
            destination.nzp_area = this.nzp_area;
            destination.area = this.area;
            destination._spls = this._spls;
            destination._adr = this._adr;
        }
    }

    /// <summary>
    /// Дом
    /// </summary>
    [Serializable]
    [DataContract]
    public class Dom : Ulica
    {
        string _ndom;
        string _nkor;
        string _ndom_po;
        string _remark;

        [DataMember]
        public int nzp_dom { get; set; }

        [DataMember]
        public string nzp_doms { get; set; }

        [DataMember]
        public string ndom { get { return Utils.ENull(this._ndom); } set { this._ndom = value; } }

        [DataMember]
        public string nkor { get { return Utils.ENull(this._nkor); } set { this._nkor = value; } }
        [DataMember]
        public string ndom_po { get { return Utils.ENull(this._ndom_po); } set { this._ndom_po = value; } }

        private _Placemark _placemark;

        [DataMember]
        public float pm_x { get { return this._placemark.x; } set { this._placemark.x = value; } }
        [DataMember]
        public float pm_y { get { return this._placemark.y; } set { this._placemark.y = value; } }
        [DataMember]
        public string pm_note { get { return Utils.ENull(this._placemark.note); } set { this._placemark.note = value; } }

        [DataMember]
        public int chekexistdom { get; set; } // 0 - не проверять существование дома по адресу, 1- проверять

        [DataMember]
        public string prms { get; set; }

        [DataMember]
        public int mark_dom { get; set; }

        [DataMember]
        public int is_blocked { get; set; }// заблокирован ли л/с, 0- нет

        [DataMember]
        public string has_pu { get; set; }

        [DataMember]
        public string remark { get { return Utils.ENull(this._remark); } set { this._remark = value; } }// примечание

        [DataMember]
        public bool clear_remark { get; set; }

        [DataMember]
        public int num_page { get; set; }
        [DataMember]
        public int nzp_house_copy_prm { get; set; }

        public void setPlacemark(_Placemark placemark)
        {
            // if (placemark) _placemark = new _Placemark();
            this._placemark = placemark;
        }

        public override string getAddress()
        {
            string address = base.getAddress();
            this.getDomAddress(ref address);
            return address;
        }

        public override string getAddressFromUlica()
        {
            string address = base.getAddressFromUlica();
            this.getDomAddress(ref address);
            return address;
        }

        private void getDomAddress(ref string address)
        {
            address += ", дом " + this.ndom;
            if (this.nkor != "" && this.nkor != "-") address += ", корп. " + this.nkor;
        }

        public Dom()
            : base()
        {
            this.nzp_dom = Constants._ZERO_;

            this.nzp_land = Constants._ZERO_;
            this.nzp_stat = Constants._ZERO_;
            this.chekexistdom = 1;
            this.num_page = 0;

            this.ndom = "";
            this.ndom_po = "";
            this.nkor = "";
            this.is_blocked = 0;
            this.town = "";

            this.prms = "";
            this.has_pu = "";
            this.remark = "";
            this.nzp_house_copy_prm = 0;
        }
    }

    [DataContract]
    [Serializable]
    public struct _Placemark
    //----------------------------------------------------------------------
    {
        [DataMember]
        public long nzp_dom { get; set; }

        [DataMember]
        public float x { get; set; }
        [DataMember]
        public float y { get; set; }
        [DataMember]
        public int nzp_wp { get; set; }
        [DataMember]
        public string note { get; set; }
        [DataMember]
        public int nzp_user { get; set; }
    }
}
