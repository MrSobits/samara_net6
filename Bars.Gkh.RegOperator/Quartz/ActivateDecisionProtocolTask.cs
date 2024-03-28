namespace Bars.Gkh.RegOperator.Quartz
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using B4.DataAccess;
    using B4.Modules.Quartz;
    using B4.Utils;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.IoC;

    using Castle.MicroKernel.Lifestyle;

    using Microsoft.Extensions.Logging;

    using Decisions.Nso.Entities;
    using Decisions.Nso.Entities.Decisions;

    using DomainService;
    using DomainService.PersonalAccount;

    using global::Quartz;

    using Gkh.Entities;
        
    /// <summary>
    /// Фоновая задача по установке актуальных статусов ЛС в соответствии с датой начала действия протокола
    /// </summary>
    public class ActivateDecisionProtocolTask : BaseTask
    {
        /// <summary>
        /// Выполнение задачи
        /// </summary>
        /// <param name="params">Параметры исполнения задачи.
        /// При вызове из планировщика передаются параметры из JobDataMap 
        /// и контекст исполнения в параметре JobContext        
        /// </param>
        public override void Execute(DynamicDictionary @params)
        {
            
            using (this.Container.BeginScope())
            {
                var logger = this.Container.Resolve<ILogger>();
                var scheduler = this.Container.Resolve<IScheduler>();
                var configProvider = this.Container.Resolve<IConfigProvider>();

                if (!configProvider.GetConfig().AppSettings.GetAs("RegOperator.ChangePersonalAccountState.Enabled", true))
                {
                    return;
                }

                var stopWatch = Stopwatch.StartNew();
                logger.LogInformation("Задание {0} успешно запущено в {1}".FormatUsing("ActivateDecisionProtocolTask", DateTime.Now));

                try
                {
                    this.ExecuteInternal(logger);
                    logger.LogInformation("Задание {0} успешно завершено в {1}, затрачено {2:g}".FormatUsing("ActivateDecisionProtocolTask", DateTime.Now, stopWatch.Elapsed));
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "Ошибка во время выполнения задания ActivateDecisionProtocolTask");
                }

                // если всё хорошо, то
                // запускаем следующую по цепочке задачу, там будут обработаны дома согласно состояния дома
                var job = JobBuilder.Create<TaskJob<UpdateConditionHouseAndPersAccsTask>>().Build();
                var trigger = TriggerBuilder.Create().WithIdentity(nameof(UpdateConditionHouseAndPersAccsTask)).StartNow().Build();
                scheduler.ScheduleJob(job, trigger);
            }
        }

        private void ExecuteInternal(ILogger logger)
        {
            var crfundDecisionDomain = this.Container.ResolveDomain<CrFundFormationDecision>();
            var accountOwnerDecisionDomain = this.Container.ResolveDomain<AccountOwnerDecision>();
            var govDecisionDomain = this.Container.ResolveDomain<GovDecision>();
            var realityObjectDomain = this.Container.ResolveDomain<RealityObject>();
            var specCalcService = this.Container.Resolve<ISpecialCalcAccountService>();
            var govDecService = this.Container.Resolve<IGovDecisionAccountService>();

            using (this.Container.Using(crfundDecisionDomain, accountOwnerDecisionDomain, govDecisionDomain, specCalcService, govDecService, realityObjectDomain))
            {
                var crFundDecisionCache = crfundDecisionDomain.GetAll()
                        .Where(x => x.Protocol.State.FinalState)
                        .Where(x => x.Protocol.DateStart <= DateTime.Today)
                        .Select(x => new { x.Protocol.RealityObject.Id, x.Protocol.DateStart, Decision = x })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, z => z.OrderByDescending(x => x.DateStart).Select(x => x.Decision).First());

                var accountOwnerDecisionCache = accountOwnerDecisionDomain.GetAll()
                        .Where(x => x.Protocol.State.FinalState)
                        .Where(x => x.Protocol.DateStart <= DateTime.Today)
                        .Select(x => new { x.Protocol.RealityObject.Id, x.Protocol.DateStart, Decision = x })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, z => z.OrderByDescending(x => x.DateStart).Select(x => x.Decision).First());

                var govDecisionCache = govDecisionDomain.GetAll()
                        .Where(x => x.State.FinalState)
                        .Where(x => x.DateStart <= DateTime.Today)
                        .Where(x => x.FundFormationByRegop)
                        .Select(x => new { x.RealityObject.Id, x.DateStart, Decision = x })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, z => z.OrderByDescending(x => x.DateStart).Select(x => x.Decision).First());

                var robjectsToUpdate = crFundDecisionCache.Select(x => x.Key).Union(govDecisionCache.Select(x => x.Key)).ToHashSet();

                foreach (var realityObjectId in robjectsToUpdate)
                {
                    try
                    {
                        this.UpdateRoDecisions(realityObjectId, specCalcService, govDecService, crFundDecisionCache, accountOwnerDecisionCache, govDecisionCache);
                    }
                    catch (ValidationException ex)
                    {
                        logger.LogInformation(
                            "Ошибка во время обработки решения на доме({0}): {1}", 
                            this.GetRealityObjectAddress(realityObjectId, crFundDecisionCache, govDecisionCache),
                            ex.Message);
                    }
                    catch (Exception ex)
                    {
                        var msg = $"Ошибка во время обработки решения на доме({this.GetRealityObjectAddress(realityObjectId, crFundDecisionCache, govDecisionCache)})";
                        logger.LogWarning(msg, ex);
                    }
                }
            }
        }

        private void UpdateRoDecisions(
            long roId,
            ISpecialCalcAccountService specAccService,
            IGovDecisionAccountService govDecService,
            Dictionary<long, CrFundFormationDecision> crFundCache,
            Dictionary<long, AccountOwnerDecision> accOwnCache,
            Dictionary<long, GovDecision> govdecCache)
        {
            var activeProtocol = this.GetActiveProtocol(roId, crFundCache, govdecCache);

            var protocol = activeProtocol as GovDecision;
            if (protocol != null)
            {
                govDecService.SetPersonalAccountStateIfNeeded(protocol);

                specAccService
                    .HandleSpecialAccountByProtocolChange(
                        null,
                        null,
                        protocol,
                        new RealityObject { Id = roId });
            }
            else
            {
                var decision = activeProtocol as CrFundFormationDecision;
                if (decision != null)
                {
                    specAccService
                        .HandleSpecialAccountByProtocolChange(
                            accOwnCache.Get(roId),
                            decision,
                            null,
                            new RealityObject {Id = roId});
                }
            }
        }

        private object GetActiveProtocol(
            long roId, 
            Dictionary<long, CrFundFormationDecision> crFundCache, 
            Dictionary<long, GovDecision> govdecCache)
        {
            var anotherProtocol = crFundCache.Get(roId);

            var oneMoreProtocol = govdecCache.Get(roId);

            if (anotherProtocol != null && oneMoreProtocol == null)
            {
                return anotherProtocol;
            }

            if (anotherProtocol == null && oneMoreProtocol != null)
            {
                return oneMoreProtocol;
            }

            if (anotherProtocol == null) return null;

            if (anotherProtocol.Protocol.DateStart > oneMoreProtocol.ProtocolDate)
            {
                return anotherProtocol;
            }

            return oneMoreProtocol;
        }

        private string GetRealityObjectAddress(
            long roId,
            Dictionary<long, CrFundFormationDecision> crFundCache,
            Dictionary<long, GovDecision> govdecCache)
        {
            var activeProtocol = this.GetActiveProtocol(roId, crFundCache, govdecCache);

            var protocol = activeProtocol as GovDecision;
            if (protocol != null)
            {
                return protocol.ReturnSafe(x => x.RealityObject).ReturnSafe(x => x.Address);
            }

            var decision = activeProtocol as CrFundFormationDecision;
            if (decision != null)
            {
                return decision.Protocol.ReturnSafe(x => x.RealityObject).ReturnSafe(x => x.Address);
            }

            return string.Empty;
        }
    }
}