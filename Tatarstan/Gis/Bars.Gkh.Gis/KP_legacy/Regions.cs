// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Global.Regions
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    public static class Regions
    {
        public static Regions.Region GetById(int id)
        {
            if (id == 63)
                return Regions.Region.Samarskaya_obl;
            if (id == 31)
                return Regions.Region.Belgorodskaya_obl;
            if (id == 71)
                return Regions.Region.Tulskaya_obl;
            if (id == 16)
                return Regions.Region.Tatarstan;
            if (id == 15)
                return Regions.Region.Rso;
            if (id == 40)
                return Regions.Region.Kaluga;
            if (id == 57)
                return Regions.Region.Orel;
            if (id == 30)
                return Regions.Region.Astrakhan;
            return id == 14 ? Regions.Region.Sakha : Regions.Region.None;
        }

        public enum Region
        {
            None = 0,
            Sakha = 14,
            Rso = 15,
            Tatarstan = 16,
            Astrakhan = 30,
            Belgorodskaya_obl = 31,
            Kaluga = 40,
            Orel = 57,
            Samarskaya_obl = 63,
            Tulskaya_obl = 71,
        }
    }
}
