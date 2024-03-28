namespace Bars.Gkh.SystemDataTransfer.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Domain.Cache;
    using Bars.Gkh.SystemDataTransfer.Meta;
    using Bars.Gkh.SystemDataTransfer.Utils;

    /// <summary>
    /// Кэш импорта системы
    /// </summary>
    public class DataTransferCache : IDataTransferCache
    {
        private readonly IDictionary<Type, IDtoEntityBuilder> entityBuilders;

        public ISessionProvider SessionProvider { get; set; }

        public GkhCache GkhCache { get; set; }

        public DataTransferCache()
        {
            this.entityBuilders = new Dictionary<Type, IDtoEntityBuilder>();
        }

        /// <inheritdoc />
        public void RegisterEntityCacheMap<TEntity>(ITransferEntityMeta meta)
            where TEntity : class, IEntity
        {
            ArgumentChecker.IsType<TransferEntityMeta<TEntity>>(meta, nameof(meta));

            var builder = new DtoTypeBuilder<TEntity>(meta);
            this.entityBuilders[typeof(TEntity)] = builder;

            builder.GetCacheHolder().RegisterCache(this.GkhCache);
        }

        /// <inheritdoc />
        public void RegisterDependecy<TEntity>(string propertyName, ITransferEntityMeta depencyMeta)
            where TEntity : class, IEntity
        {
            var dependencyHolder = this.entityBuilders[depencyMeta.Type].GetCacheHolder();
            this.entityBuilders[typeof(TEntity)].GetCacheHolder().AddDependencyType(propertyName, dependencyHolder);
        }

        /// <inheritdoc />
        public void AddInheritance<TEntity>(ITransferEntityMeta inheritMeta)
        {
            var inheritanceHolder = this.entityBuilders[typeof(TEntity)].GetCacheHolder() as IBaseClassCacheDtoHolder;
            inheritanceHolder?.AddInheritHolder(this.entityBuilders[inheritMeta.Type].GetCacheHolder());
        }

        /// <inheritdoc />
        public bool TryGetDependencyId(Type type, object importId, out object entityId)
        {
            var entity = this.entityBuilders[type].GetCacheHolder().GetFromCache(importId);

            entityId = entity?.Id;
            return entity.IsNotNull();
        }

        /// <inheritdoc />
        public bool TryGetDependencyId<TEntity>(object importId, out object entityId)
            where TEntity : class, IEntity
        {
            return this.TryGetDependencyId(typeof(TEntity), importId, out entityId);
        }

        /// <inheritdoc />
        public bool TryGetEnitity<TEntity>(IDictionary<string, object> objectMap, out TEntity entity)
            where TEntity : class, IEntity
        {
            entity = null;

            var cacheItem = this.entityBuilders[typeof(TEntity)].GetCacheHolder().GetFromCache(objectMap);

            if (cacheItem.IsNotNull())
            {
                entity = this.SessionProvider.GetCurrentSession().Load<TEntity>(cacheItem.Id);
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public bool TryGetId<TEntity>(IDictionary<string, object> objectMap, out object entityId)
        {
            entityId = 0L;

            var cacheItem = this.entityBuilders[typeof(TEntity)].GetCacheHolder().GetFromCache(objectMap);
            if (cacheItem.IsNotNull())
            {
                entityId = cacheItem.Id;
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public void AddEntity(IEntity entity, IDictionary<string, object> objectMap)
        {
            this.entityBuilders[entity.GetType()].GetCacheHolder().AddEntity(entity, objectMap);
        }

        /// <inheritdoc />
        public void WarmCache()
        {
            this.GkhCache.UpdateStateless(this.SessionProvider);

            this.entityBuilders
                .Select(x => x.Value.GetCacheHolder())
                .AsParallel()
                .ForAll(x => x.MakeDictionary());
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.GkhCache.Dispose();
        }
    }
}