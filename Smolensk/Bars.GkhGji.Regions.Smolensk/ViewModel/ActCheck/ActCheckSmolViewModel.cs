namespace Bars.GkhGji.Regions.Smolensk.ViewModel
{
    using Bars.B4;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    public class ActCheckSmolViewModel : Bars.GkhGji.ViewModel.ActCheckViewModel<ActCheckSmol>
    {
        public override IDataResult Get(IDomainService<ActCheckSmol> domainService, BaseParams baseParams)
        {
            var data = base.Get(domainService, baseParams);

            var obj = (ActCheckSmol)data.Data;

            return
                    new BaseDataResult(
                        new
                        {
                            obj.Id,
                            DocumentId = obj.Id,
                            obj.Stage,
                            obj.State,
                            obj.DocumentDate,
                            obj.DocumentDateStr,
                            obj.DocumentNum,
                            obj.DocumentNumber,
                            obj.LiteralNum,
                            obj.DocumentSubNum,
                            obj.DocumentYear,
                            obj.TypeDocumentGji,
                            obj.TypeActCheck,
                            Inspection = obj.Inspection.Id,
                            obj.ResolutionProsecutor,
                            obj.ToProsecutor,
                            obj.DateToProsecutor,
                            obj.Area,
                            InspectionId = obj.Inspection.Id,
                            InspectionContragentMoId =
                                obj.Inspection.Contragent != null && obj.Inspection.Contragent.Municipality != null ? obj.Inspection.Contragent.Municipality.Id : 0,
                            obj.Flat,
                            obj.ActToPres,
                            obj.HaveViolation,
                            obj.DateNotification,
                            obj.NumberNotification
                        });
        }

    }
}
