namespace Bars.GkhGji.Regions.Voronezh.Interceptors
{
    using System;
    using System.Collections.Generic;
    using Bars.B4.Modules.States;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Entities;
    using System.Linq;
    using Bars.Gkh.Enums;

    class LicenseReissuanceInterceptor : EmptyDomainInterceptor<LicenseReissuance>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<ManOrgLicense> ManOrgLicenseDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<LicenseReissuance> service, LicenseReissuance entity)
        {

            if (entity.Contragent != null && entity.Contragent.Id != 0)
            {
                if (entity.State == null)
                {
                    var stateProvider = Container.Resolve<IStateProvider>();
                    stateProvider.SetDefaultState(entity);
                }

                var manOrgLicense = ManOrgLicenseDomain.GetAll()
                    .Where(X => X.Contragent == entity.Contragent)
                    .Where(x => x.State.Name == "Выдана")
                    .FirstOrDefault();
                if (manOrgLicense != null)
                {
                    entity.ManOrgLicense = manOrgLicense;
                }
                return Success();
                //if (manOrgLicense != null)
                //{
                //    entity.ManOrgLicense = manOrgLicense;
                //    var stateProvider = Container.Resolve<IStateProvider>();
                //    try
                //    {
                //        stateProvider.SetDefaultState(entity);
                //        if (entity.RegisterNum.ToString() != entity.RegisterNumber)
                //        {
                //            entity.RegisterNumber = entity.RegisterNum.ToString();
                //        }
                //        return Success();
                //    }
                //    catch
                //    {
                //        return Failure("Для обращения не задан начальный статус");
                //    }
                //    finally
                //    {
                //        Container.Release(stateProvider);
                //    }
                //    // entity.State=;
                //    return Success();
                //}

            }
            else
            {
                return Failure("Не выбран лицензиат");
            }

        }
        public override IDataResult BeforeUpdateAction(IDomainService<LicenseReissuance> service, LicenseReissuance entity)
        {
            if (entity.RegisterNum.ToString() != entity.RegisterNumber)
            {
                entity.RegisterNumber = entity.RegisterNum.ToString();
            }
          
            return Success();
        }
    }
}
