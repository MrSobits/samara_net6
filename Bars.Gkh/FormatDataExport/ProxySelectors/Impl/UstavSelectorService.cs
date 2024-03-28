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
    /// Сервис получения <see cref="UstavProxy"/>
    /// </summary>
    public class UstavSelectorService : BaseProxySelectorService<UstavProxy>
    {
        /// <inheritdoc />
        protected override ICollection<UstavProxy> GetAdditionalCache()
        {
            var jskTsjContractRepository = this.Container.ResolveRepository<ManOrgJskTsjContract>();
            var roDirectManagContractRepository = this.Container.ResolveRepository<RealityObjectDirectManagContract>();

            using (this.Container.Using(jskTsjContractRepository, roDirectManagContractRepository))
            {
                return this.GetProxies(jskTsjContractRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds),
                    roDirectManagContractRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds));
            }
        }

        protected override IDictionary<long, UstavProxy> GetCache()
        {
            var jskTsjContractRepository = this.Container.ResolveRepository<ManOrgJskTsjContract>();
            var roDirectManagContractRepository = this.Container.ResolveRepository<RealityObjectDirectManagContract>();

            var filtredBaseContractHashSet = this.FilterService.GetFiltredQuery<ManOrgBaseContract>().Select(x => x.Id).ToHashSet();

            using (this.Container.Using(jskTsjContractRepository, roDirectManagContractRepository))
            {
                var contractOwnersQuery = jskTsjContractRepository.GetAll().WhereContainsBulked(x => x.Id, filtredBaseContractHashSet);
                var contractTransferQuery = roDirectManagContractRepository.GetAll().WhereContainsBulked(x => x.Id, filtredBaseContractHashSet);

                return this.GetProxies(contractOwnersQuery, contractTransferQuery).ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<UstavProxy> GetProxies(
            IQueryable<ManOrgJskTsjContract> jskTsjContractQuery,
            IQueryable<RealityObjectDirectManagContract> roDirectManagContractQuery)
        {
            var manOrgContractRelationRepository = this.Container.ResolveRepository<ManOrgContractRelation>();
            var manOrgContractRealityObjectDomain = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            
            using (this.Container.Using(manOrgContractRelationRepository, manOrgContractRealityObjectDomain))
            {
                var manOrgRoDict = manOrgContractRealityObjectDomain.GetAll()
                    .Where(x => jskTsjContractQuery.Any(y => y.Id == x.ManOrgContract.Id) || roDirectManagContractQuery.Any(y => y.Id == x.ManOrgContract.Id))
                    .Select(x => new
                    {
                        ContractId = x.ManOrgContract.Id,
                        RoId = x.RealityObject.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ContractId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.RoId).FirstOrDefault());

                var contractRealations = this.FilterService
                    .FilterByContragent(manOrgContractRelationRepository.GetAll(), x => x.Parent.ManagingOrganization.Contragent)
                    .Where(x => !x.Children.EndDate.HasValue)
                    .Select(x => x.Parent.Id)
                    .ToList();

                var jskTsjContractList = jskTsjContractQuery
                    .Select(x => new
                    {
                        x.Id,
                        x.TypeContractManOrgRealObj,
                        ContragentId = x.ManagingOrganization.Contragent.ExportId,
                        x.DocumentNumber,
                        x.DocumentDate,
                        x.StartDate,
                        x.IsLastDayMeteringDeviceValuesBeginDate,
                        x.InputMeteringDeviceValuesBeginDate,
                        x.ThisMonthInputMeteringDeviceValuesBeginDate,
                        x.IsLastDayMeteringDeviceValuesEndDate,
                        x.InputMeteringDeviceValuesEndDate,
                        x.ThisMonthInputMeteringDeviceValuesEndDate,
                        x.IsLastDayDrawingPaymentDocument,
                        x.DrawingPaymentDocumentDate,
                        x.ThisMonthPaymentDocDate,
                        x.PaymentServicePeriodDate,
                        x.IsLastDayPaymentServicePeriodDate,
                        x.ThisMonthPaymentServiceDate,
                        x.EndDate,
                        x.TerminationDate,
                        x.ContractStopReason,
                        x.TerminateReason,
                        x.FileInfo,
                        x.ProtocolFileInfo,
                        x.StartDatePaymentPeriod,
                        x.EndDatePaymentPeriod,
                        x.PaymentProtocolFile,
                        x.ManagingOrganization.Contragent.ContragentState,
                        x.CompanyReqiredPaymentAmount,
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var status = this.GetState(x.EndDate, x.ContractStopReason);
                        var statusOu = this.GetStatusOu(x.EndDate, x.ContractStopReason);

                        return new UstavProxy
                        {
                            //USTAV
                            Id = x.Id,
                            ContragentId = x.ContragentId,
                            DocumentNumber = x.DocumentNumber,
                            IsTimingInfoExists = (x.InputMeteringDeviceValuesBeginDate > 0 || x.IsLastDayMeteringDeviceValuesBeginDate)
                                && (x.InputMeteringDeviceValuesEndDate > 0 || x.IsLastDayMeteringDeviceValuesEndDate)
                                && (x.DrawingPaymentDocumentDate > 0 || x.IsLastDayDrawingPaymentDocument)
                                && (x.PaymentServicePeriodDate > 0 || x.IsLastDayPaymentServicePeriodDate)
                                    ? 1
                                    : 2,
                            IsInputMeteringDeviceValuesLastDay = x.IsLastDayMeteringDeviceValuesBeginDate ? 1 : 2,
                            InputMeteringDeviceValuesBeginDay = x.InputMeteringDeviceValuesBeginDate,
                            ThisMonthInputMeteringDeviceValuesBeginDate = x.ThisMonthInputMeteringDeviceValuesBeginDate ? 1 : 2,
                            IsLastDayMeteringDeviceValuesBeginDate = x.IsLastDayMeteringDeviceValuesEndDate ? 1 : 2,
                            InputMeteringDeviceValuesEndDay = x.InputMeteringDeviceValuesEndDate,
                            ThisMonthInputMeteringDeviceValuesEndDate = x.ThisMonthInputMeteringDeviceValuesEndDate ? 1 : 2,
                            IsDrawingPaymentDocumentLastDay = x.IsLastDayDrawingPaymentDocument ? 1 : 2,
                            DrawingPaymentDocumentDay = x.DrawingPaymentDocumentDate,
                            ThisMonthPaymentDocDate = x.ThisMonthPaymentDocDate ? 1 : 2,
                            IsPaymentServicePeriodLastDay = x.IsLastDayPaymentServicePeriodDate ? 1 : 2,
                            PaymentServicePeriodDay = x.PaymentServicePeriodDate,
                            ThisMonthPaymentServiceDate = x.ThisMonthPaymentServiceDate ? 1 : 2,
                            Status = status,
                            TerminateReason = status == 4 ? x.TerminateReason : string.Empty,
                            TerminationDate = status == 3 ? x.TerminationDate : null,
                            ContractStopReason = status == 3 ? x.TerminateReason : string.Empty,
                            IsProtocolContainsDecision = x.ProtocolFileInfo == null ? 1 : 2,

                            //USTAVFILES
                            OssFile = x.ProtocolFileInfo,
                            UstavFile = x.FileInfo,

                            //USTAVOU
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            StatusOu = statusOu,
                            IsContractReason = 1,
                            IsManagementContract = contractRealations.Contains(x.Id) ? 1 : 2,
                            IsExclusionReason = 1,
                            ExcludeDate = statusOu == 2 ? x.TerminationDate : null,
                            RealityObjectId = manOrgRoDict.Get(x.Id)
                        };
                    });

                var roDirectManagContractList = roDirectManagContractQuery
                    .Select(x => new
                    {
                        x.Id,
                        x.TypeContractManOrgRealObj,
                        ContragentId = x.ManagingOrganization.Contragent.ExportId,
                        x.DocumentNumber,
                        x.DocumentDate,
                        x.StartDate,
                        x.PlannedEndDate,
                        x.ThisMonthPaymentServiceDate,
                        x.EndDate,
                        x.ContractStopReason,
                        x.TerminateReason,
                        x.FileInfo,
                        x.StartDatePaymentPeriod,
                        x.EndDatePaymentPeriod,
                        x.ThisMonthPaymentDocDate,
                        x.PaymentServicePeriodDate,
                        x.ManagingOrganization.Contragent.ContragentState,
                        x.TerminationDate
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var status = this.GetState(x.EndDate, x.ContractStopReason);
                        var statusOu = this.GetStatusOu(x.EndDate, x.ContractStopReason);

                        return new UstavProxy
                        {
                            //USTAV
                            Id = x.Id,
                            ContragentId = x.ContragentId,
                            DocumentNumber = x.DocumentNumber,
                            ThisMonthPaymentDocDate = x.ThisMonthPaymentDocDate ? 1 : 2,
                            PaymentServicePeriodDay = x.PaymentServicePeriodDate,
                            ThisMonthPaymentServiceDate = x.ThisMonthPaymentServiceDate ? 1 : 2,
                            Status = status,

                            //USTAVFILES
                            OssFile = x.FileInfo,

                            //USTAVOU
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            StatusOu = statusOu,
                            IsContractReason = 2,
                            ProtocolMeetingFile = x.FileInfo,
                            IsManagementContract = contractRealations.Contains(x.Id) ? 1 : 2,
                            IsExclusionReason = statusOu == 2 ? 2 : default(int?),
                            ProtocolMeetingExcludeFile = statusOu == 2 ? x.FileInfo : null,
                            ExcludeDate = statusOu == 2 ? x.TerminationDate : null,
                        };
                    });

                return jskTsjContractList
                    .Union(roDirectManagContractList)
                    .ToList();
            }
        }

        private int? GetState(DateTime? endDate, ContractStopReasonEnum? contractStopReason)
        {
            if (!endDate.IsValid())
            {
                return 1;
            }

            switch (contractStopReason)
            {
                case ContractStopReasonEnum.finished_contract:
                    return 3;
                default:
                    return 4;
            }
        }

        private int? GetStatusOu(DateTime? endDate, ContractStopReasonEnum? contractStopReason)
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