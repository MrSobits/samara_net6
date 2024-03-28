namespace Bars.GkhGji.Regions.Habarovsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Enums;
    using Bars.B4.Modules.FileStorage;

    public class SMEVPropertyType : BaseEntity
    {
        /// <summary>
        /// ID сообщения в системе СМЭВ
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime CalcDate { get; set; }

        /// <summary>
        /// RealityObject
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// ИНН в запросе
        /// </summary>
        public virtual Room Room { get; set; }

        /// <summary>
        /// Кадастровый номер
        /// </summary>
        public virtual string CadastralNumber { get; set; }

        /// <summary>
        /// Уровень запроса
        /// </summary>
        public virtual PublicPropertyLevel PublicPropertyLevel { get; set; }

        /// <summary>
        /// Текущее состояние запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// Доп данные для запроса, хз че мсжет понадобиться
        /// </summary>
        public virtual string Attr1 { get; set; }
        /// <summary>
        /// Доп данные для запроса, хз че мсжет понадобиться 
        /// </summary>
        public virtual string Attr2 { get; set; }
        /// <summary>
        /// Доп данные для запроса, хз че мсжет понадобиться 
        /// </summary>
        public virtual string Attr3 { get; set; }
        /// <summary>
        /// Доп данные для запроса, хз че мсжет понадобиться 
        /// </summary>
        public virtual string Attr4 { get; set; }
        /// <summary>
        /// Доп данные для запроса, хз че мсжет понадобиться 
        /// </summary>
        public virtual string Attr5 { get; set; }

    }
}
