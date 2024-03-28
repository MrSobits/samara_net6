using System.Linq;
using Bars.B4.DataAccess;
using Bars.B4.Modules.States;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.StateChange;

namespace Bars.GkhGji.Regions.Smolensk.StateChange
{
    public class ActCheckSmolenskNumberRule : BaseDocSmolenskNumberRule
    {
        public override string Id { get { return "gji_smolensk_actcheck_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера акта проверки Смоленска"; } }

        public override string TypeId { get { return "gji_document_actcheck"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера акта проверки в соответствии с правилами Смоленска"; } }

        protected override void InternalValidate(DocumentGji document, ValidateResult result)
        {
            var docDomain = Container.ResolveDomain<DocumentGji>();

            try
            {
                if (document.Stage.Parent != null)
                {
                    var mainDoc = docDomain.GetAll()
                        .Where(x => x.Stage.Id == document.Stage.Parent.Id)
                        .Select(x => new { x.TypeDocumentGji, x.DocumentDate, x.DocumentNumber })
                        .ToList();

                    foreach (var doc in mainDoc)
                    {
                        if (string.IsNullOrEmpty(doc.DocumentNumber))
                        {
                            result.Message = string.Format(
                                "Номер не может быть присвоен, потому что у предыдущего документа {0} от {1} нет номера",
                                doc.TypeDocumentGji.GetEnumMeta().Display,
                                doc.DocumentDate.ToDateTime().ToShortDateString());
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
            }
        }

        protected override void Action(DocumentGji document)
        {
            var docDomain = Container.ResolveDomain<DocumentGji>();

            try
            {
                var parentDoc = docDomain.GetAll().FirstOrDefault(x => x.Stage.Id == document.Stage.Parent.Id);

                if (parentDoc != null)
                {
                    document.DocumentNum = parentDoc.DocumentNum;
                    document.DocumentNumber = parentDoc.DocumentNumber;
                }
            }
            finally
            {
                Container.Release(docDomain);
            }
        }
    }
}