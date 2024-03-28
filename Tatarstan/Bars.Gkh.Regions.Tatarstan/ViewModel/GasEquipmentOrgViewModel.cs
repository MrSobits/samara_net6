using System.Linq;

using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Regions.Tatarstan.Entities.GasEquipmentOrg;
using Bars.Gkh.Utils;

namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using Bars.Gkh.Domain;

    public class GasEquipmentOrgViewModel : BaseViewModel<GasEquipmentOrg>
    {
        public override IDataResult List(IDomainService<GasEquipmentOrg> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            return domainService.GetAll()
                .Select(
                    x => new
                    {
                        x.Id,
                        Municipality = x.Contragent.Municipality.Name,
                        Contragent = x.Contragent.ShortName,
                        x.Contragent.Inn,
                        x.Contragent.Kpp,
                        x.Contragent.Ogrn,
                        x.Contragent.JuridicalAddress,
                        x.Contragent.DateRegistration,
                        x.Contragent.DateTermination
                    })
                .ToListDataResult(loadParams, this.Container);
        }

        public override IDataResult Get(IDomainService<GasEquipmentOrg> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            return new BaseDataResult(
                domainService.GetAll()
                    .Select(
                        x => new
                        {
                            x.Id,
                            Municipality = x.Contragent.Municipality.Name,
                            Contragent = x.Contragent.ShortName,
                            x.Contragent.Inn,
                            x.Contragent.Kpp,
                            x.Contragent.Ogrn,
                            x.Contragent.JuridicalAddress,
                            x.Contragent.Phone,
                            x.Contragent.Name,
                            x.Contragent.DateRegistration,
                            x.Contragent.DateTermination,
                            ContragentId = x.Contragent.Id,
                            x.Contact
                        })
                    .FirstOrDefault(x => x.Id == id)
            );
        }
    }
}
