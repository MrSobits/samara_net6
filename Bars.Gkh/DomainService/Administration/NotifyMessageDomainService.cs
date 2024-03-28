namespace Bars.Gkh.DomainService
{
    using Bars.B4;
    using Bars.Gkh.Entities.Administration.Notification;

    public class NotifyMessageDomainService : BaseDomainService<NotifyMessage>
    {
        /// <inheritdoc />
        protected override void DeleteInternal(object id)
        {
            var entity = this.Repository.Load(id);
            entity.IsDelete = true;
            this.Repository.Update(entity);
        }

        /// <inheritdoc />
        protected override void DeleteEntityInternal(NotifyMessage entity)
        {
            entity.IsDelete = true;
            this.Repository.Update(entity);
        }
    }
}