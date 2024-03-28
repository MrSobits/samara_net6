namespace Bars.Gkh.RegOperator.Navigation
{
    using Bars.B4;

    public class LegalClaimWorkMenuProvider : INavigationProvider
    {
        public string Key => "LegalClaimWork";

        public string Description => "Меню карточки основания ПИР по неплательщикам-юридическим лицам";

        public void Init(MenuItem root)
        {
            root.Add("Задолженность по оплате КР", "claimwork/{0}/{1}/legaledit");
        }
    }
}