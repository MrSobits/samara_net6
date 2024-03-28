using Bars.B4.Modules.States;
using Bars.Gkh.Domain.CollectionExtensions;

namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;

    using B4;

    using Bars.B4.Utils;

    using Entities;

    public class ManOrgLicenseRequestInterceptor : EmptyDomainInterceptor<ManOrgLicenseRequest>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ManOrgLicenseRequest> service, ManOrgLicenseRequest entity)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            if (entity.State == null)
            {
                stateProvider.SetDefaultState(entity);
            }
            
            CreateNumber(service, entity);

            if (entity.RegisterNum.ToString() != entity.RegisterNumber)
            {
                entity.RegisterNumber = entity.RegisterNum.ToString();    
            }

            return ValidationNumber(service, entity) ? Failure(string.Format("Обращение с номером {0} уже существует", entity.RegisterNum)) : Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ManOrgLicenseRequest> service, ManOrgLicenseRequest entity)
        {
            CreateNumber(service, entity);

            if (entity.RegisterNum.ToString() != entity.RegisterNumber)
            {
                entity.RegisterNumber = entity.RegisterNum.ToString();
            }

            return ValidationNumber(service, entity) ? Failure(string.Format("Обращение с номером {0} уже существует", entity.RegisterNum)) : Success();
        }

        private void CreateNumber(IDomainService<ManOrgLicenseRequest> service, ManOrgLicenseRequest entity)
        {
            if (entity.RegisterNum.HasValue && entity.RegisterNum.Value > 0)
            {
                return;
            }

            var num = service.GetAll()
                .Where(x => x.Id != entity.Id)
                .Select(x => x.RegisterNum.HasValue ? x.RegisterNum.Value : 0)
                .SafeMax(x => x);

            entity.RegisterNum = num + 1;
        }

        private bool ValidationNumber(IDomainService<ManOrgLicenseRequest> service, ManOrgLicenseRequest entity)
        {
            return service.GetAll().Where(x => x.Id != entity.Id && entity.RegisterNum == x.RegisterNum).Any();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ManOrgLicenseRequest> service, ManOrgLicenseRequest entity)
        {
            var requestPersonDomain = Container.Resolve<IDomainService<ManOrgRequestPerson>>();
            var requestProvDocDomain = Container.Resolve<IDomainService<ManOrgRequestProvDoc>>();
            var licenseDomain = Container.Resolve<IDomainService<ManOrgLicense>>();

            try
            {
                if (licenseDomain.GetAll().Any(x => x.Request.Id == entity.Id))
                {
                    return Failure("По данному запросу имеется лицензия");
                }

                requestPersonDomain.GetAll().Where(x => x.LicRequest.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => requestPersonDomain.Delete(x));

                requestProvDocDomain.GetAll().Where(x => x.LicRequest.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => requestProvDocDomain.Delete(x));

                return Success();
            }
            catch (Exception exc)
            {
                return Failure("Не удалось удалить связанные записи "+ exc.Message);
            }
            finally
            {
                Container.Release(requestPersonDomain);
                Container.Release(requestProvDocDomain);
                Container.Release(requestProvDocDomain);
            }
        }
    }
}