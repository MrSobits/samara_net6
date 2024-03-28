namespace Bars.Gkh.DomainService
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;

    /// <summary>
    /// Сервис получения информации об изменении полей сущности
    /// </summary>
    public interface IEntityChangeLog
    {
        /// <summary>
        /// Списко изменений для сещности
        /// </summary>
        /// <param name="baseParams">
        /// Параметры запроса:<para />
        ///     id - идентификатор сущности <see cref="PersistentObject.Id"/><para />
        ///     entityType - полное наименование типа сущности <see cref="Type.FullName"/>
        /// </param>
        IDataResult List(BaseParams baseParams);
    }

    /// <summary>
    /// Сервис получения информации об изменении полей сущности зависимой от другой сущности
    /// </summary>
    public interface IInheritEntityChangeLog : IEntityChangeLog
    {
        /// <summary>
        /// Код сущности <see cref="Type.Name"/>
        /// </summary>
        string Code { get; }
    }
}