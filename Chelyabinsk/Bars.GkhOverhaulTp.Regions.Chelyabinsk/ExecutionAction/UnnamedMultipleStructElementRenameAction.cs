namespace Bars.GkhOverhaulTp.Regions.Chelyabinsk.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Entities;

    public class UnnamedMultipleStructElementRenameAction : BaseExecutionAction
    {
        public IDomainService<RealityObjectStructuralElement> RealityObjectStructuralElementService { get; set; }

        public IDomainService<StructuralElement> StructuralElementService { get; set; }

        public override string Name => "Переименование множественных КЭ, добавленных только один раз";

        public override string Description => @"Если в доме в разделе Конструктивные характеристики множественный КЭ добавлен только один раз 
и Наименование КЭ в доме пустое, то присвоить атрибуту Наименование этого КЭ название по тому же правилу, как и при добавлении КЭ в дом. 
Например, если в доме есть только один КЭ 'Пассажирская лифтовая кабина', то присвоить атрибуту наименование 'Пассажирская лифтовая кабина 1'.";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var seNames = this.StructuralElementService.GetAll()
                .Select(x => new {x.Id, x.Name})
                .ToDictionary(x => x.Id, x => x.Name);

            var roStructuralElementsDict = this.RealityObjectStructuralElementService.GetAll()
                .Where(x => x.StructuralElement.Group.CommonEstateObject.MultipleObject)
                .Where(x => x.Name == null || x.Name == "")
                .ToDictionary(x => x.Id);

            var roStructuralElementsToRename = this.RealityObjectStructuralElementService.GetAll()
                .Where(x => x.StructuralElement.Group.CommonEstateObject.MultipleObject)
                .Where(x => x.Name == null || x.Name == "")
                .Select(
                    x => new
                    {
                        x.Id,
                        roId = x.RealityObject.Id,
                        seId = x.StructuralElement.Id,
                        ceoId = x.StructuralElement.Group.CommonEstateObject.Id
                    })
                .AsEnumerable()
                .GroupBy(x => new {x.roId, x.ceoId})
                .Select(
                    x => new
                    {
                        x.Key,
                        isSingle = x.Count() == 1,
                        data = x.Select(y => new {y.Id, y.seId}).First()
                    })
                .Where(x => x.isSingle)
                .Select(
                    x => new
                    {
                        x.Key.roId,
                        x.data.seId,
                        x.data.Id
                    })
                .ToArray();

            const int OperationsPerTransaction = 100;
            var i = 0;

            while (i < roStructuralElementsToRename.Length)
            {
                var limit = (i + OperationsPerTransaction) < roStructuralElementsToRename.Length
                    ? (i + OperationsPerTransaction)
                    : roStructuralElementsToRename.Length;

                var res = this.InTransaction(
                    () =>
                    {
                        for (; i < limit; ++i)
                        {
                            var seId = roStructuralElementsToRename[i].seId;
                            var roStructuralElement = roStructuralElementsDict[roStructuralElementsToRename[i].Id];

                            roStructuralElement.Name = seNames[seId] + " 1";
                        }
                    });

                if (res.Success != true)
                {
                    return res;
                }
            }

            return new BaseDataResult {Success = true};
        }

        private BaseDataResult InTransaction(Action action)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    action();

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();

                        return new BaseDataResult
                        {
                            Success = false,
                            Message = exc.Message
                        };
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, exc);
                    }
                }
            }

            return new BaseDataResult {Success = true};
        }
    }
}