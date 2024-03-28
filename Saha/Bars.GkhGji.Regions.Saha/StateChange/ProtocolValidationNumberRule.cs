using System;
using System.Linq;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Saha.StateChange
{
    public class ProtocolValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "gji_protocol_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера протокола (Cаха)"; } }

        public override string TypeId { get { return "gji_document_prot"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера протокола в соответствии с правилами (Cаха)"; } }

        protected override void Action(DocumentGji document)
        {
            var protocolDomain = Container.ResolveDomain<Protocol>();

            try
            {
                var maxNum = protocolDomain.GetAll()
                    .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == DateTime.Now.Year)
                    .SafeMax(x => x.DocumentNum) ?? 0;

                document.DocumentNum = maxNum + 1;

                document.DocumentNumber = "08-01-{0}-{1}".FormatUsing(document.DocumentNum.ToStr().PadLeft(2, '0'), DateTime.Now.Year % 100);

            }
            finally
            {
                Container.Release(protocolDomain);
            }
        }
    }
}