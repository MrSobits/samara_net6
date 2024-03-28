using System.Linq;
using Bars.B4;
using Bars.B4.Modules.States;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Castle.Windsor;

namespace Bars.GkhGji.Regions.Nso.StateChange
{
	using Bars.GkhGji.Entities.Dict;

	public abstract class BaseDocNumberValidationNsoRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<Inspector> InspectorDomain { get; set; }
        public IDomainService<ZonalInspectionInspector> ZonalInspectionInspectorDomain { get; set; }
        public IDomainService<DocumentCode> DocumentCodeDomain { get; set; }
        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }
        public IDomainService<Disposal> DisposalDomain { get; set; }

        public abstract string Id { get; }

        public abstract string Name { get; }

        public abstract string TypeId { get; }

        public abstract string Description { get; }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var result = new ValidateResult();

            var document = statefulEntity as DocumentGji;

            if (document != null)
            {
                if (document.DocumentDate == null)
                {
                    result.Message = "Невозможно сформировать номер, поскольку дата документа не указана";
                    result.Success = false;
                    return result;
                }

                Inspector inspector = null;

                if (document.TypeDocumentGji == TypeDocumentGji.Disposal)
                {
                    inspector = DisposalDomain.GetAll()
                        .Where(x => x.Id == document.Id)
                        .Select(x => x.ResponsibleExecution)
                        .FirstOrDefault();
                }
                else
                {
                    var disposal = GetParentDocument(document, TypeDocumentGji.Disposal);

                    if (disposal != null)
                    {
                        inspector = DisposalDomain.GetAll()
                            .Where(x => x.Id == disposal.Id)
                            .Select(x => x.ResponsibleExecution)
                            .FirstOrDefault();
                    }
                }

                if (inspector == null)
                {
                    result.Message = "Невозможно сформировать номер, поскольку в приказе  не указан 'Ответственный за исполнение'";
                    result.Success = false;
                    return result;
                }

                var docCode = DocumentCodeDomain.GetAll()
                    .Where(x => x.Type == document.TypeDocumentGji)
                    .Select(x => x.Code)
                    .FirstOrDefault();

                if (docCode == 0)
                {
                    result.Message = "Невозможно сформировать номер, поскольку в справочнике не указан код проверки";
                    result.Success = false;
                    return result;
                }

                var departmentCode = ZonalInspectionInspectorDomain.GetAll()
                    .Where(x => x.Inspector.Id == inspector.Id)
                    .Select(x => x.ZonalInspection.DepartmentCode)
                    .FirstOrDefault();


                if (string.IsNullOrEmpty(departmentCode))
                {
                    result.Message = "Невозможно сформировать номер, поскольку невозможно определить код отдела";
                    result.Success = false;
                    return result;
                }

                Action(document, inspector, docCode, departmentCode);
            }

            result.Success = true;

            return result;
        }

        protected virtual void Action(DocumentGji document, Inspector inspector, int docCode, string departmentCode)
        {
            var docNum = GetDocNum(document);

            var docCodeStr = docCode < 10 ? "0" + docCode : docCode.ToStr();

            document.DocumentNumber = docNum + "/" + docCodeStr + "-" + departmentCode + "-" + inspector.Code;
            document.DocumentNum = docNum;
        }


        private int GetDocNum(DocumentGji document)
        {
            int docNum;

            switch (document.TypeDocumentGji)
            {
                case TypeDocumentGji.Disposal:
                    docNum = DocumentGjiDomain.GetAll()
                        .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal)
                        .Where(x => x.DocumentDate.HasValue && document.DocumentDate.HasValue &&
                            x.DocumentDate.Value.Year == document.DocumentDate.Value.Year)
                        .Where(x => x.DocumentNum.HasValue)
                        .Select(x => x.DocumentNum.Value)
                        .SafeMax(x => x);

                    break;
                default:
                {
                    // получаем ответственного инспектора
                   var respInspectorId = DisposalDomain.GetAll()
                        .Where(x => x.Stage.Id == document.Stage.Parent.Id)
                        .Select(x => x.ResponsibleExecution.Id)
                        .FirstOrDefault();

                   // получаем все проверки данного инспектора
                    var docQuery = DisposalDomain.GetAll()
                        .Where(x => x.ResponsibleExecution.Id == respInspectorId);
                          
                    docNum = DocumentGjiDomain.GetAll()
                        .Where(x =>
                            x.DocumentDate.HasValue && document.DocumentDate.HasValue &&
                            x.DocumentDate.Value.Year == document.DocumentDate.Value.Year)
                        .Where(x => docQuery.Any(y => y.Stage.Id == x.Stage.Parent.Id))
                        .Where(x => x.TypeDocumentGji == document.TypeDocumentGji)
                        .Where(x => x.DocumentNum.HasValue)
                        .Select(x => x.DocumentNum.Value)
                        .SafeMax(x => x);
                    break;
                }
            }

            return ++docNum;
        }

        public DocumentGji GetParentDocument(DocumentGji document, TypeDocumentGji type)
        {
            var result = document;

            if (document.TypeDocumentGji != type)
            {
                var docs = DocumentGjiChildrenDomain.GetAll()
                                    .Where(x => x.Children.Id == document.Id)
                                    .Select(x => x.Parent)
                                    .ToList();

                foreach (var doc in docs)
                {
                    result = GetParentDocument(doc, type);
                }
            }

            if (result != null)
            {
                return result.TypeDocumentGji == type ? result : null;
            }

            return null;
        }
    }
}