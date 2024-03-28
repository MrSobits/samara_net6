namespace Bars.GkhCr.Navigation
{
    using Bars.B4;

    public class BuildContractClaimWorkMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "BuildContractClaimWork";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки основания ПИР по договораям подряда";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Нарушение условий договора", "claimworkbc/BuildContractClaimWork/{0}/buildctredit");
        }
    }
}