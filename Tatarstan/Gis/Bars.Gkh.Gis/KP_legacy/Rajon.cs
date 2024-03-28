// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces.Rajon
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Rajon : Vill
    {
        [DataMember]
        public int nzp_raj { get; set; }

        [DataMember]
        public string nzp_rajs { get; set; }

        [DataMember]
        public string rajon { get; set; }

        [DataMember]
        public int mark { get; set; }

        [DataMember]
        public int mode { get; set; }

        public Rajon()
        {
            this.nzp_rajs = "";
            this.nzp_raj = 0;
            this.rajon = "";
            this.mark = 0;
            this.mode = 0;
        }

        public override string getAddress()
        {
            string address = base.getAddress();
            return address + (address != "" ? ", " : "") + this.rajon;
        }

        public void CopyTo(Rajon destination)
        {
            if (destination == null)
                return;
            this.CopyTo((Vill)destination);
            destination.nzp_raj = this.nzp_raj;
            destination.rajon = this.rajon;
            destination.mark = this.mark;
            destination.mode = this.mode;
        }
    }
}
