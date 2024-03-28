namespace Bars.GkhGji.ConfigSections
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Общие настройки ГЖИ
    /// </summary>
    public class GeneralConfig : IGkhConfigSection
    {
        /// <summary>
        /// Заполнение поля Исполнитель при добавлении Ответа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Заполнение поля Исполнитель при добавлении Ответа")]
        [Permissionable]
        public virtual bool ExecutorIsCurrentOperator { get; set; }

        /// <summary>
        /// Отображать обработанные обращения как "просроченные" 
        /// </summary>
        [GkhConfigProperty(DisplayName = "Отображать обработанные обращения как \"просроченные\"")]
        [Permissionable]
        public virtual bool ShowStatementsWithAnswerAsOverdue { get; set; }

        /// <summary>
        /// Ограничение срока устранения нарушений
        /// </summary>
        [GkhConfigProperty(DisplayName = "Ограничение срока устранения нарушений")]
        [Permissionable]
        public virtual TimeForCorrectingViol TimeForCorrectingViol { get; set; }
    }
}