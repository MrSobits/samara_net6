namespace Bars.Gkh.RegOperator.ViewModels.Contragent
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;

    public class ContragentViewModel : Gkh.DomainService.ContragentViewModel
    {
        public IDomainService<LegalAccountOwner> ContragentDomain { get; set; }

        protected override IQueryable<Gkh.Entities.Contragent> Filter(IQueryable<Gkh.Entities.Contragent> query, BaseParams baseParams)
        {
            var exceptLegalAccountOwners = baseParams.Params.ContainsKey("exceptLegalAccountOwners")
                && baseParams.Params["exceptLegalAccountOwners"].ToBool();

            //Исключить из результатов контрагентов, которые уже зарегистрированы в реестре абонентов.
            if (exceptLegalAccountOwners)
            {
                var accountOwners = ContragentDomain.GetAll();
                query = query.Where(c => !accountOwners.Any(o => o.Contragent == c));
            }
            return query;
        }
    }
}
