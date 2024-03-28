namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActCheck
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.ViewModel;

    public class ActCheckViewModel : ActCheckViewModel<ActCheck>
    {
        public override IDataResult Get(IDomainService<ActCheck> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);
            var docChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();

            try
            {
                var isExistsWarningDoc = docChildrenDomain.GetAll()
                    .Any(x => x.Parent.Id == id && x.Children.TypeDocumentGji == TypeDocumentGji.WarningDoc);

                return obj != null ? new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.ActCheckGjiRealityObject,
                        obj.ActToPres,
                        obj.Area,
                        obj.DateToProsecutor,
                        obj.DocumentDate,
                        obj.DocumentDateStr,
                        obj.DocumentNum,
                        obj.DocumentNumber,
                        obj.DocumentPlaceFias,
                        obj.LiteralNum,
                        obj.DocumentSubNum,
                        obj.DocumentYear,
                        obj.Flat,
                        Signer = obj.Signer?.Fio,
                        obj.ParentDocumentsList,
                        obj.RealityObjectsList,
                        obj.ResolutionProsecutor,
                        obj.Stage,
                        obj.ToProsecutor,
                        obj.State,
                        obj.TypeActCheck,
                        obj.TypeDocumentGji,
                        DocumentTime = obj.DocumentTime.HasValue ? obj.DocumentTime.Value.ToString("HH:mm") : string.Empty,
                        obj.AcquaintState,
                        obj.AcquaintedDate,
                        obj.RefusedToAcquaintPerson,
                        obj.AcquaintedPerson,
                        obj.AcquaintedPersonTitle,
                        obj.Inspection.TypeBase,
                        IsExistsWarningDoc = isExistsWarningDoc
                    }) : new BaseDataResult();
            }
            finally 
            {
                this.Container.Release(docChildrenDomain);
            }

            
        }
    }
}