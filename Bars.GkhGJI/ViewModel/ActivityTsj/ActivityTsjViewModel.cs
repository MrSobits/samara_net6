namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.GkhGji.DomainService;

    using Gkh.Entities;
    using Entities;

    public class ActivityTsjViewModel : BaseViewModel<ActivityTsj>
    {
        public override IDataResult List(IDomainService<ActivityTsj> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var serviceStatute = Container.Resolve<IDomainService<ActivityTsjStatute>>();

            var queryActivityTsjStatute = Container.Resolve<IActivityTsjService>().GetFilteredByOperator(domainService)
                .Select(x => new
                {
                    x.Id,
                    ManOrgName = x.ManagingOrganization.Contragent.Name,
                    MunicipalityName = x.ManagingOrganization.Contragent.Municipality.Name,
                    x.ManagingOrganization.Contragent.Inn,
                    HasStatute = serviceStatute.GetAll().Any(y => x.Id == y.ActivityTsj.Id)
                });

            var queryActivityTsjStatuteId = queryActivityTsjStatute.Select(x => x.Id);

            var dictMemberTsj = Container.Resolve<IDomainService<ActivityTsjMember>>()
                                         .GetAll()
                                         .Where(x => queryActivityTsjStatuteId.Contains(x.ActivityTsj.Id))
                                         .Where(x => x.Year == DateTime.Today.Year)
                                         .Select(x => new { activityTsjId = x.ActivityTsj.Id, x.State.Name })
                                         .AsEnumerable()
                                         .GroupBy(x => x.activityTsjId)
                                         .ToDictionary(
                                             x => x.Key,
                                             v => v.Select(y => y.Name).OrderByDescending(y => y).FirstOrDefault());

            var result = queryActivityTsjStatute
                                                .OrderIf(loadParam.Order.Length == 0, true, x => x.MunicipalityName)
                                                .Order(loadParam)
                                                .ToArray()
                                                .Select(x => new
                                                {
                                                    x.Id,
                                                    x.MunicipalityName,                                                    
                                                    x.ManOrgName,
                                                    Inn = x.Inn ?? string.Empty,
                                                    x.HasStatute,
                                                    StateMemberTsj = dictMemberTsj.ContainsKey(x.Id) ? dictMemberTsj[x.Id] : null
                                                })
                                                .AsQueryable()                                                
                                                .Filter(loadParam, Container);


            int totalCount = result.Count();

            return new ListDataResult(result.Paging(loadParam).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<ActivityTsj> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return obj != null ? new BaseDataResult(
                                             new
                                             {
                                                 obj.Id,
                                                 ManagingOrganization = new
                                                 {
                                                     obj.ManagingOrganization.Id,
                                                     ContragentName = obj.ManagingOrganization.Contragent.Name,
                                                     ContragentInn = obj.ManagingOrganization.Contragent.Inn,
                                                     ContragentJuridicalAddress = obj.ManagingOrganization.Contragent.JuridicalAddress,
                                                     ContragentMailingAddress = obj.ManagingOrganization.Contragent.MailingAddress,
                                                     ContragentKpp = obj.ManagingOrganization.Contragent.Kpp,
                                                 }
                                             }
                ) : new BaseDataResult();
        }
    }
}