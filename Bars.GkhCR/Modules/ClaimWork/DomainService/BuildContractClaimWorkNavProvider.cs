namespace Bars.GkhCr.Modules.ClaimWork.Navigation
{
    using Bars.B4;
    using Bars.Gkh.Modules.ClaimWork.DomainService;

    public class BuildContractCalmWorkNavProvider : IClaimWorkNavigation
    {
        public void Init(MenuItem root)
        {
            root.Add("Претензионная работа").Add("Основания претензионной работы").Add("Подрядчики, нарушившие условия договора", "buildcontractclaimwork").WithIcon("buildClw").AddRequiredPermission("Clw.ClaimWork.BuildContract.View");
        }
    }
}