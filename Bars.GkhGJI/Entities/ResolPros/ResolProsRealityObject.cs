namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Дома в постановлении прокуратуры 
    /// </summary>
    public class ResolProsRealityObject : BaseGkhEntity
    {
        /// <summary>
        /// Постановление прокуратуры
        /// </summary>
        public virtual ResolPros ResolPros { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}