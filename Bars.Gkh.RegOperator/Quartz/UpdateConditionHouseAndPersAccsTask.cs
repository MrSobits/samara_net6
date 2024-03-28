namespace Bars.Gkh.RegOperator.Quartz
{
    using System;
    using System.Linq;

    using B4.DataAccess;
    using Microsoft.Extensions.Logging;

    using Bars.B4.Config;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;

    using Castle.MicroKernel.Lifestyle;

    using Enums;

    using global::Quartz;

    using Gkh.Domain;
    using Gkh.Entities;
    using Gkh.Enums;

    /// <summary>
    /// Задача по смене статусов ЛС от состояния дома
    /// </summary>
    public class UpdateConditionHouseAndPersAccsTask : BaseTask
    {
        /// <summary>
        /// Метод выполнения задачи
        /// </summary>
        /// <param name="paramDictionary">Параметры</param>
        public override void Execute(DynamicDictionary paramDictionary)
        {
            using (this.Container.BeginScope())
            {
                var logDomain = this.Container.ResolveDomain<EntityLogLight>();
                var realityObjectRepo = this.Container.ResolveRepository<RealityObject>();
                var persAccOperationService = this.Container.Resolve<IPersonalAccountOperationService>();
                var realityObjectPersonalAccountStateProvider = this.Container.Resolve<IRealityObjectPersonalAccountStateProvider>();
                var logManager = this.Container.Resolve<ILogger>();
                var configProvider = this.Container.Resolve<IConfigProvider>();
                if (!configProvider.GetConfig().AppSettings.GetAs("RegOperator.ChangePersonalAccountState.Enabled", true))
                {
                    return;
                }

                try
                {
                    logManager.LogInformation("Запуск задания обработки состояния и типа дома");

                    try
                    {
                        this.Container.InTransaction(() =>
                        {
                            var logList =
                                logDomain.GetAll()
                                    .Where(x => x.ClassName == "RealityObject")
                                    .Where(x => x.PropertyName == "ConditionHouse")
                                    .Where(x => x.DateActualChange.Date == DateTime.Now.Date)
                                    .ToList();

                            // сначала обновляю все жилые дома
                            var roIdsToUpdate = logList.Select(x => x.EntityId).ToArray();

                            var realObjsToUpdate =
                                realityObjectRepo.GetAll().Where(x => roIdsToUpdate.Contains(x.Id)).ToDictionary(x => x.Id);

                            foreach (var log in logList)
                            {
                                var realityObject = realObjsToUpdate[log.EntityId];
                                realityObject.ConditionHouse = (ConditionHouse)log.PropertyValue.ToInt();
                                realityObjectRepo.Update(realityObject);
                            }

                            // теперь закрываю все лицевые счета по домам, у которых состояние 'Аварийный' или 'Снесен'
                            persAccOperationService.MassClosingAccounts(
                                roIdsToUpdate,
                                PersonalAccountChangeType.Close,
                                DateTime.Now,
                                () => "Закрытие ЛС в связи со сменой состояния дома");

                            logManager.LogInformation("Закрытие ЛС в связи со сменой состояния дома завершено");
                        });
                    }
                    catch (Exception e)
                    {
                        logManager.LogError(e, "Ошибка при закрытии лицевых счетов в связи со сменой состояния дома");
                    }


                    var realityObjects = realityObjectRepo.GetAll().ToList();
                    foreach (var realityObject in realityObjects)
                    {
                        try
                        {
                            this.Container.InTransaction(() => realityObjectPersonalAccountStateProvider.SetPersonalAccountStateIfNeed(realityObject));
                        }
                        catch (Exception exception)
                        {
                            logManager.LogError($"Ошибка во время смены статусов ЛС во время обработки состояния дома ({realityObject.Address})", exception);
                        }
                    }

                    logManager.LogInformation("Завершение задания обработки состояния и типа дома");
                }
                catch (Exception exception)
                {
                    logManager.LogError(exception.Message, exception);
                }
                finally
                {
                    this.Container.Release(logDomain);
                    this.Container.Release(realityObjectRepo);
                    this.Container.Release(logManager);
                    this.Container.Release(persAccOperationService);
                    this.Container.Release(realityObjectPersonalAccountStateProvider);
                }

                // если всё хорошо, то
                // запускаем следующую по цепочке задачу, там будут синхронизированы состояния аварийных домов
                var scheduler = this.Container.Resolve<IScheduler>();
                var job = JobBuilder.Create<TaskJob<SyncEmergencyObjectsTask>>().Build();
                var trigger = TriggerBuilder.Create().WithIdentity(nameof(SyncEmergencyObjectsTask)).StartNow().Build();
                scheduler.ScheduleJob(job, trigger);
            }
        }
    }
}