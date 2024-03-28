namespace Bars.GkhGji.Regions.Tula.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tula.Entities;

    public class ProtocolViewModel : GkhGji.ViewModel.ProtocolViewModel
    {
        public IDomainService<DocumentGJIPhysPersonInfo> DocumentGjiPhysPersonInfoDomain { get; set; }

        public override IDataResult Get(IDomainService<Protocol> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.Get(id);

            // Тут мы получаем постановление
            // Для этого сначала получаем id дочерних документов
            var listChildrenIds = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                .Where(x => x.Parent.Id == id)
                .Select(x => x.Children.Id)
                .Distinct()
                .ToList();

            // среди дочерних идентификаторов получаем id постановления
            obj.ResolutionId = this.Container.Resolve<IDomainService<Resolution>>().GetAll()
                .Where(x => listChildrenIds.Contains(x.Id))
                .Select(x => x.Id)
                .FirstOrDefault();

            var physPersonInfo = DocumentGjiPhysPersonInfoDomain.GetAll().FirstOrDefault(x => x.Document.Id == id);

            return new BaseDataResult(new
            {
                obj.Id,
                obj.Inspection,
                obj.Executant,
                obj.State,
                obj.Stage,
                obj.Contragent,
                obj.PhysicalPerson,
                obj.PhysicalPersonInfo,
                obj.DateToCourt,
                obj.ToCourt,
                obj.Description,
                obj.ViolationsList,
                obj.ParentDocumentsList,
                obj.TypeDocumentGji,
                obj.DocumentNumber,
                obj.DocumentDate,
                obj.ResolutionId,
                PhysPersonAddress = physPersonInfo != null ? physPersonInfo.PhysPersonAddress : null,
                PhysPersonJob = physPersonInfo != null ? physPersonInfo.PhysPersonJob : null,
                PhysPersonPosition = physPersonInfo != null ? physPersonInfo.PhysPersonPosition : null,
                PhysPersonBirthdayAndPlace = physPersonInfo != null ? physPersonInfo.PhysPersonBirthdayAndPlace : null,
                PhysPersonSalary = physPersonInfo != null ? physPersonInfo.PhysPersonSalary : null,
                PhysPersonDocument = physPersonInfo != null ? physPersonInfo.PhysPersonDocument : null,
                PhysPersonMaritalStatus = physPersonInfo != null ? physPersonInfo.PhysPersonMaritalStatus : null,
            });
        }
    }
}