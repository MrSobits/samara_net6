namespace Bars.GkhRf
{
    using System.Collections.Generic;

    using Bars.B4.Modules.States;
    using Bars.GkhRf.Entities;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("rf_transfer_record", "Перечисление средств в фонд", typeof(TransferRfRecord)),
                new StatefulEntityInfo("rf_request_transfer", "Заявка на перечисление средств", typeof(RequestTransferRf))
            };
        }
    }
}