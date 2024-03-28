namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Dict
{
    using System;
    using System.Collections.Generic;
    using Bars.B4.Modules.FIAS;
    using Newtonsoft.Json;

    /// <summary>
    /// Модель для API метода ФИАСа
    /// </summary>
    public class FiasResponse
    {
        /// <summary>
        /// Объекты
        /// </summary>
        public IEnumerable<FiasObject> Objects { get; set; }

        /// <summary>
        /// Дома
        /// </summary>
        public IEnumerable<MobileFiasHouse> Houses { get; set; }
    }

    /// <summary>
    /// Объект ФИАС
    /// </summary>
    public class FiasObject
    {
        /// <summary>
        /// Наименование объекта
        /// </summary>
        public string OffName { get; set; }

        /// <summary>
        /// Guid объекта ФИАС
        /// </summary>
        public string AoGuid { get; set; }

        /// <summary>
        /// Уровень объекта ФИАС
        /// </summary>
        public FiasLevelEnum AoLevel { get; set; }

        /// <summary>
        /// Краткое наименование уровня объекта
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Guid родительского объекта записи ФИАС
        /// </summary>
        public string ParentGuid { get; set; }

        /// <summary>
        /// Код населенного пункта
        /// </summary>
        public string PlaceCode { get; set; }

        /// <summary>
        /// Код улицы
        /// </summary>
        public string StreetCode { get; set; }

        /// <summary>
        /// Дата обновления
        /// </summary>
        /// <remarks>
        /// Уровень internal установлен, чтобы Swagger UI не отображал
        /// </remarks>
        internal DateTime? UpdateDate { get; set; }
        
        /// <summary>
        /// MirrorGuid объекта ФИАС
        /// </summary>
        [JsonIgnore]
        public string MirrorGuid { get; set; }
    }

    /// <summary>
    /// Дом ФИАС
    /// </summary>
    public class MobileFiasHouse
    {
        /// <summary>
        /// Guid дома
        /// </summary>
        public string HouseGuid { get; set; }

        /// <summary>
        /// Guid родительского объекта дома
        /// </summary>
        public string AoGuid { get; set; }

        /// <summary>
        /// Номер дома
        /// </summary>
        public string HouseNum { get; set; }
    }
}