namespace Bars.GkhGji.Entities
{
    using Gkh.Entities;

    /// <summary>
    /// Спарвочник "социальный статус"
    /// </summary>
    public class SocialStatus : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }
    }
}
