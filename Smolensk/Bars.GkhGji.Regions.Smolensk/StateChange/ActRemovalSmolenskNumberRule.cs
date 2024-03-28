using System.Linq;
using Bars.B4.DataAccess;
using Bars.B4.Modules.States;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.StateChange;

namespace Bars.GkhGji.Regions.Smolensk.StateChange
{
    using Bars.GkhGji.Enums;

    public class ActRemovalSmolenskNumberRule : BaseDocSmolenskNumberRule
    {
        public override string Id { get { return "gji_smolensk_actremoval_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера акта проверки предписания (акта устранения нарушения) Смоленска"; } }

        public override string TypeId { get { return "gji_document_actrem"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера акта проверки предписания в соответствии с правилами Смоленска"; } }

        protected override void InternalValidate(DocumentGji document, ValidateResult result)
        {
            var docDomain = Container.ResolveDomain<DocumentGji>();
            var docChildrenDomain = Container.ResolveDomain<DocumentGjiChildren>();

            try
            {
                var docChildren =
                    docChildrenDomain.GetAll()
                                     .FirstOrDefault(
                                         x =>
                                         x.Children.Id == document.Id
                                         && x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck);


                // Через этап акта проверки получаем этап распоряждения чтобы из него поулчить номер для валидации
                if (docChildren != null && docChildren.Parent.Stage.Parent != null)
                {
                    var mainDoc = docDomain.GetAll()
                        .Where(x => x.Stage.Id == docChildren.Parent.Stage.Parent.Id)
                        .Select(x => new { x.TypeDocumentGji, x.DocumentDate, x.DocumentNumber })
                        .FirstOrDefault();

                    if (mainDoc != null)
                    {
                        if (string.IsNullOrEmpty(mainDoc.DocumentNumber))
                        {
                            result.Message = string.Format(
                                "Номер не может быть присвоен, потому что у предыдущего документа {0} от {1} нет номера",
                                mainDoc.TypeDocumentGji.GetEnumMeta().Display,
                                mainDoc.DocumentDate.ToDateTime().ToShortDateString());
                            result.Success = false;

                            return;
                        }
                    }
                }

                result.Success = true;

            }
            finally 
            {
                Container.Release(docDomain);
                Container.Release(docChildrenDomain);
            }
        }

        protected override void Action(DocumentGji document)
        {
            var docDomain = Container.ResolveDomain<DocumentGji>();
            var docChildrenDomain = Container.ResolveDomain<DocumentGjiChildren>();

            try
            {
                var docChildren =
                    docChildrenDomain.GetAll()
                                     .FirstOrDefault(
                                         x =>
                                         x.Children.Id == document.Id
                                         && x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck);

                if (docChildren != null)
                {
                    // Через этап акта проверки получаем этап распоряждения чтобы из него поулчить номер
                    var parentDoc = docDomain.GetAll().FirstOrDefault(x => x.Stage.Id == docChildren.Parent.Stage.Parent.Id);

                    if (parentDoc != null)
                    {
                        document.DocumentNum = parentDoc.DocumentNum;
                        document.DocumentNumber = parentDoc.DocumentNumber;
                    }
                }
                
            }
            finally
            {
                Container.Release(docDomain);
                Container.Release(docChildrenDomain);
            }
        }
    }
}