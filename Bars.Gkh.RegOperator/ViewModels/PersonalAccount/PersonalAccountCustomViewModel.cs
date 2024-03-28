namespace Bars.Gkh.RegOperator.ViewModels.PersonalAccount
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    
    using Castle.Windsor;
    using Entities;
    using Enums;

    internal class PersonalAccountCustomViewModel : IPersonalAccountCustomViewModel
    {
        private readonly IWindsorContainer container;

        public PersonalAccountCustomViewModel(IWindsorContainer container)
        {
            this.container = container;
        }

        public DataResult.ListDataResult<AccountByRealityObjectDto> ListAccountByRealityObject(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAs<long>("roId");

            var loadParams = baseParams.GetLoadParam();

            var accountDomain = this.container.ResolveDomain<BasePersonalAccount>();

            var result = accountDomain.GetAll()
                .Where(x => x.Room.RealityObject.Id == roId)
                .Select(x => new
                {
                    x.Id,
                    x.AccountOwner
                })
                .ToArray()
                .GroupBy(x => x.AccountOwner.Id)
                .Select(x => x.First())
                .Select(x => new AccountByRealityObjectDto
                {
                    Id = x.Id,
                    AccountOwnerName =
                        x.AccountOwner.OwnerType == PersonalAccountOwnerType.Legal
                            ? x.AccountOwner.As<LegalAccountOwner>().Contragent.Name
                            : x.AccountOwner.Name
                })
                .AsQueryable()
                .Filter(loadParams, this.container);

            return new DataResult.ListDataResult<AccountByRealityObjectDto>(result.Order(loadParams).ToArray(), result.Count());
        }
    }
}