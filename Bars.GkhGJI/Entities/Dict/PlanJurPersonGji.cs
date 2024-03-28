namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Base;

    /// <summary>
    /// План проверок юр. лиц ГЖИ
    /// </summary>
    public class PlanJurPersonGji : BaseEntity, IEntityUsedInErknm
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Дата утверждения плана
        /// </summary>
        public virtual DateTime? DateApproval { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Номер приказа
        /// </summary>
        public virtual string NumberDisposal { get; set; }

        /// <summary>
        /// Дата приказа
        /// </summary>
        public virtual DateTime? DateDisposal { get; set; }

        /// <summary>
        /// Регистрационный номер плана в едином реестре проверок
        /// </summary>
        public virtual int? UriRegistrationNumber { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID
        /// </summary>
        public virtual string GisGkhGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ Transport GUID
        /// </summary>
        public virtual string GisGkhTransportGuid { get; set; }
        
        /// <inheritdoc />
        public virtual string ErknmGuid { get; set; }
    }
}