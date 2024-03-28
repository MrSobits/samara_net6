namespace Bars.Gkh.RegOperator.DomainModelServices.MassUpdater
{
    using System.Collections.Generic;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Extensions;

    using Castle.Windsor;

    /// <summary>
    /// Исполнитель по умолчанию для массовых операций
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности для изменения</typeparam>
    public abstract class DefaultMassOperationExecutor<TEntity> : IMassOperationExecutor<TEntity>
        where TEntity : PersistentObject
    {
        private readonly IDictionary<long, TEntity> entities;

        private IWindsorContainer container => ApplicationContext.Current.Container;

        protected DefaultMassOperationExecutor()
        {
            this.entities = new Dictionary<long, TEntity>();
        }

        /// <summary>
        /// Добавить сущность для изменения
        /// </summary>
        public virtual void AddEntity(TEntity entity)
        {
            if (MassUpdateContext.CurrentContext.IsNotNull())
            {
                MassUpdateContext.CurrentContext.AddHandler(this);
            }

            this.entities[entity.Id] = entity;
        }

        /// <summary>
        /// Обработать все изменения
        /// </summary>
        /// <param name="useStatelessSession"></param>
        public virtual void ProcessChanges(bool useStatelessSession)
        {
            var entitiesToSave = this.ProcessChangesInternal(this.entities.Values, useStatelessSession);

            if (useStatelessSession)
            {
                this.container.InStatelessTransaction(stateless =>
                {
                    entitiesToSave.ForEach(stateless.InsertOrUpdate);
                });
            }
            else
            {
                this.container.InTransaction(() =>
                {
                    var domain = this.container.ResolveDomain<TEntity>();
                    using (this.container.Using(domain))
                    {
                        entitiesToSave.ForEach(x => domain.SaveOrUpdate(x));
                    }
                });
            }
        }

        protected abstract IEnumerable<TEntity> ProcessChangesInternal(IEnumerable<TEntity> entities, bool useStatless);
    }
}