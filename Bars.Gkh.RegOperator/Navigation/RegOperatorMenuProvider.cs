namespace Bars.Gkh.RegOperator.Navigation
{
    using Bars.B4;

    /// <summary>
    /// Поставщик Меню карточки регионального оператора
    /// </summary>
    public class RegOperatorMenuProvider : INavigationProvider
    {
        public string Key => "RegOperator";

        public string Description => "Меню карточки регионального оператора";

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "B4.controller.regoperator.Edit");
            root.Add("Муниципальные образования", "B4.controller.regoperator.Municipality")
                .AddRequiredPermission("GkhRegOp.FormationRegionalFund.RegOperator.Municipality.View");
            root.Add("Счета", "B4.controller.regoperator.Accounts");
            root.Add("История ведения счетов", "B4.controller.regoperator.AccountHistory")
                .AddRequiredPermission("GkhRegOp.FormationRegionalFund.RegOperator.AccountHistory.View");
        }
    }
}