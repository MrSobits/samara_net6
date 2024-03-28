namespace Bars.Gkh.SystemDataTransfer.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Domain.Cache;
    using Bars.Gkh.SystemDataTransfer.Meta;
    using Bars.Gkh.SystemDataTransfer.Utils;
    using Bars.Gkh.Utils;

    using FastMember;

    /// <summary>
    /// Хранитель кэша, где хранимая сущность представляется в виде динамического Dto
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TDto">Тип Dto</typeparam>
    public class CacheDtoHolder<TEntity, TDto> : ICacheDtoHolder
        where TEntity : class, IEntity
        where TDto : class, IImportableEntity
    {
        private readonly TypeAccessor accessor;
        private readonly ITransferEntityMeta meta;
        private DtoEntityCache<TEntity, TDto> cacheEntity;

        private readonly IDictionary<string, ICacheDtoHolder> dependencyHolders;

        private Func<TDto, string> keyFunc;
        private Func<IDictionary<string, object>, string> dictionaryKeyFunc;
        private string[] keyProperties;

        /// <inheritdoc />
        public Type Type => typeof(TDto);

        /// <inheritdoc />
        public Type EntityType => typeof(TEntity);

        public CacheDtoHolder(ITransferEntityMeta meta)
        {
            ArgumentChecker.IsType<TransferEntityMeta<TEntity>>(meta, nameof(meta));

            this.meta = meta;
            this.dependencyHolders = new Dictionary<string, ICacheDtoHolder>();
            this.accessor = TypeAccessor.Create(this.Type);
        }

        /// <inheritdoc />
        public virtual IImportableEntity GetFromCache(IDictionary<string, object> objectMap)
        {
            // если сущность не сопоставляемая, то не пытаемся сопоставлять по ключам, только по ImportId
            var entity = this.GetFromCache(objectMap["Id"]) ?? (!this.meta.CreateNew ? this.cacheEntity.GetByKey(this.GetKey(objectMap)) : null);

            if (entity.IsNotNull())
            {
                if (!entity.ImportEntityId.HasValue || entity.ImportEntityId == 0)
                {
                    entity.ImportEntityId = objectMap["Id"].ToLong();
                }
                else if (entity.ImportEntityId != objectMap["Id"].ToLong())
                {
                    throw new ValidationException("Внешний идентификатор сущности не совпадает с хранимой. Это может быть связано с тем, что сопоставляемые поля неуникальны")
                    {
                        Data =
                        {
                            { "Type", typeof(TEntity) },
                            { "ExistsEntity", entity.ImportEntityId },
                            { "Value",  objectMap }
                        }
                    };
                }
            }

            return entity;
        }

        /// <inheritdoc />
        public virtual IImportableEntity GetFromCache(object importId)
        {
            var entities = this.cacheEntity.GetEntities();
            if (entities.Count > 0)
            {
                return entities.FirstOrDefault(x => x.ImportEntityId == (long)importId);
            }

            return null;
        }

        /// <inheritdoc />
        public virtual void RegisterCache(GkhCache cache)
        {
            this.cacheEntity = cache.RegisterDto<TEntity, TDto>(x => x.GetAll().Project(this.meta).To<TDto>(!this.meta.HasCustomSerializer));
        }

        /// <inheritdoc />
        public virtual void AddDependencyType(string name, ICacheDtoHolder depencyHolder)
        {
            this.dependencyHolders[name] = depencyHolder;
        }

        /// <inheritdoc />
        public virtual void MakeDictionary()
        {
            this.cacheEntity.MakeDictionary(this.GetPredicateFunc());
        }

        /// <inheritdoc />
        public virtual void AddEntity(object entity, IDictionary<string, object> objectMap)
        {
            ArgumentChecker.IsType<TEntity>(entity, nameof(entity));
            var dtoEntity = new ClassMappingExpression<TEntity>((TEntity)entity).To<TDto>();

            if (typeof(TEntity).IsNot<IImportableEntity>())
            {
                dtoEntity.ImportEntityId = objectMap["Id"].ToLong();
            }

            if (typeof(TDto).Is<IComplexImportEntity>())
            {
                var value = this.GetInternalKey("ComplexKey", objectMap["ComplexKey"]);
                ((IComplexImportEntity)dtoEntity).ComplexKey = (value as IEntity)?.Id ?? value;
            }

            this.cacheEntity.AddEntity(dtoEntity);
        }

        protected virtual string GetKey(IDictionary<string, object> objectMap)
        {
            return this.GetDictionaryFunc().Invoke(objectMap);
        }

        protected virtual Func<IDictionary<string, object>, string> GetDictionaryFunc()
        {
            if (this.dictionaryKeyFunc.IsNotNull())
            {
                return this.dictionaryKeyFunc;
            }

            this.dictionaryKeyFunc = dict => this.GetKeyProperties()
                .Select(x => this.GetInternalKey(x, dict[x]).ToStr())
                .AggregateWithSeparator("#");

            return this.dictionaryKeyFunc;
        }

        protected virtual Func<TDto, string> GetPredicateFunc()
        {
            if (this.keyFunc.IsNotNull())
            {
                return this.keyFunc;
            }

            this.keyFunc = entity => this.GetKeyProperties()
                .Select(x => this.accessor[entity, x].ToStr())
                .AggregateWithSeparator("#");

            return this.keyFunc;
        }

        protected virtual object GetInternalKey(string name, object value)
        {
            if (this.dependencyHolders.ContainsKey(name))
            {
                if (value.IsNull())
                {
                    return null;
                }

                return this.dependencyHolders[name].GetFromCache(value);
            }

            return value;
        }

        private IEnumerable<string> GetKeyProperties()
        {
            if (this.keyProperties.IsNull())
            {
                var hasComplexProperty = this.meta.Serializer?.HasComplexProperty ?? false;
                this.keyProperties = this.meta
                    .KeyProperties
                    .Select(x => x.Name)
                    .Union(new[] { hasComplexProperty ? "ComplexKey" : null })
                    .Where(x => x != null)
                    .OrderBy(x => x)
                    .ToArray();
            }

            return this.keyProperties;
        }
    }
}