using System;
using System.Linq;
using Bars.B4;
using Bars.B4.Modules.States;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.Nso.Entities;
using Castle.Windsor;

namespace Bars.GkhGji.Regions.Nso.StateChange
{
	using Bars.GkhGji.Entities.Dict;

	public abstract class SahalinBaseDocNumberValidationNsoRule : IRuleChangeStatus
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

                try
                {
                    GetDocNum(document);
                }
                catch (Exception exc)
                {
                    result.Message = exc.Message;
                    result.Success = false;
                    return result;
                }
                
            }

            result.Success = true;

            return result;
        }

        private void GetDocNum(DocumentGji document)
        {

            var docNumber = string.Empty;
            var docNum = 0;
            var docSubNum = 0;

            switch (document.TypeDocumentGji)
            {
                case TypeDocumentGji.Disposal:
                {
                    docNum = DocumentGjiDomain.GetAll()
                        .Where(x => x.Id != document.Id)
                        .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal)
                        .Where(x => x.DocumentDate.HasValue && document.DocumentDate.HasValue &&
                            x.DocumentDate.Value.Year == document.DocumentDate.Value.Year)
                        .Where(x => x.DocumentNum.HasValue)
                        .Select(x => x.DocumentNum.Value)
                        .SafeMax(x => x);

                    docNum++;
                    
                    document.DocumentNumber = docNum.ToStr();
                    document.DocumentNum = docNum;
                }
                break;

                case TypeDocumentGji.Prescription:
                {

                    // берем первого инспектора
                    var firstInspector = DocumentGjiInspectorDomain.GetAll()
                                            .Where(x => x.DocumentGji.Id == document.Id)
                                            .OrderBy(x => x.Id)
                                            .FirstOrDefault();

                    if (firstInspector == null)
                    {
                        throw new Exception("Для предписания не задан инспектор");
                    }

                    if (string.IsNullOrEmpty(firstInspector.Inspector.Code))
                    {
                        throw new Exception("Для инспектора, указанного в предписании, не заполнен Код в справочнике Инспекторов");
                    }

                    // Номера должны быт ьтакие '2 О/C' '3 П/И' тоесть  наличие пробела является разделителем между номертом и кодом инспектора
                    var code = " "+firstInspector.Inspector.Code.Trim();

                    var lastNumByInspector = DocumentGjiDomain.GetAll()
                        .Where(x => x.Id != document.Id)
                        .Where(x => x.TypeDocumentGji == document.TypeDocumentGji)
                        .Where(x => x.DocumentDate.HasValue && document.DocumentDate.HasValue)
                        .Where(x => document.DocumentDate.Value.Year == document.DocumentDate.Value.Year)
                        .Where(x => x.DocumentNumber != null && x.DocumentNumber.EndsWith(code))
                        .Where(x => x.DocumentNum.HasValue )
                        .Select(x => x.DocumentNum.Value)
                        .SafeMax(x => x);

                    lastNumByInspector++;
                    docNum = lastNumByInspector;
                    document.DocumentNum = docNum;
                    document.DocumentNumber = docNum.ToStr() + code;
                }
                break;

                case TypeDocumentGji.ActRemoval:
                {
                    var parentAct = GetParentDocument(document, TypeDocumentGji.ActCheck);

                    if (parentAct == null) 
                    {
                        throw new Exception("Родительский акт ненайден");
                    }

                    if (!parentAct.DocumentNum.HasValue || string.IsNullOrEmpty(parentAct.DocumentNumber))
                    {
                        throw new Exception("Родительскому акту неприсвоен номер");
                    }

                    docNumber = parentAct.DocumentNumber;
                    docNum = parentAct.DocumentNum.Value;

                    var query = DocumentGjiChildrenDomain.GetAll()
                                    .Where(x => x.Parent.Id == parentAct.Id)
                                    .Where(x => x.Children.Id != document.Id)
                                    .Where(x => x.Children.DocumentDate.HasValue)
                                    .Where(x => x.Children.DocumentSubNum.HasValue && x.Children.DocumentNum.HasValue);

                    if (query.Any())
                    {
                        docSubNum = query
                                    .Select(x => x.Children.DocumentSubNum.Value)
                                    .SafeMax(x => x);

                        docSubNum++;

                        docNumber = docNumber + "/" + docSubNum.ToStr();

                    }

                    document.DocumentNumber = docNumber;
                    document.DocumentNum = docNum;
                    document.DocumentSubNum = docSubNum;

                }
                break;

                default:
                {

                    var parentDisposal = GetParentDocument(document, TypeDocumentGji.Disposal);

                    if (parentDisposal == null)
                    {
                        throw new Exception("Родительский приказ ненайден");
                    }

                    if (!parentDisposal.DocumentNum.HasValue || string.IsNullOrEmpty(parentDisposal.DocumentNumber))
                    {
                        throw new Exception("Родительскому приказу неприсвоен номер");
                    }

                    docNumber = parentDisposal.DocumentNumber;
                    docNum = parentDisposal.DocumentNum.Value;

                    var query = DocumentGjiDomain.GetAll()
                        .Where(x => x.Id != document.Id)
                        .Where(x => x.DocumentDate.HasValue && document.DocumentDate.HasValue)
                        .Where(x => parentDisposal.Stage.Id == x.Stage.Parent.Id)
                        .Where(x => x.TypeDocumentGji == document.TypeDocumentGji)
                        .Where(x => x.DocumentNum.HasValue && x.DocumentSubNum.HasValue);

                    if (query.Any())
                    {
                        docSubNum = query
                                    .Select(x => x.DocumentSubNum.Value)
                                    .SafeMax(x => x);

                        docSubNum++;

                        docNumber = docNumber + "/" + docSubNum.ToStr();
                    }

                    document.DocumentNumber = docNumber;
                    document.DocumentNum = docNum;
                    document.DocumentSubNum = docSubNum;
                    
                }
                break;
            }

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