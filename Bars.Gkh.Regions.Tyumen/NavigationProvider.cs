namespace Bars.Gkh.Regions.Tyumen
{
    using Bars.B4;
    using Bars.Gkh.TextValues;

    /// <summary>
    ///     Меню, навигация
    /// </summary>
    public class NavigationProvider : INavigationProvider
    {
        /// <summary>
        /// Текстовое обозначение пунктов меню
        /// </summary>
        public IMenuItemText MenuItemText { get; set; }

        /// <summary>
        /// Ключ
        /// </summary>
        public string Key
        {
            get
            {
                return MainNavigationInfo.MenuName;
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description
        {
            get
            {
                return MainNavigationInfo.MenuDescription;
            }
        }
        
        /// <summary>
        /// Инициализация меню
        /// </summary>
        /// <param name="root"></param>
        public void Init(MenuItem root)
        {
            var dictsRoot = root.Add("Справочники").WithIcon("gkh-dict");
            var gkhDictsRoot = dictsRoot.Add("Жилищно-коммунальное хозяйство");

            gkhDictsRoot.Add("Типы лифта", "lifttype").AddRequiredPermission("Gkh.Dictionaries.TypeLift.View");
            gkhDictsRoot.Add("Модель лифта", "liftmodel").AddRequiredPermission("Gkh.Dictionaries.ModelLift.View");
            gkhDictsRoot.Add("Лифт (кабина)", "liftcabin").AddRequiredPermission("Gkh.Dictionaries.CabinLift.View");
            gkhDictsRoot.Add("Типы шахт лифта", "typeliftshaft").AddRequiredPermission("Gkh.Dictionaries.TypeLiftShaft.View");
            gkhDictsRoot.Add("Типы приводов дверей кабины лифта", "typeliftdrivedoors").AddRequiredPermission("Gkh.Dictionaries.TypeLiftDriveDoors.View");
            gkhDictsRoot.Add("Расположение машинного помещения", "typeliftmashineroom").AddRequiredPermission("Gkh.Dictionaries.TypeLiftMashineRoom.View");

            root.Add("Администрирование")
                .Add("Импорт жилых домов")
                .Add("Импорт жилых домов (Обследование)", "realityobjectexaminationimport")
                .AddRequiredPermission("Import.RealityObjectExaminationImport.View");
        }
    }
}