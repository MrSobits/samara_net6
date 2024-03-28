namespace Bars.Gkh.Overhaul.Navigation
{
    using Bars.B4;

    public class PaysizeNavigationProvider : INavigationProvider
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
            root.Add("Справочники")
                .Add("Региональный фонд")
                .Add("Размеры взносов на КР", "paysize")
                //.WithIcon("deficitMo")
                .AddRequiredPermission("GkhRegOp.Settings.PaymentSizeCrNew.View");
        }
    }
}