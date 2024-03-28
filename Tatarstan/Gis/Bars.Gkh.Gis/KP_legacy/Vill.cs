// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces.Vill
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Vill : Town
    {
        [DataMember]
        public Decimal nzp_vill { get; set; }

        [DataMember]
        public string vill { get; set; }

        public Vill()
        {
            this.nzp_vill = new Decimal(0);
            this.vill = "";
        }

        public void CopyTo(Vill destination)
        {
            if (destination == null)
                return;
            this.CopyTo((Town)destination);
            destination.nzp_vill = this.nzp_vill;
            destination.vill = this.vill;
        }
    }
}
