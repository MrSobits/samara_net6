namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ServiceJuridalGjiService : IServiceJuridalGjiService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddKindWorkNotification(BaseParams baseParams)
        {
            try
            {
                var businessId = baseParams.Params.GetAs<long>("buisnesId");
                var workIdsStr = baseParams.Params.GetAs("workIds", string.Empty);
                var workIds = !string.IsNullOrEmpty(workIdsStr) ? workIdsStr.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

                if (businessId > 0 && workIds.Length > 0)
                {
                    var serviceViolationFeatureGji = Container.Resolve<IDomainService<ServiceJuridicalGji>>();
                    var serviceKindWorkNotifGji = Container.Resolve<IDomainService<KindWorkNotifGji>>();
                    var serviceBusinessActivity = Container.Resolve<IDomainService<BusinessActivity>>();

                    var listObjects = new List<long>();

                    listObjects.AddRange(
                        serviceViolationFeatureGji.GetAll()
                            .Where(x => x.BusinessActivityNotif.Id == businessId)
                            .Select(x => x.KindWorkNotif.Id)
                            .Distinct()
                            .ToList());

                    foreach (var newId in workIds.Where(x => !listObjects.Contains(x)))
                    {
                        var newViolationFeatureGji = new ServiceJuridicalGji
                        {
                            KindWorkNotif = serviceKindWorkNotifGji.Load(newId),
                            BusinessActivityNotif = serviceBusinessActivity.Load(businessId)
                        };

                        serviceViolationFeatureGji.Save(newViolationFeatureGji);
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }
    }
}