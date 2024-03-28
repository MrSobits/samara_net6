namespace Bars.Gkh.Regions.Tomsk.Entities
{
    /// <summary>
    /// Данная сущность расширяет базовую сущность дополнительными полями 
    /// </summary>
    public class TomskOperator: Gkh.Entities.Operator
    {
        /// <summary>
        /// Показывать не назначенные обращения
        /// </summary>
        public virtual bool ShowUnassigned { get; set; }

    }
}
