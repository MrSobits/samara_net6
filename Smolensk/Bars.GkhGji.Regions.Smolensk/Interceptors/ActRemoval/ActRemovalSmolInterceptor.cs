namespace Bars.GkhGji.Regions.Smolensk.Interceptors
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;
    using GkhGji.Entities;
    using GkhGji.Interceptors;

    public class ActRemovalSmolInterceptor : DocumentGjiInterceptor<ActRemovalSmol>
    {
        /// <summary>
        /// Метод вызывается перед удалением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeDeleteAction(IDomainService<ActRemovalSmol> service, ActRemovalSmol entity)
        {
            var violGroupDomain = Container.Resolve<IDomainService<DocumentViolGroup>>();
            var domainServiceViol = Container.Resolve<IDomainService<ActRemovalViolation>>();

            try
            {
                // удаляем связку документа с группой нарушений
                violGroupDomain.GetAll()
                    .Where(x => x.Document.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => violGroupDomain.Delete(x));

                domainServiceViol.GetAll()
                    .Where(x => x.Document.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => domainServiceViol.Delete(x));

                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                Container.Release(violGroupDomain);
            }
        }
    }
}