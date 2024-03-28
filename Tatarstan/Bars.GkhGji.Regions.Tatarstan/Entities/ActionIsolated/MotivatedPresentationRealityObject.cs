namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Дом мотивированного представления КНМ без взаимодействия с контролируемыми лицами
    /// </summary>
    public class MotivatedPresentationRealityObject : BaseGkhEntity
    {
        /// <summary>
        /// Мотивированное представление
        /// </summary>
        public virtual MotivatedPresentation MotivatedPresentation { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}