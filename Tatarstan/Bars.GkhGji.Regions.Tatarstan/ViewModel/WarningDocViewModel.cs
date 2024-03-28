using Bars.B4;
using Bars.Gkh.Domain;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using Bars.Gkh.Enums;

    public class WarningDocViewModel : BaseViewModel<WarningDoc>
    {
        public override IDataResult Get(IDomainService<WarningDoc> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("Id");
            var rec = domainService.Get(id);

            return new BaseDataResult(new
            {
                rec.Id,
                rec.Autor,
                rec.BaseWarning,
                rec.CompilationPlace,
                rec.DocumentDate,
                rec.DocumentNum,
                rec.DocumentNumber,
                rec.DocumentSubNum,
                rec.DocumentYear,
                rec.Executant,
                rec.File,
                rec.LiteralNum,
                rec.NcInDate,
                rec.NcInDateLatter,
                rec.NcInNum,
                rec.NcInNumLatter,
                rec.NcInRecived,
                rec.NcOutDate,
                rec.NcOutDateLatter,
                rec.NcOutNum,
                rec.NcOutNumLatter,
                rec.NcOutSent,
                rec.ObjectionReceived,
                rec.ResultText,
                rec.Stage,
                rec.State,
                rec.TakingDate,
                rec.TypeDocumentGji,
                rec.Inspection.TypeBase,
                rec.ActionStartDate,
                rec.ActionEndDate,
                SendToErknm = string.IsNullOrEmpty(rec.ErknmGuid) ? YesNo.No : YesNo.Yes,
                ErknmId = rec.ErknmGuid,
                rec.ErknmRegistrationNumber,
                rec.ErknmRegistrationDate
            });
        }
    }
}
