namespace Bars.Gkh.SystemDataTransfer.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Контейнер описаний сущностей
    /// </summary>
    public class TransferEntityContainer
    {
        private readonly Dictionary<Type, ITransferEntityMeta> container = new Dictionary<Type, ITransferEntityMeta>();

        /// <summary>
        /// Словарь описаний
        /// </summary>
        public IDictionary<Type, ITransferEntityMeta> Container => new ReadOnlyDictionary<Type, ITransferEntityMeta>(this.container);

        /// <summary>
        /// Добавить описание
        /// </summary>
        /// <param name="description">Описание</param>
        public TransferEntityMetaBuilder<TEntity> Add<TEntity>(string description) where TEntity : class, IEntity, new()
        {
            var meta = new TransferEntityMeta<TEntity> { Description = description };
            this.container.Add(typeof(TEntity), meta);

            return new TransferEntityMetaBuilder<TEntity>(meta);
        }

        /// <summary>
        /// Заменить описание
        /// </summary>
        /// <param name="description">Описание</param>
        public TransferEntityMetaBuilder<TEntity> Replace<TRepalce, TEntity>(string description)
            where TRepalce : class, IEntity, new()
            where TEntity : class, IEntity, new()
        {
            if (this.container.ContainsKey(typeof(TRepalce)))
            {
                this.container.Remove(typeof(TRepalce));
            }

            var meta = new TransferEntityMeta<TEntity> { Description = description };
            this.container.Add(typeof(TEntity), meta);

            return new TransferEntityMetaBuilder<TEntity>(meta);
        }
    }
}