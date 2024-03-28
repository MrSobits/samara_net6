namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Overhaul.Entities;

    public class RealityObjectStructuralElementInterceptor : EmptyDomainInterceptor<RealityObjectStructuralElement>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<RealityObjectStructuralElement> service, RealityObjectStructuralElement entity)
        {
            var programService = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>();
            var programServiceStage2 = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage2>>();
            var programServiceStage3 = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage3>>();
            var versionRecordStageDomain= Container.Resolve<IDomainService<VersionRecordStage1>>();

            if (versionRecordStageDomain.GetAll().Any(x => x.StructuralElement.Id == entity.Id) || programService.GetAll().Any(x => x.StructuralElement.Id == entity.Id))
            {
                return Failure("Данный КЭ не может быть удален, т.к. присутствует в ДПКР");
            }

            if (programService.GetAll().Any(x => x.StructuralElement == entity))
            {
                var stage1ElementList = programService.GetAll()
                    .Where(x => x.StructuralElement == entity)
                    .Select(x => new { Stage2_Id = x.Stage2.Id, x.Id })
                    .ToList();

                #region Удаляем КЭ и пересчитываем программу для дома заного

                foreach (var item in stage1ElementList)
                {
                    // Удаляем КЭ из первого этапа
                    programService.Delete(item.Id);
                }

                var stage2RefList = stage1ElementList.Select(y => y.Stage2_Id).ToList();
                programService.GetAll()
                    .Where(x => stage2RefList.Contains(x.Stage2.Id))
                    .ToList()
                    .GroupBy(x => x.Stage2.Id)
                    .ForEach(
                        x =>
                        {
                            var stage2Element = programServiceStage2.Load(x.Key);
                            stage2Element.Sum = x.Sum(y => y.Sum + y.ServiceCost);
                            programServiceStage2.Update(stage2Element);

                            programServiceStage2.GetAll()
                                .Where(y => y.Stage3.Id == stage2Element.Stage3.Id)
                                .ToList()
                                .GroupBy(y => y.Stage3.Id)
                                .ForEach(
                                    y =>
                                    {
                                        var stage3Element = programServiceStage3.Load(y.Key);
                                        stage3Element.Sum = y.Sum(i => i.Sum);
                                        programServiceStage3.Update(stage3Element);
                                    });
                        });

                #endregion
            }

            return Success();
        }
    }
}