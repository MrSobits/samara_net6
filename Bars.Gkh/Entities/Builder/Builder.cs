namespace Bars.Gkh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Подрядчики
    /// </summary>
    public class Builder : BaseGkhEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual OrgStateRole OrgStateRole { get; set; }

        /// <summary>
        /// Применение прогрессивных технологий
        /// </summary>
        public virtual YesNoNotSet AdvancedTechnologies { get; set; }

        /// <summary>
        /// Согласие на предоставление информации
        /// </summary>
        public virtual YesNoNotSet ConsentInfo { get; set; }

        /// <summary>
        /// Выполнение работ без субподрядчика
        /// </summary>
        public virtual YesNoNotSet WorkWithoutContractor { get; set; }

        /// <summary>
        /// Файл cогласие на предоставление информации
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Рейтинг
        /// </summary>
        public virtual int? Rating { get; set; }

        /// <summary>
        /// телефон налогового органа
        /// </summary>
        public virtual string TaxInfoPhone { get; set; }

        /// <summary>
        /// адрес налогового органа
        /// </summary>
        public virtual string TaxInfoAddress { get; set; }

        // Деятельность

        /// <summary>
        /// Дата начала деятельности
        /// </summary>
        public virtual DateTime? ActivityDateStart { get; set; }

        /// <summary>
        /// Дата окончания деятельности
        /// </summary>
        public virtual DateTime? ActivityDateEnd { get; set; }

        /// <summary>
        /// Описание для деятельности
        /// </summary>
        public virtual string ActivityDescription { get; set; }

        /// <summary>
        /// Основание прекращения деятельности
        /// </summary>
        public virtual GroundsTermination ActivityGroundsTermination { get; set; }

        /// <summary>
        /// План обучения (переподготовки) кадров
        /// </summary>
        public virtual FileInfo FileLearningPlan { get; set; }

        /// <summary>
        /// Штатное расписание кадров
        /// </summary>
        public virtual FileInfo FileManningShedulle { get; set; }
    }
}
