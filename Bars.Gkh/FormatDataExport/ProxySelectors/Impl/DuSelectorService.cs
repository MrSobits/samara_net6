namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="DuProxy"/>
    /// </summary>
    public class DuSelectorService : BaseProxySelectorService<DuProxy>
    {
        protected enum State : byte
        {
            Active = 1,
            Prolonged = 2,
            Terminate = 3,
            Cancelled = 4
        }

        /// <inheritdoc />
        protected override ICollection<DuProxy> GetAdditionalCache()
        {
            var contractOwnersRepository = this.Container.ResolveRepository<ManOrgContractOwners>();
            var contractTransferRepository = this.Container.ResolveRepository<ManOrgContractTransfer>();

            using (this.Container.Using(contractOwnersRepository, contractTransferRepository))
            {
                return this.GetProxies(contractOwnersRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds),
                    contractTransferRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds));
            }
        }

        protected override IDictionary<long, DuProxy> GetCache()
        {
            var contractOwnersRepository = this.Container.ResolveRepository<ManOrgContractOwners>();
            var contractTransferRepository = this.Container.ResolveRepository<ManOrgContractTransfer>();

            var filtredBaseContractHashSet = this.FilterService.GetFiltredQuery<ManOrgBaseContract>().Select(x => x.Id).ToHashSet();

            using (this.Container.Using(contractOwnersRepository, contractTransferRepository))
            {
                var contractOwnersQuery = contractOwnersRepository.GetAll().WhereContainsBulked(x => x.Id, filtredBaseContractHashSet);
                var contractTransferQuery = contractTransferRepository.GetAll().WhereContainsBulked(x => x.Id, filtredBaseContractHashSet);

                return this.GetProxies(contractOwnersQuery, contractTransferQuery).ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<DuProxy> GetProxies(
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
                        x.InputMeteringDeviceValuesBeginDate,
                        x.InputMeteringDeviceValuesEndDate,
                        x.DrawingPaymentDocumentDate,
                        x.ThisMonthPaymentServiceDate,
                        x.EndDate,
                        x.TerminationDate,
                        x.ContractStopReason,
                        x.TerminateReason,
                        x.FileInfo,
                        x.ProtocolFileInfo,
                        x.StartDatePaymentPeriod,
                        x.EndDatePaymentPeriod,
                        x.PaymentAmount,
                        x.PaymentProtocolFile,
                        x.RevocationReason,
                        x.ContractFoundation,
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
                            ContractFoundation = 
                                x.ContractFoundation == ManOrgContractOwnersFoundation.OpenTenderResult
                                ? 2
                                : 1,
                            InputMeteringDeviceValuesBeginDay = x.InputMeteringDeviceValuesBeginDate,
                            InputMeteringDeviceValuesEndDay = x.InputMeteringDeviceValuesEndDate,
                            DrawingPaymentDocumentDay = x.DrawingPaymentDocumentDate,
                            DrawingPaymentDocumentMonth = x.ThisMonthPaymentServiceDate ? 1 : 2,
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
                            OwnerFile = x.FileInfo,

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
                        x.InputMeteringDeviceValuesBeginDate,
                        x.InputMeteringDeviceValuesEndDate,
                        x.DrawingPaymentDocumentDate,
                        x.ThisMonthPaymentServiceDate,
                        x.EndDate,
                        x.TerminationDate,
                        x.ContractStopReason,
                        x.TerminateReason,
                        x.FileInfo,
                        x.ProtocolFileInfo,
                        x.StartDatePaymentPeriod,
                        x.EndDatePaymentPeriod,
                        x.PaymentAmount,
                        x.PaymentProtocolFile,
                        x.RevocationReason,
                        x.ContractFoundation
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
                            ContragentOwnerId = x.ContragentOwnerId,
                            ContragentOwnerType = x.ContragentOwnerId.HasValue ? 1 : (int?) null,
                            InputMeteringDeviceValuesBeginDay = x.InputMeteringDeviceValuesBeginDate,
                            InputMeteringDeviceValuesEndDay = x.InputMeteringDeviceValuesEndDate,
                            DrawingPaymentDocumentDay = x.DrawingPaymentDocumentDate,
                            DrawingPaymentDocumentMonth = x.ThisMonthPaymentServiceDate ? 1 : 2,
                            Status = (int) status,
                            TerminationDate = status == State.Terminate ? x.TerminationDate : null,
                            TerminationReason = status == State.Terminate ? this.GetTerminationReason(x.ContractStopReason) : null,
                            CancellationReason = status == State.Cancelled ? x.TerminateReason : null,
                            ProtocolTransmittingMethod = !withoutAttachment && x.ProtocolFileInfo != null ? 2 : (int?) null,

                            //DUFILES
                            DuFile = x.FileInfo,
                            OwnerFile = x.FileInfo,

                            //DUVOTPROFILES
                            ProtocolCompetitionFile = x.FileInfo,

                            //DUOU
                            EndDate = x.EndDate,
                            IsContractReason = 1,
                            StatusDu = statusOu,
                            ReasonTermination = statusOu == 2 ? 1 : (int?) null,
                            ExceptionManagementDate = statusOu == 2 ? x.TerminationDate : (DateTime?) null,
                            RealityObjectId = manOrgRoDict.Get(x.Id)
                        };
                    });

                return manOrgContractOwnersList
                    .Union(manOrgContractTransferList)
                    .ToList();
            }
        }

        protected int? GetSetPaymentsFoundation(ManOrgSetPaymentsOwnersFoundation? value)
        {
            switch (value)
            {
                case ManOrgSetPaymentsOwnersFoundation.OwnersMeetingProtocol:
                    return 1;
                case ManOrgSetPaymentsOwnersFoundation.OpenTenderResult:
                    return 2;
                default:
                    return null;
            }
        }

        protected State GetStatus(ContractStopReasonEnum reason, bool hasTerminationDate, bool hasEndDate)
        {
            if (!hasEndDate)
            {
                return State.Active;
            }

            if (reason == ContractStopReasonEnum.finished_contract && hasTerminationDate)
            {
                return State.Terminate;
            }

            return State.Cancelled;
        }

        protected int? GetTerminationReason(ContractStopReasonEnum reason)
        {
            switch (reason)
            {
                case ContractStopReasonEnum.added_by_error:
                    return 2;
                case ContractStopReasonEnum.finished_contract:
                case ContractStopReasonEnum.is_not_filled:
                    return 5;
                case ContractStopReasonEnum.is_excluded_decision:
                    return 7;
                case ContractStopReasonEnum.revocation_of_license:
                    return 1;
                case ContractStopReasonEnum.is_excluded_refusal:
                    return 6;
                default:
                    return null;
            }
        }

        protected int? GetStatusOu(DateTime? endDate, ContractStopReasonEnum? contractStopReason)
        {
            if (!endDate.IsValid())
            {
                return 1;
            }

            switch (contractStopReason)
            {
                case ContractStopReasonEnum.finished_contract:
                case ContractStopReasonEnum.is_excluded_decision:
                case ContractStopReasonEnum.is_excluded_refusal:
                    return 2;
                case ContractStopReasonEnum.added_by_error:
                case ContractStopReasonEnum.revocation_of_license:
                    return 3;
            }

            return null;
        }
    }
}