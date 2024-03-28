namespace Bars.Gkh.RegOperator.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Квитанция на оплату Сбербанку
    /// </summary>
    public class SberbankPaymentDoc : BaseEntity
    {
        /// <summary>
        /// Период
        /// </summary>
        public virtual ChargePeriod Period { get; set; }

        /// <summary>
        /// Номер ЛС
        /// </summary>
        public virtual BasePersonalAccount Account { get; set; }

        /// <summary>
        /// Дата последнего запроса
        /// </summary>
        public virtual DateTime LastDate { get; set; }

        /// <summary>
        /// Количество запросов
        /// </summary>
        public virtual int Count { get; set; }

        /// <summary>
        /// GUID пользователя
        /// </summary>
        public virtual string GUID { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo PaymentDocFile { get; set; }

    }
}