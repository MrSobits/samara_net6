namespace Bars.GisIntegration.Base.Extensions
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils.Annotations;

    /// <summary>
    /// 
    /// </summary>
    public static class DomainServiceExtensions
    {
        /// <summary>
        /// Если Id > 0, то обновить, иначе сохранить
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="domain"></param>
        /// <param name="entity"></param>
        public static void SaveOrUpdate<T>(this IDomainService<T> domain, T entity) where T: PersistentObject
        {
            ArgumentChecker.NotNull(domain, "domain");
            ArgumentChecker.NotNull(entity, "entity");

            if (entity.Id > 0)
            {
                domain.Update(entity);
            }
            else
            {
                domain.Save(entity);
            }
        }

        /// <summary>
        /// Если Id>0, то удалить, иначе ничего не делать
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="domain"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool DeleteIfExists<T>(this IDomainService<T> domain, T entity) where T : PersistentObject
        {
            ArgumentChecker.NotNull(entity, "entity");

            return DomainServiceExtensions.DeleteIfExists(domain, entity.Id);
        }

        /// <summary>
        /// Если Id>0, то удалить, иначе ничего не делать
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="entityId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool DeleteIfExists<T>(this IDomainService<T> domain, long entityId) where T : PersistentObject
        {
            ArgumentChecker.NotNull(domain, "domain");

            if (entityId > 0)
            {
                domain.Delete(entityId);
            }

            return entityId > 0;
        }
    }
}