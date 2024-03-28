namespace Bars.GkhGji.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Bars.GkhGji.Entities;


    public class DocNumValidationRuleInterceptor : EmptyDomainInterceptor<DocNumValidationRule>
    {
        public override IDataResult BeforeCreateAction(IDomainService<DocNumValidationRule> service, DocNumValidationRule entity)
        {
            if (service.GetAll().Any(x => x.TypeDocumentGji == entity.TypeDocumentGji && x.Id != entity.Id))
            {
                return Failure("Правило для выбранного типа документа уже добавлено!");
            }

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<DocNumValidationRule> service, DocNumValidationRule entity)
        {
            return this.BeforeCreateAction(service, entity);
        }
    }
}