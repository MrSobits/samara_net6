using System;
using System.Globalization;
using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Regions.Tula.StateChange
{
    public class ActRemovalValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "gji_tula_actremoval_validation_number"; } }

        public override string Name { get { return "Тула - Проверка возможности формирования номера акта устранения нарушений"; } }

        public override string TypeId { get { return "gji_document_actrem"; } }

        public override string Description { get { return "Тула - Данное правило проверяет формирование номера акта устранения нарушений в соответствии с правилами"; } }

    }
}