using System.Linq;

using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Tyumen.StateChange
{
    public class ProtocolValidationRule : BaseValidationRule
    {
        public override string Id
        {
            get { return "gji_tyumen_protocol_validation_number"; }
        }

        public override string Name
        {
            get { return "Проверка возможности формирования номера протокола Тюмень"; }
        }

        public override string TypeId
        {
            get { return "gji_document_prot"; }
        }

        public override string Description
        {
            get { return "Данное правило проверяет формирование номера протокола в соответствии с правилами Тюмени"; }
        }

        protected override void Action(DocumentGji document)
        {
            document.DocumentYear = document.DocumentDate.Value.Year;

            document.DocumentNum =
                Container.Resolve<IDomainService<DocumentGji>>().GetAll()
                    .Where(x => x.TypeDocumentGji == TypeDocumentGji.Protocol && x.Id != document.Id && x.DocumentYear == document.DocumentYear)
                    .Max(x => x.DocumentNum).ToInt() + 1;

            document.DocumentNumber = string.Format("02-04-{0}/{1}", document.DocumentNum, document.DocumentYear);
        }
    }
}
