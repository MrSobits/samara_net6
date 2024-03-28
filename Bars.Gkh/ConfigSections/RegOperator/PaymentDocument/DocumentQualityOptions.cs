namespace Bars.Gkh.ConfigSections.RegOperator
{
    using System.ComponentModel;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.DataAnnotations;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Опции качества документа
    /// </summary>
    public class DocumentQualityOptions : IGkhConfigSection
    {
        /// <summary>
        /// Разрешение изображения 
        /// </summary>
        [GkhConfigProperty(DisplayName = "Разрешение изображения (dpi) (меньше значение - меньше размер)")]
        [GkhConfigPropertyEditor("B4.ux.config.PredefinedValuesComboEditor", "predefinedvaluescombobox")]
        [PossibleValues(10, 25, 50, 75, 100, 200, 300, 400, 500)]
        [DefaultValue(10)]
        public virtual int Dpi { get; set; }

        /// <summary>
        /// Качество изображения 
        /// </summary>
        [GkhConfigProperty(DisplayName = "Качество изображения (меньше значение - меньше размер)")]
        [GkhConfigPropertyEditor("B4.ux.config.PredefinedValuesComboEditor", "predefinedvaluescombobox")]
        [PossibleValues(0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1)]
        [DefaultValue(0.1)]
        public virtual float ImageQuality { get; set; }

        /// <summary>
        /// Стандартные PDF шрифты
        /// </summary>
        [GkhConfigProperty(DisplayName = "Использовать стандартные PDF шрифты (нет - увеличит размер)")]
        [DefaultValue(YesNo.Yes)]
        public virtual YesNo UseStandardPdfFonts { get; set; }

        /// <summary>
        /// Внедрять шрифты в пдф
        /// </summary>
        [GkhConfigProperty(DisplayName = "Поставлять pdf-файл вместе со шрифтами (да - увеличит размер)")]
        [DefaultValue(YesNo.No)]
        public virtual YesNo EmbeddedFonts { get; set; }

        /// <summary>
        /// Сжатие
        /// </summary>
        [GkhConfigProperty(DisplayName = "Сжатие (да - уменьшит размер, позволит редактировать)")]
        [DefaultValue(YesNo.Yes)]
        public virtual YesNo Compressed { get; set; }
    }
}