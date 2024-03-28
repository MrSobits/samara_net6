namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    /// <summary>
    /// Информация о доле, площади и тарифу по лс
    /// </summary>
    public class TariffAreaRecord
    {
        /// <summary>
        /// ЛС
        /// </summary>
        public BasePersonalAccount Account;

        /// <summary>
        /// Id абонента
        /// </summary>
        public long OwnerId;

        /// <summary>
        /// Площадь помещения
        /// </summary>
        public decimal RoomArea;

        /// <summary>
        /// Доля помещения
        /// </summary>
        public decimal AreaShare;

        /// <summary>
        /// Тариф решений
        /// </summary>
        public decimal Tariff;

        /// <summary>
        /// Базовый тариф
        /// </summary>
        public decimal BaseTariff;
    }
}