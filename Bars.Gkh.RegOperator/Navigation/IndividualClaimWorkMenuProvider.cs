namespace Bars.Gkh.RegOperator.Navigation
{
    using Bars.B4;

    public class IndividualClaimWorkMenuProvider : INavigationProvider
    {
        public string Key => "IndividualClaimWork";

        public string Description => "Меню карточки основания ПИР по неплательщикам-физическим лицам";

        public void Init(MenuItem root)
        {
            root.Add("Задолженность по оплате КР", "claimwork/{0}/{1}/individualedit");
        }
    }
}