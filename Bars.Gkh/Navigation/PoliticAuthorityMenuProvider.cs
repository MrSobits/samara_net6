namespace Bars.Gkh.Navigation
{
    using Bars.B4;

    public class PoliticAuthorityMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "PoliticAuthority";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки органа государственной власти";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "B4.controller.politicauth.Edit").AddRequiredPermission("Gkh.Orgs.PoliticAuth.View").WithIcon("icon-shield-rainbow");
            root.Add("Время приема", "B4.controller.politicauth.Reception").AddRequiredPermission("Gkh.Orgs.PoliticAuth.View").WithIcon("icon-time-red");
        }
    }
}
