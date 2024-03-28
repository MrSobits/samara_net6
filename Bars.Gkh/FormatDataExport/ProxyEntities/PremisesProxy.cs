namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Помещение
    /// </summary>
    public class PremisesProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код помещения в системе отправителя
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Уникальный идентификатор дома
        /// </summary>
        [ProxyId(typeof(HouseProxy))]
        public long? HouseId { get; set; }

        /// <summary>
        /// 3. Тип помещения
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 4. Помещение имеет отдельный вход
        /// </summary>
        public int? HasSeparateEntrace { get; set; }

        /// <summary>
        /// 5. Уникальный идентификатор подъезда
        /// </summary>
        [ProxyId(typeof(EntranceProxy))]
        public long? EntraceId { get; set; }

        /// <summary>
        /// 6. Номер помещения
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 7. Нежилое помещение является общим имуществом в МКД
        /// </summary>
        public int? IsCommonProperty { get; set; }

        /// <summary>
        /// 8. Характеристика жилого помещения
        /// </summary>
        public int? TypeHouse { get; set; }

        /// <summary>
        /// 9. Общая площадь помещения по паспорту помещения
        /// </summary>
        public decimal? Area { get; set; }

        /// <summary>
        /// 10. Наличие сведений о жилой площади помещения
        /// </summary>
        public int? HasLivingArea { get; set; }

        /// <summary>
        /// 11. Жилая площадь помещения по паспорту помещения
        /// </summary>
        public decimal? LivingArea { get; set; }

        /// <summary>
        /// 12. Наличие сведений о кадастровом номере или условном номере дома в ЕГРП/ГКН
        /// </summary>
        public int? HasCadastralHouseNumber { get; set; }

        /// <summary>
        /// 13. Кадастровый номер
        /// </summary>
        public string CadastralHouseNumber { get; set; }

        /// <summary>
        /// 14. Условный номер ЕГРП
        /// </summary>
        public string EigrpNumber { get; set; }

        /// <summary>
        /// 15. Этаж
        /// </summary>
        public int? Floor { get; set; }

        /// <summary>
        /// 16. Дата прекращения существования объекта
        /// </summary>
        public DateTime? TerminataionDate { get; set; }

        /// <summary>
        /// 17. Иные характеристики нежилого помещения
        /// </summary>
        public string NonLivingPremisesOtherInfo { get; set; }

        /// <summary>
        /// 18. Иные характеристики квартиры
        /// </summary>
        public string LivingPremisesOtherInfo { get; set; }

        /// <summary>
        /// 19. Наличие факта признания квартиры непригодной для проживания
        /// </summary>
        public int? HasRecognizedUnfit { get; set; }

        /// <summary>
        /// 20. Основание признания квартиры непригодной для проживания
        /// </summary>
        public string RecognizedUnfitReason { get; set; }

        /// <summary>
        /// 21. Дата документа, содержащего решение о признании квартиры непригодной для проживания
        /// </summary>
        public DateTime? RecognizedUnfitDocDate { get; set; }

        /// <summary>
        /// 22. Номер документа, содержащего решение о признании квартиры непригодной для проживания
        /// </summary>
        public string RecognizedUnfitDocNumber { get; set; }

        /// <summary>
        /// 23. Информация подтверждена поставщиком, ответственным за размещение сведений
        /// </summary>
        public int? IsSupplierConfirmed { get; set; }

        /// <summary>
        /// 24. Отсутствует установленный ПУ (ИПУ или Общий (квартирный) ПУ)
        /// </summary>
        public int? IsDeviceNotInstalled { get; set; }

        /// <summary>
        /// 25. Причина отсутствия ПУ
        /// </summary>
        public int? DeviceNotInstalledReason { get; set; }
    }
}
