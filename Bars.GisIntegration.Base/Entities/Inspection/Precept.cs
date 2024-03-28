namespace Bars.GisIntegration.Base.Entities.Inspection
{
    using System;
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Предписание
    /// </summary>
    public class Precept : BaseRisEntity
    {
        /// <summary>
        /// Родительская проверка
        /// </summary>
        public virtual Examination Examination { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? Date { get; set; }

        /// <summary>
        /// Адрес дома
        /// </summary>
        public virtual string FiasHouseGuid { get; set; }

        /// <summary>
        /// Отменено ли предписание
        /// </summary>
        public virtual bool IsCancelled { get; set; }

        /// <summary>
        /// Причина отмены документа - код
        /// </summary>
        public virtual string CancelReason { get; set; }

        /// <summary>
        /// Дата отмены документа
        /// </summary>
        public virtual DateTime? CancelDate { get; set; }

        /// <summary>
        /// Срок исполнения требований
        /// </summary>
        public virtual DateTime? Deadline { get; set; }

        /// <summary>
        /// Сведения об исполнении
        /// </summary>
        public virtual bool? IsFulfiledPrecept { get; set; }

        /// <summary>
        /// Причина отмены документа - GUID
        /// </summary>
        public virtual string CancelReasonGuid { get; set; }

        /// <summary>
        /// Идентификатор корневой сущности организации в реестре организаций
        /// </summary>
        public virtual string OrgRootEntityGuid { get; set; }

        /// <summary>
        /// Отменено ли предписание по причине исполнения
        /// </summary>
        public virtual bool IsCancelledAndIsFulfiled { get; set; }
    }
}
