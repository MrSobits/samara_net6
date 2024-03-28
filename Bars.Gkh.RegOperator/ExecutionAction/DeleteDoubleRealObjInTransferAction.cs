namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;

    public class DeleteDoubleRealObjInTransferAction : BaseExecutionAction
    {
        public override string Description => "Удаление дублирующих записей домов из перечисления средста в фонд. После этого надо опять нажать расчет.";

        public override string Name => "Удаление дублирующих записей домов из перечисления средста в фонд";

        public override Func<IDataResult> Action => this.DeleteDoubleRealObjInTransfer;

        private BaseDataResult DeleteDoubleRealObjInTransfer()
        {
            var transferObjectDomain = this.Container.ResolveDomain<TransferObject>();

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    transferObjectDomain.GetAll()
                        .Select(
                            x => new
                            {
                                TransRecId = x.TransferRecord.Id,
                                RoId = x.RealityObject.Id,
                                x.Id
                            })
                        .AsEnumerable()
                        .GroupBy(x => new {x.RoId, x.TransRecId})
                        .Select(
                            x => new
                            {
                                Count = x.Count(),
                                Ids = x.Select(y => y.Id).ToList()
                            })
                        .Where(x => x.Count > 1)
                        .SelectMany(x => x.Ids)
                        .ForEach(x => transferObjectDomain.Delete(x));

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }

            return new BaseDataResult();
        }
    }
}