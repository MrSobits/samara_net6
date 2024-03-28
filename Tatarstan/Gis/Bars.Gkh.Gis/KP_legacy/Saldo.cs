// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces.Saldo
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    [Serializable]
    public struct _RecordSaldo
    {
        [DataMember]
        public Decimal sum_real;
        [DataMember]
        public Decimal rsum_tarif;
        [DataMember]
        public Decimal sum_charge;
        [DataMember]
        public Decimal reval;
        [DataMember]
        public Decimal real_charge;
        [DataMember]
        public Decimal sum_money;
        [DataMember]
        public Decimal money_to;
        [DataMember]
        public Decimal money_from;
        [DataMember]
        public Decimal money_del;
        [DataMember]
        public Decimal sum_insaldo;
        [DataMember]
        public Decimal izm_saldo;
        [DataMember]
        public Decimal sum_outsaldo;
        [DataMember]
        public Decimal sum_fin;
        [DataMember]
        public Decimal sum_dolg;
    }

    [DataContract]
    public class Saldo : Ls
    {
        private string _groupby;
        [DataMember]
        public RecordMonth YM;
        private _RecordSaldo saldo;

        [DataMember]
        public int month_
        {
            get
            {
                return this.YM.month_;
            }
            set
            {
                this.YM.month_ = value;
            }
        }

        [DataMember]
        public int year_
        {
            get
            {
                return this.YM.year_;
            }
            set
            {
                this.YM.year_ = value;
            }
        }

        [DataMember]
        public string groupby
        {
            get
            {
                return Utils.ENull(this._groupby);
            }
            set
            {
                this._groupby = value;
            }
        }

        [DataMember]
        public int nzp_charge { get; set; }

        [DataMember]
        public int nzp_serv { get; set; }

        [DataMember]
        public string service { get; set; }

        [DataMember]
        public string measure { get; set; }

        [DataMember]
        public long nzp_supp { get; set; }

        [DataMember]
        public string supplier { get; set; }

        [DataMember]
        public Decimal sum_real
        {
            get
            {
                return this.saldo.sum_real;
            }
            set
            {
                this.saldo.sum_real = value;
            }
        }

        [DataMember]
        public Decimal rsum_tarif
        {
            get
            {
                return this.saldo.rsum_tarif;
            }
            set
            {
                this.saldo.rsum_tarif = value;
            }
        }

        [DataMember]
        public Decimal sum_charge
        {
            get
            {
                return this.saldo.sum_charge;
            }
            set
            {
                this.saldo.sum_charge = value;
            }
        }

        [DataMember]
        public Decimal reval
        {
            get
            {
                return this.saldo.reval;
            }
            set
            {
                this.saldo.reval = value;
            }
        }

        [DataMember]
        public Decimal real_charge
        {
            get
            {
                return this.saldo.real_charge;
            }
            set
            {
                this.saldo.real_charge = value;
            }
        }

        [DataMember]
        public Decimal sum_money
        {
            get
            {
                return this.saldo.sum_money;
            }
            set
            {
                this.saldo.sum_money = value;
            }
        }

        [DataMember]
        public Decimal money_to
        {
            get
            {
                return this.saldo.money_to;
            }
            set
            {
                this.saldo.money_to = value;
            }
        }

        [DataMember]
        public Decimal money_from
        {
            get
            {
                return this.saldo.money_from;
            }
            set
            {
                this.saldo.money_from = value;
            }
        }

        [DataMember]
        public Decimal money_del
        {
            get
            {
                return this.saldo.money_del;
            }
            set
            {
                this.saldo.money_del = value;
            }
        }

        [DataMember]
        public Decimal sum_insaldo
        {
            get
            {
                return this.saldo.sum_insaldo;
            }
            set
            {
                this.saldo.sum_insaldo = value;
            }
        }

        [DataMember]
        public Decimal izm_saldo
        {
            get
            {
                return this.saldo.izm_saldo;
            }
            set
            {
                this.saldo.izm_saldo = value;
            }
        }

        [DataMember]
        public Decimal sum_outsaldo
        {
            get
            {
                return this.saldo.sum_outsaldo;
            }
            set
            {
                this.saldo.sum_outsaldo = value;
            }
        }

        [DataMember]
        public Decimal sum_fin
        {
            get
            {
                return this.saldo.sum_fin;
            }
            set
            {
                this.saldo.sum_fin = value;
            }
        }

        [DataMember]
        public Decimal sum_dolg
        {
            get
            {
                return this.saldo.sum_dolg;
            }
            set
            {
                this.saldo.sum_dolg = value;
            }
        }

        [DataMember]
        public int find_from_the_start { get; set; }

        [DataMember]
        public int order_by { get; set; }

        [DataMember]
        public int ordering { get; set; }

        public Saldo()
        {
            this.order_by = this.ordering = 0;
            this.YM.month_ = 0;
            this.YM.year_ = 0;
            this.groupby = "";
            this.nzp_charge = 0;
            this.nzp_serv = 0;
            this.service = "";
            this.measure = "";
            this.nzp_supp = 0L;
            this.supplier = "";
            this.saldo.sum_real = new Decimal(0);
            this.saldo.sum_charge = new Decimal(0);
            this.saldo.reval = new Decimal(0);
            this.saldo.real_charge = new Decimal(0);
            this.saldo.sum_money = new Decimal(0);
            this.saldo.money_to = new Decimal(0);
            this.saldo.money_from = new Decimal(0);
            this.saldo.money_del = new Decimal(0);
            this.saldo.sum_insaldo = new Decimal(0);
            this.saldo.izm_saldo = new Decimal(0);
            this.saldo.sum_outsaldo = new Decimal(0);
            this.saldo.sum_fin = new Decimal(0);
            this.saldo.sum_dolg = new Decimal(0);
            this.find_from_the_start = 0;
        }
    }
}
