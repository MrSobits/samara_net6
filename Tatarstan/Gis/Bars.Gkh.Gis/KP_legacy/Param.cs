// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces.Param
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Param //: ParamCommon
    {
        [DataMember]
        public new Decimal low_ { get; set; }

        [DataMember]
        public new Decimal high_ { get; set; }

        [DataMember]
        public new int digits_ { get; set; }

        [DataMember]
        public int norm_type_id { get; set; }

        //[DataMember]
        //public List<Res_y> values { get; set; }

        //[DataMember]
        //public List<PrmTypes> norm_sprav_values { get; set; }

        [DataMember]
        public int show_point { get; set; }

        [DataMember]
        public string pref_sprav { get; set; }

        public Param()
        {
            this.low_ = new Decimal(-1);
            this.high_ = new Decimal(-1);
            this.digits_ = -1;
            this.norm_type_id = 0;
            //this.values = new List<Res_y>();
            //this.norm_sprav_values = new List<PrmTypes>();
            this.show_point = 0;
            this.pref_sprav = "";
        }

        public void CopyTo(Param destination)
        {
           // this.CopyTo((ParamCommon)destination);
            destination.low_ = this.low_;
            destination.high_ = this.high_;
            destination.digits_ = this.digits_;
          //  destination.values = this.values;
        }
    }
}
