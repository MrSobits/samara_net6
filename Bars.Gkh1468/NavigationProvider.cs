namespace Bars.Gkh1468
{
    using Bars.B4;

    /// <summary>
    /// Меню, навигация
    /// </summary>
    public class NavigationProvider : INavigationProvider
    {
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

        public void Init(MenuItem root)
        {
            /*var menu1468 = root.Add("Жилищный фонд").Add("Паспорта по ПП РФ 1468");
            menu1468.Add("Реестр паспортов домов", "housepassports/").WithIcon("housepassports").AddRequiredPermission("Gkh1468.Passport.House.View").WithIcon("house-passport");
            menu1468.Add("Реестр паспортов ОКИ", "okipassport/").WithIcon("okipassport").AddRequiredPermission("Gkh1468.Passport.Oki.View").WithIcon("oki-paspport");
            menu1468.Add("Мои паспорта домов", "myhousepassports/").WithIcon("myhousepassports").AddRequiredPermission("Gkh1468.Passport.MyHouse.View").WithIcon("house-passport-prov");
            menu1468.Add("Мои паспорта ОКИ", "myokipassports/").WithIcon("myokipassports").AddRequiredPermission("Gkh1468.Passport.MyOki.View").WithIcon("oki-passport-prov");
            menu1468.Add("Импорт паспортов 1468", "importfrom1468rf/").WithIcon("importfrom1468rf").AddRequiredPermission("Gkh1468.Passport.ImportFrom1468.View");           
            
            root.Add("Справочники").Add("Паспорта").Add("Справочник структур паспортов", "passpstructs/").AddRequiredPermission("Gkh1468.Dictionaries.PassportStruct.View");
            */
            root.Add("Участники процесса").Add("Роли контрагента").Add("Поставщики ресурсов", "publicservorg").AddRequiredPermission("Gkh1468.Orgs.PublicServiceOrg.View").WithIcon("public-serv-org");
        }
    }
}