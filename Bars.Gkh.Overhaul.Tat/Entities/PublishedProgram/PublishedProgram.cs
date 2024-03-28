namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;

    /// <summary>
    /// Опубликованная программа
    /// </summary>
    public class PublishedProgram : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Ссылка на версию
        /// </summary>
        public virtual ProgramVersion ProgramVersion { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
    }
}