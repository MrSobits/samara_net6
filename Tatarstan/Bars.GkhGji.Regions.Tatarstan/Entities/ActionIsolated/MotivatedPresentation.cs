namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated
{
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Мотивированное представление
    /// </summary>
    public class MotivatedPresentation : DocumentGji
    {
        /// <summary>
        /// Место составления
        /// </summary>
        public virtual FiasAddress CreationPlace { get; set; }

        /// <summary>
        /// Должностное лицо (ДЛ), вынесшее мотивированное представление
        /// </summary>
        public virtual Inspector IssuedMotivatedPresentation { get; set; }

        /// <summary>
        /// Ответственный за исполнение
        /// </summary>
        public virtual Inspector ResponsibleExecution { get; set; }
    }
}