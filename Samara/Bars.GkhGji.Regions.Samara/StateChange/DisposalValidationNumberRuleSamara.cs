using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Bars.GkhGji.Regions.Samara.StateChange
{
    public class DisposalValidationNumberRuleSamara : BaseDocValidationNumberSamaraRule
    {
        public override string Id { get { return "gji_samara_disposal_validation_number"; } }

        public override string Name { get { return "Правило формирования номера распоряжения (Самара)"; } }

        public override string TypeId { get { return "gji_document_disp"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера распоряжения (Самара)"; } }
    }
}