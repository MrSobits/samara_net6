namespace Bars.GkhGji.Regions.Tomsk.Entities.AppealCits
{
	using System;
	using Bars.B4.DataAccess;
	using Bars.B4.Modules.FileStorage;
	using Bars.B4.Modules.States;
	using Bars.Gkh.Entities;

	/// <summary>
    ///     Исполнитель обращения
    /// </summary>
    public class AppealCitsExecutant : BaseEntity, IStatefulEntity
    {
        /// <summary>
        ///     Обращение
        /// </summary>
        public virtual GkhGji.Entities.AppealCits AppealCits { get; set; }

        /// <summary>
        ///     Поручитель
        /// </summary>
        public virtual Inspector Author { get; set; }

        /// <summary>
        ///     Исполнитель
        /// </summary>
        public virtual Inspector Executant { get; set; }

        /// <summary>
        ///     Проверяющий инспектор
        /// </summary>
        public virtual Inspector Controller { get; set; }

        /// <summary>
        ///     Дата поручения
        /// </summary>
        public virtual DateTime OrderDate { get; set; }

        /// <summary>
        ///     Срок исполнения
        /// </summary>
        public virtual DateTime? PerformanceDate { get; set; }

        /// <summary>
        ///     Ответственный
        /// </summary>
        public virtual bool IsResponsible { get; set; }

        /// <summary>
        ///     Комментарий
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        ///     Статус
        /// </summary>
        public virtual State State { get; set; }

        public virtual FileInfo Resolution { get; set; }

        public AppealCitsExecutant() { }

        public AppealCitsExecutant(long appealCitizensId, long? inspectorId, DateTime? performanceDate, long authorId)
        {
            if (inspectorId.HasValue)
            {
                Executant = new Inspector { Id = inspectorId.Value };
            }

            AppealCits = new GkhGji.Entities.AppealCits { Id = appealCitizensId };
            OrderDate = DateTime.Now;
            PerformanceDate = performanceDate;
            IsResponsible = false;
            Author = authorId > 0 ? new Inspector { Id = authorId } : null;
        }
    }
}