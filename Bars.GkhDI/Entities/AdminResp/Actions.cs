namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Меры административной ответственности
    /// </summary>
    public class Actions : BaseGkhEntity
    {
        /// <summary>
        /// Административная ответственность
        /// </summary>
        public virtual AdminResp AdminResp { get; set; }

        /// <summary>
        /// Принятая мера по странению
        /// </summary>
        public virtual string Action { get; set; }
    }
}
