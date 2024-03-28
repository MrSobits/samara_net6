namespace Bars.Gkh.Regions.Tatarstan.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Gis.Entities.ImportAddressMatching;

    public class ExtractAlreadyMatchedAddressesAction : BaseExecutionAction
    {
        public override string Name => "Добавление уже сопоставленных адресов на страницу 'Сопоставление адресов'";

        public override string Description => "Добавляет все ранее сопоставленные адреса в реестр 'Сопоставление адресов'";

        public override Func<IDataResult> Action => this.ExtractAddressMatches;

        private BaseDataResult ExtractAddressMatches()
        {
            var importedRepository = this.Container.Resolve<IRepository<ImportedAddressMatch>>();
            var fiasRepository = this.Container.Resolve<IRepository<FiasAddressUid>>();

            try
            {
                var alreadyImported = importedRepository
                    .GetAll()
                    .Where(x => x.FiasAddress != null)
                    .Select(x => x.FiasAddress.Id);
                var alreadyMatched = fiasRepository.GetAll().Where(x => !alreadyImported.Contains(x.Id));
                var data = alreadyMatched.Select(
                    x => new ImportedAddressMatch
                    {
                        AddressCode = x.BillingId,
                        FiasAddress = x,
                        ImportDate = DateTime.Now.Date
                    });

                TransactionHelper.InsertInManyTransactions(this.Container, data);
            }
            finally
            {
                this.Container.Release(importedRepository);
                this.Container.Release(fiasRepository);
            }
            return new BaseDataResult();
        }
    }
}