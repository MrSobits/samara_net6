namespace Bars.Gkh.Overhaul.Navigation
{
    using B4;

    public class ContragentMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return Gkh.Navigation.ContragentMenuProvider.Key;
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки контрагента";
            }
        }

        public void Init(MenuItem root)
        {
            root.Get("contragentedit/{0}/bank").Caption = "Расчетные счета";
        }
    }
}
