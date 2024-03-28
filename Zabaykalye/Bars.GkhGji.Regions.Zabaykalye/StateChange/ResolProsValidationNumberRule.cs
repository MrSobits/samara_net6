using System;
using System.Globalization;
using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Regions.Zabaykalye.StateChange
{
    public class ResolProsValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "gji_zabaykalye_resolpros_validation_number"; } }

        public override string Name { get { return "Забайкалье - Проверка возможности формирования номера постановления прокуратуры"; } }

        public override string TypeId { get { return "gji_document_resolpros"; } }

        public override string Description { get { return "Забайкалье - Данное правило проверяет формирование номера постановления прокуратуры в соответствии с правилами"; } }

    }
}