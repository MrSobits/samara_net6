namespace Bars.Gkh.Navigation
{
    using Bars.B4;

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
            root.Add("Категории МКД", "realityobjectedit/{0}/categorymkd")/*.AddRequiredPermission("Gkh.RealityObject.Register.CategoryCSMKD.View")*/.WithIcon("icon-chart-bar");
            root.Add("Конструктивные характеристики", "realityobjectedit/{0}/structelement").AddRequiredPermission("Gkh.RealityObject.Register.StructElem.View").WithIcon("icon-table-gear");
            root.Add("Лифты", "realityobjectedit/{0}/lift");//.AddRequiredPermission("Gkh.RealityObject.Register.Lift.View");
            root.Add("Сведения о СКПТ", "realityobjectedit/{0}/antenna");//.AddRequiredPermission("Gkh.RealityObject.Register.SKPT.View");
            root.Add("Камеры видеонаблюдения", "realityobjectedit/{0}/videcam");//.AddRequiredPermission("Gkh.RealityObject.Register.SKPT.View");
            root.Add("Старший по дому", "realityobjectedit/{0}/housekeeper");//.AddRequiredPermission("Gkh.RealityObject.Register.SKPT.View");
            //root.Add("Приборы учета и узлы регулирования", "realityobjectedit/{0}/meteringdevice").AddRequiredPermission("Gkh.RealityObject.Register.MeteringDevice.View").WithIcon("icon-compass");
            root.Add("Фото-архив", "realityobjectedit/{0}/image").AddRequiredPermission("Gkh.RealityObject.Register.Image.View").WithIcon("icon-camera");
            root.Add("Сведения о помещениях", "realityobjectedit/{0}/room").AddRequiredPermission("Gkh.RealityObject.Register.HouseInfo.View").WithIcon("icon-neighbourhood");
            root.Add("Сведения о подъездах", "realityobjectedit/{0}/entrance").AddRequiredPermission("Gkh.RealityObject.Register.Entrance.View");
            root.Add("Сведения о домофонах", null).AddRequiredPermission("Gkh.RealityObject.Register.Intercom.View");
            root.Add("Технический мониторинг", "realityobjectedit/{0}/technicalmonitoring").AddRequiredPermission("Gkh.RealityObject.Register.TechnicalMonitoring.View");
            root.Add("Сведения о блоках", "realityobjectedit/{0}/block").AddRequiredPermission("Gkh.RealityObject.Register.Block.View");

            root.Add("Земельные участки", "realityobjectedit/{0}/land").AddRequiredPermission("Gkh.RealityObject.Register.Land.View").WithIcon("icon-picture");
            //root.Add("Страхование объекта", "realityobjectedit/{0}/belay").AddRequiredPermission("Gkh.RealityObject.Register.Belay.View");
            root.Add("Управление домом", "realityobjectedit/{0}/managorg").AddRequiredPermission("Gkh.RealityObject.Register.ManagOrg.View").WithIcon("icon-lock-key");
            root.Add("Расчетно-кассовые центры").AddRequiredPermission("Gkh.RealityObject.Register.CashPaymentCenter.View");
            root.Add("Поставщики жилищных услуг", "realityobjectedit/{0}/serviceorg").AddRequiredPermission("Gkh.RealityObject.Register.ServiceOrg.View").WithIcon("icon-lightbulb");
            root.Add("Поставщики коммунальных услуг", "realityobjectedit/{0}/resorg").AddRequiredPermission("Gkh.RealityObject.Register.ResOrg.View");
            root.Add("Совет МКД", "realityobjectedit/{0}/councilapartmenthouse").AddRequiredPermission("Gkh.RealityObject.Register.Councillors.View").WithIcon("icon-group");
            root.Add("Конструктивные элементы", "realityobjectedit/{0}/constructiveelement").AddRequiredPermission("Gkh.RealityObject.Register.ConstructiveElement.View").WithIcon("icon-shape_3d");
            root.Add("Текущий ремонт", "realityobjectedit/{0}/curentrepair").AddRequiredPermission("Gkh.RealityObject.Register.CurentRepair.View").WithIcon("icon-wrench");
            //root.Add("Проверки приборов учёта", "realityobjectedit/{0}/meteringdeviceschecks");
            root.Add("История изменений значений", "realityobjectedit/{0}/changevalueshistory");
        }
    }
}