using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Enums;
using System;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    public class ViewERKNM : PersistentObject
    {
        /// <summary>
        /// Дата создания
        /// </summary>
        public virtual DateTime ObjectCreateDate { get; set; }

        /// <summary>
        /// ID сообщения в системе СМЭВ
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// Тип запроса в гис ЕРП
        /// </summary>
        public virtual ERKNMDocumentType ERKNMDocumentType { get; set; }

        /// <summary>
        /// Тип запроса в гис ЕРП
        /// </summary>
        public virtual ERKNMRequestType GisErpRequestType { get; set; }

        /// <summary>
        /// Текущее состояние запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// Тип проверки
        /// </summary>
        public virtual KindKND KindKND { get; set; }

        /// <summary>
        /// Ответ от сервера
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual string Inspector { get; set; }

        /// <summary>
        /// Дата запроса
        /// </summary>
        public virtual DateTime RequestDate { get; set; }

        /// <summary>
        /// Время отправки
        /// </summary>
        public virtual DateTime? SendTime { get; set; }

        /// <summary>
        /// Дата ответа
        /// </summary>
        public virtual DateTime? AnswerDate { get; set; }

        /// <summary>
        /// Затрачено времени
        /// </summary>
        public virtual string TimeSpent { get; set; }

        /// <summary>
        /// Распоряжение основание
        /// </summary>
        public virtual string Disposal { get; set; }

        /// <summary>
        /// гуид результата проверки в ерп
        /// </summary>
        public virtual string ERPID { get; set; }
    }
}
