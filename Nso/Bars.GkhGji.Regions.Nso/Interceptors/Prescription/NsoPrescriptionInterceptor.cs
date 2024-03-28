using System.Linq;
using Bars.GkhGji.Interceptors;
using Bars.GkhGji.Regions.Nso.Entities;

namespace Bars.GkhGji.Regions.Nso.Interceptors
{
    using B4;

    using Bars.B4.Utils;

    public class NsoPrescriptionInterceptor : PrescriptionInterceptor<NsoPrescription>
    {
        public override IDataResult BeforeCreateAction(IDomainService<NsoPrescription> service, NsoPrescription entity)
        {
            if (entity.Contragent != null)
                entity.Contragent = entity.Inspection.Contragent;

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<NsoPrescription> service, NsoPrescription entity)
        {

            var baseDocDomain = Container.Resolve<IDomainService<PrescriptionBaseDocument>>();
            var activityDirectionDomain = Container.Resolve<IDomainService<PrescriptionActivityDirection>>();

            try
            {
                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return Failure(result.Message);
                }

                //удаляем дочерние таблицы относящиеся к Предписанию и расположенные в модуле НСО
                baseDocDomain.GetAll()
                    .Where(x => x.Prescription.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => baseDocDomain.Delete(x));

                activityDirectionDomain.GetAll()
                    .Where(x => x.Prescription.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => activityDirectionDomain.Delete(x));

                return result;
            }
            finally 
            {
                Container.Release(baseDocDomain);
                Container.Release(activityDirectionDomain);
            }
        }

    }
}
