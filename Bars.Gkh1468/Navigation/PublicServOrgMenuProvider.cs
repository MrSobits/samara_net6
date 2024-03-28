namespace Bars.Gkh1468.Navigation
{
    using Bars.B4;

    /// <summary>
    /// Меню РСО
    /// </summary>
    public class PublicServOrgMenuProvider : INavigationProvider
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public string Key => "PublicServOrg";

        /// <summary>
        /// Описание
        /// </summary>
        public string Description => "Меню карточки поставщика ресурс";

        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="root">Корень</param>
        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "publicservorgedit/{0}/edit").AddRequiredPermission("Gkh1468.Orgs.PublicServiceOrg.View").WithIcon("icon-shield-rainbow");
            root.Add("Договоры РСО", "publicservorgedit/{0}/contract").AddRequiredPermission("Gkh1468.Orgs.PublicServiceOrg.ContractsWithRealObj.View").WithIcon("icon-building-key");
        }
    }
}