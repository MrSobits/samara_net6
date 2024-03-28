// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces.CalcMonthParams
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CalcMonthParams
    {
        [DataMember]
        public string pref { get; set; }

        public CalcMonthParams()
        {
            this.pref = "";
        }

        public CalcMonthParams(string _pref)
        {
            this.pref = _pref;
        }
    }
}
