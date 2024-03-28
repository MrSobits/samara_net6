namespace Bars.Gkh.Regions.Tatarstan.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors.Impl;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="DuProxy"/>
    /// </summary>
    public class DuTatSelectorService : DuSelectorService
    {
        protected override IList<DuProxy> GetProxies(
            IQueryable<ManOrgContractOwners> contractOwnersQuery,
            IQueryable<ManOrgContractTransfer> contractTransferQuery)
        {
            var withoutAttachment = this.SelectParams.GetAs("WithoutAttachment", false);

            var manOrgContractRealityObjectDomain = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            using (this.Container.Using(manOrgContractRealityObjectDomain))
            {
                var manOrgRoDict = manOrgContractRealityObjectDomain.GetAll()
                    .Where(x => contractOwnersQuery.Any(y => y.Id == x.ManOrgContract.Id) || contractTransferQuery.Any(y => y.Id == x.ManOrgContract.Id))
                    .Select(x => new
                    {
                        ContractId = x.ManOrgContract.Id,
                        RoId = x.RealityObject.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ContractId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.RoId).FirstOrDefault());

                var manOrgContractOwnersList = contractOwnersQuery
                    .Select(x => new
                    {
                        x.Id,
                        ContragentId = (long?) x.ManagingOrganization.Contragent.ExportId,
                        x.DocumentNumber,
                        x.DocumentDate,
                        x.StartDate,
                        x.PlannedEndDate,
                        x.IsLastDayMeteringDeviceValuesBeginDate,
                        x.InputMeteringDeviceValuesBeginDate,
                        x.ThisMonthInputMeteringDeviceValuesBeginDate,
                        x.IsLastDayMeteringDeviceValuesEndDate,
                        x.InputMeteringDeviceValuesEndDate,
                        x.ThisMonthInputMeteringDeviceValuesEndDate,
                        x.IsLastDayDrawingPaymentDocument,
                        x.DrawingPaymentDocumentDate,
                        x.ThisMonthPaymentDocDate,
                        x.IsLastDayPaymentServicePeriodDate,
                        x.PaymentServicePeriodDate,
                        x.ThisMonthPaymentServiceDate,
                        x.EndDate,
                        x.TerminationDate,
                        x.ContractStopReason,
                        x.TerminateReason,
                        x.FileInfo,
                        x.TerminationFile,
                        x.StartDatePaymentPeriod,
                        x.EndDatePaymentPeriod,
                        x.PaymentAmount,
                        x.PaymentProtocolFile,
                        x.OwnersSignedContractFile,
                        x.SetPaymentsFoundation,
                        x.RevocationReason,
                        x.ContractFoundation,
                        x.ProtocolFileInfo,
                        x.ProtocolNumber,

                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var status = this.GetStatus(x.ContractStopReason, x.TerminationDate.IsValid(), x.EndDate.IsValid());
                        var statusOu = this.GetStatusOu(x.EndDate, x.ContractStopReason);

                        return new DuProxy
                        {
                            //DU
                            Id = x.Id,
                            ContragentId = x.ContragentId,
                            DocumentNumber = x.DocumentNumber,
                            DocumentDate = x.DocumentDate ?? x.StartDate,
                            StartDate = x.StartDate,
                            PlannedEndDate = x.PlannedEndDate ?? x.StartDate.Value.AddYears(5),
                            ContragentOwnerType = 1, // Собственник объекта жилищного фонда
                            ContractFoundation = x.ContractFoundation == ManOrgContractOwnersFoundation.OpenTenderResult
                                ? 2
                                : 1,
                            IsInputMeteringDeviceValuesBeginLastDay = x.IsLastDayMeteringDeviceValuesBeginDate ? 1 : 2,
                            InputMeteringDeviceValuesBeginDay = x.InputMeteringDeviceValuesBeginDate,
                            InputMeteringDeviceValuesBeginMonth = x.ThisMonthInputMeteringDeviceValuesBeginDate ? 1 : 2,
                            IsInputMeteringDeviceValuesEndLastDay = x.IsLastDayMeteringDeviceValuesEndDate ? 1 : 2,
                            InputMeteringDeviceValuesEndDay = x.InputMeteringDeviceValuesEndDate,
                            InputMeteringDeviceValuesEndMonth = x.ThisMonthInputMeteringDeviceValuesEndDate ? 1 : 2,
                            IsDrawingPaymentDocumentLastDay = x.IsLastDayDrawingPaymentDocument ? 1 : 2,
                            DrawingPaymentDocumentDay = x.DrawingPaymentDocumentDate,
                            DrawingPaymentDocumentMonth = x.ThisMonthPaymentDocDate ? 1 : 2,
                            IsPaymentTermLastDay = x.IsLastDayPaymentServicePeriodDate ? 1 : 2,
                            PaymentTermDay = x.PaymentServicePeriodDate,
                            PaymentTermMonth = x.ThisMonthPaymentDocDate ? 1 : 2,
                            Status = (int) status,
                            TerminationDate = status == State.Terminate ? x.TerminationDate : null,
                            TerminationReason = status == State.Terminate ? this.GetTerminationReason(x.ContractStopReason) : null,
                            CancellationReason = status == State.Cancelled ? x.TerminateReason : null,
                            ProtocolTransmittingMethod = !withoutAttachment && x.ProtocolFileInfo != null ? 2 : (int?) null,
                            NoticeNumber = x.ProtocolFileInfo != null && x.ContractFoundation == ManOrgContractOwnersFoundation.OpenTenderResult
                                ? x.ProtocolNumber
                                : null,

                            //DUFILES
                            DuFile = x.FileInfo,
                            TerminationFile = statusOu == 2 ? x.TerminationFile : null,
                            OwnerFile = x.OwnersSignedContractFile,

                            //DUVOTPROFILES
                            //todo: исправить при добавлении логики для InAccordanceArticle161
                            ProtocolOwnersMeetingFile =
                                x.ContractFoundation == ManOrgContractOwnersFoundation.OwnersMeetingProtocol
                                    || x.ContractFoundation == ManOrgContractOwnersFoundation.InAccordanceArticle161 
                                    ? x.ProtocolFileInfo : null,
                            ProtocolCompetitionFile = x.ContractFoundation == ManOrgContractOwnersFoundation.OpenTenderResult ? x.ProtocolFileInfo : null,

                            //DUOU
                            EndDate = x.EndDate,
                            IsContractReason = 1,
                            StatusDu = statusOu,
                            ReasonTermination = statusOu == 2 ? 1 : (int?) null,
                            ExceptionManagementDate = statusOu == 2 ? x.TerminationDate : null,
                            RealityObjectId = manOrgRoDict.Get(x.Id)
                        };
                    });

                var manOrgContractTransferList = contractTransferQuery
                    .Select(x => new
                    {
                        x.Id,
                        ContragentId = (long?) x.ManagingOrganization.Contragent.ExportId,
                        x.DocumentNumber,
                        x.DocumentDate,
                        x.StartDate,
                        x.PlannedEndDate,
                        ContragentOwnerId = (long?) x.ManOrgJskTsj.Contragent.ExportId,
                        x.IsLastDayMeteringDeviceValuesBeginDate,
                        x.InputMeteringDeviceValuesBeginDate,
                        x.ThisMonthInputMeteringDeviceValuesBeginDate,
                        x.IsLastDayMeteringDeviceValuesEndDate,
                        x.InputMeteringDeviceValuesEndDate,
                        x.ThisMonthInputMeteringDeviceValuesEndDate,
                        x.IsLastDayDrawingPaymentDocument,
                        x.DrawingPaymentDocumentDate,
                        x.ThisMonthPaymentDocDate,
                        x.IsLastDayPaymentServicePeriodDate,
                        x.PaymentServicePeriodDate,
                        x.ThisMonthPaymentServiceDate,
                        x.EndDate,
                        x.TerminationDate,
                        x.ContractStopReason,
                        x.TerminateReason,
                        x.FileInfo,
                        x.TerminationFile,
                        x.StartDatePaymentPeriod,
                        x.EndDatePaymentPeriod,
                        x.PaymentAmount,
                        x.PaymentProtocolFile,
                        x.SetPaymentsFoundation,
                        x.RevocationReason,
                        x.ProtocolFileInfo,
                        x.ProtocolNumber
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var status = this.GetStatus(x.ContractStopReason, x.TerminationDate.IsValid(), x.EndDate.IsValid());
                        var statusOu = this.GetStatusOu(x.EndDate, x.ContractStopReason);

                        return new DuProxy
                        {
                            //DU
                            Id = x.Id,
                            ContragentId = x.ContragentId,
                            DocumentNumber = x.DocumentNumber,
                            DocumentDate = x.DocumentDate ?? x.StartDate,
                            StartDate = x.StartDate,
                            PlannedEndDate = x.PlannedEndDate ?? x.StartDate.Value.AddYears(5),

                            ContragentOwnerType = 2, // ТСЖ/Кооператив
                            ContragentOwnerId = x.ContragentOwnerId,
                            ContractFoundation = 1,

                            IsInputMeteringDeviceValuesBeginLastDay = x.IsLastDayMeteringDeviceValuesBeginDate ? 1 : 2,
                            InputMeteringDeviceValuesBeginDay = x.InputMeteringDeviceValuesBeginDate,
                            InputMeteringDeviceValuesBeginMonth = x.ThisMonthInputMeteringDeviceValuesBeginDate ? 1 : 2,
                            IsInputMeteringDeviceValuesEndLastDay = x.IsLastDayMeteringDeviceValuesEndDate ? 1 : 2,
                            InputMeteringDeviceValuesEndDay = x.InputMeteringDeviceValuesEndDate,
                            InputMeteringDeviceValuesEndMonth = x.ThisMonthInputMeteringDeviceValuesEndDate ? 1 : 2,
                            IsDrawingPaymentDocumentLastDay = x.IsLastDayDrawingPaymentDocument ? 1 : 2,
                            DrawingPaymentDocumentDay = x.DrawingPaymentDocumentDate,
                            DrawingPaymentDocumentMonth = x.ThisMonthPaymentDocDate ? 1 : 2,
                            IsPaymentTermLastDay = x.IsLastDayPaymentServicePeriodDate ? 1 : 2,
                            PaymentTermDay = x.PaymentServicePeriodDate,
                            PaymentTermMonth = x.ThisMonthPaymentDocDate ? 1 : 2,
                            Status = (int) status,
                            TerminationDate = status == State.Terminate ? x.TerminationDate : null,
                            TerminationReason = status == State.Terminate ? this.GetTerminationReason(x.ContractStopReason) : null,
                            CancellationReason = status == State.Cancelled ? x.TerminateReason : null,
                            ProtocolTransmittingMethod = !withoutAttachment && x.ProtocolFileInfo != null ? 2 : (int?) null,

                            //DUFILES
                            DuFile = x.FileInfo,
                            TerminationFile = statusOu == 2 ? x.TerminationFile : null,

                            //DUVOTPROFILES
                            ProtocolCompetitionFile = x.FileInfo,

                            //DUOU
                            EndDate = x.EndDate,
                            IsContractReason = 1,
                            StatusDu = statusOu,
                            ReasonTermination = statusOu == 2 ? 1 : (int?) null,
                            ExceptionManagementDate = statusOu == 2 ? x.TerminationDate : null,
                            RealityObjectId = manOrgRoDict.Get(x.Id)
                        };

                    });

                return manOrgContractOwnersList
                    .Union(manOrgContractTransferList)
                    .ToList();
            }
        }
    }
}