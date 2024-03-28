namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using Entities;
    using System.Linq;
    using System;
    using Bars.GkhGji.Interceptors;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.SurveyPlan;
    using Bars.GkhGji.Utils;
    using GkhGji.Enums;
    using Gkh.Enums;
    using B4.Modules.States;

    public class BaseLicenseReissuanceInterceptor : InspectionGjiInterceptor<BaseLicenseReissuance>
    {
        public override IDataResult BeforeCreateAction(IDomainService<BaseLicenseReissuance> service, BaseLicenseReissuance entity)
        {
            var maxNum = service.GetAll()
                .Where(x => x.TypeBase == entity.TypeBase)
                .Select(x => x.InspectionNum).Max();

            entity.InspectionNum = maxNum.ToInt() + 1;
            entity.InspectionNumber = entity.InspectionNum.ToStr();
            entity.ObjectCreateDate = DateTime.Now;
            entity.ObjectEditDate = DateTime.Now;
            entity.ObjectVersion = 1;
            entity.PersonInspection = PersonInspection.Organization;
            entity.TypeJurPerson = TypeJurPerson.ManagingOrganization;
            entity.Contragent = entity.LicenseReissuance.Contragent;

            var stateProvider = Container.Resolve<IStateProvider>();
            try
            {
                stateProvider.SetDefaultState(entity);

                return Success();
            }
            catch
            {
                return Failure("Для проверки не задан начальный статус");
            }
            finally
            {
                Container.Release(stateProvider);
            }
            // entity.State=;
            return Success();


        }
    }

}
