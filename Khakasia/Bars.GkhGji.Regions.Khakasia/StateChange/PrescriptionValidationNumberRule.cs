using System;
using System.Linq;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Khakasia.StateChange
{
    public class PrescriptionValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "GJI_KHAKASIA_prescription_validation_number"; } }

        public override string Name { get { return "Хакасия - Проверка возможности формирования номера предписания"; } }

        public override string TypeId { get { return "gji_document_prescr"; } }

        public override string Description { get { return "Хакасия - Данное правило проверяет формирование номера предписания в соответствии с правилами"; } }
    }
}