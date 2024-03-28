namespace Bars.Gkh.Regions.Tatarstan.DomainService
{
    using Bars.B4;
    using Bars.Gkh.Modules.ClaimWork.DomainService;

    /// <summary>
    /// Навигация по должникам ЖКУ
    /// </summary>
    public class UtilityDebtClaimWorkNavProvider : IClaimWorkNavigation
    {
        /// <summary>
        /// Инициализировать
        /// </summary>
        /// <param name="root"></param>
        public void Init(MenuItem root)
        {
            root.Add("Претензионная работа").Add("Основания претензионной работы").Add("Реестр неплательщиков ЖКУ", "utilitydebtorclaimwork").WithIcon("utilityDebtor").AddRequiredPermission("Clw.ClaimWork.UtilityDebtor.View");
        }
    }
}
