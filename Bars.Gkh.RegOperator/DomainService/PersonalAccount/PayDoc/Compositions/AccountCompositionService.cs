namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.Assembly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;

    /// <summary>
    /// Сервис для обычной компоновки лс в файлы квитанций
    /// </summary>
    public class AccountCompositionService : ICompositionService
    {
        private int generated;

        /// <inheritdoc />
        public IEnumerable<IEnumerable<PaymentDocumentSnapshot>> GetAccountPortion(
            List<PaymentDocumentSnapshot> accountData,
            RegOperatorConfig config)
        {
            this.generated = 0;

            var accountsPerFile = config.PaymentDocumentConfigContainer.PaymentDocumentConfigIndividual.PhysicalAccountsPerDocument > 0
                        ? Math.Max(config.PaymentDocumentConfigContainer.PaymentDocumentConfigIndividual.PhysicalAccountsPerDocument, 1)
                    : accountData.Count;

            while (this.generated < accountData.Count)
            {
                var portion = accountData.Skip(this.generated).Take(accountsPerFile);

                yield return portion;

                this.generated += portion.Count();
            }           
        }

        /// <inheritdoc />
        public string GetFileName(IEnumerable<PaymentDocumentSnapshot> accountData)
        {
            string name = $"{this.generated + 1}-{this.generated + accountData.Count()}";
            return name;
        }
    }
}