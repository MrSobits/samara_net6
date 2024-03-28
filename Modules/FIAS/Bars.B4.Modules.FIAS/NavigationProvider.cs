namespace Bars.B4.Modules.FIAS
{
    using B4;
    using Bars.B4.Navigation;

    public class NavigationProvider : BaseMainMenuProvider
    {
        public override void Init(MenuItem container)
        {
            container.Add("Администрирование").WithIcon("content/portal/menu/icons/admin.png");
            container.Add("Администрирование").Add("Справочник адресов").Add("ФИАС", "B4.controller.FiasController").AddRequiredPermission("B4.FIAS.View");
        }
    }
}