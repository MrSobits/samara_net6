namespace Bars.GkhGji.Regions.Smolensk.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    using Bars.B4.Utils;

    public class PrescriptionInterceptor : GkhGji.Interceptors.PrescriptionInterceptor
    {
        public override IDataResult BeforeDeleteAction(IDomainService<GkhGji.Entities.Prescription> service, GkhGji.Entities.Prescription entity)
        {
            var violGroupDomain = Container.Resolve<IDomainService<DocumentViolGroup>>();

            try
            {
                // удаляем связку документа с группой нарушений
                violGroupDomain.GetAll()
                               .Where(x => x.Document.Id == entity.Id)
                               .Select(x => x.Id)
                               .ForEach(x => violGroupDomain.Delete(x));

                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                Container.Release(violGroupDomain);
            }
            
        }
    }
}
