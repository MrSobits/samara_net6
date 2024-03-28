namespace Bars.Gkh.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Authentification;
    using Entities;
    using Enums;

    /// <summary>Реализация BaseDomainService</summary>
    public class ContragentDomainService : BaseDomainService<Contragent>
    {
        public IDomainService<ContragentMunicipality> ContragentMunicipalityService { get; set; }

        public override IDataResult Update(BaseParams baseParams)
        {
            var saveParam = this.GetSaveParam(baseParams);
            var values = new List<Contragent>();
            foreach (var record in saveParam.Records)
            {
                var oldValue = this.GetAll().Select(x => new
                {
                    x.Id,
                    x.ContragentState,
                    x.DateTermination
                })
                .FirstOrDefault(x => x.Id == record.Entity.Id);

                var entity = record.AsObject();
                
                this.Update(entity);

                // Обновляем участников процесса
                // если изменились значения о прекращении деятельности
                if (oldValue != null && (entity.ContragentState != oldValue.ContragentState ||
                                         entity.DateTermination != oldValue.DateTermination))
                {
                    this.UpdateMemberProcess(entity);
                }

                values.Add(entity);
            }

            return new BaseDataResult(values);
        }

        /// <summary>
        /// метод обновления Участников процесса
        /// </summary>
        private void UpdateMemberProcess(Contragent entity)
        {
            // Если изменена информация о прекращении деятельности
            // то необходимо проапдейтить всех участников процесса по данному контрагенту и также внести вних изменения

            // После сохранения смотрим статус контрагента взависимости от него проставляем в прекращение деятельности 
            var serviceServiceOrg = this.Container.Resolve<IDomainService<ServiceOrganization>>();
            var serviceOrgList =
                serviceServiceOrg.GetAll().Where(x => x.Contragent.Id == entity.Id).ToList();

            foreach (var serviceOrg in serviceOrgList)
            {
                switch (entity.ContragentState)
                {
                    case ContragentState.Bankrupt:
                        serviceOrg.ActivityGroundsTermination = GroundsTermination.Bankruptcy;
                        break;
                    case ContragentState.Liquidated:
                        serviceOrg.ActivityGroundsTermination = GroundsTermination.Liquidation;
                        break;
                    case ContragentState.Active:
                        serviceOrg.ActivityGroundsTermination = GroundsTermination.NotSet;
                        break;
                    case ContragentState.NotManagementService:
                        serviceOrg.ActivityGroundsTermination = GroundsTermination.TerminationMkd;
                        break;
                }

                serviceOrg.DateTermination = entity.DateTermination;
                serviceServiceOrg.Update(serviceOrg);
            }

            var serviceBelayOrg = this.Container.Resolve<IDomainService<BelayOrganization>>();
            var belayOrgList = serviceBelayOrg.GetAll().Where(x => x.Contragent.Id == entity.Id).ToList();

            foreach (var belayOrg in belayOrgList)
            {
                switch (entity.ContragentState)
                {
                    case ContragentState.Bankrupt:
                        belayOrg.ActivityGroundsTermination = GroundsTermination.Bankruptcy;
                        break;
                    case ContragentState.Liquidated:
                        belayOrg.ActivityGroundsTermination = GroundsTermination.Liquidation;
                        break;
                    case ContragentState.Active:
                        belayOrg.ActivityGroundsTermination = GroundsTermination.NotSet;
                        break;
                    case ContragentState.NotManagementService:
                        belayOrg.ActivityGroundsTermination = GroundsTermination.TerminationMkd;
                        break;
                }

                belayOrg.DateTermination = entity.DateTermination;
                serviceBelayOrg.Update(belayOrg);
            }

            var serviceManOrg = this.Container.Resolve<IDomainService<ManagingOrganization>>();
            var manOrgList = serviceManOrg.GetAll().Where(x => x.Contragent.Id == entity.Id).ToList();

            foreach (var manOrg in manOrgList)
            {
                switch (entity.ContragentState) 
                {
                    case ContragentState.Bankrupt:
                        manOrg.ActivityGroundsTermination = GroundsTermination.Bankruptcy;
                        break;
                    case ContragentState.Liquidated:
                        manOrg.ActivityGroundsTermination = GroundsTermination.Liquidation;
                        break;
                    case ContragentState.Active:
                        manOrg.ActivityGroundsTermination = GroundsTermination.NotSet;
                        break;
                    case ContragentState.NotManagementService:
                        manOrg.ActivityGroundsTermination = GroundsTermination.TerminationMkd;
                        break;
                }

                manOrg.ActivityDateEnd = entity.DateTermination;
                serviceManOrg.Update(manOrg);
            }

            var serviceBuilder = this.Container.Resolve<IDomainService<Builder>>();
            var builderList = serviceBuilder.GetAll().Where(x => x.Contragent.Id == entity.Id).ToList();

            foreach (var builder in builderList)
            {
                switch (entity.ContragentState)
                {
                    case ContragentState.Bankrupt:
                        builder.ActivityGroundsTermination = GroundsTermination.Bankruptcy;
                        break;
                    case ContragentState.Liquidated:
                        builder.ActivityGroundsTermination = GroundsTermination.Liquidation;
                        break;
                    case ContragentState.Active:
                        builder.ActivityGroundsTermination = GroundsTermination.NotSet;
                        break;
                    case ContragentState.NotManagementService:
                        builder.ActivityGroundsTermination = GroundsTermination.TerminationMkd;
                        break;
                }

                builder.ActivityDateEnd = entity.DateTermination;
                serviceBuilder.Update(builder);
            }

            var serviceSupplyResourceOrg = this.Container.Resolve<IDomainService<SupplyResourceOrg>>();
            var supplyResourceOrgList = serviceSupplyResourceOrg.GetAll().Where(x => x.Contragent.Id == entity.Id).ToList();

            foreach (var supplyResourceOrg in supplyResourceOrgList)
            {
                switch (entity.ContragentState)
                {
                    case ContragentState.Bankrupt:
                        supplyResourceOrg.ActivityGroundsTermination = GroundsTermination.Bankruptcy;
                        break;
                    case ContragentState.Liquidated:
                        supplyResourceOrg.ActivityGroundsTermination = GroundsTermination.Liquidation;
                        break;
                    case ContragentState.Active:
                        supplyResourceOrg.ActivityGroundsTermination = GroundsTermination.NotSet;
                        break;
                    case ContragentState.NotManagementService:
                        supplyResourceOrg.ActivityGroundsTermination = GroundsTermination.TerminationMkd;
                        break;
                }

                supplyResourceOrg.DateTermination = entity.DateTermination;
                serviceSupplyResourceOrg.Update(supplyResourceOrg);
            }
        }

        public override IQueryable<Contragent> GetAll()
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            var municipalityList = userManager.GetMunicipalityIds();
            var contragentList = userManager.GetContragentIds();

            return base.GetAll()
                .WhereIf(municipalityList.Count > 0,
                    x => municipalityList.Contains(x.Municipality.Id) || ContragentMunicipalityService.GetAll()
                        .Any(y => municipalityList.Contains(y.Municipality.Id) && y.Contragent.Id == x.Id))
                .WhereIf(contragentList.Count > 0, x => contragentList.Contains(x.Id));
        }
    }
}