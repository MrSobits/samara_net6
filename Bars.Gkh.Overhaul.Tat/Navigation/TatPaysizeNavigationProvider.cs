namespace Bars.Gkh.Overhaul.Tat.Navigation
{
    using Bars.B4;

    public class TatPaysizeNavigationProvider : INavigationProvider
    {
        public string Key
        {
            get { return MainNavigationInfo.MenuName; }
        }

        public string Description
        {
            get
            {
                return MainNavigationInfo.MenuDescription;
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Капитальный ремонт")
                .Add("Параметры программы капитального ремонта")
                .Add("Размеры взносов на КР", "paymentsizecr")
                .AddRequiredPermission("Ovrhl.Dictionaries.PaymentSizeCr.View")
                .WithIcon("deficitMo");
        }
    }
}