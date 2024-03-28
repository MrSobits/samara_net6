// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces.SupplierFinder
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System.Runtime.Serialization;

    public class SupplierFinder
    {
        [DataMember]
        public int nzp_supp { get; set; }

        [DataMember]
        public string name_supp { get; set; }

        [DataMember]
        public int nzp_frm { get; set; }

        [DataMember]
        public int nzp_payer_princip { get; set; }

        public SupplierFinder()
        {
            this.nzp_supp = this.nzp_frm = this.nzp_payer_princip = 0;
            this.name_supp = "";
        }
    }
}
