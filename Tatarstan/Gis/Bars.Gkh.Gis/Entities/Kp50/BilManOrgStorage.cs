namespace Bars.Gkh.Gis.Entities.Kp50
{
    using B4.DataAccess;

    /// <summary>
    /// Управляющие организации (УО) биллинга
    /// </summary>
    public class BilManOrgStorage : PersistentObject
    {
        /// <summary>
        /// Код префикса схемы в БД биллинга
        /// </summary>
        public virtual BilDictSchema Schema { get; set; }
        
        /// <summary>
        /// Код УО в биллинге
        /// </summary>
        public virtual int ManOrgCode { get; set; }

        /// <summary>
        /// Наименование УО
        /// </summary>
        public virtual string ManOrgName { get; set; }

        /// <summary>
        /// ИНН УО
        /// </summary>
        public virtual string ManOrgInn { get; set; }

        /// <summary>
        /// КПП УО
        /// </summary>
        public virtual string ManOrgKpp { get; set; }

        /// <summary>
        /// ОГРН УО (в биллинге пока нет данного атрибута)
        /// </summary>
        public virtual string ManOrgOgrn { get; set; }

        /// <summary>
        /// адрес УО (юр. или факт. - неизвестно)
        /// </summary>
        public virtual string ManOrgAddress { get; set; }

    }
}