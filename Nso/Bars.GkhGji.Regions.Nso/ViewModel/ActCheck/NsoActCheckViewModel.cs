namespace Bars.GkhGji.Regions.Nso.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.GkhGji.ViewModel;
    using Entities;

    public class NsoActCheckViewModel : ActCheckViewModel<NsoActCheck>
    {
        public override IDataResult Get(IDomainService<NsoActCheck> domainService, BaseParams baseParams)
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
                    DocumentTime = obj.DocumentTime.HasValue ? obj.DocumentTime.Value.ToShortTimeString() : string.Empty,
                }) : new BaseDataResult();
        }
    }
}