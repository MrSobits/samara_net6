namespace Bars.Gkh.RegOperator.ViewModels.Owner
{
    using System;
    using System.Linq;

    using B4;

    using Bars.B4.Utils;
    using Bars.Gkh.Utils;

    using Entities;
    using Enums;
    using Gkh.Domain;

    public class PersonalAccountOwnerViewModel : BaseViewModel<PersonalAccountOwner>
    {
        public IDomainService<IndividualAccountOwner> IndividualAccountOwnerDomainService { get; set; } 

        public IDomainService<LegalAccountOwner> LegalAccountOwnerDomainService { get; set; }

        public IDomainService<BasePersonalAccount> BasePersonalAccountDomainService { get; set; }

        public override IDataResult Get(IDomainService<PersonalAccountOwner> domainService, BaseParams baseParams)
        {
            var value = domainService.Get(baseParams.Params.GetAsId());
            if (value.OwnerType == PersonalAccountOwnerType.Individual)
            {
                return new BaseDataResult(this.IndividualAccountOwnerDomainService.Get(value.Id));
            } 
            if (value.OwnerType == PersonalAccountOwnerType.Legal)
            {
                return new BaseDataResult(this.LegalAccountOwnerDomainService.Get(value.Id));
            }
            
            return new BaseDataResult(value);
        }
        public override IDataResult List(IDomainService<PersonalAccountOwner> domainService, BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);
            var roomId = baseParams.Params.GetAsId("RoomId");
            return domainService.GetAll()
                .WhereIf(roomId > 0, pao => pao.Accounts.Any(acc => acc.Room.Id == roomId))
                .Select(x => new
                    {
                        x.Id,
                        x.OwnerType,
                        x.Name,
                        AccountsCount = x.TotalAccountsCount,
                        x.TotalAccountsCount,
                        x.ActiveAccountsCount,
                        FiasFactAddress = (x as IndividualAccountOwner).FiasFactAddress.AddressName,
                        BirthDate = (DateTime?)(x as IndividualAccountOwner).BirthDate
                    })
                    .ToListDataResult(loadParam);
        }
    }
}