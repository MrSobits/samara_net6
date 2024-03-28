namespace Bars.Gkh.ConfigSections.RegOperator
{
    using System.ComponentModel;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.Enums;

    /// <summary>
    /// настройки сорти
    /// </summary>
    public class SortOptions : IGkhConfigSection
    {
        /// <summary>
        /// Расположение 2 документов на 1 листе
        /// </summary>
        [GkhConfigProperty(DisplayName = "Расположение 2 документов на 1 листе")]
        [DefaultValue(TwoDocumentsPerSheet.Ordered)]
        public virtual TwoDocumentsPerSheet DocumentsPerSheet { get; set; }

        /// <summary>
        /// Использовать индекс при сортировке
        /// </summary>
        [GkhConfigProperty(DisplayName = "Использовать индекс при сортировке")]
        [DefaultValue(YesNo.Yes)]
        public virtual YesNo UseIndexForSorting { get; set; }
    }
}