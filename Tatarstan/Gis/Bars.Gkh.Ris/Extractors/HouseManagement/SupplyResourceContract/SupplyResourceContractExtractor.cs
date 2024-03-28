namespace Bars.Gkh.Ris.Extractors.HouseManagement.SupplyResourceContract
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities.ContractPart;
    using Bars.Gkh.Modules.Gkh1468.Enums;

    /// <summary>
    /// Экстрактор договоров с поставщиками ресурсов
    /// </summary>
    public class SupplyResourceContractExtractor : BaseDataExtractor<SupplyResourceContract, PublicServiceOrgContract>
    {
        private IDictionary contractBaseDict;
        private IDictionary ownerDocumentTypeDict;
        private IDictionary terminateReasonDict;
        private Dictionary<long, TypeContractPart> contractPartTypeByContractId;
        private Dictionary<long, JurPersonOwnerContract> typeContactJurPersonByContractId;
        private Dictionary<long, IndividualOwnerContract> typeContactIndPersonByContractId;
        private Dictionary<long, RsoAndServicePerformerContract> typeContactRsoAndServicePerformerByContractId;
        private Dictionary<long, RisContragent> risContragentsByGkhId;

        /// <summary>
        /// Менеджер справочников
        /// </summary>
        public IDictionaryManager DictionaryManager { get; set; }

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Заполнить словари
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            var contractPartDomain = this.Container.ResolveDomain<BaseContractPart>();
            var jurPersonOwnerContractDomain = this.Container.ResolveDomain<JurPersonOwnerContract>();
            var individualOwnerContractDomain = this.Container.ResolveDomain<IndividualOwnerContract>();
            var rsoAndServicePerformerContractDomain = this.Container.ResolveDomain<RsoAndServicePerformerContract>();
            var risContragentDomain = this.Container.ResolveDomain<RisContragent>();

            try
            {
                this.risContragentsByGkhId = risContragentDomain.GetAll()
                    .Where(x => x.OrgRootEntityGuid != "")
                    .GroupBy(x => x.GkhId)
                    .ToDictionary(x => x.Key, x => x.First());

                this.typeContactRsoAndServicePerformerByContractId = rsoAndServicePerformerContractDomain.GetAll()
                   .Where(x => x.PublicServiceOrgContract != null)
                   .Where(x => x.PublicServiceOrgContract.PublicServiceOrg != null && x.PublicServiceOrgContract.PublicServiceOrg.Contragent != null)
                   .Where(x => x.PublicServiceOrgContract.PublicServiceOrg.Contragent.Id == this.Contragent.GkhId)
                   .Select(
                       x => new
                       {
                           ContractId = x.PublicServiceOrgContract.Id,
                           ContractOwner = x
                       })
                   .ToList()
                   .GroupBy(x => x.ContractId)
                   .ToDictionary(x => x.Key, x => x.First().ContractOwner);

                this.typeContactJurPersonByContractId = jurPersonOwnerContractDomain.GetAll()
                    .Where(x => x.PublicServiceOrgContract != null)
                    .Where(x => x.PublicServiceOrgContract.PublicServiceOrg != null && x.PublicServiceOrgContract.PublicServiceOrg.Contragent != null)
                    .Where(x => x.PublicServiceOrgContract.PublicServiceOrg.Contragent.Id == this.Contragent.GkhId)
                    .Select(
                        x => new
                        {
                            ContractId = x.PublicServiceOrgContract.Id,
                            ContractOwner = x
                        })
                    .ToList()
                    .GroupBy(x => x.ContractId)
                    .ToDictionary(x => x.Key, x => x.First().ContractOwner);

                this.typeContactIndPersonByContractId = individualOwnerContractDomain.GetAll()
                    .Where(x => x.PublicServiceOrgContract != null)
                    .Where(x => x.PublicServiceOrgContract.PublicServiceOrg != null && x.PublicServiceOrgContract.PublicServiceOrg.Contragent != null)
                    .Where(x => x.PublicServiceOrgContract.PublicServiceOrg.Contragent.Id == this.Contragent.GkhId)
                    .Select(
                        x => new
                        {
                            ContractId = x.PublicServiceOrgContract.Id,
                            ContractOwner = x
                        })
                    .ToList()
                    .GroupBy(x => x.ContractId)
                    .ToDictionary(x => x.Key, x => x.First().ContractOwner);

                this.contractPartTypeByContractId = contractPartDomain.GetAll()
                    .Where(x => x.PublicServiceOrgContract != null)
                    .Where(x => x.PublicServiceOrgContract.PublicServiceOrg != null && x.PublicServiceOrgContract.PublicServiceOrg.Contragent != null)
                    .Where(x => x.PublicServiceOrgContract.PublicServiceOrg.Contragent.Id == this.Contragent.GkhId)
                    .Select(
                        x => new
                        {
                            ContractId = x.PublicServiceOrgContract.Id,
                            x.TypeContractPart
                        })
                    .ToList()
                    .GroupBy(x => x.ContractId)
                    .ToDictionary(x => x.Key, x => x.First().TypeContractPart);

                this.contractBaseDict = this.DictionaryManager.GetDictionary("SupplyResContractBaseDictionary");

                this.ownerDocumentTypeDict = this.DictionaryManager.GetDictionary("OwnerDocumentTypeDictionary");

                this.terminateReasonDict = this.DictionaryManager.GetDictionary("StopReasonDictionary");
            }
            finally
            {
                this.Container.Release(contractPartDomain);
                this.Container.Release(jurPersonOwnerContractDomain);
                this.Container.Release(individualOwnerContractDomain);
                this.Container.Release(rsoAndServicePerformerContractDomain);
                this.Container.Release(risContragentDomain);
            }
        }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<PublicServiceOrgContract> GetExternalEntities(DynamicDictionary parameters)
        {
            var domain = this.Container.ResolveDomain<PublicServiceOrgContract>();

            try
            {
                long[] selectedIds;

                var selectedHouses = parameters.GetAs("selectedList", string.Empty);
                if (selectedHouses.ToUpper() == "ALL")
                {
                    selectedIds = null; // выбраны все, фильтрацию не накладываем
                }
                else
                {
                    selectedIds = selectedHouses.ToLongArray();
                }

                return domain.GetAll()
                    .WhereIf(selectedIds != null, x => selectedIds.Contains(x.RealityObject.Id))
                    .Where(x => x.PublicServiceOrg != null && x.PublicServiceOrg.Contragent != null)
                    .Where(x => x.PublicServiceOrg.Contragent.Id == this.Contragent.GkhId)
                    .ToList();
            }
            finally
            {
                this.Container.Release(domain);
            }
        }

        /// <summary>
        /// Получить сторону договора ресурсоснабжения
        /// </summary>
        /// <param name="contractId">Идентификатор договора</param>
        /// <returns>Сторона договора ресурсоснабжения</returns>
        private SupplyResourceContactPersonType? GetPersonType(long contractId)
        {
            SupplyResourceContactPersonType? result = null;

            var typeContractPerson =
                this.typeContactJurPersonByContractId?.Get(contractId)?.TypeContactPerson ??
                this.typeContactIndPersonByContractId?.Get(contractId)?.TypeContactPerson;

            if (typeContractPerson != null)
            {
                var gkhTypeIndex = (int)typeContractPerson;
                result = (SupplyResourceContactPersonType)gkhTypeIndex;
            }

            return result;
        }

        /// <summary>
        /// Получить тип лица/ организации договора ресурососнабжения
        /// </summary>
        /// <param name="contractId">Идентификатор договора</param>
        /// <returns>Тип лица/ организации</returns>
        private SupplyResourceContractType? GetContractType(long contractId)
        {
            SupplyResourceContractType? result = null;
            var gkhContractType = this.contractPartTypeByContractId?.Get(contractId);

            if (gkhContractType != null)
            {
                var gkhTypeIndex = (int)gkhContractType;
                result = (SupplyResourceContractType)gkhTypeIndex;
            }

            return result;
        }

        /// <summary>
        /// Получить вид организации 
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        private SupplyResourceContactPersonTypeOrganization? GetPersonTypeOrganization(long contractId)
        {
            SupplyResourceContactPersonTypeOrganization? result = null;

            if (this.typeContactJurPersonByContractId.ContainsKey(contractId))
            {
                result = SupplyResourceContactPersonTypeOrganization.RegOrg;
            }
            else if (this.typeContactIndPersonByContractId.ContainsKey(contractId))
            {
                result = SupplyResourceContactPersonTypeOrganization.Ind;
            }

            return result;
        }

        /// <summary>
        /// Получить RisGender
        /// </summary>
        /// <param name="gender">ЖКХ пол</param>
        /// <returns>RisGender</returns>
        private RisGender? GetRisSex(GenderR? gender)
        {
            RisGender? result = null;

            if (gender != null)
            {
                result = gender == GenderR.Male ? RisGender.M : RisGender.F;
            }

            return result;
        }

        /// <summary>
        /// Получить RisContragent
        /// </summary>
        /// <param name="contragent">Контрагент ЖКХ</param>
        /// <returns>RisContragent</returns>
        private RisContragent GetRisContragent(Contragent contragent)
        {
            RisContragent result = null;

            if (contragent != null)
            {
                result = this.risContragentsByGkhId?.Get(contragent.Id);
            }

            return result;
        }

        /// <summary>
        /// Получить SupResCommercialMeteringResourceType
        /// </summary>
        /// <param name="contractId">Идентификатор договора</param>
        /// <returns>SupResCommercialMeteringResourceType</returns>
        private SupResCommercialMeteringResourceType? GetMeteringResourceType(long contractId)
        {
            SupResCommercialMeteringResourceType? result = null;
            var gkhTypeContactRsoAndServicePerformer = this.typeContactRsoAndServicePerformerByContractId?.Get(contractId);
            var gkhType = gkhTypeContactRsoAndServicePerformer?.CommercialMeteringResourceType;

            if (gkhType != null)
            {
                var gkhTypeIndex = (int)gkhType;
                result = (SupResCommercialMeteringResourceType)gkhTypeIndex;
            }

            return result;
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(PublicServiceOrgContract externalEntity, SupplyResourceContract risEntity)
        {
            var contractBase = this.contractBaseDict.GetDictionaryRecord((long)externalEntity.ResOrgReason);
            var terminateReason = this.terminateReasonDict.GetDictionaryRecord(externalEntity.StopReason.Id);
            var contactJurPerson = this.typeContactJurPersonByContractId?.Get(externalEntity.Id);
            var contactIndPerson = this.typeContactIndPersonByContractId?.Get(externalEntity.Id);
            var ownerDocumentType = this.ownerDocumentTypeDict.GetDictionaryRecord((long)(contactIndPerson?.OwnerDocumentType ?? 0));

            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "gkh";
            risEntity.ComptetionDate = externalEntity.DateEnd;
            risEntity.StartDate = (byte)externalEntity.DayStart;
            risEntity.StartDateNextMonth = externalEntity.StartDeviceMetteringIndication == MonthType.NextMonth;
            risEntity.EndDate = (byte)externalEntity.DayEnd;
            risEntity.EndDateNextMonth = externalEntity.EndDeviceMetteringIndication == MonthType.NextMonth;
            risEntity.ContractBaseCode = contractBase?.GisCode;
            risEntity.ContractBaseGuid = contractBase?.GisGuid;
            risEntity.ContractType = this.GetContractType(externalEntity.Id);
            risEntity.PersonType = this.GetPersonType(externalEntity.Id);
            risEntity.PersonTypeOrganization = this.GetPersonTypeOrganization(externalEntity.Id);
            risEntity.JurPerson = this.GetRisContragent(contactJurPerson?.Contragent);
            risEntity.IndSurname = contactIndPerson?.LastName;
            risEntity.IndFirstName = contactIndPerson?.FirstName;
            risEntity.IndPatronymic = contactIndPerson?.MiddleName;
            risEntity.IndSex = this.GetRisSex(contactIndPerson?.Gender);
            risEntity.IndDateOfBirth = contactIndPerson?.BirthDate;
            risEntity.IndIdentityTypeCode = ownerDocumentType?.GisCode;
            risEntity.IndIdentityTypeGuid = ownerDocumentType?.GisGuid;
            risEntity.IndIdentitySeries = contactIndPerson?.DocumentSeries;
            risEntity.IndIdentityNumber = contactIndPerson?.DocumentNumber;
            risEntity.IndIdentityIssueDate = contactIndPerson?.IssueDate;
            risEntity.IndPlaceBirth = contactIndPerson?.BirthPlace;
            risEntity.CommercialMeteringResourceType = this.GetMeteringResourceType(externalEntity.Id);
            risEntity.FiasHouseGuid = externalEntity.RealityObject?.HouseGuid;
            risEntity.ContractNumber = externalEntity.ContractNumber;
            risEntity.SigningDate = externalEntity.ContractDate;
            risEntity.EffectiveDate = externalEntity.DateStart;
            risEntity.BillingDate = (byte)externalEntity.TermBillingPaymentNoLaterThan;
            risEntity.PaymentDate = (byte)externalEntity.TermPaymentNoLaterThan;
            risEntity.ProvidingInformationDate = (byte)externalEntity.DeadlineInformationOfDebt;
            risEntity.TerminateDate = externalEntity.DateStop;
            risEntity.TerminateReasonCode = terminateReason?.GisCode;
            risEntity.TerminateReasonGuid = terminateReason?.GisGuid;
            risEntity.RollOverDate = externalEntity.DateEnd;
        }

    }
}
