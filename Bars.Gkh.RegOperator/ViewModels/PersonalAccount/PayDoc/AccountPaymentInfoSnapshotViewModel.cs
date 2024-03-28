namespace Bars.Gkh.RegOperator.ViewModels.PersonalAccount.PayDoc
{
    using System.Linq;

    using Bars.B4;

    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    
    internal class AccountPaymentInfoSnapshotViewModel : BaseViewModel<AccountPaymentInfoSnapshot>
    {
        public IDomainService<PersonalAccountPeriodSummary> PeriodSummaryDomain { get; set; }

        public IPaymentDocumentService PaymentDocumentService { get; set; }

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<AccountPaymentInfoSnapshot> domainService, BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);

            var snapshotId = baseParams.Params.GetAsId("snapshotId");

            var data = this.PaymentDocumentService.GetPaymentInfoSnapshots(snapshotId);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), data.Count());
        }
    }
}