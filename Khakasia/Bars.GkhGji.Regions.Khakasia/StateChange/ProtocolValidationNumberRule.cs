using System;
using System.Linq;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Khakasia.StateChange
{
    public class ProtocolValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "GJI_KHAKASIA_protocol_validation_number"; } }

        public override string Name { get { return "Хакасия - Проверка возможности формирования номера протокола (Cаха)"; } }

        public override string TypeId { get { return "gji_document_prot"; } }

        public override string Description { get { return "Хакасия - Данное правило проверяет формирование номера протокола в соответствии с правилами (Cаха)"; } }
    }
}