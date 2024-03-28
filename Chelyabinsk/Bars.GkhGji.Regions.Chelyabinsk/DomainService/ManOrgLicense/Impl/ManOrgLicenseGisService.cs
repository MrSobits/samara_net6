

namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    using System.Collections;
    using System.Linq;
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.Gkh.DomainService;
    using Castle.Windsor;
    using System;
    using Enums;
    using Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    public class ManOrgLicenseGisService : IManOrgLicenseGisService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Person> PersonDomain { get; set; }

        public IDomainService<ManOrgContractRealityObject> MorgRODomain { get; set; }

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<ProtocolArticleLaw> ProtocolArtLawDomain { get; set; }

        public IDomainService<Prescription> PrescriptionDomain { get; set; }

        public IDomainService<ManagingOrganization> ManagingOrganizationDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<InspectionGjiRealityObject> InspectionGjiRealityObjectDomain { get; set; }

        public IDomainService<ManOrgLicenseRequest> RequestDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IDomainService<PublicServiceOrg> PublicServiceOrgDomain { get; set; }

        public IDomainService<ROMCalcTask> ROMCalcTaskDomain { get; set; }

        public IDomainService<ROMCategory> ROMCategoryDomain { get; set; }

        public IDomainService<ManOrgLicense> LicenseDomain { get; set; }

        public IDomainService<SupplyResourceOrg> SupplyResourceOrgDomain { get; set; }

        public IDomainService<ServiceOrganization> ServiceOrganizationDomain { get; set; }

        public IGkhUserManager UserManager { get; set; }

        public IDomainService<PersonPlaceWork> placeWorkDomain { get; set; }

        public IDomainService<PersonQualificationCertificate> qualDomain { get; set; }

        public IDomainService<PersonDisqualificationInfo> disqualDomain { get; set; }

        public IDataResult ListManOrgWithLicenseAndHouseByType(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

           // var contrList = LicenseDomain.GetAll()
           // .Where(x => x.Contragent.Id > 0)
           //.Where(x => x.Contragent.ActivityGroundsTermination != GroundsTermination.Bankruptcy
           //&& x.Contragent.ActivityGroundsTermination != GroundsTermination.Erroneous
           //&& x.Contragent.ActivityGroundsTermination != GroundsTermination.Liquidation
           //&& x.Contragent.ActivityGroundsTermination != GroundsTermination.TerminationMkd)
           //.Select(x => x.Contragent.Id).Distinct().ToList();

            List<long> listContragents = new List<long>();

            listContragents.AddRange(
            SupplyResourceOrgDomain.GetAll().Where(x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
            .Select(x => x.Contragent.Id).ToList());

            listContragents.AddRange(
           ServiceOrganizationDomain.GetAll().Where(x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
           .Select(x => x.Contragent.Id).ToList());

            listContragents.AddRange(
          ManagingOrganizationDomain.GetAll().Where(x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
          .Select(x => x.Contragent.Id).ToList());

            listContragents.AddRange(
         PublicServiceOrgDomain.GetAll().Where(x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
         .Select(x => x.Contragent.Id).ToList());

            var data = ContragentDomain.GetAll()
                .Where(x=> listContragents.Distinct().Contains(x.Id))
                   .Select(x => new
                   {
                       Id = x.Id,
                       ContragentName = x.Name,
                       Municipality = x.Municipality != null ? x.Municipality.Name : "Не указано",
                       JuridicalAddress = x.JuridicalAddress,
                       Inn = x.Inn,
                       ShortName = x.ShortName,
                       KindKND = KindKND.HousingSupervision,
                   }).AsQueryable()
                  .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                   .OrderThenIf(loadParams.Order.Length == 0, true, x => x.JuridicalAddress)
                   .Filter(loadParams, Container);

            //PublicServiceOrgDomain
            //var morgContract = MorgRODomain.GetAll()
            //       .Where(x => x.ManOrgContract.ManagingOrganization != null && x.ManOrgContract.ManagingOrganization.Contragent != null)
            //       .Where(x => x.ManOrgContract.ManagingOrganization.Contragent.Inn != null && x.ManOrgContract.ManagingOrganization.Contragent.Municipality != null)
            //    .Where(x => x.ManOrgContract.StartDate <= DateTime.Now && (x.ManOrgContract.EndDate >= DateTime.Now || x.ManOrgContract.EndDate == null))
            //    .Where(x => (x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.ManagingOrgOwners && contrList.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id))
            //    || !contrList.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id))
            //       .Select(x => x.ManOrgContract.ManagingOrganization.Contragent).Distinct().ToList();

            //var data = morgContract
            //      .Select(x => new
            //      {
            //          Id = x.Id,
            //          ContragentName = x.Name,
            //          Municipality = x.Municipality != null? x.Municipality.Name:"Не указано",
            //          JuridicalAddress = x.JuridicalAddress,
            //          Inn = x.Inn,
            //          ShortName = x.ShortName,
            //          KindKND = KindKND.HousingSupervision,
            //      }).AsQueryable()
            //      .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
            //       .OrderThenIf(loadParams.Order.Length == 0, true, x => x.JuridicalAddress)
            //       .Filter(loadParams, Container);



            // var totalCount = data.Count();
            var totalCount = 1243;//data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public IDataResult ListManOrgWithLicenseAndHouse(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var contrList = LicenseDomain.GetAll()
                .Where(x => x.Contragent.Id > 0)
               .Where(x => x.Contragent.ActivityGroundsTermination != GroundsTermination.Bankruptcy 
               && x.Contragent.ActivityGroundsTermination != GroundsTermination.Erroneous
               && x.Contragent.ActivityGroundsTermination != GroundsTermination.Liquidation
               && x.Contragent.ActivityGroundsTermination != GroundsTermination.TerminationMkd)
               .Select(x=> x.Contragent.Id).Distinct().ToList();

            var morgContract = MorgRODomain.GetAll()
                 .Where(x => x.ManOrgContract.ManagingOrganization != null && x.ManOrgContract.ManagingOrganization.Contragent != null && contrList.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id))
                 .Where(x => x.ManOrgContract.StartDate.HasValue && x.ManOrgContract.StartDate.Value <= DateTime.Now
                 && (x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate.Value >= DateTime.Now))
                 .Select(x => x.ManOrgContract.ManagingOrganization.Contragent).Distinct().ToList();
                 
            var data = morgContract
                .Select(x => new
                {
                    Id = x.Id,
                    ContragentName = x.Name,
                    Municipality = x.Municipality != null? x.Municipality.Name: "Не указано",
                    JuridicalAddress = x.JuridicalAddress,
                    Inn = x.Inn,
                    ShortName = x.ShortName,
                    KindKND = KindKND.LicenseControl,
                }).AsQueryable()
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                 .OrderThenIf(loadParams.Order.Length == 0, true, x => x.JuridicalAddress)
                 .Filter(loadParams, Container);

        //    var totalCount = data.Count();
            var totalCount = 526;

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public IDataResult GetContragentInfoById(BaseParams baseParams)
        {
            var requestId = baseParams.Params.GetAs("requestId", 0L);

            var request = ContragentDomain.GetAll().FirstOrDefault(x => x.Id == requestId);

            if (request == null)
            {
                return new BaseDataResult();
            }

            return new BaseDataResult(new
            {
                request.Name,
                request.ShortName,
                OrgForm = request.OrganizationForm != null ? request.OrganizationForm.Name : string.Empty,
                JurAddress = request.FiasJuridicalAddress != null ? request.FiasJuridicalAddress.AddressName : string.Empty,
                FactAddress = request.FiasFactAddress != null ? request.FiasFactAddress.AddressName : string.Empty,
                request.Ogrn,
                request.Inn,
                request.OgrnRegistration,
                request.Phone,
                request.Email,
                request.TaxRegistrationSeries,
                request.TaxRegistrationNumber,
                request.TaxRegistrationIssuedBy,
                request.TaxRegistrationDate
            });
        }

        public IDataResult ListManOrgWithLicense(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
       
            var licData = LicenseDomain.GetAll().AsEnumerable();

            var data = LicenseDomain.GetAll()
                .Where(x => licData.Any(y => y.Contragent == x.Contragent))
                .Select(x => new
                {
                    x.Contragent.Id,
                    x.Contragent.Name,
                    Municipality = x.Contragent.Municipality.Name,
                    x.Contragent.JuridicalAddress,
                    x.Contragent.Inn,
                    x.Contragent.ShortName
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.JuridicalAddress)
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public IDataResult ListManOrg(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var kindKND = baseParams.Params.ContainsKey("kindKND")
            ? baseParams.Params.GetValue("kindKND").ToInt()
            : 0;

            List<long> contragentList = new List<long>(); 

            var calcTask = baseParams.Params.ContainsKey("rOMCalcTaskId")
            ? Convert.ToInt64(baseParams.Params.GetValue("rOMCalcTaskId").ToInt())
            : 0;
            //KindKND
            KindKND kknd = (KindKND)kindKND;
            if (calcTask != 0)
            {
                var romCalcTask = ROMCalcTaskDomain.Get(calcTask);
                if (romCalcTask != null)
                {
                    YearEnums yearEnum = romCalcTask.YearEnums;
                    kknd = romCalcTask.KindKND;
                    contragentList = ROMCategoryDomain.GetAll()
                        .Where(x => x.YearEnums == yearEnum && x.KindKND == kknd)
                        .Select(x => x.Contragent.Id).ToList();

                }
            }

            if (kknd == KindKND.LicenseControl)
            {
                var licensesDimain = Container.Resolve<IManOrgLicenseService>();
                var licenses = LicenseDomain.GetAll()
                  //  .Where(x => x.State.Name == "Выдана")
                    .Select(x => x.Contragent).ToList();

                var data = ManagingOrganizationDomain.GetAll()
              .Where(x => x.ActivityGroundsTermination == GroundsTermination.NotSet && licenses.Contains(x.Contragent) && !contragentList.Contains(x.Contragent.Id))
              .Select(x => new
              {
                  x.Contragent.Id,
                  x.Contragent.Name,
                  Municipality = x.Contragent.Municipality.Name,
                  x.Contragent.JuridicalAddress,
                  x.Contragent.Inn,
                  x.Contragent.ShortName
              })
              .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
              .OrderThenIf(loadParams.Order.Length == 0, true, x => x.JuridicalAddress)
              .Filter(loadParams, Container);

                    var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            else
            {
                List<long> listContragents = new List<long>();

                listContragents.AddRange(
               ServiceOrganizationDomain.GetAll().Where(x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
               .Select(x => x.Contragent.Id).ToList());

                listContragents.AddRange(
              ManagingOrganizationDomain.GetAll().Where(x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
              .Select(x => x.Contragent.Id).ToList());

                var data = ContragentDomain.GetAll()
                    .Where(x=> listContragents.Contains(x.Id))
                    .Where(x => !contragentList.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        Municipality = x.Municipality.Name,
                        x.JuridicalAddress,
                        x.Inn,
                        x.ShortName
                    })
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParams.Order.Length == 0, true, x => x.JuridicalAddress)
                    .Filter(loadParams, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
        }


        public IList GetListWithRO(BaseParams baseParams, bool isPaging, ref int totalCount)
        {
            var contragentList = UserManager.GetContragentIds();
            List<long> listArtLawId = new List<long>();
            listArtLawId.Add(10013825);
            listArtLawId.Add(1599819);
            listArtLawId.Add(1599822);

            var param2AndMore = baseParams.Params.GetAs<bool>("cbShowOnly2andMore");
            var showall = baseParams.Params.GetAs<bool>("showall");
            if(!showall)
            { 
                var protocols = ProtocolArtLawDomain.GetAll()
                    //   .Where(x => listArtLawId.Contains(x.ArticleLaw != null ? x.ArticleLaw.Id : 0))
                    .Where(x => x.ArticleLaw.Id == 10013825 && x.Protocol.DocumentDate >= DateTime.Now.AddYears(-1))
                       .Select(x => new
                       {
                           ProtocolId = x.Protocol.Id,
                           DocNumber = x.Protocol.DocumentNumber,
                           InspId = x.Protocol.Inspection.Id
                       })
                       .Distinct();

                var children = ChildrenDomain.GetAll()
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription && x.Parent.DocumentDate >= DateTime.Now.AddYears(-1) && x.Children.TypeDocumentGji == TypeDocumentGji.Disposal)
                    .Select(x => new
                    {
                        Children = x.Children.Id,
                        Inspection_id = x.Children.Inspection.Id,
                        Parent = x.Parent.Id,
                        DocNumber = x.Children.DocumentNumber
                    });


                var resolutions2 = ResolutionDomain.GetAll().AsEnumerable()
                    .Where(x => x.Contragent != null && x.Inspection != null && x.DocumentDate != null && x.DocumentDate >= DateTime.Now.AddYears(-1))
                    .Join(children.AsEnumerable(), x => x.Inspection.Id + "|" + x.DocumentNumber, y => y.Inspection_id + "|" + y.DocNumber, (x, y) => new
                    {
                        Id = x.Id,
                        Inspection = x.Inspection.Id,
                        ContragentINN = x.Contragent.Inn,
                        ContragentId = x.Contragent.Id,
                        DocNum = x.DocumentNumber
                    });

                var resolutions = resolutions2.AsEnumerable()
                    .Join(protocols.AsEnumerable(), x => x.DocNum + "|" + x.Inspection, y => y.DocNumber + "|" + y.InspId, (x, y) => new
                    {
                        Id = x.Id,
                        Inspection = x.Inspection,
                        ContragentINN = x.ContragentINN,
                        ContragentId = x.ContragentId
                    });

                var inspRo = InspectionGjiRealityObjectDomain.GetAll()
                    .Where(x => x.Inspection.ObjectCreateDate >= new DateTime(2015, 5, 1))
                      .Select(x => new
                      {
                          Id = x.Id,
                          //   MgContrId = (long?)x.ManOrgContract.ManagingOrganization.Contragent.Id,
                          Reality_id = x.RealityObject.Id,
                          Inspection_id = x.Inspection.Id
                      });

                //        var resolutionsLimited = resolutions.Where(x => listResol.Contains(x.Id));
                //var resolutionsLimited = resolutions.Where(item => item.ParentList.Any(j => protocols.ToList().Contains(j)));

                var resolRo = inspRo.AsEnumerable()
                       .Join(resolutions.AsEnumerable(), x => x.Inspection_id, y => y.Inspection, (x, y) => new
                       {
                           Id = x.Id,
                           ResolutionId = y.Id,
                           ContragentINN = y.ContragentINN,
                           ContragentId = y.ContragentId,
                           RoId = x.Reality_id


                       });

                var morgContract = MorgRODomain.GetAll()
                  .Where(x => (x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= DateTime.Now)
                  && x.ManOrgContract.ManagingOrganization.Contragent.Id > 0)
                  .Select(x => new
                  {
                      Id = x.Id,
                      mcId = x.Id,
                      //   MgContrId = (long?)x.ManOrgContract.ManagingOrganization.Contragent.Id,
                      MgContrId = x.ManOrgContract.ManagingOrganization.Contragent.Id,
                      x.ManOrgContract.StartDate,
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
                   x.DateIssued,
                   Request = x.Request != null ? x.Request.Id : 0,
                   Contragent = x.Contragent.Name,
                   ContragentId = x.Contragent.Id,
                   ContragentMunicipality = x.Contragent.Municipality.Name,
                   x.Contragent.Inn

               }).AsEnumerable();


                var morgContractEnum = morgContract.Where(x => data.Any(y => y.ContragentId == x.MgContrId)).AsEnumerable();
                var resolRoEnum = resolRo.Where(x => data.Any(y => y.Inn == x.ContragentINN)).AsEnumerable();
                var loadParams = baseParams.GetLoadParam();
                Dictionary<string, int> колвоПостановлений = new Dictionary<string, int>();
                foreach (var key in resolRoEnum)
                {
                    if (!колвоПостановлений.ContainsKey(key.ContragentINN + "|" + key.RoId))
                    {
                        колвоПостановлений.Add(key.ContragentINN + "|" + key.RoId, 1);
                    }
                    else
                    {
                        колвоПостановлений[key.ContragentINN + "|" + key.RoId] += 1;
                    }
                }



                var data2 = data.AsEnumerable()
                .Join(morgContractEnum, x => x.ContragentId, y => y.MgContrId, (x, y) => new { x, y })
                .WhereIf(param2AndMore, m => (колвоПостановлений.ContainsKey(m.x.Inn + "|" + m.y.roId) ? колвоПостановлений[m.x.Inn + "|" + m.y.roId] : 0) >= 2)
                .Select(m => new
                {
                    Id = m.y.Id,
                    mcId = m.y.mcId,
                    State = m.x.State,
                    LicNum = m.x.LicNum,
                    DateIssued = m.x.DateIssued,
                    Request = m.x.Request != 0 ? m.x.Request : 0,
                    Contragent = m.x.Contragent,
                    ContragentMunicipality = m.x.ContragentMunicipality,
                    ManStartDate = m.y.StartDate,
                    ROAddress = m.y.Address,
                    ROMunicipality = m.y.Name,
                    Inn = m.x.Inn,
                    postCount = колвоПостановлений.ContainsKey(m.x.Inn + "|" + m.y.roId) ? колвоПостановлений[m.x.Inn + "|" + m.y.roId] : 0
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
                var protocols = ProtocolArtLawDomain.GetAll()
               //   .Where(x => listArtLawId.Contains(x.ArticleLaw != null ? x.ArticleLaw.Id : 0))
               .Where(x => x.ArticleLaw.Id == 10013825 && x.Protocol.DocumentDate >= new DateTime(2015,5,1))
                  .Select(x => new
                  {
                      ProtocolId = x.Protocol.Id,
                      DocNumber = x.Protocol.DocumentNumber,
                      InspId = x.Protocol.Inspection.Id
                  })
                  .Distinct();

                var children = ChildrenDomain.GetAll()
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription && x.Parent.DocumentDate >= new DateTime(2015, 5, 1) && x.Children.TypeDocumentGji == TypeDocumentGji.Disposal)
                    .Select(x => new
                    {
                        Children = x.Children.Id,
                        Inspection_id = x.Children.Inspection.Id,
                        Parent = x.Parent.Id,
                        DocNumber = x.Children.DocumentNumber
                    });


                var resolutions2 = ResolutionDomain.GetAll().AsEnumerable()
                    .Where(x => x.Contragent != null && x.Inspection != null && x.DocumentDate != null && x.DocumentDate >= new DateTime(2015, 5, 1))
                    .Join(children.AsEnumerable(), x => x.Inspection.Id + "|" + x.DocumentNumber, y => y.Inspection_id + "|" + y.DocNumber, (x, y) => new
                    {
                        Id = x.Id,
                        Inspection = x.Inspection.Id,
                        ContragentINN = x.Contragent.Inn,
                        ContragentId = x.Contragent.Id,
                        DocNum = x.DocumentNumber
                    });

                var resolutions = resolutions2.AsEnumerable()
                    .Join(protocols.AsEnumerable(), x => x.DocNum + "|" + x.Inspection, y => y.DocNumber + "|" + y.InspId, (x, y) => new
                    {
                        Id = x.Id,
                        Inspection = x.Inspection,
                        ContragentINN = x.ContragentINN,
                        ContragentId = x.ContragentId
                    });

                var inspRo = InspectionGjiRealityObjectDomain.GetAll()
                    .Where(x => x.Inspection.ObjectCreateDate >= new DateTime(2015, 5, 1))
                      .Select(x => new
                      {
                          Id = x.Id,
                      //   MgContrId = (long?)x.ManOrgContract.ManagingOrganization.Contragent.Id,
                      Reality_id = x.RealityObject.Id,
                          Inspection_id = x.Inspection.Id
                      });

                //        var resolutionsLimited = resolutions.Where(x => listResol.Contains(x.Id));
                //var resolutionsLimited = resolutions.Where(item => item.ParentList.Any(j => protocols.ToList().Contains(j)));

                var resolRo = inspRo.AsEnumerable()
                       .Join(resolutions.AsEnumerable(), x => x.Inspection_id, y => y.Inspection, (x, y) => new
                       {
                           Id = x.Id,
                           ResolutionId = y.Id,
                           ContragentINN = y.ContragentINN,
                           ContragentId = y.ContragentId,
                           RoId = x.Reality_id


                       });

                var morgContract = MorgRODomain.GetAll()
                  .Where(x => (x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= DateTime.Now)
                  && x.ManOrgContract.ManagingOrganization.Contragent.Id > 0)
                  .Select(x => new
                  {
                      Id = x.Id,
                      mcId = x.Id,
                  //   MgContrId = (long?)x.ManOrgContract.ManagingOrganization.Contragent.Id,
                  MgContrId = x.ManOrgContract.ManagingOrganization.Contragent.Id,
                      x.ManOrgContract.StartDate,
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
                   x.DateIssued,
                   Request = x.Request != null ? x.Request.Id : 0,
                   Contragent = x.Contragent.Name,
                   ContragentId = x.Contragent.Id,
                   ContragentMunicipality = x.Contragent.Municipality.Name,
                   x.Contragent.Inn

               }).AsEnumerable();


                var morgContractEnum = morgContract.Where(x => data.Any(y => y.ContragentId == x.MgContrId)).AsEnumerable();
                var resolRoEnum = resolRo.Where(x => data.Any(y => y.Inn == x.ContragentINN)).AsEnumerable();
                var loadParams = baseParams.GetLoadParam();
                Dictionary<string, int> колвоПостановлений = new Dictionary<string, int>();
                foreach (var key in resolRoEnum)
                {
                    if (!колвоПостановлений.ContainsKey(key.ContragentINN + "|" + key.RoId))
                    {
                        колвоПостановлений.Add(key.ContragentINN + "|" + key.RoId, 1);
                    }
                    else
                    {
                        колвоПостановлений[key.ContragentINN + "|" + key.RoId] += 1;
                    }
                }

                var data2 = data.AsEnumerable()
                .Join(morgContractEnum, x => x.ContragentId, y => y.MgContrId, (x, y) => new { x, y })
                .WhereIf(param2AndMore, m => (колвоПостановлений.ContainsKey(m.x.Inn + "|" + m.y.roId) ? колвоПостановлений[m.x.Inn + "|" + m.y.roId] : 0) >= 2)
                .Select(m => new
                {
                    Id = m.y.Id,
                    mcId = m.y.mcId,
                    State = m.x.State,
                    LicNum = m.x.LicNum,
                    DateIssued = m.x.DateIssued,
                    Request = m.x.Request != 0 ? m.x.Request : 0,
                    Contragent = m.x.Contragent,
                    ContragentMunicipality = m.x.ContragentMunicipality,
                    ManStartDate = m.y.StartDate,
                    ROAddress = m.y.Address,
                    ROMunicipality = m.y.Name,
                    Inn = m.x.Inn,
                    postCount = колвоПостановлений.ContainsKey(m.x.Inn + "|" + m.y.roId) ? колвоПостановлений[m.x.Inn + "|" + m.y.roId] : 0
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

        public IList GetLicenseRO(BaseParams baseParams, bool isPaging, ref int totalCount)
        //public IList GetList(BaseParams baseParams, bool isPaging, ref int totalCount)
        {
            var loadParams = baseParams.GetLoadParam();
            var mcid = baseParams.Params.Get("id");
            var morgContract = MorgRODomain.GetAll()
            .Where(x => (x.Id.ToString() == mcid.ToString()))
            .Select(x => new
            {
                Id = x.Id,
                ContragentId = x.ManOrgContract.ManagingOrganization.Contragent.Id,
                Contragent = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                x.ManOrgContract.StartDate,
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
                  x.DateIssued,
                  Request = x.Request != null ? x.Request.Id : 0,
                  Contragent = x.Contragent.Name,
                  ContragentId = x.Contragent.Id,
                  ContragentMunicipality = x.Contragent.Municipality.Name,
                  x.Contragent.Inn,
                  x.DisposalNumber,
                  x.DateDisposal,
                  x.DateRegister

              });

            var data2 = data.AsEnumerable()
     .Join(morgContract.AsEnumerable(), x => x.ContragentId, y => y.ContragentId, (x, y) => new { x, y })
     .Select(m => new
     {
         LicNum = m.x.LicNum,
         DateIssued = m.x.DateIssued,
         Contragent = m.x.Contragent,
         ManStartDate = m.y.StartDate,
         ROAddress = m.y.Address,
         Inn = m.x.Inn,
         DisposalNumber = m.x.DisposalNumber,
         DateDisposal = m.x.DateDisposal,
         DateRegister = m.x.DateRegister
     });


            return data2.ToList();
        }

        public IDataResult GetInfo(long id)
        {
           
            ManOrgLicense license = null;
            ManOrgLicenseRequest request = null;
            var morgRo = MorgRODomain.GetAll()
                .Where(x => x.Id == id)
                .Select(m => new
                {
                    ContragentId = m.ManOrgContract.ManagingOrganization.Contragent.Id

                }).Distinct();
            //    morgRo.ManOrgContract.ManagingOrganization.Contragent.Id
            var license2 = LicenseDomain.GetAll().AsEnumerable()
                .Join(morgRo.AsEnumerable(), x => x.Contragent.Id, y => y.ContragentId, (x, y) => new { x, y })
                .Select(m => new
                {
                    Id = m.x.Id
                }).ToList();

            long licenseId = license2 != null ? license2[0].Id : 0;

            license = LicenseDomain.GetAll().FirstOrDefault(x => x.Id == licenseId);

            //if (license == null)
            //            {
            //                return new BaseDataResult(false, "Не удалось определить лицензию по Id " + id.ToStr());
            //            }

            //            if (license.Request != null)
            //            {
            //                request = license.Request;
            //            }


            var t = new BaseDataResult(new ManOrgLicenseGisInfo
            {
                licenseId = license != null ? license.Id : 0,
                requestId = request != null ? request.Id : 0
            });

            return new BaseDataResult(new ManOrgLicenseGisInfo
            {
                licenseId = license != null ? license.Id : 0,
                requestId = request != null ? request.Id : 0
            });
        }

        public IList GetResolutionsByMCID(BaseParams baseParams, bool isPaging, ref int totalCount)
        {
            List<long> listArtLawId = new List<long>();
            listArtLawId.Add(10013825);
            listArtLawId.Add(1599819);
            listArtLawId.Add(1599822);
            var loadParams = baseParams.GetLoadParam();
            Int64 mcid = loadParams.Filter.GetAs("id", 0L);

            var morgContract = MorgRODomain.GetAll()
          .Where(x => x.ManOrgContract.ManagingOrganization.Contragent.Id > 0 && x.Id == mcid)
          .Select(x => new
          {
              Id = x.Id,
              MgContrId = x.ManOrgContract.ManagingOrganization.Contragent.Id,
              roId = x.RealityObject.Id
              //ManOrgContract = x.ManOrgContract.Id
          }).AsEnumerable();


            var contrId = morgContract.FirstOrDefault(x => x.Id != 0).MgContrId;
            var roId = morgContract.FirstOrDefault(x => x.Id != 0).roId;

            var protocols = ProtocolArtLawDomain.GetAll()
                //   .Where(x => listArtLawId.Contains(x.ArticleLaw != null ? x.ArticleLaw.Id : 0))
                .Where(x => x.ArticleLaw.Id == 10013825 && x.Protocol.Inspection.Contragent.Id == contrId)
                   .Select(x => new
                   {
                       ProtocolId = x.Protocol.Id,
                       DocNumber = x.Protocol.DocumentNumber,
                       InspId = x.Protocol.Inspection.Id,
                       ContragentId = x.Protocol.Inspection.Contragent.Id
                   })
                   .Distinct();

            var protocolsEnum = protocols.AsEnumerable();
            // var inspRoEnum = inspRo.Where(x => resolutions.Any(y => y.Inspection == x.Inspection_id)).AsEnumerable();

            var children = ChildrenDomain.GetAll()
        .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription && x.Parent.DocumentDate >= DateTime.Now.AddYears(-1) && x.Children.TypeDocumentGji == TypeDocumentGji.Disposal
        && x.Children.Inspection.Contragent.Id == contrId)
        .Select(x => new
        {
            Children = x.Children.Id,
            Inspection_id = x.Children.Inspection.Id,
            Parent = x.Parent.Id,
            DocNumber = x.Children.DocumentNumber
        });

           var resolutions2 = ResolutionDomain.GetAll().AsEnumerable()
                .Where(x => x.DocumentDate >= new DateTime(2015, 8, 1) && x.Contragent != null && x.Inspection != null && x.Contragent.Id == contrId)
                .Join(children.AsEnumerable(), x => x.Inspection.Id + "|" + x.DocumentNumber, y => y.Inspection_id + "|" + y.DocNumber, (x, y) => new
                {
                 Id = x.Id,
                 Inspection = x.Inspection.Id,
                 ContragentINN = x.Contragent.Inn,
                 ContragentId = x.Contragent.Id,
                 DocNum = x.DocumentNumber,
                 DocDate = x.DocumentDate,
                 x.Executant,
                 x.Inspection.TypeBase
             });

            //var resolutions2 = ResolutionDomain.GetAll().AsEnumerable()
            //    .Where(x => x.DocumentDate >= new DateTime(2015, 5, 1) && x.Contragent != null)
            //    .Select(x => new
            //    {
            //        Id = x.Id,
            //        Inspection = x.Inspection.Id,
            //        ContragentINN = x.Contragent.Inn,
            //        ContragentId = x.Contragent.Id,
            //        DocNum = x.DocumentNumber,
            //        DocDate = x.DocumentDate,
            //        x.Executant,
            //        x.Inspection.TypeBase
            //    });

            var resolutions = resolutions2.AsEnumerable()
                .Join(protocolsEnum, x => x.DocNum + "|" + x.Inspection, y => y.DocNumber + "|" + y.InspId, (x, y) => new
                {
                    Id = x.Id,
                    Inspection = x.Inspection,
                    ContragentINN = x.ContragentINN,
                    ContragentId = x.ContragentId,
                    DocNum = x.DocNum,
                    DocDate = x.DocDate,
                    x.Executant,
                    x.TypeBase
                });

            var inspRo = InspectionGjiRealityObjectDomain.GetAll()
                .Where(x=> x.Inspection.Contragent.Id == contrId && x.RealityObject.Id == roId)
               .Select(x=> new
                 {
                     Id = x.Id,
                     //   MgContrId = (long?)x.ManOrgContract.ManagingOrganization.Contragent.Id,
                     Reality_id = x.RealityObject.Id,
                     Inspection_id = x.Inspection.Id
                 });


            var qwerty = inspRo
                .Where(x => x.Reality_id == roId)
                .Select(x => x.Inspection_id).ToList();



            var inspRoEnum = resolutions.AsEnumerable()
                .Where(x => qwerty.Contains(x.Inspection))
                .AsEnumerable();


            var inspRoEnum2 = inspRoEnum
                 .Select(x => new
                 {
                     x.Id,
                     x.Inspection,
                     x.DocNum,
                     x.DocDate,
                     Executant = x.Executant.Name,
                     x.TypeBase,
                     InspectionId = x.Inspection,
                     TypeDocumentGji = TypeDocumentGji.Resolution
                 }).AsQueryable();
            

            //var count1 = inspRoEnum2.Count(x => x.Id != 0);
            //var count2 = resolutions.Count(x => x.Id != 0);

            //var tyu = inspRoEnum2.Select(x => x.Inspection_id).ToList();

            //List<Int64> list = new List<Int64>();
            //foreach(var t in inspRoEnum2)
            //{
            //    if (!list.Contains(t.Inspection_id))
            //        list.Add(t.Inspection_id);
            //}

            //int tttt = tyu.Count;

            //var resolRo = resolutions.AsEnumerable()
            //       .Where(y => tyu.Contains(y.Inspection)).Select(y => new
            //       {
            //           Id = y.Id,
            //           DocNum = y.DocNum,
            //           DocDate = y.DocDate,
            //           Executant = y.Executant.Name


            //       })
            //       .AsQueryable();
            //     //.Filter(loadParams, this.Container);

            totalCount = inspRoEnum2.Count();

            //if (isPaging)
            //{
            //    return resolRo.Order(loadParams).Paging(loadParams).ToList();
            //}

            //return resolRo.Order(loadParams).ToList();
            return inspRoEnum2.ToList();
        }



        /// <summary>
        /// Данный метод получает информацию по лицензии 
        /// Если запросили из Запроса на лицензию, либо из Лицензии
        /// </summary>
        public IDataResult GetInfo(BaseParams baseParams)
        {
           
            var id = baseParams.Params.GetAs("id", 0L);

            return GetInfo(id);
        }

        public IDataResult GetListPersonByContragentId(BaseParams baseParams, bool isPaging, out int totalCount)
        {
            totalCount = 0;

            var loadParams = baseParams.GetLoadParam();

            var ctrId = baseParams.Params.GetAs("contragentId", 0L);
            //var requestId = baseParams.Params.GetAs("requestId", 0L);
            //var dateRequest = baseParams.Params.GetAs("dateRequest", DateTime.MinValue);

            //if (dateRequest == DateTime.MinValue)
            //{
            //    return new BaseDataResult(false, "Необходимо указать дату обращения");
            //}

            var placeWorkDict = placeWorkDomain.GetAll()
                .Where(x => x.Contragent.Id == ctrId && x.StartDate.HasValue)
                .Select(x => new
                {
                    personId = x.Person.Id,
                    position = x.Position.Name,
                    date = x.StartDate.Value
                })
                .AsEnumerable()
                .GroupBy(x => x.personId)
                .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.date).Select(z => z.position).First());

            var personIds = placeWorkDomain.GetAll()
                .Where(x => x.Contragent.Id == ctrId)
                //.Where(x => x.StartDate <= dateRequest)
                //.Where(x => !x.EndDate.HasValue || x.EndDate.Value >= dateRequest)
                //.Where(x => !x.EndDate.HasValue)
                .Select(x => x.Person.Id)
                .Distinct()
                .ToList();

            personIds = qualDomain.GetAll()
                .Where(x => personIds.Contains(x.Person.Id))
                .Where(x => !x.HasCancelled || x.HasRenewed)
                //.Where(x => x.IssuedDate.HasValue && x.IssuedDate.Value <= dateRequest)
                //.Where(x => !x.EndDate.HasValue || x.EndDate.Value >= dateRequest)
                .Where(x => x.IssuedDate.HasValue)
                //.Where(x => !x.EndDate.HasValue)
                .Select(x => x.Person.Id)
                .Distinct()
                .ToList();

            var disqQuery = disqualDomain.GetAll()
                .Where(x => x.DisqDate.HasValue)
                .Where(x => !x.EndDisqDate.HasValue);
            //.Where(x => x.DisqDate.HasValue && x.DisqDate.Value <= dateRequest)
            //.Where(x => !x.EndDisqDate.HasValue || x.EndDisqDate.Value >= dateRequest);

            var data = PersonDomain.GetAll()
                .Where(x => !disqQuery.Any(y => y.Person.Id == x.Id))
                //     .Where(x => !RequestPersonDomain.GetAll().Any(y => y.Person.Id == x.Id))
                .Where(x => personIds.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.FullName
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.FullName,
                    Position = placeWorkDict.Get(x.Id) ?? ""
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            totalCount = data.Count();

            if (isPaging)
            {
                return new BaseDataResult(data.Order(loadParams).Paging(loadParams).ToList());
            }

            return new BaseDataResult(data.Order(loadParams).ToList());
        }


    }
}