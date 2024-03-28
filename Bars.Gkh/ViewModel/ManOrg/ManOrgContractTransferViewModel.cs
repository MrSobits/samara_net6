namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using B4;

    using Bars.Gkh.Enums;

    using Entities;

    /// <summary>
    /// ViewModel для ManOrgContractTransfer
    /// </summary>
    public class ManOrgContractTransferViewModel : BaseViewModel<ManOrgContractTransfer>
    {
        /// <summary>
        /// Домен-сервис <see cref="ManOrgContractRealityObject"/>
        /// </summary>
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectDomain { get; set; }

        /// <summary>
        /// Получение объекта
        /// </summary>
        /// <param name="domain">Домен ManOrgContractTransfer</param>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public override IDataResult Get(IDomainService<ManOrgContractTransfer> domain, BaseParams baseParams)
        {
            var obj = domain.Get(baseParams.Params.GetAs<long>("id"));

            var roAddress = this.ManOrgContractRealityObjectDomain.GetAll()
                .Where(x => x.ManOrgContract.Id == obj.Id)
                .Select(x => x.RealityObject.Address)
                .FirstOrDefault();

            return new BaseDataResult
            (
                new
                {
                    obj.Id,
                    obj.DocumentDate,
                    obj.DocumentName,
                    obj.DocumentNumber,
                    obj.StartDate,
                    obj.EndDate,
                    obj.PlannedEndDate,
                    obj.TerminateReason,
                    obj.FileInfo,
                    obj.Note,
                    ManOrgJskTsj = new
                    {
                        obj.ManOrgJskTsj.Id,
                        obj.ManOrgJskTsj.Contragent.Name
                    },
                    RealityObjectName = roAddress,
                    ManagingOrganization = new
                    {
                        obj.ManagingOrganization.Id,
                        ContragentName = obj.ManagingOrganization.Contragent.Name
                    },
                    obj.InputMeteringDeviceValuesBeginDate,
                    obj.InputMeteringDeviceValuesEndDate,
                    obj.DrawingPaymentDocumentDate,
                    obj.ProtocolNumber,
                    obj.ProtocolDate,
                    obj.ProtocolFileInfo,
                    obj.ThisMonthPaymentDocDate,
                    obj.PaymentServicePeriodDate,
                    obj.ThisMonthInputMeteringDeviceValuesBeginDate,
                    obj.ThisMonthInputMeteringDeviceValuesEndDate,
                    obj.ThisMonthPaymentServiceDate,
                    obj.ContractFoundation,
                    obj.ContractStopReason,
                    obj.TerminationDate,
                    obj.TerminationFile,
                    obj.StartDatePaymentPeriod,
                    obj.EndDatePaymentPeriod,
                    obj.PaymentAmount,
                    obj.PaymentProtocolFile,
                    obj.SetPaymentsFoundation,
                    obj.PaymentProtocolDescription
                });
        }
    }
}