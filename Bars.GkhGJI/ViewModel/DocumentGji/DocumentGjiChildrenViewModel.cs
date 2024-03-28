namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using B4.Utils;
    using Bars.GkhGji.Entities;

    public class DocumentGjiChildrenViewModel : BaseViewModel<DocumentGjiChildren>
    {
        public override IDataResult List(IDomainService<DocumentGjiChildren> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var parentDocumentId = baseParams.Params.ContainsKey("parentDocumentId")
                                  ? baseParams.Params["parentDocumentId"].ToLong()
                                  : 0;

            var childrenDocumentId = baseParams.Params.ContainsKey("childrenDocumentId")
                                  ? baseParams.Params["childrenDocumentId"].ToLong()
                                  : 0;

            if (parentDocumentId > 0)
            {
                var data = domainService.GetAll()
                    .Where(x => x.Parent.Id == parentDocumentId)
                    .Select(x => new
                    {
                        x.Id,
                        ParentId = x.Parent.Id,
                        ChildrenId = x.Children.Id,
                        x.Children.TypeDocumentGji,
                        DocumentId = x.Children.Id,
                        x.Children.DocumentDate,
                        x.Children.DocumentNum,
                        x.Children.DocumentNumber,
                        x.Children.DocumentSubNum,
                        x.Children.DocumentYear
                    });

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }

            if (childrenDocumentId > 0)
            {
                var data = domainService.GetAll()
                    .Where(x => x.Children.Id == childrenDocumentId)
                    .Select(x => new
                    {
                        x.Id,
                        ParentId = x.Parent.Id,
                        ChildrenId = x.Children.Id,
                        x.Parent.TypeDocumentGji,
                        DocumentId = x.Parent.Id,
                        x.Parent.DocumentDate,
                        x.Parent.DocumentNum,
                        x.Parent.DocumentNumber,
                        x.Parent.DocumentSubNum,
                        x.Parent.DocumentYear
                    });

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }

            return new ListDataResult(null, 0);
        }
    }
}