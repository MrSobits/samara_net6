namespace Bars.Gkh.ConfigSections.RegOperator
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Параметры группировки документов
    /// </summary>
    public class GroupingOptions : IGkhConfigSection
    {
        /// <summary>
        /// Группировка по адресу
        /// </summary>
        [GkhConfigProperty(DisplayName = "Группировка по адресу")]
        [DefaultValue(AddressGroupingType.FactAddressInSpecialFolder)]
        public virtual AddressGroupingType AddressGroupingType { get; set; }

        /// <summary>
        /// Группировка по административаному делению
        /// </summary>
        [GkhConfigProperty(DisplayName = "Элементы группировки")]
        public virtual List<GroupingElement> GroupingElements { get; set; }
    }

    /// <summary>
    /// Настройка использования административной единицы в группировке
    /// </summary>
    public class GroupingElement
    {
        /// <summary>
        /// Административная единица
        /// </summary>
        [Display("Элемент группировки")]
        [DefaultValue(GroupingType.Municipality)]
        public GroupingType GroupingType { get; set; }

        /// <summary>
        /// Используется
        /// </summary>
        [Display("Используется")]
        [DefaultValue(YesNo.No)]
        public YesNo IsUsed { get; set; }

        /// <summary>
        /// Порядок
        /// </summary>
        [Display("Порядок")]
        public int Order { get; set; }

        /// <summary>
        /// Использовать фактический адрес
        /// </summary>
        [Display("Использовать фактический адрес")]
        [DefaultValue(YesNo.No)]
        public YesNo UseFactAddress { get; set; }
    }

    /// <summary>
    /// Тип группировки по адресу
    /// </summary>
    public enum AddressGroupingType
    {
        /// <summary>
        /// Адрес фактического нахождения в отдельной папке
        /// </summary>
        [Display("Адрес фактического нахождения в отдельной папке")]
        FactAddressInSpecialFolder = 10,

        /// <summary>
        /// Адрес фактического нахождения в общей папке
        /// </summary>
        [Display("Адрес фактического нахождения в общей папке")]
        FactAddressInCommonFolder = 20
    }
    
    /// <summary>
    /// Административная единица
    /// </summary>
    public enum GroupingType
    {
        /// <summary>
        /// Муниципальный район
        /// </summary>
        [Display("Муниципальный район")]
        Municipality,

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        [Display("Муниципальное образование")]
        Settlement,

        /// <summary>
        /// Агент доставки
        /// </summary>
        [Display("Агент доставки")]
        DeliveryAgent,

        /// <summary>
        /// Населенный пункт
        /// </summary>
        [Display("Населенный пункт")]
        Locality,

        /// <summary>
        /// Улица
        /// </summary>
        [Display("Улица")]
        Street,

        /// <summary>
        /// Группировать по емейлу в одной квитанции
        /// </summary>
        [Display("Электронная почта")]
        Email,

        /// <summary>
        /// Группировать Адресу за пределами субъекта
        /// </summary>
        [Display("Адрес за пределами субъекта")]
        AddressOutsideSubject,

        /// <summary>
        /// Группировать по Почтовому отделению (по факту это группировка по индексу дома)
        /// </summary>
        [Display("Почтовое отделение")]
        PostCode,

        /// <summary>
        /// Группировать по способу формирования фонда кр
        /// </summary>
        [Display("Способ формирования фонда КР")]
        CrFundFormationType,

        #region Служебные группировки

        /// <summary>
        /// Группировать по количеству ЛС в одной квитанции
        /// </summary>
        [GkhConfigProperty(Hidden = true)]
        ByAccountCount

        #endregion
    }
}