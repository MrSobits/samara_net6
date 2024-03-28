namespace Bars.Gkh.Repositories
{
    using B4;
    using B4.Application;
    using B4.DataAccess;
    using Castle.Windsor;

    public class BaseDomainRepository<T>: IDomainRepository<T> where T: BaseEntity
    {
        protected IDomainService<T> DomainService { get { return Container.Resolve<IDomainService<T>>(); } }

        protected IWindsorContainer Container { get { return ApplicationContext.Current.Container; } }

        public virtual void SaveOrUpdate(T entity)
        {
            if (entity.Id > 0)
            {
                DomainService.Update(entity);
            }
            else
            {
                DomainService.Save(entity);
            }
        }
    }
}