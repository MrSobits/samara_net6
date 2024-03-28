namespace Bars.GkhGji.StateChange
{
using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;

    public class ResolutionValidationNumberTatRule : BaseDocValidationNumberTatRule
    {
        public override string Id { get { return "gji_tatar_resolution_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера постановления РТ"; } }

        public override string TypeId { get { return "gji_document_resol"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера постановления в соответствии с правилами РТ"; } }

        protected override void Action(DocumentGji document)
        {
            var documentService = Container.Resolve<IDomainService<DocumentGji>>();
            var documentGjiChildrenService = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            // Год берется из даты документа
            document.DocumentYear =
                document.DocumentDate.HasValue
                    ? document.DocumentDate.Value.Year
                    : (int?)null;

            var parentDocument =
                documentGjiChildrenService.GetAll()
                    .Where(x => x.Children.Id == document.Id)
                    .Select(x => x.Parent)
                    .FirstOrDefault();

            if (parentDocument != null && !string.IsNullOrEmpty(parentDocument.DocumentNumber))
            {
                if (parentDocument.TypeDocumentGji == TypeDocumentGji.ProtocolMvd)
                {
                    // т.к. для постановлений сформированных из протокола МВД должна быть сквозная нумерация через все распоряжения и постановления прокуратуры с начала года
                    var resolutionsByProtocolMvdQuery = documentGjiChildrenService.GetAll()
                        .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                        .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ProtocolMvd)
                        .Select(x => x.Children.Id);

                    var maxNum = documentService.GetAll()
                        .Where(x => x.DocumentYear == document.DocumentYear)
                        .Where(x => x.Id != document.Id)
                        .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal
                                    || x.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor
                                    || (x.TypeDocumentGji == TypeDocumentGji.Resolution && resolutionsByProtocolMvdQuery.Contains(x.Id)))
                        .Select(x => x.DocumentNum)
                        .Max();

                    var num = maxNum.HasValue ? maxNum.Value + 1 : 1;

                    document.DocumentNum = num;

                    var muCode = this.Container.Resolve<IDomainService<ProtocolMvdRealityObject>>().GetAll()
                        .Where(x => x.ProtocolMvd.Id == parentDocument.Id)
                        .Select(x => x.RealityObject.Municipality.Code)
                        .FirstOrDefault();

                    document.DocumentNumber = string.IsNullOrEmpty(muCode) ? num.ToStr() : muCode + "-" + num;
                }
                else
                {
                    document.DocumentNumber = parentDocument.DocumentNumber;
                    document.DocumentNum = parentDocument.DocumentNum;

                    var siblingsDocumentSubNum = documentGjiChildrenService.GetAll()
                        .Where(x => x.Parent.Id == parentDocument.Id)
                        .Where(x => x.Children.Id != document.Id)
                        .Where(x => x.Children.Stage.Id == document.Stage.Id)
                        .Where(x => x.Children.TypeDocumentGji == document.TypeDocumentGji)
                        .Select(x => x.Children.DocumentSubNum)
                        .ToList();

                    if (siblingsDocumentSubNum.Any())
                    {
                        document.DocumentSubNum = siblingsDocumentSubNum.Max().ToInt() + 1;
                    }

                    if (document.Inspection != null
                        && document.Inspection.TypeBase == TypeBase.ProsecutorsResolution
                        && document.DocumentSubNum.ToInt() > 0)
                    {
                        document.DocumentNumber += "/" + document.DocumentSubNum;
                    }
                }
            }
        }
    }
}