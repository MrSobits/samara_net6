// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces.KvarPkodes
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System.Runtime.Serialization;

    [DataContract]
    public class KvarPkodes : Finder
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int nzp_kvar { get; set; }

        [DataMember]
        public int nzp_payer { get; set; }

        [DataMember]
        public int is_princip { get; set; }

        [DataMember]
        public int area_code { get; set; }

        [DataMember]
        public string pkod10 { get; set; }

        [DataMember]
        public string pkod { get; set; }

        [DataMember]
        public int is_default { get; set; }

        [DataMember]
        public string pkod_text { get; set; }

        public KvarPkodes()
        {
            this.id = this.nzp_kvar = this.nzp_payer = this.is_princip = this.area_code = this.is_default = 0;
            this.pkod = this.pkod10 = this.pkod_text = "";
        }
    }
}
