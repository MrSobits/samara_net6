namespace Bars.GkhGji.Regions.Stavropol.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActCheckService : GkhGji.DomainService.ActCheckService
    {
        public override IDataResult GetInfo(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");

            // получаем адрес дома и количество домов
            var realityObjAddress = string.Empty;
            var realityObjCount = 0;

            var actCheckRealityObjs = ActCheckRoDomain.GetAll().Where(x => x.ActCheck.Id == documentId).ToList();
            if (actCheckRealityObjs.Count == 1)
            {
                var actCheckRealityObj = actCheckRealityObjs.FirstOrDefault();
                realityObjCount = 1;
                if (actCheckRealityObj != null)
                {
                    realityObjAddress = actCheckRealityObj.RealityObject != null && actCheckRealityObj.RealityObject.FiasAddress != null ? actCheckRealityObj.RealityObject.FiasAddress.AddressName : string.Empty;
                }
            }

            // получаем основание проверки
            var typeBase = ActCheckDomain.GetAll()
                .Where(x => x.Id == documentId)
                .Select(x => new {x.Inspection.Id, x.Inspection.TypeBase})
                .FirstOrDefault();

            TypeBaseProsClaim prosClaimTypeBase = 0;

            var prosClaimDomain = Container.ResolveDomain<BaseProsClaim>();
            using (Container.Using(prosClaimDomain))
            {
                if (typeBase != null)
                {
                    prosClaimTypeBase = prosClaimDomain.GetAll()
                        .Where(x => x.Id == typeBase.Id)
                        .Select(x => (TypeBaseProsClaim?)x.TypeBaseProsClaim)
                        .FirstOrDefault()
                        .GetValueOrDefault();
                }
            }

            var dataInspectors = InspectorDomain.GetInspectorsByDocumentId(documentId)
                .Select(x => new
                {
                    InspectorId = x.Inspector.Id,
                    x.Inspector.Fio
                })
                .ToList();

            var inspectorNames = dataInspectors.AggregateWithSeparator(x => x.Fio, ", ");
            var inspectorIds = dataInspectors.AggregateWithSeparator(x => x.InspectorId, ", ");

            // Получаем Признак выявлены или не выявлены нарушения?
            var isExistViolation = ActCheckViolDomain.GetAll().Any(x => x.Document.Id == documentId);

            // получаем вид проверки из приказа (распоряжения)
            var parentDisposalKindCheck = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                .Where(x => x.Children.Id == documentId && x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                .Select(x => (x.Parent as Disposal).KindCheck)
                .FirstOrDefault();

            var typeCheck = parentDisposalKindCheck != null ? parentDisposalKindCheck.Code : 0;

            return new BaseDataResult(new
            {
                inspectorNames, 
                inspectorIds,
                typeBase = typeBase.Return(x => x.TypeBase),
                prosClaimTypeBase, 
                realityObjAddress, 
                realityObjCount, 
                isExistViolation, 
                typeCheck
            });
        }
    }
}
