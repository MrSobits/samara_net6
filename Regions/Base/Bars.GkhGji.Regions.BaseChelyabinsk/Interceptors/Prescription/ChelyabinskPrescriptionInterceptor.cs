namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.Prescription
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Prescription;

    using Castle.Core.Internal;

    public class ChelyabinskPrescriptionInterceptor : PrescriptionInterceptor<ChelyabinskPrescription>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ChelyabinskPrescription> service, ChelyabinskPrescription entity)
        {
            if (entity.Contragent != null)
                entity.Contragent = entity.Inspection.Contragent;

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ChelyabinskPrescription> service, ChelyabinskPrescription entity)
        {

            var baseDocDomain = this.Container.Resolve<IDomainService<PrescriptionBaseDocument>>();
            var activityDirectionDomain = this.Container.Resolve<IDomainService<PrescriptionActivityDirection>>();

            try
            {
                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return this.Failure(result.Message);
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
                this.Container.Release(baseDocDomain);
                this.Container.Release(activityDirectionDomain);
            }
        }

    }
}
