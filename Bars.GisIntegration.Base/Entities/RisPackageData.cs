namespace Bars.GisIntegration.Base.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Данные хранимого пакета
    /// </summary>
    public class RisPackageData : PersistentObject
    {
        /// <summary>
        /// Сериализованный объект, содержащий набор данных Тип объекта, Идентификатор объекта, Транспортный идентификатор
        /// </summary>
        public virtual byte[] TransportGuidDictionary { get; set; }

        /// <summary>
        /// Неподписанные данные
        /// </summary>
        public virtual FileInfo Data { get; set; }

        /// <summary>
        /// Подписанные данные
        /// </summary>
        public virtual FileInfo SignedData { get; set; }
    }
}