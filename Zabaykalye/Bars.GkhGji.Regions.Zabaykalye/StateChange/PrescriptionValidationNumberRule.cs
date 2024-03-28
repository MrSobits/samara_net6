using System;
using System.Linq;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Zabaykalye.StateChange
{
    public class PrescriptionValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "gji_zabaykalye_prescription_validation_number"; } }

        public override string Name { get { return "Забайкалье - Проверка возможности формирования номера предписания"; } }

        public override string TypeId { get { return "gji_document_prescr"; } }

        public override string Description { get { return "Забайкалье - Данное правило проверяет формирование номера предписания в соответствии с правилами"; } }
    }
}