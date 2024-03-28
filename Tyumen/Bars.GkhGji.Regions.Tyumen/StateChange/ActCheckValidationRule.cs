using System.Linq;

using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Tyumen.StateChange
{
    public class ActCheckValidationRule : BaseValidationRule
    {
        public override string Id
        {
            get { return "gji_tyumen_actcheck_validation_number"; }
        }

        public override string Name
        {
            get { return "Проверка возможности формирования номера акта проверки Тюмень"; }
        }

        public override string TypeId
        {
            get { return "gji_document_actcheck"; }
        }

        public override string Description
        {
            get { return "Данное правило проверяет формирование номера акта проверки в соответствии с правилами Тюмени"; }
        }

        protected override void Action(DocumentGji document)
        {
            document.DocumentYear = document.DocumentDate.Value.Year;

            document.DocumentNum =
                Container.Resolve<IDomainService<DocumentGji>>().GetAll()
                    .Where(x => x.TypeDocumentGji == TypeDocumentGji.ActCheck && x.Id != document.Id && x.DocumentYear == document.DocumentYear)
                    .Max(x => x.DocumentNum).ToInt() + 1;

            var inspectorCode = 
                Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                    .Where(x => x.DocumentGji.Id == document.Id)
                    .Select(x => x.Inspector.Code)
                    .FirstOrDefault(x => x != null);

            document.DocumentNumber = string.Format("ТО-{0}-{1}", inspectorCode, document.DocumentNum);
        }
    }
}