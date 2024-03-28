namespace Bars.Gkh.RegOperator.Navigation
{
    using Bars.B4;
    using Bars.Gkh.Modules.ClaimWork.DomainService;

    public class RegOpClaimWorkNavigation : IClaimWorkNavigation
    {
        public void Init(MenuItem root)
        {
            root.Add("Претензионная работа")
                .Add("Основания претензионной работы")
                .Add("Реестр неплательщиков юридических лиц", "legalclaimwork")
                .WithIcon("legalClaimWork")
                .AddRequiredPermission("Clw.ClaimWork.Legal.View");

            root.Add("Претензионная работа")
                .Add("Основания претензионной работы")
                .Add("Реестр неплательщиков физических лиц", "individualclaimwork")
                .WithIcon("individualClaimWork")
                .AddRequiredPermission("Clw.ClaimWork.Individual.View");
        }
    }
}