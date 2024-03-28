namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Меры в доме для акта без взаимодействия
    /// </summary>
    public class ActIsolatedRealObjMeasure : BaseEntity
    {
        /// <summary>
        /// Дом акта без взаимодействия
        /// </summary>
        public virtual ActIsolatedRealObj ActIsolatedRealObj { get; set; }

        /// <summary>
        /// Меры, принятые по пресечению нарушения обязательных требования
        /// </summary>
        public virtual string Measure { get; set; }
    }
}