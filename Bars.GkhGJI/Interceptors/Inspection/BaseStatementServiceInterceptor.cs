namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Contracts;

    using Bars.GkhGji.Entities;

    public class BaseStatementServiceInterceptor : BaseStatementServiceInterceptor<BaseStatement>
    {
    }

    public class BaseStatementServiceInterceptor<T> : InspectionGjiInterceptor<T>
        where T: BaseStatement
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            var serviceNumberRule = Container.Resolve<IBaseStatementNumberRule>();
            try
            {
                // Перед сохранением формируем номер основания проверки
                serviceNumberRule.SetNumber(entity);

                return base.BeforeCreateAction(service, entity);
            }
            finally
            {
                Container.Release(serviceNumberRule);
            }
           
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            // Удаляем все дочерние обращения
            var domainStatAppeal = Container.Resolve<IDomainService<InspectionAppealCits>>();
            try
            {
                var statAppealIds = domainStatAppeal.GetAll()
                    .Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();

                foreach (var id in statAppealIds)
                {
                    domainStatAppeal.Delete(id);
                }

                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                Container.Release(domainStatAppeal);
            }
        }
    }
}