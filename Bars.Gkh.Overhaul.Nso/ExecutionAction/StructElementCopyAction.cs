namespace Bars.Gkh.Overhaul.Nso.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Entities;

    public class StructElementCopyAction : BaseExecutionAction
    {
        public override string Name => "Перенос данных из конструктивных элементов в конструктивные характеристики";

        public override string Description => @"1: Справочники/Жилищно-коммунальное хозяйство/Конструктивные элементы
2: Справочники/Капитальный ремонт/ООИ

Перенос информация по КЭ из старого справочника (1) в справочник ООИ (2).

КЭ идентифицируется по полю 'Код' старого справочника 'Конструктивные элементы' и поле 'Код' конструктивных элементов из нового справочника 'ООИ'.

Переносим следующие поля:
1.Год последнего кап ремонта
2.Объем (не отображается в форме паспорта, но в базе есть, берем оттуда)
3.Износ - всем КЭ присваиваем 0";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var realtyObjectConstructElemDict = this.Container.Resolve<IDomainService<RealityObjectConstructiveElement>>().GetAll()
                        .Select(x => new {x.RealityObject.Id, x.ConstructiveElement.Code, x.Volume, x.LastYearOverhaul})
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(
                            x => x.Key,
                            x => x.GroupBy(y => y.Code)
                                .ToDictionary(y => y.Key, y => y.Select(z => new {z.LastYearOverhaul, z.Volume}).First()));

                    var serviceRealityObject = this.Container.Resolve<IDomainService<RealityObject>>();
                    var serviceStructuralElement = this.Container.Resolve<IDomainService<StructuralElement>>();
                    var structuralElementDict = serviceStructuralElement.GetAll()
                        .Select(x => new {x.Id, x.Code})
                        .AsEnumerable()
                        .GroupBy(x => x.Code)
                        .ToDictionary(x => x.Key, x => x.First().Id);

                    var serviceRealityObjectStructuralElement = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>();

                    var roStructuralElementsDict = serviceRealityObjectStructuralElement.GetAll().ToDictionary(x => x.Id);

                    var realityObjectStructuralElementsDict = serviceRealityObjectStructuralElement.GetAll()
                        .Where(x => x.RealityObject != null)
                        .Select(x => new {roId = x.RealityObject.Id, x.StructuralElement.Code, x.Id})
                        .AsEnumerable()
                        .GroupBy(x => x.roId)
                        .ToDictionary(x => x.Key, x => x.GroupBy(y => y.Code).ToDictionary(y => y.Key, y => y.First().Id));

                    foreach (var realtyObjectConstructElems in realtyObjectConstructElemDict)
                    {
                        var realtyObjectId = realtyObjectConstructElems.Key;

                        if (realityObjectStructuralElementsDict.ContainsKey(realtyObjectId))
                        {
                            var roStructElemDict = realityObjectStructuralElementsDict[realtyObjectId];

                            foreach (var roConstructElement in realtyObjectConstructElems.Value)
                            {
                                var constructElementCode = roConstructElement.Key;

                                if (roStructElemDict.ContainsKey(constructElementCode))
                                {
                                    var realityObjectStructElement = roStructuralElementsDict[roStructElemDict[constructElementCode]];

                                    var needToUpdate = false;

                                    if (realityObjectStructElement.LastOverhaulYear != roConstructElement.Value.LastYearOverhaul)
                                    {
                                        realityObjectStructElement.LastOverhaulYear = roConstructElement.Value.LastYearOverhaul;
                                        needToUpdate = true;
                                    }

                                    if (roConstructElement.Value.Volume.HasValue
                                        && realityObjectStructElement.Volume != roConstructElement.Value.Volume.Value)
                                    {
                                        realityObjectStructElement.Volume = roConstructElement.Value.Volume.Value;
                                        needToUpdate = true;
                                    }

                                    if (needToUpdate)
                                    {
                                        serviceRealityObjectStructuralElement.Save(realityObjectStructElement);
                                    }
                                }
                                else
                                {
                                    if (structuralElementDict.ContainsKey(constructElementCode))
                                    {
                                        var realityObjectConstructElement = new RealityObjectStructuralElement
                                        {
                                            RealityObject = serviceRealityObject.Load(realtyObjectId),
                                            StructuralElement = serviceStructuralElement.Load(structuralElementDict[constructElementCode]),
                                            LastOverhaulYear = roConstructElement.Value.LastYearOverhaul
                                        };

                                        if (roConstructElement.Value.Volume.HasValue)
                                        {
                                            realityObjectConstructElement.Volume = roConstructElement.Value.Volume.Value;
                                        }

                                        serviceRealityObjectStructuralElement.Save(realityObjectConstructElement);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var roConstructElement in realtyObjectConstructElems.Value)
                            {
                                var constructElementCode = roConstructElement.Key;

                                if (structuralElementDict.ContainsKey(constructElementCode))
                                {
                                    var realityObjectConstructElement = new RealityObjectStructuralElement
                                    {
                                        RealityObject = serviceRealityObject.Load(realtyObjectId),
                                        StructuralElement = serviceStructuralElement.Load(structuralElementDict[constructElementCode]),
                                        LastOverhaulYear = roConstructElement.Value.LastYearOverhaul
                                    };

                                    if (roConstructElement.Value.Volume.HasValue)
                                    {
                                        realityObjectConstructElement.Volume = roConstructElement.Value.Volume.Value;
                                    }

                                    serviceRealityObjectStructuralElement.Save(realityObjectConstructElement);
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