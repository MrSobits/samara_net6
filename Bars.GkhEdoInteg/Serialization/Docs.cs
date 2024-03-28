namespace Bars.GkhEdoInteg.Serialization
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public sealed class Docs
    {
        /// <summary>
        /// идентификатор электронного документооборота
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Дата актуальности
        /// </summary>
        [JsonProperty("a_date")]
        public DateTime? DateActual { get; set; }

        /// <summary>
        /// Номер ГЖИ
        /// </summary>
        [JsonProperty("in_num")]
        public string NumberGji { get; set; }

        /// <summary>
        ///  От
        /// </summary>
        [JsonProperty("reg_date")]
        public DateTime? RegDate { get; set; }

        /// <summary>
        /// Исх.№ источника (Источник поступления)
        /// </summary>
        [JsonProperty("src_num")]
        public string SrcNum { get; set; }

        /// <summary>
        /// Корреспондент
        /// </summary>
        [JsonProperty("authors")]
        public Dictionary<string, Correspondent> Authors { get; set; }

        /// <summary>
        /// Дата (Источник поступления)
        /// </summary>
        [JsonProperty("in_date")]
        public DateTime? InDate { get; set; }

        /// <summary>
        /// Вид обращения (Реквизиты)
        /// </summary>
        [JsonProperty("doc_type")]
        public int DocType { get; set; }

        /// <summary>
        /// Описание (Реквизиты)
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// ФИО поручителя (Рассмотрение)
        /// </summary>
        [JsonProperty("res_author")]
        public string ResAuthor { get; set; }

        /// <summary>
        /// Контрольный срок
        /// </summary>
        [JsonProperty("res_contol_date")]
        public DateTime? ResContolDate { get; set; }

        /// <summary>
        /// Срок исполнения (Поручитель, Рассмотрение)
        /// </summary>
        [JsonProperty("res_to_date")]
        public DateTime? ResToDate { get; set; }

        /// <summary>
        /// ФИО исполнителя (Рассмотрение)
        /// </summary>
        [JsonProperty("executant")]
        public string Executant { get; set; }

        /// <summary>
        /// Срок исполнения (Исполнитель, Рассмотрение)
        /// </summary>
        [JsonProperty("exec_date")]
        public DateTime? ExecDate { get; set; }

        /// <summary>
        /// Проверяющий (инспектор) (Рассмотрение)
        /// </summary>
        [JsonProperty("inspector")]
        public string Inspector { get; set; }

        /// <summary>
        /// Идентификатор скана обращения гражданина
        /// </summary>
        [JsonProperty("attachments")]
        public DocumentEdo Attachments { get; set; }

        /// <summary>
        /// Форма поступления
        /// </summary>
        [JsonProperty("delivery")]
        public string Delivery { get; set; }

        /// <summary>
        /// Источник
        /// </summary>
        [JsonProperty("recipients")]
        public Dictionary<string, string> Recipients { get; set; }
    }
}
