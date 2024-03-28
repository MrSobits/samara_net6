namespace Bars.Gkh.Gis.NavigationMenu
{
    using B4;

    public class PersonalAccountInfoMenu : INavigationProvider
    {
        public void Init(MenuItem root)
        {
            root.Add("Параметры лицевого счета", "personalaccountinfo/{0}/{1}/params").WithIcon("icon-building");
            root.Add("Услуги по лицевому счету", "personalaccountinfo/{0}/{1}/services").WithIcon("icon-house-key");
            root.Add("Начисления по лицевому счету", "personalaccountinfo/{0}/{1}/accruals").WithIcon("icon-coins");
            root.Add("Показания ИПУ", "personalaccountinfo/{0}/{1}/counters").WithIcon("icon-table-edit");
            //root.Add("Сальдо по лицевому счету", "personalaccountinfo/{0}/{1}/saldo").WithIcon("icon-coins");
            root.Add("Проверочный расчет", "personalaccountinfo/{0}/{1}/delta").WithIcon("icon-coins");
            root.Add("Претензии граждан (\"НК\")", "personalaccountinfo/{0}/{1}/publiccontrol")
                .AddRequiredPermission("Gkh.RealityObject.Register.HousingComminalService.Account.PublicControl.View")
                .WithIcon("icon-note-error");
            //root.Add("Начисленные субсидии по ЛС", "personalaccountinfo/{0}/{1}/tenantsubsidy").WithIcon("icon-coins");
            //root.Add("Начисленные субсидии по услугам", "personalaccountinfo/{0}/{1}/servicesubsidy").WithIcon("icon-coins");
        }
        public string Key
        {
            get
            {
                return "PersonalAccountInfo";
            }
        }

        public string Description
        {
            get
            {
                return "Меню информация по лицевому счету";
            }
        }
    }
}
