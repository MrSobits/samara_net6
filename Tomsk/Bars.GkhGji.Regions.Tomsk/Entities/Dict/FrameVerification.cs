namespace Bars.GkhGji.Regions.Tomsk.Entities.Dict
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Рамки проверки
    /// </summary>
    public class FrameVerification : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}
