namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Прокси для лицевого счета
    /// </summary>
    public class KvarProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код лицевого счета в системе отправителя
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Уникальный номер лицевого счета в системе отправителя
        /// </summary>
        public string PersonalAccountNum { get; set; }

        /// <summary>
        /// 3. Уникальный идентификатор дома
        /// </summary>
        [ProxyId(typeof(HouseProxy))]
        public long? RealityObjectId { get; set; }

        /// <summary>
        /// 4. Фамилия абонента
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// 5. Имя абонента
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 6. Отчество абонента
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// 7. Дата рождения абонента
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// 8. Дата открытия ЛС
        /// </summary>
        public DateTime OpenDate { get; set; }

        /// <summary>
        /// 9. Дата закрытия ЛС
        /// </summary>
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// 10. Причина закрытия ЛС
        /// </summary>
        public string CloseReasonType { get; set; }

        /// <summary>
        /// 11. Основание закрытия ЛС
        /// </summary>
        public string CloseReason { get; set; }

        /// <summary>
        /// 12. Количество проживающих
        /// </summary>
        public int? ResidentCount { get; set; }

        /// <summary>
        /// 13. Количество временно прибывших жильцов
        /// </summary>
        public int? TempInResidentCount { get; set; }

        /// <summary>
        /// 14. Количество временно убывших жильцов
        /// </summary>
        public int? TempOutResidentCount { get; set; }

        /// <summary>
        /// 15. Количество комнат
        /// </summary>
        public int? RoomCount { get; set; }

        /// <summary>
        /// 16. Общая площадь (площадь применяемая для расчета большинства площадных услуг)
        /// </summary>
        public decimal? Area { get; set; }

        /// <summary>
        /// 17. Жилая площадь
        /// </summary>
        public decimal? LivingArea { get; set; }

        /// <summary>
        /// 18. Отапливаемая площадь
        /// </summary>
        public decimal? HeatedArea { get; set; }

        /// <summary>
        /// 19. Площадь для найма
        /// </summary>
        public decimal? RentArea { get; set; }

        /// <summary>
        /// 20. Наличие электрической плиты
        /// </summary>
        public int? HasElecticStove { get; set; }

        /// <summary>
        /// 21. Наличие газовой плиты
        /// </summary>
        public int? HasGasStove { get; set; }

        /// <summary>
        /// 22. Наличие газовой колонки
        /// </summary>
        public int? HasGeyser { get; set; }

        /// <summary>
        /// 23. Наличие огневой плиты
        /// </summary>
        public int? HasFireStove { get; set; }

        /// <summary>
        /// 24. Код типа жилья по газоснабжению
        /// </summary>
        public long? GasHouseTypeCode { get; set; }

        /// <summary>
        /// 25. Код типа жилья по водоснабжению
        /// </summary>
        public long? WaterHouseTypeCode { get; set; }

        /// <summary>
        /// 26. Код типа жилья по горячей воде
        /// </summary>
        public long? HotWaterHouseTypeCode { get; set; }

        /// <summary>
        /// 27. Код типа жилья по канализации
        /// </summary>
        public long? SewerHouseTypeCode { get; set; }

        /// <summary>
        /// 28. Наличие забора из открытой системы отопления
        /// </summary>
        public long? HasOpenHeatingSystem { get; set; }

        /// <summary>
        /// 29. Участок (ЖЭУ)
        /// </summary>
        public string HousingDepartmentPlace { get; set; }

        /// <summary>
        /// 30. Код контрагента, с которым у потребителя ЖКУ заключен договор на оказание услуг (принципал)
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? PrincipalContragentId { get; set; }

        /// <summary>
        /// 31. Тип лицевого счета
        /// </summary>
        public int? PersonalAccountType { get; set; }

        /// <summary>
        /// Контрагент РКЦ
        /// </summary>
        public long? CashPaymentCenterContragentId { get; set; }

        /// <summary>
        /// 32. Плательщик – Физ.лицо
        /// </summary>
        [ProxyId(typeof(IndProxy))]
        public long? IndividualOwner { get; set; }

        /// <summary>
        /// 33. Плательщик – Организация
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? ContragentId { get; set; }

        /// <summary>
        /// 34. Плательщик является нанимателем
        /// </summary>
        public int? IsArendator { get; set; }

        /// <summary>
        /// 35. ЛС на помещение(-я) разделены
        /// </summary>
        public int? IsPartial { get; set; }

        /// <summary>
        /// 36. Статус лицевого счета
        /// </summary>
        public int? State { get; set; }
        
        /// <summary>
        /// Владелец ЛС
        /// </summary>
        public long? OwnerId { get; set; }

        /// <summary>
        /// KVARACCCOM 2. Уникальный идентификатор помещения
        /// </summary>
        [ProxyId(typeof(PremisesProxy))]
        public long? PremisesId { get; set; }

        /// <summary>
        /// KVARACCCOM 3. Уникальный идентификатор дома (Не передается)
        /// </summary>
        public long? RoId { get; set; }

        /// <summary>
        /// KVARACCCOM 4. Уникальный идентификатор комнаты (Не передается)
        /// </summary>
        public long? RoomId { get; set; }

        /// <summary>
        /// KVARACCCOM 5. Доля внесения платы, размер доли в %
        /// </summary>
        public decimal? Share { get; set; }

        /// <summary>
        /// KVAROPENREASON 2. Тип основания открытия ЛС
        /// </summary>
        public int? ReasonType { get; set; }

        /// <summary>
        /// KVAROPENREASON 3. Договор управления
        /// </summary>
        [ProxyId(typeof(DuProxy))]
        public long? DuId { get; set; }

        /// <summary>
        /// KVAROPENREASON 4. Устав
        /// </summary>
        [ProxyId(typeof(UstavProxy))]
        public long? UstavId { get; set; }

        /// <summary>
        /// KVAROPENREASON 5. Решение о выбранном способе формирования фонда КР
        /// </summary>
        [ProxyId(typeof(KapremDecisionsProxy))]
        public long? KapremDecisionId { get; set; }

        /// <summary>
        /// KVAROPENREASON 6. Договор найма отсутствует в системе отправителя
        /// </summary>
        public string Param6 { get; set; }

        /// <summary>
        /// KVAROPENREASON 7. Договор найма жилого помещения
        /// </summary>
        public string Param7 { get; set; }

        /// <summary>
        /// KVAROPENREASON 8. Номер договора найма жилого помещения
        /// </summary>
        public string Param8 { get; set; }

        /// <summary>
        /// KVAROPENREASON 9. Дата заключения договора найма жилого помещения
        /// </summary>
        public string Param9 { get; set; }

        /// <summary>
        /// KVAROPENREASON 10. Тип договора найма жилого помещения
        /// </summary>
        public string Param10 { get; set; }
    }
}
