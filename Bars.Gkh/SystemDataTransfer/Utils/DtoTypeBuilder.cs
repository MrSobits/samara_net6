namespace Bars.Gkh.SystemDataTransfer.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection.Emit;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.SystemDataTransfer.Caching;
    using Bars.Gkh.SystemDataTransfer.Meta;

    /// <summary>
    /// Билдер Dto типов
    /// </summary>
    /// <typeparam name="TEntity">Тип исходной сущности</typeparam>
    public class DtoTypeBuilder<TEntity> : TypeBuilderBase, IDtoEntityBuilder where TEntity : class, IEntity
    {
        private readonly TransferEntityMeta<TEntity> meta;
        private ICacheDtoHolder cacheDtoHolder;

        /// <inheritdoc />
        public override string TypeSignature => $"{typeof(TEntity).Name}CacheDto";

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="meta">Мета-описание</param>
        public DtoTypeBuilder(ITransferEntityMeta meta)
        {
            ArgumentChecker.IsType<TransferEntityMeta<TEntity>>(meta, nameof(meta));
            this.meta = (TransferEntityMeta<TEntity>)meta;
        }

        /// <inheritdoc />
        [DebuggerStepThrough] // т.к. часто вызывается, пропускаем его отладчиком
        public ICacheDtoHolder GetCacheHolder()
        {
            this.EnsureCreatedHolder();
            return this.cacheDtoHolder;
        }

        /// <inheritdoc />
        protected override TypeBuilder GetTypeBuilder()
        {
            var tb = base.GetTypeBuilder();

            tb.AddInterfaceImplementation(typeof(IEntity));
            tb.AddInterfaceImplementation(typeof(IImportableEntity));

            if (this.meta.Serializer?.HasComplexProperty ?? false)
            {
                tb.AddInterfaceImplementation(typeof(IComplexImportEntity));
            }

            return tb;
        }

        /// <inheritdoc />
        protected override IDictionary<string, Type> GetProperties()
        {
            var properties = this.meta.KeyProperties
                .Union(new[] 
                {
                    typeof(IEntity).GetProperty("Id"),
                    typeof(IImportableEntity).GetProperty("ImportEntityId")
                })
                .ToDictionary(x => x.Name, x => x.PropertyType);

            var hasComplexProperty = this.meta.Serializer?.HasComplexProperty ?? false;
            if (hasComplexProperty)
            {
                properties["ComplexKey"] = typeof(IComplexImportEntity).GetProperty("ComplexKey").PropertyType;
            }

            return properties;
        }

        /// <inheritdoc />
        protected override void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            // если хранимая сущность, то формируем идентификатор
            if (typeof(IEntity).IsAssignableFrom(propertyType))
            {
                base.CreateProperty(tb, propertyName, typeof(long?));
            }
            else
            {
                base.CreateProperty(tb, propertyName, propertyType);
            }
        }

        private void EnsureCreatedHolder()
        {
            if (this.cacheDtoHolder.IsNotNull())
            {
               return;
            }

            if (this.meta.IsBase)
            {
                var type = typeof(BaseClassCacheDtoHolder<,>).MakeGenericType(typeof(TEntity), this.GetDefinedType());
                this.cacheDtoHolder = (ICacheDtoHolder)Activator.CreateInstance(type, this.meta);
            }
            else
            {
                var type = typeof(CacheDtoHolder<,>).MakeGenericType(typeof(TEntity), this.GetDefinedType());
                this.cacheDtoHolder = (ICacheDtoHolder)Activator.CreateInstance(type, this.meta);
            }
        }
    }
}