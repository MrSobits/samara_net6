namespace Bars.Gkh.Navigation
{
    using Bars.B4;

    public class BelayPolicyMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "BelayPolicy";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки полиса";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "B4.controller.belaypolicy.Edit").AddRequiredPermission("Gkh.BelayManOrgActivity.View");
            root.Add("Застрахованные риски", "B4.controller.belaypolicy.Risk").AddRequiredPermission("Gkh.BelayManOrgActivity.View");
            root.Add("Мкд, включенные в договор", "B4.controller.belaypolicy.MkdInclude").AddRequiredPermission("Gkh.BelayManOrgActivity.View");
            root.Add("Мкд, исключенные из договора", "B4.controller.belaypolicy.MkdExclude").AddRequiredPermission("Gkh.BelayManOrgActivity.View");
            root.Add("Оплаты договора", "B4.controller.belaypolicy.Payment").AddRequiredPermission("Gkh.BelayManOrgActivity.View");
            root.Add("Страховые случаи", "B4.controller.belaypolicy.Event").AddRequiredPermission("Gkh.BelayManOrgActivity.View");
        }
    }
}