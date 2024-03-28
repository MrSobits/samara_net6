// Decompiled with JetBrains decompiler
// Type: Bars.Billing.Core.ProxyEntities.ChargeProxyIn
// Assembly: Bars.Billing.Core, Version=1.0.5812.31365, Culture=neutral, PublicKeyToken=null
// MVID: F2D8471C-E868-44FF-9D5D-21B566471F3C
// Assembly location: C:\Repos\gkh\Main\Bars.Gkh.App\bin\Bars.Billing.Core.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System;
    using System.Collections.Generic;

    public class ChargeProxyIn : InDataProxy
    {
        public List<string> GroupBy { get; set; }

        public bool ShowPrev { get; set; }

        public bool GetFromCash { get; set; }

        public virtual string Alias { get; set; }

        public long Receiver { get; set; }
    }

    public class InDataProxy
    {
        public long PersonalAccountId { get; set; }

        public long PersonalAccountNumber { get; set; }

        public long HouseId { get; set; }

        public int Year { get; set; }

        public int YearTo { get; set; }

        public int Month { get; set; }

        public int MonthTo { get; set; }

        public int ServiceId { get; set; }

        public long SupplierId { get; set; }

        public string Pref { get; set; }

        public long UserId { get; set; }

        public bool DirectUse { get; set; }
    }

    public class ChargeProxyOut
    {
        public virtual int Id { get; set; }

        public virtual long PersonalAccountId { get; set; }

        public virtual int ServiceId { get; set; }

        public virtual string ServiceName { get; set; }

        public virtual long SupplierId { get; set; }

        public virtual string SupplierName { get; set; }

        public virtual long RecipientId { get; set; }

        public virtual string RecipientName { get; set; }

        public virtual int FormulaId { get; set; }

        public virtual string FormulaName { get; set; }

        public virtual int MeasureId { get; set; }

        public virtual string Measure { get; set; }

        public virtual long ParentId { get; set; }

        public virtual DateTime? ChargeDate { get; set; }

        public virtual DateTime? Date { get; set; }

        public virtual DateTime? RecalculationDate { get; set; }

        public virtual Decimal? Tariff { get; set; }

        public virtual Decimal? TariffPrev { get; set; }

        public virtual Decimal? Norm { get; set; }

        public virtual Decimal? NormConsumption { get; set; }

        public virtual Decimal? Consumption { get; set; }

        public virtual Decimal? ConsumptionPrev { get; set; }

        public virtual Decimal? ConsumptionFull { get; set; }

        public virtual Decimal? ConsumptionFullPrev { get; set; }

        public virtual Decimal? ConsumptionODN { get; set; }

        public virtual Decimal? Recalculation { get; set; }

        public virtual Decimal? RecalculationPositive { get; set; }

        public virtual Decimal? RecalculationNegative { get; set; }

        public virtual Decimal? FullCalculation { get; set; }

        public virtual Decimal? FullCalculationPrev { get; set; }

        public virtual Decimal? Credited { get; set; }

        public virtual Decimal? CalculationTariff { get; set; }

        public virtual Decimal? CalculationTariffPrev { get; set; }

        public virtual Decimal? ShortDelivery { get; set; }

        public virtual Decimal? ShortDeliveryPrev { get; set; }

        public virtual Decimal? Benefit { get; set; }

        public virtual Decimal? BenefitPrev { get; set; }

        public virtual Decimal? CalculationDaily { get; set; }

        public virtual Decimal? CalculationDailyPrev { get; set; }

        public virtual Decimal? Change { get; set; }

        public virtual Decimal? ChangePositive { get; set; }

        public virtual Decimal? ChangeNegative { get; set; }

        public virtual Decimal? Paid { get; set; }

        public virtual Decimal? IncomingSaldo { get; set; }

        public virtual Decimal? OutcomingSaldo { get; set; }

        public virtual Decimal? Payable { get; set; }

        public virtual Decimal? PayableEnd { get; set; }

        public virtual Decimal? BenefitAll { get; set; }

        public virtual Decimal? IncomingSaldoBegin { get; set; }

        public virtual Decimal? OutcomingSaldoEnd { get; set; }

        public virtual int HasPreviousRecalculation { get; set; }

        public virtual int HasNextRecalculation { get; set; }

        public virtual int? CalculationSign { get; set; }

        public virtual int CurYear { get; set; }

        public virtual int CurMonth { get; set; }

        public virtual int Order { get; set; }

        public virtual string Topic { get; set; }

        public virtual string TopicName { get; set; }

        public virtual DeltaValue[] Delta { get; set; }

        public virtual bool IsGis { get; set; }

        public virtual bool? IsCommunal { get; set; }

        public virtual Decimal? TariffEot { get; set; }

        public virtual Decimal? TariffEotPrev { get; set; }

        public virtual Decimal? CalcTariffEot { get; set; }

        public virtual Decimal? CalcTariffEotPrev { get; set; }

        public virtual Decimal? DeltaTariffEot { get; set; }

        public virtual Decimal? DeltaTariffEotPrev { get; set; }

        public virtual int ServiceOrderingNumber { get; set; }

        public virtual Decimal? CreditedIncludingBackorderEtc { get; set; }

        public virtual bool leaf { get; set; }

        public virtual int level { get; set; }

        public virtual List<ChargeProxyOut> children { get; set; }

        public virtual bool expanded { get; set; }
    }

    public struct DeltaValue
    {
        public string Field { get; set; }

        public Decimal? Delta { get; set; }

        public byte Type { get; set; }
    }
}
