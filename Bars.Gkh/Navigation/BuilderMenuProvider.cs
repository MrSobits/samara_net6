namespace Bars.Gkh.Navigation
{
    using Bars.B4;

    public class BuilderMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "Builder";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки подрядной организации";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "B4.controller.builder.Edit").AddRequiredPermission("Gkh.Orgs.Builder.View").WithIcon("icon-shield-rainbow");
            root.Add("Документы", "B4.controller.builder.Document").AddRequiredPermission("Gkh.Orgs.Builder.View").WithIcon("icon-paste-plain");
            root.Add("Отзывы заказчиков", "B4.controller.builder.Feedback").AddRequiredPermission("Gkh.Orgs.Builder.Register.Feedback.View").WithIcon("icon-comments");
            root.Add("Займы", "B4.controller.builder.Loan").AddRequiredPermission("Gkh.Orgs.Builder.Register.Loan.View").WithIcon("icon-money");
            root.Add("Производственные базы", "B4.controller.builder.ProductionBase").AddRequiredPermission("Gkh.Orgs.Builder.Register.ProductionBase.View").WithIcon("icon-database-yellow-stop");
            root.Add("Сведения об участии в СРО", "B4.controller.builder.SroInfo").AddRequiredPermission("Gkh.Orgs.Builder.Register.SroInfo.View").WithIcon("icon-vcard");
            root.Add("Техника, инструменты", "B4.controller.builder.Technique").AddRequiredPermission("Gkh.Orgs.Builder.Register.Technique.View").WithIcon("icon-wrench");
            root.Add("Cостав трудовых ресурсов", "B4.controller.builder.Workforce").AddRequiredPermission("Gkh.Orgs.Builder.Register.Workforce.View").WithIcon("icon-user-gray");
            root.Add("Деятельность", "B4.controller.builder.Activity").AddRequiredPermission("Gkh.Orgs.Builder.Register.Activity.View").WithIcon("icon-star");
        }
    }
}