namespace Bars.GkhGji.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;
    using NumberValidation;

#warning запрос надо исправить
    public class DocNumValidationRuleViewModel : BaseViewModel<DocNumValidationRule>
    {
        public override IDataResult List(IDomainService<DocNumValidationRule> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var numberValidations = Container.ResolveAll<INumberValidationRule>();

            var data = domainService.GetAll()
                .Select(x => new
                    {
                        x.Id,
                        x.RuleId,
                        x.TypeDocumentGji
                    })
                .AsEnumerable()
                .Select(x => new
                    {
                        x.Id,
                        x.TypeDocumentGji,
                        numberValidations.FirstOrDefault(y => y.Id == x.RuleId).Name
                    })
                .AsQueryable()
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}