namespace Bars.GkhCr.Modules.ClaimWork.DomainService
{
    using Gkh.Modules.ClaimWork.DomainService;

    /// <summary>
    /// 
    /// </summary>
    public class JurJournalBuildContractType : IJurJournalType
    {
        /// <summary>
        /// отображаемое имя
        /// </summary>
        public string DisplayName
        {
            get { return "Подрядчики, нарушившие условия договора"; }
        }

        /// <summary>
        /// роут клиенского контроллера
        /// </summary>
        public string Route
        {
            get { return "jurjournal/buildcontract"; }
        }
    }
}