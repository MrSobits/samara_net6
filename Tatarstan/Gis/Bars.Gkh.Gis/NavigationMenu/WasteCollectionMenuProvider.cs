namespace Bars.Gkh.Gis.NavigationMenu
{
    using B4;

    public class WasteCollectionMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "WasteCollection";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки площадки сбора ТБО и ЖБО";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Сведения о площадке сбора БО", "wastecollectionplaceedit/{0}/edit").AddRequiredPermission("Gkh.WasteCollectionPlaces.Edit");
        }
    }
}