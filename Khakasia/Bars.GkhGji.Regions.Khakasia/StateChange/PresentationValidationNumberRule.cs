using System;
using System.Globalization;
using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Khakasia.StateChange
{
    class PresentationValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "GJI_KHAKASIA_presentation_validation_number"; } }

        public override string Name { get { return "Хакасия - Проверка возможности формирования номера представления"; } }

        public override string TypeId { get { return "gji_document_presen"; } }

        public override string Description { get { return "Хакасия - Данное правило проверяет формирование номера представления в соответствии с правилами"; } }
    }
}