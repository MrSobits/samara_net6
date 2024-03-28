namespace Bars.GkhDi.Entities
{
    using Enums;

    public class CapRepairService : BaseService
    {
        /// <summary>
        /// Тип оказания услуги
        /// </summary>
        public virtual TypeOfProvisionServiceDi TypeOfProvisionService { get; set; }

        /// <summary>
        /// Региональный фонд
        /// </summary>
        public virtual RegionalFundDi RegionalFund { get; set; }
    }
}
