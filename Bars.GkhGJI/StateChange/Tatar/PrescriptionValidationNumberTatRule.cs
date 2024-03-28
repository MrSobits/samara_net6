namespace Bars.GkhGji.StateChange
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class PrescriptionValidationNumberTatRule : BaseDocValidationNumberTatRule
    {
        public override string Id { get { return "gji_tatar_prescription_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера предписания РТ"; } }

        public override string TypeId { get { return "gji_document_prescr"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера предписания в соответствии с правилами РТ"; } }

        protected override void Action(DocumentGji document)
        {
            var documentService = Container.Resolve<IDomainService<DocumentGji>>();
            var documentGjiChildrenService = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            // Год берется из даты документа
            document.DocumentYear =
                document.DocumentDate.HasValue
                    ? document.DocumentDate.Value.Year
                    : (int?) null;

            var parentDocument =
                documentGjiChildrenService.GetAll()
                    .Where(x => x.Children.Id == document.Id)
                    .Select(x => x.Parent)
                    .FirstOrDefault();

            //строковый номер, целая часть номера и подномер берутся из родительского документа
            if (parentDocument != null && !string.IsNullOrEmpty(parentDocument.DocumentNumber))
            {
                document.DocumentNumber = parentDocument.DocumentNumber;
                document.DocumentNum = parentDocument.DocumentNum;

                if (documentService.GetAll().Any(x => x.Stage.Id == document.Stage.Id && x.Id != document.Id))
                {
                    document.DocumentSubNum =
                        documentService.GetAll()
                            .Where(x => x.Stage.Id == document.Stage.Id && x.Id != document.Id)
                            .Select(x => x.DocumentSubNum)
                            .Max()
                            .ToInt() + 1;
                }
            }
        }
    }
}