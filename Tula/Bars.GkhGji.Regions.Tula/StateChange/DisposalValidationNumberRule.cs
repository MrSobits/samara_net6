using System;
using System.Globalization;
using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.Gkh.Entities;
using Bars.GkhGji.Contracts;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Regions.Tula.StateChange
{
    public class DisposalValidationNumberRule : BaseDocValidationNumberRule
    {
        public IDisposalText DisposalText { get; set; }

        public override string Id { get { return "gji_tula_disposal_validation_number"; } }

        public override string TypeId { get { return "gji_document_disp"; } }

        public override string Name 
        {
            get { return string.Format("Тула - Проверка возможности формирования номера {0}", DisposalText.GenetiveCase.ToLower()); } 
        }

        public override string Description 
        {
            get { return string.Format("Тула - Данное правило проверяет формирование номера {0} в соответствии с правилами", DisposalText.GenetiveCase.ToLower()); }
        }

    }
}