namespace Bars.Gkh.Entities.Dicts
{
    using Bars.Gkh.Enums;

    /// <summary>
    /// Программа переселения
    /// </summary>
    public class ResettlementProgram : BaseGkhEntity
    {
        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Соответствует ФЗ
        /// </summary>
        public virtual bool MatchFederalLaw { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual Period Period { get; set; }

        /// <summary>
        /// Состояние программы
        /// </summary>
        public virtual StateResettlementProgram StateProgram { get; set; }

        /// <summary>
        /// Тип программы
        /// </summary>
        public virtual TypeResettlementProgram TypeProgram { get; set; }

        /// <summary>
        /// Используется при экспорте
        /// </summary>
        public virtual bool UseInExport { get; set; }

        /// <summary>
        /// Видимость программы
        /// </summary>
        public virtual VisibilityResettlementProgram Visibility { get; set; }
    }
}