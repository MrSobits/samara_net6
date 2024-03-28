namespace Bars.Gkh.Navigation
{
    using Bars.B4;

    public class LocalGovernmentMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "LocalGovernment";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки органа местного самоуправления";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "B4.controller.localgov.Edit").AddRequiredPermission("Gkh.Orgs.LocalGov.View").WithIcon("icon-shield-rainbow");
            root.Add("Время приема", "B4.controller.localgov.Reception").AddRequiredPermission("Gkh.Orgs.LocalGov.View").WithIcon("icon-time-red");
        }
    }
}
