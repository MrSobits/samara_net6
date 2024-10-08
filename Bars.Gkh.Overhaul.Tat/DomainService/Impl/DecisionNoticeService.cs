﻿using Bars.B4.Utils;
using Bars.Gkh.Overhaul.Tat.Enum;

namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using Enums;
    using DomainService;
    using Entities;

    using Castle.Windsor;

    public class DecisionNoticeService : IDecisionNoticeService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<RegOpAccountDecision> RegopDecisionDomain { get; set; }

        public IDomainService<SpecialAccountDecisionNotice> DecisionNoticeDomain { get; set; }

        public IDomainService<SpecialAccountDecision> SpecialAccountDecisionDomain { get; set; }

        public IDomainService<BasePropertyOwnerDecision> BasePropertyOwnerDecision { get; set; }

        public IDataResult ListRegister(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var ids = BasePropertyOwnerDecision.GetAll()
                .Where(x => x.PropertyOwnerProtocol != null && x.PropertyOwnerProtocol.DocumentDate != null)
                .Where(x => x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SelectMethodForming)
                .Select(x => new
                {
                    x.Id,
                    x.PropertyOwnerProtocol.DocumentDate,
                    roId = x.RealityObject.Id,
                    x.MethodFormFund
                })
                .ToList()
                .GroupBy(x => x.roId)
                .Select(x => new
                {
                    x.Key,
                    x.OrderByDescending(z => z.DocumentDate).First().MethodFormFund,
                    x.OrderByDescending(z => z.DocumentDate).First().Id
                })
                .Where(x => x.MethodFormFund.HasValue && x.MethodFormFund.Value == MethodFormFundCr.SpecialAccount)
                .Select(x => x.Id)
                .ToArray();


            var data = DecisionNoticeDomain.GetAll()
                .Where(x => x.SpecialAccountDecision.MethodFormFund == MethodFormFundCr.SpecialAccount)
                .Where(y => ids.Contains(y.SpecialAccountDecision.Id))
                .Select(x => new
                {
                    x.Id,
                    x.SpecialAccountDecision,
                    x.State,
                    x.RegDate,
                    x.GjiNumber,
                    x.NoticeNumber,
                    x.NoticeDate,
                    RoId = x.SpecialAccountDecision.RealityObject.Id,
                    x.SpecialAccountDecision.RealityObject.Address,
                    RegOpContragent = x.SpecialAccountDecision.RegOperator.Contragent,
                    ManOrgContragent = x.SpecialAccountDecision.ManagingOrganization.Contragent
                })
                .Select(x => new
                {
                    x.Id,
                    SpecialAccountDecision = x.SpecialAccountDecision.Id,
                    x.State,
                    RealityObject = new {Id = x.RoId, x.Address},
                    MunicipalityName = x.SpecialAccountDecision.RealityObject.Municipality.Name,
                    x.Address,
                    ContragentName = x.RegOpContragent.Name ?? x.ManOrgContragent.Name,
                    ContragentMailingAddress = x.RegOpContragent.MailingAddress ?? x.ManOrgContragent.MailingAddress,
                    ContragentOgrn = x.RegOpContragent.Ogrn ?? x.ManOrgContragent.Ogrn,
                    ContragentInn = x.RegOpContragent.Inn ?? x.ManOrgContragent.Inn,
                    ContragentOkato = x.RegOpContragent.Okato ?? x.ManOrgContragent.Okato,
                    ContragentOktmo = x.RegOpContragent.Oktmo ?? x.ManOrgContragent.Oktmo,
                    ContragentKpp = x.RegOpContragent.Kpp ?? x.ManOrgContragent.Kpp,
                    x.RegDate,
                    x.GjiNumber,
                    x.NoticeNumber,
                    x.NoticeDate,
                    MethodFormFund = "На специальном счете",
                    DecOpenDate = x.SpecialAccountDecision.OpenDate,
                    ProtocolDate = x.SpecialAccountDecision.PropertyOwnerProtocol.DocumentDate,
                    x.SpecialAccountDecision.AccountNumber
                })
                .Filter(loadParams, Container);

             return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}