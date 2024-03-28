namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.Assembly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;

    /// <summary>
    /// Сервис для компоновки лс в файлы квитанций, сгруппированных по агенту доставки
    /// </summary>
    public class DeliveryAgentCompositionService : ICompositionService
    {
        private int generatedByAgent;

        /// <inheritdoc />
        public IEnumerable<IEnumerable<PaymentDocumentSnapshot>> GetAccountPortion(
            List<PaymentDocumentSnapshot> accountData,
            RegOperatorConfig config)
        {
            var accountsPerFile = config.PaymentDocumentConfigContainer.PaymentDocumentConfigIndividual.PhysicalAccountsPerDocument > 0
                ? Math.Max(config.PaymentDocumentConfigContainer.PaymentDocumentConfigIndividual.PhysicalAccountsPerDocument, 1)
                : 0;

            var groupedAccountData = accountData.GroupBy(x => x.DeliveryAgent);

            foreach (var group in groupedAccountData)
            {
                this.generatedByAgent = 0;
                var groupCount = group.Count();

                var accountsPerFileInGroup = accountsPerFile != 0
                    ? accountsPerFile
                    : groupCount;

                while (this.generatedByAgent < groupCount)
                {
                    var snapshots = group.Skip(this.generatedByAgent).Take(accountsPerFileInGroup);                                      
                    yield return snapshots;

                    this.generatedByAgent += snapshots.Count();
                }
            }
        }

        /// <inheritdoc />
        public string GetFileName(IEnumerable<PaymentDocumentSnapshot> accountData)
        {
            string name = $"{accountData.First().DeliveryAgent}({this.generatedByAgent + 1}-{this.generatedByAgent + accountData.Count()})";
            return name;
        }
    }
}
