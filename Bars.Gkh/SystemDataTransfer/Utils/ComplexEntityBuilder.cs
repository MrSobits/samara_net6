using Bars.Gkh.SystemDataTransfer.Meta;

namespace Bars.Gkh.SystemDataTransfer.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils.Annotations;

    /// <summary>
    /// Генератор типа от базового
    /// </summary>
    /// <typeparam name="TBase"></typeparam>
    public class ComplexEntityBuilder<TBase> : TypeBuilderBase
        where TBase : class, IEntity
    {
        private TransferEntityMeta<TBase> meta;

        /// <inheritdoc />
        public override string TypeSignature => $"{typeof(TBase).Name}ComplexCacheDto";

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="meta">Мета-описание</param>
        public ComplexEntityBuilder(ITransferEntityMeta meta)
        {
            ArgumentChecker.IsType<TransferEntityMeta<TBase>>(meta, nameof(meta));
            this.meta = (TransferEntityMeta<TBase>)meta;
        }

        protected override TypeBuilder GetTypeBuilder()
        {
            var tb = base.GetTypeBuilder();
            tb.AddInterfaceImplementation(typeof(IComplexImportEntity));

            return tb;
        }

        /// <inheritdoc />
        protected override IDictionary<string, Type> GetProperties()
        {
            return typeof(IComplexImportEntity).GetProperties()
                .Union(this.meta.GetExportableProperties())
                .ToDictionary(x => x.Name, x => x.PropertyType);
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
    }
}