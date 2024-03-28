using System;
using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Regions.Tula.StateChange
{
    public class ResolutionValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "gji_resolution_validation_number"; } }

        public override string Name { get { return "Тула - Проверка возможности формирования номера постановления"; } }

        public override string TypeId { get { return "gji_document_resol"; } }

        public override string Description { get { return "Тула - Данное правило проверяет формирование номера постановления в соответствии с правилами"; } }
    }
}