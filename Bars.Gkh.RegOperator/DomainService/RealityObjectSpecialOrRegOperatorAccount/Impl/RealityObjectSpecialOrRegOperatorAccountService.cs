using Bars.Gkh.RegOperator.Enums;

namespace Bars.Gkh.RegOperator.DomainService.RealityObjectSpecialOrRegOperatorAccount.Impl
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.States;
    using B4.Utils;
    using Castle.Windsor;
    using Decisions.Nso.Domain;
    using Decisions.Nso.Entities;
    using Entities;
    using Gkh.Domain;
    using Gkh.Entities;
    using Gkh.Enums.Decisions;

    public class RealityObjectSpecialOrRegOperatorAccountService : IRealityObjectSpecialOrRegOperatorAccountService
    {
        private readonly IWindsorContainer _container;
        private readonly IDomainService<RealityObjectSpecialOrRegOperatorAccount> _accDomain;

        public RealityObjectSpecialOrRegOperatorAccountService(IWindsorContainer container, IDomainService<RealityObjectSpecialOrRegOperatorAccount> accDomain)
        {
            _container = container;
            _accDomain = accDomain;
        }

        public void HandleSpecialAccountByProtocolChange(AccountOwnerDecision ownerDecision, CrFundFormationDecision crFundFormDecision, RealityObject ro)
        {
            var protocolDomain = _container.ResolveDomain<RealityObjectDecisionProtocol>();
            using (_container.Using(protocolDomain))
            {
                if (ownerDecision != null
                    && ownerDecision.Protocol != null
                    && protocolDomain.GetAll()
                        .Where(x => x.RealityObject == ownerDecision.Protocol.RealityObject)
                        .Where(x => x.ProtocolDate > ownerDecision.Protocol.ProtocolDate)
                        .Any(x => x.State.FinalState))
                {
                    return;
                }

                if (crFundFormDecision.Decision == CrFundFormationDecisionType.RegOpAccount)
                {
                    if (protocolDomain.GetAll()
                        .Where(x => x.RealityObject == crFundFormDecision.Protocol.RealityObject)
                        .Where(x => x.ProtocolDate > crFundFormDecision.Protocol.ProtocolDate)
                        .Any(x => x.State.FinalState))
                    {
                        return;
                    }
                }
            }

            _container.InTransaction(() =>
            {
                if (crFundFormDecision.Decision == CrFundFormationDecisionType.SpecialAccount)
                {
                    HandleSpecialAccountDecision(ro, ownerDecision);
                }
                else if (crFundFormDecision.Decision == CrFundFormationDecisionType.RegOpAccount)
                {
                    HandleRegOperatorDecision(ro);
                }
            });
        }

        public void SetNonActiveByRealityObject(RealityObject ro)
        {
            _container.UsingForResolved<IDataTransaction>((c, tr) =>
            {
                try
                {
                    var account =
                        _accDomain.GetAll().FirstOrDefault(x => x.RealityObjectChargeAccount.RealityObject.Id == ro.Id);

                    if (account != null)
                    {
                        account.IsActive = false;
                        _accDomain.Update(account);
                    }

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            });
        }

        public void SetPersonalAccountStatesActiveIfAble(RealityObjectDecisionProtocol protocol, bool deletingCurrentProtocol = false)
        {
            if (AnySuitableCrFundDecision(protocol, deletingCurrentProtocol))
            {
                return;
            }

            var persAccRepo = _container.ResolveRepository<BasePersonalAccount>();
            var stateProvider = _container.Resolve<IStateProvider>();
            var stateRepo = _container.Resolve<IRepository<State>>();
            var tr = _container.Resolve<IDataTransaction>();

            using (_container.Using(persAccRepo, stateProvider, tr, stateRepo))
            {
                var accOwnerDec = GetDecisionByProtocol<AccountOwnerDecision>(protocol);
                var crFundDec = GetDecisionByProtocol<CrFundFormationDecision>(protocol); 

                if (crFundDec == null)
                {
                    return;
                }

                if (crFundDec.Decision == CrFundFormationDecisionType.SpecialAccount && accOwnerDec == null)
                {
                    return;
                }

                /*
                 * Если (решение  о спец счете И владаелец спец счета  регоп)
                 * ИЛИ (если решение о счете регионального оператора)
                 * то ЛС -> статус Открыт, иначе в не активно 
                 */

                bool toActive = (crFundDec.Decision == CrFundFormationDecisionType.SpecialAccount && accOwnerDec.DecisionType == AccountOwnerDecisionType.RegOp)
                                  || crFundDec.Decision == CrFundFormationDecisionType.RegOpAccount;


                var typeId = "gkh_regop_personal_account";

                var state = stateRepo.GetAll().FirstOrDefault(x => x.Code == (toActive ? "1" : "4")
                                                                   && x.TypeId == typeId);

                if (state == null)
                {
                    return;
                }

                using (tr)
                {
                    try
                    {
                        persAccRepo.GetAll()
                            .Where(x => x.Room.RealityObject == protocol.RealityObject)
                            .ToList()
                            .ForEach(x =>
                            {
                                if (x.State.Code == (toActive ? "4" : "1"))
                                {
                                    stateProvider.ChangeState(x.Id, typeId, state);
                                    persAccRepo.Update(x);
                                }
                            });

                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }

        public void SetPersonalAccountStatesNonActiveIfNeeded(RealityObjectDecisionProtocol protocol, bool deletingCurrentProtocol = false)
        {
            if (AnySuitableCrFundDecision(protocol, deletingCurrentProtocol))
            {
                return;
            }

            var decisionService = _container.Resolve<IRealityObjectDecisionsService>();
            var persAccDomain = _container.ResolveDomain<BasePersonalAccount>();
            var stateProvider = _container.Resolve<IStateProvider>();
            var stateRepo = _container.Resolve<IRepository<State>>();
            var tr = _container.Resolve<IDataTransaction>();

            using (_container.Using(decisionService, persAccDomain, stateProvider, tr, stateRepo))
            {
                var accOwnerDec = decisionService.GetActualDecision<AccountOwnerDecision>(protocol.RealityObject, true, new[] { protocol });
                var crFundDec = decisionService.GetActualDecision<CrFundFormationDecision>(protocol.RealityObject, true, new[] { protocol });

                var needToChangeStates = (accOwnerDec == null || crFundDec == null);

                /*
                 * Если решение не о спец счете
                 * или владаелец спец счета не регоп - установим конечный статус
                 */
                if (needToChangeStates
                        || crFundDec.Decision != CrFundFormationDecisionType.SpecialAccount
                        || accOwnerDec.DecisionType != AccountOwnerDecisionType.RegOp)
                {
                    using (tr)
                    {
                        try
                        {
                            var type = stateProvider.GetStatefulEntityInfo(typeof(BasePersonalAccount));

                            if (type == null)
                            {
                                return;
                            }

                            var state = stateRepo.GetAll().FirstOrDefault(x => x.Code == "4" && x.TypeId == type.TypeId);

                            persAccDomain.GetAll()
                                .Where(x => x.Room.RealityObject == protocol.RealityObject)
                                .ToList()
                                .ForEach(x =>
                                {
                                    if (x.State.Code == "1")
                                    {
                                        stateProvider.ChangeState(x.Id, type.TypeId, state);
                                        persAccDomain.Update(x);
                                    }
                                });

                            tr.Commit();
                        }
                        catch
                        {
                            tr.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        public void AddPaymentForOpeningAccount(RealityObjectDecisionProtocol protocol)
        {
            var accOwnerDec = GetDecisionByProtocol<AccountOwnerDecision>(protocol);
            var crFundDec = GetDecisionByProtocol<CrFundFormationDecision>(protocol);

            if (crFundDec == null || accOwnerDec == null || crFundDec.Decision != CrFundFormationDecisionType.SpecialAccount || accOwnerDec.DecisionType != AccountOwnerDecisionType.RegOp)
            {
                return;
            }

            if (AnySuitableCrFundDecision(protocol))
            {
                return;
            }

            var roAccDomain = _container.ResolveDomain<RealityObjectPaymentAccount>();
            var creditOrgDecisionDomain = _container.ResolveDomain<CreditOrgDecision>();
            var creditOrgServCondDomain = _container.ResolveDomain<CreditOrgServiceCondition>();
            var roAccOperationDomain = _container.ResolveDomain<RealityObjectPaymentAccountOperation>();
            var regopParamDomain = _container.ResolveDomain<RegoperatorParam>();

            try
            {
                var isPayForOpenAcc = regopParamDomain.GetAll().FirstOrDefault(x => x.Key == "OpenAccCredit").Return(x => x.Value).ToBool();

                if (isPayForOpenAcc)
                {
                    var realObjPaymAcc =
                        roAccDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == protocol.RealityObject.Id);
                    var creditOrgId =
                        creditOrgDecisionDomain.GetAll()
                            .FirstOrDefault(x => x.Protocol.Id == protocol.Id)
                            .Return(x => x.Decision.Id);
                    var openingAccPay = creditOrgServCondDomain.GetAll()
                        .Where(x => x.OpeningAccDateFrom <= protocol.ProtocolDate
                                    && (!x.OpeningAccDateTo.HasValue || x.OpeningAccDateTo >= protocol.ProtocolDate))
                        .FirstOrDefault(x => x.CreditOrg.Id == creditOrgId)
                        .Return(x => x.OpeningAccPay);

                    var accOpenAccOperation = new RealityObjectPaymentAccountOperation
                    {
                        Date = DateTime.Now,
                        Account = realObjPaymAcc,
                        OperationSum = openingAccPay,
                        OperationType = PaymentOperationType.OpeningAcc,
                        OperationStatus = OperationStatus.Default
                    };

                    roAccOperationDomain.Save(accOpenAccOperation);
                }
            }
            finally
            {
                _container.Release(roAccDomain);
                _container.Release(creditOrgDecisionDomain);
                _container.Release(creditOrgServCondDomain);
                _container.Release(roAccOperationDomain);
            }
        }


        #region private methods

        private T GetDecisionByProtocol<T>(RealityObjectDecisionProtocol protocol)
            where T : UltimateDecision
        {
            var domain = _container.ResolveDomain<T>();
            using (_container.Using(domain))
            {
                return domain.GetAll().FirstOrDefault(x => x.Protocol == protocol);
            }
        }

        /// <summary>
        /// Функция проверяет, если кроме текщуего проверяемого протокола наиболее "свежий" протокол на спец счете
        /// </summary>
        /// <param name="protocol">Текущий проверяемый протокол</param>
        /// <param name="deletingCurrentProtocol">Текущий протокол удаляется. В таком случае его дата протокола не учитывается в фильтре</param>
        private bool AnySuitableCrFundDecision(RealityObjectDecisionProtocol protocol,
            bool deletingCurrentProtocol = false)
        {
            var crFundFormDomain = _container.ResolveDomain<CrFundFormationDecision>();

            /*
             * Если есть решение о выборе спец счета такое, что
             * 1) Статус протокола конечный
             * 2) Дата протокола выше, чем дата нового протокола
             * то ничего не делаем
             */
            using (_container.Using(crFundFormDomain))
            {
                if (crFundFormDomain.GetAll()
                    .Where(x => x.Protocol.RealityObject == protocol.RealityObject)
                    .WhereIf(!deletingCurrentProtocol, x => x.Protocol.ProtocolDate > protocol.ProtocolDate)
                    .Any(x => x.Protocol != protocol
                              && x.Protocol.State.FinalState
                              && x.Decision == CrFundFormationDecisionType.SpecialAccount))
                {
                    return true;
                }
            }
            return false;
        }

        private void HandleRegOperatorDecision(RealityObject ro)
        {
            var accountDomain = _container.ResolveDomain<RealityObjectSpecialOrRegOperatorAccount>();
            var account =
                accountDomain.GetAll()
                    .FirstOrDefault(x => x.RealityObjectChargeAccount.RealityObject == ro);

            ChooseRegOp(ro, account);
        }

        private void HandleSpecialAccountDecision(RealityObject ro, AccountOwnerDecision ownerDecision)
        {
            var accountDomain = _container.ResolveDomain<RealityObjectSpecialOrRegOperatorAccount>();
            var account =
                accountDomain.GetAll()
                    .FirstOrDefault(x => x.RealityObjectChargeAccount.RealityObject == ro);

            using (_container.Using(accountDomain))
            {
                if (ownerDecision == null)
                {
                    SetNonActive(account, accountDomain);
                }
                else
                {
                    var chooseRegop = ownerDecision.DecisionType == AccountOwnerDecisionType.RegOp;
                    ChooseContragent(ro, account, chooseRegop);
                }
            }
        }

        private void ChooseRegOp(RealityObject ro, RealityObjectSpecialOrRegOperatorAccount account)
        {
            var domain = _container.ResolveDomain<RealityObjectChargeAccount>();

            using (_container.Using(domain))
            {
                var chargeAccount =
                    domain.GetAll()
                        .FirstOrDefault(x => x.RealityObject == ro);
                
                if (chargeAccount == null)
                {
                    throw new Exception("Не найден Счет начислений дома");
                }

                account = account ?? new RealityObjectSpecialOrRegOperatorAccount
                {
                    RealityObjectChargeAccount = chargeAccount
                };

                account.IsActive = true;
                account.RegOperator = GetRegOperator();
                account.Contragent = null;
                account.AccountType = CrFundFormationDecisionType.RegOpAccount;

                SaveOrUpdate(account);
            }
        }

        private void ChooseContragent(RealityObject ro,
            RealityObjectSpecialOrRegOperatorAccount account,
            bool chooseRegop)
        {
            var domain = _container.ResolveDomain<RealityObjectChargeAccount>();

            using (_container.Using(domain))
            {
                var chargeAccount =
                    domain
                        .GetAll()
                        .FirstOrDefault(x => x.RealityObject == ro);

                if (chargeAccount == null)
                {
                    throw new Exception("Не найден Счет начислений дома");
                }

                account = account ?? new RealityObjectSpecialOrRegOperatorAccount
                {
                    RealityObjectChargeAccount = chargeAccount
                };

                account.IsActive = true;
                account.AccountType = CrFundFormationDecisionType.SpecialAccount;

                if (chooseRegop)
                {
                    account.RegOperator = GetRegOperator();

                    account.Contragent = null;
                }
                else
                {
                    var contract = GetCurrentManOrgContract(ro);
                    account.Contragent =
                        contract.Return(x => x.ManOrgContract)
                            .Return(x => x.ManagingOrganization)
                            .Return(x => x.Contragent);

                    account.RegOperator = null;
                }

                SaveOrUpdate(account);
            }
        }

        private string GetRegOperator()
        {
            return "РегОператор";
        }

        private void SaveOrUpdate<T>(T entity)
            where T : PersistentObject
        {
            var domain = _container.ResolveDomain<T>();

            using (_container.Using(domain))
            {
                if (entity.Id > 0)
                {
                    domain.Update(entity);
                }
                else
                {
                    domain.Save(entity);
                }
            }
        }

        private void SetNonActive(RealityObjectSpecialOrRegOperatorAccount account,
            IDomainService<RealityObjectSpecialOrRegOperatorAccount> accountDomain)
        {
            if (account != null)
            {
                account.IsActive = false;
                accountDomain.Update(account);
            }
        }

        private ManOrgContractRealityObject GetCurrentManOrgContract(RealityObject ro)
        {
            var domain = _container.ResolveDomain<ManOrgContractRealityObject>();

            using (_container.Using(domain))
            {
                var contract = domain.GetAll()
                    .Where(x => x.RealityObject.Id == ro.Id)
                    .Where(
                        x =>
                            x.ManOrgContract.StartDate <= DateTime.Now
                            && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= DateTime.Now))
                    .OrderByDescending(x => x.ManOrgContract.StartDate)
                    .FirstOrDefault();
                return contract;
            }
        }

        #endregion
    }
}