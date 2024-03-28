namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;

    /// <summary>
    /// Справочник услуг
    /// </summary>
    public class DictUslugaProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Наименование услуги
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 3. Краткое наименование услуги
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 4. Тип услуги
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 5. Тип предоставления услуги
        /// </summary>
        public int ServiceType { get; set; }

        /// <summary>
        /// 6. Код базовой услуги
        /// </summary>
        public int? BaseCode { get; set; }

        /// <summary>
        /// 7. Вид коммунальной услуги
        /// </summary>
        public int? CommunalServiceType { get; set; }

        /// <summary>
        /// 8. Вид жилищной услуги
        /// </summary>
        public int? HousingServiceType { get; set; }

        /// <summary>
        /// 9. Тип учета электроэнергии
        /// </summary>
        public int? ElectricMeteringType { get; set; }

        /// <summary>
        /// 10. Код ОКЕИ
        /// </summary>
        public string OkeiCode { get; set; }

        /// <summary>
        /// 11. Другая единица измерения
        /// </summary>
        public string AnotherUnit { get; set; }

        /// <summary>
        /// 12. Вид коммунального ресурса
        /// </summary>
        public int? CommunalResourceType { get; set; }

        /// <summary>
        /// 13. Порядок сортировки
        /// </summary>
        public int? SortOrder { get; set; }

        /// <summary>
        /// 14. Порядок сортировки не задан
        /// </summary>
        public int? IsNotSortOrder { get; set; }

        /// <summary>
        /// Идентификатор услуги в договоре РСО
        /// </summary>
        public long? DrsoServiceId { get; set; }

        /// <summary>
        /// Идентификатор шаблонной услуги WORKUSLUGA
        /// </summary>
        public long? TemplateServiceId { get; set; }
    }
}