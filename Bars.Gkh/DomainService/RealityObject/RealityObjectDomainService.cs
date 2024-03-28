namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Домен сервис для фильтрация по оператору и его МО
    /// </summary>
    public class RealityObjectDomainService : FileStorageDomainService<RealityObject>
    {
        /// <summary>
        /// DomainService Жилой дом договора управляющей организации
        /// </summary>
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRoDomain { get; set; }

        /// <summary>
        /// DomainService Органы местного самоуправления
        /// </summary>
        public IDomainService<LocalGovernment> LocalGovernmentDomain { get; set; }

        /// <summary>
        /// DomainService Региональный оператор
        /// </summary>
        public IDomainService<RegOperator> RegOperatorDomain { get; set; }

        /// <summary>
        /// DomainService Ограны госудраствненной власти
        /// </summary>
        public IDomainService<PoliticAuthority> PoliticAuthorityDomain { get; set; }
        
        /// <summary>
        /// DomainService Расчетно-кассовые центры
        /// </summary>
        public IDomainService<CashPaymentCenter> CashPaymentCenterDomain { get; set; }
        
        /// <summary>
        /// DomainService Управляющие организации
        /// </summary>
        public IDomainService<ManagingOrganization> ManOrgDomainService { get; set; }

        /// <summary>
        /// Менеджер пользователя
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// DomainService Фильтрация по оператору и его МО
        /// </summary>
        /// <returns></returns>
        public override IQueryable<RealityObject> GetAll()
        {
            var municipalityIds = this.UserManager.GetMunicipalityIds();
            var contragentIds = this.UserManager.GetContragentIds();
            
            var hasContragent = contragentIds.Any();
            var hasMunicipality = municipalityIds.Any();

            var rObjectsList = new List<long>();

            //Если в поле "Организации" содержится запись из реестра "Органы местного самоуправления", фильтровать только по МО.
            var isLocalGovernment = false;
            //Если контрагент "Региональные операторы" (Участники процесса / Роли контрагента), фильтровать только по МО.
            var isRegOperator = false;
            //Если в поле "Организации" содержится запись из реестра "Органы госудраственной власти", фильтровать только по МО.
            var isPolAuthOrCashPayment = false;
            //Если в поле "Организации" содержится запись из реестра "Расчетно-кассовые центры", фильтровать только по МО.
            var isCashPayment = false;
            //Если в поле "Организация" содержится запись из реестра "Управляющая организация", фильтровать только по МО
            var isManOrganization = false;
            
            if (hasContragent)
            {
                isManOrganization = this.ManOrgDomainService.GetAll()
                    .Select(x => x.Contragent.Id)
                    .Any(x => contragentIds.Contains(x));
                
                isLocalGovernment = this.LocalGovernmentDomain.GetAll()
                    .Select(x => x.Contragent.Id)
                    .Any(x => contragentIds.Contains(x));
                
                isRegOperator = this.RegOperatorDomain.GetAll()
                    .Select(x => x.Contragent.Id)
                    .Any(x => contragentIds.Contains(x));
                
                isPolAuthOrCashPayment = this.PoliticAuthorityDomain.GetAll()
                    .Select(x => x.Contragent.Id)
                    .Any(x => contragentIds.Contains(x));
                
                isCashPayment = this.CashPaymentCenterDomain.GetAll()
                    .Select(x => x.Contragent.Id)
                    .Any(x => contragentIds.Contains(x));

                rObjectsList = this.ManOrgContractRoDomain.GetAll()
                    .Where(x => contragentIds.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id))
                    .Where(x => x.ManOrgContract.StartDate <= DateTime.Today
                        && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= DateTime.Today))
                    .Select(x => x.RealityObject.Id)
                    .ToList();
            }

            var existsInRegisters = isLocalGovernment || isRegOperator || isPolAuthOrCashPayment || isCashPayment;
            
            // Накладываем по очереди фильтры
            // ЕСЛИ привязан МО, то фильтруем по МО
            // ЕСЛИ привязан контрагент И (контрагент не входит в реестры [РегОп, ОГВ, РКЦ, ОМС] ИЛИ есть в реестре УО), то фильтурем по договорам
            // ИНАЧЕ выводим все дома
            return base.GetAll()
               .WhereIf(hasMunicipality,
                   x => municipalityIds.Contains(x.Municipality.Id) ||
                       (x.Municipality.ParentMo != null && municipalityIds.Contains(x.Municipality.ParentMo.Id)) ||
                       municipalityIds.Contains(x.MoSettlement.Id))
                .WhereIf(rObjectsList.Any() && (!existsInRegisters || isManOrganization),
                    x => rObjectsList.Contains(x.Id));
        }
    }
}