using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using Bars.GkhGji.Regions.Voronezh.Enums;
using System;

namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    public class PayRegRequests : BaseEntity
    {      
        /// <summary>
        /// ID сообщения в системе СМЭВ
        /// </summary>
        public virtual string MessageId { get; set; }

        ///// <summary>
        ///// Тип запроса по начислению
        ///// </summary>
        //public virtual GisGmpChargeType PayRegChargeType { get; set; }

        /// <summary>
        /// Текущее состояние запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// Ответ от сервера
        /// </summary>
        public virtual String Answer { get; set; }

        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime CalcDate { get; set; }

        #region Запрос платежей

        /// <summary>
        ///Тип платежей
        /// </summary>
        public virtual GisGmpPaymentsKind PayRegPaymentsKind { get; set; } = GisGmpPaymentsKind.PAYMENT;

        /// <summary>
        ///Тип запросов оплат
        /// </summary>
        public virtual GisGmpPaymentsType PayRegPaymentsType { get; set; }
        
        /// <summary>
        /// Получить оплаты с
        /// </summary>
        public virtual DateTime? GetPaymentsStartDate { get; set; }

        /// <summary>
        /// Получить оплаты по
        /// </summary>
        public virtual DateTime? GetPaymentsEndDate { get; set; }
        #endregion
    }
}
