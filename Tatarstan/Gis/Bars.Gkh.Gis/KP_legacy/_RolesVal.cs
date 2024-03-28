// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces._RolesVal
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System.Runtime.Serialization;

    [DataContract]
    public struct _RolesVal
    {
        [DataMember]
        public int nzp_role { get; set; }

        [DataMember]
        public int tip { get; set; }

        [DataMember]
        public long kod { get; set; }

        [DataMember]
        public string val { get; set; }
    }
}
