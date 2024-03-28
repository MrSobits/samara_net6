namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict.BudgetClassificationCode
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Связка КБК и статей закона
    /// </summary>
    public class BudgetClassificationCodeArticleLaw : BaseEntity
    {
        /// <summary>
        /// КБК
        /// </summary>
        public virtual BudgetClassificationCode BudgetClassificationCode { get; set; }

        /// <summary>
        /// Cтатья закона
        /// </summary>
        public virtual ArticleLawGji ArticleLaw { get; set; }
    }
}