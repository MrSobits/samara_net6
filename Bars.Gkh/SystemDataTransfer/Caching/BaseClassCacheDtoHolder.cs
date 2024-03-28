namespace Bars.Gkh.SystemDataTransfer.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.Cache;
    using Bars.Gkh.SystemDataTransfer.Meta;

    /// <summary>
    /// Обертка для работы с наследниками через базовый класс
    /// </summary>
    public class BaseClassCacheDtoHolder<TEntity, TDto> : CacheDtoHolder<TEntity, TDto>, IBaseClassCacheDtoHolder where TEntity : class, IEntity
        where TDto : class, IImportableEntity
    {
        private readonly IList<ICacheDtoHolder> inheritHolders;

        /// <inheritdoc />
        public BaseClassCacheDtoHolder(ITransferEntityMeta meta)
            : base(meta)
        {
            this.inheritHolders = new List<ICacheDtoHolder>();
        }

        public void AddInheritHolder(ICacheDtoHolder holder)
        {
            if (!holder.EntityType.Is<TEntity>())
            {
                throw new ArgumentException($"Type {holder.Type} is not inherits from {this.Type}");
            }

            this.inheritHolders.Add(holder);
        }

        /// <inheritdoc />
        public override void AddDependencyType(string name, ICacheDtoHolder depencyHolder)
        {
            this.inheritHolders.ForEach(x => x.AddDependencyType(name, depencyHolder));
        }

        /// <inheritdoc />
        public override void AddEntity(object entity, IDictionary<string, object> objectMap)
        {
            var holder = this.inheritHolders.FirstOrDefault(x => x.Type == entity.GetType());
            if (holder.IsNotNull())
            {
                holder.AddEntity(entity, objectMap);
            }
        }

        /// <inheritdoc />
        public override IImportableEntity GetFromCache(IDictionary<string, object> objectMap)
        {
            return this.inheritHolders.Select(x => x.GetFromCache(objectMap)).SingleOrDefault(x => x != null);
        }

        /// <inheritdoc />
        public override IImportableEntity GetFromCache(object importId)
        {
            return this.inheritHolders.Select(x => x.GetFromCache(importId)).SingleOrDefault(x => x != null);
        }

        /// <inheritdoc />
        public override void RegisterCache(GkhCache cache)
        {
        }


        /// <inheritdoc />
        public override void MakeDictionary()
        {
        }
    }
}