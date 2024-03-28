namespace Bars.Gkh.RegOperator.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.Domain.ParameterVersioning;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.DomainService.RegionalFormingOfCr;
    using Bars.Gkh.RegOperator.DomainEvent;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountDto;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Core.Internal;
    using Domain.Repository;
    using Entities.PersonalAccount;
    using Enums;
    using Gkh.Domain;

    /// <summary>
    /// Интерсептор дома
    /// </summary>
    public class RealityObjectInterceptor : EmptyDomainInterceptor<RealityObject>
    {
        public IRealityObjectAccountGenerator Generator { get; set; }
        public IRealtyObjectRegopOperationService RealtyObjectRegopOperationService { get; set; }
        public IChargePeriodRepository ChargePeriodRepo { get; set; }
        public ISpecialCalcAccountService SpecialCalcAccountService { get; set; }
        public IStateProvider StateProvider { get; set; }
        public IRepository<State> StateRepo { get; set; } 
        public IDomainService<RealityObjectDecisionProtocol> RealityObjectDecisionProtocolDomain { get; set; }
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }
        public IDomainService<PersonalAccountChange> PersonalAccountChangeDomain { get; set; }
        public IPersonalAccountOperationService PersAccOperationService { get; set; }
        public IGkhUserManager UserManager { get; set; }
        public IHouseTypesConfigService GetOverhaulHmaoConfig { get; set; }
        public IPersonalAccountService PersonalAccountService { get; set; }
        public ITypeOfFormingCrProvider FormingCrProvider { get; set; }

        public IRealityObjectPersonalAccountStateProvider RealityObjectPersonalAccountStateProvider { get; set; }

        public override IDataResult AfterUpdateAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            if (entity.ConditionHouse == ConditionHouse.Emergency || entity.ConditionHouse == ConditionHouse.Razed)
            {
                var dateActual = DateTime.Now;
                if (VersionedEntityChangeContext.Current != null && VersionedEntityChangeContext.Current.ClassName.ToLower().Equals("realityobject"))
                {
                    dateActual = VersionedEntityChangeContext.Current.FactDate;
                }

                this.PersAccOperationService.MassClosingAccounts(
                    new[] { entity.Id },
                    PersonalAccountChangeType.Close,
                    dateActual,
                    () => "Закрытие ЛС в связи со сменой состояния дома");
            }
            else
            {
                this.RealityObjectPersonalAccountStateProvider.SetPersonalAccountStateIfNeed(entity);
            }
            
            // актуализация DTO
            DomainEvents.Raise(new RealityObjectForDtoChangeEvent(entity));
            return base.AfterUpdateAction(service, entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            this.Generator.GenerateChargeAccount(entity);
            this.Generator.GeneratePaymentAccount(entity);
            this.Generator.GenerateSupplierAccount(entity);
            this.Generator.GenerateSubsidyAccount(entity);

            var period = this.ChargePeriodRepo.GetCurrentPeriod();

            if (period != null)
            {
                this.RealtyObjectRegopOperationService.CreatePersonalAccountSummaries(period, entity);
            }

            return base.AfterCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            var realityObjectChargeAccountDomain = this.Container.Resolve<IDomainService<RealityObjectChargeAccount>>();
            var realityObjectPaymentAccountDomain = this.Container.Resolve<IDomainService<RealityObjectPaymentAccount>>();
            var realityObjectSupplierAccountDomain = this.Container.Resolve<IDomainService<RealityObjectSupplierAccount>>();
            var realityObjectSubsidyAccountDomain = this.Container.Resolve<IDomainService<RealityObjectSubsidyAccount>>();

            try
            {
                realityObjectChargeAccountDomain.GetAll().Where(x => x.RealityObject == entity)
                    .Select(x => x.Id)
                    .ForEach(x => realityObjectChargeAccountDomain.Delete(x));

                realityObjectPaymentAccountDomain.GetAll().Where(x => x.RealityObject == entity)
                    .Select(x => x.Id)
                    .ForEach(x => realityObjectPaymentAccountDomain.Delete(x));

                realityObjectSupplierAccountDomain.GetAll().Where(x => x.RealityObject == entity)
                    .Select(x => x.Id)
                    .ForEach(x => realityObjectSupplierAccountDomain.Delete(x));

                realityObjectSubsidyAccountDomain.GetAll().Where(x => x.RealityObject == entity)
                    .Select(x => x.Id)
                    .ForEach(x => realityObjectSubsidyAccountDomain.Delete(x));
            }
            catch (ValidationException exc)
            {
                return this.Failure(exc.Message);
            }
            finally
            {
                this.Container.Release(realityObjectChargeAccountDomain);
                this.Container.Release(realityObjectPaymentAccountDomain);
                this.Container.Release(realityObjectSupplierAccountDomain);
                this.Container.Release(realityObjectSubsidyAccountDomain);
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}