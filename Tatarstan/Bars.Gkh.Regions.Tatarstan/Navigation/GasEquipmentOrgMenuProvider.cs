namespace Bars.Gkh.Regions.Tatarstan.Navigation
{
    using Bars.B4;

    /// <summary>
    /// Меню ВДГО
    /// </summary>
    public class GasEquipmentOrgMenuProvider : INavigationProvider
    {
        /// <inheritdoc />
        public string Key => "GasEquipmentOrg";

        /// <inheritdoc />
        public string Description => "Меню карточки ВДГО";

        /// <inheritdoc />
        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "gasequipmentorgedit/{0}/edit").WithIcon("icon-book-addresses");
            root.Add("Обслуживаемые МКД", "gasequipmentorgedit/{0}/contract").WithIcon("icon-page-white-edit");
        }
    }
}