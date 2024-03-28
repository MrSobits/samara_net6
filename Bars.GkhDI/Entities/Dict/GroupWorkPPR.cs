namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Группы работ по ППР
    /// </summary>
    public class GroupWorkPpr : BaseGkhEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Шаблонная услуга (с типом ремонт)
        /// </summary>
        public virtual TemplateService Service { get; set; }

        /// <summary>
        /// Работа не актуальна
        /// </summary>
        public virtual bool IsNotActual { get; set; }
    }
}
