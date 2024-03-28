namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class SpecialAccountDecisionNoticeViewModel : BaseViewModel<SpecialAccountDecisionNotice>
    {
        public override IDataResult Get(IDomainService<SpecialAccountDecisionNotice> domainService, BaseParams baseParams)
        {
            //приходит id решения, а не id уведомления
            var decisionId = baseParams.Params["id"].To<long>();

            var value = domainService.GetAll().FirstOrDefault(x => x.SpecialAccountDecision.Id == decisionId);
                
            if (value == null)
            {
                var decisionDomain = this.Container.Resolve<IDomainService<SpecialAccountDecision>>();
                var decision = decisionDomain.Get(decisionId);
                value = new SpecialAccountDecisionNotice { SpecialAccountDecision = decision };
                domainService.Save(value);
                value = domainService.GetAll().First(x => x.SpecialAccountDecision.Id == decisionId);
            }

            var contragent =  value.SpecialAccountDecision.ManagingOrganization != null
                                       ? value.SpecialAccountDecision.ManagingOrganization.Contragent
                                       : null;

            var res = new
            {
                value.Id,
                value.State,
                value.NoticeNumber,
                value.NoticeDate,
                value.File,
                value.RegDate,
                value.GjiNumber,
                value.HasOriginal,
                value.HasCopyProtocol,
                value.HasCopyCertificate,
                value.SpecialAccountDecision,
                DecTypeOrganization = value.SpecialAccountDecision.TypeOrganization,
                ContragentName = contragent != null ? contragent.Name : null,
                ContragentOkato = contragent != null ? contragent.Okato : (int?)null,
                ContragentOktmo = contragent != null ? contragent.Oktmo : (int?)null,
                ContragentOgrn = contragent != null ? contragent.Ogrn : null,
                ContragentKpp = contragent != null ? contragent.Kpp : null,
                ContragentInn = contragent != null ? contragent.Inn : null,
                ContragentMailingAddress = contragent != null ? contragent.MailingAddress : null,

            };

            return new BaseDataResult(res);
        }
    }
}