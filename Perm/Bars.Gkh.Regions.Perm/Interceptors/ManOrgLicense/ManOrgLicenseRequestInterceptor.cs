namespace Bars.Gkh.Regions.Perm.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class ManOrgLicenseRequestInterceptor : EmptyDomainInterceptor<ManOrgLicenseRequest>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ManOrgLicenseRequest> service, ManOrgLicenseRequest entity)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            if (entity.Type == LicenseRequestType.ExtractFromRegisterLicense)
            {
                var licenseDomain = this.Container.Resolve<IDomainService<ManOrgLicense>>();
                using (this.Container.Using(licenseDomain))
                {
                    var license = licenseDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == entity.Contragent.Id && !x.DateTermination.HasValue);
                    entity.ManOrgLicense = license;
                }
            }

            this.CreateNumber(service, entity);

            if (entity.RegisterNum.ToString() != entity.RegisterNumber)
            {
                entity.RegisterNumber = entity.RegisterNum.ToString();
            }

            return this.ValidationNumber(service, entity)
                ? this.Failure($"Обращение с номером {entity.RegisterNum} уже существует")
                : this.Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ManOrgLicenseRequest> service, ManOrgLicenseRequest entity)
        {
            this.CreateNumber(service, entity);

            if (entity.RegisterNum.ToString() != entity.RegisterNumber)
            {
                entity.RegisterNumber = entity.RegisterNum.ToString();
            }

            return this.ValidationNumber(service, entity)
                ? this.Failure($"Обращение с номером {entity.RegisterNum} уже существует")
                : this.Success();
        }

        private void CreateNumber(IDomainService<ManOrgLicenseRequest> service, ManOrgLicenseRequest entity)
        {
            if (entity.RegisterNum.HasValue && entity.RegisterNum.Value > 0)
            {
                return;
            }

            var num = service.GetAll()
                .Where(x => x.Id != entity.Id)
                .Select(x => x.RegisterNum ?? 0)
                .SafeMax(x => x);

            entity.RegisterNum = num + 1;
        }

        private bool ValidationNumber(IDomainService<ManOrgLicenseRequest> service, ManOrgLicenseRequest entity)
        {
            return service.GetAll().Any(x => x.Id != entity.Id && entity.RegisterNum == x.RegisterNum);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ManOrgLicenseRequest> service, ManOrgLicenseRequest entity)
        {
            var requestPersonDomain = this.Container.Resolve<IDomainService<ManOrgRequestPerson>>();
            var requestProvDocDomain = this.Container.Resolve<IDomainService<ManOrgRequestProvDoc>>();
            var licenseDomain = this.Container.Resolve<IDomainService<ManOrgLicense>>();

            try
            {
                if (licenseDomain.GetAll().Any(x => x.Request.Id == entity.Id))
                {
                    return this.Failure("По данному запросу имеется лицензия");
                }

                requestPersonDomain.GetAll().Where(x => x.LicRequest.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => requestPersonDomain.Delete(x));

                requestProvDocDomain.GetAll().Where(x => x.LicRequest.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => requestProvDocDomain.Delete(x));

                return this.Success();
            }
            catch (Exception exc)
            {
                return this.Failure("Не удалось удалить связанные записи " + exc.Message);
            }
            finally
            {
                this.Container.Release(requestPersonDomain);
                this.Container.Release(requestProvDocDomain);
                this.Container.Release(requestProvDocDomain);
            }
        }
    }
}