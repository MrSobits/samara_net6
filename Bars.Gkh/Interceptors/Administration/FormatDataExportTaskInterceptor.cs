namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.FormatDataExport.Scheduler;
    using Bars.Gkh.FormatDataExport.Scheduler.Impl;

    /// <summary>
    /// Задача для экспорта по формату
    /// </summary>
    public class FormatDataExportTaskInterceptor : EmptyDomainInterceptor<FormatDataExportTask>
    {
        public IGkhUserManager GkhUserManager { get; set; }
        public IFormatDataExportSchedulerService FormatDataExportScheduler { get; set; }

        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<FormatDataExportTask> service, FormatDataExportTask entity)
        {
            this.PrepareEntity(entity);

            var activeUser = this.GkhUserManager.GetActiveUser();
            var checkResult = this.CheckOperator(activeUser);
            if (!checkResult.Success)
            {
                return checkResult;
            }

            entity.User = activeUser;
            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<FormatDataExportTask> service, FormatDataExportTask entity)
        {
            var activeUser = this.GkhUserManager.GetActiveUser();

            var checkResult = this.CheckOperator(activeUser);
            if (!checkResult.Success)
            {
                return checkResult;
            }

            this.PrepareEntity(entity);
            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult AfterUpdateAction(IDomainService<FormatDataExportTask> service, FormatDataExportTask entity)
        {
            var job = new FormatDataExportJobInstance(entity);
            this.FormatDataExportScheduler.UnScheduleJob(job);
            this.FormatDataExportScheduler.ScheduleJob(job);
            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult AfterDeleteAction(IDomainService<FormatDataExportTask> service, FormatDataExportTask entity)
        {
            this.FormatDataExportScheduler.UnScheduleJob(new FormatDataExportJobInstance(entity));
            return this.Success();
        }

        private void PrepareEntity(FormatDataExportTask entity)
        {
            if (entity.StartNow)
            {
                entity.StartTimeHour = DateTime.Now.Hour;
                entity.StartTimeMinutes = DateTime.Now.Minute;
            }
            if (entity.EntityGroupCodeList.Contains("All"))
            {
                entity.EntityGroupCodeList.Clear();
            }
        }

        private IDataResult CheckOperator(User user)
        {
            if (user.IsNull())
            {
                return this.Failure("Не удалось определить пользователя");
            }

            this.Container.UsingForResolved<IFormatDataExportRoleService>((container, service) =>
            {
                // Выбросит исключение, если не сопоставлена роль оператора не сопоставлена с поставщиком информации
                service.GetCustomProviderFlags(user);
            });

            return new BaseDataResult();
        }
    }
}