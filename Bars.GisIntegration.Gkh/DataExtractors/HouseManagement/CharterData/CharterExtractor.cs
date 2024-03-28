namespace Bars.GisIntegration.Gkh.DataExtractors.HouseManagement.CharterData
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Экстрактор уставов
    /// </summary>
    public class CharterExtractor : BaseDataExtractor<Charter, ManOrgJskTsjContract>
    {
        private string managers = string.Empty;
        private Dictionary<long, int> transferCountByContractId;

        /// <summary>
        /// Выполнить обработку перед подготовкой Ris сущностей
        /// Например, подготовить словари с данными
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        /// <param name="externalEntities">Выбранные сущности внешней системы</param>
        protected override void BeforePrepareRisEntitiesHandle(DynamicDictionary parameters, List<ManOrgJskTsjContract> externalEntities)
        {
            var contragentContactDomain = this.Container.ResolveDomain<ContragentContact>();
            var manOrgContractRelationDomain = this.Container.ResolveDomain<ManOrgContractRelation>();

            try
            {
                this.managers = contragentContactDomain.GetAll()
                    .Where(x => x.Contragent.Id == this.Contragent.GkhId)
                    .Where(x => x.Position != null)
                    .Select(x => x.Position.Name)
                    .ToList()
                    .AggregateWithSeparator(", ");

                this.transferCountByContractId = manOrgContractRelationDomain.GetAll()
                    .Where(x => x.Children != null && x.Parent != null && x.TypeRelation == TypeContractRelation.TransferTsjUk)
                    .Select(
                        x => new
                        {
                            transferId = x.Children.Id,
                            manOrgContractId = x.Parent.Id
                        })
                    .ToArray()
                    .GroupBy(x => x.manOrgContractId)
                    .ToDictionary(x => x.Key, x => x.Count());
            }
            finally 
            {
                this.Container.Release(contragentContactDomain);
                this.Container.Release(manOrgContractRelationDomain);
            }
        }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<ManOrgJskTsjContract> GetExternalEntities(DynamicDictionary parameters)
        {
            var selectedChaters = parameters.GetAs("selectedList", string.Empty);
            var selectedIds = selectedChaters.ToUpper() == "ALL" ? null : selectedChaters.ToLongArray();

            var manOrgJskTsjContractDomain = this.Container.ResolveDomain<ManOrgJskTsjContract>();

            try
            {
                return manOrgJskTsjContractDomain.GetAll()
                    .WhereIf(selectedIds != null, x => selectedIds.Contains(x.Id))
                    .Where(x => x.ManagingOrganization != null)
                    .Where(x => x.ManagingOrganization.Contragent.Id == this.Contragent.GkhId)
                    .Where(x => x.ManagingOrganization.TypeManagement == TypeManagementManOrg.JSK || x.ManagingOrganization.TypeManagement == TypeManagementManOrg.TSJ)
                    .Where(x => x.TypeContractManOrgRealObj == TypeContractManOrg.JskTsj)
                    .ToList();
            }
            finally
            {
                this.Container.Release(manOrgJskTsjContractDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(ManOrgJskTsjContract externalEntity, Charter risEntity)
        {
            var transferCount = this.transferCountByContractId?.Get(externalEntity.Id);

            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "gkh";
            risEntity.DocNum = externalEntity.DocumentNumber;
            risEntity.DocDate = externalEntity.ManagingOrganization?.Contragent?.DateRegistration;
            risEntity.PeriodMeteringStartDate = externalEntity.InputMeteringDeviceValuesBeginDate;
            risEntity.PeriodMeteringEndDate = externalEntity.InputMeteringDeviceValuesEndDate;
            risEntity.PaymentDateStartDate = externalEntity.DrawingPaymentDocumentDate;
            risEntity.ThisMonthPaymentDocDate = externalEntity.ThisMonthPaymentDocDate;
            risEntity.Managers = this.managers;
            risEntity.PeriodMeteringStartDateThisMonth = externalEntity.ThisMonthInputMeteringDeviceValuesBeginDate;
            risEntity.PeriodMeteringEndDateThisMonth = externalEntity.ThisMonthInputMeteringDeviceValuesEndDate;
            risEntity.PaymentServicePeriodDate = externalEntity.PaymentServicePeriodDate;
            risEntity.ThisMonthPaymentServiceDate = externalEntity.ThisMonthPaymentServiceDate;
            risEntity.IsManagedByContract = transferCount > 0;
        }
    }
}
