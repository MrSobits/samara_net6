namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Подписка инспекторов
    /// </summary>
    public class InspectorSubscription : BaseImportableEntity
    {
        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Инсектор, который подписан на {Inspector}
        /// </summary>
        public virtual Inspector SignedInspector { get; set; }
    }
}
