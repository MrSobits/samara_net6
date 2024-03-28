using System;
using System.Globalization;
using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Tula.StateChange
{
    class PresentationValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "gji_tula_presentation_validation_number"; } }

        public override string Name { get { return "Тула - Проверка возможности формирования номера представления"; } }

        public override string TypeId { get { return "gji_document_presen"; } }

        public override string Description { get { return "Тула - Данное правило проверяет формирование номера представления в соответствии с правилами"; } }
    }
}