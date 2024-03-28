namespace Bars.Gkh.Utils
{
    using B4.DataAccess;
    using B4.Utils.Annotations;

    public static class RepositoryExtensions
    {
        /// <summary>
        /// Если Id > 0, то обновить, иначе сохранить
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repo"></param>
        /// <param name="entity"></param>
        public static void SaveOrUpdate<T>(this IRepository<T> repo, T entity) where T : PersistentObject
        {
            ArgumentChecker.NotNull(repo, "repo");
            ArgumentChecker.NotNull(entity, "entity");

            if (entity.Id > 0)
            {
                repo.Update(entity);
            }
            else
            {
                repo.Save(entity);
            }
        }

        /// <summary>
        /// Если Id>0, то удалить, иначе ничего не делать
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repo"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool DeleteIfExists<T>(this IRepository<T> repo, T entity) where T : PersistentObject
        {
            ArgumentChecker.NotNull(entity, "entity");

            return repo.DeleteIfExists(entity.Id);
        }

        /// <summary>
        /// Если Id>0, то удалить, иначе ничего не делать
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="entityId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool DeleteIfExists<T>(this IRepository<T> repo, long entityId) where T : PersistentObject
        {
            ArgumentChecker.NotNull(repo, "repo");

            if (entityId > 0)
            {
                repo.Delete(entityId);
            }

            return entityId > 0;
        }
    }
}