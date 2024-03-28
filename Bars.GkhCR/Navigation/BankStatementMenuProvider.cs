namespace Bars.GkhCr.Navigation
{
    using B4;

    public class BankStatementMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "BankStatement";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки банковская выписка";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "B4.controller.bankstatement.Edit").WithIcon("icon-layout-header");
            root.Add("Приход", "B4.controller.bankstatement.PaymentOrderIn").WithIcon("icon-page-forward");
            root.Add("Расход", "B4.controller.bankstatement.PaymentOrderOut").WithIcon("icon-page-back");
        }
    }
}