namespace Bars.Gkh.Regions.Perm.Interceptors
{
    using System.Linq;

    using B4;

    using Entities;


    public class PersonQualificationCertificateInterceptor : EmptyDomainInterceptor<PersonQualificationCertificate>
    {
        public override IDataResult BeforeCreateAction(IDomainService<PersonQualificationCertificate> service, PersonQualificationCertificate entity)
        {
            if (service.GetAll().Any(x => x.Number == entity.Number))
            {
                return this.Failure("Квалификационный аттестат с таким номером уже существует!");
            }

            return this.Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PersonQualificationCertificate> service, PersonQualificationCertificate entity)
        {
            if (service.GetAll().Any(x => x.Number == entity.Number && x.Id != entity.Id))
            {
                return this.Failure("Квалификационный аттестат с таким номером уже существует!");
            }

            return this.Success();
        }
    }
}