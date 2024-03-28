// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces._RServer
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System.Runtime.Serialization;

    [DataContract]
    public struct _RServer
    {
        private string _ip_adr;
        private string _login;
        private string _pwd;
        private string _rcentr;

        [DataMember]
        public bool is_valid { get; set; }

        [DataMember]
        public int nzp_rc { get; set; }

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
        public int nzp_server { get; set; }

        [DataMember]
        public string ip_adr
        {
            get
            {
                return Utils.ENull(this._ip_adr);
            }
            set
            {
                this._ip_adr = value;
            }
        }

        [DataMember]
        public string login
        {
            get
            {
                return Utils.ENull(this._login);
            }
            set
            {
                this._login = value;
            }
        }

        [DataMember]
        public string pwd
        {
            get
            {
                return Utils.ENull(this._pwd);
            }
            set
            {
                this._pwd = value;
            }
        }
    }
}
