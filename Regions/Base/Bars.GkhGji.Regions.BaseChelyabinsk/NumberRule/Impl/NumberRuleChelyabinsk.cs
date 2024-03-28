namespace Bars.GkhGji.Regions.BaseChelyabinsk.NumberRule.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class NumberRuleChelyabinsk : INumberRuleChelyabinsk
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
                    docNum = this.DocumentGjiDomain.GetAll()
                        .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal)
                        .Where(x => x.DocumentDate.HasValue && document.DocumentDate.HasValue  &&
                            x.DocumentDate.Value.Year == document.DocumentDate.Value.Year)
                        .Where(x => x.DocumentNum.HasValue)
                        .Select(x => x.DocumentNum.Value)
                        .ToArray();
                    
                    break;
                default:
                    docNum = this.DocumentGjiInspectorDomain.GetAll()
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
                var inspectorIds = this.DocumentGjiInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == document.Id)
                    .Select(x => x.Inspector.Id);

                var inspectorId = inspectorIds.FirstOrDefault();
                if (inspectorIds.Count() > 1)
                {//если несколько инспекторов, то брать ответственного из родительского приказа
                    var respInspectorId = this.DisposalDomain.GetAll()
                        .Where(x => x.Inspection.Id == document.Inspection.Id)
                        .Select(x => x.ResponsibleExecution.Id)
                        .FirstOrDefault();

                    inspectorId = respInspectorId != 0 ? respInspectorId : inspectorId;
                }

                var docNum = this.GetDocNum(document, inspectorId);

                //КК
                var docCode = this.DocumentCodeDomain.GetAll()
                    .Where(x => x.Type == document.TypeDocumentGji)
                    .Select(x => x.Code)
                    .FirstOrDefault();

                //OO
                var departmentCode = this.ZonalInspectionInspectorDomain.GetAll()
                        .Where(x => x.Inspector.Id == inspectorId)
                        .Select(x => x.ZonalInspection.DepartmentCode)
                        .FirstOrDefault();

                //ИИИ
                var inspectorCode = inspectorId != 0 ? this.InspectorDomain.Get(inspectorId).Code : "";

                document.DocumentNumber = docNum + "/" + docCode + "-" + departmentCode + "-" + inspectorCode;
                document.DocumentNum = docNum.ToInt();
            }
        }
    }
}
