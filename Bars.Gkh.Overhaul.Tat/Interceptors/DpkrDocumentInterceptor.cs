namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class DpkrDocumentInterceptor : EmptyDomainInterceptor<DpkrDocument>
    {
        public IDomainService<DpkrDocumentRealityObject> DpkrDocumentRealityObjectService { get; set; }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<DpkrDocument> service, DpkrDocument entity)
        {
            DpkrDocumentRealityObjectService.GetAll()
                .Where(x => x.DpkrDocument.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => DpkrDocumentRealityObjectService.Delete(x));

            return base.BeforeDeleteAction(service, entity);
        }
    }
}