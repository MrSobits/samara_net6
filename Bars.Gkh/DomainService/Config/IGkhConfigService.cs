namespace Bars.Gkh.DomainService.Config
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Config.Impl.Internal;

    using Newtonsoft.Json;

    /// <summary>
    /// Сервис конфигурации приложения
    /// </summary>
    public interface IGkhConfigService
    {
        /// <summary>
        /// Получение списка дочерних элементов
        /// </summary>
        /// <param name="parent">Идентификатор родительского элемента</param>
        /// <returns></returns>
        MetaItem[] GetItems(string parent);

        /// <summary>
        /// Получение списка корневых элементов
        /// </summary>
        /// <returns></returns>
        MetaItem[] GetRoots();

        /// <summary>
        /// Обновление конфигурации
        /// </summary>
        /// <param name="config">Сериализованный словарь параметров</param>
        /// <param name="errors">Список ошибок</param>
        /// <returns>Ошибка сохранения</returns>
        bool UpdateConfigs(string config, out IDictionary<string, string> errors);

        /// <summary>
        /// Получение всех параметров конфигурации
        /// </summary>
        /// <returns></returns>
        IDictionary<string, object> GetAllConfigs();

        /// <summary>
        /// Получить сериализованный JSON объект всех параметров
        /// </summary>
        string GetSerializedConfig();

        /// <summary>
        /// Получить сериализованный JSON объект параметров
        /// </summary>
        string GetSerializedConfig(IDictionary<string, ValueHolder> configParams);
    }

    /// <summary>
    /// База для метаописания типа параметра конфигурации
    /// </summary>
    public interface ITypeDescription
    {
        /// <summary>
        /// Тип редактора параметра
        /// </summary>
        string Editor { get; }
    }

    /// <summary>
    /// Метаописание параметра конфигурации
    /// </summary>
    public class MetaItem
    {
        /// <summary>
        /// Дочерние элементы
        /// </summary>
        [JsonProperty("children")]
        public MetaItem[] Children { get; set; }

        /// <summary>
        /// Отображаемое имя
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Идентификатор параметра
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Метаописание типа параметра конфигурации
        /// </summary>
        [JsonProperty("type")]
        public ITypeDescription Type { get; set; }

        /// <summary>
        /// Значение параметра конфигурации
        /// </summary>
        [JsonProperty("value")]
        public ValueHolder Value { get; set; }

        /// <summary>
        /// Значение параметра по умолчанию
        /// </summary>
        [JsonProperty("defaultValue")]
        public object DefaultValue { get; set; }

        /// <summary>
        /// Имя группы
        /// </summary>
        [JsonProperty("groupName")]
        public string GroupName { get; set; }

        /// <summary>
        /// Дополнительные параметры для передачи на клиент
        /// </summary>
        [JsonProperty("extraParams")]
        public IDictionary<string, object> ExtraParams { get; set; }
    }
}