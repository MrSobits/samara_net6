namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Вид КНМ
    /// </summary>
    public class KnmTypes : BaseEntity
    {
        /// <summary>
        /// Идентификатор в ЕРВК
        /// </summary>
        public virtual string ErvkId { get; set; }
    }
}