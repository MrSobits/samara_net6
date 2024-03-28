namespace Bars.Gkh.Regions.Tatarstan.Entities.Egso
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;

    /// <summary>
    /// Таблица-связка между
    /// </summary>
    public class EgsoIntegrationValues : PersistentObject
    {
        /// <summary>
        /// Задача интеграции с ЕГСО ОВ
        /// </summary>
        public virtual EgsoIntegration EgsoIntegration { get; set; }

        /// <summary>
        /// Словарь МО
        /// </summary>
        public virtual EgsoMunicipalityDict MunicipalityDict { get; set; }

        /// <summary>
        /// Отправляемое значение
        /// </summary>
        public virtual int Value { get; set; }
    }
}