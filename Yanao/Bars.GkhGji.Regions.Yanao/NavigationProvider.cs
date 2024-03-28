namespace Bars.GkhGji.Regions.Yanao
{
	using Bars.B4;
	using Bars.Gkh.TextValues;

	public class NavigationProvider : INavigationProvider
    {
        public IMenuItemText MenuItemText { get; set; }

		public string Key
		{
			get { return MainNavigationInfo.MenuName; }
		}

		public string Description
		{
			get { return MainNavigationInfo.MenuDescription; }
		}

		public void Init(MenuItem root)
        {
            var licensingRoot = root.Add("Жилищная инспекция").Add("Лицензирование");
			licensingRoot.Caption = "Лицензирование управляющих организаций";
        }
    }
}