namespace Bars.Gkh.Regions.Perm.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;

    public class ManOrgLicenseInterceptor : EmptyDomainInterceptor<ManOrgLicense>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ManOrgLicense> service, ManOrgLicense entity)
        {
            if (entity.State.IsNull())
            {
                var stateProvider = this.Container.Resolve<IStateProvider>();
                stateProvider.SetDefaultState(entity);
            }

            return this.CreateNumber(service, entity);
        }

        private IDataResult CreateNumber(IDomainService<ManOrgLicense> service, ManOrgLicense entity)
        {
            if (!entity.LicNum.HasValue)
            {
                var lastNum = service.GetAll()
                    .Where(x => x.LicNum.HasValue && x.Id != entity.Id)
                    .Select(x => x.LicNum.Value)
                    .SafeMax(x => x);

                entity.LicNum = lastNum + 1;
                entity.LicNumber = $"{entity.LicNum:D6}";
            }

            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ManOrgLicense> service, ManOrgLicense entity)
        {
            var docDomain = this.Container.Resolve<IDomainService<ManOrgLicenseDoc>>();

            try
            {
                docDomain.GetAll().Where(x => x.ManOrgLicense.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => docDomain.Delete(x));

                return this.Success();
            }
            catch (Exception exc)
            {
                return this.Failure("Не удалось удалить связанные записи " + exc.Message);
            }
            finally
            {
                this.Container.Release(docDomain);
            }
        }
    }
}