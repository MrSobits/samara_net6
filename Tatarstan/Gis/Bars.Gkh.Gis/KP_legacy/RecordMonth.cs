// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces.RecordMonth
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System.Runtime.Serialization;

    public struct RecordMonth
    {
        [DataMember]
        public int year_;
        [DataMember]
        public int month_;

        [DataMember]
        public string sid
        {
            get
            {
                return Utils.PutIdMonth(this.year_, this.month_).ToString();
            }
            set
            {
                Utils.GetIdMonth(value, ref this.year_, ref this.month_);
            }
        }

        [DataMember]
        public int id
        {
            get
            {
                return Utils.PutIdMonth(this.year_, this.month_);
            }
            set
            {
                Utils.GetIdMonth(value, ref this.year_, ref this.month_);
            }
        }

        [DataMember]
        public string name
        {
            get
            {
                if (this.month_ == 0)
                    return this.year_.ToString() + " год";
                return this.year_.ToString() + "-" + this.month_.ToString("00");
            }
        }

        [DataMember]
        public string name_month
        {
            get
            {
                string str = "Не определено";
                switch (this.month_)
                {
                    case 1:
                        str = "Январь";
                        break;
                    case 2:
                        str = "Февраль";
                        break;
                    case 3:
                        str = "Март";
                        break;
                    case 4:
                        str = "Апрель";
                        break;
                    case 5:
                        str = "Май";
                        break;
                    case 6:
                        str = "Июнь";
                        break;
                    case 7:
                        str = "Июль";
                        break;
                    case 8:
                        str = "Август";
                        break;
                    case 9:
                        str = "Сентябрь";
                        break;
                    case 10:
                        str = "Октябрь";
                        break;
                    case 11:
                        str = "Ноябрь";
                        break;
                    case 12:
                        str = "Декабрь";
                        break;
                }
                return str + " " + this.year_.ToString();
            }
        }
    }
}
