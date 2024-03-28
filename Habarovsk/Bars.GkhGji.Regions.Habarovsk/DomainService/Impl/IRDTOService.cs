using Bars.B4;
using Bars.GkhGji.Regions.Habarovsk.DomainService.InterdepartmentalRequests;
using System.Linq;
using Castle.Windsor;
using Bars.Gkh.Authentification;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Entities.SMEVPremises;
using System;
using Bars.GkhGji.Regions.Habarovsk.Entities.SMEV.Helper;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService.Impl
{
    public class IRDTOService : IIRDTOService
    {
        public IWindsorContainer Container { get; set; }

        public IGkhUserManager UserManager { get; set; }

        public IDataResult GetListIRDTO(BaseParams baseParams)
        {
            var MVDDomain = this.Container.Resolve<IDomainService<SMEVMVD>>();
            var ComplaintsRequestDomain = this.Container.Resolve<IDomainService<SMEVComplaintsRequest>>();
            var SMEVNDFLDomain = this.Container.Resolve<IDomainService<SMEVNDFL>>();
            var GASUDomain = this.Container.Resolve<IDomainService<GASU>>();
            var GISERPDomain = this.Container.Resolve<IDomainService<GISERP>>();
            var ERKNMDomain = this.Container.Resolve<IDomainService<ERKNM>>();
            var PassportDomain = this.Container.Resolve<IDomainService<MVDPassport>>();
            var MVDLivingPlaceRegistrationDomain = this.Container.Resolve<IDomainService<MVDLivingPlaceRegistration>>();
            var MVDStayingPlaceRegistrationDomain = this.Container.Resolve<IDomainService<MVDStayingPlaceRegistration>>();
            var SMEVEGRNDomain = this.Container.Resolve<IDomainService<SMEVEGRN>>();
            var SMEVPremisesDomain = this.Container.Resolve<IDomainService<SMEVPremises>>();
            var SMEVDISKVLICDomain = this.Container.Resolve<IDomainService<SMEVDISKVLIC>>();
            var SMEVSNILSDomain = this.Container.Resolve<IDomainService<SMEVSNILS>>();
            var SMEVExploitResolutionDomain = this.Container.Resolve<IDomainService<SMEVExploitResolution>>();
            var SMEVChangePremisesStateDomain = this.Container.Resolve<IDomainService<SMEVChangePremisesState>>();
            var SMEVValidPassportDomain = this.Container.Resolve<IDomainService<SMEVValidPassport>>();
            var SMEVStayingPlaceDomain = this.Container.Resolve<IDomainService<SMEVStayingPlace>>();
            var SMEVSocialHireDomain = this.Container.Resolve<IDomainService<SMEVSocialHire>>();
            var SMEVEmergencyHouse = this.Container.Resolve<IDomainService<Entities.SMEVEmergencyHouse.SMEVEmergencyHouse>>();
            var SMEVRedevelopment = this.Container.Resolve<IDomainService<Entities.SMEVRedevelopment.SMEVRedevelopment>>();
            var SMEVOwnershipProperty = this.Container.Resolve<IDomainService<Entities.SMEVOwnershipProperty.SMEVOwnershipProperty>>();
            var SMEVFNSLicRequestDomain = this.Container.Resolve<IDomainService<SMEVFNSLicRequest>>();

            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.Now.AddMonths(-1));
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.Now);
                     

            var data = MVDDomain.GetAll()
                .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.CalcDate,
                    Department = "МВД",
                     Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Smevmvd,
                    FrontControllerName = "B4.controller.SMEVMVD",
                    FrontModelName = "smev.SMEVMVD"
                }).ToList();

            data.AddRange(PassportDomain.GetAll()
                .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
               .Select(x => new InterdepartmentalRequestsDTO
               {
                   Id = x.Id,
                   Answer = x.Answer,
                   Date = x.CalcDate,
                   Department = "МВД",
                   Inspector = x.Inspector != null ? x.Inspector.Fio : "",
                   Number = x.MessageId,
                   RequestState = x.RequestState,
                   NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Validpassport,
                   FrontControllerName = "B4.controller.MVDPassport",
                   FrontModelName = "smev.MVDPassport"
               }).ToList());

            data.AddRange(MVDLivingPlaceRegistrationDomain.GetAll()
             .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
            .Select(x => new InterdepartmentalRequestsDTO
            {
                Id = x.Id,
                Answer = x.Answer,
                Date = x.CalcDate,
                Department = "МВД",
                Inspector = x.Inspector != null ? x.Inspector.Fio : "",
                Number = x.MessageId,
                RequestState = x.RequestState,
                NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.MVDLivingPlaceRegistration,
                FrontControllerName = "B4.controller.MVDLivingPlaceRegistration",
                FrontModelName = "smev.MVDLivingPlaceRegistration"
            }).ToList());

            data.AddRange(MVDStayingPlaceRegistrationDomain.GetAll()
       .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
       .Select(x => new InterdepartmentalRequestsDTO
       {
           Id = x.Id,
           Answer = x.Answer,
           Date = x.CalcDate,
           Department = "МВД",
           Inspector = x.Inspector != null ? x.Inspector.Fio : "",
           Number = x.MessageId,
           RequestState = x.RequestState,
           NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.MVDStayingPlaceRegistration,
           FrontControllerName = "B4.controller.MVDStayingPlaceRegistration",
           FrontModelName = "smev.MVDStayingPlaceRegistration"
       }).ToList());

            data.AddRange(SMEVEGRNDomain.GetAll()
                 .Where(x => x.RequestDate >= dateStart && x.RequestDate <= dateEnd)
                 .Select(x => new InterdepartmentalRequestsDTO
                 {
                     Id = x.Id,
                     Answer = x.Answer,
                     Date = x.RequestDate,
                     Department = "Росреестр",
                     Inspector = x.Inspector != null ? x.Inspector.Fio : "",
                     Number = x.MessageId,
                     RequestState = x.RequestState,
                     NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Smevegrnrequest,
                     FrontControllerName = "B4.controller.SMEVEGRN",
                     FrontModelName = "smev.SMEVEGRN"
                 }).ToList());

            data.AddRange(ComplaintsRequestDomain.GetAll()
              .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.CalcDate,
                    Department = "ГОСУСЛУГИ",
                     Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Complaintsrequest,
                    FrontControllerName = "B4.controller.SMEVComplaintsRequest",
                    FrontModelName = "complaints.SMEVComplaintsRequest"
                }).ToList());
            data.AddRange(SMEVNDFLDomain.GetAll()
                .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.CalcDate,
                    Department = "ФНС",
                    Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Ndfl,
                    FrontControllerName = "B4.controller.SMEVNDFL",
                    FrontModelName = "smev.SMEVNDFL"
                }).ToList());
            data.AddRange(GASUDomain.GetAll()
                 .Where(x => x.RequestDate >= dateStart && x.RequestDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.RequestDate,
                    Department = "Росстат",
                     Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Gasu,
                    FrontControllerName = "B4.controller.GASU",
                    FrontModelName = "smev.GASU"
                }).ToList());
            data.AddRange(GISERPDomain.GetAll()
                   .Where(x => x.RequestDate >= dateStart && x.RequestDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.RequestDate,
                    Department = "Генеральная прокуратура",
                     Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Giserp,
                    FrontControllerName = "B4.controller.GISERP",
                    FrontModelName = "smev.GISERP"
                }).ToList());
            data.AddRange(ERKNMDomain.GetAll()
                  .Where(x => x.RequestDate >= dateStart && x.RequestDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.RequestDate,
                    Department = "Генеральная прокуратура",
                     Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Erknm,
                    FrontControllerName = "B4.controller.ERKNM",
                    FrontModelName = "smev.ERKNM"
                }).ToList());
            data.AddRange(SMEVPremisesDomain.GetAll()
                 .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.CalcDate,
                    Department = "ОМСУ",
                     Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Premises,
                    FrontControllerName = "B4.controller.SMEVPremises",
                    FrontModelName = "smevpremises.SMEVPremises"
                }).ToList());
            data.AddRange(SMEVDISKVLICDomain.GetAll()
                 .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.CalcDate,
                    Department = "ФНС",
                     Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Diskvlic,
                    FrontControllerName = "B4.controller.SMEVDISKVLIC",
                    FrontModelName = "smev.SMEVDISKVLIC"
                }).ToList());
            data.AddRange(SMEVSNILSDomain.GetAll()
                  .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.CalcDate,
                    Department = "ПФР",
                     Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Smevsnils,
                    FrontControllerName = "B4.controller.SMEVSNILS",
                    FrontModelName = "smev.SMEVSNILS"
                }).ToList());
            data.AddRange(SMEVExploitResolutionDomain.GetAll()
                 .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.CalcDate,
                    Department = "ОМСУ",
                     Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Exploit,
                    FrontControllerName = "B4.controller.SMEVExploitResolution",
                    FrontModelName = "smev.SMEVExploitResolution"
                }).ToList());
            data.AddRange(SMEVChangePremisesStateDomain.GetAll()
                 .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.CalcDate,
                    Department = "ОМСУ",
                     Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Changepremisesstate,
                    FrontControllerName = "B4.controller.SMEVChangePremisesState",
                    FrontModelName = "smev.SMEVChangePremisesState"
                }).ToList());
            data.AddRange(SMEVValidPassportDomain.GetAll()
                 .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.CalcDate,
                    Department = "МВД",
                     Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Validpassport,
                    FrontControllerName = "B4.controller.SMEVValidPassport",
                    FrontModelName = "smev2.SMEVValidPassport"
                }).ToList());
            data.AddRange(SMEVStayingPlaceDomain.GetAll()
                 .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.CalcDate,
                    Department = "МВД",
                     Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Stayingplace,
                    FrontControllerName = "B4.controller.SMEVStayingPlace",
                    FrontModelName = "smev2.SMEVStayingPlace"
                }).ToList());
            data.AddRange(SMEVSocialHireDomain.GetAll()
              .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.CalcDate,
                    Department = "ОМСУ",
                     Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Socialhire,
                    FrontControllerName = "B4.controller.SMEVSocialHire",
                    FrontModelName = "smev.SMEVSocialHire"
                }).ToList());
            data.AddRange(SMEVEmergencyHouse.GetAll()
                 .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.CalcDate,
                    Department = "ОМСУ",
                     Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Emergencyhouse,
                    FrontControllerName = "B4.controller.SMEVEmergencyHouse",
                    FrontModelName = "smevemergencyhouse.SMEVEmergencyHouse"
                }).ToList());
            data.AddRange(SMEVRedevelopment.GetAll()
                .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.CalcDate,
                    Department = "ОМСУ",
                     Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Redevelopment,
                    FrontControllerName = "B4.controller.SMEVRedevelopment",
                    FrontModelName = "smevredevelopment.SMEVRedevelopment"
                }).ToList());
            data.AddRange(SMEVOwnershipProperty.GetAll()
                .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.CalcDate,
                    Department = "ОМСУ",
                     Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Owproperty,
                    FrontControllerName = "B4.controller.SMEVOwnershipProperty",
                    FrontModelName = "smevownershipproperty.SMEVOwnershipProperty"
                }).ToList());
            data.AddRange(SMEVFNSLicRequestDomain.GetAll()
                .Where(x => x.CalcDate >= dateStart && x.CalcDate <= dateEnd)
                .Select(x => new InterdepartmentalRequestsDTO
                {
                    Id = x.Id,
                    Answer = x.Answer,
                    Date = x.CalcDate,
                    Department = "ФНС",
                    Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    Number = x.MessageId,
                    RequestState = x.RequestState,
                    NameOfInterdepartmentalDepartment = NameOfInterdepartmentalDepartment.Smevfnslicrequest,
                    FrontControllerName = "B4.controller.SMEVFNSLicRequest",
                    FrontModelName = "smev.SMEVFNSLicRequest"
                }).ToList());
            var loadParams = baseParams.GetLoadParam();
            var result = data.AsQueryable()
                    .Select(x => new
                    {
                        Id = x.Id,
                        Answer = x.Answer,
                        Date = x.Date,
                        Department = x.Department,
                        Inspector = x.Inspector,
                        Number = x.Number,
                        RequestState = x.RequestState,
                        NameOfInterdepartmentalDepartment = x.NameOfInterdepartmentalDepartment,
                        FrontControllerName = x.FrontControllerName,
                        FrontModelName = x.FrontModelName
                    }).Filter(loadParams, Container);

            var totalCount = result.Count();

            return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}
