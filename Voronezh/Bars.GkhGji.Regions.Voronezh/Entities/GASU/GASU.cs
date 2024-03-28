using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Voronezh.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using System;

namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    public class GASU : BaseEntity
    {      
        /// <summary>
        /// ID сообщения в системе СМЭВ
        /// </summary>
        public virtual string MessageId { get; set; }      

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
        public virtual DateTime RequestDate { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime DateFrom { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime DateTo { get; set; }

        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual GasuMessageType GasuMessageType { get; set; }
    }
}
