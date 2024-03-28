namespace Bars.Gkh.Navigation
{
    using B4;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Меню для <see cref="Contragent"/>
    /// </summary>
    public class ContragentMenuProvider : INavigationProvider
    {
        public static string Key = nameof(Contragent);

        string INavigationProvider.Key => nameof(Contragent);

        /// <inheritdoc />
        public string Description => "Меню карточки контрагента";

        /// <inheritdoc />
        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "contragentedit/{0}/edit").AddRequiredPermission("Gkh.Orgs.Contragent.View").WithIcon("icon-shield-rainbow");
            root.Add("Данные для плана проверок", "contragentedit/{0}/auditpurpose").AddRequiredPermission("Gkh.Orgs.Contragent.Register.AuditPurpose.View");
            root.Add("Муниципальные образования", "contragentedit/{0}/municipality").AddRequiredPermission("Gkh.Orgs.Contragent.Register.Municipality.View");
            root.Add("Контакты", "contragentedit/{0}/contact").AddRequiredPermission("Gkh.Orgs.Contragent.Register.Contact.View").WithIcon("icon-user");
            root.Add("Обслуживающие банки", "contragentedit/{0}/bank").AddRequiredPermission("Gkh.Orgs.Contragent.Register.Bank.View").WithIcon("icon-money-dollar");
            root.Add("Падежи", "contragentedit/{0}/casesedit").AddRequiredPermission("Gkh.Orgs.Contragent.Register.CasesEdit.View").WithIcon("icon-text-lowercase");
            root.Add("Категории риска", "contragentedit/{0}/risk").AddRequiredPermission("Gkh.Orgs.Contragent.Register.Risk.View");
        }
    }
}