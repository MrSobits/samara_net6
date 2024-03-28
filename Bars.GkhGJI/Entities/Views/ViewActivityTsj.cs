namespace Bars.GkhGji.Entities
{
    using B4.DataAccess;

    /// <summary>
    /// Вьюха деятельности тсж
    /// </summary>
    public class ViewActivityTsj : PersistentObject
    {
        /// <summary>
        /// Наименование управляющей организации
        /// </summary>
        public virtual string ManOrgName { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual string MunicipalityName { get; set; }

        /// <summary>
        /// ИНН контрагента управляющей организации
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// Наличие устава
        /// </summary>
        public virtual bool? HasStatute { get; set; }

        /// <summary>
        /// Идентификатор контрагента
        /// </summary>
        public virtual long? ContragentId { get; set; }

        /// <summary>
        /// Идентификатор муниципального образования контрагента
        /// </summary>
        public virtual long? MunicipalityId { get; set; }
    }
}