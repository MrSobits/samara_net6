namespace Bars.Gkh.Entities.Hcs
{
    using System;

    using Bars.Gkh.Entities;

    using Newtonsoft.Json;

    /// <summary>
    /// Начисления лицевого счета
    /// </summary>
    public class HouseAccountCharge : BaseImportableEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Лицевой счет дома
        /// </summary>
        public virtual HouseAccount Account { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual string Service { get; set; }

        /// <summary>
        /// Тариф
        /// </summary>
        public virtual decimal Tariff { get; set; }

        /// <summary>
        /// Расход  
        /// </summary>
        public virtual decimal Expense { get; set; }

        /// <summary>
        /// Дата начисления  
        /// </summary>
        public virtual DateTime DateCharging { get; set; }

        /// <summary>
        /// Полный расчет 
        /// </summary>
        public virtual decimal CompleteCalc { get; set; }

        /// <summary>
        /// Недопоставка 
        /// </summary>
        public virtual decimal Underdelivery { get; set; }

        /// <summary>
        /// Начислено 
        /// </summary>
        public virtual decimal Charged { get; set; }

        /// <summary>
        /// Перерасчет 
        /// </summary>
        public virtual decimal Recalc { get; set; }

        /// <summary>
        /// Изменен  
        /// </summary>
        public virtual decimal Changed { get; set; }

        /// <summary>
        /// Оплата 
        /// </summary>
        public virtual decimal Payment { get; set; }

        /// <summary>
        /// Начислено к оплате 
        /// </summary>
        public virtual decimal ChargedPayment { get; set; }

        /// <summary>
        /// Вх. Сальдо
        /// </summary>
        public virtual decimal InnerBalance { get; set; }

        /// <summary>
        /// Исх. Сальдо
        /// </summary>
        public virtual decimal OuterBalance { get; set; }

        /// <summary>
        /// Составной ключ вида Account.PaymentCode#DateCharging#Service, формируется в интерцепторе на Create и Update
        /// </summary>
        [JsonIgnore]
        public virtual string CompositeKey { get; set; }

        /// <summary>
        /// Поставщик
        /// </summary>
        public virtual string Supplier { get; set; }

        /// <summary>
        /// Размер взноса на КР
        /// </summary>
        public virtual decimal? PaymentSizeCr { get; set; }

        /// <summary>
        /// Начислено взносов Всего
        /// </summary>
        public virtual decimal? PaymentChargeAll { get; set; }

        /// <summary>
        /// Оплачено взносов Всего
        /// </summary>
        public virtual decimal? PaymentPaidAll { get; set; }

        /// <summary>
        /// Задолженность по взносам Всего
        /// </summary>
        public virtual decimal? PaymentDebtAll { get; set; }

        /// <summary>
        /// Начислено взносов за месяц
        /// </summary>
        public virtual decimal? PaymentChargeMonth { get; set; }

        /// <summary>
        /// Оплачено взносов за месяц
        /// </summary>
        public virtual decimal? PaymentPaidMonth { get; set; }

        /// <summary>
        /// Задолженность по взносам за месяц
        /// </summary>
        public virtual decimal? PaymentDebtMonth { get; set; }

        /// <summary>
        /// Начислено пени Всего
        /// </summary>
        public virtual decimal? PenaltiesChargeAll { get; set; }

        /// <summary>
        /// Оплачено пени Всего
        /// </summary>
        public virtual decimal? PenaltiesPaidAll { get; set; }

        /// <summary>
        /// Задолженность по пени Всего
        /// </summary>
        public virtual decimal? PenaltiesDebtAll { get; set; }

        /// <summary>
        /// Начислено пени за месяц
        /// </summary>
        public virtual decimal? PenaltiesChargeMonth { get; set; }

        /// <summary>
        /// Оплачено пени за месяц
        /// </summary>
        public virtual decimal? PenaltiesPaidMonth { get; set; }

        /// <summary>
        /// Задолженность по пени за месяц
        /// </summary>
        public virtual decimal? PenaltiesDebtMonth { get; set; }
    }
}
