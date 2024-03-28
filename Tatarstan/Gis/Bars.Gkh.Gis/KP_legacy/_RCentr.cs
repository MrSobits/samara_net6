// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces._RCentr
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System.Runtime.Serialization;

    [DataContract]
    public struct _RCentr
    {
        private string _rcentr;
        private string _adres;
        private string _email;
        private string _ruk;

        [DataMember]
        public bool is_valid { get; set; }

        [DataMember]
        public int nzp_rc { get; set; }

        [DataMember]
        public int nzp_raj { get; set; }

        [DataMember]
        public int pref { get; set; }

        [DataMember]
        public string rcentr
        {
            get
            {
                return Utils.ENull(this._rcentr);
            }
            set
            {
                this._rcentr = value;
            }
        }

        [DataMember]
        public string adres
        {
            get
            {
                return Utils.ENull(this._adres);
            }
            set
            {
                this._adres = value;
            }
        }

        [DataMember]
        public string email
        {
            get
            {
                return Utils.ENull(this._email);
            }
            set
            {
                this._email = value;
            }
        }

        [DataMember]
        public string ruk
        {
            get
            {
                return Utils.ENull(this._ruk);
            }
            set
            {
                this._ruk = value;
            }
        }
    }
}
