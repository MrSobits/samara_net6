namespace Bars.Gkh.Overhaul.Hmao.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Entities;

    public class CopyAreaToStructElementAction : BaseExecutionAction
    {
        public override string Name => "Копирование общей площади МКД в конструктивные характеристики";

        public override string Description => @"Всем конструктивным элементам заведенным в домах скопирует в поле 'Объем' 
значение из поля 'Паспорт дома - Общие сведения - Общая площадь МКД'.";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var realtyObjectAreaMkdDict = this.Container.Resolve<IDomainService<RealityObject>>().GetAll()
                        .Select(x => new {x.Id, x.AreaMkd})
                        .ToDictionary(x => x.Id, x => x.AreaMkd);

                    var serviceRealityObjectStructuralElement = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>();

                    var realityObjectStructuralElements = serviceRealityObjectStructuralElement.GetAll().Where(x => x.RealityObject != null).ToList();

                    foreach (var realityObjectStructuralElement in realityObjectStructuralElements)
                    {
                        var realtyObjectId = realityObjectStructuralElement.RealityObject.Id;

                        if (realtyObjectAreaMkdDict.ContainsKey(realtyObjectId))
                        {
                            var volume = realtyObjectAreaMkdDict[realtyObjectId];

                            if (volume.HasValue)
                            {
                                realityObjectStructuralElement.Volume = volume.Value;
                                serviceRealityObjectStructuralElement.Save(realityObjectStructuralElement);
                            }
                        }
                    }

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