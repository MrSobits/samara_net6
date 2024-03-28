namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Инспектор действия акта проверки
    /// </summary>
    public class ActCheckActionInspector : BaseEntity
    {
        /// <summary>
        /// Действие акта проверки
        /// </summary>
        public virtual ActCheckAction ActCheckAction { get; set; }

        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual Inspector Inspector { get; set; }
    }
}