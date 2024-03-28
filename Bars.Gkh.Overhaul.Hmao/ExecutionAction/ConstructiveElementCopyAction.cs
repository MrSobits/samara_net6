namespace Bars.Gkh.Overhaul.Hmao.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Entities;

    public class ConstructiveElementCopyAction : BaseExecutionAction
    {
        public override string Name => "Перенос данных из конструктивных характеристик дома в конструктивные элементы дома";

        public override string Description => @"1: жилой дом - конструктивные элементы
2: жилой дом - конструктивные характеристики

Перенос информация по КХ из справочника ООИ (2) в старый справочник (1).

КЭ идентифицируется по полю 'Код' старого справочника 'Конструктивные элементы' и поле 'Код' конструктивных элементов из нового справочника 'ООИ'.

Переносим следующие поля:
1.Год последнего кап ремонта
2.Объем";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var realtyObjectStructElemDict = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>().GetAll()
                        .Select(x => new {x.RealityObject.Id, x.StructuralElement.Code, x.Volume, x.LastOverhaulYear})
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(
                            x => x.Key,
                            x => x.GroupBy(y => y.Code)
                                .ToDictionary(y => y.Key, y => y.Select(z => new {z.LastOverhaulYear, z.Volume}).First()));

                    var serviceRealityObject = this.Container.Resolve<IDomainService<RealityObject>>();
                    var serviceConstructiveElement = this.Container.Resolve<IDomainService<ConstructiveElement>>();
                    var constructiveElementDict = serviceConstructiveElement.GetAll()
                        .Select(x => new {x.Id, x.Code})
                        .AsEnumerable()
                        .GroupBy(x => x.Code)
                        .ToDictionary(x => x.Key, x => x.First().Id);

                    var serviceRealityObjectConstructiveElement = this.Container.Resolve<IDomainService<RealityObjectConstructiveElement>>();

                    var roConstructElementsDict = serviceRealityObjectConstructiveElement.GetAll().ToDictionary(x => x.Id);

                    var realityObjectConstructElementsDict = serviceRealityObjectConstructiveElement.GetAll()
                        .Where(x => x.RealityObject != null)
                        .Select(x => new {roId = x.RealityObject.Id, x.ConstructiveElement.Code, x.Id})
                        .AsEnumerable()
                        .GroupBy(x => x.roId)
                        .ToDictionary(x => x.Key, x => x.GroupBy(y => y.Code).ToDictionary(y => y.Key, y => y.First().Id));

                    foreach (var realtyObjectStructElems in realtyObjectStructElemDict)
                    {
                        var realtyObjectId = realtyObjectStructElems.Key;

                        if (realityObjectConstructElementsDict.ContainsKey(realtyObjectId))
                        {
                            var constructElemDict = realityObjectConstructElementsDict[realtyObjectId];

                            foreach (var roStructElement in realtyObjectStructElems.Value)
                            {
                                var structElementCode = roStructElement.Key;

                                if (constructElemDict.ContainsKey(structElementCode))
                                {
                                    var realityObjectConstructElement = roConstructElementsDict[constructElemDict[structElementCode]];

                                    bool needToUpdate = false;

                                    if (realityObjectConstructElement.LastYearOverhaul != roStructElement.Value.LastOverhaulYear)
                                    {
                                        realityObjectConstructElement.LastYearOverhaul = roStructElement.Value.LastOverhaulYear;
                                        needToUpdate = true;
                                    }

                                    if (!realityObjectConstructElement.Volume.HasValue
                                        || realityObjectConstructElement.Volume.Value != roStructElement.Value.Volume)
                                    {
                                        realityObjectConstructElement.Volume = roStructElement.Value.Volume;
                                        needToUpdate = true;
                                    }

                                    if (needToUpdate)
                                    {
                                        serviceRealityObjectConstructiveElement.Save(realityObjectConstructElement);
                                    }
                                }
                                else
                                {
                                    if (constructiveElementDict.ContainsKey(structElementCode))
                                    {
                                        var realityObjectConstructElement = new RealityObjectConstructiveElement
                                        {
                                            RealityObject = serviceRealityObject.Load(realtyObjectId),
                                            ConstructiveElement = serviceConstructiveElement.Load(constructiveElementDict[structElementCode]),
                                            LastYearOverhaul = roStructElement.Value.LastOverhaulYear,
                                            Volume = roStructElement.Value.Volume
                                        };

                                        serviceRealityObjectConstructiveElement.Save(realityObjectConstructElement);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var roStructElement in realtyObjectStructElems.Value)
                            {
                                var structElementCode = roStructElement.Key;

                                if (constructiveElementDict.ContainsKey(structElementCode))
                                {
                                    var realityObjectConstructElement = new RealityObjectConstructiveElement
                                    {
                                        RealityObject = serviceRealityObject.Load(realtyObjectId),
                                        ConstructiveElement = serviceConstructiveElement.Load(constructiveElementDict[structElementCode]),
                                        LastYearOverhaul = roStructElement.Value.LastOverhaulYear,
                                        Volume = roStructElement.Value.Volume
                                    };

                                    serviceRealityObjectConstructiveElement.Save(realityObjectConstructElement);
                                }
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