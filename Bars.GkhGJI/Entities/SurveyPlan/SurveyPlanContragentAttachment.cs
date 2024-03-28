namespace Bars.GkhGji.Entities.SurveyPlan
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    ///     Приложение контрагента плана проверки
    /// </summary>
    public class SurveyPlanContragentAttachment : BaseEntity
    {
        /// <summary>
        ///     Дата документа
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        ///     Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        ///     Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        ///     Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///     Номер документа
        /// </summary>
        public virtual string Num { get; set; }

        /// <summary>
        ///     Контрагент плана проверки
        /// </summary>
        public virtual SurveyPlanContragent SurveyPlanContragent { get; set; }
    }
}