using System.Linq;

using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Tyumen.StateChange
{
    public class ResolutionValidationRule : BaseValidationRule
    {
        public override string Id
        {
            get { return "gji_tyumen_resolution_validation_number"; }
        }

        public override string Name
        {
            get { return "Проверка возможности формирования номера постановления Тюмень"; }
        }

        public override string TypeId
        {
            get { return "gji_document_resol"; }
        }

        public override string Description
        {
            get { return "Данное правило проверяет формирование номера постановления в соответствии с правилами Тюмени"; }
        }

        protected override void Action(DocumentGji document)
        {
            document.DocumentYear = document.DocumentDate.Value.Year;

            document.DocumentNum =
                Container.Resolve<IDomainService<DocumentGji>>().GetAll()
                    .Where(x => x.TypeDocumentGji == TypeDocumentGji.Resolution && x.DocumentYear == document.DocumentYear)
                    .Max(x => x.DocumentNum).ToInt() + 1;

            document.DocumentNumber = document.DocumentNum.ToStr();
        }
    }
}