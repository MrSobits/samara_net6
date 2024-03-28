namespace Bars.Gkh.Navigation
{
    using Bars.B4;

    /// <summary>
    /// Меню Жилищной инспекции
    /// </summary>
    public class HousingInspectionMenuProvider : INavigationProvider
    {
        /// <inheritdoc />
        public string Key => "HousingInspection";

        /// <inheritdoc />
        public string Description => "Меню карточки Жилищной инспекции";

        /// <inheritdoc />
        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "housinginspectionedit/{0}/edit").WithIcon("icon-book-addresses");
            root.Add("Мунципальные образования", "housinginspectionedit/{0}/municipality").WithIcon("icon-page-white-edit");
        }
    }
}