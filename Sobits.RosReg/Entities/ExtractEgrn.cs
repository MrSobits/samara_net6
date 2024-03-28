namespace Sobits.RosReg.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class ExtractEgrn : PersistentObject
    {
        /// <inheritdoc />
        public override long Id { get; set; }

        /// <summary>
        /// Кадастровый номер
        /// </summary>
        public virtual string CadastralNumber { get; set; }

        /// <summary>
        /// Площадь
        /// </summary>
        public virtual decimal Area { get; set; }

        /// <summary>
        /// Тип собственности
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// Назначение помещения
        /// </summary>
        public virtual string Purpose { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Площадь
        /// </summary>
        public virtual DateTime ExtractDate { get; set; }

        /// <summary>
        /// Привязка к комнате в системе
        /// </summary>
        public virtual Room RoomId { get; set; }

        /// <summary>
        /// Номер выписки
        /// </summary>
        public virtual string ExtractNumber { get; set; }

        /// <summary>
        /// Привязка к комнате в системе
        /// </summary>
        public virtual Int64 Room_id { get; set; }

        /// <summary>
        /// Выписка
        /// </summary>
        public virtual Extract ExtractId { get; set; }

        /// <summary>
        /// адрес помещения
        /// </summary>
        public virtual string FullAddress { get; set; }

        /// <summary>
        /// сопоставлена
        /// </summary>
        public virtual YesNoNotSet IsMerged { get; set; }
    }
}