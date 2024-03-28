namespace Bars.Gkh.SystemDataTransfer.Meta
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4.DataAccess;
    using Bars.Gkh.SystemDataTransfer.Meta.ProviderMeta;
    
    using Castle.Windsor;

    public abstract class AbstractDataSerializer<TEntity> : IDataSerializer<TEntity>
        where TEntity : IEntity
    {
        /// <inheritdoc />
        public virtual IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public abstract object Serialize(TEntity entity);
        /// <inheritdoc />
        public abstract IEntity Deserializer(IEntity saveEntity, Item item, Stream stream);

        /// <inheritdoc />
        public abstract void Flush(long id, Stream stream);

        /// <inheritdoc />
        public abstract string GetFileName(long id);

        /// <inheritdoc />
        public virtual bool HasComplexProperty => false;

        /// <inheritdoc />
        public virtual object Serialize(object entity)
        {
            return this.Serialize((TEntity)entity);
        }

        /// <inheritdoc />
        public virtual Func<IQueryable, IQueryable> QueryModifier()
        {
            return x => this.EntitySelector((IQueryable<TEntity>)x);
        }

        /// <inheritdoc />
        public virtual IQueryable EntitySelector(IQueryable<TEntity> query)
        {
            return null;
        }

        /// <summary>
        /// Метод возвращает селектор для получение ComplexKey
        /// </summary>
        /// <returns></returns>
        public virtual Expression GetEntitySelectExpression(ParameterExpression expression)
        {
            return null;
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
        }
    }

    public interface IDataSerializer<in TEntity> : IDataSerializer, IDisposable where TEntity : IEntity
    {
        /// <summary>
        /// Сериализовать сущность (метод может модифицировать <paramref name="entity"/>)
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <returns>Новый объект</returns>
        object Serialize(TEntity entity);

        /// <summary>
        /// Селектор проекции
        /// </summary>
        IQueryable EntitySelector(IQueryable<TEntity> query);
    }

    /// <summary>
    /// Сериализатор дополнительной информации в файл
    /// </summary>
    public interface IDataSerializer
    {
        /// <summary>
        /// Свойство указывает, что сериализуемая сущность имеет дополнительный ключ
        /// </summary>
        bool HasComplexProperty { get; }

        /// <summary>
        /// Сериализовать сущность (метод может модифицировать <paramref name="entity"/>)
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <returns>Новый объект</returns>
        object Serialize(object entity);

        /// <summary>
        /// Контейнер
        /// </summary>
        IWindsorContainer Container { set; }

        /// <summary>
        /// Сбросить файл в поток
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="stream">Целевой поток</param>
        void Flush(long id, Stream stream);

        /// <summary>
        /// Вернуть имя файла
        /// </summary>
        /// <param name="id">Id</param>
        string GetFileName(long id);

        /// <summary>
        /// Десерилиазовать
        /// </summary>
        /// <param name="saveEntity"></param>
        /// <param name="item">Сущность</param>
        /// <param name="stream">Поток</param>
        IEntity Deserializer(IEntity saveEntity, Item item, Stream stream);

        /// <summary>
        /// Селектор проекции
        /// </summary>
        Func<IQueryable, IQueryable> QueryModifier();
    }
}