using Bars.B4.Utils;
using Castle.Core.Internal;

namespace Bars.GkhGji.Regions.Nso.NumberRule
{
    using B4.DataAccess;
    using GkhGji.Entities;
    using System.Linq;
    using B4;

    using Bars.GkhGji.Entities.Dict;

    using Gkh.Entities;
    using Entities;
    using Castle.Windsor;
    using GkhGji.Enums;

    public class NumberRuleNso : INumberRuleNso
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomain { get; set; }
        public IDomainService<Inspector> InspectorDomain { get; set; }
        public IDomainService<ZonalInspectionInspector> ZonalInspectionInspectorDomain { get; set; }
        public IDomainService<DocumentCode> DocumentCodeDomain { get; set; }
        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }
        public IDomainService<Disposal> DisposalDomain { get; set; }

        public int GetDocNum(DocumentGji document, long inspectorId)
        {
            int[] docNum;

            switch (document.TypeDocumentGji)
            {
                case TypeDocumentGji.Disposal:
                    docNum = DocumentGjiDomain.GetAll()
                        .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal)
                        .Where(x => x.DocumentDate.HasValue && document.DocumentDate.HasValue  &&
                            x.DocumentDate.Value.Year == document.DocumentDate.Value.Year)
                        .Where(x => x.DocumentNum.HasValue)
                        .Select(x => x.DocumentNum.Value)
                        .ToArray();
                    
                    break;
                default:
                    docNum = DocumentGjiInspectorDomain.GetAll()
                        .Where(x => x.DocumentGji != null && document != null &&
                            x.DocumentGji.DocumentDate.HasValue && document.DocumentDate.HasValue &&
                            x.DocumentGji.DocumentDate.Value.Year == document.DocumentDate.Value.Year)
                        .Where(x => x.Inspector.Id == inspectorId)
                        .Where(x => x.DocumentGji.DocumentNum.HasValue)
                        .Select(x => x.DocumentGji.DocumentNum.Value)
                        .ToArray();
                    break;
            }

            return docNum.Any() ? docNum.Max() + 1 : 1;
        }

        public void SetNumber(IEntity entity)
        {
            //Все документы будут нумероваться по маске ННН/КК-ОО-ИИИ,
            var document = entity as DocumentGji;
            if (document != null && document.DocumentNumber.IsEmpty())
            {
                //ННН
                var inspectorIds = DocumentGjiInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == document.Id)
                    .Select(x => x.Inspector.Id);

                var inspectorId = inspectorIds.FirstOrDefault();
                if (inspectorIds.Count() > 1)
                {//если несколько инспекторов, то брать ответственного из родительского приказа
                    var respInspectorId = DisposalDomain.GetAll()
                        .Where(x => x.Inspection.Id == document.Inspection.Id)
                        .Select(x => x.ResponsibleExecution.Id)
                        .FirstOrDefault();

                    inspectorId = respInspectorId != 0 ? respInspectorId : inspectorId;
                }

                var docNum = GetDocNum(document, inspectorId);

                //КК
                var docCode = DocumentCodeDomain.GetAll()
                    .Where(x => x.Type == document.TypeDocumentGji)
                    .Select(x => x.Code)
                    .FirstOrDefault();

                //OO
                var departmentCode = ZonalInspectionInspectorDomain.GetAll()
                        .Where(x => x.Inspector.Id == inspectorId)
                        .Select(x => x.ZonalInspection.DepartmentCode)
                        .FirstOrDefault();

                //ИИИ
                var inspectorCode = inspectorId != 0 ? InspectorDomain.Get(inspectorId).Code : "";

                document.DocumentNumber = docNum + "/" + docCode + "-" + departmentCode + "-" + inspectorCode;
                document.DocumentNum = docNum.ToInt();
            }
        }
    }
}
