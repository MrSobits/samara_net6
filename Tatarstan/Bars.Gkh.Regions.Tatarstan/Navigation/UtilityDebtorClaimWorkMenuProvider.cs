namespace Bars.Gkh.Regions.Tatarstan.Navigation
{
    using Bars.B4;

    /// <summary>
    /// Меню карточки основания ПИР по неплательщикам ЖКУ
    /// </summary>
    public class UtilityDebtorClaimWorkMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "UtilityDebtorClaimWork";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки основания ПИР по неплательщикам ЖКУ";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Задолженность по оплате ЖКУ", "claimwork/{0}/{1}/utilitydebtoredit");
        }
    }
}