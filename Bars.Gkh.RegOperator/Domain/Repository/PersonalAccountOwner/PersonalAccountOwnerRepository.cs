namespace Bars.Gkh.RegOperator.Domain.Repository
{
    using System.Linq;
    using B4;

    using Bars.Gkh.Contracts.Params;

    using Entities;
    using Castle.Windsor;
    using Bars.B4.Utils;

    public class PersonalAccountOwnerRepository : IPersonalAccountOwnerRepository
    {
        private readonly IWindsorContainer _container;
        private readonly IDomainService<LegalAccountOwner> _domain;

        public PersonalAccountOwnerRepository(
            IWindsorContainer container,
            IDomainService<LegalAccountOwner> domain)
        {
            _container = container;
            _domain = domain;
        }

        #region Implementation of IPersonalAccountOwnerRepository

        public GenericListResult<LegalAccountOwner> ListLegalOwners(BaseParams @params)
        {
            var loadParam = @params.GetLoadParam();
            var inn = @params.Params.GetAs<string>("inn", "");
            var isPicking = @params.Params.GetAs<bool>("isPicking", false);

            var data = _domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Contragent,
                    x.Contragent.Inn,
                    x.Contragent.Kpp,
                    x.Contragent.Name,
                    x.OwnerType,
                    x.PrintAct,
                    x.PrivilegedCategory
                })
                .Filter(loadParam, _container);

            var result = data.Order(loadParam).Paging(loadParam)
                .ToList()
                .Select(x => new LegalAccountOwner
                {
                    Id = x.Id,
                    Contragent = x.Contragent,
                    Inn = x.Inn,
                    Kpp = x.Kpp,
                    OwnerType = x.OwnerType,
                    PrintAct = x.PrintAct,
                    PrivilegedCategory = x.PrivilegedCategory
                });

            if (isPicking)
            {
                result = result.WhereIf(inn != "", x => x.Inn == inn);
            }

            return new GenericListResult<LegalAccountOwner>(result, data.Count());
        }

        #endregion
    }
}