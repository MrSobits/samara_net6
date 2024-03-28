namespace Bars.Gkh.Gku
{
    using Bars.B4;

    public class GkuInfoMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "GkuInfo";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки сведений ЖКУ";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "B4.controller.gkuinfo.Edit").WithIcon("icon-shield-rainbow");
            root.Add("Начисления  по дому", "B4.controller.gkuinfo.housingcommunalservice.InfoOverview");
            root.Add("Лицевые счета дома", "B4.controller.gkuinfo.housingcommunalservice.Account");
            root.Add("Показания общедомовых приборов учета", "B4.controller.gkuinfo.housingcommunalservice.MeteringDeviceValue").AddRequiredPermission("Gkh.GkuInfo.MeteringDeviceValue.View");
            root.Add("Претензии граждан", "B4.controller.gis.realobj.CitizenClaim").AddRequiredPermission("Gkh.GkuInfo.CitizenClaim.View");
        }
    }
}
