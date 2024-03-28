namespace Bars.Gkh.RegOperator.DomainService.BankDocumentImport.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    public class BankDocumentImportCheckerTask : IBankDocumentImportCheckerTask
    {
        public IDomainService<BankDocumentImport> Domain { get; set; }

        public IBankDocumentImportService Service { get; set; }

        /// <summary>Выполнение задачи</summary>
        /// <param name="params">
        /// Параметры исполнения задачи.
        /// При вызове из планировщика передаются параметры из JobDataMap
        /// и контекст исполнения в параметре JobContext
        /// </param>
        public void Execute(DynamicDictionary @params)
        {
            var ids =
                this.Domain.GetAll()
                    .Where(x => x.CheckState != BankDocumentImportCheckState.Checked)
                    .Where(
                        x =>
                        x.PaymentConfirmationState == PaymentConfirmationState.Distributed
                        || x.PaymentConfirmationState == PaymentConfirmationState.PartiallyDistributed)
                    .Select(x => x.Id)
                    .ToArray();

            var baseParams = new BaseParams();
            baseParams.Params.SetValue("packetIds", ids);
            this.Service.TaskCheckPayments(baseParams);
        }
    }
}