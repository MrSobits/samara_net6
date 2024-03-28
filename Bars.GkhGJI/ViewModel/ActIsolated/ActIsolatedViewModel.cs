namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    public class ActIsolatedViewModel : BaseViewModel<ActIsolated>
    {
        public override IDataResult Get(IDomainService<ActIsolated> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);
            var docChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();

            try
            {
                return obj != null ? new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.Area,
                        obj.DocumentDate,
                        obj.DocumentDateStr,
                        obj.DocumentNum,
                        obj.DocumentNumber,
                        obj.DocumentPlaceFias,
                        obj.LiteralNum,
                        obj.DocumentSubNum,
                        obj.DocumentYear,
                        obj.Flat,
                        obj.Stage,
                        obj.State,
                        obj.TypeDocumentGji,
                        DocumentTime = obj.DocumentTime.HasValue ? obj.DocumentTime.Value.ToString("HH:mm") : string.Empty,
                        obj.Inspection.TypeBase
                    }) : new BaseDataResult();
            }
            finally 
            {
                this.Container.Release(docChildrenDomain);
            }
        }
    }
}