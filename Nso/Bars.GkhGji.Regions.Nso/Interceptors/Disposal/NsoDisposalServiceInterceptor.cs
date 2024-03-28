namespace Bars.GkhGji.Regions.Nso.Interceptors
{
    using System.Linq;

    using B4;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Nso.Entities;
    using Bars.GkhGji.Regions.Nso.Entities.Disposal;
     
    // интерцептор необходимо тоже перекрыть поскольку сущность NsoDisposal должна выполнить базовый код интерцептора Disposal
    public class NsoDisposalServiceInterceptor : DisposalServiceInterceptor<NsoDisposal>
    {
        public override IDataResult BeforeCreateAction(IDomainService<NsoDisposal> service, NsoDisposal entity)
        {
            entity.NcObtained = YesNo.No;
            entity.NcSent = YesNo.No;

            return base.BeforeCreateAction(service, entity);
        }
        
        public override IDataResult BeforeDeleteAction(IDomainService<NsoDisposal> service, NsoDisposal entity)
        {
            // Получаем предметы проверки и удаляем их
            var serviceSubj = Container.Resolve<IDomainService<DisposalVerificationSubject>>();
            var serviceDocs = Container.Resolve<IDomainService<DisposalDocConfirm>>();
            var serviceLongText = Container.Resolve<IDomainService<DisposalLongText>>();
            var factViols = Container.ResolveDomain<DisposalFactViolation>();

            try
            {
                // удаляем субтаблицы добавленные в регионе НСО

                serviceSubj.GetAll()
                .Where(x => x.Disposal.Id == entity.Id)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => serviceSubj.Delete(x));

                serviceDocs.GetAll()
                .Where(x => x.Disposal.Id == entity.Id)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => serviceDocs.Delete(x));

                serviceLongText.GetAll()
                .Where(x => x.Disposal.Id == entity.Id)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => serviceLongText.Delete(x));

                factViols.GetAll()
                    .Where(x => x.Disposal.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList()
                    .ForEach(x => factViols.Delete(x));

                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                Container.Release(serviceSubj);
                Container.Release(serviceDocs);
                Container.Release(serviceLongText);
                Container.Release(factViols);
            }
            
        }

    }
}

