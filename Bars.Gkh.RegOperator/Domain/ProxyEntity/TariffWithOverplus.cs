namespace Bars.Gkh.RegOperator.Domain.ProxyEntity
{
    /// <summary>
    /// Информация по тарифу и разнице между базовым тарифом и решением собственников
    /// </summary>
    public struct TariffWithOverplus
    {
        public decimal Tariff;

        public decimal OverPlus;

        public decimal DayCalc;
    }
}