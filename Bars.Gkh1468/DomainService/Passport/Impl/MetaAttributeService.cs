namespace Bars.Gkh1468.DomainService.Passport.Impl
{
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4;
    using Bars.Gkh1468.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Реализация интерфейса для работы с метаатрибутами
    /// </summary>
    public class MetaAttributeService : IMetaAttributeService
    {
        /// <summary>Контейнер зависимостей</summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Удаляет метаатрибут
        /// </summary>
        /// <param name="atrId">Идентифкатор атрибута</param>
        /// <param name="domainService">Сервис домена метаатрибутов</param>
        /// <returns>Результат операции</returns>
        public IDataResult RemoveMetaAttribute(long atrId, IDomainService<MetaAttribute> domainService)
        {
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var atrIds = 
                        domainService.GetAll()
                            .Where(x => x.Parent != null && x.Parent.Id == atrId)
                            .Select(x => x.Id)
                            .ToList();

                    foreach (var id in atrIds)
                    {
                        RemoveMetaAttribute(id, domainService);
                    }

                    // Проверяю атрибут на зависимые таблицы
                    var attrHaveDependency = Container.Resolve<IModuleDependencies>("Bars.Gkh1468 dependencies")
                        .CheckAnyDependencies<MetaAttribute>(atrId);
                    if (attrHaveDependency)
                    {
                        return new BaseDataResult
                        {
                            Message = "Удаляемый атрибут содержит ссылки из других таблиц",
                            Success = false
                        };
                    }

                    domainService.Delete(atrId);

                    tr.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    tr.Rollback();
                    return new BaseDataResult { Message = e.Message, Success = false };
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
        }
    }
}
