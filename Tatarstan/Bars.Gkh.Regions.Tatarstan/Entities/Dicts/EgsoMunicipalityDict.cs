namespace Bars.Gkh.Regions.Tatarstan.Entities.Dicts
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Словарь МО для интеграции с ЕГСО ОВ
    /// </summary>
    public class EgsoMunicipalityDict : PersistentObject
    {
        /// <summary>
        /// Наименование территории
        /// </summary>
        public virtual string TerritoryName { get; set; }

        /// <summary>
        /// Код территории
        /// </summary>
        public virtual string TerritoryCode { get; set; }

        /// <summary>
        /// Ключ ЕГСО
        /// </summary>
        public virtual string EgsoKey { get; set; }
    }
}