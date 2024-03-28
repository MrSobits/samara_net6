namespace Bars.Gkh.Interceptors
{
    using System;

    using Bars.B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.ExecutionAction.ExecutionActionScheduler;

    public class ExecutionActionTaskInterceptor : EmptyDomainInterceptor<ExecutionActionTask>
    {
        public IGkhUserManager GkhUserManager { get; set; }
        public IExecutionActionService ExecutionActionService { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<ExecutionActionTask> service, ExecutionActionTask entity)
        {
            this.PrepareEntity(entity);
            return this.Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ExecutionActionTask> service, ExecutionActionTask entity)
        {
            this.PrepareEntity(entity);
            return this.ExecutionActionService.ReplaceTaskAction(entity);
        }

        public override IDataResult AfterDeleteAction(IDomainService<ExecutionActionTask> service, ExecutionActionTask entity)
        {
            return this.ExecutionActionService.DeleteTaskAction(entity);
        }

        private void PrepareEntity(ExecutionActionTask entity)
        {
            if (entity.StartNow)
            {
                entity.StartTimeHour = DateTime.Now.Hour;
                entity.StartTimeMinutes = DateTime.Now.Minute;
            }

            var user = this.GkhUserManager.GetActiveUser();
            entity.User = user;
        }
    }
}