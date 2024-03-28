namespace Bars.GisIntegration.Gkh.DataExtractors.HouseManagement.ContractData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Экстрактор данных по договорам
    /// </summary>
    public class ContractDataExtractor : BaseDataExtractor<RisContract, ManOrgBaseContract>
    {
        private IDictionary contractBaseDict;

        /// <summary>
        /// Менеджер справочников
        /// </summary>
        public IDictionaryManager DictionaryManager { get; set; }

        /// <summary>
        /// Получить сущности сторонней системы - договора
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы - договора</returns>
        public override List<ManOrgBaseContract> GetExternalEntities(DynamicDictionary parameters)
        {
            long[] selectedIds;

            var selectedContracts = parameters.GetAs("selectedList", string.Empty);
            if (selectedContracts.ToUpper() == "ALL")
            {
                selectedIds = null; // выбраны все, фильтрацию не накладываем
            }
            else
            {
                selectedIds = selectedContracts.ToLongArray();
            }

            var result = new List<ManOrgBaseContract>();
            var manorg = this.GetManOrgByContagentId(this.Contragent.GkhId);

            if (manorg == null)
            {
                return result;
            }

            var manOrgContractOwnersDomain = this.Container.ResolveDomain<ManOrgContractOwners>();
            //var manOrgJskTsjContractDomain = this.Container.ResolveDomain<ManOrgJskTsjContract>();
            var manOrgContractTransferDomain = this.Container.ResolveDomain < ManOrgContractTransfer>();

            try
            {
                //Дана Крысь, [16.05.16 14:31]
                //в описании задачи:
                //1) Тип управления = УК, Вид управления = УК с собственниками; (02.08.16 добавили вид управления  "УК с ТСЖ / ЖСК")
                //2) Тип управления = ТСЖ, Вид управления = ТСЖ / ЖСК (25.07.19 убрали из условия)

                result.AddRange(manOrgContractOwnersDomain.GetAll()
                    .WhereIf(selectedIds != null, x => selectedIds.Contains(x.Id))
                    .Where(x => x.ManagingOrganization.Id == manorg.Id)
                    .Where(x => x.ManagingOrganization.ActivityGroundsTermination == GroundsTermination.NotSet)
                    .Where(x => x.ManagingOrganization.TypeManagement == TypeManagementManOrg.UK)
                    .Where(x => x.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgOwners)
                    .Where(x => x.TerminateReason == null || x.TerminateReason == string.Empty)
                    .Where(x => !x.StartDate.HasValue || x.StartDate.Value <= DateTime.Now)
                    .Where(x => !x.EndDate.HasValue || x.EndDate.Value >= DateTime.Now)
                    .ToList());


                result.AddRange(manOrgContractTransferDomain.GetAll()
                    .WhereIf(selectedIds != null, x => selectedIds.Contains(x.Id))
                    .Where(x => x.ManagingOrganization.Id == manorg.Id)
                    .Where(x => x.ManagingOrganization.ActivityGroundsTermination == GroundsTermination.NotSet)
                    .Where(x => x.ManagingOrganization.TypeManagement == TypeManagementManOrg.UK)
                    .Where(x => x.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj)
                    .Where(x => x.TerminateReason == null || x.TerminateReason == string.Empty)
                    .Where(x => !x.StartDate.HasValue || x.StartDate.Value <= DateTime.Now)
                    .Where(x => !x.EndDate.HasValue || x.EndDate.Value >= DateTime.Now)
                    .ToList());

                //result.AddRange(manOrgJskTsjContractDomain.GetAll()
                //        .WhereIf(selectedIds != null, x => selectedIds.Contains(x.Id))
                //        .Where(x => x.ManagingOrganization.Id == manorg.Id)
                //        .Where(x => x.ManagingOrganization.ActivityGroundsTermination == GroundsTermination.NotSet)
                //        .Where(x => x.ManagingOrganization.TypeManagement == TypeManagementManOrg.UK)
                //        .Where(x => x.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj)
                //        .Where(x => x.TerminateReason == null || x.TerminateReason == string.Empty)
                //        .Where(x => !x.StartDate.HasValue || x.StartDate.Value <= DateTime.Now)
                //        .Where(x => !x.EndDate.HasValue || x.EndDate.Value >= DateTime.Now)
                //        .ToList());
            }
            finally
            {
                this.Container.Release(manOrgContractOwnersDomain);
                this.Container.Release(manOrgContractTransferDomain);
                // this.Container.Release(manOrgJskTsjContractDomain);
            }

            return result;
        }

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Заполнить словари
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.contractBaseDict = this.DictionaryManager.GetDictionary("ContractBaseDictionary");      
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="baseContract">Сущность внешней системы</param>
        /// <param name="risContract">Ris сущность</param>
        protected override void UpdateRisEntity(ManOrgBaseContract baseContract, RisContract risContract)
        {
            risContract.ExternalSystemEntityId = baseContract.Id;
            risContract.ExternalSystemName = "gkh";
            risContract.DocNum = baseContract.DocumentNumber;
            risContract.SigningDate = baseContract.DocumentDate;
            risContract.EffectiveDate = baseContract.StartDate;
            risContract.PlanDateComptetion = baseContract.PlannedEndDate ?? baseContract.StartDate?.AddYears(50);
            risContract.ValidityYear = this.GetYearsDifference(baseContract.StartDate, baseContract.PlannedEndDate);
            risContract.ValidityMonth = this.GetMonthsDifference(baseContract.StartDate, baseContract.PlannedEndDate);
            risContract.OwnersType = this.GetOwnersType(baseContract.ManagingOrganization.TypeManagement);
            risContract.Org = this.Contragent;                           
            risContract.ThisMonthPaymentDocDate = baseContract.ThisMonthPaymentDocDate;
            risContract.PeriodMeteringStartDateThisMonth = baseContract.ThisMonthInputMeteringDeviceValuesBeginDate;
            risContract.PeriodMeteringEndDateThisMonth = baseContract.ThisMonthInputMeteringDeviceValuesEndDate;
            risContract.PaymentServicePeriodDate = baseContract.PaymentServicePeriodDate;
            risContract.ThisMonthPaymentServiceDate = baseContract.ThisMonthPaymentServiceDate;

            var manOrgContractOwners = baseContract as ManOrgContractOwners;

            if (manOrgContractOwners != null)
            {
                var contractBaseCode = this.contractBaseDict.GetDictionaryRecord((long) manOrgContractOwners.ContractFoundation);

                risContract.ContractBaseCode = contractBaseCode?.GisCode;
                risContract.ContractBaseGuid = contractBaseCode?.GisGuid;
                risContract.InputMeteringDeviceValuesBeginDate = manOrgContractOwners.InputMeteringDeviceValuesBeginDate;
                risContract.InputMeteringDeviceValuesEndDate = manOrgContractOwners.InputMeteringDeviceValuesEndDate;
                risContract.DrawingPaymentDocumentDate = manOrgContractOwners.DrawingPaymentDocumentDate;
                risContract.ThisMonthPaymentDocDate = manOrgContractOwners.ThisMonthPaymentDocDate;
            }
            else
            {
                var manOrgJskTsjContract = baseContract as ManOrgJskTsjContract;
                var jskTsjContractBase = this.contractBaseDict.GetDictionaryRecord((long)ManOrgContractOwnersFoundation.OwnersMeetingProtocol);  // "Решение собрания собственников" всегда передаём одно значение

                if (manOrgJskTsjContract != null)
                {
                    risContract.ContractBaseCode = jskTsjContractBase.ReturnSafe(x => x.GisCode);
                    risContract.ContractBaseGuid = jskTsjContractBase.ReturnSafe(x => x.GisGuid);
                    risContract.InputMeteringDeviceValuesBeginDate = manOrgJskTsjContract.InputMeteringDeviceValuesBeginDate;
                    risContract.InputMeteringDeviceValuesEndDate = manOrgJskTsjContract.InputMeteringDeviceValuesEndDate;
                    risContract.DrawingPaymentDocumentDate = manOrgJskTsjContract.DrawingPaymentDocumentDate;
                    risContract.ThisMonthPaymentDocDate = manOrgJskTsjContract.ThisMonthPaymentDocDate;
                }
            }
        }

        private ManagingOrganization GetManOrgByContagentId(long contragentId)
        {
            var manOrgDomain = this.Container.ResolveRepository<ManagingOrganization>();

            try
            {
                return
                    manOrgDomain.GetAll()
                        .Where(x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
                        .FirstOrDefault(x => x.Contragent.Id == contragentId);
            }
            finally
            {
                this.Container.Release(manOrgDomain);
            }
        }

        /// <summary>
        /// Получает разницу между двумя датами в полных годах.
        /// </summary>
        /// <param name="start">Дата начала</param>
        /// <param name="end">Дата окончания</param>
        /// <returns>Количество лет</returns>
        private int? GetYearsDifference(DateTime? start, DateTime? end)
        {
            int? result = null;

            if (start.HasValue && end.HasValue)
            {
                result = Math.Truncate((((DateTime)end - (DateTime)start).Days / 365.25m)).ToInt();
            }

            return result;
        }

        /// <summary>
        /// Получает разницу между двумя датами в месяцах (без учёта годов).
        /// </summary>
        /// <param name="start">Дата начала</param>
        /// <param name="end">Дата окончания</param>
        /// <returns>Количество месяцев</returns>
        private int? GetMonthsDifference(DateTime? start, DateTime? end)
        {
            int? result = null;

            if (start.HasValue && end.HasValue)
            {
                var startDate = (DateTime)start;
                var endDate = (DateTime)end;
                var differenece = endDate.Month - startDate.Month;

                result = differenece > 0 ? differenece : 0;
            }

            return result;
        }

        /// <summary>
        /// Переодит значения перечисления "Тип управления управляющей организацией" в значения "Тип владельца договора".
        /// </summary>
        /// <param name="type">Тип управления управляющей организацией</param>
        /// <returns>Тип владельца договора</returns>
        private RisContractOwnersType? GetOwnersType(TypeManagementManOrg type)
        {
            RisContractOwnersType? result = null;

            switch (type)
            {
                case TypeManagementManOrg.JSK:
                    result = RisContractOwnersType.BuildingOwner;
                    break;

                case TypeManagementManOrg.Other:
                    result = RisContractOwnersType.MunicipalHousing;
                    break;

                case TypeManagementManOrg.UK:
                    result = RisContractOwnersType.Owners;
                    break;
                case TypeManagementManOrg.TSJ:
                    result = RisContractOwnersType.Cooperative;
                    break;
            }

            return result;
        }
    }
}
