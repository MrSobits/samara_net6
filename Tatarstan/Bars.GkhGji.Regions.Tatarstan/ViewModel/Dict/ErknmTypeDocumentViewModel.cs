namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.ErknmTypeDocuments;

    public class ErknmTypeDocumentViewModel: BaseViewModel<ErknmTypeDocument>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ErknmTypeDocument> domainService, BaseParams baseParams)
        {
            var kindCheckId = baseParams.Params.GetAsId("kindCheckId");
            
            var typeDocumentsErknmKindCheckDomain = this.Container.ResolveDomain<ErknmTypeDocumentKindCheck>();
            using (this.Container.Using(typeDocumentsErknmKindCheckDomain))
            {
                var kindCheck = typeDocumentsErknmKindCheckDomain.GetAll()
                    .AsEnumerable()
                    .GroupBy(x => x.ErknmTypeDocument.Id)
                    .ToDictionary(x => x.Key, x => x.Select(y => new { y.KindCheck.Id, y.KindCheck.Name }));

                var docTypeIds = kindCheckId > 0
                    ? kindCheck
                        .Where(x => x.Value.Any(y => y.Id == kindCheckId))
                        .Select(x => x.Key)
                        .ToHashSet()
                    : null;

                return domainService.GetAll()
                    .WhereIf(docTypeIds != null, x => docTypeIds.Contains(x.Id))
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.Code,
                        x.DocumentType,
                        x.IsBasisKnm,
                        KindCheck = kindCheck.ContainsKey(x.Id)
                            ? kindCheck[x.Id] : null
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }
    }
}