namespace Bars.B4.Modules.FIAS
{
    using System;
    using Bars.B4.Modules.FIAS.Enums;
    using DataAccess;

    /// <summary>
    /// Запись идентификатора дома из справочника ФИАС
    /// </summary>
    public class FiasHouse : BaseEntity
    {
        /// <summary>
        /// Уникальный идентификатор записи дома
        /// </summary>
        public virtual Guid? HouseId { get; set; }

        /// <summary>
        /// Глобальный уникальный идентификатор дома
        /// </summary>
        public virtual Guid? HouseGuid { get; set; }

        /// <summary>
        /// Guid записи родительского объекта
        /// </summary>
        public virtual Guid AoGuid { get; set; }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public virtual string PostalCode { get; set; }

        /// <summary>
        /// ОКАТО
        /// </summary>
        public virtual string Okato { get; set; }

        /// <summary>
        /// ОКТМО
        /// </summary>
        public virtual string Oktmo { get; set; }

        /// <summary>
        /// Номер дома
        /// </summary>
        public virtual string HouseNum { get; set; }

        /// <summary>
        /// Номер корпуса 
        /// </summary>
        public virtual string BuildNum { get; set; }

        /// <summary>
        /// Номер строения
        /// </summary>
        public virtual string StrucNum { get; set; }

        /// <summary>
        /// Состояние актуальности
        /// </summary>
        public virtual FiasActualStatusEnum ActualStatus { get; set; }

        /// <summary>
        /// Дата  внесения записи
        /// </summary>
        public virtual DateTime? UpdateDate { get; set; }

        /// Если мы добавляем новую запись, то проставляется - TypeRecord.User
        /// Если мы загружаем запись из ФИАС, то проставляется - TypeRecord.Fias
        public virtual FiasTypeRecordEnum TypeRecord { get; set; }

        /// <summary>
        /// Начало действия записи
        /// <para>Загружается при обновлении ФИАС</para>
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        /// <summary>
        /// Окончание действия записи
        /// <para>Загружается при обновлении ФИАС</para>
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        ///<summary>
        /// Вид строения
        /// </summary>
        public virtual FiasStructureTypeEnum StructureType { get; set; }

        /// <summary>
        /// Признак владения
        /// </summary>
        public virtual FiasEstimateStatusEnum EstimateStatus { get; set; }
    }
}
