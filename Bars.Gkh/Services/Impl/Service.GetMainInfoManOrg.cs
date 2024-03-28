namespace Bars.Gkh.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.GetMainInfoManOrg;

    public partial class Service
    {
        public GetMainInfoManOrgResponse GetMainInfoManOrg(string moId)
        {
            long id;
            ManagementOrganization managementOrganization = null;

            if (long.TryParse(moId, out id))
            {
                managementOrganization =
                    Container.Resolve<IDomainService<ManagingOrganization>>()
                             .GetAll()
                             .Where(x => x.Id == id)
                             .Select(
                                 x =>
                                 new ManagementOrganization
                                     {
                                         Contragent = x.Contragent,
                                         TypeManagement = x.TypeManagement,
                                         Id = x.Id,
                                         ManOrgName = x.Contragent.Name,
                                         LegalAddress = x.Contragent.JuridicalAddress,
                                         RealAddress = x.Contragent.FactAddress,
                                         PostAddress = x.Contragent.MailingAddress,
                                         Email = x.Contragent.Email,
                                         Twitter = x.Contragent.TweeterAccount,
                                         FrguRegNumber = x.Contragent.FrguRegNumber,
                                         FrguOrgNumber = x.Contragent.FrguOrgNumber,
                                         FrguServiceNumber = x.Contragent.FrguServiceNumber,
                                         OGRN = x.Contragent.Ogrn,
                                         INN = x.Contragent.Inn,
                                         ManForm = GetTypeManagement(x.TypeManagement),
                                         BossName = GetBossContact(x.Contragent).FullName,
                                         BossContact = GetBossContact(x.Contragent).Phone,
                                         ContactNumber = x.Contragent.Phone,
                                         RegistOrg = x.Contragent.OgrnRegistration,
                                         OfficalSite = x.Contragent.OfficialWebsite,
                                         Dispatcher = x.Contragent.PhoneDispatchService,
                                         AssocMemberships = GetMembership(x)
                                     })
                             .FirstOrDefault();

                if (managementOrganization != null && managementOrganization.TypeManagement == TypeManagementManOrg.TSJ)
                {
                    var commitMembers = GetContacts(managementOrganization.Contragent, new[] { "5" }).ToArray();
                    managementOrganization.CommitMembers =
                        commitMembers.Select(y => new CommitMember { Name = y.FullName }).ToArray();
                    managementOrganization.CommitNumbers =
                        commitMembers.Select(y => new Phone { Number = y.Phone }).ToArray();
                    managementOrganization.BoardMembers =
                        GetContacts(managementOrganization.Contragent, new[] { "6" })
                            .Select(y => new BoardMember { Name = y.FullName })
                            .ToArray();
                }
            }

            var result = managementOrganization == null ? Result.DataNotFound : Result.NoErrors;
            return new GetMainInfoManOrgResponse { ManagementOrganization = managementOrganization, Result = result };
        }
        
        private string GetTypeManagement(TypeManagementManOrg type)
        {
            switch (type)
            {
                case TypeManagementManOrg.JSK:
                    return "ЖСК";
                case TypeManagementManOrg.TSJ:
                    return "ТСЖ";
                case TypeManagementManOrg.UK:
                    return "УК";
                default:
                    return "Другое";
            }
        }

        private ContragentContact GetBossContact(Contragent contragent)
        {
            return GetContacts(contragent, new[] { "1", "2", "4" }).FirstOrDefault() ?? new ContragentContact();
        }

        private IEnumerable<ContragentContact> GetContacts(Contragent contragent, IEnumerable<string> codes)
        {
            return
                this.Container.Resolve<IDomainService<ContragentContact>>()
                    .GetAll()
                    .Where(x => x.Contragent.Id == contragent.Id)
                    .Where(x => codes.Contains(x.Position.Code))
                    .Where(x => !x.DateEndWork.HasValue || x.DateEndWork >= DateTime.Now)
                    .Where(x => !x.DateStartWork.HasValue || x.DateStartWork <= DateTime.Now)
                    .OrderBy(x => x.Position.Code).ToArray();
        }

        private AssocMembership[] GetMembership(ManagingOrganization mo)
        {
            return
                Container.Resolve<IDomainService<ManagingOrgMembership>>()
                         .GetAll()
                         .Where(x => x.ManagingOrganization.Id == mo.Id)
                         .Select(x => new AssocMembership { Name = x.Name, AsMembSite = x.OfficialSite })
                         .ToArray();
        }
    }
}