namespace Bars.Gkh.Navigation
{
    using Bars.B4;

    public class ContragentClwMenuProvider : INavigationProvider
    {
        public string Key => "ContragentClw";

        public string Description => "Меню карточки контрагента ПИР";

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "contragentclwedit/{0}/edit");
            root.Add("Муниципальные образования", "contragentclwedit/{0}/municipality").AddRequiredPermission("Gkh.Orgs.ContragentClw.Municipality.View");
        }
    }
}