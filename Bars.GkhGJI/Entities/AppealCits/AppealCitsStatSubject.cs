namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Таблица связи тематики, подтематики и характеристики
    /// </summary>
    public class AppealCitsStatSubject : BaseGkhEntity
    {
        /// <summary>
        /// Обращение граждан
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Тематика
        /// </summary>
        public virtual StatSubjectGji Subject { get; set; }

        /// <summary>
        /// Подтематика
        /// </summary>
        public virtual StatSubsubjectGji Subsubject { get; set; }

        /// <summary>
        /// Характеристика
        /// </summary>
        public virtual FeatureViolGji Feature { get; set; }

        /// <summary>
        /// Экспортируемый код
        /// </summary>
        public virtual string ExportCode { get; set; }
    }
}