namespace Bars.Gkh.Overhaul.Hmao.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class VersionRecordActualizeFieldUpdateAction : BaseExecutionAction
    {
        public override string Description => "Проставление признаков актуализации для существующих записей версий ДПКР";

        public override string Name => "Проставление признаков актуализации для существующих записей версий ДПКР";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var stage3Domain = this.Container.ResolveDomain<VersionRecord>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var listForUpdate = new List<VersionRecord>();

            try
            {
                var records = stage3Domain.GetAll()
                    .Where(x => x.Changes != null && x.Changes != "")
                    .Select(
                        x => new
                        {
                            x.Changes,
                            x.Id
                        })
                    .ToList();

                foreach (var record in records)
                {
                    var rec = stage3Domain.Load(record.Id);
                    rec.IsAddedOnActualize = record.Changes.Contains("Добавлено");
                    rec.IsChangeSumOnActualize = record.Changes.Contains("Изменено");

                    listForUpdate.Add(rec);
                }

                TransactionHelper.InsertInManyTransactions(this.Container, listForUpdate, 10000, true, true);
            }
            finally
            {
                this.Container.Release(sessionProvider);
                this.Container.Release(stage3Domain);
            }

            return new BaseDataResult();
        }
    }
}