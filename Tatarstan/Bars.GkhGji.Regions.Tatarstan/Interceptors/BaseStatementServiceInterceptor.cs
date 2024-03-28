namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System.Linq;

    using Bars.B4;

    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Interceptors;

    public class BaseStatementServiceInterceptor : BaseStatementServiceInterceptor<BaseStatement>
    {

        public override IDataResult BeforeDeleteAction(IDomainService<BaseStatement> service, BaseStatement entity)
        {
            var domainStatAppeal = this.Container.Resolve<IDomainService<InspectionAppealCits>>();
            var documentsDomain = this.Container.Resolve<IDomainService<InspectionGjiDocumentGji>>();

            try
            {
                var statAppealIds = domainStatAppeal.GetAll()
                    .Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();

                var inspectionIds = documentsDomain.GetAll()
                    .Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();

                // Удаляем все дочерние обращения
                foreach (var id in statAppealIds)
                {
                    domainStatAppeal.Delete(id);
                }

                // Удаляем связь с проверками на основании обращения 
                foreach (var id in inspectionIds)
                {
                    documentsDomain.Delete(id);
                }

                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                this.Container.Release(domainStatAppeal);
                this.Container.Release(documentsDomain);
            }
        }
    }
}