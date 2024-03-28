namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.ErknmTypeDocuments;

    using NHibernate.Util;

    public class ErknmTypeDocumentInterceptor: EmptyDomainInterceptor<ErknmTypeDocument>
    {
        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<ErknmTypeDocument> service, ErknmTypeDocument entity)
        {
            var knmReasonDomain = this.Container.ResolveDomain<KnmReason>();
            var erknmTypeDocumentKindCheckDomain = this.Container.ResolveDomain<ErknmTypeDocumentKindCheck>();
            using (this.Container.Using(knmReasonDomain, erknmTypeDocumentKindCheckDomain))
            {
                if (knmReasonDomain.GetAll().Any(x => x.ErknmTypeDocument.Id == entity.Id))
                {
                    return Failure("Существуют связанные записи в таблице: \"Основание проведения КНМ\"");
                }
                
                erknmTypeDocumentKindCheckDomain.GetAll()
                    .Where(x => x.ErknmTypeDocument == entity)
                    .ForEach(x => erknmTypeDocumentKindCheckDomain.Delete(x.Id));
            }
            return base.BeforeDeleteAction(service, entity);
        }
    }
}