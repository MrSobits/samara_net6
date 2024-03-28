namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc
{
    using System;
    using B4.Utils;
    using B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Enums;
    using Newtonsoft.Json;

    /// <summary>
    /// Класс для хранения истории платежных документов
    /// </summary>
    public class PaymentDocumentSnapshot : BaseEntity
    {
        /// <summary>
        /// Идентификатор объекта, по которому формируется платежка
        /// <remarks>
        /// Это может быть Id либо Лицевого счета, либо Абонента
        /// </remarks>
        /// </summary>
        public virtual long HolderId { get; set; }

        /// <summary>
        /// Тип объекта, по которому формируется платежка (ЛС, абонент)
        /// </summary>
        public virtual string HolderType { get; set; }

        /// <summary>
        /// Тип абонента
        /// </summary>
        public virtual PersonalAccountOwnerType OwnerType { get; set; }

        /// <summary>
        /// Тип документа на оплату
        /// </summary>
        public virtual PaymentDocumentType PaymentDocumentType { get; set; }

        /// <summary>
        /// Период начисления
        /// </summary>
        public virtual ChargePeriod Period { get; set; }

        /// <summary>
        /// Код шаблона
        /// </summary>
        //public virtual PaymentDocumentTemplate Template { get; set; }

        /// <summary>
        /// Основные данные документа
        /// </summary>
        public virtual string Data { get; set; }

        /// <summary>
        /// Статус оплаты
        /// </summary>
        public virtual PaymentDocumentPaymentState PaymentState { get; set; }

        /// <summary>
        /// Наличие эл. почты
        /// </summary>
        public virtual YesNo HasEmail { get; set; }

        #region Grid data

        /// <summary>
        /// Номер документа (для юр, для физ пустое)
        /// </summary>
        public virtual string DocNumber { get; set; }

        /// <summary>
        /// Дата документа (для юр, для физ пустое)
        /// </summary>
        public virtual DateTime? DocDate { get; set; }

        /// <summary>
        /// Плательщик (имя || наименование контрагента)
        /// </summary>
        public virtual string Payer { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// Муниципальный район
        /// </summary>
        public virtual string Settlement { get; set; }

        /// <summary>
        /// Адрес абонента
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Р/С получателя платежа
        /// </summary>
        public virtual string PaymentReceiverAccount { get; set; }

        /// <summary>
        /// Агент доставки
        /// </summary>
        public virtual string DeliveryAgent { get; set; }

        /// <summary>
        /// Всего к оплате
        /// </summary>
        public virtual decimal TotalCharge { get; set; }

        /// <summary>
        /// Базовый слепок
        /// </summary>
        public virtual bool IsBase { get; set; }

        /// <summary>
        /// Количество ЛС
        /// </summary>
        public virtual int AccountCount { get; set; }

        /// <summary>
        /// Инн плательщика
        /// </summary>
        public virtual string OwnerInn { get; set; }

        /// <summary>
        /// Статус отправки на почту
        /// </summary>
        public virtual PaymentDocumentSendingEmailState SendingEmailState { get; set; }
        
        #endregion

        #region Methods

        public virtual T ConvertTo<T>()
        {
            return !this.Data.IsEmpty() ? JsonConvert.DeserializeObject<T>(this.Data) : default(T);
        }

        #endregion
    }
}