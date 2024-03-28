namespace Bars.Gkh.RegOperator.Tasks.Charges.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Tasks.Charges.Callbacks;
    using Bars.Gkh.RegOperator.Tasks.Charges.Executors;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Fasterflect;

    using Fasterflect;

    /// <summary>
    /// Провайдер задачи расчета начислений по ЛС
    /// </summary>
    public class PersonalAccountChargeTaskProvider : ITaskProvider
    {
        private readonly IWindsorContainer container;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        public PersonalAccountChargeTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        #region Implementation of ITaskProvider

        /// <summary>
        /// Создать задачи
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Дескрипторы задачи</returns>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var operationLock = this.container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.OperationLock;
            baseParams.Params.SetValue("operationLock", operationLock.ToDto());
            var tasks = new CreateTasksResult(this.CreateDescriptors(baseParams));

            if (operationLock.Enabled)
            {
                PersonalAccountChargeTableLocker.Lock(this.container);
            }

            return tasks;
        }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string TaskCode => "PersonalAccountChargeCreation";

        private TaskDescriptor[] CreateDescriptors(BaseParams baseParams)
        {
            var accountDomain = this.container.ResolveDomain<BasePersonalAccount>();
            var filterService = this.container.Resolve<IPersonalAccountFilterService>();
            var periodRepo = this.container.Resolve<IChargePeriodRepository>();
            var packetDomain = this.container.ResolveDomain<UnacceptedChargePacket>();
            var personalAccountService = this.container.Resolve<IPersonalAccountService>();
            var userManager = this.container.Resolve<IGkhUserManager>();

            var simpleCalculatePenalty =
                this.container.GetGkhConfig<RegOperatorConfig>().PaymentPenaltiesNodeConfig.PenaltyCalcConfig.SimpleCalculatePenalty;
            var calcIsNotActive =
                this.container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.DisplayAfterCalculation.CalcIsNotActive;

            using (this.container.Using(accountDomain, filterService, packetDomain, periodRepo, personalAccountService, userManager))
            {
                var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();
                var period = periodRepo.GetCurrentPeriod();

                if (period == null)
                {
                    return new TaskDescriptor[0];
                }

                this.SpecialAccCredit(period);

                if (simpleCalculatePenalty && this.CheckPenaltyTrace(ids, period))
                {
                    throw new Exception("Расчет пени с отсрочкой для выбранных лицевых счетов был произведен ранее.Повторная операция невозможна");
                }

                var loadParams = baseParams.GetLoadParam();
                var addressFilteredIds = personalAccountService.GetAccountIdsByAddress(loadParams);

                // проставляем уникальный guid расчётов
                var calcGuid = Guid.NewGuid().ToString();
                baseParams.Params.SetValue("calcGuid", calcGuid);

                var idsToCalculate = accountDomain.GetAll()
                    .ToDto()
                    .WhereIf(ids != null && ids.Length > 0, x => ids.Contains(x.Id))
                    .WhereIfContains(addressFilteredIds != null, x => x.Id, addressFilteredIds)
                    .FilterByBaseParamsIf(ids?.Length==0 ,baseParams, filterService)
                    .FilterCalculable(period, filterService)
                    .FilterByRegFondSetting(filterService)
                    .Filter(loadParams, this.container)
                    .Select(x => x.Id)
                    .ToArray();

                var packet = new UnacceptedChargePacket
                {
                    CreateDate = DateTime.Now,
                    Description = null,
                    PacketState = PaymentOrChargePacketState.Pending,
                    UserName = userManager.GetActiveUser()?.Login
                };

                if (calcIsNotActive)
                {
                    packet.Description = $"Начисление по {idsToCalculate.Length} лицевым счетам за период {period.Name} для сверки с ЧЭС";
                }

                packetDomain.Save(packet);

                const int take = 5000;
                var done = 0;
                var descrs = new List<TaskDescriptor>();
                while (done < idsToCalculate.Length)
                {
                    var portion = idsToCalculate.Skip(done).Take(take).ToArray();

                    descrs.Add(this.CreateDescriptor(baseParams, portion, done, period, packet));

                    done += take;
                }

                return descrs.ToArray();
            }
        }

        private TaskDescriptor CreateDescriptor(BaseParams baseParams, long[] portion, int done, ChargePeriod period, UnacceptedChargePacket packet)
        {
            var @params = baseParams.Params.DeepClone();

            @params.SetValue("idsPortion", portion);
            @params.SetValue("processed", done);
            @params.SetValue("periodId", period.Id);
            @params.SetValue("packetId", packet.Id);

            return new TaskDescriptor(
                "Расчет задолженности ЛС",
                PersonalAccountChargeExecutor.Id,
                new BaseParams { Params = @params })
                       {
                           SuccessCallback = PersonalAccountChargeSuccessCallback.Id,
                           FailCallback = PersonalAccountChargeFailCallback.Id
                       };
        }

        private void SpecialAccCredit(ChargePeriod period)
        {
            var creditOrgServCondDomain = this.container.ResolveDomain<CreditOrgServiceCondition>();
            var creditOrgDecisionDomain = this.container.ResolveDomain<CreditOrgDecision>();
            var crFundFormDomain = this.container.ResolveDomain<CrFundFormationDecision>();
            var accOwnerDecision = this.container.ResolveDomain<AccountOwnerDecision>();
            var objDecisionProtocolDomain = this.container.ResolveDomain<RealityObjectDecisionProtocol>();
            var roPayAccount = this.container.ResolveDomain<RealityObjectPaymentAccount>();
            var roPayAccountOper = this.container.ResolveDomain<RealityObjectPaymentAccountOperation>();

            using (
                this.container.Using(
                    creditOrgServCondDomain,
                    creditOrgDecisionDomain,
                    crFundFormDomain,
                    accOwnerDecision,
                    objDecisionProtocolDomain,
                    roPayAccount,
                    roPayAccountOper))
            {
                // получаю все кредитные организации подходящие по дате из грида
                // "Условия обслуживания кредитными организациями"
                var creditOrgServConds =
                    creditOrgServCondDomain.GetAll()
                        .AsEnumerable()
                        .Where(
                            x => (!period.EndDate.HasValue || x.CashServiceDateFrom <= period.EndDate.Value)
                                 && (!x.CashServiceDateTo.HasValue || x.CashServiceDateTo >= period.StartDate))
                        .Where(
                            x =>
                                (!period.EndDate.HasValue || x.OpeningAccDateFrom <= period.EndDate.Value)
                                && (!x.OpeningAccDateTo.HasValue || x.OpeningAccDateTo >= period.StartDate));

                var creditOrgIds = creditOrgServConds.Select(x => x.CreditOrg.Id).ToArray();

                // получаю id протоколов решений, подходящих условиям
                var creditOrgDecisProtocols =
                    creditOrgDecisionDomain.GetAll()
                        .Where(x => x.Decision != null && creditOrgIds.Contains(x.Decision.Id))
                        .Select(x => x.Protocol.Id)
                        .ToArray();

                var crFundFormDecisProtocols =
                    crFundFormDomain.GetAll()
                        .Where(x => x.Decision == CrFundFormationDecisionType.SpecialAccount)
                        .Select(x => x.Protocol.Id)
                        .ToArray();

                var accOwnerDecisProtocols =
                    accOwnerDecision.GetAll()
                        .Where(x => x.DecisionType == AccountOwnerDecisionType.RegOp)
                        .Select(x => x.Protocol.Id)
                        .ToArray();

                var protocols =
                    objDecisionProtocolDomain.GetAll()
                        .Where(x => x.State.Name == "Утверждено")
                        .Where(
                            x =>
                                creditOrgDecisProtocols.Contains(x.Id)
                                && crFundFormDecisProtocols.Contains(x.Id)
                                && accOwnerDecisProtocols.Contains(x.Id))
                        .ToList();

                foreach (var protocol in protocols)
                {
                    var realObj = protocol.RealityObject;
                    var realObjPaymAcc = roPayAccount.GetAll().FirstOrDefault(x => x.RealityObject.Id == realObj.Id);
                    var creditOrg =
                        creditOrgDecisionDomain.GetAll()
                            .FirstOrDefault(x => x.Protocol.Id == protocol.Id)
                            .Return(x => x.Decision);

                    var accCashServiceOperation = new RealityObjectPaymentAccountOperation
                    {
                        Date = DateTime.Now,
                        Account = realObjPaymAcc,
                        OperationSum =
                            creditOrgServConds
                                .FirstOrDefault(x => x.CreditOrg.Id == creditOrg.Id)
                                .Return(x => x.CashServiceSize),
                        OperationType = PaymentOperationType.CashService,
                        OperationStatus = OperationStatus.Default
                    };

                    roPayAccountOper.Save(accCashServiceOperation);
                }
            }
        }

        /// <summary>
        /// Производился ли в предыдущих периодах расчет пени с отсрочкой
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        private bool CheckPenaltyTrace(long[] ids, ChargePeriod period)
        {
            var calcParamTraceDomain = this.container.ResolveDomain<CalculationParameterTrace>();
            var accountChargeDomain = this.container.ResolveDomain<PersonalAccountCharge>();
            var penaltiesDeferredDomain = this.container.ResolveDomain<PenaltiesWithDeferred>();

            using (this.container.Using(calcParamTraceDomain, accountChargeDomain, penaltiesDeferredDomain))
            {
                var penaltiesParam = penaltiesDeferredDomain.GetAll().Select(
                    x => new
                    {
                        x.DateStartCalc,
                        x.DateEndCalc
                    })
                    .OrderByDescending(x => x.DateStartCalc)
                    .FirstOrDefault();

                return !calcParamTraceDomain
                    .GetAll()
                    .Where(
                        y => accountChargeDomain.GetAll()
                            .Where(x => x.ChargeDate >= penaltiesParam.DateStartCalc && x.ChargeDate <= penaltiesParam.DateEndCalc)
                            .Where(x => ids.Contains(x.BasePersonalAccount.Id))
                            .Any(x => x.Guid == y.CalculationGuid))
                    .Any(x => x.CalculationType == CalculationTraceType.DelayPenaltyRecalc);
            }
        }

        #endregion
    }
}