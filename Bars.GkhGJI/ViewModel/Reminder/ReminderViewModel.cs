namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ReminderViewModel : BaseViewModel<Reminder>
    {
        public override IDataResult Get(IDomainService<Reminder> domainService, BaseParams baseParams)
        {
            var value = domainService
                .GetAll()
                .Select(x => new
                                 {
                                     x.Id,
                                     InspectionGjiId = x.InspectionGji != null ? x.InspectionGji.Id : 0,
                                     InspectionGjiTypeBase = x.InspectionGji != null ? x.InspectionGji.TypeBase : TypeBase.Default,
                                     DocumentGjiId = x.DocumentGji != null ? x.DocumentGji.Id : 0,
                                     TypeDocumentGji = x.DocumentGji != null ? x.DocumentGji.TypeDocumentGji : TypeDocumentGji.Disposal,
                                     AppealCitsId =  x.AppealCits != null ? x.AppealCits.Id :0,
                                     x.Contragent,
                                     x.Inspector,
                                     x.Actuality,
                                     x.TypeReminder,
                                     x.CategoryReminder,
                                     x.Num,
                                     x.CheckDate
                                 })
                .FirstOrDefault(x => x.Id == baseParams.Params["id"].To<long>());

            var val = value != null
                          ? new
                                {
                                    value.Id,
                                    value.TypeReminder,
                                    DocumentGji =
                                new { Id = value.DocumentGjiId, TypeDocumentGji = value.TypeDocumentGji },
                                    InspectionGji = new
                                                     {
                                                         Id = value.InspectionGjiId,
                                                         TypeBase = value.InspectionGjiTypeBase
                                                     },
                                                     AppealCits = new
                                                                      {
                                                                          Id = value.AppealCitsId
                                                                      }
                                                     
                                }
                          : null;
            return new BaseDataResult(val);
        }
    }
}