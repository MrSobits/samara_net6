namespace Bars.Gkh.Modules.ClaimWork.Interceptors
{
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.RegOperator.Entities;

    public class DocumentClwInterceptor<T> : EmptyDomainInterceptor<T>
        where T : DocumentClw
    {
        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var documentDomain = this.Container.ResolveDomain<DocumentClwAccountDetail>();
            using (this.Container.Using(documentDomain))
            {
                documentDomain.GetAll()
                    .Where(x => x.Document.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList()
                    .ForEach(x => documentDomain.Delete(x));
            }

            return this.Success();
        }
    }
}
