namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Расчетно-кассовый центр
    /// </summary>
    public class CashPaymentCenter : BaseImportableEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Идентификатор РКЦ
        /// </summary>
        public virtual string Identifier { get; set; }

        /// <summary>
        /// РКЦ проводит начисления
        /// </summary>
        public virtual bool ConductsAccrual { get; set; }

        /// <summary>
        /// Настройка скрытия / отображения ПДн при выгрузке данных сервисом GetChargePaymentRkc
        /// </summary>
        public virtual bool ShowPersonalData { get; set; }
    }
}