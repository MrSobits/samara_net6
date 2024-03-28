namespace Bars.GisIntegration.Smev.ConfigSections
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.DataAnnotations;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Настройка параметров интеграции с ЕРКНМ
    /// </summary>
    [GkhConfigSection("ErknmIntegrationConfig", DisplayName = "Настройка параметров интеграции с ЕРКНМ")]
    [Navigation]
    public class ErknmIntegrationConfig : IGkhConfigSection
    {
        /// <summary>
        /// Интеграция включена
        /// </summary>
        [GkhConfigProperty(DisplayName = "Интеграция включена")]
        [DefaultValue(false)]
        public virtual bool IsEnabled { get; set; }

        /// <summary>
        /// Адрес прокси-сервера
        /// </summary>
        [GkhConfigProperty(DisplayName = "Адрес прокси-сервера")]
        [StringLength(255, ErrorMessage = "Адрес прокси-сервера: значение не должно превышать {1} символов")]
        public virtual string Endpoint { get; set; }

        /// <summary>
        /// Интервал запроса ответа (мин.)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Интервал запроса ответа (мин.)")]
        [GkhConfigPropertyEditor("B4.ux.config.PredefinedValuesComboEditor", "predefinedvaluescombobox")]
        [PossibleValues(5, 10, 15, 25, 40, 50, 60)]
        [DefaultValue(5)]
        public virtual int RequestInterval { get; set; }

        /// <summary>
        /// Наименование органа контроля (надзора)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Наименование органа контроля (надзора)")]
        [StringLength(255, ErrorMessage = "Наименование органа контроля (надзора): значение не должно превышать {1} символов")]
        public virtual string ControlContractorName { get; set; }

        /// <summary>
        /// Идентификатор органа контроля в ЕРВК (формата GUID)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Идентификатор органа контроля в ЕРВК (формата GUID)")]
        [StringLength(36, ErrorMessage = "Идентификатор органа контроля в ЕРВК (формата GUID): значение не должно превышать {1} символов")]
        public virtual string SupervisoryId { get; set; }

        /// <summary>
        /// Идентификатор справочника КНО в ЕРВК (формата GUID)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Идентификатор справочника КНО в ЕРВК (формата GUID)")]
        [StringLength(36, ErrorMessage = "Идентификатор справочника КНО в ЕРВК (формата GUID): значение не должно превышать {1} символов")]
        public virtual string ControlOrganizationId { get; set; }

        /// <summary>
        /// Идентификатор справочника видов КНМ в ЕРВК (формата GUID)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Идентификатор справочника видов КНМ в ЕРВК (формата GUID)")]
        [StringLength(36, ErrorMessage = "Идентификатор справочника видов КНМ в ЕРВК (формата GUID): значение не должно превышать {1} символов")]
        public virtual string KnmTypeId { get; set; }

        /// <summary>
        /// Идентификатор справочника ВК в ЕРВК (формата GUID)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Идентификатор справочника ВК в ЕРВК (формата GUID)")]
        [StringLength(36, ErrorMessage = "Идентификатор справочника ВК в ЕРВК (формата GUID): значение не должно превышать {1} символов")]
        public virtual string ControlTypeId { get; set; }

        /// <summary>
        /// Идентификатор справочника должностей участников в ЕРВК (формата GUID)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Идентификатор справочника должностей участников в ЕРВК (формата GUID)")]
        [StringLength(36, ErrorMessage = "Идентификатор справочника должностей участников в ЕРВК (формата GUID): значение не должно превышать {1} символов")]
        public virtual string InspectorPositionId { get; set; }

        /// <summary>
        /// Идентификатор справочника действий в ЕРВК (формата GUID)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Идентификатор справочника действий в ЕРВК (формата GUID)")]
        [StringLength(36, ErrorMessage = "Идентификатор справочника действий в ЕРВК (формата GUID): значение не должно превышать {1} символов")]
        public virtual string KnmActionId { get; set; }

        /// <summary>
        /// Идентификатор справочника индикаторов риска в ЕРВК (формата GUID)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Идентификатор справочника индикаторов риска в ЕРВК (формата GUID)")]
        [StringLength(36, ErrorMessage = "Идентификатор справочника индикаторов риска в ЕРВК (формата GUID): значение не должно превышать {1} символов")]
        public virtual string RiskIndicatorId { get; set; }

        /// <summary>
        /// Идентификатор справочника типов объекта в ЕРВК (формата GUID)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Идентификатор справочника типов объекта в ЕРВК (формата GUID)")]
        [StringLength(36, ErrorMessage = "Идентификатор справочника типов объекта в ЕРВК (формата GUID): значение не должно превышать {1} символов")]
        public virtual string ControlObjectTypeId { get; set; }

        /// <summary>
        /// Идентификатор справочника видов объекта в ЕРВК (формата GUID)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Идентификатор справочника видов объекта в ЕРВК (формата GUID)")]
        [StringLength(36, ErrorMessage = "Идентификатор справочника видов объекта в ЕРВК (формата GUID): значение не должно превышать {1} символов")]
        public virtual string ControlObjectKindId { get; set; }

        /// <summary>
        /// Идентификатор справочника категорий риска в ЕРВК (формата GUID)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Идентификатор справочника категорий риска в ЕРВК (формата GUID)")]
        [StringLength(36, ErrorMessage = "Идентификатор справочника категорий риска в ЕРВК (формата GUID): значение не должно превышать {1} символов")]
        public virtual string RiskCategoryId { get; set; }

        /// <summary>
        /// Идентификатор справочника должностей подписантов в ЕРВК (формата GUID)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Идентификатор справочника должностей подписантов в ЕРВК (формата GUID)")]
        [StringLength(36, ErrorMessage = "Идентификатор справочника должностей подписантов в ЕРВК (формата GUID): значение не должно превышать {1} символов")]
        public virtual string SignerPostId { get; set; }
    }
}