// Bars.B4.FileStorageWindsorHelper
using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.IoC;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.FileStorage.DomainService;
using Bars.B4.Windsor;
using Bars.Gkh.DomainService;
using Castle.Windsor;

/// <summary>
/// Помошник для регистрации имплементаций для FileStorage
/// </summary>
public static class FileStorageWindsorHelperNew
{
    /// <summary>
    /// Метод регистрации FileStorageController-а.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="container"></param>
    public static void RegisterFileStorageDataController<T>(this IWindsorContainer container) where T : class, IEntity
    {
        WindsorHelper.RegisterController<FileStorageDataController<T>>(container);
    }

    /// <summary>Зарегистрировать имплементацию FileStorageDomainService</summary>
    /// <typeparam name="TEntity">Сущность</typeparam>
    /// <param name="container">Контайнер</param>
    /// <param name="name">Имя для регистрации</param>
    public static void RegisterFileStorageDomainServiceNew<TEntity>(this IWindsorContainer container, string name = null) where TEntity : class, IEntity
    {
        container.RegisterTransient<IDomainService<TEntity>, NewFileStorageDomainService<TEntity>>(name);
    }
}