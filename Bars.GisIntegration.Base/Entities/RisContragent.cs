namespace Bars.GisIntegration.Base.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Контрагент
    /// </summary>
    public class RisContragent : BaseEntity
    {
        /// <summary>
        /// Id в системе, из которой загружен
        /// </summary>
        public virtual long GkhId { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public virtual string Ogrn { get; set; }

        /// <summary>
        /// Идентификатор контрагента в ГИС (== GUID в RisEntity)
        /// </summary>
        public virtual string OrgRootEntityGuid { get; set; }

        /// <summary>
        /// OrgVersionGuid
        /// </summary>
        public virtual string OrgVersionGuid { get; set; }

        /// <summary>
        /// Фактический адрес
        /// </summary>
        public virtual string FactAddress { get; set; }

        /// <summary>
        /// Юридический адрес
        /// </summary>
        public virtual string JuridicalAddress { get; set; }

        /// <summary>
        /// Тип организации в ГИС
        /// </summary>
        public virtual GisOrganizationType? OrganizationType { get; set; }

        /// <summary>
        /// Идентификатор зарегистрированной в ГИС организации
        /// </summary>
        public virtual string OrgPpaGuid { get; set; }

        /// <summary>
        /// Номер записи об аккредитации
        /// </summary>
        public virtual string AccreditationRecordNumber { get; set; }
    }
}