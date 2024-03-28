namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.Gkh.Config;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Migrations;
    using Bars.Gkh.Quartz.Job;

    using Castle.Windsor;

    using global::Quartz;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Сервис работы с выполняемыми действиями
    /// </summary>
    public class ExecutionActionService : IExecutionActionService
    {
        public IWindsorContainer Container { get; set; }
        public ILogger LogManager { get; set; }
        public IUserIdentityProvider UserIdentityProvider { get; set; }
        public IUserIdentity UserIdentity { get; set; }
        public IExecutionActionInfoService ExecutionActionInfoService { get; set; }
        public IExecutionActionScheduler Scheduler { get; set; }
        public IExecutionActionJobBuilder ExecutionActionJobBuilder { get; set; }

        /// <inheritdoc />
        public IDataResult CreateTaskAction(ExecutionActionTask task)
        {
            var actionInfo = this.ExecutionActionInfoService.GetInfo(task.ActionCode);

            var action = this.GetMandatoryAction(task.ActionCode);
            using (this.Container.Using(action))
            {
                if (action != null && !this.IsNeedAction(action))
                {
                    return BaseDataResult.Error($"Выполнение обязательного действия '{actionInfo.Name}' не требуется");
                }
            }

            return this.CreateInternal(task);
        }

        private IDataResult CreateInternal(ExecutionActionTask task)
        {
            try
            {
                return this.Scheduler.ScheduleJob(this.GetJob(task));
            }
            catch (ObjectAlreadyExistsException ex)
            {
                this.LogManager.LogWarning(ex, ex.Message);
                return BaseDataResult.Error("Действие уже поставлено в очередь");
            }
            catch (Exception ex)
            {
                this.LogManager.LogError(ex, ex.Message);
                return new BaseDataResult(false, ex.Message);
            }
        }

        /// <inheritdoc />
        public IDataResult ReplaceTaskAction(ExecutionActionTask task)
        {
            var actionInfo = this.ExecutionActionInfoService.GetInfo(task.ActionCode);
            var action = this.GetMandatoryAction(task.ActionCode);
            try
            {
                var result = this.Scheduler.UnScheduleJob(this.GetJob(task));
                if (!result.Success)
                {
                    return result;
                }

                if (action != null && !this.IsNeedAction(action))
                {
                    return BaseDataResult.Error($"Выполнение обязательного действия '{actionInfo.Name}' не требуется");
                }

                result = this.Scheduler.ScheduleJob(this.GetJob(task));
                if (!result.Success)
                {
                    return result;
                }

                var timespan = (DateTimeOffset)result.Data;
                result.Message = $"Действие \"{actionInfo.Name}\" перезапущено {timespan.ToLocalTime()}";

                return result;
            }
            catch (Exception ex)
            {
                this.LogManager.LogError(ex, ex.Message);
                return new BaseDataResult(false, ex.Message);
            }
        }

        /// <inheritdoc />
        public IDataResult DeleteTaskAction(ExecutionActionTask task)
        {
            try
            {
                return this.Scheduler.UnScheduleJob(this.GetJob(task));
            }
            catch (Exception ex)
            {
                this.LogManager.LogError(ex, ex.Message);
                return new BaseDataResult(false, ex.Message);
            }
        }

        private IGkhQuartzJob GetJob(ExecutionActionTask task)
        {
            var identity = this.UserIdentityProvider.GetCurrentUserIdentity() ?? this.UserIdentity;
            return this.ExecutionActionJobBuilder
                .SetSchedulableTask(task)
                .SetActionCode(task.ActionCode)
                .SetUserIdentity(identity)
                .SetParams(task.BaseParams)
                .Build();
        }

        /// <inheritdoc />
        public IDataResult GetSchedulerQueue()
        {
            var info = this.Scheduler.GetJobsInfo();
            return new ListDataResult(info, info.Count);
        }

        /// <inheritdoc />
        public void RestoreJobs()
        {
            if (MigrationChecker.IsNeedMigration())
            {
                return;
            }

            this.CheckErrorJobs();

            this.ScheduleStoredJobs();

            this.StartMandatoryExecutionActions();
        }

        /// <inheritdoc />
        public IDataResult CreateTaskFromExecutionAction(string actionCode, BaseParams actionParams = null)
        {
            var task = new ExecutionActionTask
            {
                ActionCode = actionCode,
                BaseParams = actionParams ?? new BaseParams(),
                PeriodType = TaskPeriodType.NoPeriodicity,
                StartNow = true,
                IsDelete = true
            };

            this.Container.UsingForResolved<IDomainService<ExecutionActionTask>>((ioc, domain) =>
            {
                domain.SaveOrUpdate(task);
            });

            return new BaseDataResult(task);
        }

        private void CreateTaskFromAutostart(IMandatoryExecutionAction action)
        {
            this.Container.UsingForResolved<IDomainService<ExecutionActionTask>>((ioc, domain) =>
            {
                var existsTask = domain.GetAll()
                    .Where(x => x.ActionCode == action.Code)
                    .Where(x => x.User == null)
                    .Where(x => x.IsDelete)
                    .OrderByDescending(x => x.ObjectCreateDate)
                    .FirstOrDefault();

                var task = existsTask ?? new ExecutionActionTask
                {
                    ActionCode = action.Code,
                    BaseParams = action.ExecutionParams ?? new BaseParams(),
                    PeriodType = TaskPeriodType.NoPeriodicity,
                    IsDelete = true
                };

                task.StartNow = true;
                task.BaseParams = task.BaseParams ?? new BaseParams();

                domain.SaveOrUpdate(task);
            });
        }

        private void StartMandatoryExecutionActions()
        {
            var configManager = this.Container.Resolve<IGkhParams>();
            using (this.Container.Using(configManager))
            {
                var autostart = configManager.GetParams().GetAs(
                    ExecutionActionScheduler.AutoStartMandatoryKeyName,
                    ExecutionActionScheduler.DefaultAutoStartMandatory);

                if (!autostart)
                {
                    return;
                }
            }

            try
            {
                this.Container.UsingForResolvedAll<IMandatoryExecutionAction>((ioc, actions) =>
                {
                    foreach (var mandatoryExecutionAction in actions)
                    {
                        try
                        {
                            if (mandatoryExecutionAction.IsNeedAction())
                            {
                                this.CreateTaskFromAutostart(mandatoryExecutionAction);
                            }
                        }
                        catch (ObjectAlreadyExistsException ex)
                        {
                            this.LogManager.LogWarning(ex, ex.Message);
                        }
                        catch (Exception ex)
                        {
                            this.LogManager.LogError(ex, ex.Message);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                this.LogManager.LogError(ex, ex.Message);
            }
            finally
            {
                this.Container.UsingForResolved<ISessionProvider>((container, sessionProvider) =>
                {
                    var session = sessionProvider.GetCurrentSession();
                    if (session.IsOpen)
                    {
                        sessionProvider.GetCurrentSession().Clear();
                    }
                });
            }
        }

        private IMandatoryExecutionAction GetMandatoryAction(string code)
        {
            try
            {
                return this.Container.Resolve<IMandatoryExecutionAction>(code);
            }
            catch (InvalidCastException)
            {
                return null;
            }
        }

        private bool IsNeedAction(IMandatoryExecutionAction action)
        {
            var repeateable = Attribute.IsDefined(action.GetType(), typeof(RepeatableAttribute));

            if (repeateable)
            {
                // если запускаем вручную и действие повторяемо, то не проверяем на необходимость выполнения
                return true;
            }

            return action.IsNeedAction();
        }

        private void ScheduleStoredJobs()
        {
            this.Container.UsingForResolved<IRepository<ExecutionActionTask>>((ioc, repository) =>
            {
                var now = DateTime.Now;
                var today = DateTime.Today;
                var morrow = today.AddDays(1);
                var nowHour = now.Hour;
                var nowMinute = now.Minute;
                var executionActionTasks = repository.GetAll()
                    .Where(x => !x.IsDelete)
                    .Where(x => x.PeriodType != TaskPeriodType.NoPeriodicity || 
                        x.ObjectEditDate >= today && x.ObjectEditDate < morrow && x.StartTimeHour >= nowHour && x.StartTimeMinutes >= nowMinute
                    )
                    .Where(x => !x.StartDate.HasValue || x.StartDate < DateTime.Today)
                    .Where(x => !x.EndDate.HasValue || x.EndDate > DateTime.Today)
                    .ToList();

                foreach (var task in executionActionTasks)
                {
                    var crateResult = this.CreateInternal(task);
                    if (!crateResult.Success)
                    {
                        this.LogManager.LogError($"Не удалось восстановить действие '{task.ActionCode}' при перезапуске|{crateResult.Message}");
                    }
                }
            });
        }

        private void CheckErrorJobs()
        {
            NhExtentions.InTransaction(this.Container,
                () =>
            {
                this.Container.UsingForResolved<IRepository<ExecutionActionResult>>((ioc, repository) =>
                {
                    var jobResult = repository.GetAll()
                        .Where(x => x.Status == ExecutionActionStatus.Running
                            || x.Status == ExecutionActionStatus.Queued
                            || x.Status == ExecutionActionStatus.Initial);

                    foreach (var job in jobResult)
                    {
                        job.Status = ExecutionActionStatus.AbortedOnRestart;

                        repository.Update(job);
                    }
                });
            });
        }
    }
}