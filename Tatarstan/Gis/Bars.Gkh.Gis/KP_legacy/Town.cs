// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces.Town
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Town : Finder
    {
        [DataMember]
        public int nzp_town { get; set; }

        [DataMember]
        public string town { get; set; }

        [DataMember]
        public int nzp_stat { get; set; }

        [DataMember]
        public string stat { get; set; }

        [DataMember]
        public int nzp_land { get; set; }

        [DataMember]
        public string land { get; set; }

        [DataMember]
        public int _checked { get; set; }

        [DataMember]
        public string num { get; set; }

        public Town()
        {
            this.nzp_town = 0;
            this.town = this.land = this.stat = "";
            this.nzp_stat = 0;
            this.nzp_land = 0;
            this._checked = 0;
            this.num = "";
        }

        public virtual string getAddress()
        {
            return this.town;
        }

        public void CopyTo(Town destination)
        {
            if (destination == null)
                return;
            this.CopyTo((Finder)destination);
            destination.nzp_town = this.nzp_town;
            destination.town = this.town;
            destination.nzp_stat = this.nzp_stat;
            destination.nzp_land = this.nzp_land;
            destination._checked = this._checked;
            destination.num = this.num;
        }
    }
}
