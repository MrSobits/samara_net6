namespace Bars.Gkh.Gku.Entities
{
    using System;
    using Gkh.Entities;
    using Enums;
    using Gkh1468.Entities;

    /// <summary>
    /// Тариф ЖКУ
    /// </summary>
    public class GkuTariffGji : BaseGkhEntity
    {
        /// <summary>
        /// Коммунальная услуга
        /// </summary>
        public virtual PublicService Service { get; set; }

        /// <summary>
        /// Поставщик коммунальных услуг
        /// </summary>
        public virtual SupplyResourceOrg ResourceOrg { get; set; }

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManOrg { get; set; }

        /// <summary>
        /// Вид услуги
        /// </summary>
        public virtual ServiceKindGku ServiceKind { get; set; }

        /// <summary>
        /// Дата начала действия тарифа
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия тарифа
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Закупочная стоимость коммунального ресурса
        /// </summary>
        public virtual string PurchasePrice { get; set; }

        /// <summary>
        /// Объем закупаемых ресурсов
        /// </summary>
        public virtual decimal PurchaseVolume { get; set; }

        /// <summary>
        /// Тариф РСО
        /// </summary>
        public virtual decimal TarifRso { get; set; }

        /// <summary>
        /// Тариф УО
        /// </summary>
        public virtual decimal TarifMo { get; set; }

        /// <summary>
        /// Реквизиты нормативного акта, устанавливающего тариф
        /// </summary>
        public virtual string NormativeActInfo { get; set; }

        /// <summary>
        /// Норматив
        /// </summary>
        public virtual decimal NormativeValue { get; set; }
    }
}
