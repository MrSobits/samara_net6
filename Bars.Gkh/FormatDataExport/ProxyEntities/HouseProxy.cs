namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Прокси для Дом
    /// </summary>
    public class HouseProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код дома в системе отправителя
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Город/район
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 3. Населенный пункт
        /// </summary>
        public string Settlement { get; set; }

        /// <summary>
        /// 4. Улица
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// 5. Номер дома
        /// </summary>
        public string House { get; set; }

        /// <summary>
        /// 6. Строение (секция)
        /// </summary>
        public string Building { get; set; }

        /// <summary>
        /// 7. Корпус
        /// </summary>
        public string Housing { get; set; }

        /// <summary>
        /// 8. Литера
        /// </summary>
        public string Letter { get; set; }

        /// <summary>
        /// 9. Код дома ФИАС
        /// </summary>
        public string HouseGuid { get; set; }

        /// <summary>
        /// 10. Код ОКТМО
        /// </summary>
        public string OktmoCode { get; set; }

        /// <summary>
        /// 11. Часовая зона
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// 12. Наличие сведений о кадастровом номере или условном номере дома в ЕГРП/ГКН
        /// </summary>
        public int? IsNumberExists { get; set; }

        /// <summary>
        /// 13. Кадастровый номер в ГКН
        /// </summary>
        public string CadastralHouseNumber { get; set; }

        /// <summary>
        /// 14. Условный номер ЕГРП
        /// </summary>
        public string EgrpNumber { get; set; }

        /// <summary>
        /// 15. Тип дома
        /// </summary>
        public int? TypeHouse { get; set; }

        /// <summary>
        /// 16. Несколько домов с одинаковым адресом
        /// </summary>
        public int? ManyHouseWithOneAddress { get; set; }

        /// <summary>
        /// 17. Общая площадь
        /// </summary>
        public decimal? AreaMkd { get; set; }

        /// <summary>
        /// 18. Состояние дома
        /// </summary>
        public int? ConditionHouse { get; set; }

        /// <summary>
        /// 19. Год ввода в эксплуатацию (указывается 1 число года, например 01.01.1900)
        /// </summary>
        public DateTime? CommissioningYear { get; set; }

        /// <summary>
        /// 20. Количество этажей (максимальное кол-во этажей в доме)
        /// </summary>
        public int? MaximumFloors { get; set; }

        /// <summary>
        /// 21. Минимальная этажность (минимальное кол-во этажей в доме)
        /// </summary>
        public int? MinimumFloors { get; set; }

        /// <summary>
        /// 22. Количество подземных этажей
        /// </summary>
        public int UndergroundFloorCount { get; set; }

        /// <summary>
        /// 23. Наличие у дома статуса объекта культурного наследия
        /// </summary>
        public int? IsCulturalHeritage { get; set; }

        /// <summary>
        /// 24. Площадь мест общего пользования
        /// </summary>
        public decimal? AreaCommonUsage { get; set; }

        /// <summary>
        /// 25. Полезная (отапливаемая площадь)
        /// </summary>
        public decimal? HeatingArea { get; set; }

        /// <summary>
        /// 26. Год постройки (указывается 1 число года, например 01.01.1900)
        /// </summary>
        public DateTime? BuildYear { get; set; }

        /// <summary>
        /// 27. Количество лицевых счетов в доме
        /// </summary>
        public int? PersonalAccountCount { get; set; }

        /// <summary>
        /// 28. Общая площадь жилых помещений по паспорту помещения
        /// </summary>
        public decimal? AreaLiving { get; set; }

        /// <summary>
        /// 29. Способ формирования фонда капитального ремонта
        /// </summary>
        public int? AccountFormationVariant { get; set; }

        /// <summary>
        /// 30. Способ управления домом
        /// </summary>
        public int? TypeManagement { get; set; }

        /// <summary>
        /// 31. Стадия жизненного цикла
        /// </summary>
        public string LifeCycleStage { get; set; }

        /// <summary>
        /// 32. Год проведения реконструкции
        /// </summary>
        public DateTime? ReconstructionYear { get; set; }

        /// <summary>
        /// 33. Код контрагента, с которым заключен договор на управление домом
        /// </summary>
        [ProxyId(typeof(UoProxy))]
        public long? ContragentId { get; set; }

        /// <summary>
        /// 34. Категория благоустроенности
        /// </summary>
        public int? ComfortСategory { get; set; }

        /// <summary>
        /// 35. Код Улицы ФИАС
        /// </summary>
        public DateTime? DestroyDate { get; set; }

        /// <summary>
        /// 36. Отсутствует установленный ПУ
        /// </summary>
        public int NoInstallationPu { get; set; }

        /// <summary>
        /// 37. Причина отсутствия ПУ
        /// </summary>
        public int? ReasonAbsencePu { get; set; }
    }
}