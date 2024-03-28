namespace Bars.Gkh.Gis.Entities.House.Claims
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Utils;

    /// <summary>
    /// Заявка из Открытой Казани
    /// </summary>
    [DataContract]
    class OrderInfoOk
    {
        /// <summary>
        /// Идентификатор заявки
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Платежный код
        /// </summary>
        [DataMember(Name = "pa")]
        public long PaymentAccount { get; set; }

        /// <summary>
        /// Город
        /// </summary>
        [DataMember(Name = "city")]
        public string City { get; set; }

        /// <summary>
        /// Район
        /// </summary>
        [DataMember(Name = "district")]
        public string District { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        [DataMember(Name = "street")]
        public string Street { get; set; }

        /// <summary>
        /// Номер дома
        /// </summary>
        [DataMember(Name = "housenumber")]
        public string HouseNumber { get; set; }

        /// <summary>
        /// Корпус
        /// </summary>
        [DataMember(Name = "buildingnumber")]
        public string BuildingNumber { get; set; }

        /// <summary>
        /// Номер квартиры
        /// </summary>
        [DataMember(Name = "flatnumber")]
        public string FlatNumber { get; set; }

        /// <summary>
        /// Организация
        /// </summary>
        [DataMember(Name = "organization")]
        public string Organization { get; set; }

        /// <summary>
        /// Текст
        /// </summary>
        [DataMember(Name = "Text")]
        public string Text { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        [DataMember(Name = "created")]
        [JsonConverter(typeof(TimestampDateConverter))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Created { get; set; }

        /// <summary>
        /// Дата по нормативу
        /// </summary>
        [DataMember(Name = "normaltime")]
        [JsonConverter(typeof(TimestampDateConverter))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? NormalTime { get; set; }

        /// <summary>
        /// Дата выполнения
        /// </summary>
        [DataMember(Name = "performtime")]
        [JsonConverter(typeof(TimestampDateConverter))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? PerformTime { get; set; }

        /// <summary>
        /// Является проблемой
        /// </summary>
        [DataMember(Name = "isproblem")]
        public string IsProblem { get; set; }

        /// <summary>
        /// Идентификатор проблемы
        /// </summary>
        [DataMember(Name = "problemId")]
        public int ProblemId { get; set; }

        /// <summary>
        /// Текст проблемы
        /// </summary>
        [DataMember(Name = "problemText")]
        public string ProblemText { get; set; }

        /// <summary>
        /// Статусы
        /// </summary>
        [DataMember(Name = "states")]
        public Dictionary<long, long?> States { get; set; }
    }
}
