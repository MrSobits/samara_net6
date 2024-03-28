// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces._Point
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public struct _Point
    {
        private string _pref;
        private string _point;
        private string _bd_old;
        [DataMember]
        public string b_kod_erc;
        [DataMember]
        public RecordMonth BeginWork;
        [DataMember]
        public RecordMonth BeginCalc;
        [DataMember]
        public RecordMonth CalcMonth;
        [DataMember]
        public List<RecordMonth> CalcMonths;

        [DataMember]
        public int nzp_graj { get; set; }

        [DataMember]
        public int n { get; set; }

        [DataMember]
        public int flag { get; set; }

        [DataMember]
        public int nzp_wp { get; set; }

        [DataMember]
        public string point
        {
            get
            {
                return Utils.ENull(this._point);
            }
            set
            {
                this._point = value;
            }
        }

        [DataMember]
        public string pref
        {
            get
            {
                return Utils.ENull(this._pref);
            }
            set
            {
                this._pref = value;
            }
        }

        [DataMember]
        public string ol_server
        {
            get
            {
                return Utils.ENull(this._bd_old);
            }
            set
            {
                this._bd_old = value;
            }
        }

        [DataMember]
        public int nzp_server { get; set; }
    }
}
