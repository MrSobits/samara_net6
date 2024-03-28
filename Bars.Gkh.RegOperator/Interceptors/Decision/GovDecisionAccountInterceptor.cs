namespace Bars.Gkh.RegOperator.Interceptors.Decision
{
    using System;
    using System.Linq;
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Entities.Decisions;

    public class GovDecisionAccountInterceptor : EmptyDomainInterceptor<GovDecision>
    {

        public override IDataResult AfterCreateAction(IDomainService<GovDecision> service, GovDecision entity)
        {
            // Получаем предыдущее решение по дому
            var decision = service.GetAll()
                .Where(x => x.RealityObject.Id == entity.RealityObject.Id)
                .Where(x => x.ProtocolDate <= entity.ProtocolDate)
                .OrderByDescending(x => x.ProtocolDate).FirstOrDefault();

            if (entity.TakeApartsForGov)
            {
                UndoAndClearDeactivation(decision);
                if (!entity.TakeApartsForGovDate.HasValue)
                {
                    return new BaseDataResult(false, "Не указана дата изъятия");
                }
                DeactivateAccounts(entity.RealityObject, entity.TakeApartsForGovDate.Value, entity);
            }

            if (entity.TakeLandForGov)
            {
                if (!entity.TakeLandForGovDate.HasValue)
                {
                    return new BaseDataResult(false, "Не указана дата изъятия");
                }
                UndoAndClearDeactivation(decision);
                DeactivateAccounts(entity.RealityObject, entity.TakeLandForGovDate.Value, entity);
            }

            if (entity.Destroy)
            {
                if (!entity.DestroyDate.HasValue)
                {
                    return new BaseDataResult(false, "Не указана дата сноса");
                }
                UndoAndClearDeactivation(decision);
                DeactivateAccounts(entity.RealityObject, entity.DestroyDate.Value, entity);
            }

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<GovDecision> service, GovDecision entity)
        {
            UndoAndClearDeactivation(entity);

            var coreDecisionDomain = Container.ResolveDomain<CoreDecision>();

            coreDecisionDomain.GetAll()
                .Where(x => x.GovDecision.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => coreDecisionDomain.Delete(x));

            return Success();
        }

        /// <summary>
        /// Перевод статуса счета в неактивно, начиная с unactivationDate
        /// </summary>
        /// <param name="realityObject"></param>
        /// <param name="unactivationDate">Дата, начиная с которой счета становятся неактивными</param>
        /// <param name="entity"></param>
        private void DeactivateAccounts(RealityObject realityObject, DateTime unactivationDate, GovDecision entity)
        {
            var accountDomain = Container.Resolve<IDomainService<BasePersonalAccount>>();
            var stateDomainService = Container.Resolve<IDomainService<State>>();
            var defferdUnactDomain = Container.Resolve<IDomainService<DefferedUnactivation>>();

            using (Container.Using(accountDomain, stateDomainService, defferdUnactDomain))
            {
                var unactiveState =
                    stateDomainService.FirstOrDefault(
                        x => x.TypeId == "gkh_regop_personal_account" && x.Code == "4");

                // Получаем счета дома, по которому принимается решение
                var accounts = accountDomain.GetAll()
                    .Where(x => x.Room.RealityObject.Id == realityObject.Id)
                    .Where(x => x.State.Id != unactiveState.Id);


                foreach (var account in accounts)
                {
                    if (DateTime.Now >= unactivationDate)
                    {
                        account.State = unactiveState;
                        accountDomain.Update(account);
                    }

                    var deffered = new DefferedUnactivation
                    {
                        GovDecision = entity,
                        PersonalAccount = account,
                        UnactivationDate = unactivationDate,
                        Processed = DateTime.Now >= unactivationDate
                    };

                    defferdUnactDomain.Save(deffered);
                }
            }
        }

        /// <summary>
        /// Откат изменений, проделанных предыдущим решением
        /// </summary>
        /// <param name="decision">Решение, для которого будет сделан откат изменений</param>
        private void UndoAndClearDeactivation(GovDecision decision)
        {
            if (decision == null) return;

            var stateDomainService = Container.Resolve<IDomainService<State>>();
            var defferdUnactDomain = Container.Resolve<IDomainService<DefferedUnactivation>>();
            var accountDomain = Container.Resolve<IDomainService<BasePersonalAccount>>();
            using (Container.Using(stateDomainService, defferdUnactDomain, accountDomain))
            {
                var openState =
                    stateDomainService.FirstOrDefault(x => x.TypeId == "gkh_regop_personal_account" && x.Code == "1");

                // Получаем отложенные операции по переводу статуса в неактивный
                var defferds = defferdUnactDomain.GetAll()
                    .Where(x => x.GovDecision.Id == decision.Id)
                    .Where(x => x.Processed);

                foreach (var defferd in defferds)
                {
                    // Если изменения уже поризведены, то откатываем
                    if (defferd.Processed)
                    {
                        var account = defferd.PersonalAccount;
                        account.State = openState;
                        accountDomain.Update(account);
                    }

                    // Удаляем отложенные переводы статуса для предыдущего решения
                    defferdUnactDomain.Delete(defferd.Id);
                }
            }
        }

    }
}
