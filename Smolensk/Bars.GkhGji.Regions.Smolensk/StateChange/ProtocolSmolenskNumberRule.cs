using System;
using System.Linq;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.GkhGji.Entities;
using Bars.GkhGji.StateChange;

namespace Bars.GkhGji.Regions.Smolensk.StateChange
{
    public class ProtocolSmolenskNumberTatRule : BaseDocSmolenskNumberRule
    {
        public override string Id { get { return "gji_smolensk_protocol_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера протокола Смоленска"; } }

        public override string TypeId { get { return "gji_document_prot"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера протокола в соответствии с правилами Смоленска"; } }

        protected override void Action(DocumentGji document)
        {
            var protocolDomain = Container.ResolveDomain<Protocol>();

            try
            {
                var maxNumber = protocolDomain.GetAll()
                    .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == DateTime.Now.Year)
                    .SafeMax(x => x.DocumentNum).ToInt();

                document.DocumentNum = maxNumber + 1;
                document.DocumentNumber = document.DocumentNum.ToStr().PadLeft(5, '0');

            }
            finally
            {
                Container.Release(protocolDomain);
            }
        }
    }
}