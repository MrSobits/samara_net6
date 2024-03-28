namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Представление "Жилой дом"
    /// </summary>
    /*
     * Вьюха жилых домов, для вывода агрегированных полей "Наименование управляющей организации" и "Типы договоров с управляющей организацией"
     */
    public class ViewRealityObject : PersistentObject
    {
        /// <summary>
        /// Идентификатор жилого дома
        /// </summary>
        public virtual int? RealityObjectId { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Полный адрес
        /// </summary>
        public virtual string FullAddress { get; set; }

        /// <summary>
        /// Тип дома
        /// </summary>
        public virtual TypeHouse TypeHouse { get; set; }

        /// <summary>
        /// Состояние дома
        /// </summary>
        public virtual ConditionHouse ConditionHouse { get; set; }

        /// <summary>
        /// Дата сноса
        /// </summary>
        public virtual DateTime? DateDemolition { get; set; }

        /// <summary>
        /// Этажность
        /// </summary>
        public virtual int? Floors { get; set; }

        /// <summary>
        /// Количество подъездов
        /// </summary>
        public virtual int? NumberEntrances { get; set; }

        /// <summary>
        /// Количество проживающих
        /// </summary>
        public virtual int? NumberLiving { get; set; }

        /// <summary>
        /// Количество квартир
        /// </summary>
        public virtual int? NumberApartments { get; set; }

        /// <summary>
        /// Площадь МКД
        /// </summary>
        public virtual decimal? AreaMkd { get; set; }

        /// <summary>
        /// Площадь жилая, всего (кв. м.)
        /// </summary>
        public virtual decimal? AreaLiving { get; set; }

        /// <summary>
        /// Физические повреждение
        /// </summary>
        public virtual decimal? PhysicalWear { get; set; }

        /// <summary>
        /// Количество лифтов
        /// </summary>
        public virtual int? NumberLifts { get; set; }

        /// <summary>
        /// Отопительная система
        /// </summary>
        public virtual HeatingSystem HeatingSystem { get; set; }

        /// <summary>
        /// Тип крыши
        /// </summary>
        public virtual TypeRoof TypeRoof { get; set; }

        /// <summary>
        /// Дата последнего кап ремонта
        /// </summary>
        public virtual DateTime? DateLastRenovation { get; set; }

        /// <summary>
        /// Дата сдачи объекта в эксплуатацию
        /// </summary>
        public virtual DateTime? DateCommissioning { get; set; }

        /// <summary>
        /// Код ЕРЦ
        /// </summary>
        public virtual string CodeErc { get; set; }

        /// <summary>
        /// Объект застрахован
        /// </summary>
        public virtual bool? IsInsuredObject { get; set; }

        /// <summary>
        /// Идентификатор материала крыши
        /// </summary>
        public virtual int? RoofingMaterialId { get; set; }

        /// <summary>
        /// Материал крыши
        /// </summary>
        public virtual string RoofingMaterial { get; set; }

        /// <summary>
        /// Идентификатор материала стен
        /// </summary>
        public virtual int? WallMaterialId { get; set; }

        /// <summary>
        /// Материал стен
        /// </summary>
        public virtual string WallMaterial { get; set; }

        /// <summary>
        /// Муниципальный район
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual string Settlement { get; set; }

        /// <summary>
        /// Идентификатор муниципального образования (для фильтрации по операторам)
        /// </summary>
        public virtual int MunicipalityId { get; set; }

        /// <summary>
        /// Идентификатор муниципального образования (для фильтрации по операторам)
        /// </summary>
        public virtual int? SettlementId { get; set; }

        /// <summary>
        /// Управляющие организации
        /// </summary>
        public virtual string ManOrgNames { get; set; }

        /// <summary>
        /// Типы договоров (управление домом)
        /// </summary>
        public virtual string TypeContracts { get; set; }

        /// <summary>
        /// Внутрнний Код дома
        /// </summary>
        public virtual string GkhCode { get; set; }

        /// <summary>
        /// Дом построен по соц.ипотеке(да/нет)
        /// </summary>
        public virtual YesNo IsBuildSocialMortgage { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Ремонт нецелесообразен
        /// </summary
        public virtual bool? IsRepairInadvisable { get; set; }

        /// <summary>
        /// Дом не участвует в КР (в программе кап. ремонта)
        /// </summary>
        public virtual bool IsNotInvolvedCr { get; set; }

        public virtual string ExternalId { get; set; }

        /// <summary>
        /// Имеется видеонаблюдение
        /// </summary>
        public virtual bool HasVidecam { get; set; }

        /// <summary>
        /// Округ
        /// </summary>
        public virtual string District { get; set; }
    }
}
