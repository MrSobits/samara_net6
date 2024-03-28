using System.Linq;
using Bars.B4;
using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Regions.Nso.ViewModel
{
    using Bars.GkhGji.ViewModel;
    using Entities;

    public class NsoPrescriptionViewModel : PrescriptionViewModel<NsoPrescription>
    {
        public override IDataResult Get(IDomainService<NsoPrescription> domainService, BaseParams baseParams)
        {
            
            try
            {
                var id = baseParams.Params.GetAs<long>("id");
                
                var obj =
                    domainService.GetAll()
                                 .Where(x => x.Id == id)
                                 .Select(
                                     x =>
                                     new
                                     {
                                         x.Id,
                                         TypeBase = (x.Inspection != null ? x.Inspection.TypeBase : TypeBase.Default),
                                         InspectionId = (x.Inspection != null ? x.Inspection.Id : 0),
                                         Contragent = x.Contragent != null ? new Contragent { Id = x.Contragent.Id , ShortName = x.Contragent.ShortName } : null,
                                         x.TypeDocumentGji,
                                         x.Description,
                                         x.DocumentDate,
                                         x.DocumentNum,
                                         x.DocumentNumber,
                                         x.LiteralNum,
                                         x.DocumentSubNum,
                                         x.DocumentYear,
                                         x.State,
                                         x.CloseNote,
                                         x.CloseReason,
                                         x.Closed,
                                         x.DocumentPlace,
                                         DocumentTime = x.DocumentTime.HasValue ? x.DocumentTime.Value.ToShortTimeString() : "",
                                         Executant = x.Executant != null ? new ExecutantDocGji{ Id = x.Executant.Id , Name = x.Executant.Name } : null,
                                         x.Inspection,
                                         x.IsFamiliar,
                                         x.PhysicalPerson,
                                         x.PhysicalPersonInfo
                                     })
                                 .FirstOrDefault();

                return new BaseDataResult(obj);

            }
            finally
            {
                
            }
        }

    }
}