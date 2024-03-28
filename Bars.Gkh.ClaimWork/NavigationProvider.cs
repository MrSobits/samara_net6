namespace Bars.Gkh.ClaimWork
{
    using B4;
    using B4.Application;
    using B4.Utils;
    using Modules.ClaimWork.DomainService;

    public class NavigationProvider : INavigationProvider
    {
        public void Init(MenuItem root)
        {
            var container = ApplicationContext.Current.Container;
            
            container.ResolveAll<IClaimWorkNavigation>().ForEach(x => x.Init(root));

            root.Add("Справочники").Add("Претензионная и исковая работа").Add("Виды нарушений договора подряда", "violclaimwork")
                .AddRequiredPermission("Clw.Dictionaries.ViolClaimWork");
            root.Add("Справочники").Add("Претензионная и исковая работа").Add("Учреждения в судебной практике", "jurinstitution")
                .AddRequiredPermission("Clw.Dictionaries.JurInstitution.View");
            root.Add("Справочники").Add("Претензионная и исковая работа").Add("Заявления в суд", "petitiontocourt")
                .AddRequiredPermission("Clw.Dictionaries.PetitionToCourt.View");
            root.Add("Справочники").Add("Претензионная и исковая работа").Add("Госпошлина", "stateduty")
                .AddRequiredPermission("Clw.Dictionaries.StateDuty.View");

            root.Add("Претензионная работа").Add("Документы").Add("Журнал судебной практики", "jurjournal")
                .AddRequiredPermission("Clw.JurJournal.View");
            root.Add("Претензионная работа").Add("Документы").Add("Реестр документов ПИР", "documentregister")
                .AddRequiredPermission("Clw.DocumentRegister.View").WithIcon("documentClw");
        }

        public string Key
        {
            get
            {
                return MainNavigationInfo.MenuName;
            }
        }

        public string Description
        {
            get
            {
                return MainNavigationInfo.MenuDescription;
            }
        }
    }
}