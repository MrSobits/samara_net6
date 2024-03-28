namespace Bars.Gkh.Overhaul.Entities
{
    /// <summary>
    /// Лифт со ссылкой на ООИ
    /// </summary>
    public class OvrhlRealityObjectLift : Bars.Gkh.Entities.RealityObjectLift
    {
        /// <summary>
        /// Конструктивный элемент дома
        /// </summary>
        public virtual RealityObjectStructuralElement RealityObjectStructuralElement { get; set; }
    }
}