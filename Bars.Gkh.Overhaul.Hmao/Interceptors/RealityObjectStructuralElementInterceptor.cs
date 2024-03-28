namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.GkhCr.Entities;
    using Overhaul.Entities;

    public class RealityObjectStructuralElementInterceptor : EmptyDomainInterceptor<RealityObjectStructuralElement>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RealityObjectStructuralElement> service, RealityObjectStructuralElement entity)
        {
            //if (!string.IsNullOrEmpty(entity.StructuralElement.MutuallyExclusiveGroup))
            //{
            //    var mutuallyExclusiveElems =
            //        service.GetAll()
            //            .Where(x => x.RealityObject.Id == entity.RealityObject.Id)
            //            .Where(x => x.StructuralElement.Id != entity.StructuralElement.Id)
            //            .Where(x => x.StructuralElement.MutuallyExclusiveGroup == entity.StructuralElement.MutuallyExclusiveGroup)
            //            .Where(x => x.StructuralElement.Group.Id == entity.StructuralElement.Group.Id)
            //            .Where(x => x.State.StartState)
            //            .Select(x => x.StructuralElement.Name)
            //            .ToArray();

            //    var message = string.Empty;

            //    if (mutuallyExclusiveElems.Length > 0)
            //    {
            //        message = mutuallyExclusiveElems.Aggregate(
            //            message, (current, str) => current + string.Format(" {0}; ", str));
            //    }

            //    if (!string.IsNullOrEmpty(message))
            //    {
            //        message = string.Format("Выбраны взаимоисключающие конструктивные элементы: {0};{1}", entity.StructuralElement.Name, message);
            //        return Failure(message);
            //    }
            //}

            //// Перед сохранением проставляем начальный статус
            //var stateProvider = Container.Resolve<IStateProvider>();
            //stateProvider.SetDefaultState(entity);

            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<RealityObjectStructuralElement> service, RealityObjectStructuralElement entity)
        {
            //var programService = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>();
            //var programServiceStage2 = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage2>>();
            //var programServiceStage3 = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage3>>();
            //var versionRecordStageDomain = Container.Resolve<IDomainService<VersionRecordStage1>>();
            //var structElAttrService = Container.Resolve<IDomainService<RealityObjectStructuralElementAttributeValue>>();

            //if (versionRecordStageDomain.GetAll().Any(x => x.StructuralElement.Id == entity.Id) || programService.GetAll().Any(x => x.StructuralElement.Id == entity.Id))
            //{
            //    return Failure("Данный КЭ не может быть удален, т.к. присутствует в ДПКР");
            //}

            //if (structElAttrService.GetAll().Any(x => x.Object.Id == entity.Id))
            //{
            //    return Failure("Существует зависимые записи в атрибутах конструктивного элемента");
            //}

            //if (programService.GetAll().Any(x => x.StructuralElement == entity))
            //{
            //    var stage1ElementList = programService.GetAll()
            //        .Where(x => x.StructuralElement == entity)
            //        .Select(x => new { Stage2_Id = x.Stage2.Id, x.Id })
            //        .ToList();

            //    #region Удаляем КЭ и пересчитываем программу для дома заного

            //    foreach (var item in stage1ElementList)
            //    {
            //        // Удаляем КЭ из первого этапа
            //        programService.Delete(item.Id);
            //    }

            //    var stage2RefList = stage1ElementList.Select(y => y.Stage2_Id).ToList();
            //    programService.GetAll()
            //        .Where(x => stage2RefList.Contains(x.Stage2.Id))
            //        .ToList()
            //        .GroupBy(x => x.Stage2.Id)
            //        .ForEach(
            //            x =>
            //            {
            //                var stage2Element = programServiceStage2.Load(x.Key);
            //                stage2Element.Sum = x.Sum(y => y.Sum + y.ServiceCost);
            //                programServiceStage2.Update(stage2Element);

            //                programServiceStage2.GetAll()
            //                    .Where(y => y.Stage3.Id == stage2Element.Stage3.Id)
            //                    .ToList()
            //                    .GroupBy(y => y.Stage3.Id)
            //                    .ForEach(
            //                        y =>
            //                        {
            //                            var stage3Element = programServiceStage3.Load(y.Key);
            //                            stage3Element.Sum = y.Sum(i => i.Sum);
            //                            programServiceStage3.Update(stage3Element);
            //                        });
            //            });

            //    #endregion
            //}

            return Success();
        }
        
    }
}