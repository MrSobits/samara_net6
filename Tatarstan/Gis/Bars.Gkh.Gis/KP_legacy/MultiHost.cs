// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces.MultiHost
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System.Collections.Generic;

    public static class MultiHost
    {
        public static List<_RServer> RServers = new List<_RServer>();
        public static List<_RCentr> RCentr = new List<_RCentr>();
        public static bool IsMultiHost = false;

        public static _RServer GetServer(int nzp_server)
        {
            foreach (_RServer rserver in MultiHost.RServers)
            {
                if (nzp_server == rserver.nzp_server)
                    return rserver;
            }
            return new _RServer();
        }
    }
}
