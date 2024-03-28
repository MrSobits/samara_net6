namespace Bars.Gkh.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities.Administration.Notification;

    public class NotifyMessageInterceptor : EmptyDomainInterceptor<NotifyMessage>
    {
        public IGkhUserManager GkhUserManager { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<NotifyMessage> service, NotifyMessage entity)
        {
            entity.User = this.GkhUserManager.GetActiveUser();
            return this.CheckValues(entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<NotifyMessage> service, NotifyMessage entity)
        {
            return this.CheckValues(entity);
        }

        private IDataResult CheckValues(NotifyMessage entity)
        {
            if (entity.StartDate > entity.EndDate)
            {
                return this.Failure("Некорректно задан период актуальности сообщения");
            }

            if (entity.ButtonSet == 0)
            {
                return this.Failure("Не передана конфигурация кнопок");
            }

            return this.Success();
        }
    }
}