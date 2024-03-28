namespace Bars.Gkh.Gis.NavigationMenu
{
    using B4;

    public class RealityObjMenuKey
    {
        public static string Key
        {
            get { return "RealityObj"; }
        }

        public static string Description
        {
            get
            {
                return "Меню карточки жилого дома";
            }
        }
    }

    public class RealityObjMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return RealityObjMenuKey.Key;
            }
        }

        public string Description
        {
            get
            {
                return RealityObjMenuKey.Description;
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "realityobjectedit/{0}/edit")/*.AddRequiredPermission("Gkh.RealityObject.View")*/.WithIcon("icon-outline");

			root.Add("Конструктивные характеристики", "realityobjectedit/{0}/structelement").WithIcon("icon-table-gear");

            //root.Add("Приборы учета и узлы регулирования", "realityobjectedit/{0}/meteringdevice").AddRequiredPermission("Gkh.RealityObject.Register.MeteringDevice.View").WithIcon("icon-compass");
            root.Add("Фото-архив", "realityobjectedit/{0}/image").AddRequiredPermission("Gkh.RealityObject.Register.Image.View").WithIcon("icon-camera");
            root.Add("Сведения о квартирах", "realityobjectedit/{0}/apartinfo").AddRequiredPermission("Gkh.RealityObject.Register.ApartInfo.View").WithIcon("icon-key");
            root.Add("Сведения о помещениях", "realityobjectedit/{0}/room").AddRequiredPermission("Gkh.RealityObject.Register.HouseInfo.View").WithIcon("icon-neighbourhood");

            var jku = root.Add("Сведения по ЖКУ").AddRequiredPermission("Gkh.RealityObject.Register.HousingComminalService.View");
            jku.Items.Clear();

            // подключение форм из другого проекта
            jku.Add("Параметры дома", "realityobjectedit/{0}/params").WithIcon("icon-building").AddRequiredPermission("Gkh.RealityObject.Register.HousingComminalService.Params.View");
            jku.Add("Услуги по дому", "realityobjectedit/{0}/service").WithIcon("icon-house-key").AddRequiredPermission("Gkh.RealityObject.Register.HousingComminalService.Service.View");
            jku.Add("Начисления по домам", "realityobjectedit/{0}/accruals").WithIcon("icon-coins").AddRequiredPermission("Gkh.RealityObject.Register.HousingComminalService.Accruals.View");
            jku.Add("Показания ОДПУ", "realityobjectedit/{0}/counters").WithIcon("icon-table-edit").AddRequiredPermission("Gkh.RealityObject.Register.HousingComminalService.Counters.View");
            jku.Add("Лицевые счета дома", "realityobjectedit/{0}/account").WithIcon("icon-page-copy").AddRequiredPermission("Gkh.RealityObject.Register.HousingComminalService.Account.View");
            jku.Add("Претензии граждан", "realityobjectedit/{0}/claims").WithIcon("icon-note-error");//.AddRequiredPermission("Gkh.RealityObject.Register.HousingComminalService.Params");

            root.Add("Земельные участки", "realityobjectedit/{0}/land").AddRequiredPermission("Gkh.RealityObject.Register.Land.View").WithIcon("icon-picture");
            //root.Add("Страхование объекта", "realityobjectedit/{0}/belay").AddRequiredPermission("Gkh.RealityObject.Register.Belay.View");
            root.Add("Управление домом", "realityobjectedit/{0}/managorg").AddRequiredPermission("Gkh.RealityObject.Register.ManagOrg.View").WithIcon("icon-lock-key");
            root.Add("Агенты доставки", "realityobjectedit/{0}/deliveryagent").AddRequiredPermission("Gkh.RealityObject.Register.DeliveryAgent.View");
            root.Add("Расчетно-кассовые центры").AddRequiredPermission("Gkh.RealityObject.Register.CashPaymentCenter.View");
            root.Add("Поставщики жилищных услуг", "realityobjectedit/{0}/serviceorg").AddRequiredPermission("Gkh.RealityObject.Register.ServiceOrg.View").WithIcon("icon-lightbulb");
            root.Add("Поставщики коммунальных услуг", "realityobjectedit/{0}/resorg").AddRequiredPermission("Gkh.RealityObject.Register.ResOrg.View");
            root.Add("Совет МКД", "realityobjectedit/{0}/councilapartmenthouse").AddRequiredPermission("Gkh.RealityObject.Register.Councillors.View").WithIcon("icon-group");
            root.Add("Конструктивные элементы", "realityobjectedit/{0}/constructiveelement").AddRequiredPermission("Gkh.RealityObject.Register.ConstructiveElement.View").WithIcon("icon-shape_3d");
            root.Add("Текущий ремонт", "realityobjectedit/{0}/curentrepair").AddRequiredPermission("Gkh.RealityObject.Register.CurentRepair.View").WithIcon("icon-wrench");
        }
    }
}