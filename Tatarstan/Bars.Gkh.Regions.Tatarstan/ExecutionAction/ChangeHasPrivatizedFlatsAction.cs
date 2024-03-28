namespace Bars.Gkh.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    public class ChangeHasPrivatizedFlats : BaseExecutionAction
    {
        public override string Description => "Меняет значение \"Нет\" наличия приватизированных квартир всех домов на \"Не задано\"";

        public override string Name => "Изменение значения наличия приватизированных квартир";

        public override Func<IDataResult> Action => this.ChangeValues;

        public BaseDataResult ChangeValues()
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                var roService = this.Container.Resolve<IRepository<RealityObject>>();

                try
                {
                    var data = roService.GetAll()
                        .Where(x => x.HasPrivatizedFlats.HasValue && !x.HasPrivatizedFlats.Value);

                    foreach (var realityObject in data)
                    {
                        realityObject.HasPrivatizedFlats = null;
                        roService.Update(realityObject);
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    this.Container.Release(roService);
                }
            }

            return new BaseDataResult();
        }
    }
}