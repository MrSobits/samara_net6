namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.Entities;

    using Castle.Windsor;

    public class DecisionNoticeService : IDecisionNoticeService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListRegister(BaseParams baseParams)
        {
            var decisionNoticeDomain = Container.Resolve<IDomainService<SpecialAccountDecisionNotice>>();

            var loadParams = baseParams.GetLoadParam();

            var data = decisionNoticeDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.SpecialAccountDecision,
                    x.State,
                    x.RegDate,
                    x.GjiNumber,
                    x.NoticeDate,
                    RoId = x.SpecialAccountDecision.RealityObject.Id,
                    x.SpecialAccountDecision.RealityObject.Address,
                    //RegOpContragent = x.SpecialAccountDecision.RegOperator.Contragent,
                    ManOrgContragent = x.SpecialAccountDecision.ManagingOrganization.Contragent
                })
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.RegDate,
                    x.GjiNumber,
                    x.NoticeDate,
                    RealityObject = new { Id = x.RoId, x.Address },
                    x.SpecialAccountDecision.MethodFormFund,
                    SpecialAccountDecision = x.SpecialAccountDecision.Id,
                    x.Address,
                    MunicipalityName = x.SpecialAccountDecision.RealityObject.Municipality.Name,
                    ContragentName =  x.ManOrgContragent.Name,
                    ContragentMailingAddress = x.ManOrgContragent.MailingAddress,
                    ContragentOgrn =  x.ManOrgContragent.Ogrn,
                    ContragentInn = x.ManOrgContragent.Inn,
                    ContragentOkato =  x.ManOrgContragent.Okato,
                    ContragentOktmo =  x.ManOrgContragent.Oktmo,
                    ContragentKpp =  x.ManOrgContragent.Kpp,
                    DecOpenDate = x.SpecialAccountDecision.OpenDate,
                    ProtocolDate = x.SpecialAccountDecision.PropertyOwnerProtocol.DocumentDate
                })
                .Filter(loadParams, Container);

             return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}