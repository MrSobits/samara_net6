using System.Collections;
using System.Collections.Generic;
using Bars.Gkh.Enums;

namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class AppealCitsRealObjService : IAppealCitsRealObjService
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<AppealCits> AppealCitsService { get; set; }
        public IDomainService<MKDLicRequestRealityObject> MKDLicRequestRealityObjectService { get; set; }
        public IDomainService<AppealCitsRealityObject> AppealCitsRealityObjectService { get; set; }
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectService { get; set; }
        public IDomainService<ServiceOrgRealityObjectContract> ServOrgContractRealityObjectService { get; set; }
        public IDomainService<SupplyResourceOrgRealtyObject> SupplyResourceOrgRealtyObjectService { get; set; }

        public IDataResult AddRealityObjects(BaseParams baseParams)
        {
            try
            {
                var appealCitizensId = baseParams.Params.ContainsKey("appealCitizensId") ? baseParams.Params["appealCitizensId"].ToLong() : 0;
                var objectIds = baseParams.Params.ContainsKey("objectIds") ? baseParams.Params["objectIds"].ToString() : string.Empty;

                if (!string.IsNullOrEmpty(objectIds))
                {
                    var service = Container.Resolve<IDomainService<AppealCitsRealityObject>>();

                    // получаем дома что бы не добавлять их повторно
                    var listObjects =
                        service.GetAll()
                            .Where(x => x.AppealCits.Id == appealCitizensId)
                            .Select(x => x.RealityObject.Id)
                            .ToList();

                    foreach (var item in objectIds.Split(','))
                    {

                        long newId;
                        if (!long.TryParse(item, out newId))
                        {
                            continue;
                        }

                        var newAppealCitizensGjiRealityObject = new AppealCitsRealityObject
                            {
                                RealityObject = new RealityObject { Id = newId },
                                AppealCits = new AppealCits { Id = appealCitizensId }
                            };

                        service.Save(newAppealCitizensGjiRealityObject);
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult {Success = false, Message = e.Message};
            }
        }

        public IDataResult GetRealityObjects(BaseParams baseParams)
        {
            var appealCitizensId =  baseParams.Params.GetAs<long>("appealCitizensId");

            var data = AppealCitsRealityObjectService.GetAll()
                .Where(x => x.AppealCits.Id == appealCitizensId)
                .Select(x => new
                {
                    x.RealityObject.Id, 
                    x.RealityObject.Address
                })
                .ToArray();

            return new BaseDataResult(data);
        }

        public IDataResult AddStatementRealityObjects(BaseParams baseParams)
        {
            try
            {
                var mkdlicrequestId = baseParams.Params.ContainsKey("mkdlicrequestId") ? baseParams.Params["mkdlicrequestId"].ToLong() : 0;
                var objectIds = baseParams.Params.ContainsKey("objectIds") ? baseParams.Params["objectIds"].ToString() : string.Empty;

                if (!string.IsNullOrEmpty(objectIds))
                {
                    
                    // получаем дома что бы не добавлять их повторно
                    var listObjects =
                        MKDLicRequestRealityObjectService.GetAll()
                            .Where(x => x.MKDLicRequest.Id == mkdlicrequestId)
                            .Select(x => x.RealityObject.Id)
                            .ToList();

                    foreach (var item in objectIds.Split(','))
                    {

                        long newId;
                        if (!long.TryParse(item, out newId))
                        {
                            continue;
                        }

                        var newMKDLicRequestRealityObject = new MKDLicRequestRealityObject
                        {
                            RealityObject = new RealityObject { Id = newId },
                            MKDLicRequest = new MKDLicRequest { Id = mkdlicrequestId }
                        };

                        MKDLicRequestRealityObjectService.Save(newMKDLicRequestRealityObject);
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }


        public IDataResult GetJurOrgs(BaseParams baseParams)
        {
            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            var citizenAppeal = AppealCitsService.GetAll().FirstOrDefault(x => x.Id == appealCitizensId);
	        
            if (realityObjectId == 0)
            {
                realityObjectId = AppealCitsRealityObjectService.GetAll()
	                .Where(x => x.AppealCits.Id == appealCitizensId)
	                .Select(x => x.RealityObject.Id)
	                .FirstOrDefault();
            }

            if (realityObjectId != 0)
            {
                var data = ManOrgContractRealityObjectService.GetAll()
                    .Where(x => x.ManOrgContract.StartDate <= citizenAppeal.DateFrom)
                    .Where(
                        x =>
                            x.ManOrgContract.EndDate == null ||
                            x.ManOrgContract.EndDate >= citizenAppeal.DateFrom)
                    .Where(x => x.RealityObject.Id == realityObjectId)
                    .Select(x => new
                    {
                        x.ManOrgContract.ManagingOrganization.Contragent.Id,
                        x.ManOrgContract.ManagingOrganization.Contragent.Name
                    }).ToArray();

                return new BaseDataResult(data);
            }

            return new BaseDataResult();
        }

    }
}