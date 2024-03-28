namespace Bars.Gkh.RegOperator.Interceptors.Contragent
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;

    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    using Gkh.Entities;
    using DomainEvent;
    using DomainEvent.Events.PersonalAccountDto;
    using Entities;

    /// <summary>
    /// Интерцептор <see cref="Contragent"/>
    /// </summary>
    public class RegOperatorContragentServiceInterceptor : EmptyDomainInterceptor<Contragent>
    {
        public IDomainService<RegOperator> RegOperatorDomain { get; set; }
        public IDomainService<LegalAccountOwner> LegalAccountOwnerDomain { get; set; }
        public IDomainService<RealityObjectSpecialOrRegOperatorAccount> RealityObjectSpecialOrRegOperatorAccountDomain { get; set; }
        public IDomainService<CalcAccount> CalcAccountDomain { get; set; }
        public IDomainService<SpecialCalcAccount> SpecialCalcAccountDomain { get; set; }
        public IDomainService<RegopCalcAccount> RegopCalcAccountDomain { get; set; }
        public IDomainService<DeliveryAgent> DeliveryAgentDomain { get; set; }

        /// <summary>
        /// Действие, выполняемое до обновления сущности <see cref="Contragent"/>
        /// </summary>
        /// <param name="service">Домен-сервис "Контрагент"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult AfterUpdateAction(IDomainService<Contragent> service, Contragent entity)
        {
            var legalAccountOwnerService = this.Container.Resolve<IDomainService<LegalAccountOwner>>();

            try
            {
                var owners = legalAccountOwnerService
                    .GetAll()
                    .Where(x => x.Contragent.Id == entity.Id)
                    .ToArray();

                foreach (var owner in owners)
                {
                    DomainEvents.Raise(new PersonalAccountOwnerUpdateEvent(owner));
                }
            }
            finally
            {
                this.Container.Release(legalAccountOwnerService);
            }
            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<Contragent> service, Contragent entity)
        {
            var dependencyList = new List<string>();

            if (this.RegOperatorDomain.GetAll().Any(x => x.Contragent.Id == entity.Id))
            {
                dependencyList.Add("Региональный оператор");
            }
            if (this.LegalAccountOwnerDomain.GetAll().Any(x => x.Contragent.Id == entity.Id))
            {
                dependencyList.Add("Абонент - ЮЛ");
            }
            if (this.CalcAccountDomain.GetAll().Any(x => x.AccountOwner.Id == entity.Id))
            {
                dependencyList.Add("Расчетный счет");
            }
            if (this.SpecialCalcAccountDomain.GetAll().Any(x => x.AccountOwner.Id == entity.Id))
            {
                dependencyList.Add("Специальный расчетный счет");
            }
            if (this.RegopCalcAccountDomain.GetAll().Any(x => x.AccountOwner.Id == entity.Id))
            {
                dependencyList.Add("Расчетный счет регоператора");
            }
            if (this.DeliveryAgentDomain.GetAll().Any(x => x.Contragent.Id == entity.Id))
            {
                dependencyList.Add("Агент доставки");
            }

            if (dependencyList.Any())
            {
                return this.Failure(
                    $"При удалении данной записи произойдет удаление следующих связанных объектов: {string.Join(", ", dependencyList)}. Удаление невозможно.");
            }

            return this.Success();
        }
    }
}


