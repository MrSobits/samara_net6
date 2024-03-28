namespace Bars.Gkh.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhDi.Entities;

    public class DeleteDuplicateDiRealObjPercentAction : BaseExecutionAction
    {
        public IDomainService<DiRealObjPercent> DiRealObjPercentService { get; set; }

        public override string Description => "Удалить дубли процентов по раскрытию для домов";

        public override string Name => "Удалить дубли процентов по раскрытию для домов";

        public override Func<IDataResult> Action => this.Execute;

        public BaseDataResult Execute()
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var data = this.DiRealObjPercentService.GetAll()
                        .AsEnumerable()
                        .GroupBy(x => new {x.DiRealityObject.Id, x.Code})
                        .Where(x => x.Count() > 1)
                        .SelectMany(
                            x =>
                            {
                                var listToDel =
                                    x.OrderByDescending(y => y.ObjectEditDate).ToList();
                                listToDel.RemoveAt(0);

                                return listToDel;
                            })
                        .ToList();

                    foreach (var diRealObjPercent in data.Take(500000))
                    {
                        this.DiRealObjPercentService.Delete(diRealObjPercent.Id);
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return new BaseDataResult();
        }
    }
}