namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService
{
    using Gkh.Modules.ClaimWork.DomainService;

    public class JurJournalDebtorType : IJurJournalType
    {
        /// <summary>
        /// отображаемое имя
        /// </summary>
        public string DisplayName => "Реестр неплательщиков";

        /// <summary>
        /// роут клиенского контроллера
        /// </summary>
        public string Route => "jurjournal/debtor";
    }
}