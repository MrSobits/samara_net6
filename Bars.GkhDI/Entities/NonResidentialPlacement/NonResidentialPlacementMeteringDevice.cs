namespace Bars.GkhDi.Entities
{
    using Gkh.Entities;

    /// <summary>
    /// Приборы учета сведения об использование нежилых помещений
    /// </summary>
    public class NonResidentialPlacementMeteringDevice : BaseGkhEntity
    {
        /// <summary>
        /// Сведения об использование нежилых помещений
        /// </summary>
        public virtual NonResidentialPlacement NonResidentialPlacement { get; set; }

        /// <summary>
        /// Прибор учета
        /// </summary>
        public virtual MeteringDevice MeteringDevice { get; set; }
    }
}
