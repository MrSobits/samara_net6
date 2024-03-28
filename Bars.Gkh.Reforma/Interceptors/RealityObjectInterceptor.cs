namespace Bars.Gkh.Reforma.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Reforma.Entities.ChangeTracker;

    /// <summary>
    /// Интерцептор для <see cref="RealityObject"/>
    /// </summary>
    public class RealityObjectInterceptor : EmptyDomainInterceptor<RealityObject>
    {
        /// <summary>
        /// Выполнить действие перед удаление жилого дома
        /// </summary>
        /// <param name="service">Домен сервис</param>
        /// <param name="entity">Сущность жилого дома</param>
        /// <returns></returns>
        public override IDataResult BeforeDeleteAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            var changedRobjectDomain = this.Container.Resolve<IDomainService<ChangedRobject>>();

            try
            {
                changedRobjectDomain.GetAll()
                    .Where(x => x.RealityObject == entity)
                    .Select(x => x.Id)
                    .ForEach(x => changedRobjectDomain.Delete(x));
            }
            catch (ValidationException exc)
            {
                return this.Failure(exc.Message);
            }
            finally
            {
                this.Container.Release(changedRobjectDomain);
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}