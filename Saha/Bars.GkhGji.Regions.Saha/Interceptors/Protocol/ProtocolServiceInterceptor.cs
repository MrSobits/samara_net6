namespace Bars.GkhGji.Regions.Saha.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Saha.Entities;

    using Bars.B4.Utils;
    using Bars.Gkh.Domain;

    public class ProtocolServiceInterceptor : GkhGji.Interceptors.ProtocolServiceInterceptor
    {
        public override IDataResult BeforeDeleteAction(IDomainService<Protocol> service, Protocol entity)
        {
            var violGroupDomain = Container.Resolve<IDomainService<DocumentViolGroup>>();
            var physPersonInfoDomain = Container.Resolve<IDomainService<DocumentGJIPhysPersonInfo>>();

            try
            {
                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return result;
                }

                this.Container.InTransaction(() =>
                {
                    // удаляем инфомацию по физ лицу
                    physPersonInfoDomain.GetAll()
                        .Where(x => x.Document.Id == entity.Id)
                        .Select(x => x.Id)
                        .ForEach(x => physPersonInfoDomain.Delete(x));

                    // удаляем связку документа с группой нарушений
                    violGroupDomain.GetAll()
                        .Where(x => x.Document.Id == entity.Id)
                        .Select(item => item.Id)
                        .ToList()
                        .ForEach(x => violGroupDomain.Delete(x));
                });

                return Success();
            }
            finally 
            {
                Container.Release(violGroupDomain);
                Container.Release(physPersonInfoDomain);
            }
            
        }
    }
}
