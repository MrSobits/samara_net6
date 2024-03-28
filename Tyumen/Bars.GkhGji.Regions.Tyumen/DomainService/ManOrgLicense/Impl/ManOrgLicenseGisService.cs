

namespace Bars.GkhGji.Regions.Tyumen.DomainService
{
    using System.Collections;
    using System.Linq;
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.Gkh.DomainService;
    using Castle.Windsor;
    using System;
    using Enums;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Regions.Tyumen.Entities;

    public class ManOrgLicenseGisService : IManOrgLicenseGisService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ManOrgContractRealityObject> MorgRODomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<ProtocolArticleLaw> ProtocolArtLawDomain { get; set; }

        public IDomainService<Prescription> PrescriptionDomain { get; set; }

        public IDomainService<ManagingOrganization> ManagingOrganizationDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<InspectionGjiRealityObject> InspectionGjiRealityObjectDomain { get; set; }

        public IDomainService<ManOrgLicenseRequest> RequestDomain { get; set; }

        public IDomainService<LicensePrescription> LicensePrescriptionDomain { get; set; }

        public IDomainService<ManOrgLicense> LicenseDomain { get; set; }

        public IGkhUserManager UserManager { get; set; }


        public IList GetListWithRO(BaseParams baseParams, bool isPaging, ref int totalCount)
        {
          

            var contragentList = UserManager.GetContragentIds();
          
            var param2AndMore = baseParams.Params.GetAs<bool>("cbShowOnly2andMore");
            var showall = baseParams.Params.GetAs<bool>("showall");

            Dictionary<long, int> prescrCount = new Dictionary<long, int>();
            LicensePrescriptionDomain.GetAll()
                .WhereIf(!showall, x => x.DocumentDate >= DateTime.Now.AddYears(-1)).ToList().ForEach(
                x =>
                {
                    if (!prescrCount.ContainsKey(x.MorgContractRO.Id))
                    {
                        prescrCount.Add(x.MorgContractRO.Id, 1);
                    }
                    else
                    {
                        prescrCount[x.MorgContractRO.Id]++;
                    }
                }
            );

            if (param2AndMore)
            {
                prescrCount = prescrCount.Where(x => x.Value >= 2).ToDictionary(x=> x.Key, x=> x.Value);
            }

            var showEnded = baseParams.Params.GetAs<bool>("showEnded");
            if (!showall)
            {
                
                var morgContract = MorgRODomain.GetAll()
                  .WhereIf(!showEnded, x => (x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= DateTime.Now)
                  && x.ManOrgContract.ManagingOrganization.Contragent.Id > 0)
                 .WhereIf(param2AndMore, x => prescrCount.Any(y=> y.Key == x.Id))
                  .Select(x => new
                  {
                      Id = x.Id,
                      mcId = x.Id,
                      postCount = prescrCount.ContainsKey(x.Id)? prescrCount[x.Id]: 0,
                      //   MgContrId = (long?)x.ManOrgContract.ManagingOrganization.Contragent.Id,
                      MgContrId = x.ManOrgContract.ManagingOrganization.Contragent.Id,
                      x.ManOrgContract.StartDate,
                      x.ManOrgContract.EndDate,
                      roId = x.RealityObject.Id,
                      x.RealityObject.Address,
                      x.RealityObject.Municipality.Name
                  });
                

                var data = LicenseDomain.GetAll()
               .Where(x => x.Contragent.Id > 0)
               .Select(x => new
               {
                   x.Id,
                   x.State,
                   x.LicNum,
                   StateName = x.State.Name,
                   x.DateIssued,
                   Request = x.Request != null ? x.Request.Id : 0,
                   Contragent = x.Contragent.Name,
                   ContragentId = x.Contragent.Id,
                   ContragentMunicipality = x.Contragent.Municipality.Name,
                   x.Contragent.Inn

               }).AsEnumerable();


                var morgContractEnum = morgContract.Where(x => data.Any(y => y.ContragentId == x.MgContrId)).AsEnumerable();

                var loadParams = baseParams.GetLoadParam();

                var data2 = data.AsEnumerable()
                .Join(morgContractEnum, x => x.ContragentId, y => y.MgContrId, (x, y) => new { x, y })
                .Select(m => new
                {
                    Id = m.y.Id,
                    mcId = m.y.mcId,
                    State = m.x.State,
                    LicNum = m.x.LicNum,
                    DateIssued = m.x.DateIssued,
                    m.y.postCount,
                    Request = m.x.Request != 0 ? m.x.Request : 0,
                    Contragent = m.x.Contragent,
                    ContragentMunicipality = m.x.ContragentMunicipality,
                    ManStartDate = m.y.StartDate,
                    ManEndDate = m.y.EndDate,
                    m.x.StateName,
                    ROAddress = m.y.Address,
                    ROMunicipality = m.y.Name,
                    Inn = m.x.Inn
                    //.Where(z => (z.ContragentId == m.x.ContragentId && z.RoId == m.y.roId))
                    //.Select(z=> z.ResolutionId).ToList().Count

                })
                .AsQueryable()
                .Filter(loadParams, this.Container);


                totalCount = data2.Count();

                if (isPaging)
                {
                    return data2.Order(loadParams).Paging(loadParams).ToList();
                }

                return data2.Order(loadParams).ToList();

            }
            else
            {
               
                var morgContract = MorgRODomain.GetAll()
                  .WhereIf(!showEnded, x => (x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= DateTime.Now)
                  && x.ManOrgContract.ManagingOrganization.Contragent.Id > 0)
                    .WhereIf(param2AndMore, x => prescrCount.ContainsKey(x.Id))
                  .Select(x => new
                  {
                      Id = x.Id,
                      mcId = x.Id,
                      postCount = prescrCount.ContainsKey(x.Id) ? prescrCount[x.Id] : 0,
                      //   MgContrId = (long?)x.ManOrgContract.ManagingOrganization.Contragent.Id,
                      MgContrId = x.ManOrgContract.ManagingOrganization.Contragent.Id,
                      x.ManOrgContract.StartDate,
                      x.ManOrgContract.EndDate,
                      roId = x.RealityObject.Id,
                      x.RealityObject.Address,
                      x.RealityObject.Municipality.Name
                  });
                //    .AsEnumerable();
                // .ToList();

                var data = LicenseDomain.GetAll()
               .Where(x => x.Contragent.Id > 0)
               .Select(x => new
               {
                   x.Id,
                   x.State,
                   x.LicNum,                  
                   StateName = x.State.Name,
                   x.DateIssued,
                   Request = x.Request != null ? x.Request.Id : 0,
                   Contragent = x.Contragent.Name,
                   ContragentId = x.Contragent.Id,
                   ContragentMunicipality = x.Contragent.Municipality.Name,
                   x.Contragent.Inn

               }).AsEnumerable();


                var morgContractEnum = morgContract.Where(x => data.Any(y => y.ContragentId == x.MgContrId)).AsEnumerable();
                var loadParams = baseParams.GetLoadParam();

                var data2 = data.AsEnumerable()
                .Join(morgContractEnum, x => x.ContragentId, y => y.MgContrId, (x, y) => new { x, y })
                .Select(m => new
                {
                    Id = m.y.Id,
                    mcId = m.y.mcId,
                    State = m.x.State,
                    LicNum = m.x.LicNum,
                    DateIssued = m.x.DateIssued,
                    m.y.postCount,
                    Request = m.x.Request != 0 ? m.x.Request : 0,
                    ManEndDate = m.y.EndDate,
                    Contragent = m.x.Contragent,
                    m.x.StateName,
                    ContragentMunicipality = m.x.ContragentMunicipality,
                    ManStartDate = m.y.StartDate,
                    ROAddress = m.y.Address,
                    ROMunicipality = m.y.Name,
                    Inn = m.x.Inn
                    //.Where(z => (z.ContragentId == m.x.ContragentId && z.RoId == m.y.roId))
                    //.Select(z=> z.ResolutionId).ToList().Count

                })
                .AsQueryable()
                .Filter(loadParams, this.Container);


                totalCount = data2.Count();

                if (isPaging)
                {
                    return data2.Order(loadParams).Paging(loadParams).ToList();
                }

                return data2.Order(loadParams).ToList();
            }
        }

        public IList GetRO(BaseParams baseParams, bool isPaging, ref int totalCount)
        {

            var mcId = baseParams.Params.GetAs<long>("id");

            var morgContract = MorgRODomain.Get(mcId);

            var mkdList = MorgRODomain.GetAll()
                .Where(x => x.ManOrgContract.ManagingOrganization == morgContract.ManOrgContract.ManagingOrganization)
                .Where(x => (x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= DateTime.Now))
                .Select(x=> x.RealityObject.Id).Distinct().ToList();

            var mkdCount = mkdList.Count();

            var mkdArea = RealityObjectDomain.GetAll()
                .Where(x => mkdList.Contains(x.Id))
                .Sum(x => x.AreaMkd);

                var data = LicenseDomain.GetAll()
               .Where(x => x.Contragent.Id > 0 && x.Contragent == morgContract.ManOrgContract.ManagingOrganization.Contragent)
               .Select(x => new
               {
                   x.Id,
                   x.State,
                   x.LicNum,
                   StateName = x.State.Name,
                   x.DateIssued,
                   x.DateRegister,
                   x.DateDisposal,
                   x.DisposalNumber,
                   Request = x.Request != null ? x.Request.Id : 0,
                   Contragent = x.Contragent.Name,
                   ContragentId = x.Contragent.Id,
                   ContragentMunicipality = x.Contragent.Municipality.Name,
                   x.Contragent.Inn

               }).AsEnumerable();

            var loadParams = baseParams.GetLoadParam();

            var data2 = data.AsEnumerable()
                .Select(m => new
                {
                    Id = morgContract.Id,
                    m.StateName,
                    mcId = morgContract.Id,
                    State = m.State,
                    LicNum = m.LicNum,
                    DateIssued = m.DateIssued,
                    Contragent = morgContract.ManOrgContract.ManagingOrganization.Contragent.Name,
                    ContragentMunicipality = "",
                    ManStartDate = morgContract.ManOrgContract.StartDate,
                    ManEndDate = morgContract.ManOrgContract.EndDate,
                    ROAddress = morgContract.RealityObject.Address,
                    m.DateRegister,
                    m.DateDisposal,
                    m.DisposalNumber,
                    MkdCount = mkdCount,
                    MKDArea = mkdArea,
                    ROMunicipality = morgContract.RealityObject.Municipality.Name,
                    Inn = morgContract.ManOrgContract.ManagingOrganization.Contragent.Inn
                    //.Where(z => (z.ContragentId == m.x.ContragentId && z.RoId == m.y.roId))
                    //.Select(z=> z.ResolutionId).ToList().Count

                })
                .AsQueryable()
                .Filter(loadParams, this.Container);


                totalCount = data2.Count();

                if (isPaging)
                {
                    return data2.Order(loadParams).Paging(loadParams).ToList();
                }

                return data2.Order(loadParams).ToList();

            
        }
    }
}