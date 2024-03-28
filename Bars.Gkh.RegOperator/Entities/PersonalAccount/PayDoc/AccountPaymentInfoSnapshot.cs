namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc
{
    using B4.DataAccess;

    using Bars.B4.Utils;

    using Gkh.Enums;
    using Newtonsoft.Json;

    /// <summary>
    /// Данные для документа на оплату по ЛС
    /// </summary>
    public class AccountPaymentInfoSnapshot : BaseEntity
    {
        /// <summary>
        /// Основная информация по документу
        /// </summary>
        public virtual PaymentDocumentSnapshot Snapshot { get; set; }

        /// <summary>
        /// Id ЛС
        /// </summary>
        public virtual long AccountId { get; set; }

        /// <summary>
        /// Данные для документа
        /// </summary>
        public virtual string Data { get; set; }

        /// <summary>
        /// Номер ЛС
        /// </summary>
        public virtual string AccountNumber { get; set; }

        /// <summary>
        /// Адрес до уровня квартиры. Т.е. Казань, Гаврилова 13, кв. 5
        /// </summary>
        public virtual string RoomAddress { get; set; }

        /// <summary>
        /// Тип комнаты
        /// </summary>
        public virtual RoomType RoomType { get; set; }

        /// <summary>
        /// Площадь помещения
        /// </summary>
        public virtual float Area { get; set; }

        /// <summary>
        /// Тариф
        /// </summary>
        public virtual decimal Tariff { get; set; }

        /// <summary>
        /// Сумма начисления
        /// Складывается из BaseTariffSum, DecisionTariffSum, PenaltySum
        /// </summary>
        public virtual decimal ChargeSum { get; set; }

        /// <summary>
        /// Служебное- К оплате по базовому тарифу
        /// </summary>
        public virtual decimal BaseTariffSum { get; set; }

        /// <summary>
        /// Служебное- К оплате по тарифу решения
        /// </summary>
        public virtual decimal DecisionTariffSum { get; set; }

        /// <summary>
        /// Служебное- Пени к оплате
        /// </summary>
        public virtual decimal PenaltySum { get; set; }

        /// <summary>
        /// Оказанные услуги (в нашем случае пока только кап ремонт)
        /// </summary>
        public virtual string Services { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T ConvertTo<T>()
        {
            return this.Data.IsNotEmpty() ? JsonConvert.DeserializeObject<T>(this.Data) : default(T);
        }
    } 
}