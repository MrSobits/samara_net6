namespace Bars.GkhGji.Regions.Tula.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tula.Entities;

    public class ResolutionViewModel : GkhGji.DomainService.ResolutionViewModel
    {
        public IDomainService<DocumentGJIPhysPersonInfo> DocumentGjiPhysPersonInfoDomain { get; set; }

        public override IDataResult Get(IDomainService<Resolution> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.Get(id);

            var physPersonInfo = DocumentGjiPhysPersonInfoDomain.GetAll().FirstOrDefault(x => x.Document.Id == id);

            return new BaseDataResult(new
            {
                obj.Id,
                obj.Inspection,
                obj.Executant,
                obj.Municipality,
                obj.Contragent,
                obj.Sanction,
                obj.Official,
                obj.PhysicalPerson,
                obj.PhysicalPersonInfo,
                obj.DeliveryDate,
                obj.TypeInitiativeOrg,
                obj.SectorNumber,
                obj.PenaltyAmount,
                obj.ParentDocumentsList,
                obj.TypeDocumentGji,
                obj.Paided,
                obj.DateTransferSsp,
                obj.DocumentNumSsp,
                obj.DocumentDate,
                obj.State,
                obj.DocumentNumber,
                obj.DocumentNum,
                PhysPersonAddress = physPersonInfo != null ? physPersonInfo.PhysPersonAddress : null,
                PhysPersonJob = physPersonInfo != null ? physPersonInfo.PhysPersonJob : null,
                PhysPersonPosition = physPersonInfo != null ? physPersonInfo.PhysPersonPosition : null,
                PhysPersonBirthdayAndPlace = physPersonInfo != null ? physPersonInfo.PhysPersonBirthdayAndPlace : null,
                PhysPersonSalary = physPersonInfo != null ? physPersonInfo.PhysPersonSalary : null,
                PhysPersonDocument = physPersonInfo != null ? physPersonInfo.PhysPersonDocument : null,
                PhysPersonMaritalStatus = physPersonInfo != null ? physPersonInfo.PhysPersonMaritalStatus : null,
                obj.Description,
                obj.BecameLegal
            });
        }
    }
}