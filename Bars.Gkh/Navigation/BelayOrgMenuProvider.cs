namespace Bars.Gkh.Navigation
{
    using Bars.B4;

    public class BelayOrgMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "BelayOrg";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки страховой организации";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "B4.controller.belayorg.Edit").AddRequiredPermission("Gkh.Orgs.Belay.View");
        }
    }
}