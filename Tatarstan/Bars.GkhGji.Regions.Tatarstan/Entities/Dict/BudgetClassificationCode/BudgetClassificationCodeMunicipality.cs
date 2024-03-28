namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict.BudgetClassificationCode
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    public class BudgetClassificationCodeMunicipality : BaseEntity
    {
        /// <summary>
        /// КБК
        /// </summary>
        public virtual BudgetClassificationCode BudgetClassificationCode { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}
