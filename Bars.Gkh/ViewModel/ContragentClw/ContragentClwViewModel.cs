using Bars.Gkh.Entities;

namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Bars.Gkh.Utils;

    public class ContragentClwViewModel : BaseViewModel<ContragentClw>
    {
        public override IDataResult List(IDomainService<ContragentClw> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            return domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.DateFrom,
                    x.DateTo,
                    Municipality = x.Contragent.Municipality.ParentMo == null ? x.Contragent.Municipality.Name : x.Contragent.Municipality.ParentMo.Name,
                    Settlement = x.Contragent.MoSettlement != null ? x.Contragent.MoSettlement.Name : (x.Contragent.Municipality.ParentMo != null ? x.Contragent.Municipality.Name : ""),
                    x.Contragent.Name,
                    x.Contragent.Inn,
                    x.Contragent.Kpp,
                    x.Contragent.Ogrn,
                    x.Contragent.ContragentState

                })
                .ToListDataResult(loadParams);
        }

        public override IDataResult Get(IDomainService<ContragentClw> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            return new BaseDataResult(
                domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.DateFrom,
                    x.DateTo,
                    Contragent = x.Contragent.Id,
                    x.Contragent.Name,
                    x.Contragent.ShortName,
                     x.Contragent.OrganizationForm,
                    x.Contragent.Inn,
                    x.Contragent.Kpp,
                    FactAddress = x.Contragent.FiasFactAddress.AddressName,
                    JuridicalAddress = x.Contragent.FiasJuridicalAddress.AddressName,
                    MailingAddress = x.Contragent.FiasMailingAddress.AddressName,
                    x.Contragent.DateRegistration,
                    x.Contragent.Ogrn
                })
                .FirstOrDefault()
            );
        }
    }
}