namespace Sobits.RosReg
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.Gkh.TextValues;

    /// <inheritdoc />
    /// <summary>
    /// Меню, навигация
    /// </summary>
    public class NavigationProvider : INavigationProvider
    {
        public IMenuItemText MenuItemText { get; set; }
#pragma warning disable 618
        public string Key => MainNavigationInfo.MenuName;

        public string Description => MainNavigationInfo.MenuDescription;

        public void Init(MenuItem root)
        {
            root.Add("Росреестр").WithIcon("gkh-dict");
            var extractRoot = root.Add("Росреестр").Add("Выписки");
            extractRoot.Add("Все выписки", "extract").AddRequiredPermission("Rosreg.AllRosreg.RoomEGRN.View"); ;
            extractRoot.Add("ЕГРН - Помещения", "extractegrn").AddRequiredPermission("Rosreg.AllRosreg.RoomEGRN.View"); ;
            extractRoot.Add("ЕГРН - Помещения юр.лиц", "#").AddRequiredPermission("Rosreg.AllRosreg.RoomEGRN.View"); ;

            var impRoot = root.Add("Росреестр").Add("Импорты");
            impRoot.Add("Импорт выписок Росреестра", "extractimport").AddRequiredPermission("Rosreg.AllRosreg.Import.View"); 
            
            var appSettings = ApplicationContext.Current.Configuration.AppSettings; 
            if(appSettings.GetAs<bool>("MinimalApp"))
            {
              //  RemoveAllItems(ref root);
            }
            
        }

        private void RemoveAllItems(ref MenuItem root)
        {
            MenuItem resultmain = root.Items.FirstOrDefault(Item => Item.Caption == "Жилищная инспекция");
            MenuItem second = resultmain.Items.FirstOrDefault(item => item.Caption == "Реестр обращений");
            MenuItem last = second.Items.FirstOrDefault(Item => Item.Caption == "Реестр СОПР");

    

            second.Items.Clear();
            resultmain.Items.Clear();
            second.Items.Add(last);
            resultmain.Items.Add(second);

            root.Items.Clear();
            root.Items.Add(resultmain);

        }
    }
}