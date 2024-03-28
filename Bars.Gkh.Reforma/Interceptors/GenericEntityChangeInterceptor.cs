namespace Bars.Gkh.Reforma.Interceptors
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Reforma.Domain;

    /// <summary>
    /// Обобщенный перехватчик изменения сущностей.
    /// После каждого изменения оповещает об этом трекер изменений IEntityChangeTracker
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericEntityChangeInterceptor<TEntity> : EmptyDomainInterceptor<TEntity>
        where TEntity : class, IEntity
    {
        private readonly IEntityChangeTracker tracker;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="tracker">Трекер изменений</param>
        public GenericEntityChangeInterceptor(IEntityChangeTracker tracker)
        {
            this.tracker = tracker;
        }

        public override IDataResult AfterCreateAction(IDomainService<TEntity> service, TEntity entity)
        {
            this.tracker.NotifyChanged(entity);

            return base.AfterCreateAction(service, entity);
        }

        public override IDataResult AfterUpdateAction(IDomainService<TEntity> service, TEntity entity)
        {
            this.tracker.NotifyChanged(entity);

            return base.AfterUpdateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<TEntity> service, TEntity entity)
        {
            this.tracker.NotifyChanged(entity);

            return base.BeforeDeleteAction(service, entity);
        }
    }
}