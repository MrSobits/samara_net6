namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.ActCheck
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;
    using Bars.GkhGji.ViewModel;

    public class ChelyabinskActCheckViewModel : ActCheckViewModel<ChelyabinskActCheck>
    {
        public override IDataResult Get(IDomainService<ChelyabinskActCheck> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return obj != null ? new BaseDataResult(
                new
                {
                    obj.Id,
                    obj.AcquaintedWithDisposalCopy,
                    obj.ActCheckGjiRealityObject,
                    obj.ActToPres,
                    obj.Area,
                    obj.DateToProsecutor,
                    obj.DocumentDate,
                    obj.DocumentDateStr,
                    obj.DocumentNum,
                    obj.DocumentNumber,
                    obj.DocumentPlace,
                    obj.Unavaliable,
                    obj.UnavaliableComment,
                    obj.LiteralNum,
                    obj.DocumentSubNum,
                    obj.DocumentYear,
                    obj.Flat,
                    obj.ParentDocumentsList,
                    obj.RealityObjectsList,
                    obj.ResolutionProsecutor,
                    obj.Stage,
                    obj.ToProsecutor,
                    obj.State,
                    obj.TypeActCheck,
                    obj.TypeDocumentGji,
                    obj.TypeCheckAct,
                    obj.SignatoryInspector,
                    DocumentTime = obj.DocumentTime.HasValue ? obj.DocumentTime.Value.ToShortTimeString() : string.Empty,
                }) : new BaseDataResult();
        }
    }
}