﻿namespace Bars.Gkh.Overhaul.Nso.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class DecisionNoticeExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var decisionNoticeDomain = Container.Resolve<IDomainService<SpecialAccountDecisionNotice>>();

            var loadParams = GetLoadParam(baseParams);

            return decisionNoticeDomain.GetAll()
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
                    ContragentName = x.ManOrgContragent.Name,
                    ContragentMailingAddress = x.ManOrgContragent.MailingAddress,
                    ContragentOgrn = x.ManOrgContragent.Ogrn,
                    ContragentInn = x.ManOrgContragent.Inn,
                    ContragentOkato = x.ManOrgContragent.Okato,
                    ContragentKpp =  x.ManOrgContragent.Kpp,
                    DecOpenDate = x.SpecialAccountDecision.OpenDate,
                    ProtocolDate = x.SpecialAccountDecision.PropertyOwnerProtocol.DocumentDate
                })
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();
        }
    }
}