namespace Bars.Gkh.Entities.Hcs
{
    using System;
    using Bars.Gkh.Entities;

    using Newtonsoft.Json;

    /// <summary>
    /// Общее сальдо по дому
    /// </summary>
    public class HouseOverallBalance : BaseImportableEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Дата начисления
        /// </summary>
        public virtual DateTime DateCharging { get; set; }

        /// <summary>
        /// Вх. Сальдо
        /// </summary>
        public virtual decimal InnerBalance { get; set; }

        /// <summary>
        /// Начислено за месяц
        /// </summary>
        public virtual decimal MonthCharge { get; set; }

        /// <summary>
        /// К оплате
        /// </summary>
        public virtual decimal Payment { get; set; }

        /// <summary>
        /// Оплачено
        /// </summary>
        public virtual decimal Paid { get; set; }

        /// <summary>
        /// Исх. Сальдо
        /// </summary>
        public virtual decimal OuterBalance { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual string Service { get; set; }

        /// <summary>
        /// Коэффициент коррекции 
        /// </summary>
        public virtual decimal CorrectionCoef { get; set; }

        /// <summary>
        /// Расход по дому 
        /// </summary>
        public virtual decimal HouseExpense { get; set; }

        /// <summary>
        /// Расход по лицевым счетам 
        /// </summary>
        public virtual decimal AccountsExpense { get; set; }

        /// <summary>
        /// Составной ключ вида RealityObject.CodeErc#DateCharging#Service, формируется в интерцепторе на Create и Update
        /// Не отображать в клиентской части
        /// </summary>
        [JsonIgnore]
        public virtual string CompositeKey { get; set; }
    }
}
